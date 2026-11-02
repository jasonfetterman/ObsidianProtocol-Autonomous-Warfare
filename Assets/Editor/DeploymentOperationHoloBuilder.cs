using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class DeploymentOperationHoloBuilder
{
    private const string LEFT_NAME = "[HOLO] OPERATION SELECTION - OPERATION";
    private const string RIGHT_NAME = "[HOLO] OPERATION SELECTION - THEATER";

    private static readonly Color Cyan = new Color32(0, 220, 255, 255);
    private static readonly Color CyanSoft = new Color32(0, 220, 255, 85);
    private static readonly Color White = new Color32(210, 250, 255, 230);

    [MenuItem("Obsidian Protocol/Deployment/Build Operation Selection Holo Screens")]
    public static void Build()
    {
        RemoveExistingHoloOnly();

        CreateHoloScreen(
            LEFT_NAME,
            new Vector3(-24.15f, 7.00f, 21.825f),
            new Vector3(270f, 180f, 0f),
            "OPERATION CONTROL",
            true
        );

        CreateHoloScreen(
            RIGHT_NAME,
            new Vector3(-30.35f, 7.00f, 21.825f),
            new Vector3(270f, 180f, 0f),
            "THEATER OVERVIEW",
            false
        );

        Debug.Log("[HOLO] Operation Selection holo screens created.");
    }

    private static void RemoveExistingHoloOnly()
    {
        GameObject[] all = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in all)
        {
            if (go == null)
                continue;

            if (go.name != LEFT_NAME && go.name != RIGHT_NAME)
                continue;

            if (!go.scene.IsValid())
                continue;

            Object.DestroyImmediate(go);
        }
    }

    private static GameObject CreateHoloScreen(
        string objectName,
        Vector3 position,
        Vector3 rotation,
        string title,
        bool operationScreen)
    {
        GameObject root = new GameObject(objectName);

        root.transform.position = position;
        root.transform.eulerAngles = rotation;
        root.transform.localScale = Vector3.one;

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = 50;

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 100;

        RectTransform canvasRect = root.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(580f, 240f);
        canvasRect.localScale = Vector3.one * 0.01f;

        CreatePanel(root.transform, "HOLO GLASS", Vector2.zero,
            new Vector2(580, 240),
            new Color(0.005f, 0.025f, 0.035f, 0.52f));

        CreateBorder(root.transform);

        CreateText(root.transform, "TITLE", title,
            new Vector2(0, 94), new Vector2(530, 32),
            20, Cyan, TextAnchor.MiddleLeft);

        CreateText(root.transform, "SYSTEM",
            "OBSIDIAN PROTOCOL // DEPLOYMENT SYSTEM",
            new Vector2(0, 70), new Vector2(530, 18),
            9, CyanSoft, TextAnchor.MiddleLeft);

        CreateScanLines(root.transform);
        CreateGrid(root.transform);

        if (operationScreen)
            BuildOperationContent(root.transform);
        else
            BuildTheaterContent(root.transform);

        CreateText(root.transform, "FOOTER",
            operationScreen
                ? "OPERATION DATABASE // LIVE"
                : "THEATER DATA // SYNCHRONIZED",
            new Vector2(0, -106), new Vector2(530, 16),
            8, CyanSoft, TextAnchor.MiddleLeft);

        return root;
    }

    private static void BuildOperationContent(Transform parent)
    {
        CreateSection(parent, "AVAILABLE OPERATIONS",
            -105, 44, 248, 82);

        CreateText(parent, "OPERATION 01",
            "OP-07 // VERDANT SPEAR",
            new Vector2(-215, 45), new Vector2(205, 20),
            12, White, TextAnchor.MiddleLeft);

        CreateText(parent, "LOCATION",
            "VIRELLIA // VERDANT CROWN",
            new Vector2(-215, 23), new Vector2(205, 17),
            9, CyanSoft, TextAnchor.MiddleLeft);

        CreateText(parent, "OBJECTIVE",
            "SECURE STRATEGIC ZONE",
            new Vector2(-215, 2), new Vector2(205, 17),
            9, CyanSoft, TextAnchor.MiddleLeft);

        CreateText(parent, "THREAT",
            "ELEVATED",
            new Vector2(-215, -20), new Vector2(100, 17),
            9, new Color32(255, 190, 40, 220),
            TextAnchor.MiddleLeft);

        CreateText(parent, "STATUS",
            "AVAILABLE",
            new Vector2(-110, -20), new Vector2(100, 17),
            9, new Color32(57, 255, 136, 220),
            TextAnchor.MiddleLeft);

        CreateButtonVisual(parent, "SELECT OPERATION",
            new Vector2(145, -42), new Vector2(190, 34));

        CreateText(parent, "SELECT",
            "SELECT OPERATION",
            new Vector2(145, -42), new Vector2(180, 28),
            11, Cyan, TextAnchor.MiddleCenter);
    }

    private static void BuildTheaterContent(Transform parent)
    {
        CreateSection(parent, "STRATEGIC THEATER",
            0, 20, 430, 82);

        CreateMap(parent);

        CreateText(parent, "SELECTED",
            "OP-07 // VERDANT SPEAR",
            new Vector2(-115, -45), new Vector2(210, 18),
            10, White, TextAnchor.MiddleLeft);

        CreateText(parent, "THEATER",
            "VIRELLIA // VERDANT CROWN",
            new Vector2(115, -45), new Vector2(210, 18),
            10, CyanSoft, TextAnchor.MiddleRight);
    }

    private static void CreateMap(Transform parent)
    {
        CreatePanel(parent, "TACTICAL MAP",
            new Vector2(0, 0), new Vector2(430, 82),
            new Color(0.0f, 0.035f, 0.045f, 0.34f));

        CreateLine(parent,
            new Vector2(-180, -18),
            new Vector2(165, 18),
            2, CyanSoft);

        CreatePanel(parent, "FRIENDLY ZONE",
            new Vector2(-145, -12),
            new Vector2(70, 34),
            new Color(0.0f, 0.8f, 1f, 0.08f));

        CreateMarker(parent, new Vector2(80, 15),
            Cyan, "OBJECTIVE");

        CreateMarker(parent, new Vector2(145, -18),
            new Color32(255, 90, 80, 220), "THREAT");

        CreateMarker(parent, new Vector2(105, 34),
            new Color32(255, 190, 40, 210), "THREAT");

        CreateText(parent, "ROUTE",
            "DEPLOYMENT ROUTE",
            new Vector2(0, -24), new Vector2(170, 16),
            8, CyanSoft, TextAnchor.MiddleCenter);
    }

    private static void CreateLine(
        Transform parent,
        Vector2 start,
        Vector2 end,
        float thickness,
        Color color)
    {
        Vector2 midpoint = (start + end) * 0.5f;
        Vector2 direction = end - start;

        GameObject line = CreatePanel(
            parent,
            "STRATEGIC ROUTE",
            midpoint,
            new Vector2(direction.magnitude, thickness),
            color);

        line.GetComponent<RectTransform>().localEulerAngles =
            new Vector3(
                0,
                0,
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    private static void CreateSection(
        Transform parent,
        string label,
        float x,
        float y,
        float width,
        float height)
    {
        CreatePanel(parent, label + " PANEL",
            new Vector2(x, y),
            new Vector2(width, height),
            new Color(0.0f, 0.05f, 0.065f, 0.34f));

        CreateText(parent, label, label,
            new Vector2(x, y + height / 2f - 11),
            new Vector2(width - 20, 16),
            8, Cyan, TextAnchor.MiddleLeft);
    }

    private static void CreateMarker(
        Transform parent,
        Vector2 position,
        Color color,
        string label)
    {
        CreatePanel(parent, label + " MARKER",
            position, new Vector2(12, 12), color);

        CreateText(parent, label + " LABEL",
            label, position + new Vector2(28, 0),
            new Vector2(75, 14),
            7, color, TextAnchor.MiddleLeft);
    }

    private static void CreateButtonVisual(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size)
    {
        CreatePanel(parent, name + " FRAME",
            position, size,
            new Color(0.0f, 0.55f, 0.7f, 0.10f));

        CreateBorder(parent, name + " BORDER",
            position, size, CyanSoft);
    }

    private static void CreateGrid(Transform parent)
    {
        for (int x = -250; x <= 250; x += 50)
        {
            CreatePanel(parent, "GRID VERTICAL",
                new Vector2(x, 0),
                new Vector2(1, 190),
                new Color(0.0f, 0.7f, 0.9f, 0.025f));
        }

        for (int y = -90; y <= 90; y += 30)
        {
            CreatePanel(parent, "GRID HORIZONTAL",
                new Vector2(0, y),
                new Vector2(530, 1),
                new Color(0.0f, 0.7f, 0.9f, 0.025f));
        }
    }

    private static void CreateScanLines(Transform parent)
    {
        for (int y = -100; y <= 100; y += 8)
        {
            CreatePanel(parent, "SCANLINE",
                new Vector2(0, y),
                new Vector2(550, 1),
                new Color(0.0f, 0.7f, 0.9f, 0.025f));
        }
    }

    private static GameObject CreatePanel(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size,
        Color color)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = position;

        Image image = go.AddComponent<Image>();
        image.color = color;

        return go;
    }

    private static void CreateBorder(Transform parent)
    {
        CreateBorder(parent, "HOLO BORDER",
            Vector2.zero, new Vector2(570, 230), CyanSoft);
    }

    private static void CreateBorder(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size,
        Color color)
    {
        float thickness = 2f;

        CreatePanel(parent, name + " TOP",
            position + new Vector2(0, size.y / 2),
            new Vector2(size.x, thickness), color);

        CreatePanel(parent, name + " BOTTOM",
            position + new Vector2(0, -size.y / 2),
            new Vector2(size.x, thickness), color);

        CreatePanel(parent, name + " LEFT",
            position + new Vector2(-size.x / 2, 0),
            new Vector2(thickness, size.y), color);

        CreatePanel(parent, name + " RIGHT",
            position + new Vector2(size.x / 2, 0),
            new Vector2(thickness, size.y), color);
    }

    private static void CreateText(
        Transform parent,
        string name,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize,
        Color color,
        TextAnchor alignment)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.sizeDelta = size;
        rect.anchoredPosition = position;

        Text text = go.AddComponent<Text>();

        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = alignment;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
    }
}

