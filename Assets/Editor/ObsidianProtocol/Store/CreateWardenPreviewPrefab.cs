using UnityEngine;
using UnityEditor;
using System.IO;

namespace ObsidianProtocol.EditorTools.Store
{
    public static class CreateWardenPreviewPrefab
    {
        private const string SourcePath =
            "Assets/Scenes/SCN04_GARAGE/3D units/Warden.fbx";

        private const string DestinationFolder =
            "Assets/Game/Units/Models/Air";

        private const string DestinationPath =
            DestinationFolder + "/WardenPreview.prefab";

        [MenuItem("Obsidian Protocol/Store/Create Warden Preview Prefab")]
        private static void CreatePrefab()
        {
            if (File.Exists(DestinationPath))
            {
                Debug.LogError(
                    "[STORE WARDEN] Preview prefab already exists. " +
                    "Nothing was overwritten: " + DestinationPath);
                return;
            }

            GameObject source =
                AssetDatabase.LoadAssetAtPath<GameObject>(SourcePath);

            if (source == null)
            {
                Debug.LogError(
                    "[STORE WARDEN] Could not load source FBX: " + SourcePath);
                return;
            }

            Directory.CreateDirectory(DestinationFolder);

            GameObject instance = null;

            try
            {
                instance = PrefabUtility.InstantiatePrefab(source) as GameObject;

                if (instance == null)
                {
                    Debug.LogError(
                        "[STORE WARDEN] Could not instantiate the source FBX.");
                    return;
                }

                instance.name = "WardenPreview";

                GameObject saved =
                    PrefabUtility.SaveAsPrefabAsset(
                        instance, DestinationPath);

                if (saved == null)
                {
                    Debug.LogError(
                        "[STORE WARDEN] Unity failed to save the preview prefab.");
                    return;
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Renderer[] renderers =
                    saved.GetComponentsInChildren<Renderer>(true);

                Debug.Log(
                    "[STORE WARDEN] Preview prefab created: " +
                    DestinationPath +
                    " | Renderers: " + renderers.Length);
            }
            finally
            {
                if (instance != null)
                    Object.DestroyImmediate(instance);
            }
        }
    }
}
