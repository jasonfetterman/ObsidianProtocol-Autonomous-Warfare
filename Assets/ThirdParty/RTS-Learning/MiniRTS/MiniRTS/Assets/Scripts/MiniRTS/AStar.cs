using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Deterministic eight-direction A* with diagonal corner-cut prevention.
    /// </summary>
    public static class AStar
    {
        private const int StraightCost = 10;
        private const int DiagonalCost = 14;

        private static readonly Vector2Int[] Directions =
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 1)
        };

        public static List<Vector2Int> FindPath(
            WalkGrid grid,
            Vector2Int start,
            Vector2Int goal)
        {
            if (grid == null)
            {
                throw new System.ArgumentNullException(nameof(grid));
            }

            if (!grid.IsWalkable(start) || !grid.IsWalkable(goal))
            {
                return null;
            }

            int width = grid.Width;
            int nodeCount = width * grid.Height;
            int[] gCosts = new int[nodeCount];
            int[] parents = new int[nodeCount];
            bool[] closed = new bool[nodeCount];

            for (int i = 0; i < nodeCount; i++)
            {
                gCosts[i] = int.MaxValue;
                parents[i] = -1;
            }

            int startIndex = ToIndex(start.x, start.y, width);
            int goalIndex = ToIndex(goal.x, goal.y, width);
            MinHeap open = new MinHeap();
            gCosts[startIndex] = 0;
            int startHeuristic = Heuristic(start, goal);
            open.Push(new HeapEntry(startIndex, startHeuristic, startHeuristic));

            while (open.Count > 0)
            {
                HeapEntry currentEntry = open.Pop();
                int currentIndex = currentEntry.Index;
                if (closed[currentIndex])
                {
                    continue;
                }

                if (currentIndex == goalIndex)
                {
                    return BuildPath(parents, currentIndex, width);
                }

                closed[currentIndex] = true;
                Vector2Int current = FromIndex(currentIndex, width);

                for (int i = 0; i < Directions.Length; i++)
                {
                    Vector2Int direction = Directions[i];
                    Vector2Int neighbor = current + direction;
                    if (!grid.IsWalkable(neighbor))
                    {
                        continue;
                    }

                    bool diagonal = direction.x != 0 && direction.y != 0;
                    if (diagonal &&
                        (!grid.IsWalkable(current.x + direction.x, current.y) ||
                         !grid.IsWalkable(current.x, current.y + direction.y)))
                    {
                        continue;
                    }

                    int neighborIndex = ToIndex(neighbor.x, neighbor.y, width);
                    if (closed[neighborIndex])
                    {
                        continue;
                    }

                    int stepCost = diagonal ? DiagonalCost : StraightCost;
                    int candidateCost = gCosts[currentIndex] + stepCost;
                    if (candidateCost >= gCosts[neighborIndex])
                    {
                        continue;
                    }

                    gCosts[neighborIndex] = candidateCost;
                    parents[neighborIndex] = currentIndex;
                    int heuristic = Heuristic(neighbor, goal);
                    open.Push(new HeapEntry(
                        neighborIndex,
                        candidateCost + heuristic,
                        heuristic));
                }
            }

            return null;
        }

        public static List<Vector3> FindWorldPath(
            WalkGrid grid,
            Vector3 start,
            Vector3 goal)
        {
            List<Vector2Int> cellPath = FindPath(
                grid,
                grid.WorldToCell(start),
                grid.WorldToCell(goal));
            if (cellPath == null)
            {
                return null;
            }

            List<Vector3> worldPath = new List<Vector3>(cellPath.Count);
            for (int i = 0; i < cellPath.Count; i++)
            {
                worldPath.Add(grid.CellToWorld(cellPath[i]));
            }

            return worldPath;
        }

        private static int Heuristic(Vector2Int from, Vector2Int to)
        {
            int deltaX = Mathf.Abs(from.x - to.x);
            int deltaY = Mathf.Abs(from.y - to.y);
            int diagonal = Mathf.Min(deltaX, deltaY);
            int straight = Mathf.Max(deltaX, deltaY) - diagonal;
            return diagonal * DiagonalCost + straight * StraightCost;
        }

        private static List<Vector2Int> BuildPath(int[] parents, int currentIndex, int width)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            while (currentIndex >= 0)
            {
                path.Add(FromIndex(currentIndex, width));
                currentIndex = parents[currentIndex];
            }

            path.Reverse();
            return path;
        }

        private static int ToIndex(int x, int y, int width)
        {
            return y * width + x;
        }

        private static Vector2Int FromIndex(int index, int width)
        {
            return new Vector2Int(index % width, index / width);
        }

        private readonly struct HeapEntry
        {
            public readonly int Index;
            public readonly int TotalCost;
            public readonly int Heuristic;

            public HeapEntry(int index, int totalCost, int heuristic)
            {
                Index = index;
                TotalCost = totalCost;
                Heuristic = heuristic;
            }
        }

        private sealed class MinHeap
        {
            private readonly List<HeapEntry> entries = new List<HeapEntry>();

            public int Count => entries.Count;

            public void Push(HeapEntry entry)
            {
                entries.Add(entry);
                int index = entries.Count - 1;
                while (index > 0)
                {
                    int parent = (index - 1) / 2;
                    if (!ComesBefore(entries[index], entries[parent]))
                    {
                        break;
                    }

                    Swap(index, parent);
                    index = parent;
                }
            }

            public HeapEntry Pop()
            {
                HeapEntry result = entries[0];
                int lastIndex = entries.Count - 1;
                entries[0] = entries[lastIndex];
                entries.RemoveAt(lastIndex);

                int index = 0;
                while (index < entries.Count)
                {
                    int left = index * 2 + 1;
                    int right = left + 1;
                    if (left >= entries.Count)
                    {
                        break;
                    }

                    int bestChild = right < entries.Count &&
                                    ComesBefore(entries[right], entries[left])
                        ? right
                        : left;
                    if (!ComesBefore(entries[bestChild], entries[index]))
                    {
                        break;
                    }

                    Swap(index, bestChild);
                    index = bestChild;
                }

                return result;
            }

            private static bool ComesBefore(HeapEntry left, HeapEntry right)
            {
                if (left.TotalCost != right.TotalCost)
                {
                    return left.TotalCost < right.TotalCost;
                }

                if (left.Heuristic != right.Heuristic)
                {
                    return left.Heuristic < right.Heuristic;
                }

                return left.Index < right.Index;
            }

            private void Swap(int left, int right)
            {
                HeapEntry temporary = entries[left];
                entries[left] = entries[right];
                entries[right] = temporary;
            }
        }
    }
}
