using System;

namespace MiniRTS
{
    public enum ResourceType
    {
        Minerals,
        Gas
    }

    public enum GatherPhase
    {
        Idle,
        MovingToResource,
        Harvesting,
        ReturningToDropoff,
        Interrupted
    }

    public readonly struct GatherDelivery
    {
        public readonly ResourceType Type;
        public readonly int Amount;

        public GatherDelivery(ResourceType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }

    /// <summary>
    /// Unity-free state for a worker's repeating resource trip.
    /// </summary>
    public sealed class GatherCycle
    {
        private readonly float harvestDuration;
        private readonly int carryCapacity;
        private float harvestElapsed;

        public GatherPhase Phase { get; private set; }
        public ResourceType TargetResourceType { get; private set; }
        public ResourceType CarriedResourceType { get; private set; }
        public int CarriedAmount { get; private set; }
        public float HarvestProgress =>
            harvestDuration <= 0f ? 1f : Math.Min(1f, harvestElapsed / harvestDuration);

        public GatherCycle(float harvestDuration, int carryCapacity)
        {
            if (harvestDuration <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(harvestDuration));
            }

            if (carryCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(carryCapacity));
            }

            this.harvestDuration = harvestDuration;
            this.carryCapacity = carryCapacity;
            Phase = GatherPhase.Idle;
        }

        public void Begin(ResourceType resourceType)
        {
            TargetResourceType = resourceType;
            harvestElapsed = 0f;
            Phase = CarriedAmount > 0
                ? GatherPhase.ReturningToDropoff
                : GatherPhase.MovingToResource;
        }

        public bool ArriveAtResource()
        {
            if (Phase != GatherPhase.MovingToResource)
            {
                return false;
            }

            harvestElapsed = 0f;
            Phase = GatherPhase.Harvesting;
            return true;
        }

        public bool AdvanceHarvest(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds));
            }

            if (Phase != GatherPhase.Harvesting)
            {
                return false;
            }

            harvestElapsed = Math.Min(harvestDuration, harvestElapsed + deltaSeconds);
            return harvestElapsed >= harvestDuration;
        }

        public int CompleteHarvest(int availableAmount)
        {
            if (availableAmount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(availableAmount));
            }

            if (Phase != GatherPhase.Harvesting)
            {
                return 0;
            }

            CarriedAmount = Math.Min(carryCapacity, availableAmount);
            CarriedResourceType = TargetResourceType;
            harvestElapsed = 0f;
            Phase = CarriedAmount > 0
                ? GatherPhase.ReturningToDropoff
                : GatherPhase.Idle;
            return CarriedAmount;
        }

        public GatherDelivery ArriveAtDropoff()
        {
            if (Phase != GatherPhase.ReturningToDropoff)
            {
                return default;
            }

            GatherDelivery delivery =
                new GatherDelivery(CarriedResourceType, CarriedAmount);
            CarriedAmount = 0;
            Phase = GatherPhase.MovingToResource;
            return delivery;
        }

        public bool Interrupt()
        {
            if (Phase == GatherPhase.Idle || Phase == GatherPhase.Interrupted)
            {
                return false;
            }

            harvestElapsed = 0f;
            Phase = GatherPhase.Interrupted;
            return true;
        }

        public bool Resume()
        {
            if (Phase != GatherPhase.Interrupted)
            {
                return false;
            }

            Phase = CarriedAmount > 0
                ? GatherPhase.ReturningToDropoff
                : GatherPhase.MovingToResource;
            return true;
        }

        public void Stop()
        {
            harvestElapsed = 0f;
            CarriedAmount = 0;
            Phase = GatherPhase.Idle;
        }
    }
}
