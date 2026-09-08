using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Pure footprint validity checks shared by placement preview and EditMode tests.
    /// </summary>
    public static class BuildingPlacementValidator
    {
        public static bool IsFootprintAvailable(
            WalkGrid grid,
            Vector3 snappedCenter,
            Vector2Int footprint)
        {
            return IsFootprintAvailable(
                grid,
                snappedCenter,
                footprint,
                false,
                Vector3.zero,
                0f);
        }

        public static bool IsRefineryPlacementValid(
            WalkGrid grid,
            Vector3 snappedCenter,
            Vector2Int footprint,
            bool hasGeyser,
            Vector3 geyserPosition,
            bool geyserAlreadyClaimed)
        {
            if (!hasGeyser || geyserAlreadyClaimed)
            {
                return false;
            }

            Vector3 snappedGeyser = BuildingFootprint.SnapToGrid(
                grid,
                geyserPosition,
                footprint);
            Vector3 difference = snappedCenter - snappedGeyser;
            difference.y = 0f;
            if (difference.sqrMagnitude > 0.01f)
            {
                return false;
            }

            return IsFootprintAvailable(
                grid,
                snappedCenter,
                footprint,
                true,
                geyserPosition,
                BalanceConfig.GeyserBlockingRadius);
        }

        private static bool IsFootprintAvailable(
            WalkGrid grid,
            Vector3 snappedCenter,
            Vector2Int footprint,
            bool allowGeyserCells,
            Vector3 geyserPosition,
            float geyserRadius)
        {
            Vector2Int minimum = BuildingFootprint.GetMinimumCell(
                grid,
                snappedCenter,
                footprint);
            float allowedRadiusSquared = geyserRadius * geyserRadius + 0.001f;

            for (int x = 0; x < footprint.x; x++)
            {
                for (int y = 0; y < footprint.y; y++)
                {
                    Vector2Int cell = new Vector2Int(minimum.x + x, minimum.y + y);
                    if (!grid.IsInBounds(cell))
                    {
                        return false;
                    }

                    if (grid.IsWalkable(cell))
                    {
                        continue;
                    }

                    Vector3 cellCenter = grid.CellToWorld(cell);
                    float deltaX = cellCenter.x - geyserPosition.x;
                    float deltaZ = cellCenter.z - geyserPosition.z;
                    if (!allowGeyserCells ||
                        deltaX * deltaX + deltaZ * deltaZ > allowedRadiusSquared)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
