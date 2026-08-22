using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public enum CosmeticType
    {
        Paint,
        Emblem,
        Decal,
        AlternateAppearance,
        UnitPersonalization
    }

    public sealed class CosmeticPurchase
    {
        public string PurchaseId { get; }
        public string PlayerId { get; }
        public string CosmeticId { get; }

        public CosmeticType Type { get; }

        public int CreditCost { get; }

        public PurchaseStatus Status
        {
            get;
            private set;
        }

        public CosmeticPurchase(
            string purchaseId,
            string playerId,
            string cosmeticId,
            CosmeticType type,
            int creditCost)
        {
            PurchaseId =
                purchaseId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            CosmeticId =
                cosmeticId ?? string.Empty;

            Type = type;

            CreditCost =
                Math.Max(0, creditCost);

            Status =
                PurchaseStatus.Pending;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PurchaseId) &&
            !string.IsNullOrWhiteSpace(PlayerId) &&
            !string.IsNullOrWhiteSpace(CosmeticId);

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

    public sealed class CosmeticPurchasing
    {
        private readonly Dictionary<
            string,
            CosmeticPurchase> purchases =
            new Dictionary<
                string,
                CosmeticPurchase>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            CosmeticPurchase purchase)
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
                    out CosmeticPurchase purchase))
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
                    out CosmeticPurchase purchase))
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
                    out CosmeticPurchase purchase))
            {
                return false;
            }

            purchase.Cancel();
            return true;
        }

        public bool TryGet(
            string purchaseId,
            out CosmeticPurchase purchase)
        {
            return purchases.TryGetValue(
                purchaseId,
                out purchase);
        }

        public IReadOnlyCollection<
            CosmeticPurchase>
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
