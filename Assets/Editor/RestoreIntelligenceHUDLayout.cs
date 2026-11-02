using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class RestoreIntelligenceHUDLayout
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Restore Proper HUD Layout")]
    public static void Restore()
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

        // HEADER
        SetPanel(
            hud, "HEADER",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -50f),
            new Vector2(1920f, 100f));

        // LEFT COLUMN
        SetPanel(
            hud, "CONTACTS",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(200f, 20f),
            new Vector2(365f, 760f));

        SetPanel(
            hud, "SENSORS",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(200f, -190f),
            new Vector2(330f, 370f));

        SetPanel(
            hud, "NETWORK",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(200f, -570f),
            new Vector2(330f, 370f));

        // CENTER
        SetPanel(
            hud, "THREAT ANALYSIS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 195f),
            new Vector2(670f, 400f));

        SetPanel(
            hud, "INTELLIGENCE REPORTS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, -195f),
            new Vector2(670f, 350f));

        // RIGHT COLUMN
        SetPanel(
            hud, "SURVEILLANCE",
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.5f),
            new Vector2(-200f, 195f),
            new Vector2(370f, 370f));

        SetPanel(
            hud, "ANALYSIS",
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.5f),
            new Vector2(-200f, -195f),
            new Vector2(370f, 370f));

        // OPERATIONS BAR
        SetPanel(
            hud, "OPERATIONS LINK",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 70f),
            new Vector2(680f, 110f));

        // STRATEGIC MAP
        GameObject map =
            FindChild(
                hud.transform,
                "STRATEGIC INTELLIGENCE MAP");

        if (map != null)
        {
            map.SetActive(false);

            RectTransform rect =
                map.GetComponent<RectTransform>();

            if (rect != null)
            {
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.anchoredPosition = Vector2.zero;
                rect.sizeDelta = new Vector2(900f, 760f);
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[INTELLIGENCE] PROPER HUD LAYOUT RESTORED.");
    }

    private static void SetPanel(
        GameObject hud,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 position,
        Vector2 size)
    {
        GameObject panel =
            FindChild(hud.transform, name);

        if (panel == null)
        {
            Debug.LogWarning(
                "[INTELLIGENCE] Missing: " + name);
            return;
        }

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        if (rect == null)
            return;

        panel.SetActive(true);

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }

    private static GameObject FindChild(
        Transform parent,
        string objectName)
    {
        foreach (
            Transform child
            in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == objectName)
                return child.gameObject;
        }

        return null;
    }
}
