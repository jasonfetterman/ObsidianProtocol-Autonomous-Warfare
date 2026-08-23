using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum VRStoreArea
    {
        Entrance,
        UnitShowroom,
        Equipment,
        Customization,
        Fabrication,
        Purchase,
        Exit
    }

    public sealed class VRStoreItem
    {
        public string ItemId { get; }

        public string DisplayName { get; }

        public int CreditCost { get; }

        public bool Available { get; private set; }

        public VRStoreItem(
            string itemId,
            string displayName,
            int creditCost)
        {
            ItemId =
                itemId ?? string.Empty;

            DisplayName =
                displayName ?? string.Empty;

            CreditCost =
                Math.Max(0, creditCost);

            Available = true;
        }

        public bool SetAvailable(
            bool available)
        {
            Available = available;

            return true;
        }
    }

    public sealed class VRStore
    {
        private readonly Dictionary<
            string,
            VRStoreItem> items =
            new Dictionary<
                string,
                VRStoreItem>(
                StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string>
            purchasedItems =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public VRStoreArea CurrentArea { get; private set; }

        public int ItemCount =>
            items.Count;

        public int PurchasedItemCount =>
            purchasedItems.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            items.Clear();
            purchasedItems.Clear();

            CurrentArea =
                VRStoreArea.Entrance;

            Initialized = true;

            return true;
        }

        public bool EnterArea(
            VRStoreArea area)
        {
            if (!Initialized)
            {
                return false;
            }

            CurrentArea = area;

            return true;
        }

        public bool RegisterItem(
            string itemId,
            string displayName,
            int creditCost)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId) ||
                string.IsNullOrWhiteSpace(displayName) ||
                creditCost < 0)
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
                new VRStoreItem(
                    id,
                    displayName.Trim(),
                    creditCost));

            return true;
        }

        public bool SetItemAvailable(
            string itemId,
            bool available)
        {
            VRStoreItem item =
                GetItem(itemId);

            return item != null &&
                   item.SetAvailable(available);
        }

        public bool PurchaseItem(
            string itemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return false;
            }

            VRStoreItem item =
                GetItem(itemId);

            if (item == null ||
                !item.Available)
            {
                return false;
            }

            return purchasedItems.Add(
                item.ItemId);
        }

        public bool HasPurchased(
            string itemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return false;
            }

            return purchasedItems.Contains(
                itemId.Trim());
        }

        public VRStoreItem GetItem(
            string itemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return null;
            }

            items.TryGetValue(
                itemId.Trim(),
                out VRStoreItem item);

            return item;
        }

        public IReadOnlyCollection<VRStoreItem>
            GetItems()
        {
            return items.Values;
        }

        public IReadOnlyCollection<string>
            GetPurchasedItems()
        {
            return purchasedItems;
        }

        public void Reset()
        {
            items.Clear();
            purchasedItems.Clear();

            Initialized = false;

            CurrentArea =
                VRStoreArea.Entrance;
        }
    }
}
