using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum CommandUnitType
    {
        Archive,
        Worldmap,
        CommandCore,
        Fusion,
        Nexus,
        Insight,
        Pulse,
        VectorCore
    }

    public enum CommandUnitState
    {
        Offline,
        Standby,
        Operational,
        Damaged,
        Disabled,
        Destroyed
    }

    public enum CommandCapability
    {
        FleetControl,
        DataStorage,
        WorldMapping,
        Intelligence,
        DataFusion,
        NetworkCoordination,
        Analytics,
        CommandNetwork
    }

    public sealed class CommandUnit
    {
        private readonly HashSet<CommandCapability> capabilities =
            new HashSet<CommandCapability>();

        public string UnitId { get; }
        public string UnitName { get; }

        public CommandUnitType Type { get; }

        public CommandUnitState State { get; private set; }

        public float Integrity { get; private set; }
        public float OperationalRange { get; private set; }

        public bool Autonomous { get; private set; }

        public CommandUnit(
            string unitId,
            string unitName,
            CommandUnitType type)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitName =
                unitName ?? string.Empty;

            Type =
                type;

            State =
                CommandUnitState.Standby;

            Integrity = 1f;
        }

        public void Configure(
            float operationalRange,
            bool autonomous)
        {
            OperationalRange =
                Math.Max(
                    0f,
                    operationalRange);

            Autonomous =
                autonomous;
        }

        public void AddCapability(
            CommandCapability capability)
        {
            capabilities.Add(capability);
        }

        public bool HasCapability(
            CommandCapability capability)
        {
            return capabilities.Contains(capability);
        }

        public IReadOnlyCollection<CommandCapability> GetCapabilities()
        {
            return capabilities;
        }

        public void SetOperational()
        {
            if (State !=
                CommandUnitState.Destroyed)
            {
                State =
                    CommandUnitState.Operational;
            }
        }

        public void SetStandby()
        {
            if (State !=
                CommandUnitState.Destroyed)
            {
                State =
                    CommandUnitState.Standby;
            }
        }

        public void ApplyDamage(
            float amount)
        {
            if (State ==
                CommandUnitState.Destroyed)
            {
                return;
            }

            Integrity =
                Math.Max(
                    0f,
                    Integrity - Math.Max(
                        0f,
                        amount));

            if (Integrity <= 0f)
            {
                State =
                    CommandUnitState.Destroyed;
            }
            else if (Integrity < 0.5f)
            {
                State =
                    CommandUnitState.Damaged;
            }
        }

        public void Repair(
            float amount)
        {
            if (State ==
                CommandUnitState.Destroyed)
            {
                return;
            }

            Integrity =
                Math.Min(
                    1f,
                    Integrity + Math.Max(
                        0f,
                        amount));

            if (Integrity >= 1f)
            {
                State =
                    CommandUnitState.Operational;
            }
        }

        public void Disable()
        {
            if (State !=
                CommandUnitState.Destroyed)
            {
                State =
                    CommandUnitState.Disabled;
            }
        }
    }

    public sealed class CommandUnitFramework
    {
        private readonly Dictionary<string, CommandUnit> units =
            new Dictionary<string, CommandUnit>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            string unitName,
            CommandUnitType type)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            units[unitId] =
                new CommandUnit(
                    unitId,
                    unitName,
                    type);
        }

        public void ConfigureUnit(
            string unitId,
            float operationalRange,
            bool autonomous)
        {
            if (units.TryGetValue(
                    unitId,
                    out CommandUnit unit))
            {
                unit.Configure(
                    operationalRange,
                    autonomous);
            }
        }

        public void AddCapability(
            string unitId,
            CommandCapability capability)
        {
            if (units.TryGetValue(
                    unitId,
                    out CommandUnit unit))
            {
                unit.AddCapability(capability);
            }
        }

        public void SetOperational(
            string unitId)
        {
            if (units.TryGetValue(
                    unitId,
                    out CommandUnit unit))
            {
                unit.SetOperational();
            }
        }

        public void ApplyDamage(
            string unitId,
            float amount)
        {
            if (units.TryGetValue(
                    unitId,
                    out CommandUnit unit))
            {
                unit.ApplyDamage(amount);
            }
        }

        public bool TryGetUnit(
            string unitId,
            out CommandUnit unit)
        {
            return units.TryGetValue(
                unitId,
                out unit);
        }

        public void RemoveUnit(
            string unitId)
        {
            units.Remove(unitId);
        }

        public void Clear()
        {
            units.Clear();
        }
    }
}
