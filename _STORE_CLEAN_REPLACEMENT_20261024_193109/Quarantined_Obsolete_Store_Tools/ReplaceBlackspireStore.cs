using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ReplaceBlackspireStore
{
    [MenuItem("Obsidian Protocol/Store/Replace Store In Blackspire")]
    private static void Replace()
    {
        const string scenePath = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        const string oldPath = "Assets/Prefabs/Buildings/Store/STORE_AAA_REBUILD.prefab";
        const string newPath = "Assets/Prefabs/Buildings/Store/STORE_FACILITY.prefab";

        if (EditorApplication.isPlaying) return;

        Scene scene = SceneManager.GetSceneByPath(scenePath);
        bool openedHere = !scene.IsValid() || !scene.isLoaded;

        try
        {
            if (openedHere)
                scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            GameObject oldAsset = AssetDatabase.LoadAssetAtPath<GameObject>(oldPath);
            GameObject newAsset = AssetDatabase.LoadAssetAtPath<GameObject>(newPath);

            if (oldAsset == null || newAsset == null)
            {
                Debug.LogError("[BLACKSPIRE STORE] Old or new prefab asset missing. No changes made.");
                return;
            }

            GameObject oldInstance = null;
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                foreach (Transform item in root.GetComponentsInChildren<Transform>(true))
                {
                    GameObject source = PrefabUtility.GetCorrespondingObjectFromSource(item.gameObject);
                    if (source == oldAsset || (source != null &&
                        AssetDatabase.GetAssetPath(source) == oldPath))
                    {
                        oldInstance = item.gameObject;
                        break;
                    }
                }
                if (oldInstance != null) break;
            }

            if (oldInstance == null)
            {
                Debug.LogError("[BLACKSPIRE STORE] No loaded instance of old prefab found. Scene unchanged.");
                return;
            }

            Vector3 position = oldInstance.transform.position;
            Quaternion rotation = oldInstance.transform.rotation;
            Vector3 scale = oldInstance.transform.localScale;
            Transform parent = oldInstance.transform.parent;

            GameObject replacement = PrefabUtility.InstantiatePrefab(newAsset, scene) as GameObject;
            if (replacement == null)
            {
                Debug.LogError("[BLACKSPIRE STORE] New prefab instantiation failed. Old instance preserved.");
                return;
            }

            replacement.name = "STORE_FACILITY";
            replacement.transform.SetParent(parent, true);
            replacement.transform.SetPositionAndRotation(position, rotation);
            replacement.transform.localScale = scale;

            Undo.DestroyObjectImmediate(oldInstance);
            EditorSceneManager.MarkSceneDirty(scene);

            if (!EditorSceneManager.SaveScene(scene))
            {
                Debug.LogError("[BLACKSPIRE STORE] Save failed. Inspect the scene before proceeding.");
                return;
            }

            Debug.Log("[BLACKSPIRE STORE] Replacement saved at " + position);
        }
        finally
        {
            if (openedHere && scene.IsValid() && scene.isLoaded)
                EditorSceneManager.CloseScene(scene, true);
        }
    }
}
