using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public sealed class GarageSlotPurchase
    {
        public string PurchaseId { get; }
        public string PlayerId { get; }

        public int SlotCount { get; }
        public int CreditCost { get; }

        public PurchaseStatus Status
        {
            get;
            private set;
        }

        public GarageSlotPurchase(
            string purchaseId,
            string playerId,
            int slotCount,
            int creditCost)
        {
            PurchaseId =
                purchaseId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            SlotCount =
                Math.Max(1, slotCount);

            CreditCost =
                Math.Max(0, creditCost);

            Status =
                PurchaseStatus.Pending;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PurchaseId) &&
            !string.IsNullOrWhiteSpace(PlayerId) &&
            SlotCount > 0;

        public void Complete()
        {
            if (Status == PurchaseStatus.Pending)
                Status = PurchaseStatus.Completed;
        }

        public void Reject()
        {
            if (Status == PurchaseStatus.Pending)
                Status = PurchaseStatus.Rejected;
        }

        public void Cancel()
        {
            if (Status == PurchaseStatus.Pending)
                Status = PurchaseStatus.Cancelled;
        }
    }

    public sealed class GarageSlotPurchasing
    {
        private readonly Dictionary<
            string,
            GarageSlotPurchase> purchases =
            new Dictionary<
                string,
                GarageSlotPurchase>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            GarageSlotPurchase purchase)
        {
            if (purchase == null ||
                !purchase.Valid ||
                purchases.ContainsKey(
                    purchase.PurchaseId))
            {
                return false;
            }

            purchases.Add(
                purchase.PurchaseId,
                purchase);

            return true;
        }

        public bool Complete(
            string purchaseId)
        {
            if (!purchases.TryGetValue(
                    purchaseId,
                    out GarageSlotPurchase purchase))
            {
                return false;
            }

            purchase.Complete();
            return true;
        }

        public bool Reject(
            string purchaseId)
        {
            if (!purchases.TryGetValue(
                    purchaseId,
                    out GarageSlotPurchase purchase))
            {
                return false;
            }

            purchase.Reject();
            return true;
        }

        public bool Cancel(
            string purchaseId)
        {
            if (!purchases.TryGetValue(
                    purchaseId,
                    out GarageSlotPurchase purchase))
            {
                return false;
            }

            purchase.Cancel();
            return true;
        }

        public bool TryGet(
            string purchaseId,
            out GarageSlotPurchase purchase)
        {
            return purchases.TryGetValue(
                purchaseId,
                out purchase);
        }

        public IReadOnlyCollection<
            GarageSlotPurchase>
            GetPurchases()
        {
            return purchases.Values;
        }

        public void Clear()
        {
            purchases.Clear();
        }
    }
}
