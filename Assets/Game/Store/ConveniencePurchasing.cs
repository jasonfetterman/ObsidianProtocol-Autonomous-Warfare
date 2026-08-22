using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public enum ConvenienceType
    {
        FasterManufacturing,
        RepairConvenience,
        AdditionalQueueCapacity,
        InventoryConvenience,
        GarageManagementConvenience
    }

    public sealed class ConveniencePurchase
    {
        public string PurchaseId { get; }
        public string PlayerId { get; }
        public string ConvenienceId { get; }

        public ConvenienceType Type { get; }

        public int CreditCost { get; }

        public PurchaseStatus Status
        {
            get;
            private set;
        }

        public ConveniencePurchase(
            string purchaseId,
            string playerId,
            string convenienceId,
            ConvenienceType type,
            int creditCost)
        {
            PurchaseId =
                purchaseId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            ConvenienceId =
                convenienceId ?? string.Empty;

            Type = type;

            CreditCost =
                Math.Max(0, creditCost);

            Status =
                PurchaseStatus.Pending;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PurchaseId) &&
            !string.IsNullOrWhiteSpace(PlayerId) &&
            !string.IsNullOrWhiteSpace(ConvenienceId);

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

    public sealed class ConveniencePurchasing
    {
        private readonly Dictionary<
            string,
            ConveniencePurchase> purchases =
            new Dictionary<
                string,
                ConveniencePurchase>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            ConveniencePurchase purchase)
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
                    out ConveniencePurchase purchase))
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
                    out ConveniencePurchase purchase))
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
                    out ConveniencePurchase purchase))
            {
                return false;
            }

            purchase.Cancel();
            return true;
        }

        public bool TryGet(
            string purchaseId,
            out ConveniencePurchase purchase)
        {
            return purchases.TryGetValue(
                purchaseId,
                out purchase);
        }

        public IReadOnlyCollection<
            ConveniencePurchase>
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
