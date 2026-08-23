using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class PlayerProgressionPersistence
    {
        private readonly Dictionary<
            string,
            Dictionary<string, long>> progression =
            new Dictionary<
                string,
                Dictionary<string, long>>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PlayerCount =>
            progression.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            progression.Clear();
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

            if (progression.ContainsKey(id))
            {
                return false;
            }

            progression.Add(
                id,
                new Dictionary<string, long>(
                    StringComparer.OrdinalIgnoreCase));

            return true;
        }

        public bool SetProgress(
            string playerId,
            string progressionId,
            long value)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(progressionId) ||
                value < 0)
            {
                return false;
            }

            if (!progression.TryGetValue(
                    playerId.Trim(),
                    out Dictionary<string, long> data))
            {
                return false;
            }

            data[progressionId.Trim()] =
                value;

            return true;
        }

        public long GetProgress(
            string playerId,
            string progressionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(progressionId))
            {
                return 0L;
            }

            if (!progression.TryGetValue(
                    playerId.Trim(),
                    out Dictionary<string, long> data))
            {
                return 0L;
            }

            data.TryGetValue(
                progressionId.Trim(),
                out long value);

            return value;
        }

        public bool AddProgress(
            string playerId,
            string progressionId,
            long amount)
        {
            if (amount < 0)
            {
                return false;
            }

            long current =
                GetProgress(
                    playerId,
                    progressionId);

            return SetProgress(
                playerId,
                progressionId,
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

            return progression.Remove(
                playerId.Trim());
        }

        public void Reset()
        {
            progression.Clear();
            Initialized = false;
        }
    }
}
