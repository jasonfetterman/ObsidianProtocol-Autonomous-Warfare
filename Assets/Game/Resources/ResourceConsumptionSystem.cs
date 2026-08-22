using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public sealed class ResourceCost
    {
        public string ResourceId { get; }
        public int Amount { get; }

        public ResourceCost(
            string resourceId,
            int amount)
        {
            ResourceId =
                resourceId ?? string.Empty;

            Amount =
                Math.Max(0, amount);
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ResourceId) &&
            Amount > 0;
    }

    public sealed class ResourceConsumptionSystem
    {
        public bool CanConsume(
            ResourceInventory inventory,
            IReadOnlyCollection<ResourceCost> costs)
        {
            if (inventory == null ||
                costs == null ||
                costs.Count == 0)
            {
                return false;
            }

            foreach (
                ResourceCost cost
                in costs)
            {
                if (cost == null ||
                    !cost.Valid ||
                    inventory.GetAmount(
                        cost.ResourceId) <
                    cost.Amount)
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryConsume(
            ResourceInventory inventory,
            IReadOnlyCollection<ResourceCost> costs)
        {
            if (!CanConsume(
                    inventory,
                    costs))
            {
                return false;
            }

            foreach (
                ResourceCost cost
                in costs)
            {
                if (!inventory.TrySpend(
                        cost.ResourceId,
                        cost.Amount))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
