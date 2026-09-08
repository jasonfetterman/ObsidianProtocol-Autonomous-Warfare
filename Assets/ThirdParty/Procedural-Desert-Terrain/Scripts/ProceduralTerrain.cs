using UnityEngine;

public class ProceduralTerrain : MonoBehaviour
{
    public GizmoSettingsSO gizmoSettings;
    [HideInInspector] public bool gizmoSettingsFoldout = true;
    public TerrainPropertiesSO terrainProperties;
    [HideInInspector] public bool terrainPropertiesFoldout = true;

    MeshRenderer _meshRenderer;
    MeshFilter _meshFilter;
    Mesh _mesh;
    
    private Vector3[] _vertices;
    private int[] _triangles;

    // Start is called before the first frame update
    void Start()
    {
        BuildMesh();
    }

    private void OnValidate()
    {
        BuildMesh();
    }

    public void BuildMesh()
    {
        if (terrainProperties == null) return;
        SetUpComponents();
        SetVertices();
        SetTriangles();
        UpdateMesh();
    }

    private void OnDrawGizmos()
    {
        if (_vertices == null || _vertices.Length == 0 || gizmoSettings == null) return;
        if (Application.isPlaying && !gizmoSettings.showInGame) return;
        if (!Application.isPlaying && !gizmoSettings.showInInspector) return;
        

        Gizmos.color = gizmoSettings.color;
        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(transform.TransformPoint(_vertices[i]), gizmoSettings.radius);
        }
    }


    private void SetUpComponents()
    {
        _mesh = new Mesh
        {
            name = "Terrain"
        };

        _meshRenderer = GetComponent<MeshRenderer>();
        if(_meshRenderer == null)
        {
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
        }

        _meshFilter = GetComponent<MeshFilter>();
        if (_meshFilter == null)
        {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        _meshFilter.mesh = _mesh;
    }

    private void SetVertices()
    {
        _vertices = new Vector3[(terrainProperties.xResolution + 1) * (terrainProperties.zResolution + 1)];

        float xStep = terrainProperties.xSize * 1f / terrainProperties.xResolution;
        float zStep = terrainProperties.zSize * 1f / terrainProperties.zResolution;

        for (int z = 0, v = 0; z <= terrainProperties.zResolution; z++)
        {
            for (int x = 0; x <= terrainProperties.xResolution; x++)
            {
                float dx = x * xStep;
                float dz = z * zStep;
                float y = terrainProperties.frequency * Mathf.PerlinNoise(terrainProperties.xAmp * dx, terrainProperties.zAmp * dz);
                _vertices[v++] = new Vector3(dx, y, dz);
            }
        }
    }

    private void SetTriangles()
    {
        _triangles = new int[6 * (terrainProperties.xResolution) * (terrainProperties.zResolution)];

        for (int z = 0, tris = 0, vert = 0; z < terrainProperties.zResolution; z++, vert++)
        {
            for (int x = 0; x < terrainProperties.xResolution; x++, vert++, tris += 6)
            {
                _triangles[tris] = vert;
                _triangles[tris + 1] = vert + terrainProperties.xResolution + 1;
                _triangles[tris + 2] = vert + 1;

                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + terrainProperties.xResolution + 1;
                _triangles[tris + 5] = vert + terrainProperties.xResolution + 2;
            }
        }
    }

    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }

    
}
