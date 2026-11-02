using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public static class AcheronFinalHUDBuilder
{
    const string HUD = "ACHERON_FINAL_HUD";

    [MenuItem("Obsidian Protocol/Acheron/BUILD FINAL HUD")]
    public static void Build()
    {
        string scenePath = Directory.GetFiles(
            "Assets/Scenes",
            "*.unity",
            SearchOption.AllDirectories)
            .FirstOrDefault(p =>
                Path.GetFileNameWithoutExtension(p)
                    .ToUpperInvariant()
                    .Contains("ACHERON"));

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("ACHERON scene not found.");
            return;
        }

        var scene = EditorSceneManager.OpenScene(scenePath);

        GameObject old = GameObject.Find(HUD);
        if (old != null)
            Object.DestroyImmediate(old);

        GameObject duplicate = GameObject.Find("ACHERON_PLANETARY_DATA");
        if (duplicate != null)
            Object.DestroyImmediate(duplicate);

        GameObject stub = GameObject.Find("PLANET OVERVIEW HUD");
        if (stub != null)
            stub.SetActive(false);

        GameObject root = new GameObject(HUD);

        Canvas canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 500;

        CanvasScaler scaler = root.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = .5f;

        root.AddComponent<GraphicRaycaster>();

        Background(root.transform);
        Header(root.transform);
        Information(root.transform);
        Location(root.transform);
        PlanetaryData(root.transform);
        Actions(root.transform);
        Close(root.transform);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("ACHERON FINAL HUD BUILT CLEANLY.");
    }

    static Font F()
    {
        return Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }

    static GameObject Panel(
        Transform parent,
        string name,
        float x1,
        float y1,
        float x2,
        float y2)
    {
        GameObject o = new GameObject(name);
        o.transform.SetParent(parent, false);

        RectTransform r = o.AddComponent<RectTransform>();
        r.anchorMin = new Vector2(x1, y1);
        r.anchorMax = new Vector2(x2, y2);
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;

        Image i = o.AddComponent<Image>();
        i.color = new Color(.025f, .032f, .045f, .98f);

        return o;
    }

    static Text Label(
        Transform parent,
        string name,
        string value,
        int size,
        TextAnchor anchor = TextAnchor.UpperLeft)
    {
        GameObject o = new GameObject(name);
        o.transform.SetParent(parent, false);

        RectTransform r = o.AddComponent<RectTransform>();
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.offsetMin = new Vector2(16, 10);
        r.offsetMax = new Vector2(-16, -10);

        Text t = o.AddComponent<Text>();
        t.font = F();
        t.fontSize = size;
        t.color = Color.white;
        t.alignment = anchor;
        t.text = value;
        t.horizontalOverflow = HorizontalWrapMode.Wrap;
        t.verticalOverflow = VerticalWrapMode.Truncate;

        return t;
    }

    static void Background(Transform root)
    {
        GameObject o = Panel(root, "BACKGROUND", 0, 0, 1, 1);
        o.transform.SetAsFirstSibling();

        Image i = o.GetComponent<Image>();
        i.color = new Color(.008f, .012f, .02f, 1f);
    }

    static void Header(Transform root)
    {
        GameObject p = Panel(root, "TOP_BAR", .02f, .91f, .98f, .98f);

        Label(
            p.transform,
            "TITLE",
            "ACHERON  //  PLANET OVERVIEW  //  PLN-01",
            24,
            TextAnchor.MiddleLeft);
    }

    static void Information(Transform root)
    {
        GameObject p = Panel(
            root,
            "PLANET_INFORMATION_PANEL_NEW",
            .02f, .58f, .49f, .89f);

        Label(
            p.transform,
            "DATA",
            "PLANET INFORMATION\n\n" +
            "PLANET ID        PLN-01\n" +
            "PLANET NAME      ACHERON\n" +
            "CLASS            PLANETARY WORLD\n" +
            "FACTION          MULTI-FACTION\n" +
            "WARFARE FOCUS    PLANETARY CONFLICT\n" +
            "THREAT LEVEL     HIGH\n" +
            "STATUS           OPERATIONAL",
            18);
    }

    static void Location(Transform root)
    {
        GameObject p = Panel(
            root,
            "PLANET_LOCATION_PANEL_NEW",
            .51f, .58f, .98f, .89f);

        Label(
            p.transform,
            "DATA",
            "LOCATION\n\n" +
            "GALAXY           CURRENT GALAXY\n" +
            "SECTOR           CURRENT SECTOR\n" +
            "SYSTEM           CURRENT SYSTEM\n" +
            "STAR             CURRENT STAR\n" +
            "ORBIT            STABLE\n" +
            "ORBITAL DISTANCE --\n" +
            "ORBITAL VELOCITY --",
            18);
    }

    static void PlanetaryData(Transform root)
    {
        GameObject p = Panel(
            root,
            "ACHERON_PLANETARY_DATA_MAIN",
            .02f, .17f, .98f, .56f);

        Label(
            p.transform,
            "DATA",
            "ACHERON PLANETARY DATA\n\n" +

            "CONT-01A  BLACKSPIRE CONTINENT\n" +
            "REG-01A  Blackspire Range     | BIO-01A  Volcanic Highlands\n" +
            "ISL-01A  Blackspire Expanse    | Mountain / Industrial\n" +
            "Combined arms, elevation, industrial combat | Dominion of Acheron\n\n" +

            "CONT-01A  BLACKSPIRE CONTINENT\n" +
            "REG-01B  Obsidian Lowlands    | BIO-01B  Basalt Coast\n" +
            "ISL-01B  Ashen Wastes          | Mountain / Mining\n" +
            "Ground warfare, logistics, chokepoints | Riftguard Coalition\n\n" +

            "CONT-01B  RIFTFORGE CONTINENT\n" +
            "REG-01C  Rift Plateau Zone     | BIO-01C  Charred Desert\n" +
            "ISL-01C  Obsidian Coast         | Urban\n" +
            "Close-range urban warfare      | Obsidian Compact\n\n" +

            "CONT-01C  ASHFALL CONTINENT\n" +
            "REG-01D  Ashen Frontier        | BIO-01D  Industrial Badlands\n" +
            "ISL-01D  Rift Plateau           | Volcanic / Industrial\n" +
            "Harsh terrain, visibility, environmental effects | Ashen Sovereignty",
            14);
    }

    static void Actions(Transform root)
    {
        GameObject p = Panel(
            root,
            "PLANET_ACTIONS_PANEL",
            .02f, .055f, .98f, .15f);

        Label(
            p.transform,
            "TITLE",
            "PLANET ACTIONS",
            14,
            TextAnchor.UpperLeft);

        string[] names =
        {
            "ENTER PLANET",
            "INTELLIGENCE",
            "OPERATIONS",
            "DEPLOYMENT",
            "LOGISTICS"
        };

        for (int i = 0; i < names.Length; i++)
        {
            float x1 = .20f + i * .155f;
            float x2 = x1 + .145f;

            GameObject b = new GameObject(names[i]);
            b.transform.SetParent(p.transform, false);

            RectTransform r = b.AddComponent<RectTransform>();
            r.anchorMin = new Vector2(x1, .15f);
            r.anchorMax = new Vector2(x2, .82f);
            r.offsetMin = Vector2.zero;
            r.offsetMax = Vector2.zero;

            Image image = b.AddComponent<Image>();
            image.color = new Color(.08f, .10f, .14f, 1f);

            Button button = b.AddComponent<Button>();
            button.targetGraphic = image;

            Label(
                b.transform,
                "LABEL",
                names[i],
                12,
                TextAnchor.MiddleCenter);
        }
    }

    static void Close(Transform root)
    {
        GameObject b = new GameObject("CLOSE");
        b.transform.SetParent(root, false);

        RectTransform r = b.AddComponent<RectTransform>();
        r.anchorMin = new Vector2(.91f, .005f);
        r.anchorMax = new Vector2(.98f, .045f);
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;

        Image image = b.AddComponent<Image>();
        image.color = new Color(.12f, .07f, .07f, 1f);

        Button button = b.AddComponent<Button>();
        button.targetGraphic = image;

        Label(
            b.transform,
            "LABEL",
            "CLOSE",
            12,
            TextAnchor.MiddleCenter);

        button.onClick.AddListener(() =>
        {
            root.gameObject.SetActive(false);
        });
    }
}
