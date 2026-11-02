using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class SettingsVisualBuilder
{
    private const string CanvasName = "SETTINGS_CANVAS";
    private const string RootName = "SETTINGS_HUD";

    private static Font UI_FONT
    {
        get
        {
            return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
    }

    [MenuItem("Obsidian Protocol/Settings/Build Settings HUD")]
    public static void BuildSettingsHUD()
    {
        string scenePath = FindSettingsScene();

        if (string.IsNullOrEmpty(scenePath))
        {
            EditorUtility.DisplayDialog(
                "SETTINGS HUD",
                "Could not find Settings.unity under Assets.",
                "OK"
            );

            Debug.LogError(
                "[SettingsVisualBuilder] Settings.unity was not found."
            );

            return;
        }

        Debug.Log(
            "[SettingsVisualBuilder] Building:\n" + scenePath
        );

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        DeleteObject(CanvasName);
        DeleteObject(RootName);

        GameObject canvas = CreateCanvas();

        GameObject root = CreatePanel(
            RootName,
            canvas.transform,
            Vector2.zero,
            Vector2.one,
            new Color(0.008f, 0.012f, 0.018f, 1f)
        );

        BuildBackground(root.transform);
        BuildHeader(root.transform);
        BuildNavigation(root.transform);
        BuildContent(root.transform);
        BuildHUDCustomization(root.transform);
        BuildFooter(root.transform);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = root;

        Debug.Log(
            "[SettingsVisualBuilder] SETTINGS HUD BUILD COMPLETE."
        );

        EditorUtility.DisplayDialog(
            "SETTINGS HUD",
            "Settings HUD successfully built.\n\n" +
            "Scene:\n" + scenePath,
            "OK"
        );
    }

    private static string FindSettingsScene()
    {
        string[] guids = AssetDatabase.FindAssets(
            "Settings t:Scene"
        );

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (
                System.IO.Path.GetFileName(path)
                    .Equals(
                        "Settings.unity",
                        System.StringComparison.OrdinalIgnoreCase
                    )
            )
            {
                return path;
            }
        }

        guids = AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (
                System.IO.Path.GetFileName(path)
                    .Equals(
                        "Settings.unity",
                        System.StringComparison.OrdinalIgnoreCase
                    )
            )
            {
                return path;
            }
        }

        return null;
    }

    private static void DeleteObject(string objectName)
    {
        GameObject existing = GameObject.Find(objectName);

        if (existing != null)
        {
            Object.DestroyImmediate(existing);
        }
    }

    private static GameObject CreateCanvas()
    {
        GameObject canvasObject = new GameObject(
            CanvasName,
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster)
        );

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        canvas.sortingOrder = 100;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        return canvasObject;
    }

    private static void BuildBackground(Transform parent)
    {
        GameObject bg = CreatePanel(
            "BACKGROUND",
            parent,
            Vector2.zero,
            Vector2.one,
            new Color(0.008f, 0.012f, 0.018f, 1f)
        );

        CreatePanel(
            "TOP_FIELD",
            bg.transform,
            new Vector2(0f, 0.91f),
            new Vector2(1f, 1f),
            new Color(0.018f, 0.055f, 0.070f, 1f)
        );

        CreatePanel(
            "LEFT_FIELD",
            bg.transform,
            new Vector2(0.025f, 0.115f),
            new Vector2(0.275f, 0.895f),
            new Color(0.014f, 0.025f, 0.034f, 1f)
        );

        CreatePanel(
            "CONTENT_FIELD",
            bg.transform,
            new Vector2(0.295f, 0.115f),
            new Vector2(0.975f, 0.895f),
            new Color(0.010f, 0.018f, 0.025f, 1f)
        );

        CreatePanel(
            "TOP_DIVIDER",
            bg.transform,
            new Vector2(0.025f, 0.902f),
            new Vector2(0.975f, 0.906f),
            new Color(0.08f, 0.68f, 0.76f, 1f)
        );
    }

    private static void BuildHeader(Transform parent)
    {
        GameObject header = CreatePanel(
            "HEADER",
            parent,
            new Vector2(0.025f, 0.91f),
            new Vector2(0.975f, 0.99f),
            new Color(0.018f, 0.030f, 0.040f, 0.98f)
        );

        CreateText(
            "TITLE",
            header.transform,
            "20. SETTINGS / SYSTEM",
            new Vector2(0.025f, 0.38f),
            new Vector2(0.60f, 0.88f),
            30,
            new Color(0.82f, 0.94f, 0.97f, 1f),
            TextAnchor.MiddleLeft
        );

        CreateText(
            "SUBTITLE",
            header.transform,
            "OBSIDIAN PROTOCOL  //  SYSTEM CONFIGURATION",
            new Vector2(0.025f, 0.05f),
            new Vector2(0.65f, 0.38f),
            12,
            new Color(0.35f, 0.67f, 0.73f, 1f),
            TextAnchor.MiddleLeft
        );

        CreateText(
            "ONLINE",
            header.transform,
            "●  SYSTEM ONLINE",
            new Vector2(0.76f, 0.42f),
            new Vector2(0.97f, 0.83f),
            13,
            new Color(0.30f, 0.95f, 0.70f, 1f),
            TextAnchor.MiddleRight
        );

        CreateText(
            "BUILD",
            header.transform,
            "SETTINGS CONFIGURATION",
            new Vector2(0.70f, 0.08f),
            new Vector2(0.97f, 0.37f),
            10,
            new Color(0.32f, 0.49f, 0.54f, 1f),
            TextAnchor.MiddleRight
        );
    }

    private static void BuildNavigation(Transform parent)
    {
        GameObject nav = CreatePanel(
            "SETTINGS_NAVIGATION",
            parent,
            new Vector2(0.025f, 0.115f),
            new Vector2(0.275f, 0.895f),
            new Color(0.018f, 0.030f, 0.040f, 0.98f)
        );

        CreateText(
            "NAV_TITLE",
            nav.transform,
            "SETTINGS HUD",
            new Vector2(0.06f, 0.91f),
            new Vector2(0.94f, 0.985f),
            18,
            new Color(0.78f, 0.91f, 0.94f, 1f),
            TextAnchor.MiddleLeft
        );

        CreatePanel(
            "NAV_LINE",
            nav.transform,
            new Vector2(0.06f, 0.885f),
            new Vector2(0.94f, 0.89f),
            new Color(0.07f, 0.55f, 0.62f, 1f)
        );

        string[] entries =
        {
            "GAMEPLAY",
            "CONTROLS",
            "AUDIO",
            "VIDEO",
            "VR",
            "HUD",
            "ACCESSIBILITY",
            "NETWORK",
            "ACCOUNT"
        };

        for (int i = 0; i < entries.Length; i++)
        {
            float top = 0.83f - i * 0.085f;
            float bottom = top - 0.067f;

            GameObject button = CreateButton(
                entries[i],
                nav.transform,
                entries[i],
                new Vector2(0.055f, bottom),
                new Vector2(0.945f, top),
                i == 0
                    ? new Color(0.035f, 0.19f, 0.22f, 1f)
                    : new Color(0.025f, 0.045f, 0.058f, 1f)
            );

            if (i == 0)
            {
                CreatePanel(
                    "ACTIVE_INDICATOR",
                    button.transform,
                    new Vector2(0f, 0f),
                    new Vector2(0.018f, 1f),
                    new Color(0.10f, 0.82f, 0.90f, 1f)
                );
            }
        }

        CreateButton(
            "BACK",
            nav.transform,
            "BACK",
            new Vector2(0.055f, 0.045f),
            new Vector2(0.945f, 0.105f),
            new Color(0.09f, 0.025f, 0.035f, 1f)
        );

        CreateText(
            "PREVIOUS",
            nav.transform,
            "◄  PREVIOUS SCREEN",
            new Vector2(0.055f, 0.005f),
            new Vector2(0.945f, 0.042f),
            9,
            new Color(0.32f, 0.48f, 0.52f, 1f),
            TextAnchor.MiddleCenter
        );
    }

    private static void BuildContent(Transform parent)
    {
        GameObject content = CreatePanel(
            "SETTINGS_CONTENT",
            parent,
            new Vector2(0.295f, 0.115f),
            new Vector2(0.975f, 0.895f),
            new Color(0f, 0f, 0f, 0f)
        );

        CreateGeneralPanel(content.transform);
        CreateDisplayPanel(content.transform);
        CreateAudioPanel(content.transform);
        CreateControlsPanel(content.transform);
        CreateInterfacePanel(content.transform);
        CreateAccessibilityPanel(content.transform);
    }

    private static void CreateGeneralPanel(Transform parent)
    {
        GameObject panel = CreateSettingsPanel(
            parent,
            "GENERAL_SETTINGS",
            "GENERAL SETTINGS",
            new Vector2(0f, 0.72f),
            new Vector2(0.48f, 0.99f)
        );

        string[] values =
        {
            "DISPLAY",
            "AUDIO",
            "CONTROLS",
            "INTERFACE",
            "ACCESSIBILITY"
        };

        for (int i = 0; i < values.Length; i++)
        {
            CreateText(
                "GENERAL_" + i,
                panel.transform,
                "◆  " + values[i],
                new Vector2(0.055f, 0.62f - i * 0.12f),
                new Vector2(0.92f, 0.70f - i * 0.12f),
                12,
                new Color(0.52f, 0.69f, 0.72f, 1f),
                TextAnchor.MiddleLeft
            );
        }
    }

    private static void CreateDisplayPanel(Transform parent)
    {
        GameObject panel = CreateSettingsPanel(
            parent,
            "DISPLAY_SETTINGS",
            "DISPLAY SETTINGS",
            new Vector2(0.51f, 0.72f),
            new Vector2(1f, 0.99f)
        );

        CreateSetting(panel.transform, "Resolution", "1920 × 1080", 0.68f);
        CreateSetting(panel.transform, "Brightness", "75%", 0.54f);
        CreateSetting(panel.transform, "Contrast", "50%", 0.40f);
        CreateSetting(panel.transform, "HUD Scale", "100%", 0.26f);

        CreateActionButton(
            panel.transform,
            "APPLY DISPLAY SETTINGS"
        );
    }

    private static void CreateAudioPanel(Transform parent)
    {
        GameObject panel = CreateSettingsPanel(
            parent,
            "AUDIO_SETTINGS",
            "AUDIO SETTINGS",
            new Vector2(0f, 0.405f),
            new Vector2(0.48f, 0.695f)
        );

        CreateSetting(panel.transform, "Master Volume", "100%", 0.68f);
        CreateSetting(panel.transform, "Music Volume", "80%", 0.54f);
        CreateSetting(panel.transform, "Effects Volume", "90%", 0.40f);
        CreateSetting(panel.transform, "Voice Volume", "85%", 0.26f);

        CreateActionButton(
            panel.transform,
            "APPLY AUDIO SETTINGS"
        );
    }

    private static void CreateControlsPanel(Transform parent)
    {
        GameObject panel = CreateSettingsPanel(
            parent,
            "CONTROL_SETTINGS",
            "CONTROL SETTINGS",
            new Vector2(0.51f, 0.405f),
            new Vector2(1f, 0.695f)
        );

        CreateSetting(panel.transform, "Key Bindings", "CUSTOM", 0.68f);
        CreateSetting(panel.transform, "Mouse Sensitivity", "65%", 0.54f);
        CreateSetting(panel.transform, "Controller Layout", "TACTICAL", 0.40f);

        CreateText(
            "CONTROL_INFO",
            panel.transform,
            "Configure command input and tactical controls.",
            new Vector2(0.055f, 0.18f),
            new Vector2(0.55f, 0.30f),
            10,
            new Color(0.34f, 0.49f, 0.53f, 1f),
            TextAnchor.MiddleLeft
        );

        CreateActionButton(
            panel.transform,
            "APPLY CONTROL SETTINGS"
        );
    }

    private static void CreateInterfacePanel(Transform parent)
    {
        GameObject panel = CreateSettingsPanel(
            parent,
            "INTERFACE_SETTINGS",
            "INTERFACE SETTINGS",
            new Vector2(0f, 0.09f),
            new Vector2(0.48f, 0.375f)
        );

        CreateSetting(panel.transform, "Language", "ENGLISH", 0.68f);
        CreateSetting(panel.transform, "Unit Display Mode", "TACTICAL", 0.54f);
        CreateSetting(panel.transform, "Tooltip Behavior", "DETAILED", 0.40f);

        CreateActionButton(
            panel.transform,
            "APPLY INTERFACE SETTINGS"
        );
    }

    private static void CreateAccessibilityPanel(Transform parent)
    {
        GameObject panel = CreateSettingsPanel(
            parent,
            "ACCESSIBILITY_SETTINGS",
            "ACCESSIBILITY SETTINGS",
            new Vector2(0.51f, 0.09f),
            new Vector2(1f, 0.375f)
        );

        CreateSetting(panel.transform, "Colorblind Mode", "OFF", 0.68f);
        CreateSetting(panel.transform, "Text Size", "NORMAL", 0.54f);
        CreateSetting(panel.transform, "Contrast Mode", "STANDARD", 0.40f);

        CreateActionButton(
            panel.transform,
            "APPLY ACCESSIBILITY SETTINGS"
        );
    }

    private static GameObject CreateSettingsPanel(
        Transform parent,
        string name,
        string title,
        Vector2 min,
        Vector2 max)
    {
        GameObject panel = CreatePanel(
            name,
            parent,
            min,
            max,
            new Color(0.018f, 0.030f, 0.040f, 0.98f)
        );

        CreateText(
            "TITLE",
            panel.transform,
            title,
            new Vector2(0.045f, 0.84f),
            new Vector2(0.94f, 0.98f),
            15,
            new Color(0.75f, 0.90f, 0.93f, 1f),
            TextAnchor.MiddleLeft
        );

        CreatePanel(
            "DIVIDER",
            panel.transform,
            new Vector2(0.045f, 0.815f),
            new Vector2(0.955f, 0.82f),
            new Color(0.055f, 0.40f, 0.46f, 1f)
        );

        return panel;
    }

    private static void CreateSetting(
        Transform parent,
        string label,
        string value,
        float y)
    {
        GameObject row = CreatePanel(
            label.Replace(" ", "_"),
            parent,
            new Vector2(0.045f, y),
            new Vector2(0.955f, y + 0.105f),
            new Color(0.024f, 0.043f, 0.054f, 1f)
        );

        CreateText(
            "LABEL",
            row.transform,
            label,
            new Vector2(0.025f, 0.10f),
            new Vector2(0.54f, 0.90f),
            11,
            new Color(0.58f, 0.73f, 0.76f, 1f),
            TextAnchor.MiddleLeft
        );

        CreateText(
            "VALUE",
            row.transform,
            value,
            new Vector2(0.57f, 0.10f),
            new Vector2(0.94f, 0.90f),
            11,
            new Color(0.36f, 0.86f, 0.90f, 1f),
            TextAnchor.MiddleRight
        );
    }

    private static void CreateActionButton(
        Transform parent,
        string label)
    {
        CreateButton(
            label.Replace(" ", "_"),
            parent,
            label,
            new Vector2(0.59f, 0.025f),
            new Vector2(0.955f, 0.115f),
            new Color(0.035f, 0.15f, 0.17f, 1f)
        );
    }

    private static void BuildHUDCustomization(
        Transform parent)
    {
        GameObject window = CreatePanel(
            "HUD_CUSTOMIZATION",
            parent,
            new Vector2(0.025f, 0.115f),
            new Vector2(0.975f, 0.895f),
            new Color(0.018f, 0.030f, 0.040f, 0.995f)
        );

        window.SetActive(false);

        CreateText(
            "TITLE",
            window.transform,
            "HUD CUSTOMIZATION",
            new Vector2(0.025f, 0.91f),
            new Vector2(0.70f, 0.985f),
            24,
            new Color(0.82f, 0.95f, 0.97f, 1f),
            TextAnchor.MiddleLeft
        );

        CreateText(
            "SUBTITLE",
            window.transform,
            "TACTICAL INTERFACE CONFIGURATION",
            new Vector2(0.025f, 0.855f),
            new Vector2(0.70f, 0.91f),
            11,
            new Color(0.34f, 0.65f, 0.71f, 1f),
            TextAnchor.MiddleLeft
        );

        string[] categories =
        {
            "HUD LAYOUT",
            "HUD VISIBILITY",
            "INFORMATION DENSITY",
            "TACTICAL DISPLAY",
            "ALERTS",
            "UNIT INFORMATION",
            "UNIVERSAL NAVIGATION"
        };

        for (int i = 0; i < categories.Length; i++)
        {
            float top = 0.78f - i * 0.09f;

            CreateButton(
                "HUD_CATEGORY_" + i,
                window.transform,
                categories[i],
                new Vector2(0.045f, top - 0.065f),
                new Vector2(0.39f, top),
                i == 0
                    ? new Color(0.035f, 0.19f, 0.22f, 1f)
                    : new Color(0.025f, 0.045f, 0.058f, 1f)
            );
        }

        GameObject preview = CreatePanel(
            "HUD_PREVIEW",
            window.transform,
            new Vector2(0.43f, 0.18f),
            new Vector2(0.96f, 0.79f),
            new Color(0.008f, 0.015f, 0.021f, 1f)
        );

        CreateText(
            "PREVIEW_TITLE",
            preview.transform,
            "TACTICAL HUD PREVIEW",
            new Vector2(0.04f, 0.90f),
            new Vector2(0.96f, 0.98f),
            13,
            new Color(0.38f, 0.75f, 0.80f, 1f),
            TextAnchor.MiddleLeft
        );

        CreatePanel(
            "MAP",
            preview.transform,
            new Vector2(0.05f, 0.13f),
            new Vector2(0.95f, 0.86f),
            new Color(0.025f, 0.070f, 0.075f, 1f)
        );

        CreatePanel(
            "TOP_HUD",
            preview.transform,
            new Vector2(0.09f, 0.75f),
            new Vector2(0.91f, 0.84f),
            new Color(0.012f, 0.025f, 0.032f, 1f)
        );

        CreateText(
            "LINK",
            preview.transform,
            "COMMAND LINK  //  OPERATIONAL",
            new Vector2(0.12f, 0.76f),
            new Vector2(0.65f, 0.83f),
            9,
            new Color(0.35f, 0.90f, 0.70f, 1f),
            TextAnchor.MiddleLeft
        );

        CreatePanel(
            "UNIT_PANEL",
            preview.transform,
            new Vector2(0.09f, 0.19f),
            new Vector2(0.34f, 0.57f),
            new Color(0.012f, 0.025f, 0.032f, 0.97f)
        );

        CreateText(
            "UNIT_INFO",
            preview.transform,
            "UNIT STATUS\n\nHP          100%\nENERGY       84%\nARMOR        91%\nLINK        100%\nORDERS      ACTIVE",
            new Vector2(0.12f, 0.24f),
            new Vector2(0.32f, 0.53f),
            9,
            new Color(0.70f, 0.84f, 0.86f, 1f),
            TextAnchor.UpperLeft
        );

        CreatePanel(
            "COMMAND_PANEL",
            preview.transform,
            new Vector2(0.65f, 0.19f),
            new Vector2(0.91f, 0.57f),
            new Color(0.012f, 0.025f, 0.032f, 0.97f)
        );

        CreateText(
            "COMMANDS",
            preview.transform,
            "COMMANDS\n\nMOVE\nATTACK\nDEFEND\nPATROL\nHOLD",
            new Vector2(0.68f, 0.24f),
            new Vector2(0.88f, 0.53f),
            9,
            new Color(0.70f, 0.84f, 0.86f, 1f),
            TextAnchor.UpperLeft
        );

        CreateButton(
            "CLOSE_CUSTOMIZATION",
            window.transform,
            "CLOSE",
            new Vector2(0.80f, 0.045f),
            new Vector2(0.96f, 0.115f),
            new Color(0.09f, 0.025f, 0.035f, 1f)
        );
    }

    private static void BuildFooter(Transform parent)
    {
        GameObject footer = CreatePanel(
            "FOOTER",
            parent,
            new Vector2(0.025f, 0.025f),
            new Vector2(0.975f, 0.095f),
            new Color(0.018f, 0.030f, 0.040f, 0.98f)
        );

        CreateText(
            "LEFT_HINT",
            footer.transform,
            "ESC  BACK",
            new Vector2(0.02f, 0.18f),
            new Vector2(0.20f, 0.82f),
            11,
            new Color(0.36f, 0.54f, 0.58f, 1f),
            TextAnchor.MiddleLeft
        );

        CreateText(
            "CENTER",
            footer.transform,
            "SETTINGS / SYSTEM",
            new Vector2(0.35f, 0.18f),
            new Vector2(0.65f, 0.82f),
            12,
            new Color(0.44f, 0.69f, 0.73f, 1f),
            TextAnchor.MiddleCenter
        );

        CreateText(
            "RIGHT_HINT",
            footer.transform,
            "F1  HELP",
            new Vector2(0.80f, 0.18f),
            new Vector2(0.98f, 0.82f),
            11,
            new Color(0.32f, 0.49f, 0.54f, 1f),
            TextAnchor.MiddleRight
        );
    }

    private static GameObject CreatePanel(
        string name,
        Transform parent,
        Vector2 min,
        Vector2 max,
        Color color)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image)
        );

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color = color;
        image.raycastTarget = false;

        return go;
    }

    private static GameObject CreateButton(
        string name,
        Transform parent,
        string label,
        Vector2 min,
        Vector2 max,
        Color color)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        Button button =
            go.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor = color;

        colors.highlightedColor =
            new Color(
                Mathf.Min(color.r + 0.04f, 1f),
                Mathf.Min(color.g + 0.10f, 1f),
                Mathf.Min(color.b + 0.11f, 1f),
                color.a
            );

        colors.pressedColor =
            new Color(
                Mathf.Min(color.r + 0.07f, 1f),
                Mathf.Min(color.g + 0.15f, 1f),
                Mathf.Min(color.b + 0.16f, 1f),
                color.a
            );

        colors.selectedColor =
            colors.highlightedColor;

        button.colors = colors;

        CreateText(
            "LABEL",
            go.transform,
            label,
            new Vector2(0.04f, 0.04f),
            new Vector2(0.96f, 0.96f),
            11,
            new Color(0.78f, 0.91f, 0.93f, 1f),
            TextAnchor.MiddleCenter
        );

        return go;
    }

    private static GameObject CreateText(
        string name,
        Transform parent,
        string value,
        Vector2 min,
        Vector2 max,
        int fontSize,
        Color color,
        TextAnchor alignment)
    {
        GameObject go = new GameObject(
            name,
            typeof(RectTransform),
            typeof(Text)
        );

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Text text =
            go.GetComponent<Text>();

        text.text = value;
        text.font = UI_FONT;
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = alignment;
        text.horizontalOverflow =
            HorizontalWrapMode.Wrap;
        text.verticalOverflow =
            VerticalWrapMode.Overflow;
        text.raycastTarget = false;

        return go;
    }
}
