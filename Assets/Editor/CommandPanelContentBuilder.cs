using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class CommandPanelContentBuilder
{
    [MenuItem("Obsidian Protocol/Command Center/BUILD COMMAND PANEL CONTENT")]
    public static void Build()
    {
        GameObject panel = GameObject.Find("COMMAND_PANEL");

        if (panel == null)
        {
            Debug.LogError("[COMMAND PANEL] COMMAND_PANEL not found.");
            return;
        }

        // Remove previous generated content only.
        Transform old = panel.transform.Find("COMMAND_PANEL_CONTENT");
        if (old != null)
            Object.DestroyImmediate(old.gameObject);

        GameObject content = new GameObject(
            "COMMAND_PANEL_CONTENT",
            typeof(RectTransform)
        );

        content.transform.SetParent(panel.transform, false);

        RectTransform contentRect = content.GetComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero;
        contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(35, 35);
        contentRect.offsetMax = new Vector2(-35, -85);

        CreateSection(content.transform, "COMMAND_STATUS",
            "COMMAND STATUS",
            "ACTIVE OPERATIONS     3\n" +
            "ACTIVE UNITS          24\n" +
            "UNITS AVAILABLE       18\n" +
            "UNITS DAMAGED          2\n" +
            "UNITS UNDER REPAIR     3\n" +
            "UNITS LOST             0");

        CreateSection(content.transform, "AUTONOMY_STATUS",
            "AUTONOMY STATUS",
            "AUTONOMOUS UNITS      17\n" +
            "SUPERVISED UNITS       5\n" +
            "MANUAL CONTROL         2\n" +
            "AI DIRECTIVES           6\n" +
            "PENDING DECISIONS       2\n" +
            "AUTONOMY NETWORK     ONLINE");

        CreateSection(content.transform, "STRATEGIC_OVERVIEW",
            "STRATEGIC OVERVIEW",
            "CURRENT OBJECTIVE     HOLD SECTOR 04\n" +
            "PRIMARY THREAT         UNKNOWN HOSTILE FORCE\n" +
            "FRIENDLY STRENGTH      24 UNITS\n" +
            "HOSTILE CONTACTS       11\n" +
            "INTELLIGENCE           78%\n" +
            "THREAT LEVEL           42%");

        CreateSection(content.transform, "COMMAND_QUEUE",
            "COMMAND QUEUE",
            "[01] DEPLOYMENT REQUEST       02 UNITS\n" +
            "[02] REPAIR AUTHORIZATION     BULLDOG-024\n" +
            "[03] INTELLIGENCE REVIEW      SECTOR 04\n" +
            "[04] RESEARCH FUNDING         AUTONOMY IV");

        CreateSection(content.transform, "COMMAND_ALERTS",
            "COMMAND ALERTS",
            "[WARNING] 2 UNITS REQUIRE MAINTENANCE\n" +
            "[INFO] DEPLOYMENT CAPACITY AVAILABLE\n" +
            "[INFO] RESEARCH PROJECT AWAITING FUNDING\n" +
            "[WARNING] SECTOR 04 INTELLIGENCE INCOMPLETE");

        EditorUtility.SetDirty(panel);
        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log("[COMMAND PANEL] CONTENT BUILT.");
    }

    private static void CreateSection(
        Transform parent,
        string objectName,
        string title,
        string body)
    {
        GameObject section = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(Image)
        );

        section.transform.SetParent(parent, false);

        RectTransform r = section.GetComponent<RectTransform>();

        float index = parent.childCount - 1;

        r.anchorMin = new Vector2(0, 1);
        r.anchorMax = new Vector2(1, 1);
        r.pivot = new Vector2(.5f, 1);
        r.sizeDelta = new Vector2(0, 135);
        r.anchoredPosition = new Vector2(0, -index * 150);

        Image bg = section.GetComponent<Image>();
        bg.color = new Color(.025f, .035f, .045f, .95f);

        GameObject textObject = new GameObject(
            "TEXT",
            typeof(RectTransform),
            typeof(Text)
        );

        textObject.transform.SetParent(section.transform, false);

        RectTransform tr = textObject.GetComponent<RectTransform>();
        tr.anchorMin = Vector2.zero;
        tr.anchorMax = Vector2.one;
        tr.offsetMin = new Vector2(18, 12);
        tr.offsetMax = new Vector2(-18, -12);

        Text text = textObject.GetComponent<Text>();
        text.text = title + "\n\n" + body;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 18;
        text.alignment = TextAnchor.UpperLeft;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
    }
}
