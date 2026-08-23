using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class ServerAdministration
    {
        private readonly Dictionary<
            string,
            bool> serverStatus =
            new Dictionary<
                string,
                bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ServerCount =>
            serverStatus.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            serverStatus.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterServer(
            string serverId,
            bool online)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId))
            {
                return false;
            }

            string id =
                serverId.Trim();

            if (serverStatus.ContainsKey(id))
            {
                return false;
            }

            serverStatus.Add(
                id,
                online);

            return true;
        }

        public bool SetServerOnline(
            string serverId,
            bool online)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId))
            {
                return false;
            }

            string id =
                serverId.Trim();

            if (!serverStatus.ContainsKey(id))
            {
                return false;
            }

            serverStatus[id] =
                online;

            return true;
        }

        public bool IsServerOnline(
            string serverId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId))
            {
                return false;
            }

            return serverStatus.TryGetValue(
                serverId.Trim(),
                out bool online) &&
                   online;
        }

        public bool RemoveServer(
            string serverId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId))
            {
                return false;
            }

            return serverStatus.Remove(
                serverId.Trim());
        }

        public IReadOnlyDictionary<
            string,
            bool>
            GetServers()
        {
            return serverStatus;
        }

        public void Reset()
        {
            serverStatus.Clear();
            Initialized = false;
        }
    }
}
