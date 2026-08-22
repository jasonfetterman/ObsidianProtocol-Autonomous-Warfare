using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public sealed class OwnedItem
    {
        public string OwnershipId { get; }
        public string PlayerId { get; }
        public string ItemId { get; }

        public int Quantity { get; private set; }

        public OwnedItem(
            string ownershipId,
            string playerId,
            string itemId,
            int quantity)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            ItemId =
                itemId ?? string.Empty;

            Quantity =
                Math.Max(1, quantity);
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(OwnershipId) &&
            !string.IsNullOrWhiteSpace(PlayerId) &&
            !string.IsNullOrWhiteSpace(ItemId);

        public void AddQuantity(
            int amount)
        {
            if (amount <= 0)
                return;

            if (Quantity > int.MaxValue - amount)
                Quantity = int.MaxValue;
            else
                Quantity += amount;
        }

        public bool RemoveQuantity(
            int amount)
        {
            if (amount <= 0 ||
                amount > Quantity)
            {
                return false;
            }

            Quantity -= amount;
            return Quantity > 0;
        }
    }

    public sealed class OwnershipPersistence
    {
        private readonly Dictionary<
            string,
            OwnedItem> ownedItems =
            new Dictionary<
                string,
                OwnedItem>(
                StringComparer.OrdinalIgnoreCase);

        public int OwnedItemCount =>
            ownedItems.Count;

        public bool Register(
            OwnedItem item)
        {
            if (item == null ||
                !item.Valid ||
                ownedItems.ContainsKey(
                    item.OwnershipId))
            {
                return false;
            }

            ownedItems.Add(
                item.OwnershipId,
                item);

            return true;
        }

        public bool Remove(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            return ownedItems.Remove(
                ownershipId);
        }

        public bool TryGet(
            string ownershipId,
            out OwnedItem item)
        {
            return ownedItems.TryGetValue(
                ownershipId,
                out item);
        }

        public bool AddQuantity(
            string ownershipId,
            int amount)
        {
            if (!ownedItems.TryGetValue(
                    ownershipId,
                    out OwnedItem item))
            {
                return false;
            }

            item.AddQuantity(amount);
            return true;
        }

        public bool RemoveQuantity(
            string ownershipId,
            int amount)
        {
            if (!ownedItems.TryGetValue(
                    ownershipId,
                    out OwnedItem item))
            {
                return false;
            }

            return item.RemoveQuantity(amount);
        }

        public bool PlayerOwnsItem(
            string playerId,
            string itemId)
        {
            if (string.IsNullOrWhiteSpace(playerId) ||
                string.IsNullOrWhiteSpace(itemId))
            {
                return false;
            }

            foreach (OwnedItem item in ownedItems.Values)
            {
                if (string.Equals(
                        item.PlayerId,
                        playerId,
                        StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(
                        item.ItemId,
                        itemId,
                        StringComparison.OrdinalIgnoreCase) &&
                    item.Quantity > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyCollection<
            OwnedItem>
            GetOwnedItems()
        {
            return ownedItems.Values;
        }

        public void Clear()
        {
            ownedItems.Clear();
        }
    }
}
