using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class CleanIntelligenceHUDLayout
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Clean HUD Layout")]
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

        // HEADER
        SetPanel(hud, "HEADER",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(1920f, 80f),
            new Vector2(0f, -40f));

        // LEFT COLUMN
        SetPanel(hud, "CONTACTS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(300f, 850f),
            new Vector2(-660f, -5f));

        // CENTER-LEFT
        SetPanel(hud, "SENSORS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(320f, 250f),
            new Vector2(-310f, 255f));

        SetPanel(hud, "NETWORK",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(320f, 250f),
            new Vector2(-310f, -60f));

        // CENTER
        SetPanel(hud, "THREAT ANALYSIS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(620f, 250f),
            new Vector2(170f, 255f));

        SetPanel(hud, "INTELLIGENCE REPORTS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(620f, 250f),
            new Vector2(170f, -60f));

        // RIGHT COLUMN
        SetPanel(hud, "SURVEILLANCE",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(300f, 250f),
            new Vector2(660f, 255f));

        SetPanel(hud, "ANALYSIS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(300f, 250f),
            new Vector2(660f, -60f));

        // BOTTOM CENTER
        SetPanel(hud, "OPERATIONS LINK",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(620f, 80f),
            new Vector2(170f, -400f));

        // STRATEGIC MAP starts OFF
        Transform map = FindChild(hud.transform, "STRATEGIC INTELLIGENCE MAP");

        if (map != null)
        {
            map.gameObject.SetActive(false);

            RectTransform mapRect = map.GetComponent<RectTransform>();

            if (mapRect != null)
            {
                mapRect.anchorMin = new Vector2(0.5f, 0.5f);
                mapRect.anchorMax = new Vector2(0.5f, 0.5f);
                mapRect.pivot = new Vector2(0.5f, 0.5f);
                mapRect.anchoredPosition = Vector2.zero;
                mapRect.sizeDelta = new Vector2(900f, 760f);
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[INTELLIGENCE] CLEAN HUD LAYOUT APPLIED.");
        Debug.Log("[INTELLIGENCE] ALL PANELS HAVE NON-OVERLAPPING POSITIONS.");
        Debug.Log("[INTELLIGENCE] STRATEGIC MAP = OFF.");
    }

    private static void SetPanel(
        GameObject hud,
        string panelName,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 size,
        Vector2 position)
    {
        Transform t = FindChild(hud.transform, panelName);

        if (t == null)
        {
            Debug.LogError("[INTELLIGENCE] Missing panel: " + panelName);
            return;
        }

        RectTransform r = t.GetComponent<RectTransform>();

        if (r == null)
        {
            Debug.LogError("[INTELLIGENCE] No RectTransform: " + panelName);
            return;
        }

        r.anchorMin = anchorMin;
        r.anchorMax = anchorMax;
        r.pivot = new Vector2(0.5f, 0.5f);
        r.sizeDelta = size;
        r.anchoredPosition = position;

        t.gameObject.SetActive(true);

        Debug.Log(
            "[INTELLIGENCE] " + panelName +
            " POS=" + position +
            " SIZE=" + size
        );
    }

    private static Transform FindChild(
        Transform parent,
        string objectName)
    {
        foreach (Transform child in
                 parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == objectName)
                return child;
        }

        return null;
    }
}
