using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ObsidianProtocol.UI.Store;

public static class StoreHUDSceneFix
{
    [MenuItem("Obsidian Protocol/Store/FIX HUD IN SCENE")]
    public static void Fix()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        GameObject functional = null;

        foreach (GameObject go in objects)
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
            Debug.LogError("[STORE HUD] StoreMasterController HUD not found.");
            return;
        }

        foreach (GameObject go in objects)
        {
            if (go.name == "MILITARY STORE HUD" &&
                go != functional)
            {
                Object.DestroyImmediate(go);
            }
        }

        functional.transform.SetParent(null, false);
        functional.transform.localPosition = Vector3.zero;
        functional.transform.localRotation = Quaternion.identity;
        functional.transform.localScale = Vector3.one;

        Canvas canvas = functional.GetComponent<Canvas>();

        if (canvas == null)
            canvas = functional.AddComponent<Canvas>();

        canvas.enabled = true;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler =
            functional.GetComponent<CanvasScaler>();

        if (scaler == null)
            scaler = functional.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        GraphicRaycaster raycaster =
            functional.GetComponent<GraphicRaycaster>();

        if (raycaster == null)
            functional.AddComponent<GraphicRaycaster>();

        functional.SetActive(true);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = functional;

        Debug.Log(
            "[STORE HUD] SCENE FIX COMPLETE | " +
            "ROOT LEVEL | SCALE 1,1,1 | " +
            "SCREEN SPACE OVERLAY | FUNCTIONAL HUD KEPT");
    }
}
