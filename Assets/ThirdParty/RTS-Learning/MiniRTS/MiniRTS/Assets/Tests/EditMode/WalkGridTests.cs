using NUnit.Framework;
using UnityEngine;

namespace MiniRTS.Tests.EditMode
{
    public sealed class WalkGridTests
    {
        [Test]
        public void Bounds_AreInclusiveAtZeroAndExclusiveAtDimensions()
        {
            WalkGrid grid = new WalkGrid(4, 3, 1f);

            Assert.That(grid.IsInBounds(0, 0), Is.True);
            Assert.That(grid.IsInBounds(3, 2), Is.True);
            Assert.That(grid.IsInBounds(-1, 0), Is.False);
            Assert.That(grid.IsInBounds(0, -1), Is.False);
            Assert.That(grid.IsInBounds(4, 2), Is.False);
            Assert.That(grid.IsInBounds(3, 3), Is.False);
            Assert.That(grid.IsWalkable(-1, 0), Is.False);
            Assert.That(grid.SetWalkable(4, 0, false), Is.False);
        }

        [Test]
        public void WorldConversion_UsesCellCentersAndRejectsOuterEdge()
        {
            WalkGrid grid = new WalkGrid(
                4,
                3,
                1f,
                new Vector3(-2f, 0f, -1.5f));

            Assert.That(grid.WorldToCell(new Vector3(-1.5f, 0f, -1f)),
                Is.EqualTo(new Vector2Int(0, 0)));
            Assert.That(grid.CellToWorld(3, 2),
                Is.EqualTo(new Vector3(1.5f, 0f, 1f)));
            Assert.That(grid.TryWorldToCell(new Vector3(2f, 0f, 0f), out _), Is.False);
        }
    }
}
