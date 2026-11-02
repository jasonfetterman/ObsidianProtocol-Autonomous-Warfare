using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class MainMenuBuilder
{
    private const string ScenePath =
        "Assets/Scenes/MainMenu/01_MainMenu.unity";

    private static Font BuiltInFont
    {
        get
        {
            Font font =
                Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            if (font == null)
                font =
                    Resources.GetBuiltinResource<Font>("Arial.ttf");

            return font;
        }
    }

    // ========================================================
    // MENU COMMAND
    // ========================================================

    [MenuItem("OPAW/Build Main Menu")]
    public static void Build()
    {
        BuildMainMenu();
    }

    // ========================================================
    // BUILD
    // ========================================================

    public static void BuildMainMenu()
    {
        Scene scene =
            EditorSceneManager.NewScene(
                NewSceneSetup.EmptyScene,
                NewSceneMode.Single
            );

        // ====================================================
        // EVENT SYSTEM
        // ====================================================

        GameObject eventSystem =
            new GameObject("EventSystem");

        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();

        // ====================================================
        // MAIN CAMERA
        // ====================================================

        GameObject cameraObject =
            new GameObject("Main Camera");

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.clearFlags =
            CameraClearFlags.SolidColor;

        camera.backgroundColor =
            Color.black;

        camera.orthographic = true;

        camera.orthographicSize = 5f;

        cameraObject.tag =
            "MainCamera";

        // ====================================================
        // BACKGROUND
        // ====================================================

        GameObject background =
            new GameObject("MainMenu_Background");

        VideoPlayer videoPlayer =
            background.AddComponent<VideoPlayer>();

        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;

        videoPlayer.renderMode =
            VideoRenderMode.CameraNearPlane;

        videoPlayer.targetCamera =
            camera;

        videoPlayer.aspectRatio =
            VideoAspectRatio.FitOutside;

        videoPlayer.audioOutputMode =
            VideoAudioOutputMode.Direct;

        string[] videoGuids =
            AssetDatabase.FindAssets(
                "MainMenu_Background t:VideoClip"
            );

        if (videoGuids.Length > 0)
        {
            string videoPath =
                AssetDatabase.GUIDToAssetPath(
                    videoGuids[0]
                );

            VideoClip clip =
                AssetDatabase.LoadAssetAtPath<VideoClip>(
                    videoPath
                );

            videoPlayer.clip = clip;
        }

        // ====================================================
        // CANVAS
        // ====================================================

        GameObject canvas =
            CreateCanvas("Canvas");

        CanvasScaler scaler =
            canvas.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920f, 1080f);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight =
            0.5f;

        // ====================================================
        // MAIN MENU HUD
        // ====================================================

        GameObject hud =
            CreateUIObject(
                "MainMenu_HUD",
                canvas
            );

        Stretch(hud);

        // ====================================================
        // LOGO
        // ====================================================

        GameObject logo =
            CreateText(
                "Logo",
                hud,
                "OBSIDIAN PROTOCOL",
                52,
                TextAnchor.MiddleCenter
            );

        SetAnchors(
            logo,
            new Vector2(0.05f, 0.82f),
            new Vector2(0.45f, 0.94f)
        );

        logo.GetComponent<Text>().color =
            new Color(
                0.85f,
                0.92f,
                0.95f,
                1f
            );

        // ====================================================
        // PROFILE
        // ====================================================

        GameObject profile =
            CreateText(
                "Profile",
                hud,
                "OPERATOR  //  PROFILE",
                20,
                TextAnchor.MiddleLeft
            );

        SetAnchors(
            profile,
            new Vector2(0.05f, 0.91f),
            new Vector2(0.30f, 0.97f)
        );

        profile.GetComponent<Text>().color =
            new Color(
                0.55f,
                0.65f,
                0.70f,
                1f
            );

        // ====================================================
        // CONNECTION
        // ====================================================

        GameObject connection =
            CreateText(
                "ConnectionStatus",
                hud,
                "● ONLINE",
                18,
                TextAnchor.MiddleRight
            );

        SetAnchors(
            connection,
            new Vector2(0.72f, 0.91f),
            new Vector2(0.95f, 0.97f)
        );

        connection.GetComponent<Text>().color =
            new Color(
                0.40f,
                0.85f,
                0.75f,
                1f
            );

        // ====================================================
        // MAIN BUTTONS
        // ====================================================

        GameObject mainButtons =
            CreateUIObject(
                "MainButtons",
                hud
            );

        SetAnchors(
            mainButtons,
            new Vector2(0.07f, 0.19f),
            new Vector2(0.40f, 0.80f)
        );

        VerticalLayoutGroup layout =
            mainButtons.AddComponent<VerticalLayoutGroup>();

        layout.spacing = 9f;

        layout.childAlignment =
            TextAnchor.MiddleLeft;

        layout.childControlWidth = true;
        layout.childControlHeight = true;

        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        // ====================================================
        // BUTTONS
        // ====================================================

        GameObject continueButton =
            CreateMainButton(
                "Continue_Button",
                "CONTINUE",
                mainButtons
            );

        GameObject campaignButton =
            CreateMainButton(
                "Campaign_Button",
                "CAMPAIGN",
                mainButtons
            );

        GameObject multiplayerButton =
            CreateMainButton(
                "Multiplayer_Button",
                "MULTIPLAYER",
                mainButtons
            );

        GameObject garageButton =
            CreateMainButton(
                "Garage_Button",
                "GARAGE",
                mainButtons
            );

        GameObject storeButton =
            CreateMainButton(
                "Store_Button",
                "STORE",
                mainButtons
            );

        GameObject vrButton =
            CreateMainButton(
                "VR_Operator_Button",
                "VR OPERATOR",
                mainButtons
            );

        GameObject settingsButton =
            CreateMainButton(
                "Settings_Button",
                "SETTINGS",
                mainButtons
            );

        GameObject creditsButton =
            CreateMainButton(
                "Credits_Button",
                "CREDITS",
                mainButtons
            );

        GameObject exitButton =
            CreateMainButton(
                "Exit_Button",
                "EXIT",
                mainButtons
            );

        // ====================================================
        // WINDOWS
        // ====================================================

        GameObject continueWindow =
            CreateWindow(
                "Continue_Window",
                "CONTINUE"
            );

        GameObject campaignWindow =
            CreateWindow(
                "Campaign_Window",
                "CAMPAIGN"
            );

        GameObject multiplayerWindow =
            CreateWindow(
                "Multiplayer_Window",
                "MULTIPLAYER"
            );

        GameObject settingsWindow =
            CreateWindow(
                "Settings_Window",
                "SETTINGS"
            );

        GameObject creditsWindow =
            CreateWindow(
                "Credits_Window",
                "CREDITS"
            );

        // ====================================================
        // POPUPS
        // ====================================================

        GameObject confirmationPopup =
            CreatePopup(
                "Confirmation_Popup",
                "CONFIRMATION",
                "Are you sure?"
            );

        GameObject warningPopup =
            CreatePopup(
                "Warning_Popup",
                "WARNING",
                "Warning"
            );

        GameObject errorPopup =
            CreatePopup(
                "Error_Popup",
                "ERROR",
                "An error has occurred."
            );

        GameObject connectionPopup =
            CreatePopup(
                "Connection_Popup",
                "CONNECTION",
                "Connection status"
            );

        GameObject exitPopup =
            CreatePopup(
                "Exit_Confirmation_Popup",
                "EXIT",
                "Exit Obsidian Protocol?"
            );

        // ====================================================
        // OVERLAYS
        // ====================================================

        GameObject loadingOverlay =
            CreateOverlay(
                "Loading_Overlay",
                "LOADING..."
            );

        GameObject fadeOverlay =
            CreateFadeOverlay(
                "Fade_Overlay"
            );

        // ====================================================
        // MANAGER
        // ====================================================

        GameObject manager =
            new GameObject(
                "MainMenu_Manager"
            );

        MainMenuManager managerScript =
            manager.AddComponent<MainMenuManager>();

        managerScript.continueWindow =
            continueWindow;

        managerScript.campaignWindow =
            campaignWindow;

        managerScript.multiplayerWindow =
            multiplayerWindow;

        managerScript.settingsWindow =
            settingsWindow;

        managerScript.creditsWindow =
            creditsWindow;

        managerScript.confirmationPopup =
            confirmationPopup;

        managerScript.warningPopup =
            warningPopup;

        managerScript.errorPopup =
            errorPopup;

        managerScript.connectionPopup =
            connectionPopup;

        managerScript.exitConfirmationPopup =
            exitPopup;

        managerScript.loadingOverlay =
            loadingOverlay;

        managerScript.fadeOverlay =
            fadeOverlay;

        managerScript.connectionStatus =
            connection.GetComponent<Text>();

        // ====================================================
        // BUTTON EVENTS
        // ====================================================

        AddButtonEvent(
            continueButton,
            managerScript,
            "Continue"
        );

        AddButtonEvent(
            campaignButton,
            managerScript,
            "Campaign"
        );

        AddButtonEvent(
            multiplayerButton,
            managerScript,
            "Multiplayer"
        );

        AddButtonEvent(
            garageButton,
            managerScript,
            "Garage"
        );

        AddButtonEvent(
            storeButton,
            managerScript,
            "Store"
        );

        AddButtonEvent(
            vrButton,
            managerScript,
            "VROperator"
        );

        AddButtonEvent(
            settingsButton,
            managerScript,
            "Settings"
        );

        AddButtonEvent(
            creditsButton,
            managerScript,
            "Credits"
        );

        AddButtonEvent(
            exitButton,
            managerScript,
            "Exit"
        );

        // ====================================================
        // EXIT POPUP BUTTONS
        // ====================================================

        AddPopupButton(
            exitPopup,
            "CONFIRM EXIT",
            managerScript,
            true
        );

        AddPopupButton(
            exitPopup,
            "CANCEL",
            managerScript,
            false
        );

        // ====================================================
        // SAVE
        // ====================================================

        EditorSceneManager.SaveScene(
            scene,
            ScenePath
        );

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "=========================================="
        );

        Debug.Log(
            "OPAW MAIN MENU BUILD COMPLETE"
        );

        Debug.Log(
            "SCENE: " + ScenePath
        );

        Debug.Log(
            "=========================================="
        );
    }

    // ========================================================
    // CREATE CANVAS
    // ========================================================

    private static GameObject CreateCanvas(
        string name
    )
    {
        GameObject obj =
            new GameObject(name);

        obj.AddComponent<Canvas>();

        obj.AddComponent<CanvasScaler>();

        obj.AddComponent<GraphicRaycaster>();

        Canvas canvas =
            obj.GetComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        return obj;
    }

    // ========================================================
    // CREATE UI OBJECT
    // ========================================================

    private static GameObject CreateUIObject(
        string name,
        GameObject parent
    )
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform)
            );

        obj.transform.SetParent(
            parent.transform,
            false
        );

        return obj;
    }

    // ========================================================
    // CREATE TEXT
    // ========================================================

    private static GameObject CreateText(
        string name,
        GameObject parent,
        string text,
        int fontSize,
        TextAnchor alignment
    )
    {
        GameObject obj =
            CreateUIObject(
                name,
                parent
            );

        Text textComponent =
            obj.AddComponent<Text>();

        textComponent.font =
            BuiltInFont;

        textComponent.text =
            text;

        textComponent.fontSize =
            fontSize;

        textComponent.alignment =
            alignment;

        textComponent.horizontalOverflow =
            HorizontalWrapMode.Overflow;

        textComponent.verticalOverflow =
            VerticalWrapMode.Overflow;

        textComponent.color =
            Color.white;

        return obj;
    }

    // ========================================================
    // CREATE MAIN BUTTON
    // ========================================================

    private static GameObject CreateMainButton(
        string name,
        string label,
        GameObject parent
    )
    {
        GameObject button =
            CreateUIObject(
                name,
                parent
            );

        LayoutElement layout =
            button.AddComponent<LayoutElement>();

        layout.preferredHeight =
            58f;

        layout.minHeight =
            58f;

        Image background =
            button.AddComponent<Image>();

        background.color =
            new Color(
                0.015f,
                0.025f,
                0.035f,
                0.40f
            );

        Button unityButton =
            button.AddComponent<Button>();

        unityButton.transition =
            Selectable.Transition.None;

        // ====================================================
        // HOVER EDGE
        // ====================================================

        GameObject hoverEdge =
            CreateUIObject(
                "HoverEdge",
                button
            );

        Image hoverEdgeImage =
            hoverEdge.AddComponent<Image>();

        hoverEdgeImage.color =
            new Color(
                0f,
                0.85f,
                1f,
                0f
            );

        hoverEdgeImage.raycastTarget =
            false;

        Stretch(hoverEdge);

        // ====================================================
        // EDGE PARTS
        // ====================================================

        CreateEdgePart(
            "Top",
            hoverEdge,
            new Vector2(0f, 0.96f),
            new Vector2(1f, 1f)
        );

        CreateEdgePart(
            "Bottom",
            hoverEdge,
            new Vector2(0f, 0f),
            new Vector2(1f, 0.04f)
        );

        CreateEdgePart(
            "Left",
            hoverEdge,
            new Vector2(0f, 0f),
            new Vector2(0.008f, 1f)
        );

        CreateEdgePart(
            "Right",
            hoverEdge,
            new Vector2(0.992f, 0f),
            new Vector2(1f, 1f)
        );

        // ====================================================
        // TEXT
        // ====================================================

        GameObject text =
            CreateText(
                "Text",
                button,
                label,
                22,
                TextAnchor.MiddleLeft
            );

        SetAnchors(
            text,
            new Vector2(0.08f, 0f),
            new Vector2(0.95f, 1f)
        );

        Text buttonText =
            text.GetComponent<Text>();

        buttonText.color =
            new Color(
                0.75f,
                0.80f,
                0.84f,
                1f
            );

        buttonText.raycastTarget =
            false;

        // ====================================================
        // HOVER BEHAVIOR
        // ====================================================

        MainMenuButton behavior =
            button.AddComponent<MainMenuButton>();

        behavior.hoverEdge =
            hoverEdgeImage;

        behavior.buttonBackground =
            background;

        behavior.buttonText =
            buttonText;

        return button;
    }

    // ========================================================
    // EDGE PART
    // ========================================================

    private static void CreateEdgePart(
        string name,
        GameObject parent,
        Vector2 minimum,
        Vector2 maximum
    )
    {
        GameObject part =
            CreateUIObject(
                name,
                parent
            );

        Image image =
            part.AddComponent<Image>();

        image.color =
            new Color(
                0f,
                0.85f,
                1f,
                0f
            );

        image.raycastTarget =
            false;

        RectTransform rect =
            part.GetComponent<RectTransform>();

        rect.anchorMin =
            minimum;

        rect.anchorMax =
            maximum;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;
    }

    // ========================================================
    // WINDOW
    // ========================================================

    private static GameObject CreateWindow(
        string name,
        string title
    )
    {
        GameObject canvas =
            GameObject.Find("Canvas");

        GameObject window =
            CreateUIObject(
                name,
                canvas
            );

        Stretch(window);

        Image background =
            window.AddComponent<Image>();

        background.color =
            new Color(
                0.005f,
                0.012f,
                0.018f,
                0.94f
            );

        GameObject titleObject =
            CreateText(
                "Title",
                window,
                title,
                36,
                TextAnchor.MiddleCenter
            );

        SetAnchors(
            titleObject,
            new Vector2(0.25f, 0.76f),
            new Vector2(0.75f, 0.90f)
        );

        GameObject description =
            CreateText(
                "Description",
                window,
                "OBSIDIAN PROTOCOL",
                18,
                TextAnchor.MiddleCenter
            );

        SetAnchors(
            description,
            new Vector2(0.20f, 0.48f),
            new Vector2(0.80f, 0.62f)
        );

        window.SetActive(false);

        return window;
    }

    // ========================================================
    // POPUP
    // ========================================================

    private static GameObject CreatePopup(
        string name,
        string title,
        string message
    )
    {
        GameObject canvas =
            GameObject.Find("Canvas");

        GameObject popup =
            CreateUIObject(
                name,
                canvas
            );

        Stretch(popup);

        Image dim =
            popup.AddComponent<Image>();

        dim.color =
            new Color(
                0f,
                0f,
                0f,
                0.70f
            );

        GameObject panel =
            CreateUIObject(
                "Panel",
                popup
            );

        SetAnchors(
            panel,
            new Vector2(0.32f, 0.34f),
            new Vector2(0.68f, 0.66f)
        );

        Image panelImage =
            panel.AddComponent<Image>();

        panelImage.color =
            new Color(
                0.015f,
                0.025f,
                0.035f,
                0.98f
            );

        GameObject titleObject =
            CreateText(
                "Title",
                panel,
                title,
                30,
                TextAnchor.MiddleCenter
            );

        SetAnchors(
            titleObject,
            new Vector2(0.10f, 0.70f),
            new Vector2(0.90f, 0.88f)
        );

        GameObject messageObject =
            CreateText(
                "Message",
                panel,
                message,
                18,
                TextAnchor.MiddleCenter
            );

        SetAnchors(
            messageObject,
            new Vector2(0.08f, 0.42f),
            new Vector2(0.92f, 0.64f)
        );

        popup.SetActive(false);

        return popup;
    }

    // ========================================================
    // LOADING OVERLAY
    // ========================================================

    private static GameObject CreateOverlay(
        string name,
        string text
    )
    {
        GameObject canvas =
            GameObject.Find("Canvas");

        GameObject overlay =
            CreateUIObject(
                name,
                canvas
            );

        Stretch(overlay);

        Image image =
            overlay.AddComponent<Image>();

        image.color =
            new Color(
                0f,
                0f,
                0f,
                0.88f
            );

        CreateText(
            "LoadingText",
            overlay,
            text,
            28,
            TextAnchor.MiddleCenter
        );

        overlay.SetActive(false);

        return overlay;
    }

    // ========================================================
    // FADE OVERLAY
    // ========================================================

    private static GameObject CreateFadeOverlay(
        string name
    )
    {
        GameObject canvas =
            GameObject.Find("Canvas");

        GameObject overlay =
            CreateUIObject(
                name,
                canvas
            );

        Stretch(overlay);

        Image image =
            overlay.AddComponent<Image>();

        image.color =
            Color.black;

        image.raycastTarget =
            true;

        overlay.SetActive(false);

        return overlay;
    }

    // ========================================================
    // POPUP BUTTON
    // ========================================================

    private static void AddPopupButton(
        GameObject popup,
        string label,
        MainMenuManager manager,
        bool confirmExit
    )
    {
        GameObject button =
            CreateUIObject(
                label + "_Button",
                popup
            );

        if (confirmExit)
        {
            SetAnchors(
                button,
                new Vector2(0.20f, 0.18f),
                new Vector2(0.45f, 0.32f)
            );
        }
        else
        {
            SetAnchors(
                button,
                new Vector2(0.55f, 0.18f),
                new Vector2(0.80f, 0.32f)
            );
        }

        Image image =
            button.AddComponent<Image>();

        image.color =
            new Color(
                0.02f,
                0.05f,
                0.065f,
                1f
            );

        Button unityButton =
            button.AddComponent<Button>();

        unityButton.transition =
            Selectable.Transition.ColorTint;

        GameObject text =
            CreateText(
                "Text",
                button,
                label,
                16,
                TextAnchor.MiddleCenter
            );

        Stretch(text);

        if (confirmExit)
        {
            unityButton.onClick.AddListener(
                manager.ConfirmExit
            );
        }
        else
        {
            unityButton.onClick.AddListener(
                manager.HideAllPopups
            );
        }
    }

    // ========================================================
    // MAIN BUTTON EVENT
    // ========================================================

    private static void AddButtonEvent(
        GameObject button,
        MainMenuManager manager,
        string method
    )
    {
        Button b =
            button.GetComponent<Button>();

        switch (method)
        {
            case "Continue":
                b.onClick.AddListener(
                    manager.Continue
                );
                break;

            case "Campaign":
                b.onClick.AddListener(
                    manager.Campaign
                );
                break;

            case "Multiplayer":
                b.onClick.AddListener(
                    manager.Multiplayer
                );
                break;

            case "Garage":
                b.onClick.AddListener(
                    manager.Garage
                );
                break;

            case "Store":
                b.onClick.AddListener(
                    manager.Store
                );
                break;

            case "VROperator":
                b.onClick.AddListener(
                    manager.VROperator
                );
                break;

            case "Settings":
                b.onClick.AddListener(
                    manager.Settings
                );
                break;

            case "Credits":
                b.onClick.AddListener(
                    manager.Credits
                );
                break;

            case "Exit":
                b.onClick.AddListener(
                    manager.Exit
                );
                break;
        }
    }

    // ========================================================
    // STRETCH
    // ========================================================

    private static void Stretch(
        GameObject obj
    )
    {
        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;
    }

    // ========================================================
    // ANCHORS
    // ========================================================

    private static void SetAnchors(
        GameObject obj,
        Vector2 minimum,
        Vector2 maximum
    )
    {
        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin =
            minimum;

        rect.anchorMax =
            maximum;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;
    }
}
