using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class AcheronPlanetaryDataRowsBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN30_ACHERON/Acheron.unity";

    [MenuItem("Obsidian Protocol/Acheron/BUILD PLANETARY DATA ROWS")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath);

        GameObject panel = GameObject.Find("ACHERON_PLANETARY_DATA_MAIN");

        if (panel == null)
        {
            Debug.LogError("[ACHERON DATA] ACHERON_PLANETARY_DATA_MAIN not found.");
            return;
        }

        Transform old = panel.transform.Find("INTERACTIVE_PLANETARY_ROWS");

        if (old != null)
            Object.DestroyImmediate(old.gameObject);

        GameObject container = new GameObject(
            "INTERACTIVE_PLANETARY_ROWS",
            typeof(RectTransform),
            typeof(VerticalLayoutGroup)
        );

        container.transform.SetParent(panel.transform, false);

        RectTransform rt = container.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.03f, 0.08f);
        rt.anchorMax = new Vector2(0.97f, 0.82f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        VerticalLayoutGroup layout =
            container.GetComponent<VerticalLayoutGroup>();

        layout.spacing = 8f;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = true;
        layout.childControlWidth = true;
        layout.childControlHeight = true;

        CreateRow(
            container.transform,
            "ROW_01_BLACKSPIRE_RANGE",
            "REG-01A  BLACKSPIRE RANGE",
            "BIO-01A  Volcanic Highlands",
            "ISL-01A  Blackspire Expanse",
            "Mountain / Industrial",
            "Combined arms, elevation, industrial combat",
            "Dominion of Acheron"
        );

        CreateRow(
            container.transform,
            "ROW_02_OBSIDIAN_LOWLANDS",
            "REG-01B  OBSIDIAN LOWLANDS",
            "BIO-01B  Basalt Coast",
            "ISL-01B  Ashen Wastes",
            "Mountain / Mining",
            "Ground warfare, logistics, chokepoints",
            "Riftguard Coalition"
        );

        CreateRow(
            container.transform,
            "ROW_03_RIFT_PLATEAU",
            "REG-01C  RIFT PLATEAU ZONE",
            "BIO-01C  Charred Desert",
            "ISL-01C  Obsidian Coast",
            "Urban",
            "Close-range urban warfare",
            "Obsidian Compact"
        );

        CreateRow(
            container.transform,
            "ROW_04_ASHEN_FRONTIER",
            "REG-01D  ASHEN FRONTIER",
            "BIO-01D  Industrial Badlands",
            "ISL-01D  Rift Plateau",
            "Volcanic / Industrial",
            "Harsh terrain, visibility, environmental effects",
            "Ashen Sovereignty"
        );

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[ACHERON DATA] FOUR PLANETARY DATA ROWS BUILT.");
    }

    private static void CreateRow(
        Transform parent,
        string objectName,
        string region,
        string biosphere,
        string island,
        string terrain,
        string warfare,
        string faction)
    {
        GameObject row = new GameObject(
            objectName,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        row.transform.SetParent(parent, false);

        Image image = row.GetComponent<Image>();
        image.color = new Color(0.025f, 0.035f, 0.05f, 0.92f);

        LayoutElement element = row.AddComponent<LayoutElement>();
        element.minHeight = 90f;
        element.preferredHeight = 90f;

        GameObject textObject = new GameObject(
            "DATA",
            typeof(RectTransform),
            typeof(TextMeshProUGUI)
        );

        textObject.transform.SetParent(row.transform, false);

        RectTransform textRT = textObject.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0.02f, 0.05f);
        textRT.anchorMax = new Vector2(0.98f, 0.95f);
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        TextMeshProUGUI text =
            textObject.GetComponent<TextMeshProUGUI>();

        text.text =
            region + "\n" +
            biosphere + "   |   " + island + "\n" +
            terrain + "\n" +
            warfare + "   |   " + faction;

        text.fontSize = 15f;
        text.alignment = TextAlignmentOptions.Left;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.color = Color.white;

        Button button = row.GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            Debug.Log(
                "[ACHERON DATA] SELECTED: " +
                region + " | " +
                biosphere + " | " +
                island + " | " +
                terrain + " | " +
                warfare + " | " +
                faction
            );
        });
    }
}
