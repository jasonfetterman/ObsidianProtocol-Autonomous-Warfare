using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObsidianProtocol.UI.Store;

public static class StoreMasterBuilder
{
    [MenuItem("Obsidian Protocol/Store/BUILD COMPLETE STORE")]
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

        GameObject[] roots =
            scene.GetRootGameObjects();

        GameObject storeRoot = null;
        GameObject hud = null;

        foreach (GameObject root in roots)
        {
            if (root.name == "MILITARY_STORE")
                storeRoot = root;

            if (root.name == "MILITARY STORE HUD")
                hud = root;
        }

        if (storeRoot == null)
        {
            storeRoot =
                new GameObject(
                    "MILITARY_STORE");
        }

        GameObject facility =
            GameObject.Find(
                "MILITARY_STORE/STORE_FACILITY");

        if (facility == null)
        {
            facility =
                new GameObject(
                    "STORE_FACILITY");

            facility.transform.SetParent(
                storeRoot.transform,
                false);
        }

        if (hud == null)
        {
            hud =
                new GameObject(
                    "MILITARY STORE HUD");

            hud.transform.SetParent(
                storeRoot.transform,
                false);
        }

        Canvas canvas =
            hud.GetComponent<Canvas>();

        if (canvas == null)
            canvas =
                hud.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            hud.GetComponent<CanvasScaler>();

        if (scaler == null)
            scaler =
                hud.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        GraphicRaycaster raycaster =
            hud.GetComponent<GraphicRaycaster>();

        if (raycaster == null)
            hud.AddComponent<GraphicRaycaster>();

        StoreMasterController controller =
            hud.GetComponent<StoreMasterController>();

        if (controller == null)
            controller =
                hud.AddComponent<StoreMasterController>();

        BuildFacility(facility.transform);

        EditorUtility.SetDirty(storeRoot);
        EditorUtility.SetDirty(hud);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[STORE] COMPLETE STORE BUILT.");
    }

    private static void BuildFacility(
        Transform parent)
    {
        if (parent.childCount > 0)
            return;

        GameObject title =
            new GameObject(
                "FACILITY_SIGN");

        title.transform.SetParent(
            parent,
            false);

        TextMeshPro text =
            title.AddComponent<TextMeshPro>();

        text.text =
            "MILITARY STORE\nPROCUREMENT TERMINAL";

        text.fontSize = 1.5f;
        text.alignment =
            TextAlignmentOptions.Center;
        text.color =
            new Color(
                0.80f,
                0.63f,
                0.22f,
                1f);

        RectTransform rect =
            title.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(
                8f,
                2f);

        title.transform.localPosition =
            new Vector3(
                0f,
                3f,
                4f);
    }
}
