using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class AlignIntelligenceHUDContents
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Align HUD Contents")]
    public static void Align()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE] HUD not found.");
            return;
        }

        AlignContacts(hud);
        AlignSimplePanel(hud, "SENSORS", "DATA",
            "BUTTON - SENSOR CONTROL");

        AlignSimplePanel(hud, "NETWORK", "DATA",
            "BUTTON - NETWORK CONTROL");

        AlignThreatAnalysis(hud);
        AlignReports(hud);
        AlignSurveillance(hud);
        AlignAnalysis(hud);
        AlignOperations(hud);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[INTELLIGENCE] HUD CONTENT ALIGNMENT COMPLETE.");
    }

    private static void AlignContacts(GameObject hud)
    {
        GameObject panel = Find(hud, "CONTACTS");
        if (panel == null) return;

        Set(
            panel, "TITLE",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -35f),
            new Vector2(315f, 45f));

        string[] buttons =
        {
            "BUTTON - UNKNOWN",
            "BUTTON - SUSPECTED",
            "BUTTON - IDENTIFIED",
            "BUTTON - TRACKED",
            "BUTTON - LOST CONTACT"
        };

        float y = -95f;

        foreach (string name in buttons)
        {
            Set(
                panel,
                name,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, y),
                new Vector2(315f, 48f));

            y -= 58f;
        }

        Set(
            panel, "CONTACT SUMMARY",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 125f),
            new Vector2(315f, 230f));
    }

    private static void AlignSimplePanel(
        GameObject hud,
        string panelName,
        string dataName,
        string buttonName)
    {
        GameObject panel = Find(hud, panelName);
        if (panel == null) return;

        Set(
            panel, "TITLE",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -28f),
            new Vector2(270f, 42f));

        Set(
            panel, dataName,
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -160f),
            new Vector2(270f, 175f));

        Set(
            panel, buttonName,
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 35f),
            new Vector2(270f, 44f));
    }

    private static void AlignThreatAnalysis(GameObject hud)
    {
        GameObject panel = Find(hud, "THREAT ANALYSIS");
        if (panel == null) return;

        Set(
            panel, "TITLE",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -28f),
            new Vector2(620f, 42f));

        Set(
            panel, "THREAT",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -160f),
            new Vector2(600f, 190f));

        Set(
            panel, "BUTTON - ANALYZE CONTACT",
            new Vector2(0f, 0f),
            new Vector2(0f, 0f),
            new Vector2(125f, 40f),
            new Vector2(220f, 48f));

        Set(
            panel, "BUTTON - GENERATE SUMMARY",
            new Vector2(1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(-125f, 40f),
            new Vector2(220f, 48f));
    }

    private static void AlignReports(GameObject hud)
    {
        GameObject panel = Find(hud, "INTELLIGENCE REPORTS");
        if (panel == null) return;

        Set(
            panel, "TITLE",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -28f),
            new Vector2(620f, 40f));

        Set(
            panel, "RECENT",
            new Vector2(0f, 1f),
            new Vector2(0f, 1f),
            new Vector2(170f, -125f),
            new Vector2(300f, 170f));

        Set(
            panel, "ARCHIVE",
            new Vector2(1f, 1f),
            new Vector2(1f, 1f),
            new Vector2(-150f, -125f),
            new Vector2(270f, 170f));

        Set(
            panel, "BUTTON - OPEN REPORT",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 35f),
            new Vector2(220f, 46f));
    }

    private static void AlignSurveillance(GameObject hud)
    {
        GameObject panel = Find(hud, "SURVEILLANCE");
        if (panel == null) return;

        Set(
            panel, "TITLE",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -28f),
            new Vector2(320f, 42f));

        Set(
            panel, "DATA",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -145f),
            new Vector2(310f, 180f));

        Set(
            panel, "BUTTON - VIEW FEED",
            new Vector2(0f, 0f),
            new Vector2(0f, 0f),
            new Vector2(80f, 35f),
            new Vector2(145f, 45f));

        Set(
            panel, "BUTTON - ANALYZE DATA",
            new Vector2(1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(-80f, 35f),
            new Vector2(145f, 45f));
    }

    private static void AlignAnalysis(GameObject hud)
    {
        GameObject panel = Find(hud, "ANALYSIS");
        if (panel == null) return;

        Set(
            panel, "TITLE",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -28f),
            new Vector2(320f, 42f));

        Set(
            panel, "DATA",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -145f),
            new Vector2(310f, 180f));

        Set(
            panel, "BUTTON - GENERATE SUMMARY",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 35f),
            new Vector2(260f, 46f));
    }

    private static void AlignOperations(GameObject hud)
    {
        GameObject panel = Find(hud, "OPERATIONS LINK");
        if (panel == null) return;

        Set(
            panel, "TITLE",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -22f),
            new Vector2(250f, 30f));

        Set(
            panel, "BUTTON - FORWARD TO COMMAND",
            new Vector2(0f, 0f),
            new Vector2(0f, 0f),
            new Vector2(105f, 25f),
            new Vector2(190f, 44f));

        Set(
            panel, "BUTTON - FORWARD TO FLEET",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 25f),
            new Vector2(170f, 44f));

        Set(
            panel, "BUTTON - FORWARD TO RESEARCH",
            new Vector2(1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(-105f, 25f),
            new Vector2(190f, 44f));
    }

    private static void Set(
        GameObject panel,
        string childName,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 position,
        Vector2 size)
    {
        GameObject child = Find(panel, childName);

        if (child == null)
        {
            Debug.LogWarning(
                "[INTELLIGENCE] Missing child: " +
                panel.name + "/" + childName);
            return;
        }

        RectTransform rect =
            child.GetComponent<RectTransform>();

        if (rect == null)
            return;

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    private static GameObject Find(
        GameObject root,
        string name)
    {
        foreach (
            Transform child
            in root.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child.gameObject;
        }

        return null;
    }
}
