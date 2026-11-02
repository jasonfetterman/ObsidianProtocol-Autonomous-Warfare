using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class StoreTotalReset
{
    [MenuItem("Obsidian Protocol/Store/TOTAL RESET STORE")]
    public static void ResetStore()
    {
        var scene = EditorSceneManager.GetActiveScene();

        if (!scene.IsValid())
        {
            Debug.LogError("[STORE] No active scene.");
            return;
        }

        GameObject[] roots = scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            Object.DestroyImmediate(root);
        }

        GameObject storeRoot =
            new GameObject("MILITARY_STORE");

        GameObject facility =
            new GameObject("STORE_FACILITY");

        GameObject hud =
            new GameObject("MILITARY STORE HUD");

        facility.transform.SetParent(
            storeRoot.transform,
            false);

        hud.transform.SetParent(
            storeRoot.transform,
            false);

        Canvas canvas =
            hud.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            hud.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        hud.AddComponent<GraphicRaycaster>();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[STORE] TOTAL RESET COMPLETE. " +
            "OLD STORE ERASED. NEW EMPTY STORE ROOT CREATED.");
    }
}
