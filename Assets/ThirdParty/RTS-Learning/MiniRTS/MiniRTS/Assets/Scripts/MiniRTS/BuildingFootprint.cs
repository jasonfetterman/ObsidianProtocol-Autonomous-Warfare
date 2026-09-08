using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Shared grid-snap and rectangular occupancy rules for buildings.
    /// </summary>
    public static class BuildingFootprint
    {
        public static Vector3 SnapToGrid(
            WalkGrid grid,
            Vector3 desiredCenter,
            Vector2Int footprint)
        {
            Validate(grid, footprint);
            float xOffset = footprint.x % 2 == 0 ? 0f : 0.5f;
            float zOffset = footprint.y % 2 == 0 ? 0f : 0.5f;
            float xIndex = Mathf.Round(
                (desiredCenter.x - grid.Origin.x) / grid.CellSize - xOffset);
            float zIndex = Mathf.Round(
                (desiredCenter.z - grid.Origin.z) / grid.CellSize - zOffset);

            return new Vector3(
                grid.Origin.x + (xIndex + xOffset) * grid.CellSize,
                desiredCenter.y,
                grid.Origin.z + (zIndex + zOffset) * grid.CellSize);
        }

        public static Vector2Int GetMinimumCell(
            WalkGrid grid,
            Vector3 snappedCenter,
            Vector2Int footprint)
        {
            Validate(grid, footprint);
            Vector3 minimumCenter = new Vector3(
                snappedCenter.x - (footprint.x - 1) * grid.CellSize * 0.5f,
                grid.Origin.y,
                snappedCenter.z - (footprint.y - 1) * grid.CellSize * 0.5f);
            return grid.WorldToCell(minimumCenter);
        }

        public static bool SetWalkable(
            WalkGrid grid,
            Vector3 snappedCenter,
            Vector2Int footprint,
            bool walkable)
        {
            Vector2Int minimum = GetMinimumCell(grid, snappedCenter, footprint);
            bool allCellsWereInBounds = true;

            for (int x = 0; x < footprint.x; x++)
            {
                for (int y = 0; y < footprint.y; y++)
                {
                    if (!grid.SetWalkable(minimum.x + x, minimum.y + y, walkable))
                    {
                        allCellsWereInBounds = false;
                    }
                }
            }

            return allCellsWereInBounds;
        }

        private static void Validate(WalkGrid grid, Vector2Int footprint)
        {
            if (grid == null)
            {
                throw new System.ArgumentNullException(nameof(grid));
            }

            if (footprint.x <= 0 || footprint.y <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(footprint));
            }
        }
    }
}
