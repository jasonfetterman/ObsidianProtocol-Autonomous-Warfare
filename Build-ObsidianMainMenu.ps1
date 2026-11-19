$ErrorActionPreference = "Stop"

# ============================================================
# OBSIDIAN PROTOCOL
# COMPLETE MAIN MENU BUILDER
# ============================================================

$ProjectRoot = (Get-Location).Path
$Assets = Join-Path $ProjectRoot "Assets"

if (-not (Test-Path $Assets)) {
    Write-Host ""
    Write-Host "ERROR: Assets folder was not found."
    Write-Host "Run this from:"
    Write-Host "C:\ObsidianProtocol-Autonomous-Warfare\OPAW"
    exit 1
}

Write-Host ""
Write-Host "============================================================"
Write-Host " OBSIDIAN PROTOCOL - MAIN MENU BUILD"
Write-Host "============================================================"
Write-Host ""

# ------------------------------------------------------------
# DIRECTORIES
# ------------------------------------------------------------

$MainMenuRoot = Join-Path $Assets "UI\MainMenu"
$ArtDir       = Join-Path $MainMenuRoot "Art"
$SceneDir     = Join-Path $MainMenuRoot "Scenes"
$ScriptDir    = Join-Path $MainMenuRoot "Scripts"
$EditorDir    = Join-Path $Assets "Editor\ObsidianProtocol"

New-Item -ItemType Directory -Force -Path $ArtDir | Out-Null
New-Item -ItemType Directory -Force -Path $SceneDir | Out-Null
New-Item -ItemType Directory -Force -Path $ScriptDir | Out-Null
New-Item -ItemType Directory -Force -Path $EditorDir | Out-Null

# ------------------------------------------------------------
# BACKGROUND IMAGE
# ------------------------------------------------------------

$SourceImage = "C:\Users\Black Steel Innovati\Downloads\Copilot_20260913_180130.png"
$DestinationImage = Join-Path $ArtDir "MainMenu_Background.png"

if (-not (Test-Path $SourceImage)) {
    Write-Host "ERROR: Background image was not found:"
    Write-Host $SourceImage
    exit 1
}

Copy-Item -LiteralPath $SourceImage -Destination $DestinationImage -Force

Write-Host "Background installed:"
Write-Host "  $DestinationImage"
Write-Host ""

# ------------------------------------------------------------
# MAIN MENU CONTROLLER
# ------------------------------------------------------------

$ControllerCode = @"
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObsidianProtocol.UI
{
    public class ObsidianMainMenuController : MonoBehaviour
    {
        public GameObject ExitPopup;

        public void ContinueGame()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] CONTINUE");
        }

        public void Campaign()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] CAMPAIGN");
        }

        public void Multiplayer()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] MULTIPLAYER");
        }

        public void Garage()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] GARAGE");
        }

        public void Store()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] STORE");
        }

        public void VROperator()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] VR OPERATOR");
        }

        public void Settings()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] SETTINGS");
        }

        public void Credits()
        {
            Debug.Log("[OBSIDIAN PROTOCOL] CREDITS");
        }

        public void OpenExit()
        {
            if (ExitPopup != null)
                ExitPopup.SetActive(true);
        }

        public void ConfirmExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public void CancelExit()
        {
            if (ExitPopup != null)
                ExitPopup.SetActive(false);
        }
    }
}
"@

$ControllerPath = Join-Path $ScriptDir "ObsidianMainMenuController.cs"

