using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ObsidianProtocol.UI.Store;

public static class FINAL_STORE_CLEANUP
{
    [MenuItem("Obsidian Protocol/Store/FINAL CLEAN STORE")]
    public static void Clean()
    {
        GameObject hud =
            GameObject.Find("MILITARY STORE HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[STORE] MILITARY STORE HUD NOT FOUND.");
            return;
        }

        string[] remove =
        {
            "STORE_FINAL_INTERFACE",
            "STORE_REBUILT_INTERFACE",
            "STORE_RUNTIME_INTERFACE",
            "STORE_RUNTIME_OVERLAY"
        };

        foreach (string name in remove)
        {
            Transform child =
                hud.transform.Find(name);

            if (child != null)
            {
                Object.DestroyImmediate(
                    child.gameObject);

                Debug.Log(
                    "[STORE] Removed old UI: " +
                    name);
            }
        }

        StoreRuntimeController controller =
            hud.GetComponent<StoreRuntimeController>();

        if (controller == null)
        {
            controller =
                hud.AddComponent<StoreRuntimeController>();
        }

        EditorUtility.SetDirty(hud);

        EditorSceneManager.MarkSceneDirty(
            hud.scene);

        EditorSceneManager.SaveScene(
            hud.scene);

        Debug.Log(
            "[STORE] FINAL CLEAN STORE INSTALLED.");
    }
}
