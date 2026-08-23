using System;
using System.Collections.Generic;
using System.Linq;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class LeaderboardEntry
    {
        public string PlayerId { get; }

        public long Score { get; private set; }

        public LeaderboardEntry(
            string playerId,
            long score)
        {
            PlayerId =
                playerId ?? string.Empty;

            Score =
                Math.Max(0L, score);
        }

        public bool SetScore(
            long score)
        {
            if (score < 0L)
            {
                return false;
            }

            Score = score;

            return true;
        }
    }

    public sealed class Leaderboards
    {
        private readonly Dictionary<
            string,
            Dictionary<string, LeaderboardEntry>> boards =
            new Dictionary<
                string,
                Dictionary<string, LeaderboardEntry>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int LeaderboardCount =>
            boards.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            boards.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateLeaderboard(
            string leaderboardId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(leaderboardId))
            {
                return false;
            }

            string id =
                leaderboardId.Trim();

            if (boards.ContainsKey(id))
            {
                return false;
            }

            boards.Add(
                id,
                new Dictionary<string, LeaderboardEntry>(
                    StringComparer.OrdinalIgnoreCase));

            return true;
        }

        public bool SetPlayerScore(
            string leaderboardId,
            string playerId,
            long score)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(leaderboardId) ||
                string.IsNullOrWhiteSpace(playerId) ||
                score < 0L)
            {
                return false;
            }

            if (!boards.TryGetValue(
                    leaderboardId.Trim(),
                    out Dictionary<string, LeaderboardEntry> board))
            {
                return false;
            }

            string id =
                playerId.Trim();

            if (!board.TryGetValue(
                    id,
                    out LeaderboardEntry entry))
            {
                board.Add(
                    id,
                    new LeaderboardEntry(
                        id,
                        score));

                return true;
            }

            return entry.SetScore(score);
        }

        public IReadOnlyList<
            LeaderboardEntry>
            GetLeaderboard(
                string leaderboardId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(leaderboardId))
            {
                return Array.Empty<LeaderboardEntry>();
            }

            if (!boards.TryGetValue(
                    leaderboardId.Trim(),
                    out Dictionary<string, LeaderboardEntry> board))
            {
                return Array.Empty<LeaderboardEntry>();
            }

            return board.Values
                .OrderByDescending(
                    entry => entry.Score)
                .ThenBy(
                    entry => entry.PlayerId,
                    StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public long GetPlayerScore(
            string leaderboardId,
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(leaderboardId) ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return 0L;
            }

            if (!boards.TryGetValue(
                    leaderboardId.Trim(),
                    out Dictionary<string, LeaderboardEntry> board))
            {
                return 0L;
            }

            if (!board.TryGetValue(
                    playerId.Trim(),
                    out LeaderboardEntry entry))
            {
                return 0L;
            }

            return entry.Score;
        }

        public bool RemovePlayer(
            string leaderboardId,
            string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(leaderboardId) ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return false;
            }

            if (!boards.TryGetValue(
                    leaderboardId.Trim(),
                    out Dictionary<string, LeaderboardEntry> board))
            {
                return false;
            }

            return board.Remove(
                playerId.Trim());
        }

        public bool RemoveLeaderboard(
            string leaderboardId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(leaderboardId))
            {
                return false;
            }

            return boards.Remove(
                leaderboardId.Trim());
        }

        public void Reset()
        {
            boards.Clear();
            Initialized = false;
        }
    }
}