Set-Content `
    -LiteralPath $ControllerPath `
    -Value $ControllerCode `
    -Encoding UTF8

# ------------------------------------------------------------
# UNITY EDITOR BUILDER
# ------------------------------------------------------------

$BuilderCode = @"
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class ObsidianMainMenuBuilder
{
    private const string ScenePath =
        "Assets/UI/MainMenu/Scenes/SCN-01_MainMenu.unity";

    private const string BackgroundPath =
        "Assets/UI/MainMenu/Art/MainMenu_Background.png";

    [MenuItem("Tools/Obsidian Protocol/Main Menu/BUILD COMPLETE MAIN MENU")]
    public static void BuildCompleteMainMenu()
    {
        Build();
    }

    public static void BuildFromBatchMode()
    {
        Build();
    }

    private static void Build()
    {
        // ----------------------------------------------------
        // BACKUP EXISTING MAIN MENU
        // ----------------------------------------------------

        if (File.Exists(ScenePath))
        {
            string backup =
                ScenePath.Replace(
                    ".unity",
                    "_BACKUP_" +
                    System.DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                    ".unity");

            File.Copy(ScenePath, backup, false);

            Debug.Log(
                "[OBSIDIAN PROTOCOL] Backup created: " +
                backup);
        }

        // ----------------------------------------------------
        // NEW MAIN MENU SCENE
        // ----------------------------------------------------

        var scene =
            EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single);

        // ----------------------------------------------------
        // EVENT SYSTEM
        // ----------------------------------------------------

        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();

        System.Type inputSystem =
            System.Type.GetType(
                "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");

        if (inputSystem != null)
            eventSystem.AddComponent(inputSystem);
        else
            eventSystem.AddComponent<StandaloneInputModule>();

        // ----------------------------------------------------
        // ENVIRONMENT
        // ----------------------------------------------------

        GameObject environment =
            new GameObject("Environment");

        GameObject cameraObject =
            new GameObject("MainCamera");

        cameraObject.transform.SetParent(
            environment.transform);

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            Color.black;

        cameraObject.tag = "MainCamera";

        GameObject lightObject =
            new GameObject("DirectionalLight");

        lightObject.transform.SetParent(
            environment.transform);

        Light light =
            lightObject.AddComponent<Light>();

        light.type =
            LightType.Directional;

        light.intensity =
            0.15f;

        // ----------------------------------------------------
        // CANVAS
        // ----------------------------------------------------

        GameObject canvasObject =
            new GameObject(
                "Canvas_MainMenuHUD",
                typeof(RectTransform));

        Canvas canvas =
            canvasObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920, 1080);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight =
            0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();

        // ----------------------------------------------------
        // BACKGROUND
        // ----------------------------------------------------

        GameObject background =
            CreateUI(
                "BACKGROUND_IMAGE",
                canvasObject.transform);

        Stretch(
            background.GetComponent<RectTransform>());

        RawImage backgroundImage =
            background.AddComponent<RawImage>();

        Texture2D texture =
            AssetDatabase.LoadAssetAtPath<Texture2D>(
                BackgroundPath);

        if (texture != null)
            backgroundImage.texture = texture;

        // Dark cinematic layer
        GameObject dark =
            CreateUI(
                "BACKGROUND_DARKEN",
                canvasObject.transform);

        Stretch(
            dark.GetComponent<RectTransform>());

        Image darkImage =
            dark.AddComponent<Image>();

        darkImage.color =
            new Color(
                0f,
                0f,
                0f,
                0.42f);

        // ----------------------------------------------------
        // BRANDING
        // ----------------------------------------------------

        GameObject branding =
            CreateUI(
                "PANEL_Branding",
                canvasObject.transform);

        RectTransform brandingRect =
            branding.GetComponent<RectTransform>();

        brandingRect.anchorMin =
            new Vector2(0.055f, 0.80f);

        brandingRect.anchorMax =
            new Vector2(0.65f, 0.98f);

        brandingRect.offsetMin =
            Vector2.zero;

        brandingRect.offsetMax =
            Vector2.zero;

        CreateText(
            "TITLE_ObsidianProtocol",
            branding.transform,
            "OBSIDIAN PROTOCOL",
            52,
            TextAnchor.MiddleLeft);

        CreateText(
            "TITLE_Subtitle",
            branding.transform,
            "AUTONOMOUS WARFARE",
            20,
            TextAnchor.LowerLeft);

        // ----------------------------------------------------
        // MENU
        // ----------------------------------------------------

        GameObject menu =
            CreateUI(
                "PANEL_CenterMenu",
                canvasObject.transform);

        RectTransform menuRect =
            menu.GetComponent<RectTransform>();

        menuRect.anchorMin =
            new Vector2(0.055f, 0.12f);

        menuRect.anchorMax =
            new Vector2(0.37f, 0.79f);

        menuRect.offsetMin =
            Vector2.zero;

        menuRect.offsetMax =
            Vector2.zero;

        string[] labels =
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

        string[] objectNames =
        {
            "BUTTON_Continue",
            "BUTTON_Campaign",
            "BUTTON_Multiplayer",
            "BUTTON_Garage",
            "BUTTON_Store",
            "BUTTON_VROperator",
            "BUTTON_Settings",
            "BUTTON_Credits",
            "BUTTON_Exit"
        };

        for (int i = 0; i < labels.Length; i++)
        {
            GameObject button =
                CreateButton(
                    objectNames[i],
                    menu.transform,
                    labels[i]);

            RectTransform r =
                button.GetComponent<RectTransform>();

            r.anchorMin =
                new Vector2(0f, 1f);

            r.anchorMax =
                new Vector2(0f, 1f);

            r.pivot =
                new Vector2(0f, 1f);

            r.anchoredPosition =
                new Vector2(
                    0f,
                    -i * 60f);

            r.sizeDelta =
                new Vector2(
                    540f,
                    50f);
        }

        // ----------------------------------------------------
        // RIGHT SYSTEM PANEL
        // ----------------------------------------------------

        GameObject systemPanel =
            CreateUI(
                "PANEL_RightSystem",
                canvasObject.transform);

        RectTransform systemRect =
            systemPanel.GetComponent<RectTransform>();

        systemRect.anchorMin =
            new Vector2(0.76f, 0.68f);

        systemRect.anchorMax =
            new Vector2(0.96f, 0.94f);

        systemRect.offsetMin =
            Vector2.zero;

        systemRect.offsetMax =
            Vector2.zero;

        Image systemBackground =
            systemPanel.AddComponent<Image>();

        systemBackground.color =
            new Color(
                0.01f,
                0.015f,
                0.02f,
                0.82f);

        CreateText(
            "DISPLAY_PlayerProfile",
            systemPanel.transform,
            "COMMANDER\\nPROFILE: OFFLINE",
            18,
            TextAnchor.UpperLeft);

        CreateText(
            "DISPLAY_ConnectionStatus",
            systemPanel.transform,
            "NETWORK\\nSTATUS: STANDBY",
            16,
            TextAnchor.MiddleLeft);

        CreateText(
            "DISPLAY_VersionBuild",
            systemPanel.transform,
            "BUILD 0.1.0\\nPRE-ALPHA",
            14,
            TextAnchor.LowerLeft);

        // ----------------------------------------------------
        // BOTTOM BAR
        // ----------------------------------------------------

        GameObject bottom =
            CreateUI(
                "PANEL_BottomBar",
                canvasObject.transform);

        RectTransform bottomRect =
            bottom.GetComponent<RectTransform>();

        bottomRect.anchorMin =
            new Vector2(0f, 0f);

        bottomRect.anchorMax =
            new Vector2(1f, 0.07f);

        bottomRect.offsetMin =
            Vector2.zero;

        bottomRect.offsetMax =
            Vector2.zero;

        Image bottomImage =
            bottom.AddComponent<Image>();

        bottomImage.color =
            new Color(
                0f,
                0f,
                0f,
                0.78f);

        CreateText(
            "DISPLAY_StudioTagline",
            bottom.transform,
            "COMMAND INTENT. UNLEASH AUTONOMY. WITNESS WAR EVOLVE.",
            13,
            TextAnchor.MiddleLeft);

        CreateText(
            "DISPLAY_CopyrightLegal",
            bottom.transform,
            "OBSIDIAN PROTOCOL  |  AUTONOMOUS WARFARE",
            12,
            TextAnchor.MiddleRight);

        // ----------------------------------------------------
        // EXIT POPUP
        // ----------------------------------------------------

        GameObject exitPopup =
            CreatePopup(
                "POPUP_ExitConfirmation",
                "EXIT OPERATION",
                "ARE YOU SURE YOU WANT TO EXIT?");

        exitPopup.SetActive(false);

        // ----------------------------------------------------
        // CONTROLLER
        // ----------------------------------------------------

        GameObject controllerObject =
            new GameObject(
                "MainMenuController");

        var controller =
            controllerObject.AddComponent<
                ObsidianProtocol.UI.ObsidianMainMenuController>();

        controller.ExitPopup =
            exitPopup;

        // ----------------------------------------------------
        // BUTTON CONNECTIONS
        // ----------------------------------------------------

        Connect(
            "BUTTON_Continue",
            controller,
            "ContinueGame");

        Connect(
            "BUTTON_Campaign",
            controller,
            "Campaign");

        Connect(
            "BUTTON_Multiplayer",
            controller,
            "Multiplayer");

        Connect(
            "BUTTON_Garage",
            controller,
            "Garage");

        Connect(
            "BUTTON_Store",
            controller,
            "Store");

        Connect(
            "BUTTON_VROperator",
            controller,
            "VROperator");

        Connect(
            "BUTTON_Settings",
            controller,
            "Settings");

        Connect(
            "BUTTON_Credits",
            controller,
            "Credits");

        Connect(
            "BUTTON_Exit",
            controller,
            "OpenExit");

        ConnectChild(
            exitPopup,
            "BUTTON_Confirm",
            controller,
            "ConfirmExit");

        ConnectChild(
            exitPopup,
            "BUTTON_Cancel",
            controller,
            "CancelExit");

        // ----------------------------------------------------
        // SAVE
        // ----------------------------------------------------

        EditorSceneManager.SaveScene(
            scene,
            ScenePath);

        AssetDatabase.Refresh();

        Debug.Log(
            "[OBSIDIAN PROTOCOL] MAIN MENU BUILD COMPLETE");
    }

    // ========================================================
    // UI HELPERS
    // ========================================================

    private static GameObject CreateUI(
        string name,
        Transform parent)
    {
        GameObject go =
            new GameObject(
                name,
                typeof(RectTransform));

        if (parent != null)
            go.transform.SetParent(
                parent,
                false);

        return go;
    }

    private static void Stretch(
        RectTransform rect)
    {
        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;
    }

    private static GameObject CreateText(
        string name,
        Transform parent,
        string text,
        int size,
        TextAnchor alignment)
    {
        GameObject go =
            CreateUI(
                name,
                parent);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            new Vector2(
                24f,
                8f);

        rect.offsetMax =
            new Vector2(
                -24f,
                -8f);

        Text label =
            go.AddComponent<Text>();

        label.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        label.text =
            text;

        label.fontSize =
            size;

        label.alignment =
            alignment;

        label.color =
            Color.white;

        return go;
    }

    private static GameObject CreateButton(
        string name,
        Transform parent,
        string label)
    {
        GameObject go =
            CreateUI(
                name,
                parent);

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(
                0.015f,
                0.02f,
                0.025f,
                0.86f);

        Button button =
            go.AddComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.02f,
                0.025f,
                0.03f,
                0.86f);

        colors.highlightedColor =
            new Color(
                0.95f,
                0.42f,
                0.08f,
                0.95f);

        colors.pressedColor =
            new Color(
                1f,
                0.65f,
                0.18f,
                1f);

        colors.selectedColor =
            colors.highlightedColor;

        colors.fadeDuration =
            0.08f;

        button.colors =
            colors;

        GameObject text =
            CreateText(
                "LABEL",
                go.transform,
                label,
                21,
                TextAnchor.MiddleLeft);

        RectTransform textRect =
            text.GetComponent<RectTransform>();

        textRect.offsetMin =
            new Vector2(
                28f,
                0f);

        textRect.offsetMax =
            new Vector2(
                -10f,
                0f);

        return go;
    }

    private static GameObject CreatePopup(
        string name,
        string title,
        string message)
    {
        GameObject popup =
            CreateUI(
                name,
                GameObject.Find(
                    "Canvas_MainMenuHUD").transform);

        Stretch(
            popup.GetComponent<RectTransform>());

        Image dim =
            popup.AddComponent<Image>();

        dim.color =
            new Color(
                0f,
                0f,
                0f,
                0.78f);

        GameObject box =
            CreateUI(
                "PANEL",
                popup.transform);

        RectTransform boxRect =
            box.GetComponent<RectTransform>();

        boxRect.anchorMin =
            new Vector2(
                0.32f,
                0.32f);

        boxRect.anchorMax =
            new Vector2(
                0.68f,
                0.68f);

        boxRect.offsetMin =
            Vector2.zero;

        boxRect.offsetMax =
            Vector2.zero;

        Image boxImage =
            box.AddComponent<Image>();

        boxImage.color =
            new Color(
                0.015f,
                0.02f,
                0.025f,
                0.98f);

        CreateText(
            "TITLE",
            box.transform,
            title,
            28,
            TextAnchor.UpperCenter);

        CreateText(
            "MESSAGE",
            box.transform,
            message,
            17,
            TextAnchor.MiddleCenter);

        GameObject cancel =
            CreateButton(
                "BUTTON_Cancel",
                box.transform,
                "CANCEL");

        RectTransform cancelRect =
            cancel.GetComponent<RectTransform>();

        cancelRect.anchorMin =
            new Vector2(
                0.25f,
                0.15f);

        cancelRect.anchorMax =
            new Vector2(
                0.48f,
                0.27f);

        cancelRect.offsetMin =
            Vector2.zero;

        cancelRect.offsetMax =
            Vector2.zero;

        GameObject confirm =
            CreateButton(
                "BUTTON_Confirm",
                box.transform,
                "CONFIRM");

        RectTransform confirmRect =
            confirm.GetComponent<RectTransform>();

        confirmRect.anchorMin =
            new Vector2(
                0.52f,
                0.15f);

        confirmRect.anchorMax =
            new Vector2(
                0.75f,
                0.27f);

        confirmRect.offsetMin =
            Vector2.zero;

        confirmRect.offsetMax =
            Vector2.zero;

        return popup;
    }

    private static void Connect(
        string objectName,
        MonoBehaviour controller,
        string method)
    {
        GameObject go =
            GameObject.Find(objectName);

        if (go == null)
            return;

        Button button =
            go.GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.AddListener(
            () =>
            {
                controller
                    .SendMessage(
                        method,
                        SendMessageOptions.DontRequireReceiver);
            });
    }

    private static void ConnectChild(
        GameObject parent,
        string childName,
        MonoBehaviour controller,
        string method)
    {
        Transform child =
            parent.transform.Find(
                "PANEL/" + childName);

        if (child == null)
            return;

        Button button =
            child.GetComponent<Button>();

        if (button == null)
            return;

        button.onClick.AddListener(
            () =>
            {
                controller
                    .SendMessage(
                        method,
                        SendMessageOptions.DontRequireReceiver);
            });
    }
}
"@

$BuilderPath = Join-Path $EditorDir "ObsidianMainMenuBuilder.cs"

Set-Content `
    -LiteralPath $BuilderPath `
    -Value $BuilderCode `
    -Encoding UTF8

