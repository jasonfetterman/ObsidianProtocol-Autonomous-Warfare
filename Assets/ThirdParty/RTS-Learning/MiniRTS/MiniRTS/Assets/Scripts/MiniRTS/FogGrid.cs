using System;
using UnityEngine;

namespace MiniRTS
{
    public enum FogState : byte
    {
        Undiscovered,
        Discovered,
        Visible
    }

    /// <summary>
    /// Pure visibility history for a world-aligned XZ grid.
    /// </summary>
    public sealed class FogGrid
    {
        private readonly FogState[,] states;

        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }
        public Vector3 Origin { get; }

        public FogGrid(
            int width,
            int height,
            float cellSize,
            Vector3 origin)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            if (cellSize <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(cellSize));
            }

            Width = width;
            Height = height;
            CellSize = cellSize;
            Origin = origin;
            states = new FogState[width, height];
        }

        public FogGrid(WalkGrid walkGrid)
            : this(
                walkGrid != null
                    ? walkGrid.Width
                    : throw new ArgumentNullException(nameof(walkGrid)),
                walkGrid.Height,
                walkGrid.CellSize,
                walkGrid.Origin)
        {
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public FogState GetState(int x, int y)
        {
            return IsInBounds(x, y)
                ? states[x, y]
                : FogState.Undiscovered;
        }

        public FogState GetState(Vector2Int cell)
        {
            return GetState(cell.x, cell.y);
        }

        public FogState GetState(Vector3 worldPosition)
        {
            return GetState(WorldToCell(worldPosition));
        }

        public bool IsVisible(Vector3 worldPosition)
        {
            return GetState(worldPosition) == FogState.Visible;
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt((worldPosition.x - Origin.x) / CellSize),
                Mathf.FloorToInt((worldPosition.z - Origin.z) / CellSize));
        }

        public Vector3 CellToWorld(int x, int y)
        {
            return new Vector3(
                Origin.x + (x + 0.5f) * CellSize,
                Origin.y,
                Origin.z + (y + 0.5f) * CellSize);
        }

        /// <summary>
        /// Starts a new visibility pass while preserving exploration history.
        /// </summary>
        public void BeginVisibilityUpdate()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (states[x, y] == FogState.Visible)
                    {
                        states[x, y] = FogState.Discovered;
                    }
                }
            }
        }

        /// <summary>
        /// Stamps an inclusive Euclidean circle measured in grid cells.
        /// </summary>
        public void StampCircle(Vector2Int center, int radiusInCells)
        {
            if (radiusInCells < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radiusInCells));
            }

            int radiusSquared = radiusInCells * radiusInCells;
            for (int x = center.x - radiusInCells;
                 x <= center.x + radiusInCells;
                 x++)
            {
                for (int y = center.y - radiusInCells;
                     y <= center.y + radiusInCells;
                     y++)
                {
                    int deltaX = x - center.x;
                    int deltaY = y - center.y;
                    if (IsInBounds(x, y) &&
                        deltaX * deltaX + deltaY * deltaY <= radiusSquared)
                    {
                        states[x, y] = FogState.Visible;
                    }
                }
            }
        }

        /// <summary>
        /// Stamps cells whose centers fall within a world-space vision radius.
        /// </summary>
        public void StampCircle(Vector3 worldCenter, float worldRadius)
        {
            if (worldRadius < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(worldRadius));
            }

            if (Mathf.Approximately(worldRadius, 0f))
            {
                Vector2Int cell = WorldToCell(worldCenter);
                if (IsInBounds(cell.x, cell.y))
                {
                    states[cell.x, cell.y] = FogState.Visible;
                }

                return;
            }

            int minimumX = Mathf.FloorToInt(
                (worldCenter.x - worldRadius - Origin.x) / CellSize);
            int maximumX = Mathf.FloorToInt(
                (worldCenter.x + worldRadius - Origin.x) / CellSize);
            int minimumY = Mathf.FloorToInt(
                (worldCenter.z - worldRadius - Origin.z) / CellSize);
            int maximumY = Mathf.FloorToInt(
                (worldCenter.z + worldRadius - Origin.z) / CellSize);
            float radiusSquared = worldRadius * worldRadius;

            for (int x = minimumX; x <= maximumX; x++)
            {
                for (int y = minimumY; y <= maximumY; y++)
                {
                    if (!IsInBounds(x, y))
                    {
                        continue;
                    }

                    Vector3 cellCenter = CellToWorld(x, y);
                    float deltaX = cellCenter.x - worldCenter.x;
                    float deltaZ = cellCenter.z - worldCenter.z;
                    if (deltaX * deltaX + deltaZ * deltaZ <= radiusSquared)
                    {
                        states[x, y] = FogState.Visible;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Scene-independent rules for hiding enemies and showing building memory.
    /// </summary>
    public static class FogVisibilityRules
    {
        public static bool CanSeeEntity(
            int observerOwnerId,
            int entityOwnerId,
            FogState entityCellState,
            bool observerIsOmniscient = false)
        {
            return observerOwnerId == entityOwnerId ||
                   observerIsOmniscient ||
                   entityCellState == FogState.Visible;
        }

        public static bool ShouldShowLastKnownBuilding(
            int observerOwnerId,
            int buildingOwnerId,
            bool wasSeen,
            FogState buildingCellState)
        {
            return observerOwnerId != buildingOwnerId &&
                   wasSeen &&
                   buildingCellState == FogState.Discovered;
        }
    }
}
