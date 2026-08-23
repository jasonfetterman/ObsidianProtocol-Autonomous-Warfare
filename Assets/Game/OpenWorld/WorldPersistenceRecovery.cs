using System;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum WorldRecoveryState
    {
        Ready,
        Recovering,
        Validated,
        Failed
    }

    public sealed class WorldPersistenceRecovery
    {
        public bool Initialized { get; private set; }

        public WorldRecoveryState State { get; private set; }

        public long LastRecoveredTick { get; private set; }

        public string LastSnapshotId { get; private set; }

        public WorldPersistenceRecovery()
        {
            State =
                WorldRecoveryState.Ready;

            LastRecoveredTick = -1;
            LastSnapshotId = string.Empty;
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            State =
                WorldRecoveryState.Ready;

            LastRecoveredTick = -1;
            LastSnapshotId = string.Empty;

            Initialized = true;

            return true;
        }

        public bool BeginRecovery(
            string snapshotId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(snapshotId) ||
                State ==
                    WorldRecoveryState.Recovering)
            {
                return false;
            }

            LastSnapshotId =
                snapshotId.Trim();

            State =
                WorldRecoveryState.Recovering;

            return true;
        }

        public bool CompleteRecovery(
            long recoveredTick)
        {
            if (!Initialized ||
                State !=
                    WorldRecoveryState.Recovering ||
                recoveredTick < 0)
            {
                return false;
            }

            LastRecoveredTick =
                recoveredTick;

            State =
                WorldRecoveryState.Validated;

            return true;
        }

        public bool FailRecovery()
        {
            if (!Initialized ||
                State !=
                    WorldRecoveryState.Recovering)
            {
                return false;
            }

            State =
                WorldRecoveryState.Failed;

            return true;
        }

        public bool ValidateRecovery()
        {
            if (!Initialized ||
                State !=
                    WorldRecoveryState.Validated ||
                LastRecoveredTick < 0 ||
                string.IsNullOrWhiteSpace(
                    LastSnapshotId))
            {
                return false;
            }

            return true;
        }

        public bool ResetRecovery()
        {
            if (!Initialized)
            {
                return false;
            }

            State =
                WorldRecoveryState.Ready;

            LastRecoveredTick = -1;
            LastSnapshotId = string.Empty;

            return true;
        }

        public void Reset()
        {
            Initialized = false;

            State =
                WorldRecoveryState.Ready;

            LastRecoveredTick = -1;
            LastSnapshotId = string.Empty;
        }
    }
}
