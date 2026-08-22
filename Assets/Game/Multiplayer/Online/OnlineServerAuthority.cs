using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineAuthorityType
    {
        None,
        Server,
        Player
    }

    public sealed class OnlineAuthorityRecord
    {
        public string EntityId { get; }

        public string OwnerPlayerId { get; private set; }

        public OnlineAuthorityType Authority { get; private set; }

        public long LastValidatedTick { get; private set; }

        public OnlineAuthorityRecord(
            string entityId)
        {
            EntityId =
                entityId ?? string.Empty;

            Authority =
                OnlineAuthorityType.None;
        }

        public bool SetAuthority(
            string ownerPlayerId,
            OnlineAuthorityType authority,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(EntityId) ||
                tick < 0)
            {
                return false;
            }

            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;

            Authority = authority;
            LastValidatedTick = tick;

            return true;
        }
    }

    public sealed class OnlineServerAuthority
    {
        private readonly Dictionary<
            string,
            OnlineAuthorityRecord> authorities =
            new Dictionary<
                string,
                OnlineAuthorityRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AuthorityCount =>
            authorities.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            authorities.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterEntity(
            string entityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(entityId))
            {
                return false;
            }

            string id =
                entityId.Trim();

            if (authorities.ContainsKey(id))
            {
                return false;
            }

            authorities.Add(
                id,
                new OnlineAuthorityRecord(id));

            return true;
        }

        public bool AssignServerAuthority(
            string entityId,
            long tick)
        {
            OnlineAuthorityRecord record =
                GetAuthority(entityId);

            return record != null &&
                   record.SetAuthority(
                       string.Empty,
                       OnlineAuthorityType.Server,
                       tick);
        }

        public bool AssignPlayerAuthority(
            string entityId,
            string playerId,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            OnlineAuthorityRecord record =
                GetAuthority(entityId);

            return record != null &&
                   record.SetAuthority(
                       playerId.Trim(),
                       OnlineAuthorityType.Player,
                       tick);
        }

        public bool ValidateServerAuthority(
            string entityId,
            long currentTick)
        {
            OnlineAuthorityRecord record =
                GetAuthority(entityId);

            if (record == null ||
                currentTick < 0)
            {
                return false;
            }

            return record.Authority ==
                       OnlineAuthorityType.Server &&
                   record.LastValidatedTick <= currentTick;
        }

        public bool ValidatePlayerAuthority(
            string entityId,
            string playerId,
            long currentTick)
        {
            if (string.IsNullOrWhiteSpace(playerId) ||
                currentTick < 0)
            {
                return false;
            }

            OnlineAuthorityRecord record =
                GetAuthority(entityId);

            if (record == null)
            {
                return false;
            }

            return record.Authority ==
                       OnlineAuthorityType.Player &&
                   string.Equals(
                       record.OwnerPlayerId,
                       playerId.Trim(),
                       StringComparison.OrdinalIgnoreCase) &&
                   record.LastValidatedTick <= currentTick;
        }

        public OnlineAuthorityRecord GetAuthority(
            string entityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(entityId))
            {
                return null;
            }

            authorities.TryGetValue(
                entityId.Trim(),
                out OnlineAuthorityRecord record);

            return record;
        }

        public bool RemoveEntity(
            string entityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(entityId))
            {
                return false;
            }

            return authorities.Remove(
                entityId.Trim());
        }

        public void Reset()
        {
            authorities.Clear();
            Initialized = false;
        }
    }
}
