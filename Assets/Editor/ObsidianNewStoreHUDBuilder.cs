using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public static class ObsidianNewStoreHUDBuilder
{
    private const string MenuPath = "Obsidian Protocol/Store/Create NEW Store HUD";
    private const string ImageFolder = "Assets/Resources/StoreImages";
    private static Font fallbackFont;

    [MenuItem(MenuPath)]
    public static void Build()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        Scene scene = SceneManager.GetActiveScene();

        if (string.IsNullOrEmpty(scene.path))
        {
            EditorUtility.DisplayDialog(
                "Save Scene First",
                "Open and save the store scene before building the new HUD.",
                "OK");
            return;
        }

        bool proceed = EditorUtility.DisplayDialog(
            "Build New Store HUD",
            "Create a completely NEW store HUD in the current scene?\n\n" +
            "This creates a separate canvas and does not delete your existing store objects. " +
            "The new HUD is a visual foundation; existing purchase integration is preserved for a later wiring step.",
            "BUILD NEW HUD",
            "Cancel");

        if (!proceed) return;

        foreach (GameObject oldRoot in
            Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(g => g != null &&
                    g.name == "NEW_STORE_HUD_ROOT" &&
                    g.scene.IsValid() &&
                    g.scene == scene))
        {
            Undo.DestroyObjectImmediate(oldRoot);
        }

        Selection.activeObject = null;

        GameObject root = new GameObject("NEW_STORE_HUD_ROOT");
        Undo.RegisterCreatedObjectUndo(root, "Create New Store HUD");

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.pixelPerfect = false;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
        root.AddComponent<GraphicRaycaster>();

        if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("NEW_STORE_EVENT_SYSTEM");
            Undo.RegisterCreatedObjectUndo(es, "Create Store Event System");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        BuildBackdrop(root.transform);
        BuildHeader(root.transform);
        BuildLeftRail(root.transform);
        BuildCatalog(root.transform);
        BuildDossier(root.transform);
        BuildFooter(root.transform);
        root.AddComponent<NewStoreHUDRuntime>();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = root;

        EditorUtility.DisplayDialog(
            "New Store HUD Created",
            "The new HUD has been created and the scene saved.\n\n" +
            "Next: inspect it in the Scene view and Play mode. " +
            "This first build deliberately leaves the existing store and purchasing code untouched.",
            "OK");
    }

    private static void CreateCroppedImageArea(
        Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax)
    {
        GameObject container = new GameObject(
            name, typeof(RectTransform), typeof(Image), typeof(RectMask2D));
        container.transform.SetParent(parent, false);

        RectTransform containerRect = container.GetComponent<RectTransform>();
        containerRect.anchorMin = anchorMin;
        containerRect.anchorMax = anchorMax;
        containerRect.offsetMin = Vector2.zero;
        containerRect.offsetMax = Vector2.zero;

        Image background = container.GetComponent<Image>();
        background.color = new Color(0f, 0f, 0f, 0f);
        background.raycastTarget = false;

        GameObject art = new GameObject(
            "ART_IMAGE", typeof(RectTransform), typeof(CanvasRenderer),
            typeof(Image), typeof(AspectRatioFitter));
        art.transform.SetParent(container.transform, false);

        RectTransform artRect = art.GetComponent<RectTransform>();
        artRect.anchorMin = Vector2.zero;
        artRect.anchorMax = Vector2.one;
        artRect.offsetMin = Vector2.zero;
        artRect.offsetMax = Vector2.zero;

        Image artImage = art.GetComponent<Image>();
        artImage.color = Color.white;
        artImage.preserveAspect = false;
        artImage.raycastTarget = false;

        AspectRatioFitter fitter = art.GetComponent<AspectRatioFitter>();
        fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        fitter.aspectRatio = 1f;
    }
    private static Color C(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color c)) return c;
        return Color.white;
    }

    private static GameObject Panel(
        Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax,
        Vector2 offsetMin, Vector2 offsetMax, Color color)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = offsetMin;
        rt.offsetMax = offsetMax;

        Image image = go.GetComponent<Image>();
        image.color = color;
        image.raycastTarget = false;
        return go;
    }

    private static TextMeshProUGUI Text(
        Transform parent, string name, string value, int size, Color color,
        TextAlignmentOptions alignment = TextAlignmentOptions.Left)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(10, 4);
        rt.offsetMax = new Vector2(-10, -4);

        TextMeshProUGUI tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = value;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = alignment;
        tmp.textWrappingMode = TMPro.TextWrappingModes.Normal;
        tmp.raycastTarget = false;
        tmp.font = TMP_Settings.defaultFontAsset;
        return tmp;
    }

    private static void Label(
        Transform parent, string name, string value, int size, Color color,
        Vector2 amin, Vector2 amax, Vector2 omin, Vector2 omax,
        TextAlignmentOptions align = TextAlignmentOptions.Left)
    {
        TextMeshProUGUI t = Text(parent, name, value, size, color, align);
        RectTransform rt = t.rectTransform;
        rt.anchorMin = amin;
        rt.anchorMax = amax;
        rt.offsetMin = omin;
        rt.offsetMax = omax;
    }

    private static void Button(
        Transform parent, string name, string caption,
        Vector2 amin, Vector2 amax, Vector2 omin, Vector2 omax,
        Color background, Color foreground, int fontSize = 16)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = amin;
        rt.anchorMax = amax;
        rt.offsetMin = omin;
        rt.offsetMax = omax;

        Image img = go.GetComponent<Image>();
        img.color = background;

        Button button = go.GetComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = background;
        colors.highlightedColor = new Color(
            Mathf.Min(1, background.r + 0.12f),
            Mathf.Min(1, background.g + 0.12f),
            Mathf.Min(1, background.b + 0.12f), 1);
        colors.pressedColor = C("#1A3544");
        colors.selectedColor = background;
        button.colors = colors;

        TextMeshProUGUI t = Text(go.transform, "Caption", caption, fontSize, foreground,
            TextAlignmentOptions.Center);
        t.fontStyle = FontStyles.Bold;
    }

    private static void BuildBackdrop(Transform root)
    {
        Panel(root, "BACKGROUND", Vector2.zero, Vector2.one,
            Vector2.zero, Vector2.zero, C("#080F17"));

        Panel(root, "BACKGROUND_TOP_GLOW", new Vector2(0, .82f), Vector2.one,
            Vector2.zero, Vector2.zero, C("#101E2A"));

        Panel(root, "TOP_ACCENT_LINE", new Vector2(0, .943f), Vector2.one,
            Vector2.zero, new Vector2(0, -2), C("#47D7D0"));

        Panel(root, "LEFT_ACCENT_LINE", Vector2.zero, new Vector2(0, 1),
            new Vector2(0, 0), new Vector2(3, 0), C("#47D7D0"));
    }

    private static void BuildHeader(Transform root)
    {
        Transform header = Panel(root, "COMMAND_HEADER",
            new Vector2(0, .92f), new Vector2(1, 1),
            new Vector2(18, 0), new Vector2(-18, -10), C("#111E2A")).transform;

        Label(header, "BRAND", "OBSIDIAN PROTOCOL", 24, C("#EAF4FA"),
            new Vector2(0, 0), new Vector2(.27f, 1),
            new Vector2(12, 4), new Vector2(0, -4));

        Label(header, "SUBBRAND", "MILITARY PROCUREMENT  /  UNIT ARCHIVE", 11, C("#7F9AAA"),
            new Vector2(.0f, 0), new Vector2(.39f, .36f),
            new Vector2(13, 0), Vector2.zero);

        Label(header, "CENTER_TITLE", "TACTICAL ASSET EXCHANGE", 20, C("#47D7D0"),
            new Vector2(.37f, 0), new Vector2(.68f, 1),
            Vector2.zero, Vector2.zero, TextAlignmentOptions.Center);

        Label(header, "NETWORK_STATUS", "Ã¢â€”Â NETWORK ONLINE", 13, C("#77E0A4"),
            new Vector2(.72f, .5f), new Vector2(.86f, 1),
            Vector2.zero, Vector2.zero, TextAlignmentOptions.Center);

        Label(header, "CREDITS", "CREDITS   Ã¢â‚¬â€", 16, C("#F2D38A"),
            new Vector2(.86f, 0), new Vector2(1, 1),
            Vector2.zero, new Vector2(-12, 0), TextAlignmentOptions.Right);

        Panel(header, "HEADER_DIVIDER",
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(8, 0), new Vector2(-8, 1), C("#294251"));
    }

    private static void BuildLeftRail(Transform root)
    {
        Transform rail = Panel(root, "CATEGORY_RAIL",
            new Vector2(0, .12f), new Vector2(.16f, .91f),
            new Vector2(18, 0), new Vector2(-8, 0), C("#101A25")).transform;

        Label(rail, "RAIL_TITLE", "FORCE INDEX", 14, C("#47D7D0"),
            new Vector2(0, .91f), new Vector2(1, 1),
            new Vector2(10, 0), new Vector2(-10, -2));

        Label(rail, "RAIL_HINT", "SELECT A DIVISION", 10, C("#7793A3"),
            new Vector2(0, .865f), new Vector2(1, .92f),
            new Vector2(10, 0), new Vector2(-10, 0));

        string[] categories =
        {
            "ALL ASSETS",
            "AIR UNIT",
            "GROUND UNIT",
            "FIGHTER",
            "STATION",
            "SATELLITE",
            "NAVAL",
            "TRANSPORT",
            "RECON",
            "SHUTTLE",
            "MEDICAL",
            "EXPLORATION"
        };

        float top = .82f;
        float height = .066f;

        for (int i = 0; i < categories.Length; i++)
        {
            float yMax = top - i * height;
            float yMin = yMax - height + .006f;
            Color bg = i == 0 ? C("#1B3A46") : C("#15232F");
            Color fg = i == 0 ? C("#6FF4E8") : C("#B7C9D3");

            Button(rail, "CATEGORY_" + i, "Ã¢â€“Â¸   " + categories[i],
                new Vector2(.035f, yMin), new Vector2(.965f, yMax),
                Vector2.zero, Vector2.zero, bg, fg, 13);
        }

        Panel(rail, "RAIL_RULE",
            new Vector2(.06f, .08f), new Vector2(.94f, .08f),
            Vector2.zero, new Vector2(0, 1), C("#294251"));

        Label(rail, "RAIL_FOOTER", "OPAW  /  PROCUREMENT", 10, C("#607D8C"),
            new Vector2(0, 0), new Vector2(1, .07f),
            new Vector2(10, 0), new Vector2(-10, 0));
    }

    private static void BuildCatalog(Transform root)
    {
        Transform catalog = Panel(root, "ASSET_CATALOG",
            new Vector2(.17f, .12f), new Vector2(.66f, .91f),
            new Vector2(0, 0), new Vector2(-8, 0), C("#0D1721")).transform;

        Label(catalog, "CATALOG_TITLE", "AVAILABLE ASSETS", 22, C("#EAF4FA"),
            new Vector2(0, .91f), new Vector2(.62f, 1),
            new Vector2(14, 0), new Vector2(0, -4));

        Label(catalog, "CATALOG_COUNT", "UNIT DATABASE  /  71 ASSETS", 11, C("#7F9AAA"),
            new Vector2(0, .87f), new Vector2(.62f, .92f),
            new Vector2(14, 0), Vector2.zero);

        Transform search = Panel(catalog, "SEARCH_FIELD",
            new Vector2(.61f, .89f), new Vector2(.99f, .98f),
            Vector2.zero, Vector2.zero, C("#172733")).transform;

        Label(search, "SEARCH_PLACEHOLDER", "Ã¢Å’â€¢   SEARCH UNITS...", 13, C("#91A9B7"),
            Vector2.zero, Vector2.one, new Vector2(8, 0), new Vector2(-8, 0));

        Panel(catalog, "CATALOG_RULE",
            new Vector2(.025f, .855f), new Vector2(.975f, .855f),
            Vector2.zero, new Vector2(0, 1), C("#294251"));

        float left = .025f;
        float right = .975f;
        float gapX = .018f;
        float cardW = (right - left - gapX * 2) / 3f;
        float top = .835f;
        float cardH = .245f;
        float gapY = .018f;

        for (int i = 0; i < 9; i++)
        {
            int row = i / 3;
            int col = i % 3;
            float xMin = left + col * (cardW + gapX);
            float xMax = xMin + cardW;
            float yMax = top - row * (cardH + gapY);
            float yMin = yMax - cardH;

            Transform card = Panel(catalog, "ASSET_CARD_" + (i + 1),
                new Vector2(xMin, yMin), new Vector2(xMax, yMax),
                Vector2.zero, Vector2.zero, C("#15232F")).transform;

            Panel(card, "CARD_TOP_EDGE",
                new Vector2(0, .985f), new Vector2(1, 1),
                Vector2.zero, Vector2.zero,
                i == 0 ? C("#47D7D0") : C("#294251"));

            CreateCroppedImageArea(card, "ART_FRAME",
                new Vector2(.045f, .32f), new Vector2(.955f, .91f));

            Label(card, "ART_STATUS", "ASSET IMAGE", 10, C("#607D8C"),
                new Vector2(.045f, .32f), new Vector2(.955f, .91f),
                Vector2.zero, Vector2.zero, TextAlignmentOptions.Center);

            Label(card, "UNIT_NAME", "UNIT NAME", 15, C("#EAF4FA"),
                new Vector2(.035f, .17f), new Vector2(.72f, .32f),
                Vector2.zero, Vector2.zero);

            Label(card, "UNIT_ROLE", "UNIT CLASS / ROLE", 10, C("#819AA8"),
                new Vector2(.035f, .06f), new Vector2(.72f, .17f),
                Vector2.zero, Vector2.zero);

            Label(card, "UNIT_COST", "COST  Ã¢â‚¬â€", 12, C("#F2D38A"),
                new Vector2(.68f, .06f), new Vector2(.97f, .32f),
                Vector2.zero, Vector2.zero, TextAlignmentOptions.Right);
        }

        Label(catalog, "CATALOG_NOTE",
            "SELECT AN ASSET TO REVIEW ITS OPERATIONAL DOSSIER",
            10, C("#607D8C"),
            new Vector2(.02f, .005f), new Vector2(.98f, .035f),
            Vector2.zero, Vector2.zero, TextAlignmentOptions.Center);
    }

    private static void BuildDossier(Transform root)
    {
        Transform detail = Panel(root, "UNIT_DOSSIER",
            new Vector2(.67f, .12f), new Vector2(.99f, .91f),
            new Vector2(0, 0), new Vector2(-18, 0), C("#101B26")).transform;

        Label(detail, "DOSSIER_KICKER", "SELECTED ASSET  /  TECHNICAL RECORD", 11, C("#47D7D0"),
            new Vector2(.02f, .94f), new Vector2(.98f, 1),
            new Vector2(8, 0), new Vector2(-8, 0));

        CreateCroppedImageArea(detail, "DOSSIER_ART",
            new Vector2(.01f, .64f), new Vector2(.99f, .94f));

        Label(detail, "DOSSIER_ART_HINT", "UNIT IMAGE / RENDER", 11, C("#607D8C"),
            new Vector2(.03f, .67f), new Vector2(.97f, .93f),
            Vector2.zero, Vector2.zero, TextAlignmentOptions.Center);


        Label(detail, "DOSSIER_UNIT_NAME", "WARDEN", 28, C("#EAF4FA"),
            new Vector2(.04f, .59f), new Vector2(.96f, .68f),
            new Vector2(0, 0), new Vector2(0, -2));

        Label(detail, "DOSSIER_ROLE", "AERIAL SURVEILLANCE / RECON INTELLIGENCE", 11, C("#7F9AAA"),
            new Vector2(.04f, .55f), new Vector2(.96f, .60f),
            Vector2.zero, Vector2.zero);

        Label(detail, "DOSSIER_ID", "UNIT ID   obs-air-warden", 11, C("#47D7D0"),
            new Vector2(.04f, .50f), new Vector2(.96f, .55f),
            Vector2.zero, Vector2.zero);

        Panel(detail, "DOSSIER_RULE",
            new Vector2(.04f, .49f), new Vector2(.96f, .49f),
            Vector2.zero, new Vector2(0, 1), C("#294251"));

        Label(detail, "DOSSIER_SECTION_TITLE", "OPERATIONAL OVERVIEW", 13, C("#EAF4FA"),
            new Vector2(.04f, .445f), new Vector2(.96f, .49f),
            Vector2.zero, Vector2.zero);

        Label(detail, "DOSSIER_SUMMARY",
            "Autonomous aerial reconnaissance platform. Conducts visual and thermal surveillance, " +
            "identifies and marks targets, and relays intelligence. Non-combat platform; no weapons.",
            12, C("#B5C7D1"),
            new Vector2(.04f, .34f), new Vector2(.96f, .445f),
            Vector2.zero, Vector2.zero);
        Label(detail, "DOSSIER_STATS_TITLE", "TECHNICAL RECORD", 13, C("#EAF4FA"),
            new Vector2(.04f, .295f), new Vector2(.96f, .34f),
            Vector2.zero, Vector2.zero);

        GameObject technicalScrollObject = new GameObject(
            "DOSSIER_TECHNICAL_SCROLL",
            typeof(RectTransform),
            typeof(Image),
            typeof(ScrollRect));
        technicalScrollObject.transform.SetParent(detail, false);

        RectTransform technicalScrollRect =
            technicalScrollObject.GetComponent<RectTransform>();
        technicalScrollRect.anchorMin = new Vector2(.04f, .09f);
        technicalScrollRect.anchorMax = new Vector2(.96f, .292f);
        technicalScrollRect.offsetMin = Vector2.zero;
        technicalScrollRect.offsetMax = Vector2.zero;

        Image technicalScrollBackground =
            technicalScrollObject.GetComponent<Image>();
        technicalScrollBackground.color = C("#0B141D");
        technicalScrollBackground.raycastTarget = true;

        GameObject viewportObject = new GameObject(
            "VIEWPORT",
            typeof(RectTransform),
            typeof(Image),
            typeof(RectMask2D));
        viewportObject.transform.SetParent(technicalScrollObject.transform, false);

        RectTransform viewportRect = viewportObject.GetComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(8f, 6f);
        viewportRect.offsetMax = new Vector2(-8f, -6f);

        Image viewportImage = viewportObject.GetComponent<Image>();
        viewportImage.color = Color.white;
        viewportImage.raycastTarget = true;

        GameObject contentObject = new GameObject(
            "CONTENT",
            typeof(RectTransform),
            typeof(TextMeshProUGUI),
            typeof(ContentSizeFitter));
        contentObject.transform.SetParent(viewportObject.transform, false);

        RectTransform contentRect = contentObject.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);
        contentRect.pivot = new Vector2(.5f, 1f);
        contentRect.offsetMin = Vector2.zero;
        contentRect.offsetMax = Vector2.zero;

        TextMeshProUGUI technicalText = contentObject.GetComponent<TextMeshProUGUI>();
        technicalText.text = "SELECT A UNIT TO LOAD ITS TECHNICAL RECORD.";
        technicalText.fontSize = 18;
        technicalText.color = C("#B8CBD5");
        technicalText.alignment = TextAlignmentOptions.TopLeft;
        technicalText.textWrappingMode = TextWrappingModes.Normal;
        technicalText.richText = false;
        technicalText.raycastTarget = false;
        technicalText.margin = new Vector4(2f, 2f, 2f, 8f);

        ContentSizeFitter contentFitter = contentObject.GetComponent<ContentSizeFitter>();
        contentFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        ScrollRect technicalScroll = technicalScrollObject.GetComponent<ScrollRect>();
        technicalScroll.viewport = viewportRect;
        technicalScroll.content = contentRect;
        technicalScroll.horizontal = false;
        technicalScroll.vertical = true;
        technicalScroll.movementType = ScrollRect.MovementType.Clamped;
        technicalScroll.scrollSensitivity = 24f;

        Panel(detail, "DOSSIER_PURCHASE_PANEL",
            new Vector2(.035f, .035f), new Vector2(.965f, .075f),
            Vector2.zero, Vector2.zero, C("#1B3A46"));

        Label(detail, "PURCHASE_PRICE", "ACQUISITION COST  /  DATABASE LINK PENDING",
            11, C("#F2D38A"),
            new Vector2(.04f, .035f), new Vector2(.64f, .075f),
            Vector2.zero, Vector2.zero);

        Button(detail, "PURCHASE_BUTTON", "ACQUIRE UNIT",
            new Vector2(.65f, .025f), new Vector2(.965f, .085f),
            Vector2.zero, Vector2.zero, C("#47D7D0"), C("#07141A"), 13);

        Label(detail, "DOSSIER_NOTICE",
            "PURCHASE ACTION NOT CONNECTED IN THIS VISUAL BUILD",
            9, C("#D3A56B"),
            new Vector2(.04f, .005f), new Vector2(.96f, .03f),
            Vector2.zero, Vector2.zero, TextAlignmentOptions.Center);
    }

    private static void BuildFooter(Transform root)
    {
        Transform footer = Panel(root, "STATUS_FOOTER",
            new Vector2(0, .035f), new Vector2(1, .105f),
            new Vector2(18, 0), new Vector2(-18, 0), C("#0D1721")).transform;

        Panel(footer, "FOOTER_RULE",
            new Vector2(0, .98f), new Vector2(1, 1),
            Vector2.zero, Vector2.zero, C("#294251"));

        Label(footer, "FOOTER_LEFT",
            "OBSIDIAN PROTOCOL  /  AUTONOMOUS WARFARE",
            10, C("#7893A2"),
            new Vector2(0, 0), new Vector2(.48f, 1),
            new Vector2(10, 0), Vector2.zero);

        Label(footer, "FOOTER_CENTER",
            "STORE INTERFACE  //  NEW HUD",
            10, C("#47D7D0"),
            new Vector2(.35f, 0), new Vector2(.7f, 1),
            Vector2.zero, Vector2.zero, TextAlignmentOptions.Center);

        Label(footer, "FOOTER_RIGHT",
            "CATALOG / DOSSIER / PROCUREMENT",
            10, C("#7893A2"),
            new Vector2(.7f, 0), new Vector2(1, 1),
            Vector2.zero, new Vector2(-10, 0), TextAlignmentOptions.Right);
    }
}








