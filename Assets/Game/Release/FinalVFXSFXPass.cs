using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Release
{
    public enum VFXSFXCategory
    {
        CombatVFX,
        EnvironmentalVFX,
        UnitVFX,
        UIAudio,
        CombatAudio,
        AmbientAudio,
        Music,
        Voice,
        VRFeedback
    }

    public sealed class VFXSFXReviewItem
    {
        public string ItemId { get; }

        public VFXSFXCategory Category { get; }

        public bool Passed { get; private set; }

        public string Notes { get; private set; }

        public VFXSFXReviewItem(
            string itemId,
            VFXSFXCategory category)
        {
            ItemId =
                itemId ?? string.Empty;

            Category = category;
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

    public sealed class FinalVFXSFXPass
    {
        private readonly Dictionary<
            string,
            VFXSFXReviewItem> items =
            new Dictionary<
                string,
                VFXSFXReviewItem>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ItemCount =>
            items.Count;

        public int PassedCount
        {
            get
            {
                int count = 0;

                foreach (
                    VFXSFXReviewItem item
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
            VFXSFXCategory category)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
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
                new VFXSFXReviewItem(
                    id,
                    category));

            return true;
        }

        public bool PassItem(
            string itemId,
            string notes)
        {
            VFXSFXReviewItem item =
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
            VFXSFXReviewItem item =
                GetItem(itemId);

            if (item == null)
            {
                return false;
            }

            item.Fail(notes);

            return true;
        }

        public VFXSFXReviewItem GetItem(
            string itemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return null;
            }

            items.TryGetValue(
                itemId.Trim(),
                out VFXSFXReviewItem item);

            return item;
        }

        public IReadOnlyCollection<
            VFXSFXReviewItem>
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
