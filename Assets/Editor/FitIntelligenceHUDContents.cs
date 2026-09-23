using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class FitIntelligenceHUDContents
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Fit HUD Contents")]
    public static void Build()
    {
        Scene scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single
        );

        GameObject hud = GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError("[INTELLIGENCE] INTELLIGENCE HUD not found.");
            return;
        }

        // ============================================================
        // HEADER
        // ============================================================

        Fit(hud, "HEADER", "TITLE",
            new Vector2(500, 32),
            new Vector2(-600, 5));

        Fit(hud, "HEADER", "SUBTITLE",
            new Vector2(600, 24),
            new Vector2(-250, -18));

        Fit(hud, "HEADER", "STATUS",
            new Vector2(500, 32),
            new Vector2(650, 0));

        // ============================================================
        // CONTACTS 300 x 850
        // ============================================================

        Fit(hud, "CONTACTS", "TITLE",
            new Vector2(260, 38),
            new Vector2(0, 390));

        Fit(hud, "CONTACTS", "BUTTON - UNKNOWN",
            new Vector2(260, 44),
            new Vector2(0, 325));

        Fit(hud, "CONTACTS", "BUTTON - SUSPECTED",
            new Vector2(260, 44),
            new Vector2(0, 270));

        Fit(hud, "CONTACTS", "BUTTON - IDENTIFIED",
            new Vector2(260, 44),
            new Vector2(0, 215));

        Fit(hud, "CONTACTS", "BUTTON - TRACKED",
            new Vector2(260, 44),
            new Vector2(0, 160));

        Fit(hud, "CONTACTS", "BUTTON - LOST CONTACT",
            new Vector2(260, 44),
            new Vector2(0, 105));

        Fit(hud, "CONTACTS", "CONTACT SUMMARY",
            new Vector2(260, 300),
            new Vector2(0, -180));

        // ============================================================
        // SENSORS 320 x 250
        // ============================================================

        Fit(hud, "SENSORS", "TITLE",
            new Vector2(280, 34),
            new Vector2(0, 95));

        Fit(hud, "SENSORS", "DATA",
            new Vector2(280, 125),
            new Vector2(0, -5));

        Fit(hud, "SENSORS", "BUTTON - SENSOR CONTROL",
            new Vector2(280, 42),
            new Vector2(0, -92));

        // ============================================================
        // NETWORK 320 x 250
        // ============================================================

        Fit(hud, "NETWORK", "TITLE",
            new Vector2(280, 34),
            new Vector2(0, 95));

        Fit(hud, "NETWORK", "DATA",
            new Vector2(280, 125),
            new Vector2(0, -5));

        Fit(hud, "NETWORK", "BUTTON - NETWORK CONTROL",
            new Vector2(280, 42),
            new Vector2(0, -92));

        // ============================================================
        // THREAT ANALYSIS 620 x 250
        // ============================================================

        Fit(hud, "THREAT ANALYSIS", "TITLE",
            new Vector2(580, 34),
            new Vector2(0, 95));

        Fit(hud, "THREAT ANALYSIS", "THREAT",
            new Vector2(580, 125),
            new Vector2(0, -5));

        Fit(hud, "THREAT ANALYSIS", "BUTTON - GENERATE SUMMARY",
            new Vector2(260, 42),
            new Vector2(-150, -92));

        Fit(hud, "THREAT ANALYSIS", "BUTTON - ANALYZE CONTACT",
            new Vector2(260, 42),
            new Vector2(150, -92));

        // ============================================================
        // INTELLIGENCE REPORTS 620 x 250
        // ============================================================

        Fit(hud, "INTELLIGENCE REPORTS", "TITLE",
            new Vector2(580, 34),
            new Vector2(0, 95));

        Fit(hud, "INTELLIGENCE REPORTS", "RECENT",
            new Vector2(280, 120),
            new Vector2(-145, -5));

        Fit(hud, "INTELLIGENCE REPORTS", "ARCHIVE",
            new Vector2(280, 120),
            new Vector2(145, -5));

        Fit(hud, "INTELLIGENCE REPORTS", "BUTTON - OPEN REPORT",
            new Vector2(220, 42),
            new Vector2(0, -92));

        // ============================================================
        // SURVEILLANCE 300 x 250
        // ============================================================

        Fit(hud, "SURVEILLANCE", "TITLE",
            new Vector2(270, 34),
            new Vector2(0, 95));

        Fit(hud, "SURVEILLANCE", "DATA",
            new Vector2(270, 120),
            new Vector2(0, -5));

        Fit(hud, "SURVEILLANCE", "BUTTON - VIEW FEED",
            new Vector2(125, 42),
            new Vector2(-67, -92));

        Fit(hud, "SURVEILLANCE", "BUTTON - ANALYZE DATA",
            new Vector2(125, 42),
            new Vector2(67, -92));

        // ============================================================
        // ANALYSIS 300 x 250
        // ============================================================

        Fit(hud, "ANALYSIS", "TITLE",
            new Vector2(270, 34),
            new Vector2(0, 95));

        Fit(hud, "ANALYSIS", "DATA",
            new Vector2(270, 120),
            new Vector2(0, -5));

        Fit(hud, "ANALYSIS", "BUTTON - GENERATE SUMMARY",
            new Vector2(270, 42),
            new Vector2(0, -92));

        // ============================================================
        // OPERATIONS LINK 620 x 80
        // ============================================================

        Fit(hud, "OPERATIONS LINK", "TITLE",
            new Vector2(250, 24),
            new Vector2(0, 25));

        Fit(hud, "OPERATIONS LINK", "BUTTON - FORWARD TO COMMAND",
            new Vector2(180, 38),
            new Vector2(205, -18));

        Fit(hud, "OPERATIONS LINK", "BUTTON - FORWARD TO FLEET",
            new Vector2(160, 38),
            new Vector2(0, -18));

        Fit(hud, "OPERATIONS LINK", "BUTTON - FORWARD TO RESEARCH",
            new Vector2(180, 38),
            new Vector2(-205, -18));

        // ============================================================
        // MAP REMAINS OFF
        // ============================================================

        Transform map = FindChild(
            hud.transform,
            "STRATEGIC INTELLIGENCE MAP"
        );

        if (map != null)
            map.gameObject.SetActive(false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[INTELLIGENCE] HUD CONTENTS FIT TO PANELS.");
        Debug.Log("[INTELLIGENCE] MAP REMAINS OFF.");
    }

    private static void Fit(
        GameObject hud,
        string panelName,
        string childName,
        Vector2 size,
        Vector2 position)
    {
        Transform panel = FindChild(
            hud.transform,
            panelName
        );

        if (panel == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] Missing panel: " + panelName
            );
            return;
        }

        Transform child = FindChild(
            panel,
            childName
        );

        if (child == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] Missing child: " +
                panelName + " / " + childName
            );
            return;
        }

        RectTransform rect =
            child.GetComponent<RectTransform>();

        if (rect == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] No RectTransform: " +
                panelName + " / " + childName
            );
            return;
        }

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static Transform FindChild(
        Transform parent,
        string objectName)
    {
        foreach (
            Transform child
            in parent.GetComponentsInChildren<Transform>(true)
        )
        {
            if (child.name == objectName)
                return child;
        }

        return null;
    }
}
