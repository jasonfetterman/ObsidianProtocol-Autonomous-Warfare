using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public static class CommandPanelBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity";

    [MenuItem("Obsidian Protocol/Command Center/BUILD COMMAND PANEL")]
    public static void Build()
    {
        var scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath);

        var panel = FindSceneObject("COMMAND_REAL_PANEL");
        if (panel == null)
        {
            Debug.LogError("[COMMAND PANEL] COMMAND_REAL_PANEL not found.");
            return;
        }

        var content = panel.transform.Find("CONTENT");
        if (content == null)
        {
            Debug.LogError("[COMMAND PANEL] CONTENT not found.");
            return;
        }

        var oldRoot = content.Find("COMMAND_PANEL_INTERFACE");
        if (oldRoot != null)
            Object.DestroyImmediate(oldRoot.gameObject);

        var oldText = content.GetComponent<Text>();
        if (oldText != null)
            Object.DestroyImmediate(oldText);

        var root = new GameObject("COMMAND_PANEL_INTERFACE",
            typeof(RectTransform), typeof(ScrollRect));
        root.transform.SetParent(content, false);
        Stretch(root.GetComponent<RectTransform>());

        var viewport = new GameObject("COMMAND_VIEWPORT",
            typeof(RectTransform), typeof(Image), typeof(Mask));
        viewport.transform.SetParent(root.transform, false);
        Stretch(viewport.GetComponent<RectTransform>());
        viewport.GetComponent<Image>().color = Color.white;
        viewport.GetComponent<Mask>().showMaskGraphic = false;

        var panelContent = new GameObject("COMMAND_CONTENT",
            typeof(RectTransform), typeof(VerticalLayoutGroup),
            typeof(ContentSizeFitter));
        panelContent.transform.SetParent(viewport.transform, false);

        var contentRect = panelContent.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 1);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = Vector2.zero;

        var scrollLayout = panelContent.GetComponent<VerticalLayoutGroup>();
        scrollLayout.padding = new RectOffset(14, 14, 12, 16);
        scrollLayout.spacing = 12;
        scrollLayout.childControlWidth = true;
        scrollLayout.childControlHeight = true;
        scrollLayout.childForceExpandWidth = true;
        scrollLayout.childForceExpandHeight = false;

        panelContent.GetComponent<ContentSizeFitter>().verticalFit =
            ContentSizeFitter.FitMode.PreferredSize;

        var scroll = root.GetComponent<ScrollRect>();
        scroll.viewport = viewport.GetComponent<RectTransform>();
        scroll.content = contentRect;
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;

        Header(panelContent.transform);

        var status = Section(panelContent.transform, "OPERATION STATUS");
        Label(status.transform, "ACTIVE OPERATION: NO OPERATION SELECTED");
        Label(status.transform, "COMMAND LINK: STANDBY");
        Label(status.transform, "DEPLOYED UNITS: AWAITING BATTLEFIELD CONNECTION");

        var body = new GameObject("COMMAND_WORKSPACE",
            typeof(RectTransform), typeof(HorizontalLayoutGroup),
            typeof(LayoutElement));
        body.transform.SetParent(panelContent.transform, false);

        var bodyLayout = body.GetComponent<HorizontalLayoutGroup>();
        bodyLayout.spacing = 12;
        bodyLayout.childControlWidth = true;
        bodyLayout.childControlHeight = true;
        bodyLayout.childForceExpandWidth = true;
        bodyLayout.childForceExpandHeight = true;
        body.GetComponent<LayoutElement>().preferredHeight = 270;

        var units = Section(body.transform, "DEPLOYED UNITS");
        units.AddComponent<LayoutElement>().flexibleWidth = 1;
        Label(units.transform, "Unit designation");
        Label(units.transform, "Unit classification");
        Label(units.transform, "Operational status");
        Label(units.transform, "Location");
        Label(units.transform, "Selection");

        var selected = Section(body.transform, "SELECTED UNIT");
        selected.AddComponent<LayoutElement>().flexibleWidth = 1;
        Label(selected.transform, "DESIGNATION: NONE");
        Label(selected.transform, "UNIT TYPE: NONE");
        Label(selected.transform, "HEALTH: --");
        Label(selected.transform, "BATTERY: --");
        Label(selected.transform, "CURRENT ORDER: NONE");
        Label(selected.transform, "ASSIGNMENT: NONE");

        var orders = Section(panelContent.transform, "TACTICAL ORDERS");
        var orderGridObject = new GameObject("ORDER_BUTTONS",
            typeof(RectTransform), typeof(GridLayoutGroup),
            typeof(LayoutElement));
        orderGridObject.transform.SetParent(orders.transform, false);

        var grid = orderGridObject.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(125, 36);
        grid.spacing = new Vector2(8, 8);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 5;
        orderGridObject.GetComponent<LayoutElement>().preferredHeight = 80;

        AddButton(orderGridObject.transform, "MOVE");
        AddButton(orderGridObject.transform, "ATTACK");
        AddButton(orderGridObject.transform, "DEFEND");
        AddButton(orderGridObject.transform, "RECON");
        AddButton(orderGridObject.transform, "PATROL");
        AddButton(orderGridObject.transform, "PURSUE");
        AddButton(orderGridObject.transform, "RETREAT");
        AddButton(orderGridObject.transform, "HOLD");
        AddButton(orderGridObject.transform, "CAPTURE");
        AddButton(orderGridObject.transform, "REINFORCE");

        var targeting = Section(panelContent.transform, "TARGETING & DESTINATION");
        Label(targeting.transform, "Target designation: NOT SET");
        Label(targeting.transform, "Movement destination: NOT SET");
        Label(targeting.transform, "Patrol area: NOT SET");

        var activeOrders = Section(panelContent.transform, "ACTIVE ORDERS");
        Label(activeOrders.transform, "No active orders are currently connected.");

        var controls = Section(panelContent.transform, "COMMAND CONTROLS");
        var controlRow = new GameObject("GLOBAL_CONTROLS",
            typeof(RectTransform), typeof(HorizontalLayoutGroup));
        controlRow.transform.SetParent(controls.transform, false);

        var row = controlRow.GetComponent<HorizontalLayoutGroup>();
        row.spacing = 8;
        row.childControlWidth = true;
        row.childControlHeight = true;
        row.childForceExpandWidth = true;
        row.childForceExpandHeight = false;

        AddButton(controlRow.transform, "SELECT ALL");
        AddButton(controlRow.transform, "CLEAR SELECTION");
        AddButton(controlRow.transform, "CANCEL ORDERS");
        AddButton(controlRow.transform, "EMERGENCY RECALL");

        var log = Section(panelContent.transform, "COMMAND ACTIVITY LOG");
        Label(log.transform, "TIMESTAMP     UNIT     ORDER     STATUS");
        Label(log.transform, "Awaiting command system connection.");

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[COMMAND PANEL] Scrollable layout rebuilt successfully.");
    }

    private static GameObject FindSceneObject(string objectName)
    {
        foreach (var candidate in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (candidate.name == objectName &&
                candidate.scene.IsValid() &&
                candidate.scene.isLoaded)
                return candidate;
        }

        return null;
    }

    private static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static GameObject Section(Transform parent, string title)
    {
        var section = new GameObject(
            title.Replace(" ", "_"),
            typeof(RectTransform),
            typeof(Image),
            typeof(VerticalLayoutGroup),
            typeof(ContentSizeFitter));

        section.transform.SetParent(parent, false);
        section.GetComponent<Image>().color =
            new Color(0.025f, 0.055f, 0.08f, 0.96f);

        var layout = section.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(12, 12, 10, 10);
        layout.spacing = 7;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        section.GetComponent<ContentSizeFitter>().verticalFit =
            ContentSizeFitter.FitMode.PreferredSize;

        Label(section.transform, title, 16, true);
        return section;
    }

    private static void Header(Transform parent)
    {
        var header = Section(parent, "COMMAND_HEADER");
        Label(header.transform, "COMMAND AUTHORITY", 23, true);
        Label(header.transform,
            "OPERATIONAL COMMAND  //  UNIT CONTROL", 11);
    }

    private static void Label(
        Transform parent, string value, int size = 12, bool bold = false)
    {
        var go = new GameObject(
            "LABEL_" + parent.childCount,
            typeof(RectTransform),
            typeof(TextMeshProUGUI),
            typeof(LayoutElement));

        go.transform.SetParent(parent, false);

        var text = go.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        text.color = new Color(0.55f, 0.88f, 0.96f);
        text.textWrappingMode = TextWrappingModes.Normal;
        text.enableAutoSizing = false;

        go.GetComponent<LayoutElement>().minHeight = size + 8;
        go.GetComponent<LayoutElement>().preferredHeight = size + 10;
    }

    private static void AddButton(Transform parent, string caption)
    {
        var go = new GameObject(
            caption.Replace(" ", "_"),
            typeof(RectTransform),
            typeof(Image),
            typeof(Button),
            typeof(LayoutElement));

        go.transform.SetParent(parent, false);
        go.GetComponent<Image>().color =
            new Color(0.04f, 0.19f, 0.25f, 1f);

        var element = go.GetComponent<LayoutElement>();
        element.minHeight = 36;
        element.preferredHeight = 36;
        element.minWidth = 100;
        element.preferredWidth = 125;

        var textGo = new GameObject(
            "LABEL",
            typeof(RectTransform),
            typeof(TextMeshProUGUI));

        textGo.transform.SetParent(go.transform, false);
        Stretch(textGo.GetComponent<RectTransform>());

        var text = textGo.GetComponent<TextMeshProUGUI>();
        text.text = caption;
        text.fontSize = 11;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.textWrappingMode = TextWrappingModes.Normal;
    }
}
