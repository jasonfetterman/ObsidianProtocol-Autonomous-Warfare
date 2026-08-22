using System;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public sealed class SharedBattlefieldSimulation
    {
        private SharedWorldState worldState;
        private OfflineMultiplayerSession session;

        public bool Initialized { get; private set; }

        public bool Running { get; private set; }

        public long SimulationTick =>
            worldState != null
                ? worldState.SimulationTick
                : 0;

        public bool Initialize(
            OfflineMultiplayerSession offlineSession,
            SharedWorldState sharedWorldState)
        {
            if (Initialized ||
                offlineSession == null ||
                sharedWorldState == null)
            {
                return false;
            }

            if (!offlineSession.Valid ||
                !sharedWorldState.Initialized)
            {
                return false;
            }

            session = offlineSession;
            worldState = sharedWorldState;

            Initialized = true;
            Running = false;

            return true;
        }

        public bool Start()
        {
            if (!Initialized ||
                session == null ||
                worldState == null)
            {
                return false;
            }

            if (session.State !=
                    OfflineSessionState.Running)
            {
                return false;
            }

            if (worldState.WorldPaused)
            {
                return false;
            }

            Running = true;

            return true;
        }

        public bool Pause()
        {
            if (!Initialized ||
                !Running ||
                worldState == null)
            {
                return false;
            }

            worldState.SetPaused(true);

            Running = false;

            return true;
        }

        public bool Resume()
        {
            if (!Initialized ||
                worldState == null)
            {
                return false;
            }

            if (session.State !=
                    OfflineSessionState.Running)
            {
                return false;
            }

            worldState.SetPaused(false);

            Running = true;

            return true;
        }

        public bool SimulateTick()
        {
            if (!Initialized ||
                !Running ||
                worldState == null)
            {
                return false;
            }

            return worldState
                .AdvanceSimulationTick();
        }

        public bool Stop()
        {
            if (!Initialized)
            {
                return false;
            }

            Running = false;

            if (worldState != null)
            {
                worldState.SetPaused(false);
            }

            return true;
        }

        public bool IsPlayerCommandValid(
            OfflinePlayerId playerId)
        {
            if (!Initialized ||
                session == null ||
                playerId == OfflinePlayerId.None)
            {
                return false;
            }

            OfflinePlayerContext player =
                session.GetPlayer(playerId);

            return player != null &&
                   player.Connected &&
                   player.CommandAuthority;
        }

        public void Reset()
        {
            Stop();

            worldState = null;
            session = null;

            Initialized = false;
        }
    }
}
