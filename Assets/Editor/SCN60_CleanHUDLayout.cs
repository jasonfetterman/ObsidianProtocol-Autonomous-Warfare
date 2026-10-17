using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class SCN60_CleanHUDLayout
{
    private const string ScenePath =
        "Assets/Scenes/SCN60_GALAXY_MAP/Galaxy_Map.unity";

    [MenuItem("Obsidian Protocol/SCN60/REDO CLEAN HUD LAYOUT")]
    public static void Build()
    {
        Scene scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single
        );

        GameObject hud = GameObject.Find("HUD-201 GalaxyMapHUD");

        if (hud == null)
        {
            Debug.LogError("[SCN60] HUD-201 GalaxyMapHUD not found.");
            return;
        }

        RectTransform root = hud.GetComponent<RectTransform>();

        if (root == null)
        {
            Debug.LogError("[SCN60] HUD root has no RectTransform.");
            return;
        }

        // ============================================================
        // ROOT
        // ============================================================

        root.localScale = Vector3.one;
        root.anchoredPosition = Vector2.zero;
        root.sizeDelta = Vector2.zero;
        root.anchorMin = Vector2.zero;
        root.anchorMax = Vector2.one;
        root.pivot = new Vector2(0.5f, 0.5f);

        // ============================================================
        // MAJOR HUD REGIONS
        //
        // Screen layout:
        //
        //  TOP BAR
        //
        //  LEFT              MAP              RIGHT
        //
        //  Navigation                         Object Info
        //  Quick Actions                      Map Layers
        //  Legend                             Alerts
        //
        //              BOTTOM BAR
        // ============================================================

        SetRegion(
            hud,
            "HUD-201-01 TopBar",
            0.01f, 0.91f,
            0.99f, 0.99f
        );

        SetRegion(
            hud,
            "HUD-201-02 NavigationPanel",
            0.01f, 0.56f,
            0.20f, 0.89f
        );

        SetRegion(
            hud,
            "HUD-201-03 MapLayerPanel",
            0.80f, 0.56f,
            0.99f, 0.89f
        );

        SetRegion(
            hud,
            "HUD-201-04 GalaxyObjectInfoPanel",
            0.80f, 0.30f,
            0.99f, 0.53f
        );

        SetRegion(
            hud,
            "HUD-201-05 GalaxyLegendPanel",
            0.01f, 0.30f,
            0.20f, 0.53f
        );

        SetRegion(
            hud,
            "HUD-201-06 QuickActionsPanel",
            0.01f, 0.14f,
            0.20f, 0.27f
        );

        SetRegion(
            hud,
            "HUD-201-07 AlertsPanel",
            0.80f, 0.14f,
            0.99f, 0.27f
        );

        SetRegion(
            hud,
            "HUD-201-08 BottomBar",
            0.22f, 0.015f,
            0.78f, 0.095f
        );

        // ============================================================
        // GALAXY MAP
        //
        // Full central area. Existing planet positions are untouched.
        // ============================================================

        SetRegion(
            hud,
            "GALAXY MAP DISPLAY",
            0.215f, 0.105f,
            0.795f, 0.905f
        );

        // ============================================================
        // TOP BAR
        // ============================================================

        SetChildRegion(
            hud,
            "HUD-201-01 TopBar",
            "HUD-201-01C PlayerFactionEmblem",
            0.01f, 0.10f,
            0.075f, 0.90f
        );

        SetChildRegion(
            hud,
            "HUD-201-01 TopBar",
            "HUD-201-01A CurrentGalaxyNameDisplay",
            0.085f, 0.10f,
            0.34f, 0.90f
        );

        SetChildRegion(
            hud,
            "HUD-201-01 TopBar",
            "HUD-201-01B CurrentSectorNameDisplay",
            0.35f, 0.10f,
            0.54f, 0.90f
        );

        SetChildRegion(
            hud,
            "HUD-201-01 TopBar",
            "HUD-201-01D PlayerResourcesPanel",
            0.63f, 0.10f,
            0.80f, 0.90f
        );

        SetChildRegion(
            hud,
            "HUD-201-01 TopBar",
            "HUD-201-01E TimeControlPanel",
            0.82f, 0.10f,
            0.99f, 0.90f
        );

        // ============================================================
        // NAVIGATION PANEL
        // ============================================================

        SetVerticalChildren(
            hud,
            "HUD-201-02 NavigationPanel",
            new string[]
            {
                "HUD-201-02C SectorListDropdown",
                "HUD-201-02D PlanetSearchField",
                "HUD-201-02A GalaxyZoomInButton",
                "HUD-201-02B GalaxyZoomOutButton",
                "HUD-201-02E FilterPanelButton"
            }
        );

        // ============================================================
        // MAP LAYERS
        // ============================================================

        SetVerticalChildren(
            hud,
            "HUD-201-03 MapLayerPanel",
            new string[]
            {
                "HUD-201-03A LayerToggle_Galaxy",
                "HUD-201-03B LayerToggle_Sectors",
                "HUD-201-03C LayerToggle_Planets",
                "HUD-201-03D LayerToggle_Factions",
                "HUD-201-03E LayerToggle_WarfareZones",
                "HUD-201-03F LayerToggle_Resources",
                "HUD-201-03G LayerToggle_TradeRoutes"
            }
        );

        // ============================================================
        // OBJECT INFORMATION
        // ============================================================

        SetVerticalChildren(
            hud,
            "HUD-201-04 GalaxyObjectInfoPanel",
            new string[]
            {
                "HUD-201-04A ObjectNameDisplay",
                "HUD-201-04B ObjectTypeDisplay",
                "HUD-201-04C ObjectOwnerFactionDisplay",
                "HUD-201-04D ObjectStatusDisplay",
                "HUD-201-04E ObjectThreatLevelDisplay",
                "HUD-201-04F ObjectEnterButton"
            }
        );

        // ============================================================
        // LEGEND
        // ============================================================

        SetVerticalChildren(
            hud,
            "HUD-201-05 GalaxyLegendPanel",
            new string[]
            {
                "HUD-201-05A FACTIONCOLORLEGEND",
                "HUD-201-05B WARFAREZONELEGEND",
                "HUD-201-05C RESOURCELEGEND",
                "HUD-201-05D ENVIRONMENTLEGEND"
            }
        );

        // ============================================================
        // QUICK ACTIONS
        // ============================================================

        SetHorizontalChildren(
            hud,
            "HUD-201-06 QuickActionsPanel",
            new string[]
            {
                "HUD-201-06A JumpToHomeworldButton",
                "HUD-201-06B JumpToLastViewedPlanetButton",
                "HUD-201-06C OpenIntelNetworkButton",
                "HUD-201-06D OpenDoctrineManagerButton",
                "HUD-201-06E OpenLogisticsOverviewButton"
            }
        );

        // ============================================================
        // ALERTS
        // ============================================================

        SetVerticalChildren(
            hud,
            "HUD-201-07 AlertsPanel",
            new string[]
            {
                "HUD-201-07A StrategicAlertEntry",
                "HUD-201-07B WarfareAlertEntry",
                "HUD-201-07C FactionAlertEntry",
                "HUD-201-07D ResourceAlertEntry"
            }
        );

        // ============================================================
        // BOTTOM BAR
        // ============================================================

        SetHorizontalChildren(
            hud,
            "HUD-201-08 BottomBar",
            new string[]
            {
                "HUD-201-08A BackButton",
                "HUD-201-08B ExitToCommandCenterButton",
                "HUD-201-08C SettingsButton",
                "HUD-201-08D HelpButton"
            }
        );

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[SCN60] CLEAN HUD LAYOUT COMPLETE. " +
            "Existing functionality and planet positions were not modified."
        );
    }

    private static Transform Find(GameObject root, string path)
    {
        Transform current = root.transform;

        string[] parts = path.Split('/');

        foreach (string part in parts)
        {
            current = current.Find(part);

            if (current == null)
                return null;
        }

        return current;
    }

    private static void SetRegion(
        GameObject root,
        string objectName,
        float minX,
        float minY,
        float maxX,
        float maxY
    )
    {
        Transform t = Find(root, objectName);

        if (t == null)
        {
            Debug.LogWarning("[SCN60] Missing: " + objectName);
            return;
        }

        RectTransform rt = t as RectTransform;

        if (rt == null)
        {
            Debug.LogWarning(
                "[SCN60] Not RectTransform: " + objectName
            );
            return;
        }

        rt.anchorMin = new Vector2(minX, minY);
        rt.anchorMax = new Vector2(maxX, maxY);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.localScale = Vector3.one;
    }

    private static void SetChildRegion(
        GameObject root,
        string parentPath,
        string childName,
        float minX,
        float minY,
        float maxX,
        float maxY
    )
    {
        Transform parent = Find(root, parentPath);

        if (parent == null)
        {
            Debug.LogWarning("[SCN60] Missing parent: " + parentPath);
            return;
        }

        Transform child = parent.Find(childName);

        if (child == null)
        {
            Debug.LogWarning(
                "[SCN60] Missing child: " +
                parentPath + "/" + childName
            );
            return;
        }

        RectTransform rt = child as RectTransform;

        if (rt == null)
            return;

        rt.anchorMin = new Vector2(minX, minY);
        rt.anchorMax = new Vector2(maxX, maxY);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = Vector2.zero;
        rt.localScale = Vector3.one;
    }

    private static void SetVerticalChildren(
        GameObject root,
        string parentPath,
        string[] names
    )
    {
        Transform parent = Find(root, parentPath);

        if (parent == null)
            return;

        float count = names.Length;
        float gap = 0.025f;
        float usable = 1f - gap * (count + 1);
        float height = usable / count;

        for (int i = 0; i < names.Length; i++)
        {
            Transform child = parent.Find(names[i]);

            if (child == null)
            {
                Debug.LogWarning(
                    "[SCN60] Missing child: " +
                    parentPath + "/" + names[i]
                );
                continue;
            }

            RectTransform rt = child as RectTransform;

            if (rt == null)
                continue;

            float top = 1f - gap - i * (height + gap);
            float bottom = top - height;

            rt.anchorMin = new Vector2(0.05f, bottom);
            rt.anchorMax = new Vector2(0.95f, top);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.localScale = Vector3.one;
        }
    }

    private static void SetHorizontalChildren(
        GameObject root,
        string parentPath,
        string[] names
    )
    {
        Transform parent = Find(root, parentPath);

        if (parent == null)
            return;

        float count = names.Length;
        float gap = 0.025f;
        float usable = 1f - gap * (count + 1);
        float width = usable / count;

        for (int i = 0; i < names.Length; i++)
        {
            Transform child = parent.Find(names[i]);

            if (child == null)
            {
                Debug.LogWarning(
                    "[SCN60] Missing child: " +
                    parentPath + "/" + names[i]
                );
                continue;
            }

            RectTransform rt = child as RectTransform;

            if (rt == null)
                continue;

            float left = gap + i * (width + gap);
            float right = left + width;

            rt.anchorMin = new Vector2(left, 0.12f);
            rt.anchorMax = new Vector2(right, 0.88f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = Vector2.zero;
            rt.localScale = Vector3.one;
        }
    }
}

