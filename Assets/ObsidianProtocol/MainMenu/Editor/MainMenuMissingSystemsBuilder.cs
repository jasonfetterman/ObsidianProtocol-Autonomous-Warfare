using ObsidianProtocol.UI.MainMenu;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class MainMenuMissingSystemsBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN‑01  MAIN MENU/[HUD] MAIN MENU HUD/SCN‑01  MAIN MENU.unity";

    private static readonly Color Background =
        new Color(0.008f, 0.012f, 0.018f, 0.99f);

    private static readonly Color Panel =
        new Color(0.012f, 0.024f, 0.036f, 0.97f);

    private static readonly Color Panel2 =
        new Color(0.018f, 0.034f, 0.050f, 0.98f);

    private static readonly Color Accent =
        new Color(0.18f, 0.72f, 0.95f, 1f);

    private static readonly Color White =
        new Color(0.88f, 0.95f, 1f, 1f);

    private static readonly Color Muted =
        new Color(0.45f, 0.58f, 0.66f, 1f);

    private static readonly Color Danger =
        new Color(0.85f, 0.20f, 0.20f, 1f);

    private static Font BuiltinFont;

    [MenuItem(
        "Obsidian Protocol/Build/MAIN MENU - MISSING SYSTEMS")]
    public static void Build()
    {
        VerifyScene();

        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        RemovePreviousBuild();

        Canvas canvas = FindMainMenuCanvas();

        if (canvas == null)
            throw new Exception(
                "MAIN_MENU_CANVAS was not found.");

        Transform root =
            canvas.transform;

        GameObject systemsRoot =
            new GameObject(
                "MAIN MENU SYSTEMS");

        systemsRoot.transform.SetParent(
            root,
            false);

        MainMenuMissingSystems controller =
            systemsRoot.AddComponent<
                MainMenuMissingSystems>();

        GameObject patchNotes =
            CreatePatchNotes(root);

        GameObject networkError =
            CreateNetworkError(root);

        GameObject profileLogin =
            CreateProfileLogin(root);

        GameObject transition =
            CreateTransitionOverlay(root);

        GameObject loading =
            CreateLoadingScreen(root);

        CreateSystemBar(
            root,
            controller);

        CreateTooltipDemo(
            root);

        CreateAudioFeedbackSystem(
            systemsRoot);

        AssignControllerReferences(
            controller,
            patchNotes,
            networkError,
            profileLogin,
            transition,
            loading);

        EditorSceneManager.MarkSceneDirty(scene);

        EditorSceneManager.SaveScene(scene);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "MAIN MENU MISSING SYSTEMS BUILD COMPLETE: " +
            ScenePath);
    }

    private static void VerifyScene()
    {
        string projectRoot =
            Directory.GetParent(
                Application.dataPath).FullName;

        string absolute =
            Path.Combine(
                projectRoot,
                ScenePath.Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()));

        if (!File.Exists(absolute))
        {
            throw new Exception(
                "EXACT MAIN MENU SCENE DOES NOT EXIST: " +
                absolute);
        }
    }

    private static Canvas FindMainMenuCanvas()
    {
        GameObject named =
            GameObject.Find(
                "MAIN_MENU_CANVAS");

        if (named != null)
        {
            Canvas canvas =
                named.GetComponent<Canvas>();

            if (canvas != null)
                return canvas;
        }

        Canvas[] canvases =
            UnityEngine.Object.FindObjectsByType<
                Canvas>(
                    FindObjectsSortMode.None);

        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode ==
                RenderMode.ScreenSpaceOverlay)
            {
                return canvas;
            }
        }

        return null;
    }

    private static void RemovePreviousBuild()
    {
        GameObject old =
            GameObject.Find(
                "MAIN MENU SYSTEMS");

        if (old != null)
            UnityEngine.Object.DestroyImmediate(old);

        string[] generatedNames =
        {
            "POPUP_PATCH_NOTES",
            "POPUP_NETWORK_ERROR",
            "POPUP_PROFILE_LOGIN",
            "EXTRA_TRANSITION_OVERLAY",
            "EXTRA_LOADING_SCREEN",
            "MAIN MENU SYSTEM CONTROLS"
        };

        foreach (string name in generatedNames)
        {
            GameObject obj =
                GameObject.Find(name);

            if (obj != null)
                UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    private static GameObject CreatePatchNotes(
        Transform parent)
    {
        GameObject popup =
            CreatePopup(
                "POPUP_PATCH_NOTES",
                parent,
                "PATCH NOTES",
                "OBSIDIAN PROTOCOL // COMMAND SYSTEM UPDATE");

        CreateText(
            popup.transform,
            "VERSION",
            "VERSION 0.1.0 // AUTONOMOUS WARFARE",
            new Vector2(0, 190),
            new Vector2(680, 42),
            22,
            Accent);

        CreateText(
            popup.transform,
            "NOTES",
            "COMMAND SYSTEMS\n" +
            "• Universal HUD navigation integrated\n" +
            "• Persistent military records online\n" +
            "• Intelligence and logistics systems expanded\n" +
            "• Autonomous command interfaces improved\n" +
            "• Deployment Budget enforcement active\n\n" +
            "COMPETITIVE DEPLOYMENT\n" +
            "10,000 DP BATTLE LIMIT",
            new Vector2(0, -5),
            new Vector2(700, 330),
            20,
            White);

        Button close =
            CreateButton(
                popup.transform,
                "CLOSE",
                new Vector2(0, -210),
                new Vector2(180, 52));

        close.onClick.AddListener(
            () => popup.SetActive(false));

        popup.SetActive(false);

        return popup;
    }

    private static GameObject CreateNetworkError(
        Transform parent)
    {
        GameObject popup =
            CreatePopup(
                "POPUP_NETWORK_ERROR",
                parent,
                "NETWORK ERROR",
                "COMMAND NETWORK CONNECTION FAILURE");

        CreateText(
            popup.transform,
            "ERROR",
            "NETWORK LINK UNAVAILABLE",
            new Vector2(0, 155),
            new Vector2(650, 55),
            28,
            Danger);

        CreateText(
            popup.transform,
            "DETAILS",
            "The command network could not establish a stable link.\n\n" +
            "CHECK CONNECTION\n" +
            "VERIFY AUTHENTICATION\n" +
            "RETRY COMMAND NETWORK",
            new Vector2(0, 35),
            new Vector2(680, 210),
            20,
            White);

        Button retry =
            CreateButton(
                popup.transform,
                "RETRY CONNECTION",
                new Vector2(-110, -175),
                new Vector2(220, 52));

        retry.onClick.AddListener(
            () => popup.SetActive(false));

        Button close =
            CreateButton(
                popup.transform,
                "CLOSE",
                new Vector2(130, -175),
                new Vector2(150, 52));

        close.onClick.AddListener(
            () => popup.SetActive(false));

        popup.SetActive(false);

        return popup;
    }

    private static GameObject CreateProfileLogin(
        Transform parent)
    {
        GameObject popup =
            CreatePopup(
                "POPUP_PROFILE_LOGIN",
                parent,
                "PROFILE LOGIN",
                "COMMANDER AUTHENTICATION");

        CreateText(
            popup.transform,
            "STATUS",
            "COMMANDER IDENTITY REQUIRED",
            new Vector2(0, 150),
            new Vector2(650, 48),
            22,
            Accent);

        CreateInputField(
            popup.transform,
            "COMMANDER ID",
            new Vector2(0, 70));

        CreateInputField(
            popup.transform,
            "ACCESS CODE",
            new Vector2(0, -5));

        CreateText(
            popup.transform,
            "SECURITY",
            "SECURE COMMAND CHANNEL // ENCRYPTION ACTIVE",
            new Vector2(0, -90),
            new Vector2(650, 35),
            15,
            Muted);

        Button login =
            CreateButton(
                popup.transform,
                "AUTHENTICATE",
                new Vector2(-110, -175),
                new Vector2(220, 52));

        login.onClick.AddListener(
            () =>
            {
                popup.SetActive(false);
            });

        Button cancel =
            CreateButton(
                popup.transform,
                "CANCEL",
                new Vector2(130, -175),
                new Vector2(150, 52));

        cancel.onClick.AddListener(
            () => popup.SetActive(false));

        popup.SetActive(false);

        return popup;
    }

    private static GameObject CreateTransitionOverlay(
        Transform parent)
    {
        GameObject overlay =
            new GameObject(
                "EXTRA_TRANSITION_OVERLAY");

        overlay.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            overlay.AddComponent<RectTransform>();

        Stretch(rect);

        Image image =
            overlay.AddComponent<Image>();

        image.color =
            new Color(0.002f, 0.006f, 0.010f, 1f);

        CanvasGroup group =
            overlay.AddComponent<CanvasGroup>();

        group.alpha = 0f;

        overlay.SetActive(false);

        return overlay;
    }

    private static GameObject CreateLoadingScreen(
        Transform parent)
    {
        GameObject screen =
            new GameObject(
                "EXTRA_LOADING_SCREEN");

        screen.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            screen.AddComponent<RectTransform>();

        Stretch(rect);

        Image background =
            screen.AddComponent<Image>();

        background.color =
            Background;

        CanvasGroup group =
            screen.AddComponent<CanvasGroup>();

        group.alpha = 0f;

        CreateText(
            screen.transform,
            "TITLE",
            "OBSIDIAN PROTOCOL",
            new Vector2(0, 130),
            new Vector2(850, 70),
            38,
            White);

        CreateText(
            screen.transform,
            "SUBTITLE",
            "INITIALIZING COMMAND NETWORK",
            new Vector2(0, 72),
            new Vector2(850, 42),
            19,
            Accent);

        GameObject barObject =
            new GameObject(
                "LOADING_PROGRESS");

        barObject.transform.SetParent(
            screen.transform,
            false);

        RectTransform barRect =
            barObject.AddComponent<RectTransform>();

        barRect.sizeDelta =
            new Vector2(720, 28);

        barObject.AddComponent<CanvasRenderer>();

        Image barBackground =
            barObject.AddComponent<Image>();

        barBackground.color =
            Panel2;

        Slider slider =
            barObject.AddComponent<Slider>();

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.value = 0f;

        GameObject fill =
            new GameObject("FILL");

        fill.transform.SetParent(
            barObject.transform,
            false);

        RectTransform fillRect =
            fill.AddComponent<RectTransform>();

        fillRect.anchorMin =
            new Vector2(0f, 0f);

        fillRect.anchorMax =
            new Vector2(0f, 1f);

        fillRect.pivot =
            new Vector2(0f, 0.5f);

        fillRect.sizeDelta =
            new Vector2(720, 0);

        Image fillImage =
            fill.AddComponent<Image>();

        fillImage.color = Accent;

        slider.fillRect = fillRect;

        Text status =
            CreateText(
                screen.transform,
                "STATUS",
                "INITIALIZING COMMAND SYSTEMS...",
                new Vector2(0, -10),
                new Vector2(850, 42),
                18,
                Muted);

        CreateText(
            screen.transform,
            "CLASSIFICATION",
            "CLASSIFIED // COMMAND AUTHORITY",
            new Vector2(0, -110),
            new Vector2(850, 40),
            14,
            Muted);

        screen.SetActive(false);

        return screen;
    }

    private static void CreateSystemBar(
        Transform parent,
        ObsidianProtocol.UI.MainMenu.MainMenuMissingSystems controller)
    {
        GameObject bar =
            new GameObject(
                "MAIN MENU SYSTEM CONTROLS");

        bar.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            bar.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0f);

        rect.anchorMax =
            new Vector2(0.5f, 0f);

        rect.pivot =
            new Vector2(0.5f, 0f);

        rect.anchoredPosition =
            new Vector2(0, 62);

        rect.sizeDelta =
            new Vector2(760, 54);

        CreateText(
            bar.transform,
            "LABEL",
            "COMMAND SYSTEMS",
            new Vector2(-285, 0),
            new Vector2(180, 42),
            15,
            Muted);

        Button patch =
            CreateButton(
                bar.transform,
                "PATCH NOTES",
                new Vector2(-125, 0),
                new Vector2(160, 42));

        patch.onClick.AddListener(
            controller.OpenPatchNotes);

        Button network =
            CreateButton(
                bar.transform,
                "NETWORK",
                new Vector2(45, 0),
                new Vector2(140, 42));

        network.onClick.AddListener(
            controller.OpenNetworkError);

        Button profile =
            CreateButton(
                bar.transform,
                "PROFILE LOGIN",
                new Vector2(205, 0),
                new Vector2(170, 42));

        profile.onClick.AddListener(
            controller.OpenProfileLogin);
    }

    private static void CreateTooltipDemo(
        Transform parent)
    {
        GameObject tooltip =
            new GameObject(
                "TOOLTIP_SYSTEM");

        tooltip.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            tooltip.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(1f, 1f);

        rect.anchorMax =
            new Vector2(1f, 1f);

        rect.pivot =
            new Vector2(1f, 1f);

        rect.anchoredPosition =
            new Vector2(-34, -150);

        rect.sizeDelta =
            new Vector2(300, 82);

        Image image =
            tooltip.AddComponent<Image>();

        image.color =
            new Color(
                0.005f,
                0.015f,
                0.025f,
                0.96f);

        Outline outline =
            tooltip.AddComponent<Outline>();

        outline.effectColor =
            Accent;

        outline.effectDistance =
            new Vector2(1, 1);

        CreateText(
            tooltip.transform,
            "TEXT",
            "SYSTEM STATUS\nCOMMAND NETWORK READY",
            Vector2.zero,
            new Vector2(280, 70),
            15,
            White);

        tooltip.SetActive(false);

        ObsidianProtocol.UI.MainMenu.MainMenuTooltipSystem
            component =
                tooltip.AddComponent<
                    ObsidianProtocol.UI.MainMenu.MainMenuTooltipSystem>();

        component.tooltipObject = tooltip;
    }

    private static void CreateAudioFeedbackSystem(
        GameObject parent)
    {
        GameObject audio =
            new GameObject(
                "AUDIO FEEDBACK SYSTEM");

        audio.transform.SetParent(
            parent.transform,
            false);

        AudioSource source =
            audio.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f;

        ObsidianProtocol.UI.MainMenu.MainMenuAudioFeedbackSystem
            feedback =
                audio.AddComponent<
                    ObsidianProtocol.UI.MainMenu.MainMenuAudioFeedbackSystem>();

        feedback.audioSource = source;
    }

    private static void AssignControllerReferences(
        ObsidianProtocol.UI.MainMenu.MainMenuMissingSystems controller,
        GameObject patchNotes,
        GameObject networkError,
        GameObject profileLogin,
        GameObject transition,
        GameObject loading)
    {
        SerializedObject serialized =
            new SerializedObject(controller);

        serialized.FindProperty(
            "patchNotesPopup").objectReferenceValue =
            patchNotes;

        serialized.FindProperty(
            "networkErrorPopup").objectReferenceValue =
            networkError;

        serialized.FindProperty(
            "profileLoginPopup").objectReferenceValue =
            profileLogin;

        serialized.FindProperty(
            "transitionOverlay").objectReferenceValue =
            transition;

        serialized.FindProperty(
            "loadingScreen").objectReferenceValue =
            loading;

        Transform progress =
            loading.transform.Find(
                "LOADING_PROGRESS");

        if (progress != null)
        {
            serialized.FindProperty(
                "loadingProgress").objectReferenceValue =
                progress.GetComponent<Slider>();
        }

        Transform status =
            loading.transform.Find("STATUS");

        if (status != null)
        {
            serialized.FindProperty(
                "loadingStatus").objectReferenceValue =
                status.GetComponent<Text>();
        }

        serialized.ApplyModifiedPropertiesWithoutUndo();
    }

    private static GameObject CreatePopup(
        string name,
        Transform parent,
        string title,
        string subtitle)
    {
        GameObject popup =
            new GameObject(name);

        popup.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            popup.AddComponent<RectTransform>();

        Stretch(rect);

        Image backdrop =
            popup.AddComponent<Image>();

        backdrop.color =
            new Color(
                0f,
                0f,
                0f,
                0.72f);

        CanvasGroup group =
            popup.AddComponent<CanvasGroup>();

        GameObject window =
            new GameObject(
                "WINDOW");

        window.transform.SetParent(
            popup.transform,
            false);

        RectTransform windowRect =
            window.AddComponent<RectTransform>();

        windowRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        windowRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        windowRect.pivot =
            new Vector2(0.5f, 0.5f);

        windowRect.anchoredPosition =
            Vector2.zero;

        windowRect.sizeDelta =
            new Vector2(820, 600);

        Image windowImage =
            window.AddComponent<Image>();

        windowImage.color = Panel;

        Outline outline =
            window.AddComponent<Outline>();

        outline.effectColor = Accent;
        outline.effectDistance =
            new Vector2(2, 2);

        CreateText(
            window.transform,
            "TITLE",
            title,
            new Vector2(0, 235),
            new Vector2(730, 58),
            32,
            White);

        CreateText(
            window.transform,
            "SUBTITLE",
            subtitle,
            new Vector2(0, 190),
            new Vector2(730, 38),
            15,
            Accent);

        GameObject close =
            CreateButton(
                window.transform,
                "X",
                new Vector2(355, 245),
                new Vector2(52, 42)).gameObject;

        close.GetComponent<Button>()
            .onClick.AddListener(
                () => popup.SetActive(false));

        return popup;
    }

    private static InputField CreateInputField(
        Transform parent,
        string label,
        Vector2 position)
    {
        GameObject root =
            new GameObject(
                "INPUT_" + label);

        root.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            root.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            new Vector2(560, 62);

        Image image =
            root.AddComponent<Image>();

        image.color = Panel2;

        InputField input =
            root.AddComponent<InputField>();

        GameObject textObject =
            new GameObject("TEXT");

        textObject.transform.SetParent(
            root.transform,
            false);

        RectTransform textRect =
            textObject.AddComponent<RectTransform>();

        Stretch(textRect);

        Text text =
            textObject.AddComponent<Text>();

        text.font = GetFont();
        text.fontSize = 19;
        text.color = White;
        text.alignment =
            TextAnchor.MiddleLeft;

        text.supportRichText = false;

        input.textComponent = text;

        Text labelText =
            CreateText(
                parent,
                "LABEL_" + label,
                label,
                new Vector2(
                    position.x - 280,
                    position.y + 39),
                new Vector2(560, 28),
                13,
                Muted);

        return input;
    }

    private static Button CreateButton(
        Transform parent,
        string text,
        Vector2 position,
        Vector2 size)
    {
        GameObject buttonObject =
            new GameObject(
                "BUTTON_" + text);

        buttonObject.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            buttonObject.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Image image =
            buttonObject.AddComponent<Image>();

        image.color =
            new Color(
                0.025f,
                0.080f,
                0.110f,
                0.98f);

        Button button =
            buttonObject.AddComponent<Button>();

        ColorBlock colors =
            button.colors;

        colors.normalColor =
            new Color(
                0.025f,
                0.080f,
                0.110f,
                1f);

        colors.highlightedColor =
            new Color(
                0.08f,
                0.30f,
                0.40f,
                1f);

        colors.pressedColor =
            Accent;

        colors.selectedColor =
            new Color(
                0.08f,
                0.30f,
                0.40f,
                1f);

        colors.disabledColor =
            new Color(
                0.03f,
                0.04f,
                0.05f,
                0.55f);

        button.colors = colors;

        CreateText(
            buttonObject.transform,
            "LABEL",
            text,
            Vector2.zero,
            size - new Vector2(10, 8),
            15,
            White);

        return button;
    }

    private static Text CreateText(
        Transform parent,
        string name,
        string text,
        Vector2 position,
        Vector2 size,
        int fontSize,
        Color color)
    {
        GameObject objectRoot =
            new GameObject(name);

        objectRoot.transform.SetParent(
            parent,
            false);

        RectTransform rect =
            objectRoot.AddComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.5f, 0.5f);

        rect.anchorMax =
            new Vector2(0.5f, 0.5f);

        rect.pivot =
            new Vector2(0.5f, 0.5f);

        rect.anchoredPosition =
            position;

        rect.sizeDelta =
            size;

        Text label =
            objectRoot.AddComponent<Text>();

        label.font = GetFont();
        label.text = text;
        label.fontSize = fontSize;
        label.color = color;
        label.alignment =
            TextAnchor.MiddleCenter;

        label.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        label.verticalOverflow =
            VerticalWrapMode.Overflow;

        return label;
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

    private static Font GetFont()
    {
        if (BuiltinFont == null)
        {
            BuiltinFont =
                Resources.GetBuiltinResource<Font>(
                    "LegacyRuntime.ttf");
        }

        return BuiltinFont;
    }
}