Write-Host "Unity scripts installed."
Write-Host ""

# ------------------------------------------------------------
# UNITY DETECTION
# ------------------------------------------------------------

$Unity = $null

$UnityPatterns = @(
    "$env:ProgramFiles\Unity\Hub\Editor\*\Editor\Unity.exe",
    "$env:ProgramFiles(x86)\Unity\Hub\Editor\*\Editor\Unity.exe"
)

foreach ($Pattern in $UnityPatterns) {

    $Found = Get-ChildItem `
        -Path $Pattern `
        -ErrorAction SilentlyContinue |
        Sort-Object FullName -Descending |
        Select-Object -First 1

    if ($Found) {
        $Unity = $Found.FullName
        break
    }
}

# ------------------------------------------------------------
# UNITY ALREADY OPEN?
# ------------------------------------------------------------

$UnityProcess =
    Get-Process `
        -Name Unity `
        -ErrorAction SilentlyContinue

if ($UnityProcess) {

    Write-Host ""
    Write-Host "============================================================"
    Write-Host " UNITY IS ALREADY OPEN"
    Write-Host "============================================================"
    Write-Host ""
    Write-Host "The scripts have been created successfully."
    Write-Host ""
    Write-Host "Return to Unity and wait for compilation."
    Write-Host ""
    Write-Host "Then click:"
    Write-Host ""
    Write-Host "Tools"
    Write-Host "  > Obsidian Protocol"
    Write-Host "    > Main Menu"
    Write-Host "      > BUILD COMPLETE MAIN MENU"
    Write-Host ""
    Write-Host "Do NOT run the old builder."
    Write-Host ""

    exit 0
}

