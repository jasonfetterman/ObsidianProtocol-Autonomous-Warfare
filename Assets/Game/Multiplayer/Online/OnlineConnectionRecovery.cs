using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineConnectionState
    {
        Connected,
        Disconnected,
        Reconnecting,
        Recovered,
        Failed
    }

    public sealed class OnlineConnectionRecovery
    {
        private readonly Dictionary<
            string,
            OnlineConnectionState> playerStates =
            new Dictionary<
                string,
                OnlineConnectionState>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<
            string,
            int> reconnectAttempts =
            new Dictionary<
                string,
                int>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PlayerCount =>
            playerStates.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            playerStates.Clear();
            reconnectAttempts.Clear();

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

            if (playerStates.ContainsKey(id))
            {
                return false;
            }

            playerStates.Add(
                id,
                OnlineConnectionState.Connected);

            reconnectAttempts.Add(
                id,
                0);

            return true;
        }

        public bool MarkDisconnected(
            string playerId)
        {
            return SetState(
                playerId,
                OnlineConnectionState.Disconnected);
        }

        public bool BeginReconnect(
            string playerId)
        {
            if (!SetState(
                    playerId,
                    OnlineConnectionState.Reconnecting))
            {
                return false;
            }

            string id =
                playerId.Trim();

            reconnectAttempts[id] =
                reconnectAttempts[id] + 1;

            return true;
        }

        public bool MarkRecovered(
            string playerId)
        {
            if (!SetState(
                    playerId,
                    OnlineConnectionState.Recovered))
            {
                return false;
            }

            reconnectAttempts[
                playerId.Trim()] = 0;

            return true;
        }

        public bool MarkFailed(
            string playerId)
        {
            return SetState(
                playerId,
                OnlineConnectionState.Failed);
        }

        public OnlineConnectionState
            GetState(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return OnlineConnectionState.Failed;
            }

            playerStates.TryGetValue(
                playerId.Trim(),
                out OnlineConnectionState state);

            return state;
        }

        public int GetReconnectAttempts(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return 0;
            }

            reconnectAttempts.TryGetValue(
                playerId.Trim(),
                out int attempts);

            return attempts;
        }

        public bool CanReconnect(
            string playerId,
            int maximumAttempts)
        {
            if (maximumAttempts <= 0)
            {
                return false;
            }

            OnlineConnectionState state =
                GetState(playerId);

            return state ==
                       OnlineConnectionState.Disconnected ||
                   state ==
                       OnlineConnectionState.Reconnecting;
        }

        private bool SetState(
            string playerId,
            OnlineConnectionState state)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (!playerStates.ContainsKey(id))
            {
                return false;
            }

            playerStates[id] = state;

            return true;
        }

        public void Reset()
        {
            playerStates.Clear();
            reconnectAttempts.Clear();

            Initialized = false;
        }
    }
}
