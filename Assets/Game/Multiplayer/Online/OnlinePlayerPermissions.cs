using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    [Flags]
    public enum OnlinePermission
    {
        None = 0,
        Command = 1 << 0,
        Deploy = 1 << 1,
        Build = 1 << 2,
        Production = 1 << 3,
        Logistics = 1 << 4,
        Communication = 1 << 5,
        Administration = 1 << 6
    }

    public sealed class OnlinePlayerPermissionRecord
    {
        public string PlayerId { get; }

        public OnlinePermission Permissions { get; private set; }

        public OnlinePlayerPermissionRecord(
            string playerId)
        {
            PlayerId =
                playerId ?? string.Empty;

            Permissions =
                OnlinePermission.None;
        }

        public void SetPermissions(
            OnlinePermission permissions)
        {
            Permissions = permissions;
        }

        public bool HasPermission(
            OnlinePermission permission)
        {
            if (permission == OnlinePermission.None)
            {
                return false;
            }

            return (Permissions & permission) ==
                   permission;
        }
    }

    public sealed class OnlinePlayerPermissions
    {
        private readonly Dictionary<
            string,
            OnlinePlayerPermissionRecord> players =
            new Dictionary<
                string,
                OnlinePlayerPermissionRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PlayerCount =>
            players.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            players.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterPlayer(
            string playerId,
            OnlinePermission permissions)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (players.ContainsKey(id))
            {
                return false;
            }

            OnlinePlayerPermissionRecord record =
                new OnlinePlayerPermissionRecord(id);

            record.SetPermissions(permissions);

            players.Add(id, record);

            return true;
        }

        public bool SetPermissions(
            string playerId,
            OnlinePermission permissions)
        {
            OnlinePlayerPermissionRecord record =
                GetPlayer(playerId);

            if (record == null)
            {
                return false;
            }

            record.SetPermissions(permissions);

            return true;
        }

        public bool GrantPermission(
            string playerId,
            OnlinePermission permission)
        {
            OnlinePlayerPermissionRecord record =
                GetPlayer(playerId);

            if (record == null ||
                permission == OnlinePermission.None)
            {
                return false;
            }

            record.SetPermissions(
                record.Permissions | permission);

            return true;
        }

        public bool RevokePermission(
            string playerId,
            OnlinePermission permission)
        {
            OnlinePlayerPermissionRecord record =
                GetPlayer(playerId);

            if (record == null ||
                permission == OnlinePermission.None)
            {
                return false;
            }

            record.SetPermissions(
                record.Permissions & ~permission);

            return true;
        }

        public bool HasPermission(
            string playerId,
            OnlinePermission permission)
        {
            OnlinePlayerPermissionRecord record =
                GetPlayer(playerId);

            return record != null &&
                   record.HasPermission(permission);
        }

        public OnlinePlayerPermissionRecord
            GetPlayer(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            players.TryGetValue(
                playerId.Trim(),
                out OnlinePlayerPermissionRecord record);

            return record;
        }

        public IReadOnlyCollection<
            OnlinePlayerPermissionRecord>
            GetPlayers()
        {
            return players.Values;
        }

        public void Reset()
        {
            players.Clear();
            Initialized = false;
        }
    }
}
