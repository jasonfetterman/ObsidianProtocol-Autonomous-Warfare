using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public sealed class OnlineResourceState
    {
        private readonly Dictionary<string, int> resources =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

        public string OwnerPlayerId { get; }

        public OnlineResourceState(
            string ownerPlayerId)
        {
            OwnerPlayerId =
                ownerPlayerId ?? string.Empty;
        }

        public bool RegisterResource(
            string resourceId,
            int amount = 0)
        {
            if (string.IsNullOrWhiteSpace(resourceId) ||
                amount < 0)
            {
                return false;
            }

            string id =
                resourceId.Trim();

            if (resources.ContainsKey(id))
            {
                return false;
            }

            resources.Add(id, amount);

            return true;
        }

        public bool SetAmount(
            string resourceId,
            int amount)
        {
            if (string.IsNullOrWhiteSpace(resourceId) ||
                amount < 0)
            {
                return false;
            }

            string id =
                resourceId.Trim();

            if (!resources.ContainsKey(id))
            {
                return false;
            }

            resources[id] = amount;

            return true;
        }

        public int GetAmount(
            string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                return 0;
            }

            resources.TryGetValue(
                resourceId.Trim(),
                out int amount);

            return amount;
        }

        public IReadOnlyDictionary<string, int>
            GetResources()
        {
            return resources;
        }
    }

    public sealed class OnlineResourceSynchronization
    {
        private readonly Dictionary<
            string,
            OnlineResourceState> playerResources =
            new Dictionary<
                string,
                OnlineResourceState>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PlayerCount =>
            playerResources.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            playerResources.Clear();
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

            if (playerResources.ContainsKey(id))
            {
                return false;
            }

            playerResources.Add(
                id,
                new OnlineResourceState(id));

            return true;
        }

        public bool RegisterResource(
            string playerId,
            string resourceId,
            int amount = 0)
        {
            OnlineResourceState state =
                GetPlayerResources(playerId);

            return state != null &&
                   state.RegisterResource(
                       resourceId,
                       amount);
        }

        public bool SynchronizeResource(
            string playerId,
            string resourceId,
            int amount)
        {
            OnlineResourceState state =
                GetPlayerResources(playerId);

            return state != null &&
                   state.SetAmount(
                       resourceId,
                       amount);
        }

        public int GetAmount(
            string playerId,
            string resourceId)
        {
            OnlineResourceState state =
                GetPlayerResources(playerId);

            return state == null
                ? 0
                : state.GetAmount(resourceId);
        }

        public OnlineResourceState
            GetPlayerResources(
                string playerId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(playerId))
            {
                return null;
            }

            playerResources.TryGetValue(
                playerId.Trim(),
                out OnlineResourceState state);

            return state;
        }

        public IReadOnlyCollection<
            OnlineResourceState>
            GetAllPlayerResources()
        {
            return playerResources.Values;
        }

        public void Reset()
        {
            playerResources.Clear();
            Initialized = false;
        }
    }
}
