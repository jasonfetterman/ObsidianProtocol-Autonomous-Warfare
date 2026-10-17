using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class PlanetaryBiosphereMapHUD220Builder
{
    private const string MenuPath =
        "Obsidian Protocol/Worlds/BUILD HUD-220 PLANETARY BIOSPHERE";

    private const string ScenePath =
        "Assets/Scenes/SCN78_BIOSPHERE/Planetary_Biosphere.unity";

    [MenuItem(MenuPath)]
    public static void Build()
    {
        EnsureFolder();

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        BuildWorld();
        BuildCamera();
        BuildEventSystem();

        GameObject canvas = BuildCanvas();
        GameObject hud = Create(canvas.transform, "HUD-220 PlanetaryBiosphereMapHUD");

        BuildTopBar(hud.transform);
        BuildNavigation(hud.transform);
        BuildLayers(hud.transform);
        BuildLegend(hud.transform);
        BuildInfo(hud.transform);
        BuildEcologicalStats(hud.transform);
        BuildEcologicalImpact(hud.transform);
        BuildQuickActions(hud.transform);
        BuildAlerts(hud.transform);
        BuildBottomBar(hud.transform);

        hud.AddComponent<PlanetaryBiosphereMapHUD220Controller>();

        SaveAndRegister(scene);

        Selection.activeGameObject = hud;

        Debug.Log(
            "[HUD-220] BUILD COMPLETE: Planetary Biosphere Map.");
    }

    private static void EnsureFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes/SCN78_BIOSPHERE"))
        {
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN78_BIOSPHERE");
        }
    }

    private static void BuildWorld()
    {
        GameObject world =
            new GameObject("PLANETARY_BIOSPHERE_WORLD");

        GameObject planet =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        planet.name = "PLANET_SURFACE_AURELIA";
        planet.transform.SetParent(world.transform, false);
        planet.transform.localScale = Vector3.one * 18f;

        Renderer renderer = planet.GetComponent<Renderer>();
        renderer.sharedMaterial =
            CreateMaterial(
                "AURELIA_BIOSPHERE_SURFACE",
                new Color(0.10f, 0.24f, 0.13f, 1f));

        GameObject atmosphere =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        atmosphere.name = "BIOSPHERE_ATMOSPHERE_SHELL";
        atmosphere.transform.SetParent(world.transform, false);
        atmosphere.transform.localScale = Vector3.one * 18.6f;

        Renderer atmosphereRenderer =
            atmosphere.GetComponent<Renderer>();

        atmosphereRenderer.sharedMaterial =
            CreateMaterial(
                "BIOSPHERE_ATMOSPHERE",
                new Color(0.12f, 0.28f, 0.34f, 0.12f));

        CreateLayer(world.transform, "BIOME_TYPES");
        CreateLayer(world.transform, "VEGETATION_DENSITY");
        CreateLayer(world.transform, "FAUNA_DISTRIBUTION");
        CreateLayer(world.transform, "WATER_SYSTEMS");
        CreateLayer(world.transform, "SOIL_COMPOSITION");
        CreateLayer(world.transform, "CLIMATE_BANDS");
        CreateLayer(world.transform, "LONG_TERM_WEATHER_PATTERNS");
        CreateLayer(world.transform, "ENVIRONMENTAL_HAZARDS");
        CreateLayer(world.transform, "RESOURCE_NODES");
        CreateLayer(world.transform, "FACTION_CONTROL");
        CreateLayer(world.transform, "WARFARE_ZONES");
        CreateLayer(world.transform, "LOGISTICS_ROUTES");
        CreateLayer(world.transform, "TERRAIN_MODIFIERS");

        BuildBiosphereMarkers(world.transform);
    }

    private static void CreateLayer(
        Transform parent,
        string name)
    {
        GameObject layer =
            new GameObject(
                "BIOSPHERE_LAYER_" + name);

        layer.transform.SetParent(
            parent,
            false);
    }

    private static void BuildBiosphereMarkers(
        Transform parent)
    {
        string[] names =
        {
            "FOREST_ZONE",
            "RAINFOREST_ZONE",
            "GRASSLAND_ZONE",
            "WETLAND_ZONE",
            "DESERT_ZONE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            GameObject marker =
                GameObject.CreatePrimitive(
                    PrimitiveType.Cylinder);

            marker.name =
                "BIOSPHERE_ZONE_" + names[i];

            marker.transform.SetParent(
                parent,
                false);

            marker.transform.localPosition =
                new Vector3(
                    Mathf.Cos(i * 1.25f) * 9f,
                    Mathf.Sin(i * 0.7f) * 5f,
                    Mathf.Sin(i * 1.25f) * 9f);

            marker.transform.localScale =
                new Vector3(0.7f, 0.08f, 0.7f);

            Renderer r =
                marker.GetComponent<Renderer>();

            r.sharedMaterial =
                CreateMaterial(
                    "ZONE_" + names[i],
                    new Color(
                        0.15f + i * 0.04f,
                        0.55f - i * 0.05f,
                        0.22f + i * 0.05f,
                        1f));
        }
    }

    private static Material CreateMaterial(
        string name,
        Color color)
    {
        Material material =
            new Material(
                Shader.Find("Universal Render Pipeline/Lit"));

        material.name = name;
        material.color = color;

        return material;
    }

    private static void BuildCamera()
    {
        GameObject camera =
            new GameObject("BiosphereCamera");

        Camera cam =
            camera.AddComponent<Camera>();

        camera.transform.position =
            new Vector3(0f, 4f, -27f);

        camera.transform.rotation =
            Quaternion.Euler(8f, 0f, 0f);

        cam.fieldOfView = 55f;
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 200f;
    }

    private static void BuildEventSystem()
    {
        GameObject es =
            new GameObject("EventSystem");

        es.AddComponent<EventSystem>();
        es.AddComponent<InputSystemUIInputModule>();
    }

    private static GameObject BuildCanvas()
    {
        GameObject canvas =
            new GameObject(
                "HUD-220 Canvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas c =
            canvas.GetComponent<Canvas>();

        c.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvas.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        return canvas;
    }

    private static void BuildTopBar(Transform root)
    {
        GameObject panel =
            Panel(root, "HUD-220-01 TopBar",
                new Vector2(0f, 0.92f),
                new Vector2(1f, 1f));

        Text(panel.transform,
            "HUD-220-01A PlanetNameDisplay",
            "AURELIA",
            new Vector2(0.01f, 0.35f),
            new Vector2(0.16f, 0.9f),
            24, true);

        Text(panel.transform,
            "HUD-220-01B HemisphereDisplay",
            "NORTHERN HEMISPHERE",
            new Vector2(0.17f, 0.35f),
            new Vector2(0.35f, 0.9f),
            16, false);

        Text(panel.transform,
            "HUD-220-01C BiosphereLayerDisplay",
            "BIOSPHERE // ECOLOGICAL MODEL",
            new Vector2(0.36f, 0.35f),
            new Vector2(0.58f, 0.9f),
            16, true);

        Text(panel.transform,
            "HUD-220-01D OwnerFactionEmblem",
            "A",
            new Vector2(0.82f, 0.25f),
            new Vector2(0.87f, 0.85f),
            24, true);

        Text(panel.transform,
            "HUD-220-01E PlayerResourcesPanel",
            "RES  84,250   //   ENERGY 72%   //   SUPPLY 91%",
            new Vector2(0.59f, 0.25f),
            new Vector2(0.82f, 0.85f),
            13, false);

        Text(panel.transform,
            "HUD-220-01F TimeControlPanel",
            "SIM TIME  0.0 h",
            new Vector2(0.88f, 0.25f),
            new Vector2(0.995f, 0.85f),
            13, true);
    }

    private static void BuildNavigation(Transform root)
    {
        string[] names =
        {
            "ReturnToClimateModelViewButton",
            "ReturnToStormCellAnalysisButton",
            "ReturnToAtmosphericSimulationButton",
            "ReturnToWeatherSystemViewButton",
            "ReturnToLogisticsNetworkMapButton",
            "ReturnToResourceDistributionMapButton",
            "ReturnToStrategicHeatmapButton",
            "ReturnToFactionTerritoryOverlayButton",
            "ReturnToFactionControlMapButton",
            "ReturnToWarfareProfileButton",
            "ReturnToEnvironmentProfileButton",
            "ReturnToIslandMapButton",
            "ReturnToBiomeMapButton",
            "ReturnToRegionMapButton",
            "ReturnToContinentMapButton",
            "ReturnToSurfaceMapButton",
            "ReturnToPlanetOverviewButton"
        };

        GameObject panel =
            Panel(root,
                "HUD-220-02 NavigationPanel",
                new Vector2(0.005f, 0.16f),
                new Vector2(0.18f, 0.90f));

        float y = 0.95f;

        for (int i = 0; i < names.Length; i++)
        {
            float h = 0.048f;

            Button(
                panel.transform,
                "HUD-220-02" +
                (char)('A' + i) + " " + names[i],
                ShortLabel(names[i]),
                new Vector2(0.05f, y - h),
                new Vector2(0.95f, y));

            y -= 0.055f;

            if (y < 0.02f)
                break;
        }
    }

    private static void BuildLayers(Transform root)
    {
        string[] names =
        {
            "BiomeTypes",
            "VegetationDensity",
            "FaunaDistribution",
            "WaterSystems",
            "SoilComposition",
            "ClimateBands",
            "LongTermWeatherPatterns",
            "EnvironmentalHazards",
            "ResourceNodes",
            "FactionControl",
            "WarfareZones",
            "LogisticsRoutes",
            "TerrainModifiers"
        };

        GameObject panel =
            Panel(root,
                "HUD-220-03 BiosphereLayersPanel",
                new Vector2(0.185f, 0.54f),
                new Vector2(0.39f, 0.90f));

        float y = 0.93f;

        for (int i = 0; i < names.Length; i++)
        {
            float h = 0.065f;

            Toggle(
                panel.transform,
                "HUD-220-03" +
                (char)('A' + i) +
                " LayerToggle_" + names[i],
                Pretty(names[i]),
                new Vector2(0.05f, y - h),
                new Vector2(0.95f, y));

            y -= 0.068f;
        }
    }

    private static void BuildLegend(Transform root)
    {
        string[] names =
        {
            "BiomeTypeLegend",
            "VegetationDensityLegend",
            "FaunaDistributionLegend",
            "WaterSystemLegend",
            "SoilCompositionLegend",
            "ClimateBandLegend",
            "EnvironmentalHazardLegend"
        };

        GameObject panel =
            Panel(root,
                "HUD-220-04 BiosphereLegendPanel",
                new Vector2(0.185f, 0.32f),
                new Vector2(0.39f, 0.51f));

        float y = 0.90f;

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                panel.transform,
                "HUD-220-04" +
                (char)('A' + i) + " " + names[i],
                Pretty(names[i]) +
                "   //   ● ● ● ●",
                new Vector2(0.05f, y - 0.105f),
                new Vector2(0.95f, y),
                12, false);

            y -= 0.125f;
        }
    }

    private static void BuildInfo(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD-220-05 BiosphereInfoPanel",
                new Vector2(0.40f, 0.48f),
                new Vector2(0.61f, 0.90f));

        string[] names =
        {
            "SelectedZoneName",
            "SelectedZoneBiomeType",
            "SelectedVegetationDensity",
            "SelectedFaunaDensity",
            "SelectedWaterSystemType",
            "SelectedSoilComposition",
            "SelectedClimateBand",
            "SelectedEnvironmentalHazardLevel"
        };

        string[] values =
        {
            "AURELIA NORTHERN FOREST",
            "BIOME // TEMPERATE FOREST",
            "VEGETATION DENSITY // 42%",
            "FAUNA DENSITY // 35%",
            "WATER // RIVER / LAKE NETWORK",
            "SOIL // RICH LOAM",
            "CLIMATE // TEMPERATE",
            "HAZARD LEVEL // LOW"
        };

        float y = 0.94f;

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                panel.transform,
                "HUD-220-05" +
                (char)('A' + i) + " " + names[i],
                values[i],
                new Vector2(0.06f, y - 0.095f),
                new Vector2(0.94f, y),
                12 + (i == 0 ? 3 : 0),
                i == 0);

            y -= 0.105f;
        }

        Button(
            panel.transform,
            "HUD-220-05I EnterZoneButton",
            "ENTER ZONE",
            new Vector2(0.25f, 0.03f),
            new Vector2(0.75f, 0.12f));
    }

    private static void BuildEcologicalStats(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD-220-06 EcologicalStatsPanel",
                new Vector2(0.62f, 0.65f),
                new Vector2(0.80f, 0.90f));

        string[] names =
        {
            "BiodiversityIndexDisplay",
            "BiomassDensityDisplay",
            "PredatorPreyBalanceDisplay",
            "EcosystemStabilityDisplay",
            "EnvironmentalStressDisplay",
            "ResourceRenewalRateDisplay"
        };

        string[] values =
        {
            "BIODIVERSITY INDEX // 74.0",
            "BIOMASS DENSITY // 68%",
            "PREDATOR / PREY BALANCE // 82%",
            "ECOSYSTEM STABILITY // 88%",
            "ENVIRONMENTAL STRESS // 19%",
            "RESOURCE RENEWAL RATE // 71%"
        };

        float y = 0.92f;

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                panel.transform,
                "HUD-220-06" +
                (char)('A' + i) + " " + names[i],
                values[i],
                new Vector2(0.05f, y - 0.13f),
                new Vector2(0.95f, y),
                12, false);

            y -= 0.145f;
        }
    }

    private static void BuildEcologicalImpact(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD-220-07 EcologicalImpactPanel",
                new Vector2(0.81f, 0.48f),
                new Vector2(0.995f, 0.90f));

        string[] names =
        {
            "WarfareImpactDisplay",
            "LogisticsImpactDisplay",
            "ClimateImpactDisplay",
            "HazardPropagationDisplay",
            "FactionActivityImpactDisplay"
        };

        string[] values =
        {
            "WARFARE IMPACT // 12%",
            "LOGISTICS IMPACT // 18%",
            "CLIMATE IMPACT // 24%",
            "HAZARD PROPAGATION // 15%",
            "FACTION ACTIVITY IMPACT // 21%"
        };

        float y = 0.92f;

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                panel.transform,
                "HUD-220-07" +
                (char)('A' + i) + " " + names[i],
                values[i],
                new Vector2(0.06f, y - 0.15f),
                new Vector2(0.94f, y),
                12, false);

            y -= 0.17f;
        }
    }

    private static void BuildQuickActions(Transform root)
    {
        string[] names =
        {
            "DeployReconButton",
            "DeployEcologySurveyTeamButton",
            "DeployAtmosphericDroneButton",
            "OpenIntelNetworkButton",
            "OpenDoctrineManagerButton",
            "OpenLogisticsOverviewButton",
            "OpenFactionDiplomacyButton"
        };

        GameObject panel =
            Panel(root,
                "HUD-220-08 QuickActionsPanel",
                new Vector2(0.62f, 0.40f),
                new Vector2(0.80f, 0.63f));

        float y = 0.91f;

        for (int i = 0; i < names.Length; i++)
        {
            Button(
                panel.transform,
                "HUD-220-08" +
                (char)('A' + i) + " " + names[i],
                ShortLabel(names[i]),
                new Vector2(0.05f, y - 0.105f),
                new Vector2(0.95f, y));

            y -= 0.12f;
        }
    }

    private static void BuildAlerts(Transform root)
    {
        string[] names =
        {
            "StrategicAlertEntry",
            "EcologicalAlertEntry",
            "EnvironmentalHazardAlertEntry",
            "LogisticsAlertEntry",
            "WeatherAlertEntry",
            "WarfareAlertEntry",
            "BiodiversityCollapseAlertEntry",
            "HazardPropagationAlertEntry"
        };

        string[] values =
        {
            "STRATEGIC // BIOSPHERE MONITORING NOMINAL",
            "ECOLOGICAL // BIODIVERSITY WITHIN PROJECTED RANGE",
            "HAZARD // NO CRITICAL PROPAGATION",
            "LOGISTICS // ECOLOGICAL ROUTES OPERATIONAL",
            "WEATHER // SEASONAL CONDITIONS STABLE",
            "WARFARE // NO SIGNIFICANT ECOLOGICAL DISRUPTION",
            "BIODIVERSITY // COLLAPSE RISK 4.2%",
            "HAZARD PROPAGATION // MONITORING ACTIVE"
        };

        GameObject panel =
            Panel(root,
                "HUD-220-09 AlertsPanel",
                new Vector2(0.185f, 0.04f),
                new Vector2(0.80f, 0.30f));

        float y = 0.93f;

        for (int i = 0; i < names.Length; i++)
        {
            Text(
                panel.transform,
                "HUD-220-09" +
                (char)('A' + i) + " " + names[i],
                values[i],
                new Vector2(0.03f, y - 0.105f),
                new Vector2(0.97f, y),
                11, false);

            y -= 0.11f;
        }
    }

    private static void BuildBottomBar(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD-220-10 BottomBar",
                new Vector2(0f, 0f),
                new Vector2(1f, 0.035f));

        string[] names =
        {
            "BackButton",
            "ExitToCommandCenterButton",
            "SettingsButton",
            "HelpButton"
        };

        string[] labels =
        {
            "BACK",
            "COMMAND CENTER",
            "SETTINGS",
            "HELP"
        };

        for (int i = 0; i < names.Length; i++)
        {
            float x0 = 0.01f + i * 0.15f;

            Button(
                panel.transform,
                "HUD-220-10" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(x0, 0.08f),
                new Vector2(x0 + 0.13f, 0.92f));
        }
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        Vector2 min,
        Vector2 max)
    {
        GameObject go =
            Create(parent, name);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(0.015f, 0.025f, 0.035f, 0.94f);

        image.raycastTarget = true;

        return go;
    }

    private static GameObject Create(
        Transform parent,
        string name)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform));

        go.transform.SetParent(
            parent,
            false);

        return go;
    }

    private static void Text(
        Transform parent,
        string name,
        string value,
        Vector2 min,
        Vector2 max,
        int size,
        bool bold)
    {
        GameObject go =
            Create(parent, name);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp =
            go.AddComponent<TextMeshProUGUI>();

        tmp.text = value;
        tmp.fontSize = size;
        tmp.fontStyle =
            bold
                ? FontStyles.Bold
                : FontStyles.Normal;

        tmp.color =
            new Color(0.78f, 0.86f, 0.90f, 1f);

        tmp.alignment =
            TextAlignmentOptions.Left;

        tmp.textWrappingMode =
            TextWrappingModes.Normal;

        tmp.raycastTarget = false;
    }

    private static void Button(
        Transform parent,
        string name,
        string label,
        Vector2 min,
        Vector2 max)
    {
        GameObject go =
            Create(parent, name);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(0.06f, 0.10f, 0.13f, 0.98f);

        Button button =
            go.AddComponent<Button>();

        button.targetGraphic = image;

        GameObject textObject =
            new GameObject(
                "Text",
                typeof(RectTransform));

        textObject.transform.SetParent(
            go.transform,
            false);

        RectTransform textRect =
            textObject.GetComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp =
            textObject.AddComponent<TextMeshProUGUI>();

        tmp.text = label;
        tmp.fontSize = 11;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = new Color(0.82f, 0.88f, 0.92f, 1f);
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.textWrappingMode = TextWrappingModes.Normal;
        tmp.raycastTarget = false;
    }

    private static void Toggle(
        Transform parent,
        string name,
        string label,
        Vector2 min,
        Vector2 max)
    {
        GameObject go =
            Create(parent, name);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Toggle toggle =
            go.AddComponent<Toggle>();

        GameObject background =
            new GameObject(
                "Background",
                typeof(RectTransform),
                typeof(Image));

        background.transform.SetParent(
            go.transform,
            false);

        RectTransform bgRect =
            background.GetComponent<RectTransform>();

        bgRect.anchorMin =
            new Vector2(0f, 0.15f);

        bgRect.anchorMax =
            new Vector2(0.08f, 0.85f);

        bgRect.offsetMin =
            Vector2.zero;

        bgRect.offsetMax =
            Vector2.zero;

        Image bg =
            background.GetComponent<Image>();

        bg.color =
            new Color(0.08f, 0.13f, 0.16f, 1f);

        GameObject check =
            new GameObject(
                "Checkmark",
                typeof(RectTransform),
                typeof(Image));

        check.transform.SetParent(
            background.transform,
            false);

        RectTransform checkRect =
            check.GetComponent<RectTransform>();

        checkRect.anchorMin = Vector2.zero;
        checkRect.anchorMax = Vector2.one;
        checkRect.offsetMin = Vector2.zero;
        checkRect.offsetMax = Vector2.zero;

        Image checkImage =
            check.GetComponent<Image>();

        checkImage.color =
            new Color(0.25f, 0.75f, 0.55f, 1f);

        toggle.graphic = checkImage;
        toggle.targetGraphic = bg;
        toggle.isOn = true;

        GameObject textObject =
            new GameObject(
                "Label",
                typeof(RectTransform));

        textObject.transform.SetParent(
            go.transform,
            false);

        RectTransform textRect =
            textObject.GetComponent<RectTransform>();

        textRect.anchorMin =
            new Vector2(0.11f, 0f);

        textRect.anchorMax =
            new Vector2(1f, 1f);

        textRect.offsetMin =
            Vector2.zero;

        textRect.offsetMax =
            Vector2.zero;

        TextMeshProUGUI tmp =
            textObject.AddComponent<TextMeshProUGUI>();

        tmp.text = label;
        tmp.fontSize = 11;
        tmp.color =
            new Color(0.76f, 0.84f, 0.88f, 1f);

        tmp.alignment =
            TextAlignmentOptions.Left;

        tmp.textWrappingMode =
            TextWrappingModes.Normal;

        tmp.raycastTarget = false;
    }

    private static string Pretty(string value)
    {
        string result = value.Replace("_", " ");

        System.Text.StringBuilder builder =
            new System.Text.StringBuilder();

        for (int i = 0; i < result.Length; i++)
        {
            char c = result[i];

            if (i > 0 &&
                char.IsUpper(c) &&
                !char.IsUpper(result[i - 1]))
            {
                builder.Append(' ');
            }

            builder.Append(c);
        }

        return builder.ToString().ToUpperInvariant();
    }

    private static string ShortLabel(string value)
    {
        string result =
            value
                .Replace("ReturnTo", "")
                .Replace("Button", "")
                .Replace("Deploy", "DEPLOY ")
                .Replace("Open", "OPEN ");

        return Pretty(result);
    }

    private static void SaveAndRegister(Scene scene)
    {
        EditorSceneManager.SaveScene(
            scene,
            ScenePath);

        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        bool exists = false;

        foreach (EditorBuildSettingsScene buildScene in scenes)
        {
            if (buildScene != null &&
                buildScene.path == ScenePath)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            scenes.Add(
                new EditorBuildSettingsScene(
                    ScenePath,
                    true));

            EditorBuildSettings.scenes =
                scenes.ToArray();
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
