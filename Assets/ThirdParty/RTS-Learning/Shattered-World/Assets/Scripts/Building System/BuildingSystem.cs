using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout;
    public Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase BuildingTilemap;
    [SerializeField] GameObject[] buildings;
    private PlaceableObject objectToPlace; 

    #region Utils
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 SnapCoordinateToGrid(Vector3 coordinate)
    {
        Vector3Int cellPosition = grid.WorldToCell(coordinate);
        Vector3 cellCenter = grid.GetCellCenterWorld(cellPosition);
        return cellCenter;
    }
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] tiles = new TileBase[area.size.x * area.size.y * area.size.z];
        int index = 0;
        for(int z = 0; z < area.size.z; z++)
        {
            for(int y = 0; y < area.size.y; y++)
            {
                for(int x = 0; x < area.size.x; x++)
                {
                    Vector3Int position = new Vector3Int(x + area.xMin, y + area.yMin, z + area.zMin);
                    tiles[index] = tilemap.GetTile(position);
                    index++;
                }
            }
        }
        return tiles;
    }
    #endregion

    #region Building Placement
    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }
    public void PlaceObject()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            InitializeWithObject(buildings[0]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitializeWithObject(buildings[1]);
        }
    }
    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var tileBase in baseArray)
        {
            if(tileBase == BuildingTilemap)
            {
                return false;
            }
            //return true; // Onat burayı ben değiştirdim sorunu gidermek için bir sıkıntı varsa ilk buraya bak //
        }
        return true;
    } 
    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, BuildingTilemap, start.x, start.y, start.x + size.x, start.y + size.y); // 2. Parametreye tilebase referansını verdim //
    }
    #endregion

    #region Unity Methods
    public void Awake() 
    {
        current = this;
        grid = gridLayout.GetComponent<Grid>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
