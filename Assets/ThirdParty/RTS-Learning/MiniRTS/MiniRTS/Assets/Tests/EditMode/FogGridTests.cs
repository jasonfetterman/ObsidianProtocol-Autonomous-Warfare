using NUnit.Framework;
using UnityEngine;

namespace MiniRTS.Tests.EditMode
{
    public sealed class FogGridTests
    {
        [Test]
        public void VisibilityPass_PreservesDiscoveredHistory()
        {
            FogGrid grid = new FogGrid(8, 8, 1f, Vector3.zero);
            Vector2Int cell = new Vector2Int(3, 4);

            Assert.That(
                grid.GetState(cell),
                Is.EqualTo(FogState.Undiscovered));

            grid.BeginVisibilityUpdate();
            grid.StampCircle(cell, 0);
            Assert.That(
                grid.GetState(cell),
                Is.EqualTo(FogState.Visible));

            grid.BeginVisibilityUpdate();
            Assert.That(
                grid.GetState(cell),
                Is.EqualTo(FogState.Discovered));

            grid.StampCircle(cell, 0);
            Assert.That(
                grid.GetState(cell),
                Is.EqualTo(FogState.Visible));
        }

        [Test]
        public void CellStamp_UsesInclusiveEuclideanRadius()
        {
            FogGrid grid = new FogGrid(9, 9, 1f, Vector3.zero);
            Vector2Int center = new Vector2Int(4, 4);

            grid.BeginVisibilityUpdate();
            grid.StampCircle(center, 2);

            Assert.That(grid.GetState(4, 4), Is.EqualTo(FogState.Visible));
            Assert.That(grid.GetState(6, 4), Is.EqualTo(FogState.Visible));
            Assert.That(grid.GetState(5, 5), Is.EqualTo(FogState.Visible));
            Assert.That(grid.GetState(6, 6), Is.EqualTo(FogState.Undiscovered));

            int visibleCount = 0;
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid.GetState(x, y) == FogState.Visible)
                    {
                        visibleCount++;
                    }
                }
            }

            Assert.That(visibleCount, Is.EqualTo(13));
        }

        [Test]
        public void WorldStamp_UsesCellCentersAndClipsAtGridBounds()
        {
            FogGrid grid = new FogGrid(
                9,
                9,
                1f,
                new Vector3(-4.5f, 0f, -4.5f));

            grid.BeginVisibilityUpdate();
            grid.StampCircle(Vector3.zero, 1.01f);

            Assert.That(grid.GetState(4, 4), Is.EqualTo(FogState.Visible));
            Assert.That(grid.GetState(3, 4), Is.EqualTo(FogState.Visible));
            Assert.That(grid.GetState(4, 3), Is.EqualTo(FogState.Visible));
            Assert.That(grid.GetState(3, 3), Is.EqualTo(FogState.Undiscovered));

            Assert.DoesNotThrow(
                () => grid.StampCircle(new Vector2Int(0, 0), 3));
            Assert.That(grid.GetState(0, 0), Is.EqualTo(FogState.Visible));
        }

        [Test]
        public void EnemyVisibilityAndBuildingMemory_RespectFogState()
        {
            Assert.That(
                FogVisibilityRules.CanSeeEntity(0, 0, FogState.Undiscovered),
                Is.True);
            Assert.That(
                FogVisibilityRules.CanSeeEntity(0, 1, FogState.Visible),
                Is.True);
            Assert.That(
                FogVisibilityRules.CanSeeEntity(0, 1, FogState.Discovered),
                Is.False);
            Assert.That(
                FogVisibilityRules.CanSeeEntity(
                    1,
                    0,
                    FogState.Undiscovered,
                    true),
                Is.True);

            Assert.That(
                FogVisibilityRules.ShouldShowLastKnownBuilding(
                    0,
                    1,
                    true,
                    FogState.Discovered),
                Is.True);
            Assert.That(
                FogVisibilityRules.ShouldShowLastKnownBuilding(
                    0,
                    1,
                    false,
                    FogState.Discovered),
                Is.False);
            Assert.That(
                FogVisibilityRules.ShouldShowLastKnownBuilding(
                    0,
                    1,
                    true,
                    FogState.Visible),
                Is.False);
        }
    }
}
