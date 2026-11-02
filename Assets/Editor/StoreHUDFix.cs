using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ObsidianProtocol.UI.Store;

public static class StoreHUDFix
{
    [MenuItem("Obsidian Protocol/Store/FIX STORE HUD")]
    public static void Fix()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject[] all =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        GameObject functional = null;

        foreach (GameObject go in all)
        {
            if (go.name != "MILITARY STORE HUD")
                continue;

            if (go.GetComponent<StoreMasterController>() != null)
            {
                functional = go;
                break;
            }
        }

        if (functional == null)
        {
            Debug.LogError("[STORE HUD] Functional HUD not found.");
            return;
        }

        foreach (GameObject go in all)
        {
            if (go.name == "MILITARY STORE HUD" &&
                go != functional)
            {
                Object.DestroyImmediate(go);
            }
        }

        functional.SetActive(true);
        functional.transform.localScale = Vector3.one;

        Canvas canvas =
            functional.GetComponent<Canvas>();

        if (canvas == null)
            canvas = functional.AddComponent<Canvas>();

        canvas.enabled = true;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            functional.GetComponent<CanvasScaler>();

        if (scaler == null)
            scaler = functional.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920, 1080);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        if (functional.GetComponent<GraphicRaycaster>() == null)
            functional.AddComponent<GraphicRaycaster>();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = functional;

        Debug.Log("[STORE HUD] FIXED.");
    }
}
