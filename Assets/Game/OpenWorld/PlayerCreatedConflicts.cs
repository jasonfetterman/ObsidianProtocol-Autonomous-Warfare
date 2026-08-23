using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum PlayerConflictType
    {
        Territorial,
        Resource,
        StrategicObjective,
        Base,
        Retaliation
    }

    public enum PlayerConflictState
    {
        Proposed,
        Declared,
        Active,
        Ceasefire,
        Resolved,
        Cancelled
    }

    public sealed class PlayerCreatedConflictRecord
    {
        public string ConflictId { get; }

        public string InitiatorId { get; }

        public string TargetId { get; }

        public string RegionId { get; }

        public PlayerConflictType Type { get; }

        public PlayerConflictState State { get; private set; }

        public long CreatedTick { get; }

        public long ResolvedTick { get; private set; }

        public PlayerCreatedConflictRecord(
            string conflictId,
            string initiatorId,
            string targetId,
            string regionId,
            PlayerConflictType type,
            long createdTick)
        {
            ConflictId =
                conflictId ?? string.Empty;

            InitiatorId =
                initiatorId ?? string.Empty;

            TargetId =
                targetId ?? string.Empty;

            RegionId =
                regionId ?? string.Empty;

            Type = type;

            State =
                PlayerConflictState.Proposed;

            CreatedTick = createdTick;
            ResolvedTick = -1;
        }

        public bool Declare()
        {
            if (State !=
                PlayerConflictState.Proposed)
            {
                return false;
            }

            State =
                PlayerConflictState.Declared;

            return true;
        }

        public bool Activate()
        {
            if (State !=
                    PlayerConflictState.Declared &&
                State !=
                    PlayerConflictState.Ceasefire)
            {
                return false;
            }

            State =
                PlayerConflictState.Active;

            return true;
        }

        public bool BeginCeasefire()
        {
            if (State !=
                PlayerConflictState.Active)
            {
                return false;
            }

            State =
                PlayerConflictState.Ceasefire;

            return true;
        }

        public bool Resolve(
            long resolvedTick)
        {
            if (State ==
                    PlayerConflictState.Resolved ||
                State ==
                    PlayerConflictState.Cancelled ||
                resolvedTick < CreatedTick)
            {
                return false;
            }

            State =
                PlayerConflictState.Resolved;

            ResolvedTick =
                resolvedTick;

            return true;
        }

        public bool Cancel()
        {
            if (State ==
                    PlayerConflictState.Active ||
                State ==
                    PlayerConflictState.Resolved)
            {
                return false;
            }

            State =
                PlayerConflictState.Cancelled;

            return true;
        }
    }

    public sealed class PlayerCreatedConflicts
    {
        private readonly Dictionary<
            string,
            PlayerCreatedConflictRecord> conflicts =
            new Dictionary<
                string,
                PlayerCreatedConflictRecord>(
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

        public bool CreateConflict(
            string conflictId,
            string initiatorId,
            string targetId,
            string regionId,
            PlayerConflictType type,
            long createdTick)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(conflictId) ||
                string.IsNullOrWhiteSpace(initiatorId) ||
                string.IsNullOrWhiteSpace(targetId) ||
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
                new PlayerCreatedConflictRecord(
                    id,
                    initiatorId.Trim(),
                    targetId.Trim(),
                    regionId.Trim(),
                    type,
                    createdTick));

            return true;
        }

        public bool Declare(
            string conflictId)
        {
            PlayerCreatedConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.Declare();
        }

        public bool Activate(
            string conflictId)
        {
            PlayerCreatedConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.Activate();
        }

        public bool BeginCeasefire(
            string conflictId)
        {
            PlayerCreatedConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.BeginCeasefire();
        }

        public bool Resolve(
            string conflictId,
            long resolvedTick)
        {
            PlayerCreatedConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.Resolve(resolvedTick);
        }

        public bool Cancel(
            string conflictId)
        {
            PlayerCreatedConflictRecord record =
                GetConflict(conflictId);

            return record != null &&
                   record.Cancel();
        }

        public PlayerCreatedConflictRecord GetConflict(
            string conflictId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(conflictId))
            {
                return null;
            }

            conflicts.TryGetValue(
                conflictId.Trim(),
                out PlayerCreatedConflictRecord record);

            return record;
        }

        public IReadOnlyCollection<
            PlayerCreatedConflictRecord>
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
