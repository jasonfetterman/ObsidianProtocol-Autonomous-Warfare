using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum ExperimentalUnitType
    {
        Echo,
        Nullpoint,
        Specter,
        Shadowgrid,
        Phantom,
        Helix
    }

    public enum ExperimentalUnitState
    {
        Contained,
        Standby,
        Operational,
        Unstable,
        Disabled,
        Destroyed
    }

    public enum ExperimentalCapability
    {
        SignalIntelligence,
        ElectronicDisruption,
        Stealth,
        CovertNetworking,
        ExperimentalAI,
        ExperimentalAbilities
    }

    public sealed class ExperimentalUnit
    {
        private readonly HashSet<ExperimentalCapability> capabilities =
            new HashSet<ExperimentalCapability>();

        public string UnitId { get; }
        public string UnitName { get; }

        public ExperimentalUnitType Type { get; }

        public ExperimentalUnitState State { get; private set; }

        public float Stability { get; private set; }
        public float Integrity { get; private set; }

        public bool Authorized { get; private set; }
        public bool Autonomous { get; private set; }

        public ExperimentalUnit(
            string unitId,
            string unitName,
            ExperimentalUnitType type)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitName =
                unitName ?? string.Empty;

            Type =
                type;

            State =
                ExperimentalUnitState.Contained;

            Stability = 1f;
            Integrity = 1f;

            Authorized = false;
            Autonomous = false;
        }

        public void Configure(
            bool authorized,
            bool autonomous)
        {
            Authorized =
                authorized;

            Autonomous =
                autonomous;
        }

        public void AddCapability(
            ExperimentalCapability capability)
        {
            capabilities.Add(
                capability);
        }

        public bool HasCapability(
            ExperimentalCapability capability)
        {
            return capabilities.Contains(
                capability);
        }

        public IReadOnlyCollection<ExperimentalCapability>
            GetCapabilities()
        {
            return capabilities;
        }

        public void SetOperational()
        {
            if (!Authorized ||
                State ==
                ExperimentalUnitState.Destroyed)
            {
                return;
            }

            State =
                ExperimentalUnitState.Operational;
        }

        public void SetStandby()
        {
            if (State !=
                ExperimentalUnitState.Destroyed)
            {
                State =
                    ExperimentalUnitState.Standby;
            }
        }

        public void SetContained()
        {
            if (State !=
                ExperimentalUnitState.Destroyed)
            {
                State =
                    ExperimentalUnitState.Contained;
            }
        }

        public void ApplyInstability(
            float amount)
        {
            if (State ==
                ExperimentalUnitState.Destroyed)
            {
                return;
            }

            Stability =
                Math.Max(
                    0f,
                    Stability - Math.Max(
                        0f,
                        amount));

            if (Stability <= 0f)
            {
                State =
                    ExperimentalUnitState.Unstable;
            }
        }

        public void Stabilize(
            float amount)
        {
            if (State ==
                ExperimentalUnitState.Destroyed)
            {
                return;
            }

            Stability =
                Math.Min(
                    1f,
                    Stability + Math.Max(
                        0f,
                        amount));

            if (Stability >= 1f &&
                State ==
                ExperimentalUnitState.Unstable)
            {
                State =
                    ExperimentalUnitState.Standby;
            }
        }

        public void ApplyDamage(
            float amount)
        {
            if (State ==
                ExperimentalUnitState.Destroyed)
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
                    ExperimentalUnitState.Destroyed;
            }
            else if (Integrity < 0.5f)
            {
                State =
                    ExperimentalUnitState.Unstable;
            }
        }

        public void SetAuthorization(
            bool authorized)
        {
            Authorized =
                authorized;

            if (!authorized &&
                State ==
                ExperimentalUnitState.Operational)
            {
                State =
                    ExperimentalUnitState.Contained;
            }
        }
    }

    public sealed class ExperimentalFramework
    {
        private readonly Dictionary<string, ExperimentalUnit> units =
            new Dictionary<string, ExperimentalUnit>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            string unitName,
            ExperimentalUnitType type)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            units[unitId] =
                new ExperimentalUnit(
                    unitId,
                    unitName,
                    type);
        }

        public void ConfigureUnit(
            string unitId,
            bool authorized,
            bool autonomous)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.Configure(
                    authorized,
                    autonomous);
            }
        }

        public void AddCapability(
            string unitId,
            ExperimentalCapability capability)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.AddCapability(
                    capability);
            }
        }

        public void SetAuthorization(
            string unitId,
            bool authorized)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.SetAuthorization(
                    authorized);
            }
        }

        public void SetOperational(
            string unitId)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.SetOperational();
            }
        }

        public void SetStandby(
            string unitId)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.SetStandby();
            }
        }

        public void SetContained(
            string unitId)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.SetContained();
            }
        }

        public void ApplyInstability(
            string unitId,
            float amount)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.ApplyInstability(
                    amount);
            }
        }

        public void Stabilize(
            string unitId,
            float amount)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.Stabilize(
                    amount);
            }
        }

        public void ApplyDamage(
            string unitId,
            float amount)
        {
            if (units.TryGetValue(
                    unitId,
                    out ExperimentalUnit unit))
            {
                unit.ApplyDamage(
                    amount);
            }
        }

        public bool TryGetUnit(
            string unitId,
            out ExperimentalUnit unit)
        {
            return units.TryGetValue(
                unitId,
                out unit);
        }

        public IReadOnlyCollection<ExperimentalUnit>
            GetUnits()
        {
            return units.Values;
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
