using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Release
{
    public sealed class UIUXReviewItem
    {
        public string ItemId { get; }

        public string Area { get; }

        public bool Passed { get; private set; }

        public string Notes { get; private set; }

        public UIUXReviewItem(
            string itemId,
            string area)
        {
            ItemId =
                itemId ?? string.Empty;

            Area =
                area ?? string.Empty;

            Passed = false;
            Notes = string.Empty;
        }

        public void Pass(string notes)
        {
            Passed = true;
            Notes = notes ?? string.Empty;
        }

        public void Fail(string notes)
        {
            Passed = false;
            Notes = notes ?? string.Empty;
        }
    }

    public sealed class FinalUIUXPass
    {
        private readonly Dictionary<
            string,
            UIUXReviewItem> items =
            new Dictionary<
                string,
                UIUXReviewItem>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ItemCount =>
            items.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (UIUXReviewItem item
                         in items.Values)
                {
                    if (item.Passed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int FailedCount =>
            ItemCount - PassedCount;

        public bool Approved =>
            Initialized &&
            ItemCount > 0 &&
            FailedCount == 0;

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
            string itemId,
            string area)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId) ||
                string.IsNullOrWhiteSpace(area))
            {
                return false;
            }

            string id =
                itemId.Trim();

            if (items.ContainsKey(id))
            {
                return false;
            }

            items.Add(
                id,
                new UIUXReviewItem(
                    id,
                    area.Trim()));

            return true;
        }

        public bool PassItem(
            string itemId,
            string notes)
        {
            UIUXReviewItem item =
                GetItem(itemId);

            if (item == null)
            {
                return false;
            }

            item.Pass(notes);

            return true;
        }

        public bool FailItem(
            string itemId,
            string notes)
        {
            UIUXReviewItem item =
                GetItem(itemId);

            if (item == null)
            {
                return false;
            }

            item.Fail(notes);

            return true;
        }

        public UIUXReviewItem GetItem(
            string itemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return null;
            }

            items.TryGetValue(
                itemId.Trim(),
                out UIUXReviewItem item);

            return item;
        }

        public IReadOnlyCollection<
            UIUXReviewItem>
            GetItems()
        {
            return items.Values;
        }

        public void Reset()
        {
            items.Clear();
            Initialized = false;
        }
    }
}
