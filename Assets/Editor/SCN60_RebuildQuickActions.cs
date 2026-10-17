using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SCN60_RebuildQuickActions
{
    private const string ScenePath =
        "Assets/Scenes/SCN60_GALAXY_MAP/Galaxy_Map.unity";

    [MenuItem("Obsidian Protocol/SCN60/REBUILD QUICK ACTIONS PANEL")]
    public static void Rebuild()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("[SCN60] Stop Play mode before rebuilding Quick Actions.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError(
                "[SCN60] Open Galaxy_Map.unity before running this repair. " +
                "No scene was changed."
            );
            return;
        }

        GameObject hud = GameObject.Find("HUD-201 GalaxyMapHUD");
        if (hud == null)
        {
            Debug.LogError("[SCN60] GalaxyMapHUD not found. No changes saved.");
            return;
        }

        Transform panel = hud.transform.Find("HUD-201-06 QuickActionsPanel");
        if (panel == null)
        {
            Debug.LogError("[SCN60] QuickActionsPanel not found. No changes saved.");
            return;
        }

        string[] names =
        {
            "HUD-201-06A JumpToHomeworldButton",
            "HUD-201-06B JumpToLastViewedPlanetButton",
            "HUD-201-06C OpenIntelNetworkButton",
            "HUD-201-06D OpenDoctrineManagerButton",
            "HUD-201-06E OpenLogisticsOverviewButton"
        };

        Transform[] buttons = new Transform[names.Length];

        for (int i = 0; i < names.Length; i++)
        {
            buttons[i] = panel.Find(names[i]);

            if (buttons[i] == null ||
                buttons[i].GetComponent<RectTransform>() == null)
            {
                Debug.LogError(
                    "[SCN60] Missing button or RectTransform: " +
                    names[i] + ". No changes saved."
                );
                return;
            }
        }

        Undo.RecordObject(panel, "Rebuild Galaxy Map Quick Actions");

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.01f, 0.10f);
        panelRect.anchorMax = new Vector2(0.20f, 0.29f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.localScale = Vector3.one;

        const float gap = 0.035f;
        float height = (1f - gap * (names.Length + 1)) / names.Length;

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rect = buttons[i].GetComponent<RectTransform>();
            Undo.RecordObject(rect, "Arrange Quick Actions Vertically");

            float top = 1f - gap - i * (height + gap);
            float bottom = top - height;

            rect.anchorMin = new Vector2(0.05f, bottom);
            rect.anchorMax = new Vector2(0.95f, top);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            rect.localScale = Vector3.one;

            EditorUtility.SetDirty(rect);
        }

        EditorUtility.SetDirty(panelRect);
        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("[SCN60] Scene save failed.");
            return;
        }

        Debug.Log(
            "[SCN60] Quick Actions rebuilt as a vertical stack. " +
            "Only the panel and its five button layouts were changed."
        );
    }
}
