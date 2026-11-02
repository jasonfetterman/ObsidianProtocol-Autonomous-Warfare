using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public static class BuildCreditsScene
{
    private const string ScenePath =
        "Assets/Scenes/SCN23_CREDITS/Credits.unity";

    [MenuItem("Obsidian Protocol/Build SCN17 Credits")]
    public static void Build()
    {
        CreateScene();
    }

    public static void CreateScene()
    {
        EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        GameObject eventSystem = new GameObject(
            "EVENT SYSTEM",
            typeof(EventSystem),
            typeof(StandaloneInputModule));

        GameObject canvasObject = new GameObject(
            "CREDITS CANVAS",
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster));

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler =
            canvasObject.GetComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        scaler.referenceResolution =
            new Vector2(1920, 1080);

        scaler.screenMatchMode =
            CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        scaler.matchWidthOrHeight = 0.5f;

        // ----------------------------------------------------
        // BACKGROUND
        // ----------------------------------------------------

        GameObject background =
            CreatePanel(
                "CREDITS BACKGROUND",
                canvasObject.transform,
                new Color(0.008f, 0.012f, 0.018f, 1f));

        SetFullScreen(background);

        // ----------------------------------------------------
        // HEADER
        // ----------------------------------------------------

        GameObject header =
            CreateText(
                "OBSIDIAN PROTOCOL",
                canvasObject.transform,
                52,
                FontStyle.Bold);

        SetRect(
            header,
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0, -75),
            new Vector2(1000, 70));

        Text headerText =
            header.GetComponent<Text>();

        headerText.alignment =
            TextAnchor.MiddleCenter;

        GameObject title =
            CreateText(
                "CREDITS",
                canvasObject.transform,
                30,
                FontStyle.Bold);

        SetRect(
            title,
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0, -135),
            new Vector2(800, 50));

        title.GetComponent<Text>().alignment =
            TextAnchor.MiddleCenter;

        GameObject classification =
            CreateText(
                "AUTONOMOUS WARFARE  //  PERSONNEL & SYSTEMS RECORD",
                canvasObject.transform,
                16,
                FontStyle.Normal);

        SetRect(
            classification,
            new Vector2(0.5f, 1f),
            new Vector2(0.5f, 1f),
            new Vector2(0, -180),
            new Vector2(1100, 40));

        classification.GetComponent<Text>().alignment =
            TextAnchor.MiddleCenter;

        // ----------------------------------------------------
        // SCROLL VIEW
        // ----------------------------------------------------

        GameObject scrollObject =
            new GameObject(
                "CreditsScrollArea",
                typeof(RectTransform),
                typeof(Image),
                typeof(ScrollRect));

        scrollObject.transform.SetParent(
            canvasObject.transform,
            false);

        RectTransform scrollRect =
            scrollObject.GetComponent<RectTransform>();

        scrollRect.anchorMin =
            new Vector2(0.5f, 0.5f);

        scrollRect.anchorMax =
            new Vector2(0.5f, 0.5f);

        scrollRect.anchoredPosition =
            new Vector2(0, -15);

        scrollRect.sizeDelta =
            new Vector2(1250, 650);

        Image scrollImage =
            scrollObject.GetComponent<Image>();

        scrollImage.color =
            new Color(0f, 0f, 0f, 0.18f);

        ScrollRect scroll =
            scrollObject.GetComponent<ScrollRect>();

        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType =
            ScrollRect.MovementType.Clamped;

        // ----------------------------------------------------
        // CONTENT
        // ----------------------------------------------------

        GameObject viewport =
            new GameObject(
                "Viewport",
                typeof(RectTransform),
                typeof(RectMask2D));

        viewport.transform.SetParent(
            scrollObject.transform,
            false);

        SetFullScreen(viewport);

        GameObject content =
            new GameObject(
                "CreditsContent",
                typeof(RectTransform));

        content.transform.SetParent(
            viewport.transform,
            false);

        RectTransform contentRect =
            content.GetComponent<RectTransform>();

        contentRect.anchorMin =
            new Vector2(0.5f, 1f);

        contentRect.anchorMax =
            new Vector2(0.5f, 1f);

        contentRect.pivot =
            new Vector2(0.5f, 1f);

        contentRect.anchoredPosition =
            Vector2.zero;

        contentRect.sizeDelta =
            new Vector2(1150, 2600);

        scroll.viewport =
            viewport.GetComponent<RectTransform>();

        scroll.content =
            contentRect;

        // ----------------------------------------------------
        // CREDITS TEXT
        // ----------------------------------------------------

        string credits =
@"OBSIDIAN PROTOCOL

AUTONOMOUS WARFARE


DEVELOPMENT

[YOUR NAME]
Lead Developer


GAME DESIGN

[YOUR NAME]
Game Director
Systems Designer
Gameplay Designer


PROGRAMMING

[YOUR NAME]
Lead Programmer

Core Architecture
Game Systems
Persistence Systems
Scene Management
Combat Systems
Command Systems


AI & AUTONOMY

AUTONOMOUS WARFARE SYSTEMS

AI Command
Unit Autonomy
Squad Decision Making
Sensor Processing
Command & Intent
Battlefield Decision Systems


WORLD & BATTLEFIELD

World Simulation
Terrain Systems
Battlefield Systems
Environmental Systems
Strategic World Systems


MILITARY SYSTEMS

Fleet Management
Logistics
Deployment
Military Budget
Operations
Intelligence
Research
Technology
Resource Management


GAMEPLAY

