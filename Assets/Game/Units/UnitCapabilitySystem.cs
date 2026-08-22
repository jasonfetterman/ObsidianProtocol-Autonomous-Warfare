using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public enum UnitCapability
    {
        Move,
        Attack,
        Defend,
        Recon,
        Surveillance,
        SensorFusion,
        Relay,
        Transport,
        Repair,
        Recovery,
        Rescue,
        Capture,
        Escort,
        Patrol,
        Flank,
        Breach,
        Suppress,
        Pursue,
        Retreat,
        Reinforce,
        Command,
        Communication,
        ElectronicWarfare,
        Deploy,
        OperateAtSea,
        OperateInAir,
        OperateOnGround
    }

    public sealed class UnitCapabilitySet
    {
        public string UnitId { get; }

        private readonly HashSet<UnitCapability> capabilities =
            new HashSet<UnitCapability>();

        public UnitCapabilitySet(string unitId)
        {
            UnitId = unitId ?? string.Empty;
        }

        public void Add(UnitCapability capability)
        {
            capabilities.Add(capability);
        }

        public void Remove(UnitCapability capability)
        {
            capabilities.Remove(capability);
        }

        public bool Has(UnitCapability capability)
        {
            return capabilities.Contains(capability);
        }

        public void Set(
            UnitCapability capability,
            bool enabled)
        {
            if (enabled)
            {
                Add(capability);
            }
            else
            {
                Remove(capability);
            }
        }

        public void Clear()
        {
            capabilities.Clear();
        }

        public IReadOnlyCollection<UnitCapability> GetAll()
        {
            return capabilities;
        }
    }

    public sealed class UnitCapabilitySystem
    {
        private readonly Dictionary<string, UnitCapabilitySet> unitCapabilities =
            new Dictionary<string, UnitCapabilitySet>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!unitCapabilities.ContainsKey(unitId))
            {
                unitCapabilities.Add(
                    unitId,
                    new UnitCapabilitySet(unitId));
            }
        }

        public void SetCapability(
            string unitId,
            UnitCapability capability,
            bool enabled)
        {
            RegisterUnit(unitId);

            unitCapabilities[unitId].Set(
                capability,
                enabled);
        }

        public bool HasCapability(
            string unitId,
            UnitCapability capability)
        {
            return unitCapabilities.TryGetValue(
                       unitId,
                       out UnitCapabilitySet set) &&
                   set.Has(capability);
        }

        public bool TryGetCapabilities(
            string unitId,
            out UnitCapabilitySet set)
        {
            return unitCapabilities.TryGetValue(
                unitId,
                out set);
        }

        public void RemoveUnit(string unitId)
        {
            unitCapabilities.Remove(unitId);
        }

        public void Clear()
        {
            unitCapabilities.Clear();
        }
    }
}
