using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ObsidianProtocol.UI.Store;

public static class StoreRebuildBuilder
{
    [MenuItem("Obsidian Protocol/Store/Install Rebuilt Store")]
    public static void Build()
    {
        var scene =
            EditorSceneManager.GetActiveScene();

        if (!scene.IsValid())
        {
            Debug.LogError(
                "[STORE] No valid active scene.");
            return;
        }

        GameObject hud =
            GameObject.Find("MILITARY STORE HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[STORE] MILITARY STORE HUD not found.");
            return;
        }

        StoreRuntimeController existing =
            hud.GetComponent<StoreRuntimeController>();

        if (existing == null)
            existing =
                hud.AddComponent<StoreRuntimeController>();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[STORE] Rebuilt Store controller installed.");
    }
}
