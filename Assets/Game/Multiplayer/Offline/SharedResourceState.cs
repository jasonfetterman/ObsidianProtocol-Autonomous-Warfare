using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public sealed class SharedResourceState
    {
        private readonly Dictionary<string, int> resources =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            resources.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterResource(
            string resourceId,
            int startingAmount = 0)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId) ||
                startingAmount < 0)
            {
                return false;
            }

            string id = resourceId.Trim();

            if (resources.ContainsKey(id))
            {
                return false;
            }

            resources.Add(id, startingAmount);

            return true;
        }

        public bool AddResource(
            string resourceId,
            int amount)
        {
            if (!Initialized ||
                amount < 0 ||
                string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            string id = resourceId.Trim();

            if (!resources.ContainsKey(id))
            {
                return false;
            }

            resources[id] += amount;

            return true;
        }

        public bool SpendResource(
            string resourceId,
            int amount)
        {
            if (!Initialized ||
                amount < 0 ||
                string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            string id = resourceId.Trim();

            if (!resources.TryGetValue(
                    id,
                    out int currentAmount))
            {
                return false;
            }

            if (currentAmount < amount)
            {
                return false;
            }

            resources[id] =
                currentAmount - amount;

            return true;
        }

        public bool TransferResource(
            string resourceId,
            int amount)
        {
            return SpendResource(
                resourceId,
                amount);
        }

        public int GetAmount(
            string resourceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId))
            {
                return 0;
            }

            resources.TryGetValue(
                resourceId.Trim(),
                out int amount);

            return amount;
        }

        public bool HasResource(
            string resourceId,
            int requiredAmount)
        {
            if (requiredAmount < 0)
            {
                return false;
            }

            return GetAmount(resourceId) >=
                   requiredAmount;
        }

        public IReadOnlyDictionary<
            string,
            int> GetResources()
        {
            return resources;
        }

        public void Reset()
        {
            resources.Clear();
            Initialized = false;
        }
    }
}
