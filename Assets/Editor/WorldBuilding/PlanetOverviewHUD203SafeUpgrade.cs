using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public static class PlanetOverviewHUD203SafeUpgrade
{
    private const string ScenePath =
        "Assets/Scenes/SCN62_PLANET_OVERVIEW/Planet_Overview.unity";

    private const string PrefabFolder = "Assets/Art/Models/Planets/";

    private static readonly string[] PrefabPaths =
    {
        PrefabFolder + "Acheron/ACHERON_REALISTIC_PLANET.prefab",
        PrefabFolder + "Virellia/VIRELLIA_PLANET.prefab",
        PrefabFolder + "Karthos/KARTHOS_PLANET.prefab",
        PrefabFolder + "Solara Prime/SOLARA_PRIME_PLANET.prefab",
        PrefabFolder + "Nox Meridian/NOX_MERIDIAN_PLANET.prefab"
    };

    private static readonly string[] PlanetNames =
    {
        "ACHERON",
        "VIRELLIA",
        "KARTHOS",
        "SOLARA PRIME",
        "NOX MERIDIAN"
    };

    [MenuItem("Obsidian Protocol/Worlds/HUD-203/SAFE UPGRADE FIVE PLANETS")]
    public static void Build()
    {
        if (!File.Exists(ScenePath))
        {
            Debug.LogError("[HUD-203] Scene not found: " + ScenePath);
            return;
        }

        GameObject[] prefabs = new GameObject[PrefabPaths.Length];

        for (int i = 0; i < PrefabPaths.Length; i++)
        {
            prefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(
                PrefabPaths[i]);

            if (prefabs[i] == null)
            {
                Debug.LogError(
                    "[HUD-203] Missing prefab. No scene changes made: " +
                    PrefabPaths[i]);
                return;
            }
        }

        GameObject[] existingController =
            AssetDatabase.LoadAssetAtPath<GameObject>(ScenePath) == null
                ? null
                : null;

        string backupPath = ScenePath + ".pre_HUD203_upgrade_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";

        File.Copy(ScenePath, backupPath, false);

        Scene scene = EditorSceneManager.OpenScene(
            ScenePath, OpenSceneMode.Single);

        GameObject canvasObject = FindSceneObject("HUD-203 PlanetOverviewHUD");

        if (canvasObject == null)
        {
            Debug.LogError(
                "[HUD-203] Existing HUD canvas not found. " +
                "Backup created at " + backupPath);
            return;
        }

        Canvas canvas = canvasObject.GetComponent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError(
                "[HUD-203] Expected Canvas component is missing. " +
                "Backup created at " + backupPath);
            return;
        }

        GameObject controllerObject =
            FindSceneObject("HUD-203 Runtime Controller");

        if (controllerObject == null)
        {
            controllerObject = new GameObject(
                "HUD-203 Runtime Controller");
            Undo.RegisterCreatedObjectUndo(
                controllerObject, "Create HUD-203 Controller");
        }

        PlanetOverviewHUD203RuntimeController controller =
            controllerObject.GetComponent<
                PlanetOverviewHUD203RuntimeController>();

        if (controller == null)
            controller = controllerObject.AddComponent<
                PlanetOverviewHUD203RuntimeController>();

        GameObject mountObject = FindSceneObject(
            "HUD-203 REAL PLANET DISPLAY MOUNT");

        if (mountObject == null)
        {
            mountObject = new GameObject(
                "HUD-203 REAL PLANET DISPLAY MOUNT");
            Undo.RegisterCreatedObjectUndo(
                mountObject, "Create HUD-203 Planet Mount");
        }

        mountObject.transform.SetParent(null);
        mountObject.transform.position = new Vector3(0f, -0.8f, 4f);
        mountObject.transform.rotation = Quaternion.identity;
        mountObject.transform.localScale = Vector3.one;

        Transform mount = mountObject.transform;

        for (int i = mount.childCount - 1; i >= 0; i--)
            Undo.DestroyObjectImmediate(mount.GetChild(i).gameObject);

        RemovePlaceholderObjects();

        GameObject preview = (GameObject)PrefabUtility.InstantiatePrefab(
            prefabs[0], scene);

        if (preview == null)
        {
            Debug.LogError(
                "[HUD-203] Could not instantiate Acheron preview. " +
                "Backup created at " + backupPath);
            return;
        }

        Undo.RegisterCreatedObjectUndo(preview, "Create Acheron Preview");
        preview.name = PlanetNames[0] + " - HUD203 PLANET";
        preview.transform.SetParent(mount, false);
        FitPlanet(preview, 9f);

        TMP_Text selectedLabel = FindText(
            "HUD-203-05A SelectedObjectName");

        TMP_Text statusLabel = FindText(
            "HUD-203 Runtime Status");

        if (statusLabel == null)
        {
            statusLabel = CreateStatusLabel(canvasObject.transform);
        }

        Button[] selectorButtons = CreatePlanetSelectors(
            canvasObject.transform);
        RepairPanelLayout(canvasObject.transform);

        SerializedObject serializedController =
            new SerializedObject(controller);

        serializedController.FindProperty("planetMount").objectReferenceValue =
            mount;

        SerializedProperty prefabArray =
            serializedController.FindProperty("planetPrefabs");

        prefabArray.arraySize = prefabs.Length;

        for (int i = 0; i < prefabs.Length; i++)
            prefabArray.GetArrayElementAtIndex(i).objectReferenceValue =
                prefabs[i];

        SerializedProperty buttonArray =
            serializedController.FindProperty("planetButtons");

        buttonArray.arraySize = selectorButtons.Length;

        for (int i = 0; i < selectorButtons.Length; i++)
            buttonArray.GetArrayElementAtIndex(i).objectReferenceValue =
                selectorButtons[i];

        serializedController.FindProperty("selectedPlanetLabel")
            .objectReferenceValue = selectedLabel;

        serializedController.FindProperty("statusLabel")
            .objectReferenceValue = statusLabel;

        serializedController.FindProperty("targetPlanetDiameter")
            .floatValue = 9f;

        serializedController.FindProperty("activePlanet")
            .objectReferenceValue = preview;

        serializedController.FindProperty("selectedPlanetIndex")
            .intValue = 0;

        serializedController.ApplyModifiedPropertiesWithoutUndo();

        UpgradeReadableText(canvasObject);

        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError(
                "[HUD-203] Scene save failed. Backup: " + backupPath);
            return;
        }

        Debug.Log(
            "[HUD-203] SAFE FIVE-PLANET UPGRADE COMPLETE. " +
            "Existing HUD preserved; Acheron preview installed; " +
            "five prefab references and selector buttons assigned. " +
            "Backup: " + backupPath);
    }

    private static GameObject FindSceneObject(string name)
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject item in objects)
        {
            if (item != null &&
                item.name == name &&
                item.scene.IsValid() &&
                !EditorUtility.IsPersistent(item))
                return item;
        }

        return null;
    }

    private static TMP_Text FindText(string objectName)
    {
        GameObject item = FindSceneObject(objectName);

        return item != null ? item.GetComponent<TMP_Text>() : null;
    }

    private static void RemovePlaceholderObjects()
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject item in objects)
        {
            if (item == null || !item.scene.IsValid() ||
                EditorUtility.IsPersistent(item))
                continue;

            bool placeholder =
                item.name == "SELECTED PLANET - PLANET OVERVIEW" ||
                (item.name.StartsWith("SURFACE_REGION_") &&
                 item.name.Length == "SURFACE_REGION_00".Length);

            if (placeholder)
                Undo.DestroyObjectImmediate(item);
        }
    }

    private static void FitPlanet(GameObject planet, float diameter)
    {
        planet.transform.localPosition = Vector3.zero;
        planet.transform.localRotation = Quaternion.identity;
        planet.transform.localScale = Vector3.one;

        Renderer[] renderers =
            planet.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogError(
                "[HUD-203] Acheron prefab has no child renderers.");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);

        float largest = Mathf.Max(
            bounds.size.x, bounds.size.y, bounds.size.z);

        if (largest <= 0.001f)
        {
            Debug.LogError(
                "[HUD-203] Acheron prefab has invalid renderer bounds.");
            return;
        }

        planet.transform.localScale = Vector3.one * (diameter / largest);
        planet.transform.localPosition = Vector3.zero;
    }

    private static Button[] CreatePlanetSelectors(Transform canvas)
    {
        Transform old = canvas.Find("HUD-203 Five Planet Selector");

        if (old != null)
            Undo.DestroyObjectImmediate(old.gameObject);

        GameObject panel = new GameObject(
            "HUD-203 Five Planet Selector",
            typeof(RectTransform),
            typeof(Image));

        Undo.RegisterCreatedObjectUndo(panel, "Create Planet Selector");
        panel.transform.SetParent(canvas, false);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.24f, 0.82f);
        panelRect.anchorMax = new Vector2(0.76f, 0.89f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        panel.GetComponent<Image>().color =
            new Color(0.015f, 0.035f, 0.06f, 0.94f);

        Button[] buttons = new Button[PlanetNames.Length];

        for (int i = 0; i < PlanetNames.Length; i++)
        {
            GameObject buttonObject = new GameObject(
                "HUD-203 Select " + PlanetNames[i],
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

            buttonObject.transform.SetParent(panel.transform, false);

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            float minX = i / 5f;
            float maxX = (i + 1) / 5f;

            rect.anchorMin = new Vector2(minX, 0.08f);
            rect.anchorMax = new Vector2(maxX, 0.92f);
            rect.offsetMin = new Vector2(4f, 2f);
            rect.offsetMax = new Vector2(-4f, -2f);

            Image image = buttonObject.GetComponent<Image>();
            image.color = i == 0
                ? new Color(0.04f, 0.27f, 0.39f, 1f)
                : new Color(0.055f, 0.085f, 0.12f, 1f);

            Button button = buttonObject.GetComponent<Button>();
            button.targetGraphic = image;

            ColorBlock colors = button.colors;
            colors.normalColor = image.color;
            colors.highlightedColor = new Color(0.08f, 0.32f, 0.44f, 1f);
            colors.pressedColor = new Color(0.1f, 0.55f, 0.7f, 1f);
            colors.selectedColor = colors.highlightedColor;
            button.colors = colors;

            GameObject labelObject = new GameObject(
                "Label",
                typeof(RectTransform),
                typeof(TextMeshProUGUI));

            labelObject.transform.SetParent(buttonObject.transform, false);

            RectTransform labelRect =
                labelObject.GetComponent<RectTransform>();

            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = new Vector2(3f, 2f);
            labelRect.offsetMax = new Vector2(-3f, -2f);

            TextMeshProUGUI label =
                labelObject.GetComponent<TextMeshProUGUI>();

            label.text = PlanetNames[i];
            label.fontSize = 18f;
            label.color = Color.white;
            label.alignment = TextAlignmentOptions.Center;
            label.enableAutoSizing = true;
            label.fontSizeMin = 18f;
            label.fontSizeMax = 22f;
            label.textWrappingMode = TextWrappingModes.Normal;
            label.raycastTarget = false;

            buttons[i] = button;
        }

        return buttons;
    }

    private static TMP_Text CreateStatusLabel(Transform canvas)
    {
        GameObject labelObject = new GameObject(
            "HUD-203 Runtime Status",
            typeof(RectTransform),
            typeof(TextMeshProUGUI));

        labelObject.transform.SetParent(canvas, false);

        RectTransform rect = labelObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.24f, 0.775f);
        rect.anchorMax = new Vector2(0.76f, 0.815f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        TMP_Text text = labelObject.GetComponent<TMP_Text>();
        text.text = "ACHERON SELECTED";
        text.fontSize = 18f;
        text.color = new Color(0.25f, 0.85f, 1f, 1f);
        text.alignment = TextAlignmentOptions.Center;
        text.raycastTarget = false;

        return text;
    }

    private static void RepairPanelLayout(Transform canvas)
    {
        SetPanelRect(canvas, "HUD-203-01 TopBar",
            new Vector2(0.02f, 0.92f), new Vector2(0.98f, 0.99f));

        SetPanelRect(canvas, "HUD-203-02 NavigationPanel",
            new Vector2(0.02f, 0.16f), new Vector2(0.17f, 0.90f));

        SetPanelRect(canvas, "HUD-203-03 PlanetStatsPanel",
            new Vector2(0.82f, 0.54f), new Vector2(0.98f, 0.90f));

        SetPanelRect(canvas, "HUD-203-04 PlanetLayersPanel",
            new Vector2(0.82f, 0.28f), new Vector2(0.98f, 0.51f));

        SetPanelRect(canvas, "HUD-203-05 PlanetObjectInfoPanel",
            new Vector2(0.19f, 0.13f), new Vector2(0.57f, 0.31f));

        SetPanelRect(canvas, "HUD-203-06 QuickActionsPanel",
            new Vector2(0.59f, 0.14f), new Vector2(0.80f, 0.40f));

        SetPanelRect(canvas, "HUD-203-07 AlertsPanel",
            new Vector2(0.59f, 0.43f), new Vector2(0.80f, 0.66f));

        SetPanelRect(canvas, "HUD-203-08 BottomBar",
            new Vector2(0.19f, 0.025f), new Vector2(0.80f, 0.10f));

        SetPanelRect(canvas, "HUD-203 Five Planet Selector",
            new Vector2(0.19f, 0.82f), new Vector2(0.80f, 0.91f));

        SetPanelRect(canvas, "HUD-203 Runtime Status",
            new Vector2(0.19f, 0.775f), new Vector2(0.80f, 0.815f));

        Transform top = canvas.Find("HUD-203-01 TopBar");
        if (top != null)
        {
            SetDescendantRect(top, "PlanetNameDisplay",
                new Vector2(0.01f, 0.10f), new Vector2(0.22f, 0.90f));
            SetDescendantRect(top, "PlanetTypeDisplay",
                new Vector2(0.23f, 0.10f), new Vector2(0.48f, 0.90f));
            SetDescendantRect(top, "PlanetOwnerFactionEmblem",
                new Vector2(0.50f, 0.10f), new Vector2(0.60f, 0.90f));
            SetDescendantRect(top, "PlayerResourcesPanel",
                new Vector2(0.61f, 0.10f), new Vector2(0.81f, 0.90f));
            SetDescendantRect(top, "TimeControlPanel",
                new Vector2(0.82f, 0.10f), new Vector2(0.99f, 0.90f));
        }

        Transform nav = canvas.Find("HUD-203-02 NavigationPanel");
        if (nav != null)
        {
            string[] names =
            {
                "ReturnToStarSystemButton", "ReturnToGalaxyMapButton",
                "SurfaceMapButton", "ContinentMapButton",
                "RegionMapButton", "IslandMapButton", "BiomeMapButton"
            };

            for (int i = 0; i < names.Length; i++)
            {
                float topY = 0.97f - i * 0.14f;
                float bottomY = topY - 0.12f;

                SetDescendantRect(nav, names[i],
                    new Vector2(0.06f, bottomY),
                    new Vector2(0.94f, topY));
            }
        }

        Transform stats = canvas.Find("HUD-203-03 PlanetStatsPanel");
        if (stats != null)
        {
            string[] names =
            {
                "PopulationDisplay", "StabilityDisplay",
                "ResourceOutputDisplay", "LogisticsCapacityDisplay",
                "ThreatLevelDisplay", "EnvironmentTypeDisplay"
            };

            for (int i = 0; i < names.Length; i++)
            {
                float topY = 0.97f - i * 0.16f;
                float bottomY = topY - 0.14f;

                SetDescendantRect(stats, names[i],
                    new Vector2(0.05f, bottomY),
                    new Vector2(0.95f, topY));
            }
        }

        Transform layers = canvas.Find("HUD-203-04 PlanetLayersPanel");
        if (layers != null)
        {
            string[] names =
            {
                "LayerToggle_Continents", "LayerToggle_Regions",
                "LayerToggle_Biomes", "LayerToggle_Islands",
                "LayerToggle_FactionControl", "LayerToggle_WarfareZones",
                "LayerToggle_ResourceNodes", "LayerToggle_LogisticsRoutes"
            };

            for (int i = 0; i < names.Length; i++)
            {
                int column = i < 4 ? 0 : 1;
                int row = i % 4;
                float topY = 0.97f - row * 0.24f;
                float bottomY = topY - 0.20f;
                float leftX = column == 0 ? 0.04f : 0.52f;
                float rightX = column == 0 ? 0.48f : 0.96f;

                SetDescendantRect(layers, names[i],
                    new Vector2(leftX, bottomY),
                    new Vector2(rightX, topY));
            }
        }

        Transform info = canvas.Find("HUD-203-05 PlanetObjectInfoPanel");
        if (info != null)
        {
            SetDescendantRect(info, "SelectedObjectName",
                new Vector2(0.05f, 0.79f), new Vector2(0.95f, 0.96f));
            SetDescendantRect(info, "SelectedObjectType",
                new Vector2(0.05f, 0.61f), new Vector2(0.95f, 0.77f));
            SetDescendantRect(info, "SelectedObjectOwnerFaction",
                new Vector2(0.05f, 0.43f), new Vector2(0.95f, 0.59f));
            SetDescendantRect(info, "SelectedObjectStatus",
                new Vector2(0.05f, 0.25f), new Vector2(0.95f, 0.41f));
            SetDescendantRect(info, "SelectedObjectThreatLevel",
                new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.22f));
            SetDescendantRect(info, "EnterObjectButton",
                new Vector2(0.55f, 0.05f), new Vector2(0.95f, 0.22f));
        }

        Transform actions = canvas.Find("HUD-203-06 QuickActionsPanel");
        if (actions != null)
        {
            string[] names =
            {
                "DeployReconButton", "OpenIntelNetworkButton",
                "OpenDoctrineManagerButton", "OpenLogisticsOverviewButton",
                "OpenWarfareProfileButton"
            };

            for (int i = 0; i < names.Length; i++)
            {
                float topY = 0.97f - i * 0.19f;
                float bottomY = topY - 0.16f;

                SetDescendantRect(actions, names[i],
                    new Vector2(0.06f, bottomY),
                    new Vector2(0.94f, topY));
            }
        }

        Transform alerts = canvas.Find("HUD-203-07 AlertsPanel");
        if (alerts != null)
        {
            string[] names =
            {
                "StrategicAlertEntry", "WarfareAlertEntry",
                "FactionAlertEntry", "ResourceAlertEntry"
            };

            for (int i = 0; i < names.Length; i++)
            {
                float topY = 0.97f - i * 0.24f;
                float bottomY = topY - 0.20f;

                SetDescendantRect(alerts, names[i],
                    new Vector2(0.05f, bottomY),
                    new Vector2(0.95f, topY));
            }
        }
    }

    private static void SetPanelRect(
        Transform canvas, string objectName, Vector2 min, Vector2 max)
    {
        Transform item = canvas.Find(objectName);
        if (item == null)
            return;

        RectTransform rect = item as RectTransform;
        if (rect == null)
            return;

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void SetDescendantRect(
        Transform root, string objectName, Vector2 min, Vector2 max)
    {
        RectTransform[] rects =
            root.GetComponentsInChildren<RectTransform>(true);

        foreach (RectTransform rect in rects)
        {
            if (rect.name != objectName &&
                !rect.name.EndsWith(objectName, StringComparison.Ordinal))
                continue;

            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            return;
        }
    }
    private static void UpgradeReadableText(GameObject canvas)
    {
        TMP_Text[] texts = canvas.GetComponentsInChildren<TMP_Text>(true);

        foreach (TMP_Text text in texts)
        {
            if (text.fontSize < 18f)
                text.fontSize = 18f;
            else if (text.fontSize > 34f)
                text.fontSize = 34f;
        }
    }
}





