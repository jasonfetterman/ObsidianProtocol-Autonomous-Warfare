using System;

namespace ObsidianProtocol.Game.Logistics
{
    public sealed class ResourceLogisticsSystem
    {
        private readonly SupplyDepotSystem depotSystem;

        public ResourceLogisticsSystem(
            SupplyDepotSystem depotSystem)
        {
            this.depotSystem =
                depotSystem;
        }

        public bool Valid =>
            depotSystem != null;

        public bool StoreResources(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.StoreSupply(
                depotId,
                SupplyType.Resources,
                amount);
        }

        public bool WithdrawResources(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.WithdrawSupply(
                depotId,
                SupplyType.Resources,
                amount);
        }

        public bool HasResources(
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
                SupplyType.Resources,
                amount);
        }

        public float GetResources(
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
                SupplyType.Resources);
        }

        public bool ConsumeResources(
            string depotId,
            float amount)
        {
            return WithdrawResources(
                depotId,
                amount);
        }
    }
}
