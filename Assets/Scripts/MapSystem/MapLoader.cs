using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class MapLoader
{
    public MapDefinition map;
    public NavMeshSurface navSurface;

    public GameObject playerBasePrefab;
    public GameObject enemyBasePrefab;

    public void LoadMap()
    {
        if (map == null)
        {
            Debug.LogError("No map assigned!");
            return;
        }

        Object.Instantiate(map.terrainPrefab, Vector3.zero, Quaternion.identity);

        SpawnBases();
        SpawnResources();

        if (navSurface != null)
            navSurface.BuildNavMesh();
    }

    void SpawnBases()
    {
        Object.Instantiate(playerBasePrefab, map.playerSpawn, Quaternion.identity);
        Object.Instantiate(enemyBasePrefab, map.enemySpawn, Quaternion.identity);
    }

    void SpawnResources()
    {
        foreach (var node in map.resourceNodes)
        {
            Object.Instantiate(node.prefab, node.position, Quaternion.identity);
        }
    }
}
