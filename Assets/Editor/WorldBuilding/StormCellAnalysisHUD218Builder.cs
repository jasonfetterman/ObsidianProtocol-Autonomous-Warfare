using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public static class StormCellAnalysisHUD218Builder
{
    private const string ScenePath =
        "Assets/Scenes/SCN76_STORM_CELL_ANALYSIS/Storm_Cell_Analysis.unity";

    private const string RootName =
        "HUD\u2011218 StormCellAnalysisHUD";

    [MenuItem("Obsidian Protocol/Worlds/BUILD HUD-218 STORM CELL ANALYSIS")]
    public static void Build()
    {
        EnsureFolder();

        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single
        );

        BuildWorld();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        SaveScene();
        AddToBuildSettings();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[HUD-218] Storm Cell Analysis build complete.");
    }

    private static void EnsureFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        if (!AssetDatabase.IsValidFolder(
            "Assets/Scenes/SCN76_STORM_CELL_ANALYSIS"))
        {
            AssetDatabase.CreateFolder(
                "Assets/Scenes",
                "SCN76_STORM_CELL_ANALYSIS");
        }
    }

    private static void BuildWorld()
    {
        GameObject world =
            new GameObject("STORM_CELL_ANALYSIS_WORLD");

        GameObject planet =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        planet.name = "PLANET_SURFACE";
        planet.transform.SetParent(world.transform);
        planet.transform.position = Vector3.zero;
        planet.transform.localScale = Vector3.one * 18f;
        ApplyMaterial(planet, new Color(0.08f, 0.20f, 0.28f));

        GameObject atmosphere =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        atmosphere.name = "ATMOSPHERIC_SHELL";
        atmosphere.transform.SetParent(world.transform);
        atmosphere.transform.position = Vector3.zero;
        atmosphere.transform.localScale = Vector3.one * 18.8f;
        ApplyMaterial(atmosphere, new Color(0.10f, 0.32f, 0.48f));

        BuildStormLayer(world);
        BuildPrecipitationLayer(world);
        BuildWindLayer(world);
        BuildTurbulenceLayer(world);
        BuildVisibilityLayer(world);
        BuildPressureLayer(world);
        BuildDensityLayer(world);
        BuildWeatherLayer(world);
        BuildFactionLayer(world);
        BuildWarfareLayer(world);
        BuildLogisticsLayer(world);
        BuildTerrainLayer(world);

        GameObject lightObject = new GameObject("WORLD_LIGHT");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        light.transform.rotation =
            Quaternion.Euler(35f, -35f, 0f);
    }

    private static void BuildStormLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_STORM_CELLS");

        layer.transform.SetParent(world.transform);

        CreateStormCell(layer, "STORM_CELL_ALPHA",
            new Vector3(-5f, 5f, -3f), 3.2f);

        CreateStormCell(layer, "STORM_CELL_BRAVO",
            new Vector3(5f, 2f, -1f), 2.3f);

        CreateStormCell(layer, "STORM_CELL_CHARLIE",
            new Vector3(1f, -5f, 4f), 2.7f);
    }

    private static void CreateStormCell(
        GameObject parent,
        string name,
        Vector3 position,
        float size)
    {
        GameObject cell =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        cell.name = name;
        cell.transform.SetParent(parent.transform);
        cell.transform.position = position;
        cell.transform.localScale =
            Vector3.one * size;

        ApplyMaterial(
            cell,
            new Color(0.32f, 0.08f, 0.52f));
    }

    private static void BuildPrecipitationLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_PRECIPITATION");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 28; i++)
        {
            GameObject drop =
                GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            drop.name = "PRECIPITATION_" + i.ToString("00");
            drop.transform.SetParent(layer.transform);

            float x = Mathf.Sin(i * 1.71f) * 11f;
            float z = Mathf.Cos(i * 1.13f) * 11f;

            drop.transform.position =
                new Vector3(x, 7f + (i % 5), z);

            drop.transform.localScale =
                new Vector3(0.035f, 1.1f, 0.035f);

            ApplyMaterial(
                drop,
                new Color(0.20f, 0.48f, 0.90f));
        }
    }

    private static void BuildWindLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_WIND_SPEED");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 10; i++)
        {
            GameObject arrow =
                GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            arrow.name = "WIND_VECTOR_" + i.ToString("00");
            arrow.transform.SetParent(layer.transform);

            arrow.transform.position =
                new Vector3(
                    -9f + i * 2f,
                    3f + Mathf.Sin(i) * 3f,
                    -7f);

            arrow.transform.localScale =
                new Vector3(0.08f, 2.2f, 0.08f);

            arrow.transform.rotation =
                Quaternion.Euler(0f, 0f, -65f);

            ApplyMaterial(
                arrow,
                new Color(0.10f, 0.82f, 0.86f));
        }
    }

    private static void BuildTurbulenceLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_TURBULENCE");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 6; i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            zone.name = "TURBULENCE_ZONE_" + i.ToString("00");
            zone.transform.SetParent(layer.transform);

            zone.transform.position =
                new Vector3(
                    -7f + i * 2.8f,
                    0f,
                    5f);

            zone.transform.localScale =
                new Vector3(1.4f, 0.05f, 1.4f);

            ApplyMaterial(
                zone,
                new Color(0.72f, 0.18f, 0.75f));
        }
    }

    private static void BuildVisibilityLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_VISIBILITY");

        layer.transform.SetParent(world.transform);

        GameObject haze =
            GameObject.CreatePrimitive(PrimitiveType.Sphere);

        haze.name = "VISIBILITY_HAZE";
        haze.transform.SetParent(layer.transform);
        haze.transform.localScale = Vector3.one * 20.5f;

        ApplyMaterial(
            haze,
            new Color(0.24f, 0.28f, 0.32f));
    }

    private static void BuildPressureLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_PRESSURE");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 5; i++)
        {
            GameObject ring =
                GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            ring.name = "PRESSURE_GRADIENT_" + i.ToString("00");
            ring.transform.SetParent(layer.transform);

            ring.transform.position =
                new Vector3(
                    -5f + i * 2.5f,
                    -6f,
                    5f);

            ring.transform.localScale =
                new Vector3(
                    1.2f + i * 0.35f,
                    0.04f,
                    1.2f + i * 0.35f);

            ApplyMaterial(
                ring,
                new Color(0.12f, 0.45f, 0.95f));
        }
    }

    private static void BuildDensityLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_AIR_DENSITY");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 8; i++)
        {
            GameObject marker =
                GameObject.CreatePrimitive(PrimitiveType.Sphere);

            marker.name = "DENSITY_ZONE_" + i.ToString("00");
            marker.transform.SetParent(layer.transform);

            marker.transform.position =
                new Vector3(
                    Mathf.Sin(i * 0.8f) * 9f,
                    Mathf.Cos(i * 0.9f) * 6f,
                    2f);

            marker.transform.localScale =
                Vector3.one * 0.22f;

            ApplyMaterial(
                marker,
                new Color(0.92f, 0.76f, 0.16f));
        }
    }

    private static void BuildWeatherLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_WEATHER");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 7; i++)
        {
            GameObject cloud =
                GameObject.CreatePrimitive(PrimitiveType.Sphere);

            cloud.name = "WEATHER_SYSTEM_" + i.ToString("00");
            cloud.transform.SetParent(layer.transform);

            cloud.transform.position =
                new Vector3(
                    -8f + i * 2.6f,
                    8f + Mathf.Sin(i) * 1.5f,
                    4f);

            cloud.transform.localScale =
                new Vector3(2.5f, 0.8f, 1.4f);

            ApplyMaterial(
                cloud,
                new Color(0.48f, 0.50f, 0.54f));
        }
    }

    private static void BuildFactionLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_FACTION");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 6; i++)
        {
            GameObject marker =
                GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            marker.name = "FACTION_CONTROL_" + i.ToString("00");
            marker.transform.SetParent(layer.transform);

            marker.transform.position =
                new Vector3(
                    -8f + i * 3f,
                    -7f,
                    -2f);

            marker.transform.localScale =
                new Vector3(0.5f, 0.12f, 0.5f);

            ApplyMaterial(
                marker,
                new Color(0.10f, 0.68f, 0.30f));
        }
    }

    private static void BuildWarfareLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_WARFARE");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 5; i++)
        {
            GameObject zone =
                GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            zone.name = "WARFARE_ZONE_" + i.ToString("00");
            zone.transform.SetParent(layer.transform);

            zone.transform.position =
                new Vector3(
                    -6f + i * 3f,
                    -7.3f,
                    6f);

            zone.transform.localScale =
                new Vector3(0.7f, 0.08f, 0.7f);

            ApplyMaterial(
                zone,
                new Color(0.72f, 0.10f, 0.12f));
        }
    }

    private static void BuildLogisticsLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_LOGISTICS");

        layer.transform.SetParent(world.transform);

        GameObject route =
            new GameObject("LOGISTICS_ROUTE_MAIN");

        route.transform.SetParent(layer.transform);

        LineRenderer line =
            route.AddComponent<LineRenderer>();

        line.positionCount = 5;
        line.startWidth = 0.08f;
        line.endWidth = 0.08f;
        line.material =
            MakeMaterial(new Color(0.88f, 0.72f, 0.16f));

        line.SetPosition(0, new Vector3(-10f, -6f, -6f));
        line.SetPosition(1, new Vector3(-5f, -4f, -3f));
        line.SetPosition(2, new Vector3(0f, -5f, 0f));
        line.SetPosition(3, new Vector3(5f, -3f, 3f));
        line.SetPosition(4, new Vector3(10f, -5f, 6f));
    }

    private static void BuildTerrainLayer(GameObject world)
    {
        GameObject layer =
            new GameObject("STORM_LAYER_TERRAIN");

        layer.transform.SetParent(world.transform);

        for (int i = 0; i < 7; i++)
        {
            GameObject modifier =
                GameObject.CreatePrimitive(PrimitiveType.Cube);

            modifier.name = "TERRAIN_MODIFIER_" + i.ToString("00");
            modifier.transform.SetParent(layer.transform);

            modifier.transform.position =
                new Vector3(
                    -9f + i * 3f,
                    -8f,
                    -8f);

            modifier.transform.localScale =
                new Vector3(1.2f, 0.2f, 0.8f);

            ApplyMaterial(
                modifier,
                new Color(0.34f, 0.28f, 0.18f));
        }
    }

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject("HUD218_Camera");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.orthographic = true;
        camera.orthographicSize = 22f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor =
            new Color(0.008f, 0.015f, 0.025f);

        cameraObject.transform.position =
            new Vector3(0f, 7f, -32f);

        cameraObject.transform.rotation =
            Quaternion.Euler(8f, 0f, 0f);

        cameraObject.tag = "MainCamera";
    }

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            new GameObject(
                "EventSystem",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));
    }

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "HUD218_Canvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.Expand;

        GameObject root =
            Panel(
                canvasObject.transform,
                RootName,
                Vector2.zero,
                Vector2.zero,
                new Color(0f, 0f, 0f, 0f));

        RectTransform rootRect =
            root.GetComponent<RectTransform>();

        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.offsetMin = Vector2.zero;
        rootRect.offsetMax = Vector2.zero;

        BuildTopBar(root.transform);
        BuildNavigation(root.transform);
        BuildLayers(root.transform);
        BuildLegend(root.transform);
        BuildInfo(root.transform);
        BuildTrajectory(root.transform);
        BuildImpact(root.transform);
        BuildForecast(root.transform);
        BuildQuickActions(root.transform);
        BuildAlerts(root.transform);
        BuildBottomBar(root.transform);

        root.AddComponent<StormCellAnalysisHUD218Controller>();
    }

    private static void BuildTopBar(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201101 TopBar",
                new Vector2(0f, 495f),
                new Vector2(1880f, 70f),
                new Color(0.025f, 0.05f, 0.08f, 0.97f));

        Label(panel.transform,
            "HUD\u2011218\u201101A PlanetNameDisplay",
            "PLANET // AURELIA",
            new Vector2(-720f, 0f),
            new Vector2(220f, 50f),
            18);

        Label(panel.transform,
            "HUD\u2011218\u201101B RegionNameDisplay",
            "REGION // NORTHERN CONTINENT",
            new Vector2(-440f, 0f),
            new Vector2(300f, 50f),
            16);

        Label(panel.transform,
            "HUD\u2011218\u201101C StormCellIDDisplay",
            "CELL // SC-218",
            new Vector2(-90f, 0f),
            new Vector2(180f, 50f),
            18);

        Label(panel.transform,
            "HUD\u2011218\u201101D StormCellCategoryDisplay",
            "CATEGORY // SUPERCELL",
            new Vector2(140f, 0f),
            new Vector2(240f, 50f),
            16);

        Label(panel.transform,
            "HUD\u2011218\u201101E OwnerFactionEmblem",
            "A",
            new Vector2(430f, 0f),
            new Vector2(55f, 50f),
            22);

        Label(panel.transform,
            "HUD\u2011218\u201101F PlayerResourcesPanel",
            "RESOURCES // 84,250 C // 6,420 MAT",
            new Vector2(610f, 0f),
            new Vector2(310f, 50f),
            15);

        Label(panel.transform,
            "HUD\u2011218\u201101G TimeControlPanel",
            "SIMULATION // RUNNING x1.00",
            new Vector2(825f, 0f),
            new Vector2(260f, 50f),
            15);
    }

    private static void BuildNavigation(Transform root)
    {
        string panelName =
            "HUD\u2011218\u201102 NavigationPanel";

        GameObject panel =
            Panel(root,
                panelName,
                new Vector2(-790f, 115f),
                new Vector2(270f, 700f),
                new Color(0.025f, 0.045f, 0.07f, 0.96f));

        string[] names =
        {
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

        string[] labels =
        {
            "ATMOSPHERIC SIM",
            "WEATHER SYSTEM",
            "LOGISTICS NETWORK",
            "RESOURCE DISTRIBUTION",
            "STRATEGIC HEATMAP",
            "FACTION TERRITORY",
            "FACTION CONTROL",
            "WARFARE PROFILE",
            "ENVIRONMENT PROFILE",
            "ISLAND MAP",
            "BIOME MAP",
            "REGION MAP",
            "CONTINENT MAP",
            "SURFACE MAP",
            "PLANET OVERVIEW"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button(
                panel.transform,
                "HUD\u2011218\u201102" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 315f - i * 42f),
                new Vector2(240f, 34f));
        }
    }

    private static void BuildLayers(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201103 StormCellLayersPanel",
                new Vector2(-490f, 185f),
                new Vector2(280f, 560f),
                new Color(0.025f, 0.045f, 0.07f, 0.96f));

        string[] names =
        {
            "LayerToggle_StormCells",
            "LayerToggle_Precipitation",
            "LayerToggle_WindSpeed",
            "LayerToggle_TurbulenceZones",
            "LayerToggle_Visibility",
            "LayerToggle_AtmosphericPressure",
            "LayerToggle_AirDensity",
            "LayerToggle_WeatherSystems",
            "LayerToggle_FactionControl",
            "LayerToggle_WarfareZones",
            "LayerToggle_LogisticsRoutes",
            "LayerToggle_TerrainModifiers"
        };

        string[] labels =
        {
            "STORM CELLS  [ON]",
            "PRECIPITATION  [ON]",
            "WIND SPEED  [ON]",
            "TURBULENCE  [ON]",
            "VISIBILITY  [ON]",
            "PRESSURE  [ON]",
            "AIR DENSITY  [ON]",
            "WEATHER SYSTEMS  [ON]",
            "FACTION CONTROL  [ON]",
            "WARFARE ZONES  [ON]",
            "LOGISTICS ROUTES  [ON]",
            "TERRAIN MODIFIERS  [ON]"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button(
                panel.transform,
                "HUD\u2011218\u201103" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 220f - i * 38f),
                new Vector2(245f, 31f));
        }
    }

    private static void BuildLegend(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201104 StormCellLegendPanel",
                new Vector2(-490f, -235f),
                new Vector2(280f, 250f),
                new Color(0.025f, 0.045f, 0.07f, 0.96f));

        string[] labels =
        {
            "STORM INTENSITY // LOW -> EXTREME",
            "PRECIPITATION // MM/H",
            "WIND SPEED // KM/H",
            "TURBULENCE // INDEX",
            "VISIBILITY // % REMAINING",
            "PRESSURE GRADIENT // HPA"
        };

        for (int i = 0; i < labels.Length; i++)
        {
            Label(
                panel.transform,
                "HUD\u2011218\u201104" +
                (char)('A' + i) + " " +
                new string[]
                {
                    "StormIntensityLegend",
                    "PrecipitationLegend",
                    "WindSpeedLegend",
                    "TurbulenceLegend",
                    "VisibilityLegend",
                    "PressureGradientLegend"
                }[i],
                labels[i],
                new Vector2(0f, 82f - i * 32f),
                new Vector2(250f, 28f),
                13);
        }
    }

    private static void BuildInfo(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201105 StormCellInfoPanel",
                new Vector2(540f, 250f),
                new Vector2(360f, 400f),
                new Color(0.025f, 0.045f, 0.07f, 0.97f));

        string[] names =
        {
            "StormCellNameDisplay",
            "StormCellCategoryDisplay",
            "StormCellIntensityDisplay",
            "StormCellWindSpeedDisplay",
            "StormCellPrecipitationDisplay",
            "StormCellPressureDisplay",
            "StormCellAirDensityDisplay",
            "StormCellTurbulenceDisplay",
            "StormCellVisibilityDisplay"
        };

        string[] labels =
        {
            "NAME // STORM CELL ALPHA",
            "CATEGORY // SUPERCELL",
            "INTENSITY // 68%",
            "WIND // 92 KM/H",
            "PRECIPITATION // 74%",
            "PRESSURE // 952 HPA",
            "AIR DENSITY // 1.08 KG/M³",
            "TURBULENCE // 71%",
            "VISIBILITY // 34%"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD\u2011218\u201105" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 145f - i * 34f),
                new Vector2(320f, 28f),
                14);
        }

        Button(
            panel.transform,
            "HUD\u2011218\u201105J EnterStormCellButton",
            "ENTER STORM CELL",
            new Vector2(0f, -170f),
            new Vector2(300f, 38f));
    }

    private static void BuildTrajectory(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201106 TrajectoryPanel",
                new Vector2(540f, -100f),
                new Vector2(360f, 230f),
                new Color(0.025f, 0.045f, 0.07f, 0.97f));

        string[] names =
        {
            "CurrentTrajectoryDisplay",
            "PredictedTrajectoryDisplay",
            "TrajectoryConfidenceDisplay",
            "StormSpeedDisplay",
            "StormDirectionDisplay",
            "LandfallPredictionDisplay"
        };

        string[] labels =
        {
            "CURRENT // 032° NE",
            "PREDICTED // 047° NE",
            "CONFIDENCE // 91%",
            "SPEED // 19.3 KM/H",
            "DIRECTION // NORTHEAST",
            "LANDFALL // 18H 42M"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD\u2011218\u201106" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 78f - i * 30f),
                new Vector2(320f, 26f),
                13);
        }
    }

    private static void BuildImpact(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201107 ImpactAnalysisPanel",
                new Vector2(155f, -270f),
                new Vector2(350f, 230f),
                new Color(0.025f, 0.045f, 0.07f, 0.97f));

        string[] names =
        {
            "MobilityImpactDisplay",
            "ReconVisibilityImpactDisplay",
            "SensorInterferenceImpactDisplay",
            "LogisticsDifficultyImpactDisplay",
            "CombatEffectivenessImpactDisplay",
            "EnvironmentalHazardLevelDisplay"
        };

        string[] labels =
        {
            "MOBILITY IMPACT // -42%",
            "RECON VISIBILITY // -58%",
            "SENSOR INTERFERENCE // +31%",
            "LOGISTICS DIFFICULTY // +47%",
            "COMBAT EFFECTIVENESS // -19%",
            "ENVIRONMENTAL HAZARD // HIGH"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD\u2011218\u201107" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 78f - i * 30f),
                new Vector2(315f, 26f),
                12);
        }
    }

    private static void BuildForecast(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201108 ForecastPanel",
                new Vector2(-215f, -270f),
                new Vector2(350f, 230f),
                new Color(0.025f, 0.045f, 0.07f, 0.97f));

        string[] names =
        {
            "ShortTermForecastDisplay",
            "MidTermForecastDisplay",
            "LongTermForecastDisplay",
            "StormGrowthProbabilityDisplay",
            "StormDecayProbabilityDisplay",
            "AtmosphericInstabilityDisplay"
        };

        string[] labels =
        {
            "0-6H // INTENSIFICATION EXPECTED",
            "6-18H // EASTWARD PROPAGATION",
            "18-48H // GRADUAL DECAY",
            "GROWTH PROBABILITY // 64%",
            "DECAY PROBABILITY // 36%",
            "INSTABILITY // SEVERE"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD\u2011218\u201108" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 78f - i * 30f),
                new Vector2(315f, 26f),
                12);
        }
    }

    private static void BuildQuickActions(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201109 QuickActionsPanel",
                new Vector2(-215f, 35f),
                new Vector2(350f, 240f),
                new Color(0.025f, 0.045f, 0.07f, 0.97f));

        string[] names =
        {
            "DeployReconButton",
            "DeployWeatherMonitoringTeamButton",
            "DeployAtmosphericDroneButton",
            "OpenIntelNetworkButton",
            "OpenDoctrineManagerButton",
            "OpenLogisticsOverviewButton",
            "OpenFactionDiplomacyButton"
        };

        string[] labels =
        {
            "DEPLOY RECON",
            "DEPLOY WEATHER TEAM",
            "DEPLOY ATMOSPHERIC DRONE",
            "OPEN INTEL NETWORK",
            "OPEN DOCTRINE MANAGER",
            "OPEN LOGISTICS OVERVIEW",
            "OPEN FACTION DIPLOMACY"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Button(
                panel.transform,
                "HUD\u2011218\u201109" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 92f - i * 30f),
                new Vector2(315f, 25f));
        }
    }

    private static void BuildAlerts(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201110 AlertsPanel",
                new Vector2(585f, -315f),
                new Vector2(450f, 230f),
                new Color(0.025f, 0.045f, 0.07f, 0.97f));

        string[] names =
        {
            "StrategicAlertEntry",
            "WeatherAlertEntry",
            "EnvironmentalHazardAlertEntry",
            "LogisticsAlertEntry",
            "WarfareAlertEntry",
            "StormTrajectoryAlertEntry",
            "VisibilityDropAlertEntry",
            "AtmosphericInstabilityAlertEntry"
        };

        string[] labels =
        {
            "STRATEGIC // CELL TRACKING ACTIVE",
            "WEATHER // SEVERE CELL DETECTED",
            "HAZARD // HIGH",
            "LOGISTICS // ROUTE RISK HIGH",
            "WARFARE // AIR OPERATIONS DEGRADED",
            "TRAJECTORY // MODEL ACTIVE",
            "VISIBILITY // RAPID DROP POSSIBLE",
            "ATMOSPHERE // INSTABILITY ELEVATED"
        };

        for (int i = 0; i < names.Length; i++)
        {
            Label(
                panel.transform,
                "HUD\u2011218\u201110" +
                (char)('A' + i) + " " + names[i],
                labels[i],
                new Vector2(0f, 82f - i * 27f),
                new Vector2(415f, 24f),
                11);
        }
    }

    private static void BuildBottomBar(Transform root)
    {
        GameObject panel =
            Panel(root,
                "HUD\u2011218\u201111 BottomBar",
                new Vector2(0f, -495f),
                new Vector2(1880f, 60f),
                new Color(0.025f, 0.05f, 0.08f, 0.98f));

        Button(
            panel.transform,
            "HUD\u2011218\u201111A BackButton",
            "BACK",
            new Vector2(-600f, 0f),
            new Vector2(180f, 34f));

        Button(
            panel.transform,
            "HUD\u2011218\u201111B ExitToCommandCenterButton",
            "COMMAND CENTER",
            new Vector2(-180f, 0f),
            new Vector2(220f, 34f));

        Button(
            panel.transform,
            "HUD\u2011218\u201111C SettingsButton",
            "SETTINGS",
            new Vector2(180f, 0f),
            new Vector2(180f, 34f));

        Button(
            panel.transform,
            "HUD\u2011218\u201111D HelpButton",
            "HELP",
            new Vector2(600f, 0f),
            new Vector2(180f, 34f));
    }

    private static GameObject Panel(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size,
        Color color)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        return go;
    }

    private static void Label(
        Transform parent,
        string name,
        string text,
        Vector2 position,
        Vector2 size,
        float fontSize)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

        go.transform.SetParent(parent, false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        TextMeshProUGUI tmp =
            go.GetComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.textWrappingMode = TextWrappingModes.Normal;
    }

    private static void Button(
        Transform parent,
        string name,
        string text,
        Vector2 position,
        Vector2 size)
    {
        GameObject go = new GameObject(name);

        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = go.AddComponent<Image>();

        image.color =
            new Color(0.06f, 0.12f, 0.17f, 1f);

        Button button = go.AddComponent<Button>();

        button.targetGraphic = image;

        ColorBlock colors = button.colors;

        colors.normalColor =
            new Color(0.06f, 0.12f, 0.17f, 1f);

        colors.highlightedColor =
            new Color(0.12f, 0.26f, 0.34f, 1f);

        colors.pressedColor =
            new Color(0.18f, 0.36f, 0.42f, 1f);

        colors.selectedColor =
            new Color(0.12f, 0.26f, 0.34f, 1f);

        button.colors = colors;

        GameObject textObject = new GameObject("Text");

        textObject.transform.SetParent(go.transform, false);

        RectTransform textRect =
            textObject.AddComponent<RectTransform>();

        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp =
            textObject.AddComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.fontSize = 12f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.textWrappingMode = TextWrappingModes.Normal;
    }
    private static void ApplyMaterial(
        GameObject go,
        Color color)
    {
        Renderer renderer =
            go.GetComponent<Renderer>();

        if (renderer != null)
            renderer.sharedMaterial = MakeMaterial(color);
    }

    private static Material MakeMaterial(Color color)
    {
        Shader shader =
            Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material material =
            new Material(shader);

        material.color = color;

        return material;
    }

    private static void SaveScene()
    {
        EditorSceneManager.SaveScene(
            SceneManager.GetActiveScene(),
            ScenePath);
    }

    private static void AddToBuildSettings()
    {
        List<EditorBuildSettingsScene> scenes =
            new List<EditorBuildSettingsScene>(
                EditorBuildSettings.scenes);

        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].path == ScenePath)
                return;
        }

        scenes.Add(
            new EditorBuildSettingsScene(
                ScenePath,
                true));

        EditorBuildSettings.scenes =
            scenes.ToArray();
    }
}


