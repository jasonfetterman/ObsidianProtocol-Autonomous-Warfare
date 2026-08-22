using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineMatchState
    {
        None,
        Lobby,
        Preparing,
        Running,
        Ending,
        Ended
    }

    public sealed class OnlineSessionManagement
    {
        private readonly Dictionary<string, OnlinePlayerIdentity> players =
            new Dictionary<string, OnlinePlayerIdentity>(
                StringComparer.OrdinalIgnoreCase);

        public string SessionId { get; private set; }

        public OnlineMatchState State { get; private set; }

        public bool Initialized { get; private set; }

        public int PlayerCount =>
            players.Count;

        public bool Initialize(string sessionId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(sessionId))
            {
                return false;
            }

            SessionId = sessionId.Trim();
            State = OnlineMatchState.Lobby;
            players.Clear();

            Initialized = true;

            return true;
        }

        public bool AddPlayer(
            OnlinePlayerIdentity player)
        {
            if (!Initialized ||
                player == null ||
                !player.Valid ||
                players.ContainsKey(player.PlayerId))
            {
                return false;
            }

            players.Add(player.PlayerId, player);

            return true;
        }

        public bool RemovePlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return players.Remove(playerId.Trim());
        }

        public OnlinePlayerIdentity GetPlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            players.TryGetValue(
                playerId.Trim(),
                out OnlinePlayerIdentity player);

            return player;
        }

        public bool BeginPreparation()
        {
            if (!Initialized ||
                State != OnlineMatchState.Lobby)
            {
                return false;
            }

            State = OnlineMatchState.Preparing;

            return true;
        }

        public bool StartMatch()
        {
            if (!Initialized ||
                State != OnlineMatchState.Preparing ||
                players.Count == 0)
            {
                return false;
            }

            State = OnlineMatchState.Running;

            return true;
        }

        public bool EndMatch()
        {
            if (!Initialized ||
                State != OnlineMatchState.Running)
            {
                return false;
            }

            State = OnlineMatchState.Ending;
            State = OnlineMatchState.Ended;

            return true;
        }

        public bool IsRunning()
        {
            return Initialized &&
                   State == OnlineMatchState.Running;
        }

        public IReadOnlyCollection<OnlinePlayerIdentity>
            GetPlayers()
        {
            return players.Values;
        }

        public void Reset()
        {
            players.Clear();

            SessionId = string.Empty;
            State = OnlineMatchState.None;
            Initialized = false;
        }
    }
}
