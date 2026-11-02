using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireRockTest
{
    [MenuItem("Obsidian Protocol/World Building/Place Blackspire Rock Test")]
    public static void Place()
    {
        const string scenePath = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(scenePath);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        if (openedHere)
            scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

        try
        {
            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.name == "BLACKSPIRE_ROCK_TEST")
                {
                    Debug.LogWarning("[BLACKSPIRE ROCK TEST] Test group already exists. No changes made.");
                    return;
                }
            }

            var terrainRoot = System.Array.Find(scene.GetRootGameObjects(),
                go => go.name == "BLACKSPIRE MESHY TERRAIN");
            if (terrainRoot == null)
            {
                Debug.LogError("[BLACKSPIRE ROCK TEST] Terrain root not found.");
                return;
            }

            var terrainCollider = terrainRoot.GetComponentInChildren<MeshCollider>(true);
            if (terrainCollider == null || !terrainCollider.enabled)
            {
                Debug.LogError("[BLACKSPIRE ROCK TEST] Enabled terrain MeshCollider not found.");
                return;
            }

            string folder = "Assets/Rocks and Boulders 2/Rocks/Prefabs/";
            string[] names = { "Rock2", "Rock5A", "Rock1A", "Rock4A", "Rock6B" };
            Vector3[] offsets =
            {
                new Vector3(0, 0, 0),
                new Vector3(12, 0, 3),
                new Vector3(-11, 0, 5),
                new Vector3(5, 0, -12),
                new Vector3(-5, 0, -9)
            };

            var group = new GameObject("BLACKSPIRE_ROCK_TEST");
            SceneManager.MoveGameObjectToScene(group, scene);

            Vector3 testCenter = new Vector3(700f, 250f, 700f);
            int placed = 0;

            for (int i = 0; i < names.Length; i++)
            {
                Vector3 rayStart = testCenter + offsets[i] + Vector3.up * 500f;
                if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit,
                    1000f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    Debug.LogWarning("[BLACKSPIRE ROCK TEST] No ground hit for " + names[i]);
                    continue;
                }

                if (hit.collider != terrainCollider)
                {
                    Debug.LogWarning("[BLACKSPIRE ROCK TEST] Ray hit another collider for " + names[i] +
                        ". Skipped to avoid placing on the wrong surface.");
                    continue;
                }

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(folder + names[i] + ".prefab");
                if (prefab == null)
                {
                    Debug.LogWarning("[BLACKSPIRE ROCK TEST] Missing prefab " + names[i]);
                    continue;
                }

                var rock = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
                rock.name = names[i] + "_TEST";
                rock.transform.SetParent(group.transform, true);
                rock.transform.position = hit.point;
                rock.transform.rotation = Quaternion.Euler(0f, i * 67f, 0f);

                var renderers = rock.GetComponentsInChildren<Renderer>(true);
                if (renderers.Length > 0)
                {
                    Bounds bounds = renderers[0].bounds;
                    foreach (var renderer in renderers)
                        bounds.Encapsulate(renderer.bounds);

                    float bottomOffset = bounds.min.y - rock.transform.position.y;
                    rock.transform.position += Vector3.up * (-bottomOffset + 0.05f);
                }

                placed++;
                Debug.Log("[BLACKSPIRE ROCK TEST] Placed " + names[i] +
                    " at " + rock.transform.position.ToString("F2"));
            }

            if (placed == 0)
            {
                Object.DestroyImmediate(group);
                Debug.LogError("[BLACKSPIRE ROCK TEST] No rocks placed; scene left unchanged.");
                return;
            }

            EditorSceneManager.MarkSceneDirty(scene);
            if (EditorSceneManager.SaveScene(scene))
                Debug.Log("[BLACKSPIRE ROCK TEST] Saved " + placed + " test rocks.");
        }
        finally
        {
            if (openedHere)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
