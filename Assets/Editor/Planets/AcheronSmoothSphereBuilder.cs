using UnityEngine;
using UnityEditor;

public static class AcheronSmoothSphereBuilder
{
    [MenuItem("Tools/Acheron/Build Smooth Planet Sphere")]
    private static void Build()
    {
        GameObject planet = Selection.activeGameObject;

        if (planet == null || planet.name != "ACHERON_PLANET_CORE")
        {
            Debug.LogError("Select ACHERON_PLANET_CORE first.");
            return;
        }

        MeshFilter filter = planet.GetComponent<MeshFilter>();
        if (filter == null) return;

        const int segments = 128;
        const int rings = 64;
        const float radius = 0.5f;

        Mesh mesh = new Mesh();
        mesh.name = "ACHERON_HIGH_RESOLUTION_SPHERE";

        Vector3[] vertices = new Vector3[(rings + 1) * (segments + 1)];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int y = 0; y <= rings; y++)
        {
            float v = (float)y / rings;
            float latitude = Mathf.PI * v;

            for (int x = 0; x <= segments; x++)
            {
                float u = (float)x / segments;
                float longitude = 2f * Mathf.PI * u;
                int index = y * (segments + 1) + x;

                vertices[index] = new Vector3(
                    -radius * Mathf.Sin(latitude) * Mathf.Cos(longitude),
                    radius * Mathf.Cos(latitude),
                    radius * Mathf.Sin(latitude) * Mathf.Sin(longitude)
                );

                uv[index] = new Vector2(u, 1f - v);
            }
        }

        int[] triangles = new int[rings * segments * 6];
        int t = 0;

        for (int y = 0; y < rings; y++)
        {
            for (int x = 0; x < segments; x++)
            {
                int a = y * (segments + 1) + x;
                int b = a + segments + 1;

                triangles[t++] = a;
                triangles[t++] = b;
                triangles[t++] = a + 1;

                triangles[t++] = a + 1;
                triangles[t++] = b;
                triangles[t++] = b + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        const string path = "Assets/Art/Models/others/Planets/Acheron/ACHERON_HIGH_RESOLUTION_SPHERE.asset";

        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();

        filter.sharedMesh = mesh;
        EditorUtility.SetDirty(filter);

        Debug.Log("Acheron smooth sphere created. Existing material preserved.");
    }
}
