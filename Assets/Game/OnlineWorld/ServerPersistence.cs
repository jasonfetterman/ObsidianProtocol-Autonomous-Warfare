using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class ServerPersistence
    {
        private readonly Dictionary<
            string,
            Dictionary<string, string>> servers =
            new Dictionary<
                string,
                Dictionary<string, string>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ServerCount =>
            servers.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            servers.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterServer(
            string serverId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId))
            {
                return false;
            }

            string id =
                serverId.Trim();

            if (servers.ContainsKey(id))
            {
                return false;
            }

            servers.Add(
                id,
                new Dictionary<string, string>(
                    StringComparer.OrdinalIgnoreCase));

            return true;
        }

        public bool SetServerState(
            string serverId,
            string key,
            string value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId) ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            if (!servers.TryGetValue(
                    serverId.Trim(),
                    out Dictionary<string, string> state))
            {
                return false;
            }

            state[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public string GetServerState(
            string serverId,
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId) ||
                string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            if (!servers.TryGetValue(
                    serverId.Trim(),
                    out Dictionary<string, string> state))
            {
                return null;
            }

            state.TryGetValue(
                key.Trim(),
                out string value);

            return value;
        }

        public bool RemoveServer(
            string serverId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serverId))
            {
                return false;
            }

            return servers.Remove(
                serverId.Trim());
        }

        public void Reset()
        {
            servers.Clear();
            Initialized = false;
        }
    }
}
