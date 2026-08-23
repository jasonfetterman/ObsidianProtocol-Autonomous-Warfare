using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionStore
    {
        private readonly HashSet<string> items =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ItemCount =>
            items.Count;

        public bool Complete =>
            ItemCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            items.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterItem(
            string itemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return false;
            }

            return items.Add(
                itemId.Trim());
        }

        public bool ContainsItem(
            string itemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return false;
            }

            return items.Contains(
                itemId.Trim());
        }

        public IReadOnlyCollection<string>
            GetItems()
        {
            return items;
        }

        public void Reset()
        {
            items.Clear();
            Initialized = false;
        }
    }
}
