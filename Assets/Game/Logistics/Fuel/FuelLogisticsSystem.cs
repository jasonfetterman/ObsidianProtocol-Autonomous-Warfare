using System;

namespace ObsidianProtocol.Game.Logistics
{
    public sealed class FuelLogisticsSystem
    {
        private readonly SupplyDepotSystem depotSystem;

        public FuelLogisticsSystem(
            SupplyDepotSystem depotSystem)
        {
            this.depotSystem =
                depotSystem;
        }

        public bool Valid =>
            depotSystem != null;

        public bool StoreFuel(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.StoreSupply(
                depotId,
                SupplyType.Fuel,
                amount);
        }

        public bool WithdrawFuel(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.WithdrawSupply(
                depotId,
                SupplyType.Fuel,
                amount);
        }

        public bool HasFuel(
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
                SupplyType.Fuel,
                amount);
        }

        public float GetFuel(
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
                SupplyType.Fuel);
        }
    }
}
