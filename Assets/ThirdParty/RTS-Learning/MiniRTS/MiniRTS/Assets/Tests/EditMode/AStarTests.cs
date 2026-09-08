using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace MiniRTS.Tests.EditMode
{
    public sealed class AStarTests
    {
        [Test]
        public void FindPath_RoutesAroundObstacleWall()
        {
            WalkGrid grid = new WalkGrid(7, 7, 1f);
            for (int y = 0; y < grid.Height; y++)
            {
                if (y != 5)
                {
                    grid.SetWalkable(3, y, false);
                }
            }

            Vector2Int start = new Vector2Int(1, 1);
            Vector2Int goal = new Vector2Int(5, 1);
            List<Vector2Int> path = AStar.FindPath(grid, start, goal);

            Assert.That(path, Is.Not.Null);
            Assert.That(path[0], Is.EqualTo(start));
            Assert.That(path[path.Count - 1], Is.EqualTo(goal));
            Assert.That(path, Does.Contain(new Vector2Int(3, 5)));
            Assert.That(path.TrueForAll(grid.IsWalkable), Is.True);
        }

        [Test]
        public void FindPath_ReturnsNullWhenGoalIsSeparatedByBlockedCells()
        {
            WalkGrid grid = new WalkGrid(5, 5, 1f);
            for (int y = 0; y < grid.Height; y++)
            {
                grid.SetWalkable(2, y, false);
            }

            List<Vector2Int> path = AStar.FindPath(
                grid,
                new Vector2Int(0, 2),
                new Vector2Int(4, 2));

            Assert.That(path, Is.Null);
        }
    }
}
