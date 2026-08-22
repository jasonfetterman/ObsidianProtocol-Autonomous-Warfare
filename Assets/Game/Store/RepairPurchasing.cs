using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public sealed class RepairPurchase
    {
        public string PurchaseId { get; }
        public string PlayerId { get; }
        public string OwnershipId { get; }

        public int CreditCost { get; }

        public PurchaseStatus Status
        {
            get;
            private set;
        }

        public RepairPurchase(
            string purchaseId,
            string playerId,
            string ownershipId,
            int creditCost)
        {
            PurchaseId =
                purchaseId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            OwnershipId =
                ownershipId ?? string.Empty;

            CreditCost =
                Math.Max(0, creditCost);

            Status =
                PurchaseStatus.Pending;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PurchaseId) &&
            !string.IsNullOrWhiteSpace(PlayerId) &&
            !string.IsNullOrWhiteSpace(OwnershipId);

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

    public sealed class RepairPurchasing
    {
        private readonly Dictionary<
            string,
            RepairPurchase> purchases =
            new Dictionary<
                string,
                RepairPurchase>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            RepairPurchase purchase)
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
                    out RepairPurchase purchase))
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
                    out RepairPurchase purchase))
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
                    out RepairPurchase purchase))
            {
                return false;
            }

            purchase.Cancel();
            return true;
        }

        public bool TryGet(
            string purchaseId,
            out RepairPurchase purchase)
        {
            return purchases.TryGetValue(
                purchaseId,
                out purchase);
        }

        public IReadOnlyCollection<
            RepairPurchase>
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
