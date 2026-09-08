using UnityEngine;

[CreateAssetMenu()]
public class TerrainPropertiesSO : ScriptableObject
{
    public bool autoRefresh;
    [Range(1, 128)] public int xSize = 1, zSize = 1;
    [Range(1, 255)] public int xResolution = 1, zResolution = 1;
    [Range(-1f, 1f)] public float xAmp, zAmp;
    [Range(0, 5f)] public float frequency = 0f;
}
