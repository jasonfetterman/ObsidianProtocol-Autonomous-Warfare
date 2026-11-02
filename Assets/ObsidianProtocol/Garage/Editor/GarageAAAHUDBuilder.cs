using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class GarageAAAHUDBuilder
{
    private const string TestScene =
        "Assets/Scenes/SCN04_GARAGE/SCN04_GARAGE_ARCHITECTURE_TEST.unity";
    private const string OverlayName = "[AAA] GARAGE COMMAND OVERLAY";
    private const string WorkspaceName = "CENTRAL PANEL WORKSPACE";

    private static Font UiFont =>
        Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

    [MenuItem("Obsidian Protocol/Garage/Build AAA HUD (TEST SCENE ONLY)")]
    public static void Build() => Rebuild();

    [MenuItem("Obsidian Protocol/Garage/Rebuild AAA HUD (TEST SCENE ONLY)")]
    public static void Rebuild()
    {
        var scene = EditorSceneManager.GetActiveScene();

        if (scene.path != TestScene)
        {
            Debug.LogError("[AAA HUD] Open SCN04_GARAGE_ARCHITECTURE_TEST.unity first. Nothing changed.");
            return;
        }

        var hud = GameObject.Find("GARAGE_HUD");
        if (hud == null)
        {
            Debug.LogError("[AAA HUD] GARAGE_HUD not found. Nothing changed.");
            return;
        }

        // Keep the existing navigation buttons and their listeners intact.
        // Only the existing panel roots are moved into the central workspace.
        var oldOverlay = GameObject.Find(OverlayName);
        if (oldOverlay != null)
            Undo.DestroyObjectImmediate(oldOverlay);

        BuildInterface(hud.transform);
        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("[AAA HUD] Clean layout built in TEST scene. Inspect before saving.");
    }

    private static void BuildInterface(Transform hud)
    {
        var root = new GameObject(OverlayName,
            typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler),
            typeof(GraphicRaycaster));
        Undo.RegisterCreatedObjectUndo(root, "Build Clean AAA Garage HUD");

        var canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 101;

        var scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        // Use a separate, opaque viewport so old HUD elements cannot show
        // through the new command layout.
        Box(root.transform, "VIEWPORT BACKPLATE",
            .225f, .135f, .775f, .895f, new Color(.018f, .028f, .037f, 1f),
            false);

        BuildTopBar(root.transform);
        BuildFleetRail(root.transform);
        BuildSystemsRail(root.transform);
        BuildDock(root.transform, hud);
        BuildViewportLabels(root.transform);
        BuildWorkspace(root.transform, hud);
    }

    private static void BuildTopBar(Transform root)
    {
        Box(root, "TOP COMMAND BAR", 0, .915f, 1, 1,
            new Color(.012f, .021f, .029f, 1f), false);
        Box(root, "TOP CYAN KEYLINE", 0, .91f, 1, .915f,
            new Color(.02f, .62f, .76f, 1f), false);

        Label(root, "BRAND", "OBSIDIAN  /  PROTOCOL", 24,
            TextAnchor.MiddleLeft, .025f, .935f, .30f, .99f);
        Label(root, "LOCATION", "GARAGE COMMAND     /     MOTOR POOL", 13,
            TextAnchor.MiddleLeft, .31f, .935f, .55f, .99f);
        Label(root, "FACILITY STATUS", "FACILITY 01  /  ONLINE", 13,
            TextAnchor.MiddleCenter, .57f, .935f, .72f, .99f);
        Label(root, "NETWORK STATUS", "COMMAND LINK  /  READY", 13,
            TextAnchor.MiddleCenter, .73f, .935f, .87f, .99f);
        Label(root, "ALERT STATUS", "ALERTS  /  --", 13,
            TextAnchor.MiddleCenter, .88f, .935f, .98f, .99f);
    }

    private static void BuildFleetRail(Transform root)
    {
        Box(root, "FLEET RAIL", .018f, .135f, .215f, .895f,
            new Color(.018f, .031f, .041f, 1f), false);
        Box(root, "FLEET RAIL HEADER", .018f, .835f, .215f, .895f,
            new Color(.035f, .071f, .088f, 1f), false);
        Label(root, "FLEET TITLE", "FORCE ROSTER", 18,
            TextAnchor.MiddleLeft, .032f, .855f, .20f, .89f);
        Label(root, "FLEET SUBTITLE", "GARAGE ASSET ACCESS", 11,
            TextAnchor.MiddleLeft, .032f, .825f, .20f, .85f);

        string[] names = {
            "FLEET", "STORE", "REPAIR", "CUSTOMIZE",
            "EQUIPMENT", "UPGRADE", "AI CORE", "FABRICATION",
            "STORAGE", "SALVAGE", "DEPLOYMENT", "COMMAND CENTER"
        };

        for (int i = 0; i < names.Length; i++)
        {
            float top = .79f - i * .052f;
            float bottom = top - .043f;
            Box(root, "FLEET ITEM BACK " + i, .028f, bottom, .205f, top,
                new Color(.027f, .047f, .059f, 1f), false);
            Box(root, "FLEET ITEM EDGE " + i, .028f, bottom, .031f, top,
                new Color(.02f, .42f, .53f, 1f), false);
            Label(root, "FLEET ITEM " + i, names[i], 12,
                TextAnchor.MiddleLeft, .039f, bottom, .20f, top);
        }

        Label(root, "FLEET FOOTNOTE",
            "Choose a system from the dock below.",
            10, TextAnchor.LowerLeft, .032f, .145f, .20f, .18f);
    }

    private static void BuildSystemsRail(Transform root)
    {
        Box(root, "SYSTEMS RAIL", .785f, .135f, .982f, .895f,
            new Color(.018f, .031f, .041f, 1f), false);
        Box(root, "SYSTEMS RAIL HEADER", .785f, .835f, .982f, .895f,
            new Color(.035f, .071f, .088f, 1f), false);
        Label(root, "SYSTEMS TITLE", "SYSTEMS", 18,
            TextAnchor.MiddleLeft, .80f, .855f, .97f, .89f);
        Label(root, "SYSTEMS SUBTITLE", "STATUS & DIAGNOSTICS", 11,
            TextAnchor.MiddleLeft, .80f, .825f, .97f, .85f);

        Label(root, "SELECTION CAPTION", "CURRENT SELECTION", 11,
            TextAnchor.MiddleLeft, .80f, .765f, .97f, .80f);
        Box(root, "SELECTION BACKPLATE", .80f, .705f, .97f, .755f,
            new Color(.028f, .051f, .064f, 1f), false);
        Label(root, "SELECTION VALUE", "No unit selected", 12,
            TextAnchor.MiddleLeft, .81f, .71f, .96f, .75f);

        Box(root, "SYSTEMS DIVIDER", .80f, .69f, .97f, .693f,
            new Color(.02f, .42f, .53f, 1f), false);
        Label(root, "DATA NOTICE TITLE", "DATA CONNECTION", 12,
            TextAnchor.MiddleLeft, .80f, .64f, .97f, .68f);
        Label(root, "DATA NOTICE",
            "Live vehicle telemetry is not connected to this display.\n\nOpen an existing Garage panel to view its available records.",
            11, TextAnchor.UpperLeft, .80f, .48f, .97f, .63f);
        Label(root, "SYSTEMS FOOTNOTE", "DISPLAY STATUS  /  STATIC",
            10, TextAnchor.LowerLeft, .80f, .15f, .97f, .19f);
    }

    private static void BuildDock(Transform root, Transform hud)
    {
        Box(root, "BOTTOM NAVIGATION DOCK", .225f, .015f, .775f, .115f,
            new Color(.012f, .022f, .030f, 1f), false);
        Box(root, "DOCK KEYLINE", .225f, .11f, .775f, .115f,
            new Color(.02f, .62f, .76f, 1f), false);

        string[] ids = {
            "FLEET", "STORE", "REPAIR", "CUSTOMIZE",
            "EQUIPMENT", "UPGRADE", "AI_CORE", "FABRICATION",
            "STORAGE", "SALVAGE", "DEPLOYMENT", "COMMAND_CENTER"
        };
        string[] captions = {
            "FLEET", "STORE", "MAINT.", "CUSTOM",
            "EQUIP", "UPGRADES", "AI CORE", "FAB",
            "STORAGE", "SALVAGE", "DEPLOY", "COMMAND"
        };

        float cell = 1f / ids.Length;
        for (int i = 0; i < ids.Length; i++)
        {
            string oldName = "BUTTON_" + ids[i];
            var oldButtonObject = FindChildRecursive(hud, oldName);
            var oldButton = oldButtonObject != null
                ? oldButtonObject.GetComponent<Button>() : null;

            var go = new GameObject("AAA NAV " + ids[i],
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            go.transform.SetParent(root, false);

            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(.225f + i * .046f + .001f, .035f);
            rt.anchorMax = new Vector2(.225f + (i + 1) * .046f - .001f, .095f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            var image = go.GetComponent<Image>();
            image.color = new Color(.027f, .061f, .076f, 1f);

            var button = go.GetComponent<Button>();
            button.targetGraphic = image;
            var colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(.15f, .48f, .58f, 1f);
            colors.pressedColor = new Color(.04f, .29f, .38f, 1f);
            button.colors = colors;

            if (oldButton != null)
            {
                // Proxy to the original button's existing listeners without
                // moving or replacing that original button.
                Button captured = oldButton;
                button.onClick.AddListener(() => captured.onClick.Invoke());
            }
            else
            {
                Debug.LogWarning("[AAA HUD] Original button not found: " + oldName);
            }

            Label(go.transform, "LABEL", captions[i], 10,
                TextAnchor.MiddleCenter, .02f, .02f, .98f, .98f);
        }
    }

    private static void BuildViewportLabels(Transform root)
    {
        Label(root, "VIEWPORT TITLE", "GARAGE BAY", 14,
            TextAnchor.MiddleLeft, .24f, .86f, .42f, .89f);
        Label(root, "VIEWPORT SUBTITLE", "FACILITY VIEW  /  COMMAND WORKSPACE", 10,
            TextAnchor.MiddleLeft, .24f, .835f, .55f, .86f);
        Label(root, "VIEWPORT HINT",
            "Select a Garage system from the navigation dock.",
            12, TextAnchor.MiddleCenter, .28f, .15f, .72f, .19f);
    }

    private static void BuildWorkspace(Transform root, Transform hud)
    {
        var workspace = new GameObject(WorkspaceName, typeof(RectTransform));
        workspace.transform.SetParent(root, false);
        var wrt = workspace.GetComponent<RectTransform>();
        wrt.anchorMin = new Vector2(.235f, .205f);
        wrt.anchorMax = new Vector2(.765f, .825f);
        wrt.offsetMin = Vector2.zero;
        wrt.offsetMax = Vector2.zero;

        string[] panelNames = {
            "FleetPanel", "StorePanel", "RepairPanel", "CustomizePanel",
            "EquipmentPanel", "UpgradePanel", "AICorePanel", "FabricationPanel",
            "StoragePanel", "SalvagePanel", "DeploymentPanel", "CommandCenterPanel"
        };

        foreach (string panelName in panelNames)
        {
            Transform panel = FindChildRecursive(hud, panelName);
            if (panel == null || panel == workspace.transform)
            {
                Debug.LogWarning("[AAA HUD] Panel not found: " + panelName);
                continue;
            }

            var rt = panel.GetComponent<RectTransform>();
            if (rt == null)
                continue;

            Undo.RecordObject(rt, "Place Garage panel in clean workspace");
            panel.SetParent(workspace.transform, false);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(.5f, .5f);
            rt.offsetMin = new Vector2(12f, 12f);
            rt.offsetMax = new Vector2(-12f, -12f);
            rt.localScale = Vector3.one;
            EditorUtility.SetDirty(rt);
        }
    }

    private static Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;
            var found = FindChildRecursive(child, name);
            if (found != null)
                return found;
        }
        return null;
    }

    private static Image Box(Transform parent, string name,
        float xMin, float yMin, float xMax, float yMax, Color color, bool raycast)
    {
        var go = new GameObject(name,
            typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(xMin, yMin);
        rt.anchorMax = new Vector2(xMax, yMax);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        var image = go.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = raycast;
        return image;
    }

    private static Text Label(Transform parent, string name, string value, int size,
        TextAnchor alignment, float xMin, float yMin, float xMax, float yMax)
    {
        var go = new GameObject(name,
            typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(xMin, yMin);
        rt.anchorMax = new Vector2(xMax, yMax);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var text = go.GetComponent<Text>();
        text.font = UiFont;
        text.fontSize = size;
        text.fontStyle = FontStyle.Bold;
        text.alignment = alignment;
        text.color = new Color(.80f, .91f, .96f, 1f);
        text.text = value;
        text.raycastTarget = false;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }
}
