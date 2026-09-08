using System;
using UnityEngine;

namespace MiniRTS
{
    [DisallowMultipleComponent]
    public sealed class Factory : Building
    {
        public ProductionQueue ProductionQueue { get; private set; }

        public void InitializeFactory(
            WalkGrid walkGrid,
            PlayerEconomy factionEconomy,
            int ownerId,
            Color factionColor,
            Func<UnitType, Vector3, Unit> spawnUnit,
            bool startsConstructed = false)
        {
            Initialize(
                walkGrid,
                factionEconomy,
                ownerId,
                factionColor,
                BuildingType.Factory,
                new Vector2Int(
                    BalanceConfig.FactoryFootprintWidth,
                    BalanceConfig.FactoryFootprintDepth),
                BalanceConfig.FactoryHitPoints,
                startsConstructed,
                BalanceConfig.FactoryBuildSeconds);
            ProductionQueue = gameObject.AddComponent<ProductionQueue>();
            ProductionQueue.Initialize(
                this,
                factionEconomy,
                new[] { UnitType.Tank },
                spawnUnit);
        }
    }
}
