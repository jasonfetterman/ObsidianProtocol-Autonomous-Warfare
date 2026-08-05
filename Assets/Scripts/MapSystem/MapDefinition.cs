using UnityEngine;

[CreateAssetMenu(fileName = "MapDefinition", menuName = "RTS/Map Definition")]
public class MapDefinition : ScriptableObject
{
    public GameObject terrainPrefab;

    public Vector3 playerSpawn;
    public Vector3 enemySpawn;

    public ResourceNodeData[] resourceNodes;
}
