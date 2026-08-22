using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public sealed class OnlinePlayerState
    {
        public string PlayerId { get; }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public bool Connected { get; private set; }

        public long LastUpdateTick { get; private set; }

        public OnlinePlayerState(string playerId)
        {
            PlayerId = playerId ?? string.Empty;
        }

        public bool Update(
            float x,
            float y,
            float z,
            bool connected,
            long tick)
        {
            if (string.IsNullOrWhiteSpace(PlayerId))
            {
                return false;
            }

            X = x;
            Y = y;
            Z = z;
            Connected = connected;
            LastUpdateTick = tick;

            return true;
        }
    }

    public sealed class OnlinePlayerSynchronization
    {
        private readonly Dictionary<string, OnlinePlayerState> players =
            new Dictionary<string, OnlinePlayerState>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PlayerCount => players.Count;

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

            string id = playerId.Trim();

            if (players.ContainsKey(id))
            {
                return false;
            }

            players.Add(
                id,
                new OnlinePlayerState(id));

            return true;
        }

        public bool SynchronizePlayer(
            string playerId,
            float x,
            float y,
            float z,
            bool connected,
            long tick)
        {
            OnlinePlayerState player =
                GetPlayer(playerId);

            return player != null &&
                   player.Update(
                       x,
                       y,
                       z,
                       connected,
                       tick);
        }

        public OnlinePlayerState GetPlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            players.TryGetValue(
                playerId.Trim(),
                out OnlinePlayerState player);

            return player;
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

        public IReadOnlyCollection<OnlinePlayerState>
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
