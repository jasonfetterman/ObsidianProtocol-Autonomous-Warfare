using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixIntelligenceHUDLayout
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Fix HUD Layout")]
    public static void Fix()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        GameObject hud =
            GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] INTELLIGENCE HUD not found.");
            return;
        }

        SetPanel(
            hud,
            "HEADER",
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0f, -50f),
            new Vector2(1920f, 100f));

        SetPanel(
            hud,
            "CONTACTS",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(195f, 20f),
            new Vector2(350f, 700f));

        SetPanel(
            hud,
            "SENSORS",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(570f, 190f),
            new Vector2(300f, 300f));

        SetPanel(
            hud,
            "NETWORK",
            new Vector2(0f, 0.5f),
            new Vector2(0f, 0.5f),
            new Vector2(570f, -160f),
            new Vector2(300f, 300f));

        SetPanel(
            hud,
            "THREAT ANALYSIS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 190f),
            new Vector2(500f, 330f));

        SetPanel(
            hud,
            "INTELLIGENCE REPORTS",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, -130f),
            new Vector2(500f, 300f));

        SetPanel(
            hud,
            "SURVEILLANCE",
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.5f),
            new Vector2(-195f, 190f),
            new Vector2(350f, 300f));

        SetPanel(
            hud,
            "ANALYSIS",
            new Vector2(1f, 0.5f),
            new Vector2(1f, 0.5f),
            new Vector2(-195f, -160f),
            new Vector2(350f, 300f));

        SetPanel(
            hud,
            "OPERATIONS LINK",
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0f, 65f),
            new Vector2(500f, 100f));

        GameObject map =
            FindChild(
                hud.transform,
                "STRATEGIC INTELLIGENCE MAP");

        if (map != null)
        {
            map.SetActive(false);

            RectTransform mapRect =
                map.GetComponent<RectTransform>();

            if (mapRect != null)
            {
                mapRect.anchorMin =
                    new Vector2(0.5f, 0.5f);

                mapRect.anchorMax =
                    new Vector2(0.5f, 0.5f);

                mapRect.pivot =
                    new Vector2(0.5f, 0.5f);

                mapRect.anchoredPosition =
                    Vector2.zero;

                mapRect.sizeDelta =
                    new Vector2(900f, 760f);
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[INTELLIGENCE] HUD LAYOUT FIX COMPLETE.");
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
                "[INTELLIGENCE] Missing panel: " + name);
            return;
        }

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        if (rect == null)
        {
            Debug.LogWarning(
                "[INTELLIGENCE] No RectTransform: " + name);
            return;
        }

        panel.SetActive(true);

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Debug.Log(
            "[INTELLIGENCE] Positioned: " + name);
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
