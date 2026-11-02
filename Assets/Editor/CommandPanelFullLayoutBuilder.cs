using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CommandPanelFullLayoutBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/REDO COMMAND PANEL FULL LAYOUT")]
    public static void Build()
    {
        GameObject panel = FindObject("COMMAND_PANEL");

        if (panel == null)
        {
            Debug.LogError("[COMMAND PANEL] COMMAND_PANEL not found.");
            return;
        }

        // Remove everything inside COMMAND_PANEL except CLOSE.
        for (int i = panel.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = panel.transform.GetChild(i);

            if (child.name == "CLOSE")
                continue;

            Object.DestroyImmediate(child.gameObject);
        }

        GameObject root = CreateRect(
            "COMMAND_PANEL_CONTENT",
            panel.transform,
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero
        );

        // HEADER
        CreateText(
            "HEADER",
            root.transform,
            "COMMAND // STRATEGIC CONTROL",
            new Vector2(0.03f, 0.91f),
            new Vector2(0.97f, 0.985f),
            28,
            TextAnchor.MiddleLeft
        );

        CreateText(
            "SUBHEADER",
            root.transform,
            "COMMAND NETWORK: ONLINE     |     AUTONOMY: ACTIVE     |     THEATER STATUS: OPERATIONAL",
            new Vector2(0.03f, 0.865f),
            new Vector2(0.97f, 0.91f),
            14,
            TextAnchor.MiddleLeft
        );

        // TOP ROW
        CreateCard(
            root.transform,
            "COMMAND_STATUS",
            "COMMAND STATUS",
            "ACTIVE OPERATIONS       3\n" +
            "ACTIVE UNITS            24\n" +
            "UNITS AVAILABLE         18\n" +
            "DAMAGED                  2\n" +
            "UNDER REPAIR             3\n" +
            "LOSSES                   0",
            0.03f, 0.68f, 0.32f, 0.85f
        );

        CreateCard(
            root.transform,
            "AUTONOMY_STATUS",
            "AUTONOMY STATUS",
            "AUTONOMOUS UNITS       17\n" +
            "SUPERVISED UNITS         5\n" +
            "MANUAL CONTROL           2\n" +
            "AI DIRECTIVES             6\n" +
            "PENDING DECISIONS         2\n" +
            "NETWORK              ONLINE",
            0.34f, 0.68f, 0.63f, 0.85f
        );

        CreateCard(
            root.transform,
            "STRATEGIC_OVERVIEW",
            "STRATEGIC OVERVIEW",
            "CURRENT OBJECTIVE\nHOLD SECTOR 04\n\n" +
            "PRIMARY THREAT\nUNKNOWN HOSTILE FORCE\n\n" +
            "INTELLIGENCE CONFIDENCE     78%\n" +
            "THREAT LEVEL                42%",
            0.65f, 0.68f, 0.97f, 0.85f
        );

        // MIDDLE ROW - COMMAND QUEUE
        GameObject queue = CreateCard(
            root.transform,
            "COMMAND_QUEUE",
            "COMMAND QUEUE",
            "[01] DEPLOYMENT REQUEST       02 UNITS\n" +
            "[02] REPAIR AUTHORIZATION     BULLDOG-024\n" +
            "[03] INTELLIGENCE REVIEW      SECTOR 04\n" +
            "[04] RESEARCH FUNDING         AUTONOMY IV",
            0.03f, 0.39f, 0.63f, 0.65f
        );

        CreateButtonRow(
            queue.transform,
            "QUEUE_BUTTONS",
            new string[]
            {
                "VIEW",
                "APPROVE",
                "REJECT",
                "CLEAR"
            },
            0.04f,
            0.04f,
            0.96f,
            0.27f
        );

        // MIDDLE ROW - ALERTS
        GameObject alerts = CreateCard(
            root.transform,
            "COMMAND_ALERTS",
            "COMMAND ALERTS",
            "[WARNING] 2 UNITS REQUIRE MAINTENANCE\n" +
            "[INFO] DEPLOYMENT CAPACITY AVAILABLE\n" +
            "[INFO] RESEARCH AWAITING FUNDING\n" +
            "[WARNING] SECTOR 04 INTELLIGENCE INCOMPLETE",
            0.65f, 0.39f, 0.97f, 0.65f
        );

        CreateButtonRow(
            alerts.transform,
            "ALERT_BUTTONS",
            new string[]
            {
                "VIEW",
                "DISMISS"
            },
            0.04f,
            0.04f,
            0.96f,
            0.27f
        );

        // BOTTOM QUICK COMMANDS
        GameObject quick = CreateCard(
            root.transform,
            "QUICK_COMMANDS",
            "QUICK COMMANDS",
            "",
            0.03f, 0.08f, 0.97f, 0.36f
        );

        CreateButtonGrid(
            quick.transform,
            "QUICK_COMMAND_BUTTONS",
            new string[]
            {
                "OPERATIONS",
                "FLEET",
                "DEPLOY",
                "LOGISTICS",
                "RESEARCH",
                "GARAGE",
                "INTELLIGENCE",
                "EMERGENCY RECALL"
            }
        );

        // BOTTOM STATUS
        CreateText(
            "BOTTOM_STATUS",
            root.transform,
            "COMMAND NETWORK: ONLINE     |     UNITS: 24     |     OPERATIONS: 3     |     DEPLOYMENT CAPACITY AVAILABLE     |     ALERTS: 2",
            new Vector2(0.03f, 0.015f),
            new Vector2(0.97f, 0.065f),
            13,
            TextAnchor.MiddleCenter
        );

        EditorUtility.SetDirty(panel);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log("[COMMAND PANEL] FULL DASHBOARD LAYOUT BUILT.");
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
            new Vector2(xMax, yMax),
            new Vector2(8, 8),
            new Vector2(-8, -8)
        );

        Image image = card.AddComponent<Image>();
        image.color = new Color(0.025f, 0.035f, 0.045f, 0.97f);

        CreateText(
            "TITLE",
            card.transform,
            title,
            new Vector2(0.04f, 0.80f),
            new Vector2(0.96f, 0.98f),
            17,
            TextAnchor.MiddleLeft
        );

        CreateText(
            "BODY",
            card.transform,
            body,
            new Vector2(0.05f, 0.30f),
            new Vector2(0.95f, 0.76f),
            14,
            TextAnchor.UpperLeft
        );

        return card;
    }

    private static void CreateButtonRow(
        Transform parent,
        string name,
        string[] labels,
        float xMin,
        float yMin,
        float xMax,
        float yMax)
    {
        GameObject row = CreateRect(
            name,
            parent,
            new Vector2(xMin, yMin),
            new Vector2(xMax, yMax),
            Vector2.zero,
            Vector2.zero
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
        string name,
        string[] labels)
    {
        GameObject grid = CreateRect(
            name,
            parent,
            new Vector2(0.03f, 0.12f),
            new Vector2(0.97f, 0.78f),
            Vector2.zero,
            Vector2.zero
        );

        int columns = 4;
        int rows = 2;

        for (int i = 0; i < labels.Length; i++)
        {
            int column = i % columns;
            int row = i / columns;

            float w = 1f / columns;
            float h = 1f / rows;

            CreateButton(
                labels[i],
                grid.transform,
                new Vector2(column * w, 1f - ((row + 1) * h)),
                new Vector2((column + 1) * w, 1f - (row * h))
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
            max,
            new Vector2(5, 5),
            new Vector2(-5, -5)
        );

        Image image = button.AddComponent<Image>();
        image.color = new Color(0.07f, 0.10f, 0.13f, 1f);

        button.AddComponent<Button>();

        CreateText(
            "TEXT",
            button.transform,
            label,
            Vector2.zero,
            Vector2.one,
            13,
            TextAnchor.MiddleCenter
        );

        return button;
    }

    private static GameObject CreateText(
        string name,
        Transform parent,
        string textValue,
        Vector2 min,
        Vector2 max,
        int fontSize,
        TextAnchor alignment)
    {
        GameObject obj = CreateRect(
            name,
            parent,
            min,
            max,
            Vector2.zero,
            Vector2.zero
        );

        Text text = obj.AddComponent<Text>();

        text.text = textValue;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.color = Color.white;
        text.alignment = alignment;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        return obj;
    }

    private static GameObject CreateRect(
        string name,
        Transform parent,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 offsetMin,
        Vector2 offsetMax)
    {
        GameObject obj = new GameObject(
            name,
            typeof(RectTransform)
        );

        obj.transform.SetParent(parent, false);

        RectTransform rect = obj.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;

        return obj;
    }

    private static GameObject FindObject(string objectName)
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (GameObject obj in objects)
        {
            if (obj.name == objectName)
                return obj;
        }

        return null;
    }
}
