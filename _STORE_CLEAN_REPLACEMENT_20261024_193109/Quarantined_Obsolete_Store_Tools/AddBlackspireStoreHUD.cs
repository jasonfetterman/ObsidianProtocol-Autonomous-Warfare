using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.UI.Store;

public static class AddBlackspireStoreHUD
{
    [MenuItem("Obsidian Protocol/Store/Add Blackspire Store HUD")]
    private static void Run()
    {
        const string expectedScene =
            "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";

        var scene = SceneManager.GetActiveScene();

        if (scene.path != expectedScene)
        {
            Debug.LogError("[STORE HUD] Open Blackspire_Continent first.");
            return;
        }

        bool storeFound = false;
        foreach (var root in scene.GetRootGameObjects())
        {
            if (root.name == "STORE_AAA_REBUILD")
            {
                storeFound = true;
                break;
            }

            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == "STORE_AAA_REBUILD")
                {
                    storeFound = true;
                    break;
                }
            }

            if (storeFound) break;
        }

        if (!storeFound)
        {
            Debug.LogError("[STORE HUD] STORE_AAA_REBUILD not found.");
            return;
        }

        foreach (var root in scene.GetRootGameObjects())
        {
            if (root.GetComponent<StoreMasterController>() != null)
            {
                Debug.Log("[STORE HUD] Controller already exists on " + root.name);
                return;
            }
        }

        var hud = new GameObject(
            "BLACKSPIRE STORE HUD",
            typeof(RectTransform));

        Undo.RegisterCreatedObjectUndo(hud, "Add Blackspire Store HUD");
        Undo.AddComponent<StoreMasterController>(hud);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[STORE HUD] Added dedicated controller to Blackspire. Existing Store geometry unchanged.");
    }
}
