using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum ForwardBaseState
    {
        Planned,
        Constructing,
        Operational,
        Damaged,
        Abandoned,
        Destroyed
    }

    public sealed class ForwardBaseRecord
    {
        public string BaseId { get; }

        public string OwnerId { get; }

        public string RegionId { get; }

        public ForwardBaseState State { get; private set; }

        public float Integrity { get; private set; }

        public float ConstructionProgress { get; private set; }

        public ForwardBaseRecord(
            string baseId,
            string ownerId,
            string regionId)
        {
            BaseId =
                baseId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            RegionId =
                regionId ?? string.Empty;

            State =
                ForwardBaseState.Planned;

            Integrity = 100f;
            ConstructionProgress = 0f;
        }

        public bool SetConstructionProgress(
            float progress)
        {
            if (progress < 0f ||
                progress > 100f)
            {
                return false;
            }

            ConstructionProgress = progress;

            if (progress >= 100f &&
                State == ForwardBaseState.Constructing)
            {
                State = ForwardBaseState.Operational;
            }

            return true;
        }

        public bool SetState(
            ForwardBaseState state)
        {
            if (string.IsNullOrWhiteSpace(BaseId))
            {
                return false;
            }

            State = state;

            if (state ==
                ForwardBaseState.Destroyed)
            {
                Integrity = 0f;
            }

            return true;
        }

        public bool SetIntegrity(
            float integrity)
        {
            if (integrity < 0f ||
                integrity > 100f)
            {
                return false;
            }

            Integrity = integrity;

            if (integrity <= 0f)
            {
                State = ForwardBaseState.Destroyed;
            }
            else if (integrity < 100f &&
                     State ==
                         ForwardBaseState.Operational)
            {
                State = ForwardBaseState.Damaged;
            }

            return true;
        }
    }

    public sealed class ForwardBases
    {
        private readonly Dictionary<
            string,
            ForwardBaseRecord> bases =
            new Dictionary<
                string,
                ForwardBaseRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BaseCount =>
            bases.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            bases.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterBase(
            string baseId,
            string ownerId,
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(baseId) ||
                string.IsNullOrWhiteSpace(ownerId) ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            string id =
                baseId.Trim();

            if (bases.ContainsKey(id))
            {
                return false;
            }

            bases.Add(
                id,
                new ForwardBaseRecord(
                    id,
                    ownerId.Trim(),
                    regionId.Trim()));

            return true;
        }

        public bool BeginConstruction(
            string baseId)
        {
            ForwardBaseRecord record =
                GetBase(baseId);

            if (record == null)
            {
                return false;
            }

            return record.SetState(
                ForwardBaseState.Constructing);
        }

        public bool SetConstructionProgress(
            string baseId,
            float progress)
        {
            ForwardBaseRecord record =
                GetBase(baseId);

            return record != null &&
                   record.SetConstructionProgress(
                       progress);
        }

        public bool SetBaseState(
            string baseId,
            ForwardBaseState state)
        {
            ForwardBaseRecord record =
                GetBase(baseId);

            return record != null &&
                   record.SetState(state);
        }

        public bool SetBaseIntegrity(
            string baseId,
            float integrity)
        {
            ForwardBaseRecord record =
                GetBase(baseId);

            return record != null &&
                   record.SetIntegrity(integrity);
        }

        public ForwardBaseRecord GetBase(
            string baseId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(baseId))
            {
                return null;
            }

            bases.TryGetValue(
                baseId.Trim(),
                out ForwardBaseRecord record);

            return record;
        }

        public IReadOnlyCollection<
            ForwardBaseRecord>
            GetBases()
        {
            return bases.Values;
        }

        public void Reset()
        {
            bases.Clear();
            Initialized = false;
        }
    }
}
