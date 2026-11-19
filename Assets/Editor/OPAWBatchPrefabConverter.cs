using System.IO;
using UnityEditor;
using UnityEngine;

public static class OPAWBatchPrefabConverter
{
    private const string SourceFolder = "Assets/Models/others";

    [MenuItem("Obsidian Protocol/Units/CONVERT ALL IMPORTED MODELS TO PREFABS")]
    public static void ConvertAllModels()
    {
        if (!AssetDatabase.IsValidFolder(SourceFolder))
        {
            Debug.LogError("[OPAW PREFAB CONVERTER] Source folder not found: " + SourceFolder);
            return;
        }

        string[] modelGuids = AssetDatabase.FindAssets("t:Model", new[] { SourceFolder });

        int created = 0;
        int skipped = 0;
        int failed = 0;

        try
        {
            for (int i = 0; i < modelGuids.Length; i++)
            {
                string modelPath = AssetDatabase.GUIDToAssetPath(modelGuids[i]);

                EditorUtility.DisplayProgressBar(
                    "OPAW Model to Prefab Conversion",
                    Path.GetFileName(modelPath),
                    modelGuids.Length == 0 ? 1f : (float)i / modelGuids.Length
                );

                GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);

                if (model == null)
                {
                    failed++;
                    Debug.LogWarning("[OPAW PREFAB CONVERTER] Could not load model: " + modelPath);
                    continue;
                }

                string prefabPath = Path.ChangeExtension(modelPath, ".prefab");

                if (File.Exists(prefabPath))
                {
                    skipped++;
                    continue;
                }

                bool success;
                PrefabUtility.SaveAsPrefabAsset(model, prefabPath, out success);

                if (success)
                {
                    created++;
                    Debug.Log("[OPAW PREFAB CONVERTER] Created: " + prefabPath);
                }
                else
                {
                    failed++;
                    Debug.LogError("[OPAW PREFAB CONVERTER] Failed: " + modelPath);
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        Debug.Log(
            "[OPAW PREFAB CONVERTER] COMPLETE | Created: " + created +
            " | Already existed: " + skipped +
            " | Failed: " + failed
        );
    }
}
