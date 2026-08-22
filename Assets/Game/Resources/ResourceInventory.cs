using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public sealed class ResourceInventory
    {
        private readonly Dictionary<string, int> amounts = new();

        public int GetAmount(string resourceId)
        {
            return amounts.TryGetValue(resourceId, out int amount) ? amount : 0;
        }

        public void Add(string resourceId, int amount)
        {
            if (string.IsNullOrEmpty(resourceId) || amount <= 0)
            {
                return;
            }

            amounts[resourceId] = GetAmount(resourceId) + amount;
        }

        public bool TrySpend(string resourceId, int amount)
        {
            if (string.IsNullOrEmpty(resourceId) || amount <= 0)
            {
                return false;
            }

            int current = GetAmount(resourceId);

            if (current < amount)
            {
                return false;
            }

            amounts[resourceId] = current - amount;
            return true;
        }
    }
}
