using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class FogOfWarManager : MonoBehaviour
{
    [System.Serializable]
    public class VisionGrid
    {
        public Vector2Int size { get; private set; }

        private FactionTemplate.PlayerID[] values = null;
        private FactionTemplate.PlayerID[] visited = null;
        private BitArray changed = null;

        public VisionGrid(Vector2Int gridSize)
        {
            size = gridSize;
            int lenght = size.x * size.y;
            values = new FactionTemplate.PlayerID[lenght];
            visited = new FactionTemplate.PlayerID[lenght];
        }

        public void SetVisible(Vector2Int pos, FactionTemplate.PlayerID players, bool value)
        {
            int index = pos.x + pos.y * size.y;

            if (value)
            {
                if ((values[index] & players) == 0)
                {
                    changed?.Set(index, true);
                }

                values[index] |= players;
                visited[index] |= players;
            }
            else
            {
                if ((values[index] & players) > 0)
                {
                    changed?.Set(index, true);
                }

                values[index] ^= ~players;
            }
        }

        public void ClearViewed()
        {
            int lenght = size.x * size.y;
            Array.Clear(values, 0, lenght);
        }

        public void ClearChangedMarks()
        {
            changed?.SetAll(false);
        }

        public bool IsVisible(int index, FactionTemplate.PlayerID players)
        {
            return (values[index] & players) > 0;
        }

        public bool IsVisible(Vector2Int pos, FactionTemplate.PlayerID players)
        {
            return (values[pos.x + pos.y * size.y] & players) > 0;
        }

        public bool WasVisible(int index, FactionTemplate.PlayerID players)
        {
            return (visited[index] & players) > 0;
        }

        public bool WasVisible(Vector2Int pos, FactionTemplate.PlayerID players)
        {
            return (visited[pos.x + pos.y * size.y] & players) > 0;
        }
    }

    [System.Serializable]
    public class GridTreeCell
    {
        public Vector2Int localPos;
        public GridTreeCell parent { get; private set; }

        [SerializeField]
        private List<GridTreeCell> branchedOffCells;

        public GridTreeCell(Vector2Int pos, GridTreeCell parentCell = null)
        {
            localPos = pos;
            branchedOffCells = new List<GridTreeCell>();
            parent = parentCell;
            parentCell?.AddBranchCell(this);
        }

        public void AddBranchCell(GridTreeCell branchCell)
        {
            branchedOffCells.Add(branchCell);
        }

        public bool ContainsChildAt(Vector2Int pos)
        {
            return pos == localPos ||
                   branchedOffCells.Exists(cell => cell.localPos == pos);
        }

        public List<GridTreeCell> GetChildren()
        {
            return branchedOffCells;
        }
    }

    public Vector2Int gridSize = new Vector2Int(128, 128);
    public int cellSize = 1;

    private VisionGrid visionGrid;
    private TerrainHeightMap terrainGrid;
    private RectInt gridBounds;

    private Texture2D texture;
    private Color[] colors;
    private List<int> activeCellList = new List<int>();
    private BitArray activeCells;

    public Material material;
    public FilterMode filter = FilterMode.Bilinear;
    public Projector projector;

    private Color fowUnexplored = Color.black;
    private Color fowNotViewed = Color.black.ToWithA(0.6f);
    private Color fowViewed = Color.white.ToWithA(0f);

    public float decline = 1f;

    [Space]

    public int drawFunction;
    public bool drawGrid;
    public bool drawHeightValues;
    public Mesh drawQuadMesh;

    private static System.Comparison<Vector2Int> signedAngleComparison =
        new System.Comparison<Vector2Int>(Vector_Extension.SignedAngle);

    [Space]

    public RegisterObject units;
    public RegisterObject buildings;

    private List<ClickableObject> queueCurrent = new List<ClickableObject>();
    private int queueIndex = 0;

    public int perFrame = 10;

    public FactionTemplate.PlayerID activePlayersFlag;

    private float lastCheck;

    public float interpolateColorSpeed = 6f;

    public Vector2Int gridOffset
    {
        get
        {
            return new Vector2Int(
                Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.z));
        }
    }

    [NonSerialized]
    public Dictionary<int, GridTreeCell> trees =
        new Dictionary<int, GridTreeCell>();

    // REST OF FILE REMAINS UNCHANGED
}