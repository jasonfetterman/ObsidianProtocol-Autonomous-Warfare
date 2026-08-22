using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public enum StoreItemCategory
    {
        Unit,
        Equipment,
        Module,
        FabricationMaterial,
        Repair,
        GarageSlot,
        Cosmetic,
        Convenience,
        CampaignResource
    }

    public enum StoreItemAvailability
    {
        Unavailable,
        Available,
        Locked,
        SoldOut
    }

    public sealed class StoreItem
    {
        public string ItemId { get; }
        public string DisplayName { get; }
        public StoreItemCategory Category { get; }

        public int CreditCost { get; }

        public StoreItemAvailability Availability
        {
            get;
            private set;
        }

        public bool Purchasable =>
            Availability ==
            StoreItemAvailability.Available;

        public StoreItem(
            string itemId,
            string displayName,
            StoreItemCategory category,
            int creditCost)
        {
            ItemId =
                itemId ?? string.Empty;

            DisplayName =
                displayName ?? string.Empty;

            Category = category;

            CreditCost =
                Math.Max(0, creditCost);

            Availability =
                StoreItemAvailability.Unavailable;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(ItemId) &&
            !string.IsNullOrWhiteSpace(DisplayName) &&
            CreditCost >= 0;

        public void MakeAvailable()
        {
            Availability =
                StoreItemAvailability.Available;
        }

        public void Lock()
        {
            Availability =
                StoreItemAvailability.Locked;
        }

        public void MarkSoldOut()
        {
            Availability =
                StoreItemAvailability.SoldOut;
        }

        public void ResetAvailability()
        {
            Availability =
                StoreItemAvailability.Unavailable;
        }
    }

    public sealed class MilitaryStore
    {
        private readonly Dictionary<
            string,
            StoreItem> catalog =
            new Dictionary<
                string,
                StoreItem>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterItem(
            StoreItem item)
        {
            if (item == null ||
                !item.Valid ||
                catalog.ContainsKey(
                    item.ItemId))
            {
                return false;
            }

            catalog.Add(
                item.ItemId,
                item);

            return true;
        }

        public bool RemoveItem(
            string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                return false;

            return catalog.Remove(itemId);
        }

        public bool TryGetItem(
            string itemId,
            out StoreItem item)
        {
            return catalog.TryGetValue(
                itemId,
                out item);
        }

        public bool SetAvailable(
            string itemId)
        {
            if (!catalog.TryGetValue(
                    itemId,
                    out StoreItem item))
            {
                return false;
            }

            item.MakeAvailable();
            return true;
        }

        public bool SetLocked(
            string itemId)
        {
            if (!catalog.TryGetValue(
                    itemId,
                    out StoreItem item))
            {
                return false;
            }

            item.Lock();
            return true;
        }

        public bool SetSoldOut(
            string itemId)
        {
            if (!catalog.TryGetValue(
                    itemId,
                    out StoreItem item))
            {
                return false;
            }

            item.MarkSoldOut();
            return true;
        }

        public IReadOnlyCollection<
            StoreItem>
            GetCatalog()
        {
            return catalog.Values;
        }

        public void Clear()
        {
            catalog.Clear();
        }
    }
}
