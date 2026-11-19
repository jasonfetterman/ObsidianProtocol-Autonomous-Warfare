using UnityEditor;
using UnityEngine;
using System.Linq;

public static class BlackspireUnitPrefabInspector
{
    [MenuItem("Obsidian Protocol/Battlefield/INSPECT GROUND UNIT MODEL DIMENSIONS")]
    public static void Inspect()
    {
        string[] keywords =
        {
            "Ironclad_Dozer_1013083154",
            "Ironclad_Hauler_1013083044",
            "Ironclad_Marauder_1015181433",
            "Ironclad_Viper_1015181751",
            "Iron_Hound_1015181916",
            "Iron_Mule_Transport_1015183422",
            "Iron_Sentinel_1015183225",
            "Iron_Sentinel_Rover_1015181555"
        };

        Debug.Log("========== BLACKSPIRE GROUND UNIT DIMENSIONS ==========");

        foreach (string keyword in keywords)
        {
            string guid = AssetDatabase.FindAssets(keyword + " t:Prefab",
                new[] { "Assets/Models/others" }).FirstOrDefault();

            if (string.IsNullOrEmpty(guid))
            {
                Debug.LogWarning("[UNIT INSPECT] NOT FOUND: " + keyword);
                continue;
            }

            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
            {
                Debug.LogWarning("[UNIT INSPECT] LOAD FAILED: " + path);
                continue;
            }

            GameObject instance = null;

            try
            {
                instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                if (instance == null)
                {
                    Debug.LogWarning("[UNIT INSPECT] INSTANTIATE FAILED: " + path);
                    continue;
                }

                Renderer[] renderers = instance.GetComponentsInChildren<Renderer>(true);

                if (renderers.Length == 0)
                {
                    Debug.LogWarning("[UNIT INSPECT] NO RENDERERS: " + path);
                    continue;
                }

                Bounds bounds = renderers[0].bounds;

                for (int i = 1; i < renderers.Length; i++)
                    bounds.Encapsulate(renderers[i].bounds);

                Vector3 scale = instance.transform.localScale;

                Debug.Log(
                    "[UNIT MODEL] " + prefab.name +
                    " | Renderers: " + renderers.Length +
                    " | Root Scale: " + scale +
                    " | World Dimensions: " +
                    bounds.size.x.ToString("F2") + " x " +
                    bounds.size.y.ToString("F2") + " x " +
                    bounds.size.z.ToString("F2") +
                    " | Asset: " + path
                );
            }
            finally
            {
                if (instance != null)
                    Object.DestroyImmediate(instance);
            }
        }

        Debug.Log("========== INSPECTION COMPLETE - NO SCENE CHANGES ==========");
    }
}
