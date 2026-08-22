using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum AerialCombatState
    {
        Standby,
        Engaging,
        Pursuing,
        Disengaging,
        Returning
    }

    public sealed class AerialCombatAssignment
    {
        public string UnitId { get; }

        public string TargetId { get; private set; }

        public AerialCombatState State { get; private set; }

        public float EngagementRange { get; private set; }
        public float AttackCooldown { get; private set; }

        public bool WeaponsEnabled { get; private set; }

        public AerialCombatAssignment(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            TargetId = string.Empty;
            State = AerialCombatState.Standby;

            WeaponsEnabled = false;
        }

        public void Configure(
            float engagementRange,
            float attackCooldown)
        {
            EngagementRange =
                Math.Max(0f, engagementRange);

            AttackCooldown =
                Math.Max(0f, attackCooldown);
        }

        public void SetTarget(
            string targetId)
        {
            TargetId =
                targetId ?? string.Empty;

            State =
                string.IsNullOrWhiteSpace(TargetId)
                    ? AerialCombatState.Standby
                    : AerialCombatState.Engaging;
        }

        public void SetWeaponsEnabled(
            bool enabled)
        {
            WeaponsEnabled = enabled;
        }

        public void Pursue()
        {
            if (!string.IsNullOrWhiteSpace(TargetId))
            {
                State = AerialCombatState.Pursuing;
            }
        }

        public void Disengage()
        {
            State = AerialCombatState.Disengaging;
        }

        public void ReturnToBase()
        {
            State = AerialCombatState.Returning;
            TargetId = string.Empty;
        }

        public void Reset()
        {
            TargetId = string.Empty;
            State = AerialCombatState.Standby;
            WeaponsEnabled = false;
        }
    }

    public sealed class AerialCombatSystem
    {
        private readonly Dictionary<string, AerialCombatAssignment> assignments =
            new Dictionary<string, AerialCombatAssignment>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!assignments.ContainsKey(unitId))
            {
                assignments.Add(
                    unitId,
                    new AerialCombatAssignment(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float engagementRange,
            float attackCooldown)
        {
            RegisterUnit(unitId);

            assignments[unitId].Configure(
                engagementRange,
                attackCooldown);
        }

        public void SetTarget(
            string unitId,
            string targetId)
        {
            RegisterUnit(unitId);

            assignments[unitId].SetTarget(targetId);
        }

        public void SetWeaponsEnabled(
            string unitId,
            bool enabled)
        {
            RegisterUnit(unitId);

            assignments[unitId].SetWeaponsEnabled(enabled);
        }

        public void Pursue(string unitId)
        {
            if (assignments.TryGetValue(
                    unitId,
                    out AerialCombatAssignment assignment))
            {
                assignment.Pursue();
            }
        }

        public void Disengage(string unitId)
        {
            if (assignments.TryGetValue(
                    unitId,
                    out AerialCombatAssignment assignment))
            {
                assignment.Disengage();
            }
        }

        public void ReturnToBase(string unitId)
        {
            if (assignments.TryGetValue(
                    unitId,
                    out AerialCombatAssignment assignment))
            {
                assignment.ReturnToBase();
            }
        }

        public bool TryGetAssignment(
            string unitId,
            out AerialCombatAssignment assignment)
        {
            return assignments.TryGetValue(
                unitId,
                out assignment);
        }

        public void RemoveUnit(string unitId)
        {
            assignments.Remove(unitId);
        }

        public void Clear()
        {
            assignments.Clear();
        }
    }
}
