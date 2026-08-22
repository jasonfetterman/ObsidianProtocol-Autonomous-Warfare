using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class FleetInventoryEntry
    {
        public string UnitId { get; }
        public string UnitType { get; }

        public int Quantity { get; private set; }

        public bool Available { get; private set; }

        public FleetInventoryEntry(
            string unitId,
            string unitType,
            int quantity)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitType =
                unitType ?? string.Empty;

            Quantity =
                Math.Max(0, quantity);

            Available = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(UnitId);

        public void Add(int amount)
        {
            if (amount <= 0)
                return;

            Quantity += amount;
            Available = Quantity > 0;
        }

        public bool Remove(int amount)
        {
            if (amount <= 0 ||
                amount > Quantity)
            {
                return false;
            }

            Quantity -= amount;
            Available = Quantity > 0;

            return true;
        }

        public void SetAvailable(bool available)
        {
            Available = available && Quantity > 0;
        }

        public void ResetAvailability()
        {
            Available = Quantity > 0;
        }
    }

    public sealed class FleetInventory
    {
        private readonly Dictionary<
            string,
            FleetInventoryEntry> entries =
            new Dictionary<
                string,
                FleetInventoryEntry>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            FleetInventoryEntry entry)
        {
            if (entry == null ||
                !entry.Valid ||
                entries.ContainsKey(entry.UnitId))
            {
                return false;
            }

            entries.Add(
                entry.UnitId,
                entry);

            return true;
        }

        public bool Remove(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return entries.Remove(unitId);
        }

        public bool TryGet(
            string unitId,
            out FleetInventoryEntry entry)
        {
            return entries.TryGetValue(
                unitId,
                out entry);
        }

        public bool AddUnits(
            string unitId,
            int amount)
        {
            if (!entries.TryGetValue(
                    unitId,
                    out FleetInventoryEntry entry))
            {
                return false;
            }

            entry.Add(amount);
            return true;
        }

        public bool RemoveUnits(
            string unitId,
            int amount)
        {
            if (!entries.TryGetValue(
                    unitId,
                    out FleetInventoryEntry entry))
            {
                return false;
            }

            return entry.Remove(amount);
        }

        public IReadOnlyCollection<
            FleetInventoryEntry>
            GetEntries()
        {
            return entries.Values;
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
