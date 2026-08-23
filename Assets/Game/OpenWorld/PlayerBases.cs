using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum PlayerBaseState
    {
        Inactive,
        Operational,
        Damaged,
        UnderAttack,
        Destroyed
    }

    public sealed class PlayerBaseRecord
    {
        public string BaseId { get; }

        public string PlayerId { get; }

        public string RegionId { get; }

        public PlayerBaseState State { get; private set; }

        public float Integrity { get; private set; }

        public PlayerBaseRecord(
            string baseId,
            string playerId,
            string regionId)
        {
            BaseId = baseId ?? string.Empty;
            PlayerId = playerId ?? string.Empty;
            RegionId = regionId ?? string.Empty;

            State = PlayerBaseState.Inactive;
            Integrity = 100f;
        }

        public bool SetState(
            PlayerBaseState state)
        {
            if (string.IsNullOrWhiteSpace(BaseId))
            {
                return false;
            }

            State = state;

            if (state == PlayerBaseState.Destroyed)
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
                State = PlayerBaseState.Destroyed;
            }
            else if (integrity < 100f &&
                     State == PlayerBaseState.Operational)
            {
                State = PlayerBaseState.Damaged;
            }

            return true;
        }
    }

    public sealed class PlayerBases
    {
        private readonly Dictionary<
            string,
            PlayerBaseRecord> bases =
            new Dictionary<
                string,
                PlayerBaseRecord>(
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
            string playerId,
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(baseId) ||
                string.IsNullOrWhiteSpace(playerId) ||
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
                new PlayerBaseRecord(
                    id,
                    playerId.Trim(),
                    regionId.Trim()));

            return true;
        }

        public bool SetBaseState(
            string baseId,
            PlayerBaseState state)
        {
            PlayerBaseRecord record =
                GetBase(baseId);

            return record != null &&
                   record.SetState(state);
        }

        public bool SetBaseIntegrity(
            string baseId,
            float integrity)
        {
            PlayerBaseRecord record =
                GetBase(baseId);

            return record != null &&
                   record.SetIntegrity(integrity);
        }

        public PlayerBaseRecord GetBase(
            string baseId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(baseId))
            {
                return null;
            }

            bases.TryGetValue(
                baseId.Trim(),
                out PlayerBaseRecord record);

            return record;
        }

        public IReadOnlyCollection<PlayerBaseRecord>
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
