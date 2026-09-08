using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// A world-aligned XZ walkability grid. Out-of-bounds positions are never walkable.
    /// </summary>
    public sealed class WalkGrid
    {
        private readonly bool[,] walkable;

        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }
        public Vector3 Origin { get; }

        public WalkGrid(int width, int height, float cellSize)
            : this(width, height, cellSize, Vector3.zero)
        {
        }

        public WalkGrid(int width, int height, float cellSize, Vector3 origin)
        {
            if (width <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(height));
            }

            if (cellSize <= 0f)
            {
                throw new System.ArgumentOutOfRangeException(nameof(cellSize));
            }

            Width = width;
            Height = height;
            CellSize = cellSize;
            Origin = origin;
            walkable = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    walkable[x, y] = true;
                }
            }
        }

        public static WalkGrid CreateMapGrid()
        {
            float halfMap = BalanceConfig.MapSize * BalanceConfig.CellSize * 0.5f;
            return new WalkGrid(
                BalanceConfig.MapSize,
                BalanceConfig.MapSize,
                BalanceConfig.CellSize,
                new Vector3(-halfMap, 0f, -halfMap));
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool IsInBounds(Vector2Int cell)
        {
            return IsInBounds(cell.x, cell.y);
        }

        public bool IsWalkable(int x, int y)
        {
            return IsInBounds(x, y) && walkable[x, y];
        }

        public bool IsWalkable(Vector2Int cell)
        {
            return IsWalkable(cell.x, cell.y);
        }

        /// <summary>
        /// Changes a cell and returns false when the supplied coordinates are out of bounds.
        /// </summary>
        public bool SetWalkable(int x, int y, bool value)
        {
            if (!IsInBounds(x, y))
            {
                return false;
            }

            walkable[x, y] = value;
            return true;
        }

        public bool SetWalkable(Vector2Int cell, bool value)
        {
            return SetWalkable(cell.x, cell.y, value);
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition.x - Origin.x) / CellSize);
            int y = Mathf.FloorToInt((worldPosition.z - Origin.z) / CellSize);
            return new Vector2Int(x, y);
        }

        public bool TryWorldToCell(Vector3 worldPosition, out Vector2Int cell)
        {
            cell = WorldToCell(worldPosition);
            return IsInBounds(cell);
        }

        public Vector3 CellToWorld(Vector2Int cell)
        {
            return CellToWorld(cell.x, cell.y);
        }

        public Vector3 CellToWorld(int x, int y)
        {
            return new Vector3(
                Origin.x + (x + 0.5f) * CellSize,
                Origin.y,
                Origin.z + (y + 0.5f) * CellSize);
        }

        public Vector3 ClampWorldPosition(Vector3 worldPosition)
        {
            const float edgeEpsilon = 0.001f;
            float maximumX = Origin.x + Width * CellSize - edgeEpsilon;
            float maximumZ = Origin.z + Height * CellSize - edgeEpsilon;
            worldPosition.x = Mathf.Clamp(worldPosition.x, Origin.x, maximumX);
            worldPosition.z = Mathf.Clamp(worldPosition.z, Origin.z, maximumZ);
            return worldPosition;
        }

        public void SetCircleWalkable(Vector3 worldCenter, float radius, bool value)
        {
            Vector2Int center = WorldToCell(worldCenter);
            int cellRadius = Mathf.CeilToInt(radius / CellSize) + 1;
            float radiusSquared = radius * radius;

            for (int x = center.x - cellRadius; x <= center.x + cellRadius; x++)
            {
                for (int y = center.y - cellRadius; y <= center.y + cellRadius; y++)
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
                        walkable[x, y] = value;
                    }
                }
            }
        }

        public bool TryFindNearestWalkable(Vector2Int desired, int maxRadius, out Vector2Int result)
        {
            if (IsWalkable(desired))
            {
                result = desired;
                return true;
            }

            for (int radius = 1; radius <= maxRadius; radius++)
            {
                int minimumX = desired.x - radius;
                int maximumX = desired.x + radius;
                int minimumY = desired.y - radius;
                int maximumY = desired.y + radius;

                for (int x = minimumX; x <= maximumX; x++)
                {
                    if (IsWalkable(x, minimumY))
                    {
                        result = new Vector2Int(x, minimumY);
                        return true;
                    }

                    if (IsWalkable(x, maximumY))
                    {
                        result = new Vector2Int(x, maximumY);
                        return true;
                    }
                }

                for (int y = minimumY + 1; y < maximumY; y++)
                {
                    if (IsWalkable(minimumX, y))
                    {
                        result = new Vector2Int(minimumX, y);
                        return true;
                    }

                    if (IsWalkable(maximumX, y))
                    {
                        result = new Vector2Int(maximumX, y);
                        return true;
                    }
                }
            }

            result = default;
            return false;
        }
    }
}
