using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum FleetUnitStatus
    {
        Stored,
        Operational,
        Repairing,
        Upgrading,
        Fabricating,
        Salvaging,
        Deployed,
        Disabled
    }

    public sealed class FleetUnit
    {
        public string OwnershipId { get; }
        public string UnitId { get; }

        public FleetUnitStatus Status { get; private set; }

        public string GarageSlotId { get; private set; }

        public FleetUnit(
            string ownershipId,
            string unitId)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            Status =
                FleetUnitStatus.Stored;

            GarageSlotId =
                string.Empty;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                OwnershipId) &&
            !string.IsNullOrWhiteSpace(
                UnitId);

        public void SetStatus(
            FleetUnitStatus status)
        {
            Status = status;
        }

        public void AssignSlot(
            string slotId)
        {
            GarageSlotId =
                slotId ?? string.Empty;
        }

        public void ClearSlot()
        {
            GarageSlotId = string.Empty;
        }
    }

    public sealed class FleetManagement
    {
        private readonly Dictionary<
            string,
            FleetUnit> fleet =
            new Dictionary<
                string,
                FleetUnit>(
                StringComparer.OrdinalIgnoreCase);

        public int FleetSize =>
            fleet.Count;

        public bool Register(
            FleetUnit unit)
        {
            if (unit == null ||
                !unit.Valid ||
                fleet.ContainsKey(
                    unit.OwnershipId))
            {
                return false;
            }

            fleet.Add(
                unit.OwnershipId,
                unit);

            return true;
        }

        public bool Remove(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            return fleet.Remove(
                ownershipId);
        }

        public bool TryGet(
            string ownershipId,
            out FleetUnit unit)
        {
            return fleet.TryGetValue(
                ownershipId,
                out unit);
        }

        public bool SetStatus(
            string ownershipId,
            FleetUnitStatus status)
        {
            if (!fleet.TryGetValue(
                    ownershipId,
                    out FleetUnit unit))
            {
                return false;
            }

            unit.SetStatus(status);
            return true;
        }

        public bool AssignSlot(
            string ownershipId,
            string slotId)
        {
            if (!fleet.TryGetValue(
                    ownershipId,
                    out FleetUnit unit))
            {
                return false;
            }

            unit.AssignSlot(slotId);
            return true;
        }

        public bool ClearSlot(
            string ownershipId)
        {
            if (!fleet.TryGetValue(
                    ownershipId,
                    out FleetUnit unit))
            {
                return false;
            }

            unit.ClearSlot();
            return true;
        }

        public int CountByStatus(
            FleetUnitStatus status)
        {
            int count = 0;

            foreach (FleetUnit unit in fleet.Values)
            {
                if (unit.Status == status)
                    count++;
            }

            return count;
        }

        public IReadOnlyCollection<
            FleetUnit>
            GetFleet()
        {
            return fleet.Values;
        }

        public void Clear()
        {
            fleet.Clear();
        }
    }
}
