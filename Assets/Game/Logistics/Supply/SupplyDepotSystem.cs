using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Logistics
{
    public sealed class SupplyDepot
    {
        private readonly Dictionary<SupplyType, float> inventory =
            new Dictionary<SupplyType, float>();

        public string DepotId { get; }

        public string LocationId { get; }

        public float Capacity { get; }

        public float StoredAmount { get; private set; }

        public float AvailableCapacity =>
            Math.Max(
                0f,
                Capacity - StoredAmount);

        public bool Valid =>
            !string.IsNullOrWhiteSpace(DepotId) &&
            !string.IsNullOrWhiteSpace(LocationId) &&
            Capacity > 0f;

        public SupplyDepot(
            string depotId,
            string locationId,
            float capacity)
        {
            DepotId =
                depotId ?? string.Empty;

            LocationId =
                locationId ?? string.Empty;

            Capacity =
                Math.Max(
                    0f,
                    capacity);

            StoredAmount = 0f;
        }

        public float GetAmount(
            SupplyType supplyType)
        {
            if (inventory.TryGetValue(
                    supplyType,
                    out float amount))
            {
                return amount;
            }

            return 0f;
        }

        public bool Store(
            SupplyType supplyType,
            float amount)
        {
            if (amount <= 0f ||
                amount > AvailableCapacity)
            {
                return false;
            }

            float current =
                GetAmount(supplyType);

            inventory[supplyType] =
                current + amount;

            StoredAmount += amount;

            return true;
        }

        public bool Withdraw(
            SupplyType supplyType,
            float amount)
        {
            if (amount <= 0f ||
                GetAmount(supplyType) < amount)
            {
                return false;
            }

            float remaining =
                GetAmount(supplyType) - amount;

            if (remaining <= 0f)
            {
                inventory.Remove(supplyType);
            }
            else
            {
                inventory[supplyType] =
                    remaining;
            }

            StoredAmount -= amount;

            if (StoredAmount < 0f)
            {
                StoredAmount = 0f;
            }

            return true;
        }

        public bool HasSupply(
            SupplyType supplyType,
            float amount)
        {
            return
                amount > 0f &&
                GetAmount(supplyType) >= amount;
        }

        public void ClearInventory()
        {
            inventory.Clear();
            StoredAmount = 0f;
        }
    }

    public sealed class SupplyDepotSystem
    {
        private readonly Dictionary<string, SupplyDepot> depots =
            new Dictionary<string, SupplyDepot>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterDepot(
            SupplyDepot depot)
        {
            if (depot == null ||
                !depot.Valid ||
                depots.ContainsKey(depot.DepotId))
            {
                return false;
            }

            depots.Add(
                depot.DepotId,
                depot);

            return true;
        }

        public bool RemoveDepot(
            string depotId)
        {
            if (string.IsNullOrWhiteSpace(depotId))
            {
                return false;
            }

            return depots.Remove(depotId);
        }

        public bool TryGetDepot(
            string depotId,
            out SupplyDepot depot)
        {
            return depots.TryGetValue(
                depotId,
                out depot);
        }

        public bool StoreSupply(
            string depotId,
            SupplyType supplyType,
            float amount)
        {
            if (!depots.TryGetValue(
                    depotId,
                    out SupplyDepot depot))
            {
                return false;
            }

            return depot.Store(
                supplyType,
                amount);
        }

        public bool WithdrawSupply(
            string depotId,
            SupplyType supplyType,
            float amount)
        {
            if (!depots.TryGetValue(
                    depotId,
                    out SupplyDepot depot))
            {
                return false;
            }

            return depot.Withdraw(
                supplyType,
                amount);
        }

        public IReadOnlyCollection<SupplyDepot>
            GetDepots()
        {
            return depots.Values;
        }

        public void Clear()
        {
            depots.Clear();
        }
    }
}
