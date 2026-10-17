using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RepairBlackspireStoreTerminal
{
    [MenuItem("Obsidian Protocol/Store/Repair Blackspire Terminal Interaction")]
    private static void Run()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity")
        {
            Debug.LogError("[STORE REPAIR] Open Blackspire_Continent first.");
            return;
        }

        GameObject root = null;
        foreach (var go in scene.GetRootGameObjects())
        {
            if (go.name == "STORE_AAA_REBUILD") { root = go; break; }
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
                if (t.name == "STORE_AAA_REBUILD") { root = t.gameObject; break; }
            if (root) break;
        }

        if (!root) { Debug.LogError("[STORE REPAIR] Store not found."); return; }

        int added = 0;
        foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
        {
            if (t.name != "Procurement terminal pedestal" ||
                !t.GetComponent<Collider>()) continue;

            if (!t.TryGetComponent<StoreTerminalInteraction>(out _))
            {
                Undo.AddComponent<StoreTerminalInteraction>(t.gameObject);
                added++;
            }
        }

        int controllers = Object.FindObjectsByType<ObsidianProtocol.UI.Store.StoreMasterController>(
            FindObjectsInactive.Include, FindObjectsSortMode.None).Length;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log($"[STORE REPAIR] Interactions added={added}; StoreMasterControllers found={controllers}.");
    }
}
