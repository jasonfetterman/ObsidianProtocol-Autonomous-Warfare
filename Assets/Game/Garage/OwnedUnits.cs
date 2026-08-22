using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum OwnedUnitState
    {
        Stored,
        Configuring,
        Maintenance,
        Staged,
        Deployed,
        Disabled,
        Salvaged
    }

    public sealed class OwnedUnit
    {
        public string OwnershipId { get; }
        public string UnitId { get; }
        public string DisplayName { get; }

        public OwnedUnitState State { get; private set; }

        public bool Available =>
            State == OwnedUnitState.Stored ||
            State == OwnedUnitState.Configuring;

        public OwnedUnit(
            string ownershipId,
            string unitId,
            string displayName)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            DisplayName =
                displayName ?? string.Empty;

            State = OwnedUnitState.Stored;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                OwnershipId) &&
            !string.IsNullOrWhiteSpace(
                UnitId);

        public void Configure()
        {
            if (State == OwnedUnitState.Stored)
                State = OwnedUnitState.Configuring;
        }

        public void Store()
        {
            if (State != OwnedUnitState.Deployed &&
                State != OwnedUnitState.Salvaged)
            {
                State = OwnedUnitState.Stored;
            }
        }

        public void EnterMaintenance()
        {
            if (State != OwnedUnitState.Deployed &&
                State != OwnedUnitState.Salvaged)
            {
                State = OwnedUnitState.Maintenance;
            }
        }

        public void Stage()
        {
            if (State == OwnedUnitState.Stored ||
                State == OwnedUnitState.Configuring)
            {
                State = OwnedUnitState.Staged;
            }
        }

        public void Deploy()
        {
            if (State == OwnedUnitState.Staged)
                State = OwnedUnitState.Deployed;
        }

        public void Disable()
        {
            if (State == OwnedUnitState.Deployed)
                State = OwnedUnitState.Disabled;
        }

        public void Salvage()
        {
            if (State != OwnedUnitState.Deployed)
                State = OwnedUnitState.Salvaged;
        }

        public void Reset()
        {
            State = OwnedUnitState.Stored;
        }
    }

    public sealed class OwnedUnitRegistry
    {
        private readonly Dictionary<
            string,
            OwnedUnit> units =
            new Dictionary<
                string,
                OwnedUnit>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            OwnedUnit unit)
        {
            if (unit == null ||
                !unit.Valid ||
                units.ContainsKey(
                    unit.OwnershipId))
            {
                return false;
            }

            units.Add(
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

            return units.Remove(
                ownershipId);
        }

        public bool TryGet(
            string ownershipId,
            out OwnedUnit unit)
        {
            return units.TryGetValue(
                ownershipId,
                out unit);
        }

        public IReadOnlyCollection<OwnedUnit>
            GetUnits()
        {
            return units.Values;
        }

        public void Clear()
        {
            units.Clear();
        }
    }
}
