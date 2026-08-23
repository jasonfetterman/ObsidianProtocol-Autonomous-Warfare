using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum BattlefieldChangeType
    {
        BuildingDamage,
        RoadDamage,
        BridgeDestruction,
        TerrainDeformation,
        FireDamage,
        EnvironmentalHazard,
        Debris
    }

    public sealed class BattlefieldChange
    {
        public string ChangeId { get; }

        public BattlefieldChangeType Type { get; }

        public string TargetId { get; }

        public float Severity { get; private set; }

        public bool Persistent { get; }

        public DateTime LastUpdatedUtc { get; private set; }

        public BattlefieldChange(
            string changeId,
            BattlefieldChangeType type,
            string targetId,
            float severity,
            bool persistent)
        {
            ChangeId =
                changeId ?? string.Empty;

            Type = type;

            TargetId =
                targetId ?? string.Empty;

            Severity =
                Math.Max(0f, severity);

            Persistent = persistent;

            LastUpdatedUtc =
                DateTime.UtcNow;
        }

        public bool UpdateSeverity(
            float severity)
        {
            if (severity < 0f)
            {
                return false;
            }

            Severity = severity;

            LastUpdatedUtc =
                DateTime.UtcNow;

            return true;
        }
    }

    public sealed class PersistentBattlefieldChanges
    {
        private readonly Dictionary<
            string,
            BattlefieldChange> changes =
            new Dictionary<
                string,
                BattlefieldChange>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ChangeCount =>
            changes.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            changes.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterChange(
            string changeId,
            BattlefieldChangeType type,
            string targetId,
            float severity,
            bool persistent)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(changeId) ||
                string.IsNullOrWhiteSpace(targetId) ||
                severity < 0f)
            {
                return false;
            }

            string id =
                changeId.Trim();

            if (changes.ContainsKey(id))
            {
                return false;
            }

            changes.Add(
                id,
                new BattlefieldChange(
                    id,
                    type,
                    targetId.Trim(),
                    severity,
                    persistent));

            return true;
        }

        public bool UpdateChange(
            string changeId,
            float severity)
        {
            BattlefieldChange change =
                GetChange(changeId);

            return change != null &&
                   change.UpdateSeverity(
                       severity);
        }

        public bool RemoveChange(
            string changeId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(changeId))
            {
                return false;
            }

            return changes.Remove(
                changeId.Trim());
        }

        public BattlefieldChange GetChange(
            string changeId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(changeId))
            {
                return null;
            }

            changes.TryGetValue(
                changeId.Trim(),
                out BattlefieldChange change);

            return change;
        }

        public IReadOnlyCollection<
            BattlefieldChange>
            GetChanges()
        {
            return changes.Values;
        }

        public IReadOnlyCollection<
            BattlefieldChange>
            GetPersistentChanges()
        {
            List<BattlefieldChange> result =
                new List<BattlefieldChange>();

            foreach (BattlefieldChange change
                     in changes.Values)
            {
                if (change.Persistent)
                {
                    result.Add(change);
                }
            }

            return result;
        }

        public void Reset()
        {
            changes.Clear();

            Initialized = false;
        }
    }
}
