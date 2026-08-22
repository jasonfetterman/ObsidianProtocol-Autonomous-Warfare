using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public sealed class CampaignResourcePurchase
    {
        public string PurchaseId { get; }
        public string PlayerId { get; }
        public string ResourceId { get; }

        public int Quantity { get; }
        public int CreditCost { get; }

        public PurchaseStatus Status
        {
            get;
            private set;
        }

        public CampaignResourcePurchase(
            string purchaseId,
            string playerId,
            string resourceId,
            int quantity,
            int creditCost)
        {
            PurchaseId =
                purchaseId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            ResourceId =
                resourceId ?? string.Empty;

            Quantity =
                Math.Max(1, quantity);

            CreditCost =
                Math.Max(0, creditCost);

            Status =
                PurchaseStatus.Pending;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PurchaseId) &&
            !string.IsNullOrWhiteSpace(PlayerId) &&
            !string.IsNullOrWhiteSpace(ResourceId) &&
            Quantity > 0;

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

    public sealed class CampaignResourcePurchasing
    {
        private readonly Dictionary<
            string,
            CampaignResourcePurchase> purchases =
            new Dictionary<
                string,
                CampaignResourcePurchase>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            CampaignResourcePurchase purchase)
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
                    out CampaignResourcePurchase purchase))
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
                    out CampaignResourcePurchase purchase))
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
                    out CampaignResourcePurchase purchase))
            {
                return false;
            }

            purchase.Cancel();
            return true;
        }

        public bool TryGet(
            string purchaseId,
            out CampaignResourcePurchase purchase)
        {
            return purchases.TryGetValue(
                purchaseId,
                out purchase);
        }

        public IReadOnlyCollection<
            CampaignResourcePurchase>
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
