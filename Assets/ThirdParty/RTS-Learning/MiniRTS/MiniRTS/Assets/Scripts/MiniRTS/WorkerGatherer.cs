using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Connects the pure gather cycle to UnitMover, ResourceNode, and faction economy.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(UnitMover))]
    public sealed class WorkerGatherer : MonoBehaviour
    {
        private GatherCycle cycle;
        private Unit unit;
        private UnitMover mover;
        private PlayerEconomy economy;
        private ResourceNode targetNode;
        private GameObject carryIndicator;
        private float nextMoveRetryTime;

        public GatherPhase Phase => cycle != null ? cycle.Phase : GatherPhase.Idle;
        public ResourceNode TargetNode => targetNode;
        public int CarriedAmount => cycle != null ? cycle.CarriedAmount : 0;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            mover = GetComponent<UnitMover>();
            cycle = new GatherCycle(
                BalanceConfig.HarvestSeconds,
                BalanceConfig.HarvestCarryCapacity);
            CreateCarryIndicator();
        }

        public void Initialize(PlayerEconomy factionEconomy)
        {
            economy = factionEconomy ??
                throw new System.ArgumentNullException(nameof(factionEconomy));
        }

        public bool BeginGather(ResourceNode resourceNode)
        {
            if (resourceNode == null || !resourceNode.IsHarvestable)
            {
                return false;
            }

            if (unit != null && unit.Combatant != null)
            {
                unit.Combatant.CancelForUtilityOrder();
            }

            targetNode = resourceNode;
            cycle.Begin(resourceNode.Type);
            CommandCurrentPhase();
            return true;
        }

        public void InterruptForMove()
        {
            cycle.Interrupt();
        }

        public void StopGathering()
        {
            targetNode = null;
            cycle.Stop();
            UpdateCarryIndicator();
        }

        private void Update()
        {
            if (economy == null || unit == null || mover == null)
            {
                return;
            }

            switch (cycle.Phase)
            {
                case GatherPhase.MovingToResource:
                    UpdateMovingToResource();
                    break;
                case GatherPhase.Harvesting:
                    UpdateHarvesting();
                    break;
                case GatherPhase.ReturningToDropoff:
                    UpdateReturningToDropoff();
                    break;
                case GatherPhase.Interrupted:
                    if (!mover.IsMoving)
                    {
                        cycle.Resume();
                        CommandCurrentPhase();
                    }
                    break;
            }
        }

        private void UpdateMovingToResource()
        {
            if (targetNode == null || !targetNode.IsHarvestable)
            {
                StopGathering();
                return;
            }

            if (targetNode.IsInInteractionRange(transform.position))
            {
                mover.Stop();
                cycle.ArriveAtResource();
                return;
            }

            RetryMoveIfStopped(targetNode.transform.position);
        }

        private void UpdateHarvesting()
        {
            if (targetNode == null || !targetNode.IsHarvestable)
            {
                StopGathering();
                return;
            }

            if (!targetNode.IsInInteractionRange(transform.position))
            {
                cycle.Interrupt();
                cycle.Resume();
                CommandCurrentPhase();
                return;
            }

            if (!cycle.AdvanceHarvest(Time.deltaTime))
            {
                return;
            }

            targetNode.TryHarvest(
                BalanceConfig.HarvestCarryCapacity,
                out int harvestedAmount);
            cycle.CompleteHarvest(harvestedAmount);
            UpdateCarryIndicator();

            if (harvestedAmount > 0)
            {
                CommandCurrentPhase();
            }
            else
            {
                StopGathering();
            }
        }

        private void UpdateReturningToDropoff()
        {
            Building dropoff = Building.FindNearestDropoff(
                unit.OwnerId,
                transform.position);
            if (dropoff == null)
            {
                return;
            }

            if (dropoff.DistanceToFootprint(transform.position) <=
                BalanceConfig.DropoffInteractionRange)
            {
                mover.Stop();
                GatherDelivery delivery = cycle.ArriveAtDropoff();
                economy.AddResources(delivery.Type, delivery.Amount);
                UpdateCarryIndicator();

                if (targetNode != null && targetNode.IsHarvestable)
                {
                    CommandCurrentPhase();
                }
                else
                {
                    StopGathering();
                }

                return;
            }

            RetryMoveIfStopped(dropoff.transform.position);
        }

        private void CommandCurrentPhase()
        {
            nextMoveRetryTime = 0f;
            if (cycle.Phase == GatherPhase.MovingToResource && targetNode != null)
            {
                mover.MoveTo(targetNode.transform.position);
            }
            else if (cycle.Phase == GatherPhase.ReturningToDropoff)
            {
                Building dropoff = Building.FindNearestDropoff(
                    unit.OwnerId,
                    transform.position);
                if (dropoff != null)
                {
                    mover.MoveTo(dropoff.transform.position);
                }
            }
        }

        private void RetryMoveIfStopped(Vector3 destination)
        {
            if (mover.IsMoving || Time.time < nextMoveRetryTime)
            {
                return;
            }

            mover.MoveTo(destination);
            nextMoveRetryTime = Time.time + 0.5f;
        }

        private void CreateCarryIndicator()
        {
            carryIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            carryIndicator.name = "CarryIndicator";
            carryIndicator.transform.SetParent(transform, false);
            carryIndicator.transform.localPosition = new Vector3(0f, 1.45f, 0f);
            carryIndicator.transform.localScale = Vector3.one * 0.34f;

            Collider indicatorCollider = carryIndicator.GetComponent<Collider>();
            if (indicatorCollider != null)
            {
                indicatorCollider.enabled = false;
                Destroy(indicatorCollider);
            }

            carryIndicator.SetActive(false);
        }

        private void UpdateCarryIndicator()
        {
            bool isCarrying = cycle.CarriedAmount > 0;
            carryIndicator.SetActive(isCarrying);
            if (!isCarrying)
            {
                return;
            }

            Renderer indicatorRenderer = carryIndicator.GetComponent<Renderer>();
            indicatorRenderer.material.color =
                cycle.CarriedResourceType == ResourceType.Minerals
                    ? BalanceConfig.MineralColor
                    : BalanceConfig.GasColor;
        }
    }
}