Ground Warfare
Air Warfare
Naval Warfare
Command Units
Squads
Vehicles
Weapons
Destruction
Recovery


UI / UX

Command Center
Operations
Garage
Research
Logistics
Intelligence
Deployment
Battlefield Interface
Universal HUD


AUDIO

Music
Sound Design
Combat Audio
Interface Audio


NARRATIVE

Obsidian Protocol
Autonomous Warfare
World & Faction Lore
Military Operations


TECHNOLOGY

Unity
C# Systems
Simulation Technology
Artificial Intelligence
Persistent World Systems


SPECIAL THANKS

Everyone who contributed feedback,
testing, ideas, and support.


OBSIDIAN PROTOCOL

AUTONOMOUS WARFARE


THE WAR DOES NOT WAIT.

COMMAND IT.


© 2026";

        GameObject creditsTextObject =
            CreateText(
                credits,
                content.transform,
                25,
                FontStyle.Normal);

        RectTransform creditsRect =
            creditsTextObject.GetComponent<RectTransform>();

        creditsRect.anchorMin =
            new Vector2(0.5f, 1f);

        creditsRect.anchorMax =
            new Vector2(0.5f, 1f);

        creditsRect.pivot =
            new Vector2(0.5f, 1f);

        creditsRect.anchoredPosition =
            new Vector2(0, -40);

        creditsRect.sizeDelta =
            new Vector2(1050, 2500);

        Text creditsText =
            creditsTextObject.GetComponent<Text>();

        creditsText.alignment =
            TextAnchor.UpperCenter;

        creditsText.horizontalOverflow =
            HorizontalWrapMode.Wrap;

        creditsText.verticalOverflow =
            VerticalWrapMode.Overflow;

        creditsText.lineSpacing = 1.35f;

        // ----------------------------------------------------
        // BACK BUTTON
        // ----------------------------------------------------

        GameObject back =
            CreateButton(
                "BACK",
                canvasObject.transform);

        SetRect(
            back,
            new Vector2(0.5f, 0f),
            new Vector2(0.5f, 0f),
            new Vector2(0, 65),
            new Vector2(260, 70));

        // ----------------------------------------------------
        // VERSION
        // ----------------------------------------------------

        GameObject version =
            CreateText(
                "OBSIDIAN PROTOCOL  //  v0.1",
                canvasObject.transform,
                15,
                FontStyle.Normal);

        SetRect(
            version,
            new Vector2(1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(-110, 30),
            new Vector2(300, 40));

        version.GetComponent<Text>().alignment =
            TextAnchor.MiddleRight;

        // ----------------------------------------------------
        // CONTROLLER
        // ----------------------------------------------------

        GameObject controllerObject =
            new GameObject(
                "[SYSTEM] CREDITS SCENE MANAGER");

        controllerObject.AddComponent<
            ObsidianProtocol.UI.Credits.CreditsSceneController>();

        SerializedObject controllerSerialized =
            new SerializedObject(
                controllerObject.GetComponent<
                    ObsidianProtocol.UI.Credits.CreditsSceneController>());

        controllerSerialized.FindProperty("creditsScroll")
            .objectReferenceValue = scroll;

        controllerSerialized.FindProperty("backButton")
            .objectReferenceValue =
                back.GetComponent<Button>();

        controllerSerialized.ApplyModifiedProperties();

        // ----------------------------------------------------
        // SAVE
        // ----------------------------------------------------

        EditorSceneManager.SaveScene(
            SceneManager.GetActiveScene(),
            ScenePath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            "[CREDITS] SCN17 Credits scene created successfully.");
    }

    private static GameObject CreatePanel(
        string name,
        Transform parent,
        Color color)
    {
        GameObject objectRoot =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(Image));

        objectRoot.transform.SetParent(
            parent,
            false);

        objectRoot.GetComponent<Image>().color =
            color;

        return objectRoot;
    }

    private static GameObject CreateText(
        string text,
        Transform parent,
        int size,
        FontStyle style)
    {
        GameObject objectRoot =
            new GameObject(
                "Text",
                typeof(RectTransform),
                typeof(Text));

        objectRoot.transform.SetParent(
            parent,
            false);

        Text textComponent =
            objectRoot.GetComponent<Text>();

        textComponent.text = text;
        textComponent.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        textComponent.fontSize = size;
        textComponent.fontStyle = style;
        textComponent.color = Color.white;

        textComponent.raycastTarget = false;

        return objectRoot;
    }

    private static GameObject CreateButton(
        string label,
        Transform parent)
    {
        GameObject buttonObject =
            new GameObject(
                label,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button));

        buttonObject.transform.SetParent(
            parent,
            false);

        Image image =
            buttonObject.GetComponent<Image>();

        image.color =
            new Color(0.08f, 0.10f, 0.13f, 0.95f);

        GameObject textObject =
            CreateText(
                label,
                buttonObject.transform,
                22,
                FontStyle.Bold);

        SetFullScreen(textObject);

        textObject.GetComponent<Text>().alignment =
            TextAnchor.MiddleCenter;

        return buttonObject;
    }

    private static void SetFullScreen(
        GameObject objectRoot)
    {
        RectTransform rect =
            objectRoot.GetComponent<RectTransform>();

        rect.anchorMin =
            Vector2.zero;

        rect.anchorMax =
            Vector2.one;

        rect.offsetMin =
            Vector2.zero;

        rect.offsetMax =
            Vector2.zero;
    }

    private static void SetRect(
        GameObject objectRoot,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 position,
        Vector2 size)
    {
        RectTransform rect =
            objectRoot.GetComponent<RectTransform>();

        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
    }
}

