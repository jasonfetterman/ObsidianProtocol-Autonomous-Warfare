using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignResources
    {
        private readonly Dictionary<string, int> resources =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ResourceTypeCount =>
            resources.Count;

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
            int startingAmount)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId) ||
                startingAmount < 0)
            {
                return false;
            }

            string id =
                resourceId.Trim();

            if (resources.ContainsKey(id))
            {
                return false;
            }

            resources.Add(
                id,
                startingAmount);

            return true;
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

        public bool Add(
            string resourceId,
            int amount)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId) ||
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

            resources[id] += amount;

            return true;
        }

        public bool Spend(
            string resourceId,
            int amount)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId) ||
                amount < 0)
            {
                return false;
            }

            string id =
                resourceId.Trim();

            if (!resources.TryGetValue(
                    id,
                    out int currentAmount) ||
                currentAmount < amount)
            {
                return false;
            }

            resources[id] =
                currentAmount - amount;

            return true;
        }

        public IReadOnlyDictionary<string, int>
            GetResources()
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
