using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum DynamicConflictType
    {
        Territory,
        Resource,
        Base,
        StrategicObjective,
        Convoy,
        Regional
    }

    public enum DynamicConflictState
    {
        Detected,
        Escalating,
        Active,
        Resolving,
        Resolved
    }

    public sealed class DynamicConflictRecord
    {
        public string ConflictId { get; }

        public DynamicConflictType Type { get; }

        public string RegionId { get; }

        public string InitiatorId { get; }

        public string DefenderId { get; }

        public DynamicConflictState State { get; private set; }

        public float Intensity { get; private set; }

        public long CreatedTick { get; }

        public long ResolvedTick { get; private set; }

        public DynamicConflictRecord(
            string conflictId,
            DynamicConflictType type,
            string regionId,
            string initiatorId,
            string defenderId,
            long createdTick)
        {
            ConflictId =
                conflictId ?? string.Empty;

            Type = type;

            RegionId =
                regionId ?? string.Empty;

            InitiatorId =
                initiatorId ?? string.Empty;

            DefenderId =
                defenderId ?? string.Empty;

            State =
                DynamicConflictState.Detected;

            Intensity = 0f;

            CreatedTick = createdTick;

            ResolvedTick = -1;
        }

        public bool SetIntensity(
            float intensity)
        {
            if (intensity < 0f ||
                intensity > 100f)
            {
                return false;
            }

            Intensity = intensity;

            if (State ==
                DynamicConflictState.Detected &&
                intensity > 0f)
            {
                State =
                    DynamicConflictState.Escalating;
            }

            return true;
        }

        public bool Activate()
        {
            if (State !=
                    DynamicConflictState.Escalating &&
                State !=
                    DynamicConflictState.Detected)
            {
                return false;
            }

            State =
                DynamicConflictState.Active;

            return true;
        }

        public bool BeginResolution()
        {
            if (State !=
                DynamicConflictState.Active)
            {
                return false;
            }

            State =
                DynamicConflictState.Resolving;

            return true;
        }

        public bool Resolve(
            long resolvedTick)
        {
            if (State !=
                DynamicConflictState.Resolving ||
                resolvedTick < CreatedTick)
            {
                return false;
            }

            ResolvedTick = resolvedTick;
            Intensity = 0f;

            State =
                DynamicConflictState.Resolved;

            return true;
        }
    }

    public sealed class DynamicConflicts
    {
        private readonly Dictionary<
            string,
            DynamicConflictRecord> conflicts =
            new Dictionary<
                string,
                DynamicConflictRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ConflictCount =>
            conflicts.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            conflicts.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterConflict(
            string conflictId,
            DynamicConflictType type,
            string regionId,
            string initiatorId,
            string defenderId,
            long createdTick)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(conflictId) ||
                string.IsNullOrWhiteSpace(regionId) ||
                createdTick < 0)
            {
                return false;
            }

            string id =
                conflictId.Trim();

            if (conflicts.ContainsKey(id))
            {
                return false;
            }

            conflicts.Add(
                id,
                new DynamicConflictRecord(
                    id,
                    type,
                    regionId,
                    initiatorId,
                    defenderId,
                    createdTick));

            return true;
        }

        public bool SetIntensity(
            string conflictId,
            float intensity)
        {
            DynamicConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.SetIntensity(intensity);
        }

        public bool Activate(
            string conflictId)
        {
            DynamicConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.Activate();
        }

        public bool BeginResolution(
            string conflictId)
        {
            DynamicConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.BeginResolution();
        }

        public bool Resolve(
            string conflictId,
            long resolvedTick)
        {
            DynamicConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.Resolve(resolvedTick);
        }

        public DynamicConflictRecord GetConflict(
            string conflictId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(conflictId))
            {
                return null;
            }

            conflicts.TryGetValue(
                conflictId.Trim(),
                out DynamicConflictRecord record);

            return record;
        }

        public IReadOnlyCollection<
            DynamicConflictRecord>
            GetConflicts()
        {
            return conflicts.Values;
        }

        public void Reset()
        {
            conflicts.Clear();
            Initialized = false;
        }
    }
}
