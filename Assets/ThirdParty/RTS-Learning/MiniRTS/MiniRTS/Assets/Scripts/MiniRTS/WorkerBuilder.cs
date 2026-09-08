using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Moves a worker to a construction site and contributes construction time.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(UnitMover))]
    [RequireComponent(typeof(WorkerGatherer))]
    public sealed class WorkerBuilder : MonoBehaviour
    {
        private UnitMover mover;
        private WorkerGatherer gatherer;
        private Building target;
        private float nextMoveRetryTime;

        public Building Target => target;
        public bool IsBuilding => target != null && !target.IsConstructed;

        private void Awake()
        {
            mover = GetComponent<UnitMover>();
            gatherer = GetComponent<WorkerGatherer>();
        }

        public bool BeginConstruction(Building constructionSite)
        {
            if (constructionSite == null || constructionSite.IsConstructed)
            {
                return false;
            }

            Unit unit = GetComponent<Unit>();
            if (unit != null && unit.Combatant != null)
            {
                unit.Combatant.CancelForUtilityOrder();
            }

            target = constructionSite;
            gatherer.StopGathering();
            nextMoveRetryTime = 0f;
            CommandMove();
            return true;
        }

        public void CancelConstruction()
        {
            target = null;
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            if (target.IsConstructed)
            {
                target = null;
                return;
            }

            if (target.DistanceToFootprint(transform.position) <=
                BalanceConfig.BuildInteractionRange)
            {
                mover.Stop();
                if (target.AdvanceConstruction(Time.deltaTime))
                {
                    target = null;
                }

                return;
            }

            if (!mover.IsMoving && Time.time >= nextMoveRetryTime)
            {
                CommandMove();
            }
        }

        private void CommandMove()
        {
            if (target == null)
            {
                return;
            }

            mover.MoveTo(target.transform.position);
            nextMoveRetryTime = Time.time + BalanceConfig.BuildMoveRetrySeconds;
        }
    }
}
