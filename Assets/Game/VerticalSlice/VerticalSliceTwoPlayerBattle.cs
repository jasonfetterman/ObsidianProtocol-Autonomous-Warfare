using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSlicePlayerSide
    {
        Warden,
        Enemy
    }

    public sealed class VerticalSlicePlayer
    {
        public string PlayerId { get; }

        public VerticalSlicePlayerSide Side { get; }

        public bool Connected { get; private set; }

        public VerticalSlicePlayer(
            string playerId,
            VerticalSlicePlayerSide side)
        {
            PlayerId =
                playerId ?? string.Empty;

            Side =
                side;

            Connected = false;
        }

        public bool Connect()
        {
            if (Connected)
            {
                return false;
            }

            Connected = true;

            return true;
        }

        public bool Disconnect()
        {
            if (!Connected)
            {
                return false;
            }

            Connected = false;

            return true;
        }
    }

    public sealed class VerticalSliceTwoPlayerBattle
    {
        private readonly Dictionary<
            string,
            VerticalSlicePlayer> players =
            new Dictionary<
                string,
                VerticalSlicePlayer>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool BattleActive { get; private set; }

        public int PlayerCount =>
            players.Count;

        public bool TwoPlayersConnected =>
            GetConnectedPlayerCount() >= 2;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            players.Clear();

            BattleActive = false;
            Initialized = true;

            return true;
        }

        public bool RegisterPlayer(
            string playerId,
            VerticalSlicePlayerSide side)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                players.Count >= 2)
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (players.ContainsKey(id))
            {
                return false;
            }

            foreach (VerticalSlicePlayer player
                     in players.Values)
            {
                if (player.Side == side)
                {
                    return false;
                }
            }

            players.Add(
                id,
                new VerticalSlicePlayer(
                    id,
                    side));

            return true;
        }

        public bool ConnectPlayer(
            string playerId)
        {
            VerticalSlicePlayer player =
                GetPlayer(playerId);

            return player != null &&
                   player.Connect();
        }

        public bool StartBattle()
        {
            if (!Initialized ||
                BattleActive ||
                !TwoPlayersConnected)
            {
                return false;
            }

            BattleActive = true;

            return true;
        }

        public bool EndBattle()
        {
            if (!BattleActive)
            {
                return false;
            }

            BattleActive = false;

            return true;
        }

        public VerticalSlicePlayer GetPlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            players.TryGetValue(
                playerId.Trim(),
                out VerticalSlicePlayer player);

            return player;
        }

        public int GetConnectedPlayerCount()
        {
            int count = 0;

            foreach (VerticalSlicePlayer player
                     in players.Values)
            {
                if (player.Connected)
                {
                    count++;
                }
            }

            return count;
        }

        public IReadOnlyCollection<
            VerticalSlicePlayer>
            GetPlayers()
        {
            return players.Values;
        }

        public void Reset()
        {
            players.Clear();

            BattleActive = false;
            Initialized = false;
        }
    }
}
