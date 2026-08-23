using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class MatchHistoryEntry
    {
        public string MatchId { get; }

        public string PlayerId { get; }

        public string Result { get; }

        public DateTime RecordedAtUtc { get; }

        public MatchHistoryEntry(
            string matchId,
            string playerId,
            string result)
        {
            MatchId =
                matchId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            Result =
                result ?? string.Empty;

            RecordedAtUtc =
                DateTime.UtcNow;
        }
    }

    public sealed class MatchHistory
    {
        private readonly List<
            MatchHistoryEntry> history =
            new List<
                MatchHistoryEntry>();

        public bool Initialized { get; private set; }

        public int MatchCount =>
            history.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            history.Clear();
            Initialized = true;

            return true;
        }

        public bool RecordMatch(
            string matchId,
            string playerId,
            string result)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(matchId) ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(result))
            {
                return false;
            }

            history.Add(
                new MatchHistoryEntry(
                    matchId.Trim(),
                    playerId.Trim(),
                    result.Trim()));

            return true;
        }

        public IReadOnlyList<
            MatchHistoryEntry>
            GetHistory()
        {
            return history;
        }

        public IReadOnlyList<
            MatchHistoryEntry>
            GetPlayerHistory(
                string playerId)
        {
            List<MatchHistoryEntry> result =
                new List<MatchHistoryEntry>();

            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return result;
            }

            string id =
                playerId.Trim();

            foreach (MatchHistoryEntry entry
                     in history)
            {
                if (string.Equals(
                        entry.PlayerId,
                        id,
                        StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(entry);
                }
            }

            return result;
        }

        public void Reset()
        {
            history.Clear();
            Initialized = false;
        }
    }
}
