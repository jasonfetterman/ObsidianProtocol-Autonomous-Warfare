using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class AfterActionReportVisualBuilder
{
    private const string MENU =
        "Obsidian Protocol/Build/AFTER ACTION REPORT - FULL VISUAL";

    [MenuItem(MENU)]
    public static void Build()
    {
        string scenePath = FindExistingScene();

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError(
                "AFTER ACTION REPORT SCENE NOT FOUND.\n\n" +
                "Unity could not find an existing scene named:\n" +
                "After_Action_HUD.unity\n\n" +
                "NO DUPLICATE SCENE WAS CREATED."
            );

            return;
        }

        Debug.Log(
            "AFTER ACTION REPORT SCENE FOUND:\n" +
            scenePath
        );

        Scene scene =
            EditorSceneManager.OpenScene(
                scenePath,
                OpenSceneMode.Single
            );

        if (!scene.IsValid())
        {
            Debug.LogError(
                "AFTER ACTION REPORT SCENE COULD NOT BE OPENED:\n" +
                scenePath
            );

            return;
        }

        ClearScene();

        BuildFacility();
        BuildLighting();
        BuildCamera();
        BuildEventSystem();
        BuildHUD();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "==================================================\n" +
            "AFTER ACTION REPORT BUILD COMPLETE\n" +
            "==================================================\n" +
            "Existing scene updated:\n" +
            scenePath +
            "\n\n" +
            "NO DUPLICATE SCENE CREATED."
        );
    }

    // ============================================================
    // FIND EXISTING SCENE
    // ============================================================

    private static string FindExistingScene()
    {
        string[] guids =
            AssetDatabase.FindAssets(
                "After_Action_HUD t:Scene"
            );

        List<string> matches =
            new List<string>();

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            if (!path.EndsWith(
                    "/After_Action_HUD.unity",
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            matches.Add(path);
        }

        if (matches.Count == 0)
        {
            Debug.LogError(
                "UNITY COULD NOT FIND After_Action_HUD.unity.\n\n" +
                "Searching Assets/Scenes for the existing scene."
            );

            return FindSceneByFileSystem();
        }

        if (matches.Count == 1)
        {
            return matches[0];
        }

        foreach (string path in matches)
        {
            if (
                path.IndexOf(
                    "AFTER ACTION REPORT",
                    StringComparison.OrdinalIgnoreCase
                ) >= 0
                &&
                path.IndexOf(
                    "[HUD] AFTER ACTION REPORT HUD",
                    StringComparison.OrdinalIgnoreCase
                ) >= 0
            )
            {
                return path;
            }
        }

        Debug.LogError(
            "MULTIPLE After_Action_HUD.unity SCENES FOUND.\n\n" +
            "Builder refuses to guess.\n" +
            "NO DUPLICATE SCENE WILL BE CREATED.\n\n" +
            string.Join("\n", matches)
        );

        return null;
    }

    private static string FindSceneByFileSystem()
    {
        string scenesRoot =
            System.IO.Path.Combine(
                Application.dataPath,
                "Scenes"
            );

        if (!System.IO.Directory.Exists(scenesRoot))
        {
            return null;
        }

        string[] files =
            System.IO.Directory.GetFiles(
                scenesRoot,
                "After_Action_HUD.unity",
                System.IO.SearchOption.AllDirectories
            );

        List<string> valid =
            new List<string>();

        foreach (string file in files)
        {
            string normalized =
                file.Replace("\\", "/");

            int assetsIndex =
                normalized.IndexOf(
                    "/Assets/",
                    StringComparison.OrdinalIgnoreCase
                );

            if (assetsIndex < 0)
            {
                continue;
            }

            string assetPath =
                normalized.Substring(
                    assetsIndex + 1
                );

            if (
                assetPath.IndexOf(
                    "AFTER ACTION REPORT",
                    StringComparison.OrdinalIgnoreCase
                ) >= 0
            )
            {
                valid.Add(assetPath);
            }
        }

        if (valid.Count == 1)
        {
            return valid[0];
        }

        if (valid.Count > 1)
        {
            Debug.LogError(
                "MULTIPLE AFTER ACTION REPORT SCENES FOUND:\n\n" +
                string.Join("\n", valid) +
                "\n\nNO DUPLICATE SCENE CREATED."
            );

            return null;
        }

        return null;
    }

    // ============================================================
    // CLEAR SCENE
    // ============================================================

    private static void ClearScene()
    {
        GameObject[] roots =
            SceneManager
                .GetActiveScene()
                .GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            UnityEngine.Object.DestroyImmediate(root);
        }
    }

    // ============================================================
    // FACILITY
    // ============================================================

    private static void BuildFacility()
    {
        GameObject root =
            new GameObject(
                "AFTER ACTION REPORT FACILITY"
            );

        CreateCube(
            "Floor",
            new Vector3(0f, -0.5f, 8f),
            new Vector3(76f, 1f, 60f),
            new Color(0.025f, 0.035f, 0.045f)
        );

        CreateCube(
            "Back Wall",
            new Vector3(0f, 12f, 38f),
            new Vector3(76f, 25f, 1f),
            new Color(0.035f, 0.045f, 0.055f)
        );

        CreateCube(
            "Left Wall",
            new Vector3(-38f, 12f, 8f),
            new Vector3(1f, 25f, 60f),
            new Color(0.03f, 0.04f, 0.05f)
        );

        CreateCube(
            "Right Wall",
            new Vector3(38f, 12f, 8f),
            new Vector3(1f, 25f, 60f),
            new Color(0.03f, 0.04f, 0.05f)
        );

        CreateCube(
            "Ceiling",
            new Vector3(0f, 25f, 8f),
            new Vector3(76f, 1f, 60f),
            new Color(0.018f, 0.025f, 0.032f)
        );

        CreateCube(
            "Central Operations Platform",
            new Vector3(0f, 0.15f, 8f),
            new Vector3(44f, 0.3f, 30f),
            new Color(0.045f, 0.055f, 0.065f)
        );

        CreateStation(
            "MISSION RESULT",
            new Vector3(0f, 0f, 30f),
            new Vector3(28f, 7f, 3f),
            "VICTORY\nMISSION SCORE 87%\nOBJECTIVES 8 / 9"
        );

        CreateStation(
            "OBJECTIVE RESULTS",
            new Vector3(-23f, 0f, 17f),
            new Vector3(20f, 6f, 3f),
            "PRIMARY OBJECTIVE COMPLETE\nSECONDARY OBJECTIVES 7 / 8\nBONUS OBJECTIVES 3"
        );

        CreateStation(
            "UNIT LOSSES",
            new Vector3(0f, 0f, 17f),
            new Vector3(20f, 6f, 3f),
            "DESTROYED 02\nMISSING 00\nCONFIRMED LOSSES"
        );

        CreateStation(
            "UNIT DAMAGE",
            new Vector3(23f, 0f, 17f),
            new Vector3(20f, 6f, 3f),
            "DAMAGED 05\nCRITICAL 01\nFIELD REPAIR REQUIRED"
        );

        CreateStation(
            "RECOVERY",
            new Vector3(-23f, 0f, 7f),
            new Vector3(20f, 6f, 3f),
            "RECOVERABLE 03\nRECOVERY READY\nRETURN TO GARAGE"
        );

        CreateStation(
            "INTELLIGENCE",
            new Vector3(0f, 0f, 7f),
            new Vector3(20f, 6f, 3f),
            "CONTACTS IDENTIFIED 42\nHOSTILE STRUCTURES 11\nINTEL CONFIDENCE 91%"
        );

        CreateStation(
            "RESOURCE ANALYSIS",
            new Vector3(23f, 0f, 7f),
            new Vector3(20f, 6f, 3f),
            "FUEL -1,840\nENERGY -620\nALLOY -410\nELECTRONICS -185"
        );

        CreateStation(
            "COMMAND REVIEW",
            new Vector3(-23f, 0f, -3f),
            new Vector3(20f, 6f, 3f),
            "COMMANDS ISSUED 128\nSUCCESS RATE 94%\nTACTICAL EFFICIENCY 88%"
        );

        CreateStation(
            "AI REVIEW",
            new Vector3(0f, 0f, -3f),
            new Vector3(20f, 6f, 3f),
            "AUTONOMOUS DECISIONS 76\nOVERRIDES 04\nAI EFFECTIVENESS 93%"
        );

        CreateStation(
            "BATTLE REPLAY",
            new Vector3(23f, 0f, -3f),
            new Vector3(20f, 6f, 3f),
            "REPLAY AVAILABLE\nTIMELINE READY\nTACTICAL REVIEW"
        );

        CreateGate(
            "RETURN TO GARAGE",
            new Vector3(-15f, 0f, 31f)
        );

        CreateGate(
            "NEXT OPERATION",
            new Vector3(15f, 0f, 31f)
        );

        CreateText(
            "FACILITY TITLE",
            "AFTER ACTION REPORT",
            new Vector3(0f, 8f, 35f),
            2.2f,
            Color.white
        );

        CreateText(
            "FACILITY SUBTITLE",
            "MISSION DEBRIEF / RECOVERY / COMMAND ANALYSIS",
            new Vector3(0f, 5.8f, 35f),
            0.8f,
            new Color(0.55f, 0.75f, 0.9f)
        );
    }

    private static void CreateStation(
        string title,
        Vector3 position,
        Vector3 size,
        string information)
    {
        GameObject station =
            new GameObject(title);

        CreateCube(
            title + " BODY",
            position + new Vector3(0f, 3f, 0f),
            size,
            new Color(0.055f, 0.07f, 0.085f)
        );

        CreateText(
            title + " TITLE",
            title,
            position + new Vector3(0f, 6.7f, 0f),
            0.9f,
            Color.white
        );

        CreateText(
            title + " DATA",
            information,
            position + new Vector3(0f, 3.5f, -1.6f),
            0.42f,
            new Color(0.55f, 0.8f, 0.95f)
        );
    }

    private static void CreateGate(
        string title,
        Vector3 position)
    {
        CreateCube(
            title + " GATE",
            position + new Vector3(0f, 5f, 0f),
            new Vector3(13f, 10f, 1f),
            new Color(0.04f, 0.06f, 0.075f)
        );

        CreateText(
            title + " LABEL",
            title,
            position + new Vector3(0f, 7f, -0.7f),
            0.8f,
            Color.white
        );
    }

    // ============================================================
    // LIGHTING
    // ============================================================

    private static void BuildLighting()
    {
        GameObject lightObject =
            new GameObject(
                "AFTER ACTION KEY LIGHT"
            );

        Light light =
            lightObject.AddComponent<Light>();

        light.type =
            LightType.Directional;

        light.intensity =
            1.2f;

        lightObject.transform.rotation =
            Quaternion.Euler(
                50f,
                -30f,
                0f
            );

        CreatePointLight(
            "LIGHT A",
            new Vector3(-22f, 18f, 15f)
        );

        CreatePointLight(
            "LIGHT B",
            new Vector3(22f, 18f, 15f)
        );

        CreatePointLight(
            "LIGHT C",
            new Vector3(0f, 18f, -8f)
        );
    }

    private static GameObject CreatePointLight(
        string name,
        Vector3 position)
    {
        GameObject go =
            new GameObject(name);

        Light light =
            go.AddComponent<Light>();

        light.type =
            LightType.Point;

        light.range =
            30f;

        light.intensity =
            4f;

        go.transform.position =
            position;

        return go;
    }

    // ============================================================
    // CAMERA
    // ============================================================

    private static void BuildCamera()
    {
        GameObject cameraObject =
            new GameObject(
                "AFTER ACTION REPORT CAMERA",
                typeof(Camera),
                typeof(AudioListener)
            );

        Camera camera =
            cameraObject.GetComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            new Color(
                0.012f,
                0.018f,
                0.025f,
                1f
            );

        camera.fieldOfView =
            68f;

        camera.nearClipPlane =
            0.01f;

        camera.farClipPlane =
            1000f;

        camera.depth =
            -100f;

        cameraObject.transform.position =
            new Vector3(
                0f,
                25f,
                -68f
            );

        cameraObject.transform.LookAt(
            new Vector3(
                0f,
                7f,
                10f
            )
        );

        camera.tag =
            "MainCamera";
    }

    // ============================================================
    // EVENT SYSTEM
    // ============================================================

    private static void BuildEventSystem()
    {
        GameObject eventSystem =
            new GameObject(
                "EventSystem",
                typeof(
                    UnityEngine.EventSystems.EventSystem
                ),
                typeof(
                    UnityEngine.InputSystem.UI
                        .InputSystemUIInputModule
                )
            );
    }

    // ============================================================
    // HUD
    // ============================================================

    private static void BuildHUD()
    {
        GameObject canvasObject =
            new GameObject(
                "AFTER ACTION REPORT HUD",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster)
            );

        Canvas canvas =
            canvasObject.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(
                1920f,
                1080f
            );

        scaler.matchWidthOrHeight =
            0.5f;

        CreateHUDText(
            canvasObject.transform,
            "HEADER",
            "15. BATTLE RESULTS / AFTER ACTION",
            new Vector2(0.5f, 0.95f),
            34f
        );

        CreateHUDText(
            canvasObject.transform,
            "MISSION",
            "MISSION RESULT     VICTORY     SCORE 87%",
            new Vector2(0.5f, 0.875f),
            26f
        );

        CreateHUDText(
            canvasObject.transform,
            "OBJECTIVES",
            "OBJECTIVES     8 / 9 COMPLETE",
            new Vector2(0.12f, 0.78f),
            22f
        );

        CreateHUDText(
            canvasObject.transform,
            "LOSSES",
            "UNIT LOSSES     DESTROYED 02     DAMAGED 05",
            new Vector2(0.5f, 0.78f),
            22f
        );

        CreateHUDText(
            canvasObject.transform,
            "RECOVERY",
            "RECOVERY     03 UNITS RECOVERABLE",
            new Vector2(0.88f, 0.78f),
            22f
        );

        CreateHUDPanel(
            canvasObject.transform,
            "OBJECTIVE PANEL",
            new Vector2(0.18f, 0.57f),
            "OBJECTIVE RESULTS\n\nPRIMARY     COMPLETE\nSECONDARY   7 / 8\nBONUS       3\n\nMISSION PERFORMANCE 87%"
        );

        CreateHUDPanel(
            canvasObject.transform,
            "INTELLIGENCE PANEL",
            new Vector2(0.50f, 0.57f),
            "INTELLIGENCE\n\nCONTACTS IDENTIFIED  42\nSTRUCTURES             11\nCONFIDENCE              91%\n\nNEW INTEL STORED"
        );

        CreateHUDPanel(
            canvasObject.transform,
            "COMMAND PANEL",
            new Vector2(0.82f, 0.57f),
            "COMMAND REVIEW\n\nORDERS ISSUED       128\nSUCCESS RATE         94%\nTACTICAL EFFICIENCY 88%\n\nAI DECISIONS         76"
        );

        CreateHUDText(
            canvasObject.transform,
            "RESOURCES",
            "RESOURCES CONSUMED     FUEL -1,840     ENERGY -620     ALLOY -410     ELECTRONICS -185",
            new Vector2(0.5f, 0.34f),
            18f
        );

        CreateHUDText(
            canvasObject.transform,
            "BUDGET",
            "BATTLE BUDGET 10,000 DP     AVAILABLE DEPLOYMENT 10,000 DP     COMPETITIVE POWER LIMIT ENFORCED",
            new Vector2(0.5f, 0.29f),
            17f
        );

        CreateHUDButton(
            canvasObject.transform,
            "BATTLE REPLAY",
            "BATTLE REPLAY",
            new Vector2(0.25f, 0.12f)
        );

        CreateHUDButton(
            canvasObject.transform,
            "RETURN GARAGE",
            "RETURN TO GARAGE",
            new Vector2(0.50f, 0.12f)
        );

        CreateHUDButton(
            canvasObject.transform,
            "NEXT OPERATION",
            "NEXT OPERATION",
            new Vector2(0.75f, 0.12f)
        );

        CreateHUDText(
            canvasObject.transform,
            "STATUS",
            "RECOVERY STATUS: READY     REPAIR QUEUE: 05     REPLAY: AVAILABLE",
            new Vector2(0.5f, 0.055f),
            16f
        );
    }

    private static void CreateHUDPanel(
        Transform parent,
        string name,
        Vector2 anchor,
        string text)
    {
        GameObject panel =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image)
            );

        panel.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
            panel.GetComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.sizeDelta =
            new Vector2(
                500f,
                260f
            );

        Image image =
            panel.GetComponent<Image>();

        image.color =
            new Color(
                0.018f,
                0.03f,
                0.045f,
                0.94f
            );

        CreateHUDText(
            panel.transform,
            name + " TEXT",
            text,
            new Vector2(
                0.5f,
                0.5f
            ),
            18f
        );
    }

    private static void CreateHUDButton(
        Transform parent,
        string name,
        string label,
        Vector2 anchor)
    {
        GameObject button =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button)
            );

        button.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
            button.GetComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.sizeDelta =
            new Vector2(
                300f,
                70f
            );

        Image image =
            button.GetComponent<Image>();

        image.color =
            new Color(
                0.04f,
                0.075f,
                0.11f,
                0.96f
            );

        CreateHUDText(
            button.transform,
            name + " LABEL",
            label,
            new Vector2(
                0.5f,
                0.5f
            ),
            20f
        );
    }

    private static void CreateHUDText(
        Transform parent,
        string name,
        string text,
        Vector2 anchor,
        float fontSize)
    {
        GameObject textObject =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Text)
            );

        textObject.transform.SetParent(
            parent,
            false
        );

        RectTransform rect =
            textObject.GetComponent<RectTransform>();

        rect.anchorMin =
            anchor;

        rect.anchorMax =
            anchor;

        rect.sizeDelta =
            new Vector2(
                900f,
                100f
            );

        Text uiText =
            textObject.GetComponent<Text>();

        uiText.text =
            text;

        uiText.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        uiText.fontSize =
            Mathf.RoundToInt(fontSize);

        uiText.alignment =
            TextAnchor.MiddleCenter;

        uiText.color =
            Color.white;

        uiText.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        uiText.verticalOverflow =
            VerticalWrapMode.Overflow;
    }

    // ============================================================
    // WORLD CUBES
    // ============================================================

    private static void CreateCube(
        string name,
        Vector3 position,
        Vector3 scale,
        Color color)
    {
        GameObject cube =
            GameObject.CreatePrimitive(
                PrimitiveType.Cube
            );

        cube.name =
            name;

        cube.transform.position =
            position;

        cube.transform.localScale =
            scale;

        Renderer renderer =
            cube.GetComponent<Renderer>();

        Shader shader =
            Shader.Find(
                "Universal Render Pipeline/Lit"
            );

        if (shader == null)
        {
            shader =
                Shader.Find(
                    "Standard"
                );
        }

        Material material =
            new Material(shader);

        material.color =
            color;

        renderer.sharedMaterial =
            material;
    }

    // ============================================================
    // WORLD TEXT
    // ============================================================

    private static void CreateText(
        string name,
        string text,
        Vector3 position,
        float size,
        Color color)
    {
        GameObject textObject =
            new GameObject(name);

        TextMesh mesh =
            textObject.AddComponent<TextMesh>();

        mesh.text =
            text;

        mesh.fontSize =
            32;

        mesh.characterSize =
            size * 0.08f;

        mesh.anchor =
            TextAnchor.MiddleCenter;

        mesh.alignment =
            TextAlignment.Center;

        mesh.color =
            color;

        textObject.transform.position =
            position;

        textObject.transform.rotation =
            Quaternion.Euler(
                0f,
                180f,
                0f
            );
    }
}
