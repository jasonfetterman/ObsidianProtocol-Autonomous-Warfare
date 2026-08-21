using UnityEngine;

public class VRSpawnAtTerrainCenter : MonoBehaviour
{
    [SerializeField] private float heightOffset = 0.05f;

    private void Start()
    {
        Terrain terrain = Terrain.activeTerrain;

        if (terrain == null)
        {
            Debug.LogWarning("VRSpawnAtTerrainCenter: No active Terrain found.");
            return;
        }

        Vector3 center = terrain.transform.position;
        center.x += terrain.terrainData.size.x * 0.5f;
        center.z += terrain.terrainData.size.z * 0.5f;
        center.y = terrain.SampleHeight(center) + terrain.transform.position.y + heightOffset;

        transform.position = center;
    }
}
