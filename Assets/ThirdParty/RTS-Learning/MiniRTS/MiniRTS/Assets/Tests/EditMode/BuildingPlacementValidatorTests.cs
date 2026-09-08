using NUnit.Framework;
using UnityEngine;

namespace MiniRTS.Tests.EditMode
{
    public sealed class BuildingPlacementValidatorTests
    {
        [Test]
        public void FootprintAvailability_RejectsOverlapAndOutOfBounds()
        {
            WalkGrid grid = new WalkGrid(12, 12, 1f);
            Vector2Int footprint = new Vector2Int(3, 3);
            Vector3 center = BuildingFootprint.SnapToGrid(
                grid,
                new Vector3(6f, 0f, 6f),
                footprint);

            Assert.That(
                BuildingPlacementValidator.IsFootprintAvailable(
                    grid,
                    center,
                    footprint),
                Is.True);

            Vector2Int minimum =
                BuildingFootprint.GetMinimumCell(grid, center, footprint);
            grid.SetWalkable(minimum.x + 1, minimum.y + 1, false);
            Assert.That(
                BuildingPlacementValidator.IsFootprintAvailable(
                    grid,
                    center,
                    footprint),
                Is.False);

            Vector3 outside = BuildingFootprint.SnapToGrid(
                grid,
                new Vector3(-1f, 0f, -1f),
                footprint);
            Assert.That(
                BuildingPlacementValidator.IsFootprintAvailable(
                    grid,
                    outside,
                    footprint),
                Is.False);
        }

        [Test]
        public void RefineryRule_AllowsOnlyAnUnclaimedGeyserAtItsSnappedCenter()
        {
            WalkGrid grid = new WalkGrid(12, 12, 1f);
            Vector2Int footprint = new Vector2Int(4, 4);
            Vector3 geyser = new Vector3(6f, 0f, 6f);
            grid.SetCircleWalkable(
                geyser,
                BalanceConfig.GeyserBlockingRadius,
                false);
            Vector3 center = BuildingFootprint.SnapToGrid(
                grid,
                geyser,
                footprint);

            Assert.That(
                BuildingPlacementValidator.IsFootprintAvailable(
                    grid,
                    center,
                    footprint),
                Is.False,
                "Ordinary buildings cannot overlap geyser cells.");
            Assert.That(
                BuildingPlacementValidator.IsRefineryPlacementValid(
                    grid,
                    center,
                    footprint,
                    true,
                    geyser,
                    false),
                Is.True);
            Assert.That(
                BuildingPlacementValidator.IsRefineryPlacementValid(
                    grid,
                    center,
                    footprint,
                    false,
                    geyser,
                    false),
                Is.False);
            Assert.That(
                BuildingPlacementValidator.IsRefineryPlacementValid(
                    grid,
                    center,
                    footprint,
                    true,
                    geyser,
                    true),
                Is.False);
            Assert.That(
                BuildingPlacementValidator.IsRefineryPlacementValid(
                    grid,
                    center + Vector3.right,
                    footprint,
                    true,
                    geyser,
                    false),
                Is.False);
        }

        [Test]
        public void RefineryRule_RejectsUnrelatedBlockedCellInsideFootprint()
        {
            WalkGrid grid = new WalkGrid(12, 12, 1f);
            Vector2Int footprint = new Vector2Int(4, 4);
            Vector3 geyser = new Vector3(6f, 0f, 6f);
            grid.SetCircleWalkable(
                geyser,
                BalanceConfig.GeyserBlockingRadius,
                false);
            grid.SetWalkable(4, 4, false);
            Vector3 center = BuildingFootprint.SnapToGrid(
                grid,
                geyser,
                footprint);

            Assert.That(
                BuildingPlacementValidator.IsRefineryPlacementValid(
                    grid,
                    center,
                    footprint,
                    true,
                    geyser,
                    false),
                Is.False);
        }
    }
}
