using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum WorldEventType
    {
        Conflict,
        ResourceSurge,
        ResourceShortage,
        BaseAttack,
        TerritoryChange,
        Environmental,
        Discovery
    }

    public enum WorldEventState
    {
        Scheduled,
        Active,
        Resolved,
        Cancelled
    }

    public sealed class WorldEventRecord
    {
        public string EventId { get; }

        public WorldEventType Type { get; }

        public string RegionId { get; }

        public WorldEventState State { get; private set; }

        public long StartTick { get; }

        public long EndTick { get; private set; }

        public WorldEventRecord(
            string eventId,
            WorldEventType type,
            string regionId,
            long startTick)
        {
            EventId =
                eventId ?? string.Empty;

            Type = type;

            RegionId =
                regionId ?? string.Empty;

            State =
                WorldEventState.Scheduled;

            StartTick = startTick;
            EndTick = -1;
        }

        public bool Activate()
        {
            if (State !=
                WorldEventState.Scheduled)
            {
                return false;
            }

            State =
                WorldEventState.Active;

            return true;
        }

        public bool Resolve(
            long endTick)
        {
            if (State !=
                WorldEventState.Active ||
                endTick < StartTick)
            {
                return false;
            }

            EndTick = endTick;

            State =
                WorldEventState.Resolved;

            return true;
        }

        public bool Cancel()
        {
            if (State ==
                WorldEventState.Resolved)
            {
                return false;
            }

            State =
                WorldEventState.Cancelled;

            return true;
        }
    }

    public sealed class WorldEvents
    {
        private readonly Dictionary<
            string,
            WorldEventRecord> events =
            new Dictionary<
                string,
                WorldEventRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int EventCount =>
            events.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            events.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterEvent(
            string eventId,
            WorldEventType type,
            string regionId,
            long startTick)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(eventId) ||
                startTick < 0)
            {
                return false;
            }

            string id =
                eventId.Trim();

            if (events.ContainsKey(id))
            {
                return false;
            }

            events.Add(
                id,
                new WorldEventRecord(
                    id,
                    type,
                    regionId,
                    startTick));

            return true;
        }

        public bool Activate(
            string eventId)
        {
            WorldEventRecord record =
                GetEvent(eventId);

            return record != null &&
                   record.Activate();
        }

        public bool Resolve(
            string eventId,
            long endTick)
        {
            WorldEventRecord record =
                GetEvent(eventId);

            return record != null &&
                   record.Resolve(endTick);
        }

        public bool Cancel(
            string eventId)
        {
            WorldEventRecord record =
                GetEvent(eventId);

            return record != null &&
                   record.Cancel();
        }

        public WorldEventRecord GetEvent(
            string eventId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(eventId))
            {
                return null;
            }

            events.TryGetValue(
                eventId.Trim(),
                out WorldEventRecord record);

            return record;
        }

        public IReadOnlyCollection<
            WorldEventRecord>
            GetEvents()
        {
            return events.Values;
        }

        public void Reset()
        {
            events.Clear();
            Initialized = false;
        }
    }
}
