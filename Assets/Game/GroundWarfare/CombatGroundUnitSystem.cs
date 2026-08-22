using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundCombatRole
    {
        Scout,
        InfantrySupport,
        DirectFire,
        AntiArmor,
        AntiAir,
        Artillery,
        Assault,
        HeavyCombat
    }

    public enum GroundCombatState
    {
        Idle,
        Moving,
        Engaging,
        Suppressing,
        Pursuing,
        Retreating,
        Disabled
    }

    public sealed class CombatGroundUnit
    {
        public string UnitId { get; }
        public GroundCombatRole Role { get; private set; }

        public GroundCombatState State { get; private set; }

        public float CombatRange { get; private set; }
        public float ThreatPriority { get; private set; }

        public string CurrentTargetId { get; private set; }

        public bool CombatEnabled { get; private set; }

        public CombatGroundUnit(
            string unitId,
            GroundCombatRole role)
        {
            UnitId =
                unitId ?? string.Empty;

            Role =
                role;

            State =
                GroundCombatState.Idle;

            CurrentTargetId =
                string.Empty;

            CombatEnabled = true;
        }

        public void Configure(
            float combatRange,
            float threatPriority)
        {
            CombatRange =
                Math.Max(
                    0f,
                    combatRange);

            ThreatPriority =
                Math.Max(
                    0f,
                    threatPriority);
        }

        public void SetTarget(
            string targetId)
        {
            CurrentTargetId =
                targetId ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(
                    CurrentTargetId) &&
                CombatEnabled)
            {
                State =
                    GroundCombatState.Engaging;
            }
        }

        public void ClearTarget()
        {
            CurrentTargetId =
                string.Empty;

            if (State ==
                GroundCombatState.Engaging ||
                State ==
                GroundCombatState.Suppressing)
            {
                State =
                    GroundCombatState.Idle;
            }
        }

        public void SetState(
            GroundCombatState state)
        {
            State = state;
        }

        public void SetCombatEnabled(
            bool enabled)
        {
            CombatEnabled = enabled;

            if (!enabled)
            {
                CurrentTargetId =
                    string.Empty;

                State =
                    GroundCombatState.Disabled;
            }
            else if (State ==
                     GroundCombatState.Disabled)
            {
                State =
                    GroundCombatState.Idle;
            }
        }
    }

    public sealed class CombatGroundUnitSystem
    {
        private readonly Dictionary<string, CombatGroundUnit> units =
            new Dictionary<string, CombatGroundUnit>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            GroundCombatRole role)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!units.ContainsKey(unitId))
            {
                units.Add(
                    unitId,
                    new CombatGroundUnit(
                        unitId,
                        role));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float combatRange,
            float threatPriority)
        {
            if (units.TryGetValue(
                    unitId,
                    out CombatGroundUnit unit))
            {
                unit.Configure(
                    combatRange,
                    threatPriority);
            }
        }

        public void SetTarget(
            string unitId,
            string targetId)
        {
            if (units.TryGetValue(
                    unitId,
                    out CombatGroundUnit unit))
            {
                unit.SetTarget(targetId);
            }
        }

        public void ClearTarget(
            string unitId)
        {
            if (units.TryGetValue(
                    unitId,
                    out CombatGroundUnit unit))
            {
                unit.ClearTarget();
            }
        }

        public void SetState(
            string unitId,
            GroundCombatState state)
        {
            if (units.TryGetValue(
                    unitId,
                    out CombatGroundUnit unit))
            {
                unit.SetState(state);
            }
        }

        public void SetCombatEnabled(
            string unitId,
            bool enabled)
        {
            if (units.TryGetValue(
                    unitId,
                    out CombatGroundUnit unit))
            {
                unit.SetCombatEnabled(enabled);
            }
        }

        public bool TryGetUnit(
            string unitId,
            out CombatGroundUnit unit)
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