if (-not $Unity) {

    Write-Host ""
    Write-Host "Unity Editor was not found automatically."
    Write-Host ""
    Write-Host "The scripts were created successfully."
    Write-Host ""
    Write-Host "Open Unity and use:"
    Write-Host ""
    Write-Host "Tools > Obsidian Protocol > Main Menu > BUILD COMPLETE MAIN MENU"
    Write-Host ""

    exit 0
}

# ------------------------------------------------------------
# BUILD AUTOMATICALLY
# ------------------------------------------------------------

$LogFile =
    Join-Path `
        $ProjectRoot `
        "ObsidianMainMenuBuild.log"

Write-Host ""
Write-Host "Unity found:"
Write-Host $Unity
Write-Host ""
Write-Host "Building Main Menu..."
Write-Host ""

& $Unity `
    -batchmode `
    -quit `
    -projectPath $ProjectRoot `
    -executeMethod ObsidianMainMenuBuilder.BuildFromBatchMode `
    -logFile $LogFile

if ($LASTEXITCODE -ne 0) {

    Write-Host ""
    Write-Host "============================================================"
    Write-Host " UNITY BUILD FAILED"
    Write-Host "============================================================"
    Write-Host ""
    Write-Host "Check:"
    Write-Host $LogFile
    Write-Host ""

    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "============================================================"
Write-Host " OBSIDIAN PROTOCOL MAIN MENU COMPLETE"
Write-Host "============================================================"
Write-Host ""
Write-Host "Scene:"
Write-Host "Assets\UI\MainMenu\Scenes\SCN-01_MainMenu.unity"
Write-Host ""
Write-Host "Background:"
Write-Host "Assets\UI\MainMenu\Art\MainMenu_Background.png"
Write-Host ""
Write-Host "Open Unity and press PLAY."
Write-Host ""