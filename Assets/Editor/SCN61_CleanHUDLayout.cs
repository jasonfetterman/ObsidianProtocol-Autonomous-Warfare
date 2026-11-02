using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SCN61_CleanHUDLayout
{
    private const string ScenePath =
        "Assets/Scenes/SCN61_STAR_SYSTEM/Star_System.unity";

    [MenuItem("Obsidian Protocol/SCN61/REDO CLEAN HUD LAYOUT")]
    public static void Build()
    {
        Scene scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single
        );

        GameObject root = GameObject.Find("HUD-202 StarSystemHUD");

        if (root == null)
        {
            Debug.LogError("[SCN61] HUD-202 StarSystemHUD not found.");
            return;
        }

        RectTransform hud = root.GetComponent<RectTransform>();

        if (hud == null)
        {
            Debug.LogError("[SCN61] HUD root has no RectTransform.");
            return;
        }

        // ------------------------------------------------------------
        // MAIN HUD REGIONS
        // ------------------------------------------------------------

        SetRect(hud, 0f, 0f, 1f, 1f);

        SetRect(Find(root, "HUD-202-01 TopBar"),
            .01f, .91f, .99f, .99f);

        SetRect(Find(root, "HUD-202-02 NavigationPanel"),
            .01f, .56f, .20f, .89f);

        SetRect(Find(root, "HUD-202-03 MapLayerPanel"),
            .80f, .56f, .99f, .89f);

        SetRect(Find(root, "HUD-202-04 SystemObjectInfoPanel"),
            .80f, .30f, .99f, .53f);

        SetRect(Find(root, "HUD-202-05 OrbitPanel"),
            .01f, .30f, .20f, .53f);

        SetRect(Find(root, "HUD-202-06 QuickActionsPanel"),
            .01f, .14f, .20f, .27f);

        SetRect(Find(root, "HUD-202-07 AlertsPanel"),
            .80f, .14f, .99f, .27f);

        SetRect(Find(root, "HUD-202-08 BottomBar"),
            .22f, .015f, .78f, .095f);

        // Keep the actual system map centered.
        Transform map = root.transform.Find("SYSTEM STAR");
        if (map != null)
        {
            RectTransform mapRect = map.GetComponent<RectTransform>();

            if (mapRect != null)
                SetRect(mapRect, .215f, .105f, .795f, .905f);
        }

        // ------------------------------------------------------------
        // TOP BAR
        // ------------------------------------------------------------

        Transform top = Find(root, "HUD-202-01 TopBar");

        if (top != null)
        {
            SetRect(Find(top.gameObject,
                "HUD-202-01C PlayerFactionEmblem"),
                .01f, .10f, .075f, .90f);

            SetRect(Find(top.gameObject,
                "HUD-202-01A CurrentSystemNameDisplay"),
                .085f, .10f, .34f, .90f);

            SetRect(Find(top.gameObject,
                "HUD-202-01B CurrentStarNameDisplay"),
                .35f, .10f, .54f, .90f);

            SetRect(Find(top.gameObject,
                "HUD-202-01D PlayerResourcesPanel"),
                .63f, .10f, .80f, .90f);

            SetRect(Find(top.gameObject,
                "HUD-202-01E TimeControlPanel"),
                .82f, .10f, .99f, .90f);
        }

        // ------------------------------------------------------------
        // NAVIGATION PANEL
        // ------------------------------------------------------------

        Transform nav = Find(root, "HUD-202-02 NavigationPanel");

        if (nav != null)
        {
            SetVerticalChildren(nav.gameObject, new[]
            {
                "HUD-202-02C PlanetListDropdown",
                "HUD-202-02D SystemSearchField",
                "HUD-202-02A SystemZoomInButton",
                "HUD-202-02B SystemZoomOutButton",
                "HUD-202-02E FilterPanelButton"
            });
        }

        // ------------------------------------------------------------
        // MAP LAYERS
        // ------------------------------------------------------------

        Transform layers = Find(root, "HUD-202-03 MapLayerPanel");

        if (layers != null)
        {
            SetVerticalChildren(layers.gameObject, new[]
            {
                "HUD-202-03A LayerToggle_Star",
                "HUD-202-03B LayerToggle_Planets",
                "HUD-202-03C LayerToggle_Orbits",
                "HUD-202-03D LayerToggle_Factions",
                "HUD-202-03E LayerToggle_WarfareZones",
                "HUD-202-03F LayerToggle_Resources",
                "HUD-202-03G LayerToggle_LogisticsRoutes"
            });
        }

        // ------------------------------------------------------------
        // OBJECT INFORMATION
        // ------------------------------------------------------------

        Transform info = Find(root, "HUD-202-04 SystemObjectInfoPanel");

        if (info != null)
        {
            SetVerticalChildren(info.gameObject, new[]
            {
                "HUD-202-04A ObjectNameDisplay",
                "HUD-202-04B ObjectTypeDisplay",
                "HUD-202-04C ObjectOwnerFactionDisplay",
                "HUD-202-04D ObjectStatusDisplay",
                "HUD-202-04E ObjectThreatLevelDisplay",
                "HUD-202-04F ObjectEnterButton"
            });
        }

        // ------------------------------------------------------------
        // ORBIT PANEL
        // ------------------------------------------------------------

        Transform orbit = Find(root, "HUD-202-05 OrbitPanel");

        if (orbit != null)
        {
            SetVerticalChildren(orbit.gameObject, new[]
            {
                "HUD-202-05A OrbitPathToggle",
                "HUD-202-05B OrbitalVelocityIndicator",
                "HUD-202-05C OrbitalDistanceDisplay",
                "HUD-202-05D OrbitalStabilityIndicator"
            });
        }

        // ------------------------------------------------------------
        // QUICK ACTIONS
        // ------------------------------------------------------------

        Transform quick = Find(root, "HUD-202-06 QuickActionsPanel");

        if (quick != null)
        {
            SetHorizontalChildren(quick.gameObject, new[]
            {
                "HUD-202-06A JumpToGalaxyMapButton",
                "HUD-202-06B JumpToPlanetOverviewButton",
                "HUD-202-06C OpenIntelNetworkButton",
                "HUD-202-06D OpenDoctrineManagerButton",
                "HUD-202-06E OpenLogisticsOverviewButton"
            });
        }

        // ------------------------------------------------------------
        // ALERTS
        // ------------------------------------------------------------

        Transform alerts = Find(root, "HUD-202-07 AlertsPanel");

        if (alerts != null)
        {
            SetVerticalChildren(alerts.gameObject, new[]
            {
                "HUD-202-07A StrategicAlertEntry",
                "HUD-202-07B WarfareAlertEntry",
                "HUD-202-07C FactionAlertEntry",
                "HUD-202-07D ResourceAlertEntry"
            });
        }

        // ------------------------------------------------------------
        // BOTTOM BAR
        // ------------------------------------------------------------

        Transform bottom = Find(root, "HUD-202-08 BottomBar");

        if (bottom != null)
        {
            SetHorizontalChildren(bottom.gameObject, new[]
            {
                "HUD-202-08A BackButton",
                "HUD-202-08B ExitToCommandCenterButton",
                "HUD-202-08C SettingsButton",
                "HUD-202-08D HelpButton"
            });
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[SCN61] CLEAN HUD LAYOUT COMPLETE.");
    }

    private static Transform Find(GameObject root, string path)
    {
        if (root == null)
            return null;

        return root.transform.Find(path);
    }

    private static void SetRect(
        Transform target,
        float minX,
        float minY,
        float maxX,
        float maxY)
    {
        if (target == null)
            return;

        RectTransform rect = target.GetComponent<RectTransform>();

        if (rect == null)
            return;

        rect.anchorMin = new Vector2(minX, minY);
        rect.anchorMax = new Vector2(maxX, maxY);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;
    }

    private static void SetVerticalChildren(
        GameObject parent,
        string[] names)
    {
        int count = names.Length;

        for (int i = 0; i < count; i++)
        {
            Transform child = Find(parent, names[i]);

            if (child == null)
                continue;

            float top = 1f - ((float)i / count);
            float bottom = 1f - ((float)(i + 1) / count);

            float margin = .05f;

            SetRect(
                child,
                margin,
                bottom + margin,
                1f - margin,
                top - margin
            );
        }
    }

    private static void SetHorizontalChildren(
        GameObject parent,
        string[] names)
    {
        int count = names.Length;

        for (int i = 0; i < count; i++)
        {
            Transform child = Find(parent, names[i]);

            if (child == null)
                continue;

            float left = (float)i / count;
            float right = (float)(i + 1) / count;

            float margin = .025f;

            SetRect(
                child,
                left + margin,
                .10f,
                right - margin,
                .90f
            );
        }
    }
}
