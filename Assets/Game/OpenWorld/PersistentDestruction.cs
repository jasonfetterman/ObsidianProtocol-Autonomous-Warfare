using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum DestructionState
    {
        Intact,
        Damaged,
        Disabled,
        Destroyed,
        Ruined,
        Recovered
    }

    public sealed class PersistentDestructionRecord
    {
        public string ObjectId { get; }

        public string RegionId { get; }

        public DestructionState State { get; private set; }

        public float Integrity { get; private set; }

        public long LastUpdateTick { get; private set; }

        public PersistentDestructionRecord(
            string objectId,
            string regionId)
        {
            ObjectId =
                objectId ?? string.Empty;

            RegionId =
                regionId ?? string.Empty;

            State =
                DestructionState.Intact;

            Integrity = 100f;
            LastUpdateTick = 0;
        }

        public bool SetIntegrity(
            float integrity,
            long updateTick)
        {
            if (integrity < 0f ||
                integrity > 100f ||
                updateTick < LastUpdateTick)
            {
                return false;
            }

            Integrity = integrity;
            LastUpdateTick = updateTick;

            if (Integrity <= 0f)
            {
                State =
                    DestructionState.Destroyed;
            }
            else if (Integrity <= 25f)
            {
                State =
                    DestructionState.Disabled;
            }
            else if (Integrity < 100f)
            {
                State =
                    DestructionState.Damaged;
            }
            else
            {
                State =
                    DestructionState.Intact;
            }

            return true;
        }

        public bool SetState(
            DestructionState state,
            long updateTick)
        {
            if (updateTick < LastUpdateTick)
            {
                return false;
            }

            if (State ==
                    DestructionState.Destroyed &&
                state !=
                    DestructionState.Ruined &&
                state !=
                    DestructionState.Recovered)
            {
                return false;
            }

            State = state;
            LastUpdateTick = updateTick;

            if (state ==
                DestructionState.Destroyed)
            {
                Integrity = 0f;
            }
            else if (state ==
                DestructionState.Recovered)
            {
                Integrity = 100f;
            }

            return true;
        }
    }

    public sealed class PersistentDestruction
    {
        private readonly Dictionary<
            string,
            PersistentDestructionRecord> objects =
            new Dictionary<
                string,
                PersistentDestructionRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ObjectCount =>
            objects.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            objects.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterObject(
            string objectId,
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId) ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            string id =
                objectId.Trim();

            if (objects.ContainsKey(id))
            {
                return false;
            }

            objects.Add(
                id,
                new PersistentDestructionRecord(
                    id,
                    regionId.Trim()));

            return true;
        }

        public bool SetIntegrity(
            string objectId,
            float integrity,
            long updateTick)
        {
            PersistentDestructionRecord record =
                GetObject(objectId);

            return record != null &&
                   record.SetIntegrity(
                       integrity,
                       updateTick);
        }

        public bool SetState(
            string objectId,
            DestructionState state,
            long updateTick)
        {
            PersistentDestructionRecord record =
                GetObject(objectId);

            return record != null &&
                   record.SetState(
                       state,
                       updateTick);
        }

        public PersistentDestructionRecord GetObject(
            string objectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(objectId))
            {
                return null;
            }

            objects.TryGetValue(
                objectId.Trim(),
                out PersistentDestructionRecord record);

            return record;
        }

        public IReadOnlyCollection<
            PersistentDestructionRecord>
            GetObjects()
        {
            return objects.Values;
        }

        public void Reset()
        {
            objects.Clear();
            Initialized = false;
        }
    }
}
