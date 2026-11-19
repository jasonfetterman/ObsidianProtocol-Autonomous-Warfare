using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class BuildBattlefieldCollisionMesh
{
    private const int Resolution = 256;
    private const string AssetPath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/Battlefield_Terrain_Collision.asset";

    [MenuItem("Obsidian Protocol/Battlefield/Build Simplified Terrain Collision")]
    private static void Build()
    {
        GameObject terrain = GameObject.Find("Battlefield_Terrain");

        if (terrain == null)
        {
            Debug.LogError("[OPAW] Battlefield_Terrain was not found.");
            return;
        }

        MeshFilter filter = terrain.GetComponent<MeshFilter>();
        MeshCollider original = terrain.GetComponent<MeshCollider>();

        if (filter == null || filter.sharedMesh == null)
        {
            Debug.LogError("[OPAW] Terrain visual mesh is missing.");
            return;
        }

        Mesh source = filter.sharedMesh;
        Vector3[] sourceVertices = source.vertices;

        if (sourceVertices.Length != 4194304)
        {
            Debug.LogError("[OPAW] Unexpected terrain vertex count: " +
                           sourceVertices.Length);
            return;
        }

        Vector3 min = source.bounds.min;
        Vector3 max = source.bounds.max;

        float[,] heightSums = new float[Resolution, Resolution];
        int[,] counts = new int[Resolution, Resolution];

        foreach (Vector3 v in sourceVertices)
        {
            int x = Mathf.Clamp(
                Mathf.RoundToInt((v.x - min.x) / (max.x - min.x) *
                (Resolution - 1)), 0, Resolution - 1);

            int z = Mathf.Clamp(
                Mathf.RoundToInt((v.z - min.z) / (max.z - min.z) *
                (Resolution - 1)), 0, Resolution - 1);

            heightSums[z, x] += v.y;
            counts[z, x]++;
        }

        Vector3[] vertices = new Vector3[Resolution * Resolution];

        for (int z = 0; z < Resolution; z++)
        {
            for (int x = 0; x < Resolution; x++)
            {
                int i = z * Resolution + x;

                if (counts[z, x] == 0)
                {
                    Debug.LogError("[OPAW] Missing terrain sample at " +
                                   x + ", " + z);
                    return;
                }

                vertices[i] = new Vector3(
                    Mathf.Lerp(min.x, max.x, (float)x / (Resolution - 1)),
                    heightSums[z, x] / counts[z, x],
                    Mathf.Lerp(min.z, max.z, (float)z / (Resolution - 1)));
            }
        }

        int[] triangles = new int[
            (Resolution - 1) * (Resolution - 1) * 6];

        int t = 0;

        for (int z = 0; z < Resolution - 1; z++)
        {
            for (int x = 0; x < Resolution - 1; x++)
            {
                int a = z * Resolution + x;
                int b = a + 1;
                int c = a + Resolution;
                int d = c + 1;

                triangles[t++] = a;
                triangles[t++] = c;
                triangles[t++] = b;

                triangles[t++] = b;
                triangles[t++] = c;
                triangles[t++] = d;
            }
        }

        Mesh collisionMesh = new Mesh();
        collisionMesh.name = "Battlefield_Simplified_Collision";
        collisionMesh.indexFormat =
            UnityEngine.Rendering.IndexFormat.UInt32;
        collisionMesh.vertices = vertices;
        collisionMesh.triangles = triangles;
        collisionMesh.RecalculateBounds();

        AssetDatabase.DeleteAsset(AssetPath);
        AssetDatabase.CreateAsset(collisionMesh, AssetPath);
        AssetDatabase.SaveAssets();

        Transform existing = terrain.transform.Find(
            "Battlefield_Terrain_Collision");

        if (existing != null)
            Object.DestroyImmediate(existing.gameObject);

        GameObject collisionObject = new GameObject(
            "Battlefield_Terrain_Collision");

        collisionObject.transform.SetParent(terrain.transform, false);

        MeshCollider collider =
            collisionObject.AddComponent<MeshCollider>();

        collider.sharedMesh = collisionMesh;
        collider.convex = false;
        collider.enabled = false;

        EditorUtility.SetDirty(terrain);
        EditorSceneManager.MarkSceneDirty(terrain.scene);
        EditorSceneManager.SaveScene(terrain.scene);

        Debug.Log("[OPAW] Simplified collision mesh created: " +
                  vertices.Length + " vertices, " +
                  triangles.Length / 3 + " triangles. " +
                  "Simplified collider enabled; original collider preserved but disabled.");
    }
}


