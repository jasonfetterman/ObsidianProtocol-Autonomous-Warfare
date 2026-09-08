using System;
using UnityEngine;

namespace MiniRTS
{
    [DisallowMultipleComponent]
    public sealed class Barracks : Building
    {
        public ProductionQueue ProductionQueue { get; private set; }

        public void InitializeBarracks(
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
                BuildingType.Barracks,
                new Vector2Int(
                    BalanceConfig.BarracksFootprintWidth,
                    BalanceConfig.BarracksFootprintDepth),
                BalanceConfig.BarracksHitPoints,
                startsConstructed,
                BalanceConfig.BarracksBuildSeconds);
            ProductionQueue = gameObject.AddComponent<ProductionQueue>();
            ProductionQueue.Initialize(
                this,
                factionEconomy,
                new[] { UnitType.Marine },
                spawnUnit);
        }
    }
}
