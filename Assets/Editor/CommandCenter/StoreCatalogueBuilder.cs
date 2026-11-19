using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.Store.UI;
using ObsidianProtocol.Game.Units;

public static class StoreCatalogueBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity";

    private static readonly string[] Folders =
    {
        "AIR UNIT", "COMMAND UNIT", "EXPERIMENTAL",
        "GROUND UNIT", "SATELLITE", "SEA UNIT"
    };

    [MenuItem("Obsidian Protocol/Command Center/BUILD UNIT STORE CATALOGUE")]
    public static void Build()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath);

        GameObject store = FindObject("STORE_REAL_PANEL");
        GameObject root = FindObject("AAA_UNIT_STORE");
        GameObject scrollObject = FindObject("CATALOGUE_SCROLL");
        GameObject indicators = FindObject("CATEGORY_INDICATORS");

        if (store == null || root == null || scrollObject == null || indicators == null)
        {
            Debug.LogError("[UNIT STORE] Required existing Store objects are missing.");
            return;
        }

        var controller = root.GetComponent<StoreCatalogueInteractionController>();
        RectTransform catalogueRect = scrollObject.GetComponent<RectTransform>();
        RectTransform detailsRect = FindObject("UNIT_DETAILS_PANEL").GetComponent<RectTransform>();

        catalogueRect.anchorMin = new Vector2(0f, 0f);
        catalogueRect.anchorMax = new Vector2(1f, 1f);
        catalogueRect.offsetMin = new Vector2(0f, 0f);
        catalogueRect.offsetMax = new Vector2(0f, -190f);
        catalogueRect.anchoredPosition = new Vector2(0f, -95f);
        catalogueRect.sizeDelta = new Vector2(0f, -190f);

        detailsRect.anchorMin = new Vector2(0.5f, 0f);
        detailsRect.anchorMax = new Vector2(1f, 1f);
        detailsRect.pivot = new Vector2(0.5f, 0.5f);
        detailsRect.anchoredPosition = new Vector2(0f, -95f);
        detailsRect.sizeDelta = new Vector2(-24f, -190f);
        var scroll = scrollObject.GetComponent<ScrollRect>();

        if (controller == null || scroll == null || scroll.content == null)
        {
            Debug.LogError("[UNIT STORE] Controller or scroll content is missing.");
            return;
        }

        Transform duplicate = store.transform.Find("UNIT_STORE_CATALOGUE");
        if (duplicate != null)
            Object.DestroyImmediate(duplicate.gameObject);

        Transform body = scroll.content;
        for (int i = body.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(body.GetChild(i).gameObject);

        var bodyRect = body.GetComponent<RectTransform>();
        bodyRect.anchorMin = new Vector2(0, 1);
        bodyRect.anchorMax = new Vector2(1, 1);
        bodyRect.pivot = new Vector2(0.5f, 1);
        bodyRect.anchoredPosition = Vector2.zero;
        bodyRect.sizeDelta = new Vector2(0, bodyRect.sizeDelta.y);

        var oldVertical = body.GetComponent<VerticalLayoutGroup>();
        if (oldVertical != null) Object.DestroyImmediate(oldVertical);

        var grid = body.GetComponent<GridLayoutGroup>();
        if (grid == null) grid = body.gameObject.AddComponent<GridLayoutGroup>();
        grid.padding = new RectOffset(14, 14, 14, 14);
        grid.spacing = new Vector2(12, 12);
        grid.cellSize = new Vector2(290, 184);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 6;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.UpperLeft;

        var fitter = body.GetComponent<ContentSizeFitter>();
        if (fitter == null) fitter = body.gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        BuildNavigation(indicators.transform, controller);

        int total = 0;
        foreach (string category in Folders)
        {
            string folder = "Assets/Game/Units/Database/" + category;
            string[] guids = AssetDatabase.FindAssets("t:UnitDefinition", new[] { folder });
            System.Array.Sort(guids);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnitDefinition unit = AssetDatabase.LoadAssetAtPath<UnitDefinition>(path);
                if (unit == null || string.IsNullOrWhiteSpace(unit.UnitId))
                    continue;

                CreateCard(body, unit, unit.Category.ToString());
                total++;
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[UNIT STORE] Rebuilt clean catalogue: " + total + " units across six categories.");
    }

    private static void BuildNavigation(Transform parent, StoreCatalogueInteractionController controller)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(parent.GetChild(i).gameObject);

        var layout = parent.GetComponent<HorizontalLayoutGroup>();
        if (layout == null) layout = parent.gameObject.AddComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(8, 8, 4, 4);
        layout.spacing = 8;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        foreach (string category in Folders)
        {
            GameObject go = new GameObject(category, typeof(RectTransform), typeof(Image),
                typeof(Button), typeof(LayoutElement));
            go.transform.SetParent(parent, false);

            var image = go.GetComponent<Image>();
            image.color = new Color(0.025f, 0.12f, 0.17f, 1f);

            var element = go.GetComponent<LayoutElement>();
            element.preferredWidth = 170;
            element.preferredHeight = 46;

            var button = go.GetComponent<Button>();
            button.targetGraphic = image;
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            var categoryData = go.AddComponent<StoreCategoryButton>();
            categoryData.Category = FolderToCategory(category);
            go.AddComponent<StoreNeonButtonFeedback>();

            AddText(go.transform, category, 12, true, TextAnchor.MiddleCenter);
        }
    }

    private static string FolderToCategory(string folder)
    {
        switch (folder)
        {
            case "AIR UNIT": return "Air";
            case "GROUND UNIT": return "Ground";
            case "SEA UNIT": return "Sea";
            case "COMMAND UNIT": return "Command";
            case "EXPERIMENTAL": return "Experimental";
            case "SATELLITE": return "Satellite";
            default: return string.Empty;
        }
    }

    private static void CreateCard(Transform parent, UnitDefinition unit, string category)
    {
        GameObject card = new GameObject("UNIT_" + unit.UnitId,
            typeof(RectTransform), typeof(Image), typeof(Button),
            typeof(VerticalLayoutGroup), typeof(LayoutElement), typeof(StoreCatalogueCard), typeof(StoreNeonButtonFeedback));

        card.transform.SetParent(parent, false);

        var background = card.GetComponent<Image>();
        background.color = new Color(0.025f, 0.075f, 0.10f, 0.98f);

        var button = card.GetComponent<Button>();
        button.targetGraphic = background;
        button.navigation = new Navigation { mode = Navigation.Mode.None };

        var layout = card.GetComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(10, 10, 9, 9);
        layout.spacing = 5;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        var size = card.GetComponent<LayoutElement>();
        size.preferredWidth = 290;
        size.preferredHeight = 184;

        var data = card.GetComponent<StoreCatalogueCard>();
        data.UnitId = unit.UnitId;
        data.Category = category;

        GameObject artObject = new GameObject("UNIT_ARTWORK",
            typeof(RectTransform), typeof(Image), typeof(LayoutElement));
        artObject.transform.SetParent(card.transform, false);
        var art = artObject.GetComponent<Image>();
        art.sprite = unit.StoreImage;
        art.preserveAspect = true;
        art.raycastTarget = false;
        art.color = unit.StoreImage != null ? Color.white : new Color(0.06f, 0.16f, 0.20f);
        var artSize = artObject.GetComponent<LayoutElement>();
        artSize.preferredHeight = 105;
        artSize.minHeight = 105;

        AddText(card.transform, unit.DisplayName, 16, true, TextAnchor.MiddleLeft);
        AddText(card.transform,
            category + "  //  " + unit.Role + "  //  " + unit.Tier,
            10, false, TextAnchor.MiddleLeft);
        AddText(card.transform, "SELECT TO INSPECT", 10, true, TextAnchor.MiddleLeft);
    }

    private static void AddText(Transform parent, string value, int size, bool bold, TextAnchor alignment)
    {
        GameObject go = new GameObject("LABEL",
            typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));
        go.transform.SetParent(parent, false);

        var text = go.GetComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = size;
        text.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        text.color = new Color(0.60f, 0.89f, 0.96f);
        text.alignment = alignment == TextAnchor.MiddleCenter ? TextAlignmentOptions.Center : TextAlignmentOptions.Left;
        text.raycastTarget = false;
        text.textWrappingMode = TextWrappingModes.Normal;
        text.overflowMode = TextOverflowModes.Ellipsis;

        var element = go.GetComponent<LayoutElement>();
        element.minHeight = size + 7;
        element.preferredHeight = size + 9;
    }

    private static GameObject FindObject(string objectName)
    {
        foreach (GameObject candidate in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (candidate.name == objectName &&
                candidate.scene.IsValid() && candidate.scene.isLoaded)
                return candidate;
        }
        return null;
    }
}