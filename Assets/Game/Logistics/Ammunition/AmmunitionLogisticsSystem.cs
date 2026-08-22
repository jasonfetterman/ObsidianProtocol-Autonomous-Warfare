using System;

namespace ObsidianProtocol.Game.Logistics
{
    public sealed class AmmunitionLogisticsSystem
    {
        private readonly SupplyDepotSystem depotSystem;

        public AmmunitionLogisticsSystem(
            SupplyDepotSystem depotSystem)
        {
            this.depotSystem =
                depotSystem;
        }

        public bool Valid =>
            depotSystem != null;

        public bool StoreAmmunition(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.StoreSupply(
                depotId,
                SupplyType.Ammunition,
                amount);
        }

        public bool WithdrawAmmunition(
            string depotId,
            float amount)
        {
            if (!Valid)
            {
                return false;
            }

            return depotSystem.WithdrawSupply(
                depotId,
                SupplyType.Ammunition,
                amount);
        }

        public bool HasAmmunition(
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
                SupplyType.Ammunition,
                amount);
        }

        public float GetAmmunition(
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
                SupplyType.Ammunition);
        }
    }
}
