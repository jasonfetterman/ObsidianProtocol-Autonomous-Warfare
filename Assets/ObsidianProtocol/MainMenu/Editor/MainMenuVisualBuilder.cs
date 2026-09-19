using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
public static class OPAWMainMenuVisualBuilder
{
    private const string ExactScenePath =
        "Assets/Scenes/SCN-01  MAIN MENU/[HUD] MAIN MENU HUD/SCN-01  MAIN MENU.unity";

    private const string RootName = "MAIN_MENU_WORLD";
    private const string CanvasName = "MAIN_MENU_CANVAS";

    private static Material darkMetal;
    private static Material blackMetal;
    private static Material panelMetal;
    private static Material accentMaterial;
    private static Material glassMaterial;
    private static Material whiteMaterial;
    private static Material floorMaterial;

    [MenuItem("Obsidian Protocol/Main Menu/BUILD COMPLETE MAIN MENU")]
    public static void BuildCompleteMainMenu()
    {
        Debug.Log("[OPAW] ========================================");
        Debug.Log("[OPAW] BUILDING COMPLETE MAIN MENU");
        Debug.Log("[OPAW] ========================================");

        string scenePath = FindMainMenuScene();

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError(
                "[OPAW] MAIN MENU SCENE COULD NOT BE FOUND.\n\n" +
                "Expected:\n" + ExactScenePath);
            return;
        }

        Scene scene;

        try
        {
            scene = EditorSceneManager.OpenScene(
                scenePath,
                OpenSceneMode.Single);
        }
        catch (Exception ex)
        {
            Debug.LogError("[OPAW] Failed to open Main Menu scene:\n" + ex);
            return;
        }

        ClearScene();

