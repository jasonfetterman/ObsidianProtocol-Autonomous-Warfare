using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum TacticalConditionType
    {
        Visibility,
        Mobility,
        Fire,
        Smoke,
        Weather,
        Terrain,
        StructuralDamage,
        Hazard,
        Debris
    }

    public sealed class TacticalCondition
    {
        public string ConditionId { get; }

        public TacticalConditionType Type { get; }

        public float Severity { get; private set; }

        public bool Active { get; private set; }

        public DateTime LastUpdatedUtc { get; private set; }

        public TacticalCondition(
            string conditionId,
            TacticalConditionType type,
            float severity)
        {
            ConditionId =
                conditionId ?? string.Empty;

            Type = type;

            Severity =
                ClampSeverity(severity);

            Active =
                Severity > 0f;

            LastUpdatedUtc =
                DateTime.UtcNow;
        }

        public bool SetSeverity(
            float severity)
        {
            Severity =
                ClampSeverity(severity);

            Active =
                Severity > 0f;

            LastUpdatedUtc =
                DateTime.UtcNow;

            return true;
        }

        public bool Activate()
        {
            if (Severity <= 0f)
            {
                return false;
            }

            Active = true;

            LastUpdatedUtc =
                DateTime.UtcNow;

            return true;
        }

        public bool Deactivate()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;

            LastUpdatedUtc =
                DateTime.UtcNow;

            return true;
        }

        private static float ClampSeverity(
            float severity)
        {
            return Math.Max(
                0f,
                Math.Min(1f, severity));
        }
    }

    public sealed class DynamicTacticalConditions
    {
        private readonly Dictionary<
            string,
            TacticalCondition> conditions =
            new Dictionary<
                string,
                TacticalCondition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ConditionCount =>
            conditions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            conditions.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterCondition(
            string conditionId,
            TacticalConditionType type,
            float severity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(conditionId) ||
                severity < 0f)
            {
                return false;
            }

            string id =
                conditionId.Trim();

            if (conditions.ContainsKey(id))
            {
                return false;
            }

            conditions.Add(
                id,
                new TacticalCondition(
                    id,
                    type,
                    severity));

            return true;
        }

        public bool SetSeverity(
            string conditionId,
            float severity)
        {
            TacticalCondition condition =
                GetCondition(conditionId);

            return condition != null &&
                   condition.SetSeverity(severity);
        }

        public bool Activate(
            string conditionId)
        {
            TacticalCondition condition =
                GetCondition(conditionId);

            return condition != null &&
                   condition.Activate();
        }

        public bool Deactivate(
            string conditionId)
        {
            TacticalCondition condition =
                GetCondition(conditionId);

            return condition != null &&
                   condition.Deactivate();
        }

        public TacticalCondition GetCondition(
            string conditionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(conditionId))
            {
                return null;
            }

            conditions.TryGetValue(
                conditionId.Trim(),
                out TacticalCondition condition);

            return condition;
        }

        public IReadOnlyCollection<
            TacticalCondition>
            GetConditions()
        {
            return conditions.Values;
        }

        public void Reset()
        {
            conditions.Clear();

            Initialized = false;
        }
    }
}
