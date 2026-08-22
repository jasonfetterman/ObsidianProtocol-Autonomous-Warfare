using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum NavalCombatRole
    {
        Patrol,
        Recon,
        Escort,
        DirectFire,
        AntiSurface,
        AntiSubmersible,
        Support,
        HeavyCombat
    }

    public enum NavalCombatState
    {
        Idle,
        Tracking,
        Engaging,
        Pursuing,
        Disengaging,
        Disabled
    }

    public sealed class NavalCombatUnit
    {
        public string UnitId { get; }
        public NavalCombatRole Role { get; }

        public NavalCombatState State { get; private set; }

        public string CurrentTargetId { get; private set; }

        public float CombatRange { get; private set; }
        public float WeaponEffectiveness { get; private set; }

        public bool CombatEnabled { get; private set; }

        public NavalCombatUnit(
            string unitId,
            NavalCombatRole role)
        {
            UnitId =
                unitId ?? string.Empty;

            Role =
                role;

            State =
                NavalCombatState.Idle;

            CurrentTargetId =
                string.Empty;

            CombatEnabled = true;
        }

        public void Configure(
            float combatRange,
            float weaponEffectiveness)
        {
            CombatRange =
                Math.Max(
                    0f,
                    combatRange);

            WeaponEffectiveness =
                Math.Max(
                    0f,
                    weaponEffectiveness);
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
                    NavalCombatState.Tracking;
            }
        }

        public void Engage()
        {
            if (CombatEnabled &&
                !string.IsNullOrWhiteSpace(
                    CurrentTargetId))
            {
                State =
                    NavalCombatState.Engaging;
            }
        }

        public void ClearTarget()
        {
            CurrentTargetId =
                string.Empty;

            if (State ==
                NavalCombatState.Tracking ||
                State ==
                NavalCombatState.Engaging)
            {
                State =
                    NavalCombatState.Idle;
            }
        }

        public void SetState(
            NavalCombatState state)
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
                    NavalCombatState.Disabled;
            }
            else if (State ==
                     NavalCombatState.Disabled)
            {
                State =
                    NavalCombatState.Idle;
            }
        }
    }

    public sealed class NavalCombatSystem
    {
        private readonly Dictionary<string, NavalCombatUnit> units =
            new Dictionary<string, NavalCombatUnit>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            NavalCombatRole role)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!units.ContainsKey(unitId))
            {
                units.Add(
                    unitId,
                    new NavalCombatUnit(
                        unitId,
                        role));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float combatRange,
            float weaponEffectiveness)
        {
            if (units.TryGetValue(
                    unitId,
                    out NavalCombatUnit unit))
            {
                unit.Configure(
                    combatRange,
                    weaponEffectiveness);
            }
        }

        public void SetTarget(
            string unitId,
            string targetId)
        {
            if (units.TryGetValue(
                    unitId,
                    out NavalCombatUnit unit))
            {
                unit.SetTarget(targetId);
            }
        }

        public void Engage(
            string unitId)
        {
            if (units.TryGetValue(
                    unitId,
                    out NavalCombatUnit unit))
            {
                unit.Engage();
            }
        }

        public void ClearTarget(
            string unitId)
        {
            if (units.TryGetValue(
                    unitId,
                    out NavalCombatUnit unit))
            {
                unit.ClearTarget();
            }
        }

        public void SetState(
            string unitId,
            NavalCombatState state)
        {
            if (units.TryGetValue(
                    unitId,
                    out NavalCombatUnit unit))
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
                    out NavalCombatUnit unit))
            {
                unit.SetCombatEnabled(enabled);
            }
        }

        public bool TryGetUnit(
            string unitId,
            out NavalCombatUnit unit)
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
