using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SurveyBlackspireTerrain
{
    private const string ScenePath =
        "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";

    [MenuItem("Obsidian Protocol/World Building/Survey Blackspire Buildable Ground")]
    public static void Run()
    {
        var scene = SceneManager.GetSceneByPath(ScenePath);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;
        if (openedHere)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Additive);

        try
        {
            GameObject terrain = null;
            GameObject citadel = null;

            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.name == "BLACKSPIRE MESHY TERRAIN")
                    terrain = root;

                if (root.name == "CRIMSON_CLOWN_CITADEL_HEADQUARTERS")
                    citadel = root;
            }

            if (!terrain || !citadel)
            {
                Debug.LogError("[GROUND SURVEY] Terrain or citadel not found.");
                return;
            }

            var colliders = terrain.GetComponentsInChildren<MeshCollider>(true);
            if (colliders.Length == 0)
            {
                Debug.LogError("[GROUND SURVEY] No terrain MeshCollider found.");
                return;
            }

            Vector3 center = citadel.transform.position;
            var candidates = new List<string>();
            int samples = 0;
            int hitCount = 0;
            float bestSlope = 90f;
            Vector3 bestPoint = Vector3.zero;

            // Survey a 600 m x 600 m area around the user's current citadel.
            for (float x = center.x - 300f; x <= center.x + 300f; x += 25f)
            for (float z = center.z - 300f; z <= center.z + 300f; z += 25f)
            {
                samples++;
                foreach (var col in colliders)
                {
                    if (!col || !col.enabled || !col.sharedMesh) continue;

                    Bounds b = col.bounds;
                    Ray ray = new Ray(
                        new Vector3(x, b.max.y + 200f, z), Vector3.down);

                    if (!col.Raycast(ray, out RaycastHit hit, b.size.y + 500f))
                        continue;

                    hitCount++;
                    float slope = Vector3.Angle(hit.normal, Vector3.up);

                    if (slope < bestSlope)
                    {
                        bestSlope = slope;
                        bestPoint = hit.point;
                    }

                    if (slope <= 8f)
                    {
                        float distance = Vector2.Distance(
                            new Vector2(x, z),
                            new Vector2(center.x, center.z));

                        candidates.Add(
                            distance.ToString("F1") + "|" +
                            slope.ToString("F1") + "|" +
                            hit.point.x.ToString("F1") + "|" +
                            hit.point.y.ToString("F1") + "|" +
                            hit.point.z.ToString("F1"));
                    }

                    break;
                }
            }

            candidates.Sort((a, b) =>
            {
                float da = float.Parse(a.Split('|')[0]);
                float db = float.Parse(b.Split('|')[0]);
                return da.CompareTo(db);
            });

            Debug.Log("[GROUND SURVEY] Read-only survey complete. No scene changes made.");
            Debug.Log("[GROUND SURVEY] Citadel position: " + center.ToString("F2"));
            Debug.Log("[GROUND SURVEY] Grid samples: " + samples +
                      " | Terrain hits: " + hitCount +
                      " | Flat candidates (slope <= 8 degrees): " + candidates.Count);
            Debug.Log("[GROUND SURVEY] Lowest-slope sample: " +
                      bestPoint.ToString("F2") + " | Slope: " +
                      bestSlope.ToString("F1") + " degrees");

            int count = Mathf.Min(20, candidates.Count);
            for (int i = 0; i < count; i++)
            {
                string[] v = candidates[i].Split('|');
                Debug.Log("[GROUND CANDIDATE] Distance=" + v[0] +
                          "m | Slope=" + v[1] + "deg | Position=(" +
                          v[2] + ", " + v[3] + ", " + v[4] + ")");
            }
        }
        finally
        {
            if (openedHere && scene.IsValid() && scene.isLoaded)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