        CreateMaterials();
        BuildWorld();
        BuildCamera();
        BuildLighting();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[OPAW] ========================================");
        Debug.Log("[OPAW] MAIN MENU BUILD COMPLETE");
        Debug.Log("[OPAW] Scene: " + scenePath);
        Debug.Log("[OPAW] ========================================");
    }

    private static string FindMainMenuScene()
    {
        SceneAsset exact =
            AssetDatabase.LoadAssetAtPath<SceneAsset>(ExactScenePath);

        if (exact != null)
            return ExactScenePath;

        string[] guids =
            AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            string normalized =
                Normalize(path);

            if (normalized.Contains("SCN01MAINMENU") &&
                normalized.Contains("MAINMENU"))
            {
                return path;
            }
        }

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            string normalized =
                Normalize(path);

            if (normalized.Contains("SCN01") &&
                normalized.Contains("MENU"))
            {
                return path;
            }
        }

        return null;
    }

    private static string Normalize(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        char[] chars = value.ToUpperInvariant().ToCharArray();

        System.Text.StringBuilder result =
            new System.Text.StringBuilder();

        foreach (char c in chars)
        {
            if (char.IsLetterOrDigit(c))
                result.Append(c);
        }

        return result.ToString();
    }

    private static void ClearScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject[] roots =
            scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            UnityEngine.Object.DestroyImmediate(root);
        }
    }

    // =========================================================
    // MATERIALS
    // =========================================================

    private static void CreateMaterials()
    {
        darkMetal = CreateMaterial(
            "MM_DarkMetal",
            new Color(0.025f, 0.035f, 0.042f));

        blackMetal = CreateMaterial(
            "MM_BlackMetal",
            new Color(0.008f, 0.012f, 0.016f));

        panelMetal = CreateMaterial(
            "MM_PanelMetal",
            new Color(0.045f, 0.065f, 0.075f));

        accentMaterial = CreateMaterial(
            "MM_Accent",
            new Color(0.05f, 0.65f, 0.70f));

        glassMaterial = CreateMaterial(
            "MM_Glass",
            new Color(0.025f, 0.12f, 0.14f));

        whiteMaterial = CreateMaterial(
            "MM_White",
            new Color(0.75f, 0.88f, 0.90f));

        floorMaterial = CreateMaterial(
            "MM_Floor",
            new Color(0.018f, 0.024f, 0.028f));
    }

    private static Material CreateMaterial(
        string name,
        Color color)
    {
        Material material =
            new Material(
                Shader.Find("Universal Render Pipeline/Lit"));

        if (material.shader == null)
            material.shader = Shader.Find("Standard");

        material.name = name;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);

        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);

        if (material.HasProperty("_Metallic"))
            material.SetFloat("_Metallic", 0.75f);

        if (material.HasProperty("_Smoothness"))
            material.SetFloat("_Smoothness", 0.65f);

        return material;
    }

    // =========================================================
    // WORLD
    // =========================================================

    private static void BuildWorld()
    {
        GameObject world =
            new GameObject(RootName);

        CreateFloor(world.transform);
        CreateBackWall(world.transform);
        CreateSideWalls(world.transform);
        CreateCommandDesk(world.transform);
        CreateCentralCommandCore(world.transform);
        CreateServerColumns(world.transform);
        CreateCeilingStructure(world.transform);
        CreateLightPanels(world.transform);
        CreateAccentStrips(world.transform);
        CreateDecorativePanels(world.transform);
    }

    private static void CreateFloor(Transform parent)
    {
        CreateCube(
            "COMMAND_FLOOR",
            parent,
            new Vector3(0f, -0.15f, 0f),
            new Vector3(32f, 0.3f, 22f),
            floorMaterial);

        for (int x = -15; x <= 15; x += 3)
        {
            CreateCube(
                "FLOOR_GRID_X",
                parent,
                new Vector3(x, 0.012f, 0f),
                new Vector3(0.025f, 0.02f, 21f),
                accentMaterial);
        }

        for (int z = -10; z <= 10; z += 3)
        {
            CreateCube(
                "FLOOR_GRID_Z",
                parent,
                new Vector3(0f, 0.013f, z),
                new Vector3(31f, 0.02f, 0.025f),
                accentMaterial);
        }
    }

    private static void CreateBackWall(Transform parent)
    {
        CreateCube(
            "BACK_WALL",
            parent,
            new Vector3(0f, 6f, 10f),
            new Vector3(32f, 12f, 0.5f),
            blackMetal);

        CreateCube(
            "BACK_WALL_PANEL",
            parent,
            new Vector3(0f, 6.5f, 9.68f),
            new Vector3(25f, 8f, 0.15f),
            darkMetal);

        CreateCube(
            "BACK_ACCENT",
            parent,
            new Vector3(0f, 9.2f, 9.55f),
            new Vector3(22f, 0.08f, 0.08f),
            accentMaterial);

        CreateCube(
            "BACK_ACCENT_LOW",
            parent,
            new Vector3(0f, 3.2f, 9.55f),
            new Vector3(22f, 0.05f, 0.05f),
            accentMaterial);
    }

    private static void CreateSideWalls(Transform parent)
    {
        CreateCube(
            "LEFT_WALL",
            parent,
            new Vector3(-16f, 6f, 0f),
            new Vector3(0.5f, 12f, 22f),
            blackMetal);

        CreateCube(
            "RIGHT_WALL",
            parent,
            new Vector3(16f, 6f, 0f),
            new Vector3(0.5f, 12f, 22f),
            blackMetal);
    }

    private static void CreateCommandDesk(Transform parent)
    {
        CreateCube(
            "COMMAND_DESK",
            parent,
            new Vector3(0f, 1.2f, 3.2f),
            new Vector3(14f, 0.35f, 2.2f),
            darkMetal);

        CreateCube(
            "DESK_FRONT",
            parent,
            new Vector3(0f, 0.65f, 4.1f),
            new Vector3(13.5f, 1.2f, 0.2f),
            blackMetal);

        for (int x = -6; x <= 6; x += 3)
        {
            CreateCube(
                "DESK_LIGHT",
                parent,
                new Vector3(x, 1.42f, 3.2f),
                new Vector3(1.7f, 0.035f, 0.9f),
                glassMaterial);
        }
    }

    private static void CreateCentralCommandCore(Transform parent)
    {
        GameObject core =
            GameObject.CreatePrimitive(
                PrimitiveType.Cylinder);

        core.name = "CENTRAL_COMMAND_CORE";
        core.transform.SetParent(parent);
        core.transform.position =
            new Vector3(0f, 3.0f, 7.8f);
        core.transform.localScale =
            new Vector3(2.4f, 2.0f, 2.4f);

        core.GetComponent<Renderer>().sharedMaterial =
            glassMaterial;

        CreateCube(
            "CORE_BASE",
            parent,
            new Vector3(0f, 1.4f, 7.8f),
            new Vector3(5f, 0.25f, 5f),
            darkMetal);

        CreateCube(
            "CORE_RING",
            parent,
            new Vector3(0f, 4.7f, 7.8f),
            new Vector3(4.5f, 0.08f, 4.5f),
            accentMaterial);
    }

    private static void CreateServerColumns(Transform parent)
    {
        for (int i = -3; i <= 3; i++)
        {
            float x = i * 4f;

            CreateCube(
                "SERVER_COLUMN",
                parent,
                new Vector3(x, 4f, 9.0f),
                new Vector3(1.4f, 7f, 0.7f),
                darkMetal);

            CreateCube(
                "SERVER_LIGHT",
                parent,
                new Vector3(x, 4f, 8.58f),
                new Vector3(0.15f, 5.5f, 0.04f),
                accentMaterial);
        }
    }

    private static void CreateCeilingStructure(Transform parent)
    {
        for (int x = -12; x <= 12; x += 4)
        {
            CreateCube(
                "CEILING_BEAM",
                parent,
                new Vector3(x, 11f, 0f),
                new Vector3(0.35f, 0.35f, 21f),
                darkMetal);
        }

        CreateCube(
            "CEILING_MAIN",
            parent,
            new Vector3(0f, 11.4f, 0f),
            new Vector3(31f, 0.3f, 21f),
            blackMetal);
    }

    private static void CreateLightPanels(Transform parent)
    {
        for (int x = -12; x <= 12; x += 4)
        {
            CreateCube(
                "CEILING_LIGHT",
                parent,
                new Vector3(x, 11.15f, 1f),
                new Vector3(1.5f, 0.05f, 5f),
                glassMaterial);
        }
    }

    private static void CreateAccentStrips(Transform parent)
    {
        CreateCube(
            "LEFT_ACCENT_STRIP",
            parent,
            new Vector3(-14.5f, 5f, 9.6f),
            new Vector3(0.08f, 9f, 0.08f),
            accentMaterial);

        CreateCube(
            "RIGHT_ACCENT_STRIP",
            parent,
            new Vector3(14.5f, 5f, 9.6f),
            new Vector3(0.08f, 9f, 0.08f),
            accentMaterial);
    }

    private static void CreateDecorativePanels(Transform parent)
    {
        for (int i = -2; i <= 2; i++)
        {
            CreateCube(
                "WALL_PANEL",
                parent,
                new Vector3(i * 5f, 6f, 9.45f),
                new Vector3(3.8f, 4.8f, 0.08f),
                panelMetal);
        }
    }

    private static GameObject CreateCube(
        string name,
        Transform parent,
        Vector3 position,
        Vector3 scale,
        Material material)
    {
        GameObject go =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube);

        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = position;
        go.transform.localScale = scale;

        Renderer renderer =
            go.GetComponent<Renderer>();

        renderer.sharedMaterial = material;

        return go;
    }

    // =========================================================
    // CAMERA
    // =========================================================

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "MAIN_MENU_CAMERA",
                typeof(Camera));

        Camera camera =
            cameraObject.GetComponent<Camera>();

        cameraObject.transform.position =
            new Vector3(0f, 4.5f, -17f);

        cameraObject.transform.rotation =
            Quaternion.Euler(
                7f,
                0f,
                0f);

        camera.fieldOfView = 62f;
        camera.nearClipPlane = 0.05f;
        camera.farClipPlane = 200f;

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(0.004f, 0.008f, 0.012f);

        camera.depth = -10;

      

        cameraObject.tag = "MainCamera";
    }

    // =========================================================
    // LIGHTING
    // =========================================================

    private static void BuildLighting()
    {
        RenderSettings.ambientMode =
            UnityEngine.Rendering.AmbientMode.Flat;

        RenderSettings.ambientLight =
            new Color(0.025f, 0.04f, 0.05f);

        RenderSettings.ambientIntensity = 0.65f;

        GameObject keyObject =
            new GameObject("MAIN_MENU_KEY_LIGHT");

        Light key =
            keyObject.AddComponent<Light>();

        key.type = LightType.Directional;
        key.intensity = 0.75f;
        key.color =
            new Color(0.65f, 0.80f, 0.85f);

        keyObject.transform.rotation =
            Quaternion.Euler(45f, -25f, 0f);

        CreatePointLight(
            "COMMAND_CORE_LIGHT",
            new Vector3(0f, 4f, 7f),
            5f,
            12f,
            new Color(0.05f, 0.75f, 0.80f));

        CreatePointLight(
            "LEFT_COMMAND_LIGHT",
            new Vector3(-10f, 5f, 4f),
            3f,
            9f,
            new Color(0.08f, 0.35f, 0.42f));

        CreatePointLight(
            "RIGHT_COMMAND_LIGHT",
            new Vector3(10f, 5f, 4f),
            3f,
            9f,
            new Color(0.08f, 0.35f, 0.42f));
    }

    private static void CreatePointLight(
        string name,
        Vector3 position,
        float intensity,
        float range,
        Color color)
    {
        GameObject go =
            new GameObject(name);

        go.transform.position = position;

        Light light =
            go.AddComponent<Light>();

        light.type = LightType.Point;
        light.intensity = intensity;
        light.range = range;
        light.color = color;
    }

    // =========================================================
    // EVENT SYSTEM
    // =========================================================

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            new GameObject(
                "EVENT_SYSTEM",
                typeof(EventSystem),
                typeof(InputSystemUIInputModule));
    }

    // =========================================================
    // HUD
    // =========================================================

    private static void BuildHUD()
    {
        GameObject canvas =
            new GameObject(
                CanvasName,
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

        Canvas canvasComponent =
            canvas.GetComponent<Canvas>();

        canvasComponent.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvas.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.matchWidthOrHeight = 0.5f;

        CreateFullScreenPanel(
            canvas.transform,
            "HUD_Vignette",
            new Color(0.005f, 0.01f, 0.014f, 0.32f));

        BuildTopHUD(canvas.transform);
        BuildLeftHUD(canvas.transform);
        BuildCenterMenu(canvas.transform);
        BuildRightHUD(canvas.transform);
        BuildBottomHUD(canvas.transform);
        BuildDecorativeHUD(canvas.transform);
        BuildPopups(canvas.transform);
    }

    private static void BuildTopHUD(Transform parent)
    {
        GameObject panel =
            CreateUIBox(
                parent,
                "TOP_COMMAND_BAR",
                new Vector2(0.02f, 0.89f),
                new Vector2(0.98f, 0.975f),
                new Color(0.008f, 0.018f, 0.024f, 0.94f));

        CreateUIText(
            panel.transform,
            "TITLE",
            "OBSIDIAN PROTOCOL",
            30,
            TextAnchor.MiddleLeft,
            Color.white,
            new Vector2(0.025f, 0.15f),
            new Vector2(0.40f, 0.85f));

        CreateUIText(
            panel.transform,
            "SUBTITLE",
            "AUTONOMOUS WARFARE // COMMAND INTERFACE",
            12,
            TextAnchor.MiddleLeft,
            new Color(0.35f, 0.68f, 0.72f),
            new Vector2(0.025f, 0.02f),
            new Vector2(0.55f, 0.30f));

        CreateUIText(
            panel.transform,
            "STATUS",
            "● COMMAND NETWORK ONLINE",
            13,
            TextAnchor.MiddleRight,
            new Color(0.35f, 0.85f, 0.78f),
            new Vector2(0.60f, 0.25f),
            new Vector2(0.97f, 0.75f));
    }

    private static void BuildLeftHUD(Transform parent)
    {
        GameObject panel =
            CreateUIBox(
                parent,
                "LEFT_SYSTEM_PANEL",
                new Vector2(0.025f, 0.20f),
                new Vector2(0.255f, 0.84f),
                new Color(0.008f, 0.018f, 0.024f, 0.90f));

        CreateUIText(
            panel.transform,
            "HEADER",
            "COMMAND STATUS",
            17,
            TextAnchor.MiddleLeft,
            Color.white,
            new Vector2(0.06f, 0.91f),
            new Vector2(0.94f, 0.98f));

        string[] labels =
        {
            "FLEET",
            "AI CORE",
            "RESEARCH",
            "LOGISTICS",
            "FINANCE",
            "DEPLOYMENT"
        };

        for (int i = 0; i < labels.Length; i++)
        {
            float y =
                0.79f - i * 0.095f;

            CreateStatusBlock(
                panel.transform,
                labels[i],
                i == 5 ? "STANDBY" : "OPERATIONAL",
                y);
        }

        CreateUIText(
            panel.transform,
            "LOWER",
            "NO PRIORITY ALERTS",
            11,
            TextAnchor.MiddleCenter,
            new Color(0.38f, 0.55f, 0.58f),
            new Vector2(0.05f, 0.04f),
            new Vector2(0.95f, 0.10f));
    }

    private static void BuildCenterMenu(Transform parent)
    {
        GameObject panel =
            CreateUIBox(
                parent,
                "CENTER_COMMAND_MENU",
                new Vector2(0.315f, 0.17f),
                new Vector2(0.685f, 0.86f),
                new Color(0.006f, 0.014f, 0.019f, 0.93f));

        CreateUIText(
            panel.transform,
            "HEADER",
            "COMMAND INTERFACE",
            22,
            TextAnchor.MiddleCenter,
            Color.white,
            new Vector2(0.08f, 0.91f),
            new Vector2(0.92f, 0.98f));

        CreateUIText(
            panel.transform,
            "LINE",
            "SELECT DESTINATION",
            10,
            TextAnchor.MiddleCenter,
            new Color(0.35f, 0.65f, 0.68f),
            new Vector2(0.08f, 0.855f),
            new Vector2(0.92f, 0.90f));

        string[] buttons =
        {
            "CONTINUE",
            "CAMPAIGN",
            "MULTIPLAYER",
            "GARAGE",
            "STORE",
            "VR OPERATOR",
            "SETTINGS",
            "CREDITS",
            "EXIT"
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            float top =
                0.82f - i * 0.078f;

            float bottom =
                top - 0.060f;

            CreateUIButton(
                panel.transform,
                buttons[i],
                buttons[i],
                new Vector2(0.10f, bottom),
                new Vector2(0.90f, top));
        }

        CreateUIText(
            panel.transform,
            "FOOTER",
            "COMMAND INTENT // UNLEASH AUTONOMY // WITNESS WAR EVOLVE",
            9,
            TextAnchor.MiddleCenter,
            new Color(0.25f, 0.42f, 0.45f),
            new Vector2(0.04f, 0.02f),
            new Vector2(0.96f, 0.065f));
    }

    private static void BuildRightHUD(Transform parent)
    {
        GameObject panel =
            CreateUIBox(
                parent,
                "RIGHT_COMMANDER_PANEL",
                new Vector2(0.745f, 0.20f),
                new Vector2(0.975f, 0.84f),
                new Color(0.008f, 0.018f, 0.024f, 0.90f));

        CreateUIText(
            panel.transform,
            "HEADER",
            "COMMANDER",
            17,
            TextAnchor.MiddleLeft,
            Color.white,
            new Vector2(0.06f, 0.91f),
            new Vector2(0.94f, 0.98f));

        CreateUIBox(
            panel.transform,
            "AVATAR",
            new Vector2(0.08f, 0.70f),
            new Vector2(0.32f, 0.87f),
            new Color(0.025f, 0.09f, 0.105f, 1f));

        CreateUIText(
            panel.transform,
            "AVATAR_SYMBOL",
            "OP",
            26,
            TextAnchor.MiddleCenter,
            new Color(0.35f, 0.75f, 0.78f),
            new Vector2(0f, 0f),
            new Vector2(1f, 1f));

        CreateUIText(
            panel.transform,
            "NAME",
            "OBSIDIAN COMMANDER",
            15,
            TextAnchor.MiddleLeft,
            Color.white,
            new Vector2(0.38f, 0.79f),
            new Vector2(0.93f, 0.87f));

        CreateUIText(
            panel.transform,
            "LEVEL",
            "LEVEL 01 // COMMAND RATING: UNASSIGNED",
            9,
            TextAnchor.MiddleLeft,
            new Color(0.35f, 0.65f, 0.68f),
            new Vector2(0.38f, 0.72f),
            new Vector2(0.94f, 0.77f));

        string[] status =
        {
            "CONNECTION       ONLINE",
            "NETWORK          STABLE",
            "FLEET            READY",
            "DEPLOYMENT       STANDBY",
            "ALERTS           0 ACTIVE",
            "BUILD            0.1.0"
        };

        for (int i = 0; i < status.Length; i++)
        {
            CreateUIText(
                panel.transform,
                "STATUS_" + i,
                status[i],
                10,
                TextAnchor.MiddleLeft,
                i == 3
                    ? new Color(0.85f, 0.65f, 0.35f)
                    : new Color(0.60f, 0.70f, 0.72f),
                new Vector2(0.08f, 0.61f - i * 0.065f),
                new Vector2(0.92f, 0.65f - i * 0.065f));
        }

        CreateUIText(
            panel.transform,
            "NOTICE",
            "SYSTEM READY\nAWAITING COMMAND",
            12,
            TextAnchor.MiddleCenter,
            new Color(0.35f, 0.65f, 0.68f),
            new Vector2(0.08f, 0.08f),
            new Vector2(0.92f, 0.22f));
    }

    private static void BuildBottomHUD(Transform parent)
    {
        GameObject panel =
            CreateUIBox(
                parent,
                "BOTTOM_SYSTEM_BAR",
                new Vector2(0.02f, 0.025f),
                new Vector2(0.98f, 0.105f),
                new Color(0.006f, 0.014f, 0.019f, 0.95f));

        CreateUIText(
            panel.transform,
            "LEFT",
            "OBSIDIAN PROTOCOL // AUTONOMOUS WARFARE",
            10,
            TextAnchor.MiddleLeft,
            new Color(0.38f, 0.58f, 0.61f),
            new Vector2(0.02f, 0.20f),
            new Vector2(0.45f, 0.80f));

        CreateUIText(
            panel.transform,
            "CENTER",
            "© 2026 OBSIDIAN PROTOCOL",
            9,
            TextAnchor.MiddleCenter,
            new Color(0.28f, 0.38f, 0.40f),
            new Vector2(0.35f, 0.20f),
            new Vector2(0.65f, 0.80f));

        CreateUIText(
            panel.transform,
            "RIGHT",
            "SYSTEM READY  ●",
            10,
            TextAnchor.MiddleRight,
            new Color(0.35f, 0.78f, 0.72f),
            new Vector2(0.70f, 0.20f),
            new Vector2(0.98f, 0.80f));
    }

    private static void BuildDecorativeHUD(Transform parent)
    {
        CreateUIBox(
            parent,
            "TOP_LEFT_BRACKET",
            new Vector2(0.02f, 0.84f),
            new Vector2(0.16f, 0.845f),
            new Color(0.1f, 0.65f, 0.70f, 0.8f));

        CreateUIBox(
            parent,
            "TOP_RIGHT_BRACKET",
            new Vector2(0.84f, 0.84f),
            new Vector2(0.98f, 0.845f),
            new Color(0.1f, 0.65f, 0.70f, 0.8f));

        CreateUIText(
            parent,
            "TOP_LEFT_CODE",
            "SECURE COMMAND NETWORK // NODE 01",
            8,
            TextAnchor.MiddleLeft,
            new Color(0.30f, 0.50f, 0.53f),
            new Vector2(0.025f, 0.855f),
            new Vector2(0.30f, 0.875f));

        CreateUIText(
            parent,
            "TOP_RIGHT_CODE",
            "ENCRYPTED // AUTHORIZED PERSONNEL",
            8,
            TextAnchor.MiddleRight,
            new Color(0.30f, 0.50f, 0.53f),
            new Vector2(0.70f, 0.855f),
            new Vector2(0.975f, 0.875f));
    }

    private static void BuildPopups(Transform parent)
    {
        GameObject popup =
            CreateUIBox(
                parent,
                "EXIT_CONFIRMATION",
                new Vector2(0.34f, 0.34f),
                new Vector2(0.66f, 0.66f),
                new Color(0.006f, 0.014f, 0.019f, 0.98f));

        CreateUIText(
            popup.transform,
            "TITLE",
            "EXIT OBSIDIAN PROTOCOL?",
            22,
            TextAnchor.MiddleCenter,
            Color.white,
            new Vector2(0.08f, 0.70f),
            new Vector2(0.92f, 0.88f));

        CreateUIText(
            popup.transform,
            "MESSAGE",
            "COMMAND SESSION WILL BE TERMINATED.",
            11,
            TextAnchor.MiddleCenter,
            new Color(0.45f, 0.58f, 0.60f),
            new Vector2(0.08f, 0.54f),
            new Vector2(0.92f, 0.66f));

        CreateUIButton(
            popup.transform,
            "CONFIRM",
            "CONFIRM",
            new Vector2(0.10f, 0.18f),
            new Vector2(0.43f, 0.34f));

        CreateUIButton(
            popup.transform,
            "CANCEL",
            "CANCEL",
            new Vector2(0.57f, 0.18f),
            new Vector2(0.90f, 0.34f));

        popup.SetActive(false);
    }

    // =========================================================
    // UI HELPERS
    // =========================================================

    private static GameObject CreateFullScreenPanel(
        Transform parent,
        string name,
        Color color)
    {
        return CreateUIBox(
            parent,
            name,
            Vector2.zero,
            Vector2.one,
            color);
    }

    private static GameObject CreateUIBox(
        Transform parent,
        string name,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Color color)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            go.GetComponent<Image>();

        image.color = color;

        return go;
    }

    private static GameObject CreateUIButton(
        Transform parent,
        string name,
        string label,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        GameObject buttonObject =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        buttonObject.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            buttonObject.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image =
            buttonObject.GetComponent<Image>();

        image.color =
            new Color(
                0.018f,
                0.055f,
                0.065f,
                0.96f);

        Button button =
            buttonObject.GetComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.018f,
                0.055f,
                0.065f,
                0.96f);

        colors.highlightedColor =
            new Color(
                0.05f,
                0.24f,
                0.27f,
                1f);

        colors.pressedColor =
            new Color(
                0.08f,
                0.38f,
                0.40f,
                1f);

        colors.selectedColor =
            colors.highlightedColor;

        colors.disabledColor =
            new Color(
                0.02f,
                0.025f,
                0.028f,
                0.5f);

        button.colors = colors;

        CreateUIText(
            buttonObject.transform,
            "LABEL",
            label,
            15,
            TextAnchor.MiddleCenter,
            Color.white,
            Vector2.zero,
            Vector2.one);

        CreateUIBox(
            buttonObject.transform,
            "LEFT_ACCENT",
            new Vector2(0f, 0f),
            new Vector2(0.012f, 1f),
            new Color(0.05f, 0.65f, 0.70f, 1f));

        return buttonObject;
    }

    private static void CreateStatusBlock(
        Transform parent,
        string label,
        string value,
        float y)
    {
        CreateUIBox(
            parent,
            label + "_BLOCK",
            new Vector2(0.05f, y),
            new Vector2(0.95f, y + 0.07f),
            new Color(0.015f, 0.035f, 0.042f, 0.9f));

        CreateUIText(
            parent,
            label,
            label,
            10,
            TextAnchor.MiddleLeft,
            new Color(0.42f, 0.58f, 0.60f),
            new Vector2(0.08f, y + 0.01f),
            new Vector2(0.50f, y + 0.06f));

        CreateUIText(
            parent,
            label + "_VALUE",
            value,
            9,
            TextAnchor.MiddleRight,
            new Color(0.38f, 0.78f, 0.72f),
            new Vector2(0.50f, y + 0.01f),
            new Vector2(0.92f, y + 0.06f));
    }

    private static GameObject CreateUIText(
        Transform parent,
        string name,
        string text,
        int fontSize,
        TextAnchor alignment,
        Color color,
        Vector2 anchorMin,
        Vector2 anchorMax)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text));

        go.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Text textComponent =
            go.GetComponent<Text>();

        textComponent.text = text;
        textComponent.fontSize = fontSize;
        textComponent.alignment = alignment;
        textComponent.color = color;
        textComponent.horizontalOverflow =
            HorizontalWrapMode.Wrap;
        textComponent.verticalOverflow =
            VerticalWrapMode.Overflow;

        Font font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        if (font != null)
            textComponent.font = font;

        return go;
    }
}