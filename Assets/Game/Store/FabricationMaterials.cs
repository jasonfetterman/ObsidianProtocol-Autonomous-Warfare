using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public sealed class FabricationMaterial
    {
        public string MaterialId { get; }
        public string DisplayName { get; }

        public int CreditCost { get; }

        public FabricationMaterial(
            string materialId,
            string displayName,
            int creditCost)
        {
            MaterialId =
                materialId ?? string.Empty;

            DisplayName =
                displayName ?? string.Empty;

            CreditCost =
                Math.Max(0, creditCost);
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                MaterialId) &&
            !string.IsNullOrWhiteSpace(
                DisplayName);
    }

    public sealed class FabricationMaterialPurchase
    {
        public string PurchaseId { get; }
        public string PlayerId { get; }
        public string MaterialId { get; }

        public int Quantity { get; }
        public int CreditCost { get; }

        public PurchaseStatus Status
        {
            get;
            private set;
        }

        public FabricationMaterialPurchase(
            string purchaseId,
            string playerId,
            string materialId,
            int quantity,
            int creditCost)
        {
            PurchaseId =
                purchaseId ?? string.Empty;

            PlayerId =
                playerId ?? string.Empty;

            MaterialId =
                materialId ?? string.Empty;

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
            !string.IsNullOrWhiteSpace(MaterialId);

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

    public sealed class FabricationMaterials
    {
        private readonly Dictionary<
            string,
            FabricationMaterial> materials =
            new Dictionary<
                string,
                FabricationMaterial>(
                StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<
            string,
            FabricationMaterialPurchase> purchases =
            new Dictionary<
                string,
                FabricationMaterialPurchase>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterMaterial(
            FabricationMaterial material)
        {
            if (material == null ||
                !material.Valid ||
                materials.ContainsKey(
                    material.MaterialId))
            {
                return false;
            }

            materials.Add(
                material.MaterialId,
                material);

            return true;
        }

        public bool RemoveMaterial(
            string materialId)
        {
            if (string.IsNullOrWhiteSpace(
                    materialId))
            {
                return false;
            }

            return materials.Remove(materialId);
        }

        public bool TryGetMaterial(
            string materialId,
            out FabricationMaterial material)
        {
            return materials.TryGetValue(
                materialId,
                out material);
        }

        public bool RegisterPurchase(
            FabricationMaterialPurchase purchase)
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

        public bool CompletePurchase(
            string purchaseId)
        {
            if (!purchases.TryGetValue(
                    purchaseId,
                    out FabricationMaterialPurchase purchase))
            {
                return false;
            }

            purchase.Complete();
            return true;
        }

        public bool RejectPurchase(
            string purchaseId)
        {
            if (!purchases.TryGetValue(
                    purchaseId,
                    out FabricationMaterialPurchase purchase))
            {
                return false;
            }

            purchase.Reject();
            return true;
        }

        public bool CancelPurchase(
            string purchaseId)
        {
            if (!purchases.TryGetValue(
                    purchaseId,
                    out FabricationMaterialPurchase purchase))
            {
                return false;
            }

            purchase.Cancel();
            return true;
        }

        public IReadOnlyCollection<
            FabricationMaterial>
            GetMaterials()
        {
            return materials.Values;
        }

        public IReadOnlyCollection<
            FabricationMaterialPurchase>
            GetPurchases()
        {
            return purchases.Values;
        }

        public void Clear()
        {
            materials.Clear();
            purchases.Clear();
        }
    }
}
