using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public static class BuildEditableGarageHUD
{
    const string ScenePath =
        "Assets/Scenes/SCN20_VR_COMMAND/SCN20_VR_COMMAND.unity";

    static Font FontAsset =>
        Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

    [MenuItem("Obsidian Protocol/VR Command/Build Editable Garage HUD")]
    public static void Build()
    {
        Scene scene = EditorSceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError("Open SCN20_VR_COMMAND.unity first.");
            return;
        }

        if (GameObject.Find("[HUD] EDITABLE GARAGE HUD"))
        {
            Debug.LogWarning("Editable Garage HUD already exists.");
            return;
        }

        var root = new GameObject("[HUD] EDITABLE GARAGE HUD",
            typeof(RectTransform), typeof(Canvas),
            typeof(CanvasScaler), typeof(GraphicRaycaster));

        Undo.RegisterCreatedObjectUndo(root, "Build Editable Garage HUD");

        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 25;

        CanvasScaler scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        if (!Object.FindFirstObjectByType<EventSystem>())
        {
            var es = new GameObject("GARAGE HUD EVENT SYSTEM",
                typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(es, "Create HUD Event System");
        }

        Image header = Panel(root.transform, "HEADER",
            new Vector2(0, .89f), new Vector2(1, 1),
            new Color(.025f, .055f, .085f, .96f));

        Label(header.transform, "TITLE",
            "OBSIDIAN PROTOCOL  //  GARAGE OPERATIONS",
            26, TextAnchor.MiddleLeft,
            new Vector2(.025f, .05f), new Vector2(.72f, .95f));

        Label(header.transform, "STATUS",
            "FACILITY ONLINE  |  COMMAND LINK ACTIVE",
            18, TextAnchor.MiddleRight,
            new Vector2(.70f, .05f), new Vector2(.975f, .95f));

        Image footer = Panel(root.transform, "NAVIGATION BAR",
            new Vector2(0, 0), new Vector2(1, .105f),
            new Color(.025f, .045f, .07f, .97f));

        var window = Panel(root.transform, "GARAGE CONTENT WINDOW",
            new Vector2(.17f, .17f), new Vector2(.83f, .85f),
            new Color(.035f, .065f, .09f, .94f));

        Label(window.transform, "SECTION TITLE",
            "GARAGE FACILITY",
            30, TextAnchor.MiddleLeft,
            new Vector2(.04f, .79f), new Vector2(.96f, .96f));

        Label(window.transform, "SECTION SUBTITLE",
            "Fleet staging  /  Maintenance  /  Deployment readiness",
            18, TextAnchor.MiddleLeft,
            new Vector2(.04f, .70f), new Vector2(.96f, .79f));

        string[] sections =
        {
            "GARAGE", "COMMAND", "FLEET",
            "INTELLIGENCE", "DEPLOYMENT", "MAINTENANCE"
        };

        string[] descriptions =
        {
            "Physical facility overview\n\nFleet bays\nUnit storage\nMaintenance access\nDeployment staging",
            "Command center\nOperational overview\nFacility systems",
            "Fleet inventory\nUnit inspection\nAvailable assets",
            "Intelligence systems\nReconnaissance\nSensor status",
            "Deployment planning\nForce readiness\nMission staging",
            "Maintenance queue\nRepair status\nService readiness"
        };

        var content = new GameObject("SECTION CONTENT",
            typeof(RectTransform));
        content.transform.SetParent(window.transform, false);
        RectTransform cr = content.GetComponent<RectTransform>();
        cr.anchorMin = new Vector2(.04f, .08f);
        cr.anchorMax = new Vector2(.96f, .67f);
        cr.offsetMin = cr.offsetMax = Vector2.zero;

        Text contentText = Label(content.transform, "CONTENT TEXT",
            descriptions[0], 22, TextAnchor.UpperLeft,
            Vector2.zero, Vector2.one);
        contentText.horizontalOverflow = HorizontalWrapMode.Wrap;
        contentText.verticalOverflow = VerticalWrapMode.Overflow;

        Label(window.transform, "FOOTER HINT",
            "Select a navigation section to open its panel.",
            18, TextAnchor.MiddleLeft,
            new Vector2(.04f, .015f), new Vector2(.96f, .075f));

        float width = 1f / sections.Length;

        for (int i = 0; i < sections.Length; i++)
        {
            int index = i;
            Button b = MakeButton(footer.transform,
                "BUTTON " + sections[i], sections[i],
                new Vector2(i * width + .006f, .12f),
                new Vector2((i + 1) * width - .006f, .88f));

            b.onClick.AddListener(() =>
            {
                contentText.text = descriptions[index];
                var title = window.transform.Find("SECTION TITLE")
                    .GetComponent<Text>();
                title.text = sections[index] == "GARAGE"
                    ? "GARAGE FACILITY"
                    : sections[index] + " SYSTEMS";
            });
        }

        EditorSceneManager.MarkSceneDirty(scene);
        Selection.activeGameObject = root;
        SceneView.lastActiveSceneView?.FrameSelected();

        Debug.Log("Editable Garage HUD created. Save SCN20_VR_COMMAND.");
    }

    static Image Panel(Transform parent, string name,
        Vector2 min, Vector2 max, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        Image image = go.GetComponent<Image>();
        image.color = color;
        return image;
    }

    static Text Label(Transform parent, string name, string value,
        int size, TextAnchor alignment, Vector2 min, Vector2 max)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        Text text = go.GetComponent<Text>();
        text.font = FontAsset;
        text.fontSize = Mathf.Max(18, size);
        text.color = new Color(.75f, .9f, 1f);
        text.alignment = alignment;
        text.text = value;
        text.raycastTarget = false;
        return text;
    }

    static Button MakeButton(Transform parent, string name, string caption,
        Vector2 min, Vector2 max)
    {
        var go = new GameObject(name, typeof(RectTransform),
            typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        Image image = go.GetComponent<Image>();
        image.color = new Color(.07f, .16f, .23f, .98f);

        Button button = go.GetComponent<Button>();
        button.targetGraphic = image;
        var colors = button.colors;
        colors.normalColor = new Color(.07f, .16f, .23f, .98f);
        colors.highlightedColor = new Color(.12f, .32f, .42f, 1f);
        colors.pressedColor = new Color(.15f, .45f, .58f, 1f);
        button.colors = colors;

        Label(go.transform, "LABEL", caption, 18,
            TextAnchor.MiddleCenter, new Vector2(.02f, .02f),
            new Vector2(.98f, .98f));

        return button;
    }
}

