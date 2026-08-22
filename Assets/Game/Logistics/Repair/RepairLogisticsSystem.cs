using System;

namespace ObsidianProtocol.Game.Logistics
{
    public sealed class RepairLogisticsSystem
    {
        private readonly SupplyDepotSystem depotSystem;

        public RepairLogisticsSystem(
            SupplyDepotSystem depotSystem)
        {
            this.depotSystem =
                depotSystem;
        }

        public bool Valid =>
            depotSystem != null;

        public bool StoreSpareParts(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.StoreSupply(
                depotId,
                SupplyType.SpareParts,
                amount);
        }

        public bool WithdrawSpareParts(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.WithdrawSupply(
                depotId,
                SupplyType.SpareParts,
                amount);
        }

        public bool HasRepairSupplies(
            string depotId,
            float amount)
        {
            if (!Valid ||
                amount <= 0f)
            {
                return false;
            }

            if (!depotSystem.TryGetDepot(
                    depotId,
                    out SupplyDepot depot))
            {
                return false;
            }

            return depot.HasSupply(
                SupplyType.SpareParts,
                amount);
        }

        public float GetSpareParts(
            string depotId)
        {
            if (!Valid)
            {
                return 0f;
            }

            if (!depotSystem.TryGetDepot(
                    depotId,
                    out SupplyDepot depot))
            {
                return 0f;
            }

            return depot.GetAmount(
                SupplyType.SpareParts);
        }

        public bool ConsumeRepairSupplies(
            string depotId,
            float amount)
        {
            return WithdrawSpareParts(
                depotId,
                amount);
        }
    }
}
