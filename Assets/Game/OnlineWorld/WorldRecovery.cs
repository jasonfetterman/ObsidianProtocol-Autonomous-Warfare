using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class WorldRecovery
    {
        private readonly Dictionary<
            string,
            string> recoveryState =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RecoveryStateCount =>
            recoveryState.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            recoveryState.Clear();
            Initialized = true;

            return true;
        }

        public bool SaveCheckpoint(
            string checkpointId,
            string state)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(checkpointId))
            {
                return false;
            }

            recoveryState[
                checkpointId.Trim()] =
                state ?? string.Empty;

            return true;
        }

        public string GetCheckpoint(
            string checkpointId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(checkpointId))
            {
                return null;
            }

            recoveryState.TryGetValue(
                checkpointId.Trim(),
                out string state);

            return state;
        }

        public bool RestoreCheckpoint(
            string checkpointId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(checkpointId))
            {
                return false;
            }

            return recoveryState.ContainsKey(
                checkpointId.Trim());
        }

        public bool RemoveCheckpoint(
            string checkpointId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(checkpointId))
            {
                return false;
            }

            return recoveryState.Remove(
                checkpointId.Trim());
        }

        public IReadOnlyDictionary<
            string,
            string>
            GetCheckpoints()
        {
            return recoveryState;
        }

        public void Reset()
        {
            recoveryState.Clear();
            Initialized = false;
        }
    }
}
