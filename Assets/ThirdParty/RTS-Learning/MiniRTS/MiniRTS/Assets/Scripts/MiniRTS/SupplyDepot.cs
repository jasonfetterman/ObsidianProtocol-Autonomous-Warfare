using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Economy building that contributes eight supply, capped by the faction maximum.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SupplyDepot : Building
    {
        private bool suppliedEconomy;

        public void InitializeSupplyDepot(
            WalkGrid walkGrid,
            PlayerEconomy factionEconomy,
            int ownerId,
            Color factionColor,
            bool startsConstructed = true)
        {
            Initialize(
                walkGrid,
                factionEconomy,
                ownerId,
                factionColor,
                BuildingType.SupplyDepot,
                new Vector2Int(
                    BalanceConfig.SupplyDepotFootprintWidth,
                    BalanceConfig.SupplyDepotFootprintDepth),
                BalanceConfig.SupplyDepotHitPoints,
                startsConstructed,
                BalanceConfig.SupplyDepotBuildSeconds);
        }

        protected override void OnDestroy()
        {
            if (suppliedEconomy && Economy != null)
            {
                Economy.RemoveSupplyCap(BalanceConfig.SupplyDepotSupply);
            }

            base.OnDestroy();
        }

        protected override void OnConstructionCompleted()
        {
            if (!suppliedEconomy && Economy != null)
            {
                Economy.AddSupplyCap(BalanceConfig.SupplyDepotSupply);
                suppliedEconomy = true;
            }
        }
    }
}
