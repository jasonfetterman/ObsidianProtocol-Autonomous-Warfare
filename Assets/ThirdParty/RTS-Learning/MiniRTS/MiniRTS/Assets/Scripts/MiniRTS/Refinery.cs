using UnityEngine;

namespace MiniRTS
{
    [DisallowMultipleComponent]
    public sealed class Refinery : Building
    {
        public ResourceNode Geyser { get; private set; }

        public void InitializeRefinery(
            WalkGrid walkGrid,
            PlayerEconomy factionEconomy,
            int ownerId,
            Color factionColor,
            ResourceNode geyser,
            bool startsConstructed = false)
        {
            if (geyser == null ||
                geyser.Type != ResourceType.Gas ||
                !geyser.RequiresRefinery)
            {
                throw new System.ArgumentException(
                    "A refinery requires a gas geyser.",
                    nameof(geyser));
            }

            Geyser = geyser;
            Initialize(
                walkGrid,
                factionEconomy,
                ownerId,
                factionColor,
                BuildingType.Refinery,
                new Vector2Int(
                    BalanceConfig.RefineryFootprintWidth,
                    BalanceConfig.RefineryFootprintDepth),
                BalanceConfig.RefineryHitPoints,
                startsConstructed,
                BalanceConfig.RefineryBuildSeconds);
        }

        public static bool IsGeyserClaimed(ResourceNode geyser)
        {
            if (geyser == null)
            {
                return false;
            }

            System.Collections.Generic.IReadOnlyList<Building> buildings =
                ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Refinery refinery = buildings[i] as Refinery;
                if (refinery != null && refinery.Geyser == geyser)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnConstructionCompleted()
        {
            if (Geyser != null)
            {
                Geyser.SetRefineryPresent(true);
            }
        }

        protected override void OnDestroy()
        {
            if (Geyser != null)
            {
                Geyser.SetRefineryPresent(false);
            }

            base.OnDestroy();
            if (Grid != null && Geyser != null)
            {
                Grid.SetCircleWalkable(
                    Geyser.transform.position,
                    BalanceConfig.GeyserBlockingRadius,
                    false);
            }
        }
    }
}
