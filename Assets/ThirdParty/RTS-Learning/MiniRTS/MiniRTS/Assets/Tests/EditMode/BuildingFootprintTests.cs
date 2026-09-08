using NUnit.Framework;
using UnityEngine;

namespace MiniRTS.Tests.EditMode
{
    public sealed class BuildingFootprintTests
    {
        [Test]
        public void SetWalkable_BlocksEveryCellUnderTheBuilding()
        {
            WalkGrid grid = new WalkGrid(12, 12, 1f);
            Vector2Int footprint = new Vector2Int(4, 3);
            Vector3 center = BuildingFootprint.SnapToGrid(
                grid,
                new Vector3(5f, 0f, 5.5f),
                footprint);

            Assert.That(
                BuildingFootprint.SetWalkable(grid, center, footprint, false),
                Is.True);

            Vector2Int minimum =
                BuildingFootprint.GetMinimumCell(grid, center, footprint);
            int blockedCount = 0;
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (!grid.IsWalkable(x, y))
                    {
                        blockedCount++;
                    }
                }
            }

            Assert.That(blockedCount, Is.EqualTo(footprint.x * footprint.y));
            Assert.That(grid.IsWalkable(minimum), Is.False);
            Assert.That(
                grid.IsWalkable(
                    minimum.x + footprint.x - 1,
                    minimum.y + footprint.y - 1),
                Is.False);
            Assert.That(grid.IsWalkable(minimum.x - 1, minimum.y), Is.True);
        }

        [Test]
        public void SetWalkable_UnblocksEveryFootprintCellOnDestruction()
        {
            WalkGrid grid = new WalkGrid(12, 12, 1f);
            Vector2Int footprint = new Vector2Int(5, 4);
            Vector3 center = BuildingFootprint.SnapToGrid(
                grid,
                new Vector3(6f, 0f, 6f),
                footprint);
            BuildingFootprint.SetWalkable(grid, center, footprint, false);

            Assert.That(
                BuildingFootprint.SetWalkable(grid, center, footprint, true),
                Is.True);

            Vector2Int minimum =
                BuildingFootprint.GetMinimumCell(grid, center, footprint);
            for (int x = 0; x < footprint.x; x++)
            {
                for (int y = 0; y < footprint.y; y++)
                {
                    Assert.That(
                        grid.IsWalkable(minimum.x + x, minimum.y + y),
                        Is.True);
                }
            }
        }
    }
}
