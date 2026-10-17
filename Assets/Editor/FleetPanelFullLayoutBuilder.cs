using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class FleetPanelFullLayoutBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/BUILD FLEET PANEL")]
    public static void Build()
    {
        GameObject panel = FindObject("FLEET_PANEL");

        if (panel == null)
        {
            Debug.LogError("[FLEET] FLEET_PANEL not found.");
            return;
        }

        for (int i = panel.transform.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(panel.transform.GetChild(i).gameObject);

        GameObject root = CreateRect(
            "FLEET_CONTENT",
            panel.transform,
            Vector2.zero,
            Vector2.one
        );

        CreateText(
            "HEADER",
            root.transform,
            "FLEET // FORCE MANAGEMENT",
            new Vector2(.03f, .91f),
            new Vector2(.75f, .985f),
            26
        );

        CreateText(
            "SUBHEADER",
            root.transform,
            "FLEET NETWORK: ONLINE     |     ACTIVE: 24     |     AVAILABLE: 18",
            new Vector2(.03f, .865f),
            new Vector2(.75f, .91f),
            12
        );

        // CLOSE
        GameObject close = CreateButton(
            "CLOSE",
            root.transform,
            new Vector2(.88f, .91f),
            new Vector2(.97f, .985f)
        );

        // FORCE STATUS
        CreateCard(
            root.transform,
            "FORCE_STATUS",
            "FORCE STATUS",
            "TOTAL UNITS              42\n" +
            "DEPLOYED                 24\n" +
            "AVAILABLE                18\n" +
            "IN COMBAT                 8\n" +
            "STANDBY                   6\n" +
            "DAMAGED                   3\n" +
            "REPAIR                    2\n" +
            "LOST                      0",
            .03f, .66f, .48f, .85f
        );

        // FLEET READINESS
        CreateCard(
            root.transform,
            "FLEET_READINESS",
            "FLEET READINESS",
            "COMBAT READY             86%\n" +
            "MOBILITY                 91%\n" +
            "ARMAMENT                 83%\n" +
            "SUPPLY                   78%\n" +
            "COMMUNICATIONS           94%\n" +
            "AVERAGE READINESS        86%",
            .52f, .66f, .97f, .85f
        );

        // ACTIVE FLEET
        GameObject active = CreateCard(
            root.transform,
            "ACTIVE_FLEET",
            "ACTIVE FLEET",
            "BULLDOG-024     VETERAN II     ACTIVE\n" +
            "WARDEN-011      VETERAN I      ACTIVE\n" +
            "SENTINEL-032    STANDARD        ACTIVE\n" +
            "BEACON-018      SUPPORT         ACTIVE\n" +
            "FORGE-006       LOGISTICS       STANDBY\n" +
            "DROP-014        TRANSPORT       STANDBY",
            .03f, .39f, .48f, .63f
        );

        CreateButtonRow(
            active.transform,
            new string[] { "VIEW", "INSPECT", "RECALL" }
        );

        // AVAILABLE UNITS
        GameObject available = CreateCard(
            root.transform,
            "AVAILABLE_UNITS",
            "AVAILABLE UNITS",
            "WARDEN-021       READY\n" +
            "BEACON-024       READY\n" +
            "BULLDOG-031      READY\n" +
            "SENTINEL-041     READY\n" +
            "FORGE-012        READY\n" +
            "DROP-019         READY",
            .52f, .39f, .97f, .63f
        );

        CreateButtonRow(
            available.transform,
            new string[] { "VIEW", "SELECT", "DEPLOY" }
        );

        // UNIT CATEGORIES
        GameObject categories = CreateCard(
            root.transform,
            "UNIT_CATEGORIES",
            "UNIT CATEGORIES",
            "GROUND                 24\n" +
            "AIR                     7\n" +
            "SUPPORT                 6\n" +
            "LOGISTICS               5\n" +
            "SPECIAL                 0",
            .03f, .18f, .48f, .36f
        );

        CreateButtonRow(
            categories.transform,
            new string[] { "FILTER", "GARAGE", "RESEARCH" }
        );

        // FLEET COMMAND
        GameObject command = CreateCard(
            root.transform,
            "FLEET_COMMAND",
            "FLEET COMMAND",
            "Manage deployed forces, inspect units,\n" +
            "configure readiness, and issue fleet orders.",
            .52f, .18f, .97f, .36f
        );

        CreateButtonRow(
            command.transform,
            new string[] { "INSPECT", "CONFIGURE", "OPERATIONS" }
        );

        // BOTTOM COMMAND STRIP
        GameObject controls = CreateCard(
            root.transform,
            "FLEET_CONTROLS",
            "FLEET CONTROLS",
            "",
            .03f, .07f, .97f, .15f
        );

        CreateButtonGrid(
            controls.transform,
            new string[]
            {
                "FLEET OVERVIEW",
                "UNIT INSPECTION",
                "DEPLOY",
                "RECALL ALL"
            }
        );

        CreateText(
            "FOOTER",
            root.transform,
            "FLEET NETWORK: ONLINE     |     42 TOTAL     |     24 DEPLOYED     |     18 AVAILABLE",
            new Vector2(.03f, .015f),
            new Vector2(.85f, .05f),
            11
        );

        EditorUtility.SetDirty(panel);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log("[FLEET] FULL FLEET PANEL BUILT.");
    }

    private static GameObject CreateCard(
        Transform parent,
        string name,
        string title,
        string body,
        float xMin,
        float yMin,
        float xMax,
        float yMax)
    {
        GameObject card = CreateRect(
            name,
            parent,
            new Vector2(xMin, yMin),
            new Vector2(xMax, yMax)
        );

        Image image = card.AddComponent<Image>();
        image.color = new Color(.025f, .035f, .045f, .97f);

        CreateText(
            "TITLE",
            card.transform,
            title,
            new Vector2(.04f, .82f),
            new Vector2(.96f, .98f),
            16
        );

        CreateText(
            "BODY",
            card.transform,
            body,
            new Vector2(.05f, .27f),
            new Vector2(.95f, .76f),
            12
        );

        return card;
    }

    private static void CreateButtonRow(
        Transform parent,
        string[] labels)
    {
        GameObject row = CreateRect(
            "BUTTONS",
            parent,
            new Vector2(.04f, .035f),
            new Vector2(.96f, .22f)
        );

        float width = 1f / labels.Length;

        for (int i = 0; i < labels.Length; i++)
        {
            CreateButton(
                labels[i],
                row.transform,
                new Vector2(i * width, 0),
                new Vector2((i + 1) * width, 1)
            );
        }
    }

    private static void CreateButtonGrid(
        Transform parent,
        string[] labels)
    {
        GameObject row = CreateRect(
            "BUTTONS",
            parent,
            new Vector2(.025f, .08f),
            new Vector2(.975f, .78f)
        );

        float width = 1f / labels.Length;

        for (int i = 0; i < labels.Length; i++)
        {
            CreateButton(
                labels[i],
                row.transform,
                new Vector2(i * width, 0),
                new Vector2((i + 1) * width, 1)
            );
        }
    }

    private static GameObject CreateButton(
        string label,
        Transform parent,
        Vector2 min,
        Vector2 max)
    {
        GameObject button = CreateRect(
            label,
            parent,
            min,
            max
        );

        Image image = button.AddComponent<Image>();
        image.color = new Color(.07f, .10f, .13f, 1f);

        button.AddComponent<Button>();

        CreateText(
            "TEXT",
            button.transform,
            label,
            Vector2.zero,
            Vector2.one,
            11
        );

        return button;
    }

    private static GameObject CreateText(
        string name,
        Transform parent,
        string value,
        Vector2 min,
        Vector2 max,
        int fontSize)
    {
        GameObject obj = CreateRect(
            name,
            parent,
            min,
            max
        );

        Text text = obj.AddComponent<Text>();
        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = TextAnchor.UpperLeft;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        return obj;
    }

    private static GameObject CreateRect(
        string name,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        GameObject obj = new GameObject(
            name,
            typeof(RectTransform)
        );

        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = new Vector2(7, 7);
        rect.offsetMax = new Vector2(-7, -7);

        return obj;
    }

    private static GameObject FindObject(string name)
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (GameObject obj in objects)
        {
            if (obj.name == name)
                return obj;
        }

        return null;
    }
}
