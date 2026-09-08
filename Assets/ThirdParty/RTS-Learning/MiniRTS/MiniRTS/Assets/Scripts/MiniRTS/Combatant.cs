using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    public enum CombatOrderType
    {
        Idle,
        Move,
        AttackMove,
        AttackTarget,
        Hold
    }

    /// <summary>
    /// Unit combat order state, enemy acquisition, chasing, facing, and instant-hit attacks.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(UnitMover))]
    public sealed class Combatant : MonoBehaviour
    {
        private readonly List<ICombatTarget> potentialTargets =
            new List<ICombatTarget>();
        private readonly List<CombatTargetCandidate> targetCandidates =
            new List<CombatTargetCandidate>();

        private Unit unit;
        private UnitMover mover;
        private CombatStats stats;
        private ICombatTarget currentTarget;
        private Vector3 orderDestination;
        private float nextAttackTime;
        private float nextAcquireTime;
        private float nextChaseRepathTime;
        private bool initialized;

        public CombatOrderType CurrentOrder { get; private set; } =
            CombatOrderType.Idle;
        public ICombatTarget CurrentTarget => currentTarget;
        public CombatStats Stats => stats;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            mover = GetComponent<UnitMover>();
        }

        public void Initialize()
        {
            unit = GetComponent<Unit>();
            mover = GetComponent<UnitMover>();
            stats = UnitDefinition.Get(unit.Type).Combat;
            unit.SetCombatant(this);
            initialized = true;
        }

        public bool IssueMove(Vector3 destination)
        {
            if (!CanReceiveOrders())
            {
                return false;
            }

            ClearTarget();
            orderDestination = destination;
            CurrentOrder = CombatOrderType.Move;
            if (mover.MoveTo(destination))
            {
                return true;
            }

            CurrentOrder = CombatOrderType.Idle;
            return false;
        }

        public bool IssueAttackMove(Vector3 destination)
        {
            if (!CanReceiveOrders())
            {
                return false;
            }

            ClearTarget();
            orderDestination = destination;
            CurrentOrder = CombatOrderType.AttackMove;
            nextAcquireTime = 0f;
            return mover.MoveTo(destination);
        }

        public bool IssueAttackTarget(ICombatTarget target)
        {
            if (!CanReceiveOrders() || !IsValidEnemy(target))
            {
                return false;
            }

            currentTarget = target;
            CurrentOrder = CombatOrderType.AttackTarget;
            nextChaseRepathTime = 0f;
            return true;
        }

        public void Stop()
        {
            if (!initialized)
            {
                return;
            }

            ClearTarget();
            mover.Stop();
            CurrentOrder = CombatOrderType.Hold;
        }

        public void CancelForUtilityOrder()
        {
            ClearTarget();
            CurrentOrder = CombatOrderType.Idle;
        }

        private void Update()
        {
            if (!CanReceiveOrders())
            {
                return;
            }

            if (CurrentOrder == CombatOrderType.Move)
            {
                if (!mover.IsMoving)
                {
                    CurrentOrder = CombatOrderType.Idle;
                }

                return;
            }

            if (CurrentOrder == CombatOrderType.Hold)
            {
                return;
            }

            if (IsUtilityBusy())
            {
                ClearTarget();
                return;
            }

            if (!IsValidEnemy(currentTarget))
            {
                HandleLostTarget();
            }

            if (currentTarget == null)
            {
                if (CurrentOrder == CombatOrderType.Idle && mover.IsMoving)
                {
                    return;
                }

                TryAcquireTarget();
            }

            if (currentTarget != null)
            {
                UpdateAttack();
                return;
            }

            UpdateAttackMoveTravel();
        }

        private void UpdateAttack()
        {
            float distance = currentTarget.DistanceTo(transform.position);
            if (distance > stats.Range)
            {
                if (Time.time >= nextChaseRepathTime)
                {
                    mover.MoveTo(currentTarget.CombatTargetPosition);
                    nextChaseRepathTime =
                        Time.time + BalanceConfig.CombatChaseRepathInterval;
                }

                return;
            }

            mover.Stop();
            if (!FaceTarget(currentTarget.CombatTargetPosition))
            {
                return;
            }

            if (!CombatMath.IsCooldownReady(Time.time, nextAttackTime))
            {
                return;
            }

            Vector3 attackOrigin = unit.CombatTargetPosition;
            Vector3 attackEnd = currentTarget.CombatTargetPosition;
            currentTarget.TakeDamage(stats.Damage);
            Vector3 unitScale = transform.lossyScale;
            float weaponScale = Mathf.Max(
                unitScale.x,
                Mathf.Max(unitScale.y, unitScale.z));
            CombatEffects.PlayTracer(
                attackOrigin,
                attackEnd,
                unit.FactionColor,
                weaponScale);
            nextAttackTime = CombatMath.ScheduleNextAttack(
                Time.time,
                stats.CooldownSeconds);
        }

        private bool FaceTarget(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude <= 0.0001f)
            {
                return true;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                UnitDefinition.Get(unit.Type).TurnSpeed *
                Mathf.Min(Time.deltaTime, 0.1f));
            return Quaternion.Angle(transform.rotation, targetRotation) <=
                   BalanceConfig.AttackFacingTolerance;
        }

        private void TryAcquireTarget()
        {
            if (CurrentOrder != CombatOrderType.Idle &&
                CurrentOrder != CombatOrderType.AttackMove)
            {
                return;
            }

            if (Time.time < nextAcquireTime)
            {
                return;
            }

            nextAcquireTime = Time.time + BalanceConfig.CombatAcquireInterval;
            potentialTargets.Clear();
            targetCandidates.Clear();

            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                AddCandidate(units[i], 0);
            }

            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                AddCandidate(buildings[i], 1);
            }

            int targetIndex = CombatTargeting.FindNearestEnemyIndex(
                targetCandidates,
                unit.OwnerId,
                stats.AggroRadius);
            if (targetIndex < 0)
            {
                return;
            }

            currentTarget = potentialTargets[targetIndex];
            nextChaseRepathTime = 0f;
        }

        private void AddCandidate(ICombatTarget candidate, int tieBreakPriority)
        {
            Component candidateComponent = candidate as Component;
            if (candidateComponent == null)
            {
                return;
            }

            float distance = candidate.DistanceTo(transform.position);
            potentialTargets.Add(candidate);
            targetCandidates.Add(new CombatTargetCandidate(
                candidate.OwnerId,
                distance * distance,
                candidate.IsAlive,
                tieBreakPriority,
                FogOfWar.CanOwnerSeeTarget(unit.OwnerId, candidate)));
        }

        private void HandleLostTarget()
        {
            ClearTarget();
            if (CurrentOrder == CombatOrderType.AttackTarget)
            {
                CurrentOrder = CombatOrderType.Idle;
            }
        }

        private void UpdateAttackMoveTravel()
        {
            if (CurrentOrder != CombatOrderType.AttackMove || mover.IsMoving)
            {
                return;
            }

            Vector3 difference = orderDestination - transform.position;
            difference.y = 0f;
            if (difference.sqrMagnitude <=
                BalanceConfig.FormationSpacing * BalanceConfig.FormationSpacing)
            {
                CurrentOrder = CombatOrderType.Idle;
                return;
            }

            mover.MoveTo(orderDestination);
        }

        private bool IsUtilityBusy()
        {
            WorkerGatherer gatherer = GetComponent<WorkerGatherer>();
            if (gatherer != null && gatherer.Phase != GatherPhase.Idle)
            {
                return true;
            }

            WorkerBuilder builder = GetComponent<WorkerBuilder>();
            return builder != null && builder.IsBuilding;
        }

        private bool IsValidEnemy(ICombatTarget target)
        {
            if (target == null ||
                target.OwnerId == unit.OwnerId ||
                !target.IsAlive ||
                !FogOfWar.CanOwnerSeeTarget(unit.OwnerId, target))
            {
                return false;
            }

            Component targetComponent = target as Component;
            return targetComponent != null;
        }

        private bool CanReceiveOrders()
        {
            return initialized &&
                   unit != null &&
                   unit.IsAlive &&
                   mover != null;
        }

        private void ClearTarget()
        {
            currentTarget = null;
        }
    }
}
