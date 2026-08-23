using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class PersistentPlayerData
    {
        private readonly Dictionary<
            string,
            string> values =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public string PlayerId { get; }

        public bool Initialized { get; private set; }

        public int DataCount =>
            values.Count;

        public PersistentPlayerData(
            string playerId)
        {
            PlayerId =
                playerId ?? string.Empty;
        }

        public bool Initialize()
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(PlayerId))
            {
                return false;
            }

            values.Clear();
            Initialized = true;

            return true;
        }

        public bool SetValue(
            string key,
            string value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            values[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public string GetValue(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            values.TryGetValue(
                key.Trim(),
                out string value);

            return value;
        }

        public bool RemoveValue(
            string key)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return values.Remove(
                key.Trim());
        }

        public IReadOnlyDictionary<
            string,
            string>
            GetValues()
        {
            return values;
        }

        public void Reset()
        {
            values.Clear();
            Initialized = false;
        }
    }

    public sealed class PersistentPlayerDataService
    {
        private readonly Dictionary<
            string,
            PersistentPlayerData> players =
            new Dictionary<
                string,
                PersistentPlayerData>(
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
            string playerId)
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

            PersistentPlayerData data =
                new PersistentPlayerData(id);

            data.Initialize();

            players.Add(
                id,
                data);

            return true;
        }

        public PersistentPlayerData GetPlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            players.TryGetValue(
                playerId.Trim(),
                out PersistentPlayerData data);

            return data;
        }

        public bool RemovePlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return players.Remove(
                playerId.Trim());
        }

        public IReadOnlyCollection<
            PersistentPlayerData>
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
