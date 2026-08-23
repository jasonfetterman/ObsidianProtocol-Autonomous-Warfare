using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class Statistics
    {
        private readonly Dictionary<
            string,
            Dictionary<string, long>> playerStats =
            new Dictionary<
                string,
                Dictionary<string, long>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PlayerCount =>
            playerStats.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            playerStats.Clear();
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

            if (playerStats.ContainsKey(id))
            {
                return false;
            }

            playerStats.Add(
                id,
                new Dictionary<string, long>(
                    StringComparer.OrdinalIgnoreCase));

            return true;
        }

        public bool SetStatistic(
            string playerId,
            string statisticId,
            long value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(statisticId) ||
                value < 0)
            {
                return false;
            }

            if (!playerStats.TryGetValue(
                    playerId.Trim(),
                    out Dictionary<string, long> stats))
            {
                return false;
            }

            stats[statisticId.Trim()] =
                value;

            return true;
        }

        public long GetStatistic(
            string playerId,
            string statisticId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(statisticId))
            {
                return 0L;
            }

            if (!playerStats.TryGetValue(
                    playerId.Trim(),
                    out Dictionary<string, long> stats))
            {
                return 0L;
            }

            stats.TryGetValue(
                statisticId.Trim(),
                out long value);

            return value;
        }

        public bool AddStatistic(
            string playerId,
            string statisticId,
            long amount)
        {
            if (amount < 0)
            {
                return false;
            }

            long current =
                GetStatistic(
                    playerId,
                    statisticId);

            return SetStatistic(
                playerId,
                statisticId,
                current + amount);
        }

        public bool RemovePlayer(
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            return playerStats.Remove(
                playerId.Trim());
        }

        public void Reset()
        {
            playerStats.Clear();
            Initialized = false;
        }
    }
}
