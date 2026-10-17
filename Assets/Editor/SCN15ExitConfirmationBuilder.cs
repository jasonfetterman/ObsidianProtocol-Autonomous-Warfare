using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using ObsidianProtocol.UI;
using ObsidianProtocol.UI.Popups.System;

public static class SCN15ExitConfirmationBuilder
{
    private const string MenuPath =
        "Obsidian Protocol/SCN-15/BUILD EXIT CONFIRMATION";

    [MenuItem(MenuPath)]
    public static void Build()
    {
        string scenePath = FindScene();

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("[SCN-15] Exit Confirmation scene not found.");
            return;
        }

        Scene scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        // ------------------------------------------------------------
        // CLEAN EXISTING SCN-15 CONTENT
        // ------------------------------------------------------------

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Object.DestroyImmediate(root);
        }

        // ------------------------------------------------------------
        // CAMERA
        // ------------------------------------------------------------

        GameObject cameraGO = new GameObject(
            "MAIN CAMERA",
            typeof(Camera),
            typeof(AudioListener)
        );

        Camera mainCamera = cameraGO.GetComponent<Camera>();
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = new Color(0.015f, 0.02f, 0.027f, 1f);
        mainCamera.depth = -100f;
        mainCamera.nearClipPlane = 0.01f;
        mainCamera.farClipPlane = 1000f;

        cameraGO.tag = "MainCamera";

        // ------------------------------------------------------------
        // EVENT SYSTEM
        // ------------------------------------------------------------

        GameObject eventSystem = new GameObject(
            "EVENT SYSTEM",
            typeof(EventSystem),
            typeof(StandaloneInputModule)
        );

        // ------------------------------------------------------------
        // CANVAS
        // ------------------------------------------------------------

        GameObject canvasGO = new GameObject(
            "EXIT CONFIRMATION HUD",
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster)
        );

        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // ------------------------------------------------------------
        // BACKGROUND
        // ------------------------------------------------------------

        GameObject background = CreatePanel(
            "BACKGROUND",
            canvasGO.transform,
            Vector2.zero,
            new Vector2(1920, 1080)
        );

        Image bgImage = background.GetComponent<Image>();
        bgImage.color = new Color(0.015f, 0.02f, 0.027f, 1f);

        // ------------------------------------------------------------
        // DIM OVERLAY
        // ------------------------------------------------------------

        GameObject overlay = CreatePanel(
            "DIM OVERLAY",
            canvasGO.transform,
            Vector2.zero,
            new Vector2(1920, 1080)
        );

        Image overlayImage = overlay.GetComponent<Image>();
        overlayImage.color = new Color(0f, 0f, 0f, 0.62f);

        // ------------------------------------------------------------
        // CONFIRMATION WINDOW
        // ------------------------------------------------------------

        GameObject window = CreatePanel(
            "EXIT CONFIRMATION WINDOW",
            canvasGO.transform,
            Vector2.zero,
            new Vector2(900, 500)
        );

        Image windowImage = window.GetComponent<Image>();
        windowImage.color = new Color(0.055f, 0.065f, 0.08f, 0.98f);

        // ------------------------------------------------------------
        // TOP LINE
        // ------------------------------------------------------------

        GameObject topLine = CreatePanel(
            "HEADER ACCENT",
            window.transform,
            new Vector2(0, 220),
            new Vector2(900, 5)
        );

        topLine.GetComponent<Image>().color =
            new Color(0.8f, 0.12f, 0.12f, 1f);

        // ------------------------------------------------------------
        // TITLE
        // ------------------------------------------------------------

        TMP_Text title = CreateText(
            "TITLE",
            window.transform,
            "EXIT CONFIRMATION",
            34,
            new Vector2(0, 150),
            new Vector2(760, 70)
        );

        title.alignment = TextAlignmentOptions.Center;
        title.fontStyle = FontStyles.Bold;

        // ------------------------------------------------------------
        // STATUS LABEL
        // ------------------------------------------------------------

        TMP_Text status = CreateText(
            "STATUS",
            window.transform,
            "COMMAND TERMINATION REQUEST",
            16,
            new Vector2(0, 105),
            new Vector2(760, 40)
        );

        status.alignment = TextAlignmentOptions.Center;

        // ------------------------------------------------------------
        // MESSAGE
        // ------------------------------------------------------------

        TMP_Text message = CreateText(
            "CONFIRMATION MESSAGE",
            window.transform,
            "Are you sure you want to exit Obsidian Protocol?",
            24,
            new Vector2(0, 25),
            new Vector2(760, 90)
        );

        message.alignment = TextAlignmentOptions.Center;

        // ------------------------------------------------------------
        // WARNING
        // ------------------------------------------------------------

        TMP_Text warning = CreateText(
            "WARNING",
            window.transform,
            "Unsaved command state may be lost.",
            16,
            new Vector2(0, -65),
            new Vector2(760, 50)
        );

        warning.alignment = TextAlignmentOptions.Center;

        // ------------------------------------------------------------
        // BUTTONS
        // ------------------------------------------------------------

        GameObject cancelButton = CreateButton(
            "CANCEL",
            window.transform,
            "CANCEL",
            new Vector2(-190, -165),
            new Vector2(300, 75)
        );

        GameObject confirmButton = CreateButton(
            "CONFIRM EXIT",
            window.transform,
            "EXIT",
            new Vector2(190, -165),
            new Vector2(300, 75)
        );

        // ------------------------------------------------------------
        // POP-024
        // ------------------------------------------------------------

        GameObject controller = new GameObject(
            "POP-024 EXIT CONFIRMATION"
        );

        POP024_ExitConfirmation popup =
            controller.AddComponent<POP024_ExitConfirmation>();

        popup.SetMessage(message);

        // ------------------------------------------------------------
        // UI CONFIRMATION SYSTEM
        // ------------------------------------------------------------

        UIConfirmationSystem confirmationSystem =
            controller.AddComponent<UIConfirmationSystem>();

        SerializedObject so =
            new SerializedObject(confirmationSystem);

        so.FindProperty("confirmationRoot").objectReferenceValue = window;
        so.FindProperty("confirmationText").objectReferenceValue = message;

        so.ApplyModifiedPropertiesWithoutUndo();

        // ------------------------------------------------------------
        // BUTTON EVENTS
        // ------------------------------------------------------------

        Button cancel = cancelButton.GetComponent<Button>();
        Button confirm = confirmButton.GetComponent<Button>();

        cancel.onClick.AddListener(() =>
        {
            popup.CancelExit();
        });

        confirm.onClick.AddListener(() =>
        {
            popup.ConfirmExit();
        });

        // ------------------------------------------------------------
        // INITIALIZE
        // ------------------------------------------------------------

        EditorUtility.SetDirty(controller);
        EditorUtility.SetDirty(canvasGO);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[SCN-15] EXIT CONFIRMATION BUILD COMPLETE.");
    }

    private static string FindScene()
    {
        string[] guids = AssetDatabase.FindAssets(
            "Exit_Confirmation t:Scene"
        );

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (path.Contains("EXIT CONFIRMATION"))
                return path;
        }

        return null;
    }

    private static GameObject CreatePanel(
        string name,
        Transform parent,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image)
        );

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        return go;
    }

    private static TMP_Text CreateText(
        string name,
        Transform parent,
        string text,
        float size,
        Vector2 position,
        Vector2 dimensions)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(TextMeshProUGUI)
        );

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = position;
        rt.sizeDelta = dimensions;

        TMP_Text tmp = go.GetComponent<TextMeshProUGUI>();

        // Always assign the project's default TMP font asset.
        if (TMP_Settings.defaultFontAsset != null)
            tmp.font = TMP_Settings.defaultFontAsset;

        tmp.text = text;
        tmp.fontSize = size;
        tmp.color = Color.white;
        tmp.textWrappingMode = TMPro.TextWrappingModes.Normal;
        tmp.raycastTarget = false;

        return tmp;
    }

    private static GameObject CreateButton(
        string name,
        Transform parent,
        string label,
        Vector2 position,
        Vector2 size)
    {
        GameObject buttonGO = CreatePanel(
            name,
            parent,
            position,
            size
        );

        Image image = buttonGO.GetComponent<Image>();
        image.color = new Color(0.10f, 0.12f, 0.15f, 1f);

        Button button = buttonGO.AddComponent<Button>();
        button.targetGraphic = image;

        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.10f, 0.12f, 0.15f, 1f);
        colors.highlightedColor = new Color(0.18f, 0.22f, 0.27f, 1f);
        colors.pressedColor = new Color(0.25f, 0.29f, 0.34f, 1f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;

        TMP_Text text = CreateText(
            "LABEL",
            buttonGO.transform,
            label,
            20,
            Vector2.zero,
            size
        );

        text.alignment = TextAlignmentOptions.Center;
        text.fontStyle = FontStyles.Bold;

        return buttonGO;
    }
}




