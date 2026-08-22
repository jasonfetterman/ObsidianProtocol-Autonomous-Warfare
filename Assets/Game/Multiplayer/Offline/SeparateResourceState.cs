using System;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public sealed class SeparateResourceState
    {
        private readonly SharedResourceState player1Resources =
            new SharedResourceState();

        private readonly SharedResourceState player2Resources =
            new SharedResourceState();

        public bool Initialized { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            player1Resources.Initialize();
            player2Resources.Initialize();

            Initialized = true;

            return true;
        }

        public bool RegisterResource(
            string resourceId,
            int player1StartingAmount = 0,
            int player2StartingAmount = 0)
        {
            if (!Initialized ||
                player1StartingAmount < 0 ||
                player2StartingAmount < 0)
            {
                return false;
            }

            bool player1Registered =
                player1Resources.RegisterResource(
                    resourceId,
                    player1StartingAmount);

            bool player2Registered =
                player2Resources.RegisterResource(
                    resourceId,
                    player2StartingAmount);

            return player1Registered &&
                   player2Registered;
        }

        public bool AddResource(
            OfflinePlayerId player,
            string resourceId,
            int amount)
        {
            SharedResourceState state =
                GetState(player);

            return state != null &&
                   state.AddResource(
                       resourceId,
                       amount);
        }

        public bool SpendResource(
            OfflinePlayerId player,
            string resourceId,
            int amount)
        {
            SharedResourceState state =
                GetState(player);

            return state != null &&
                   state.SpendResource(
                       resourceId,
                       amount);
        }

        public int GetAmount(
            OfflinePlayerId player,
            string resourceId)
        {
            SharedResourceState state =
                GetState(player);

            return state == null
                ? 0
                : state.GetAmount(resourceId);
        }

        public bool HasResource(
            OfflinePlayerId player,
            string resourceId,
            int requiredAmount)
        {
            SharedResourceState state =
                GetState(player);

            return state != null &&
                   state.HasResource(
                       resourceId,
                       requiredAmount);
        }

        public bool TransferBetweenPlayers(
            string resourceId,
            int amount,
            OfflinePlayerId fromPlayer,
            OfflinePlayerId toPlayer)
        {
            if (!Initialized ||
                amount <= 0 ||
                fromPlayer == toPlayer)
            {
                return false;
            }

            SharedResourceState from =
                GetState(fromPlayer);

            SharedResourceState to =
                GetState(toPlayer);

            if (from == null || to == null)
            {
                return false;
            }

            if (!from.SpendResource(
                    resourceId,
                    amount))
            {
                return false;
            }

            if (!to.AddResource(
                    resourceId,
                    amount))
            {
                from.AddResource(
                    resourceId,
                    amount);

                return false;
            }

            return true;
        }

        private SharedResourceState GetState(
            OfflinePlayerId player)
        {
            if (!Initialized)
            {
                return null;
            }

            switch (player)
            {
                case OfflinePlayerId.Player1:
                    return player1Resources;

                case OfflinePlayerId.Player2:
                    return player2Resources;

                default:
                    return null;
            }
        }

        public void Reset()
        {
            player1Resources.Reset();
            player2Resources.Reset();

            Initialized = false;
        }
    }
}
