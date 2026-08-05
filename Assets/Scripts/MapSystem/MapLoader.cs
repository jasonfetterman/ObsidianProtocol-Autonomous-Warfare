using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapLoader : MonoBehaviour
{
    public MapDefinition map;
    public NavMeshSurface navSurface;

    public GameObject playerBasePrefab;
    public GameObject enemyBasePrefab;

    void Start()
    {
        LoadMap();
    }

    public void LoadMap()
    {
        if (map == null)
        {
            Debug.LogError("No map assigned!");
            return;
        }

        Instantiate(map.terrainPrefab, Vector3.zero, Quaternion.identity);

        SpawnBases();
        SpawnResources();

        if (navSurface != null)
            navSurface.BuildNavMesh();
    }

    void SpawnBases()
    {
        Instantiate(playerBasePrefab, map.playerSpawn, Quaternion.identity);
        Instantiate(enemyBasePrefab, map.enemySpawn, Quaternion.identity);
    }

    void SpawnResources()
    {
        foreach (var node in map.resourceNodes)
        {
            Instantiate(node.prefab, node.position, Quaternion.identity);
        }
    }
}
