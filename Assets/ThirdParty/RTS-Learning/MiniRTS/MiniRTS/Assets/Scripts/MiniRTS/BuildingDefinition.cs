using System;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Immutable construction values sourced from BalanceConfig.
    /// </summary>
    public readonly struct BuildingDefinition
    {
        public BuildingType Type { get; }
        public string DisplayName { get; }
        public int MineralCost { get; }
        public int GasCost { get; }
        public int HitPoints { get; }
        public Vector2Int Footprint { get; }
        public float Height { get; }
        public float BuildSeconds { get; }
        public bool RequiresGeyser { get; }

        public BuildingDefinition(
            BuildingType type,
            string displayName,
            int mineralCost,
            int gasCost,
            int hitPoints,
            Vector2Int footprint,
            float height,
            float buildSeconds,
            bool requiresGeyser)
        {
            Type = type;
            DisplayName = displayName;
            MineralCost = mineralCost;
            GasCost = gasCost;
            HitPoints = hitPoints;
            Footprint = footprint;
            Height = height;
            BuildSeconds = buildSeconds;
            RequiresGeyser = requiresGeyser;
        }

        public static BuildingDefinition Get(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.Headquarters:
                    return new BuildingDefinition(
                        type,
                        "Headquarters",
                        BalanceConfig.HeadquartersMineralCost,
                        BalanceConfig.HeadquartersGasCost,
                        BalanceConfig.HeadquartersHitPoints,
                        new Vector2Int(
                            BalanceConfig.HeadquartersFootprintWidth,
                            BalanceConfig.HeadquartersFootprintDepth),
                        BalanceConfig.HeadquartersHeight,
                        0f,
                        false);
                case BuildingType.SupplyDepot:
                    return new BuildingDefinition(
                        type,
                        "Supply Depot",
                        BalanceConfig.SupplyDepotMineralCost,
                        BalanceConfig.SupplyDepotGasCost,
                        BalanceConfig.SupplyDepotHitPoints,
                        new Vector2Int(
                            BalanceConfig.SupplyDepotFootprintWidth,
                            BalanceConfig.SupplyDepotFootprintDepth),
                        BalanceConfig.SupplyDepotHeight,
                        BalanceConfig.SupplyDepotBuildSeconds,
                        false);
                case BuildingType.Barracks:
                    return new BuildingDefinition(
                        type,
                        "Barracks",
                        BalanceConfig.BarracksMineralCost,
                        BalanceConfig.BarracksGasCost,
                        BalanceConfig.BarracksHitPoints,
                        new Vector2Int(
                            BalanceConfig.BarracksFootprintWidth,
                            BalanceConfig.BarracksFootprintDepth),
                        BalanceConfig.BarracksHeight,
                        BalanceConfig.BarracksBuildSeconds,
                        false);
                case BuildingType.Factory:
                    return new BuildingDefinition(
                        type,
                        "Factory",
                        BalanceConfig.FactoryMineralCost,
                        BalanceConfig.FactoryGasCost,
                        BalanceConfig.FactoryHitPoints,
                        new Vector2Int(
                            BalanceConfig.FactoryFootprintWidth,
                            BalanceConfig.FactoryFootprintDepth),
                        BalanceConfig.FactoryHeight,
                        BalanceConfig.FactoryBuildSeconds,
                        false);
                case BuildingType.Refinery:
                    return new BuildingDefinition(
                        type,
                        "Refinery",
                        BalanceConfig.RefineryMineralCost,
                        BalanceConfig.RefineryGasCost,
                        BalanceConfig.RefineryHitPoints,
                        new Vector2Int(
                            BalanceConfig.RefineryFootprintWidth,
                            BalanceConfig.RefineryFootprintDepth),
                        BalanceConfig.RefineryHeight,
                        BalanceConfig.RefineryBuildSeconds,
                        true);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
