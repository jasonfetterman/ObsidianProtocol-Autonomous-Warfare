using System;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public sealed class TwoPlayerBattleTest
    {
        private OfflineMultiplayerSession session;
        private SharedWorldState worldState;
        private SharedBattlefieldSimulation simulation;

        public bool Initialized { get; private set; }

        public bool Passed { get; private set; }

        public long SimulationTick =>
            worldState != null
                ? worldState.SimulationTick
                : 0;

        public bool Initialize(
            OfflineMultiplayerSession offlineSession,
            SharedWorldState sharedWorld,
            SharedBattlefieldSimulation battlefield)
        {
            if (Initialized ||
                offlineSession == null ||
                sharedWorld == null ||
                battlefield == null)
            {
                return false;
            }

            if (!offlineSession.Valid ||
                !sharedWorld.Initialized ||
                !battlefield.Initialized)
            {
                return false;
            }

            session = offlineSession;
            worldState = sharedWorld;
            simulation = battlefield;

            Initialized = true;
            Passed = false;

            return true;
        }

        public bool RunBattleTest()
        {
            if (!Initialized ||
                session == null ||
                worldState == null ||
                simulation == null)
            {
                return false;
            }

            if (session.State !=
                OfflineSessionState.Running)
            {
                return false;
            }

            if (!ValidatePlayers())
            {
                return false;
            }

            if (!simulation.Running)
            {
                if (!simulation.Start())
                {
                    return false;
                }
            }

            long startingTick =
                worldState.SimulationTick;

            if (!simulation.SimulateTick())
            {
                return false;
            }

            if (worldState.SimulationTick <=
                startingTick)
            {
                return false;
            }

            Passed = true;

            return true;
        }

        private bool ValidatePlayers()
        {
            OfflinePlayerContext player1 =
                session.GetPlayer(
                    OfflinePlayerId.Player1);

            OfflinePlayerContext player2 =
                session.GetPlayer(
                    OfflinePlayerId.Player2);

            return player1 != null &&
                   player2 != null &&
                   player1.Connected &&
                   player2.Connected &&
                   player1.CommandAuthority &&
                   player2.CommandAuthority;
        }

        public void Reset()
        {
            if (simulation != null)
            {
                simulation.Stop();
            }

            session = null;
            worldState = null;
            simulation = null;

            Initialized = false;
            Passed = false;
        }
    }
}
