using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class OperationsPanelFullLayoutBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/REDO OPERATIONS PANEL LAYOUT")]
    public static void Build()
    {
        GameObject panel = FindObject("OPERATIONS_PANEL");

        if (panel == null)
        {
            Debug.LogError("[OPERATIONS] OPERATIONS_PANEL not found.");
            return;
        }

        // Remove everything currently inside the panel.
        for (int i = panel.transform.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(panel.transform.GetChild(i).gameObject);
        }

        // ROOT CONTENT
        GameObject root = CreateRect(
            "OPERATIONS_CONTENT",
            panel.transform,
            Vector2.zero,
            Vector2.one,
            Vector2.zero,
            Vector2.zero
        );

        // HEADER AREA
        CreateText(
            "HEADER",
            root.transform,
            "OPERATIONS // MISSION CONTROL",
            new Vector2(.03f, .91f),
            new Vector2(.72f, .985f),
            26,
            TextAnchor.MiddleLeft
        );

        CreateText(
            "SUBHEADER",
            root.transform,
            "MISSION NETWORK: ONLINE     |     ACTIVE: 3     |     QUEUED: 2",
            new Vector2(.03f, .865f),
            new Vector2(.72f, .91f),
            12,
            TextAnchor.MiddleLeft
        );

        // EXIT BUTTON — RESERVED TOP RIGHT
        CreateButton(
            "CLOSE",
            root.transform,
            new Vector2(.88f, .91f),
            new Vector2(.97f, .985f)
        );

        // LEFT COLUMN — ACTIVE MISSIONS
        CreateCard(
            root.transform,
            "ACTIVE_MISSIONS",
            "ACTIVE MISSIONS",
            "OP-041  |  SECTOR 04 SECURITY\n" +
            "STATUS: ACTIVE       FORCE: 8\n" +
            "OBJECTIVE: HOLD POSITION\n" +
            "PROGRESS: 64%\n\n" +
            "OP-042  |  RECONNAISSANCE\n" +
            "STATUS: ACTIVE       FORCE: 5\n" +
            "OBJECTIVE: IDENTIFY CONTACTS\n" +
            "PROGRESS: 38%\n\n" +
            "OP-043  |  FORWARD RESUPPLY\n" +
            "STATUS: ACTIVE       FORCE: 3\n" +
            "OBJECTIVE: SUPPLY DEPOT 02\n" +
            "PROGRESS: 71%",
            .03f, .49f, .48f, .85f
        );

        // RIGHT COLUMN — OPERATION STATUS
        CreateCard(
            root.transform,
            "OPERATION_STATUS",
            "OPERATION STATUS",
            "ACTIVE OPERATIONS       3\n" +
            "QUEUED OPERATIONS        2\n" +
            "COMPLETED               41\n" +
            "FAILED                   0\n" +
            "UNITS DEPLOYED          24\n" +
            "UNITS AVAILABLE         18\n" +
            "UNITS IN COMBAT          8\n" +
            "UNITS STANDBY            6\n" +
            "UNITS SUPPORT           10",
            .52f, .67f, .97f, .85f
        );

        // RIGHT MIDDLE — CURRENT OBJECTIVE
        CreateCard(
            root.transform,
            "CURRENT_OBJECTIVE",
            "CURRENT OBJECTIVE",
            "HOLD SECTOR 04\n\n" +
            "PRIMARY:\n" +
            "Maintain defensive perimeter.\n\n" +
            "SECONDARY:\n" +
            "Locate unidentified hostile force.\n\n" +
            "INTELLIGENCE CONFIDENCE: 78%\n" +
            "THREAT LEVEL: 42%",
            .52f, .49f, .97f, .65f
        );

        // LEFT LOWER — MISSION QUEUE
        GameObject queue = CreateCard(
            root.transform,
            "MISSION_QUEUE",
            "MISSION QUEUE",
            "[01] SECURE CHECKPOINT 07\n" +
            "     FORCE: 4     PRIORITY: HIGH\n\n" +
            "[02] RECON SECTOR 05\n" +
            "     FORCE: 3     PRIORITY: NORMAL\n\n" +
            "[03] RESUPPLY FORWARD DEPOT\n" +
            "     FORCE: 2     PRIORITY: NORMAL\n\n" +
            "[04] RECOVER DAMAGED UNIT\n" +
            "     FORCE: 1     PRIORITY: LOW",
            .03f, .22f, .48f, .46f
        );

        CreateButtonRow(
            queue.transform,
            new string[] { "VIEW", "PRIORITIZE", "CANCEL" }
        );

        // RIGHT LOWER — SELECTED OPERATION
        GameObject details = CreateCard(
            root.transform,
            "SELECTED_OPERATION",
            "SELECTED OPERATION",
            "OP-041 // SECTOR 04 SECURITY\n\n" +
            "COMMANDER: AUTONOMOUS\n" +
            "AUTONOMY: SUPERVISED\n" +
            "FORMATION: DEFENSIVE LINE\n" +
            "ROE: STANDARD\n" +
            "SUPPLY: 86%\n" +
            "INTELLIGENCE: 78%",
            .52f, .22f, .97f, .46f
        );

        CreateButtonRow(
            details.transform,
            new string[] { "VIEW", "CONFIGURE" }
        );

        // BOTTOM COMMAND STRIP
        GameObject controls = CreateCard(
            root.transform,
            "OPERATION_CONTROLS",
            "OPERATION CONTROLS",
            "",
            .03f, .07f, .97f, .19f
        );

        CreateButtonGrid(
            controls.transform,
            new string[]
            {
                "NEW OPERATION",
                "PAUSE ALL",
                "RESUME ALL",
                "RECALL ALL",
                "FLEET",
                "DEPLOY",
                "INTELLIGENCE",
                "LOGISTICS"
            }
        );

        // FOOTER
        CreateText(
            "FOOTER",
            root.transform,
            "OPERATIONS NETWORK: ONLINE     |     3 ACTIVE     |     2 QUEUED     |     41 COMPLETED",
            new Vector2(.03f, .015f),
            new Vector2(.85f, .055f),
            11,
            TextAnchor.MiddleLeft
        );

        EditorUtility.SetDirty(panel);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log("[OPERATIONS] CLEAN FULL LAYOUT BUILT.");
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
            new Vector2(7, 7),
            new Vector2(-7, -7)
        );

        Image image = card.AddComponent<Image>();
        image.color = new Color(.025f, .035f, .045f, .97f);

        CreateText(
            "TITLE",
            card.transform,
            title,
            new Vector2(.04f, .84f),
            new Vector2(.96f, .98f),
            16,
            TextAnchor.MiddleLeft
        );

        CreateText(
            "BODY",
            card.transform,
            body,
            new Vector2(.05f, .28f),
            new Vector2(.95f, .78f),
            12,
            TextAnchor.UpperLeft
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
            new Vector2(.96f, .22f),
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
        string[] labels)
    {
        GameObject grid = CreateRect(
            "BUTTONS",
            parent,
            new Vector2(.025f, .08f),
            new Vector2(.975f, .78f),
            Vector2.zero,
            Vector2.zero
        );

        int columns = 4;
        int rows = 2;

        for (int i = 0; i < labels.Length; i++)
        {
            int col = i % columns;
            int row = i / columns;

            float w = 1f / columns;
            float h = 1f / rows;

            CreateButton(
                labels[i],
                grid.transform,
                new Vector2(col * w, 1f - ((row + 1) * h)),
                new Vector2((col + 1) * w, 1f - (row * h))
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
        image.color = new Color(.07f, .10f, .13f, 1f);

        button.AddComponent<Button>();

        CreateText(
            "TEXT",
            button.transform,
            label,
            Vector2.zero,
            Vector2.one,
            11,
            TextAnchor.MiddleCenter
        );

        return button;
    }

    private static GameObject CreateText(
        string name,
        Transform parent,
        string value,
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
        text.text = value;
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
