using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Garage;
using ObsidianProtocol.Game.Core;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Units;

[DisallowMultipleComponent]
public sealed class FleetPanelManager : MonoBehaviour
{
    static readonly Color BG = new Color(.008f,.022f,.038f,1);
    static readonly Color PANEL = new Color(.015f,.045f,.072f,.98f);
    static readonly Color CYAN = new Color(.0f,.65f,1f,1);
    static readonly Color WHITE = new Color(.82f,.91f,1f,1);
    static readonly Color MUTED = new Color(.35f,.53f,.68f,1);
    static readonly Color GREEN = new Color(.1f,1f,.42f,1);

    readonly List<FleetUnit> units = new List<FleetUnit>();
    readonly List<UnitDefinition> definitions = new List<UnitDefinition>();
    Transform root;
    Transform listContent;
    TextMeshProUGUI totalText, operationalText, deployedText, maintenanceText, unavailableText;
    TextMeshProUGUI heroName, heroClass, heroStatus;
    Image heroImage;
    int selectedIndex;

    void OnEnable() { Build(); }

    public void RefreshList() { Build(); }

    void Build()
    {
        LoadFleet();

        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        root = transform;
        RectTransform panel = GetComponent<RectTransform>();
        if (panel != null)
        {
            panel.anchorMin = Vector2.zero;
            panel.anchorMax = Vector2.one;
            panel.offsetMin = Vector2.zero;
            panel.offsetMax = Vector2.zero;
        }

        Image background = GetComponent<Image>();
        if (background != null) background.color = BG;

        CreateText("BRAND", "OBSIDIAN PROTOCOL", 18, CYAN, .02f,.94f,.22f,.99f, FontStyles.Bold);
        CreateText("BRAND_SUB", "AUTONOMOUS WARFARE", 10, MUTED, .02f,.91f,.22f,.95f);
        CreateText("FLEET_TITLE", "FLEET OPERATIONS", 38, WHITE, .025f,.82f,.56f,.92f, FontStyles.Bold);
        CreateText("FLEET_SUBTITLE", "DEPLOY  /  MAINTAIN  /  UPGRADE", 16, CYAN, .03f,.78f,.55f,.83f);

        totalText = CreateText("TOTAL", "0", 32, WHITE, .67f,.85f,.76f,.94f, FontStyles.Bold);
        CreateText("TOTAL_LABEL", "REGISTERED UNITS", 12, MUTED, .67f,.81f,.82f,.86f);
        operationalText = CreateText("OPERATIONAL", "0", 32, GREEN, .82f,.85f,.9f,.94f, FontStyles.Bold);
        CreateText("OPERATIONAL_LABEL", "OPERATIONAL", 12, MUTED, .82f,.81f,.96f,.86f);

        BuildCategoryStrip();
        BuildAssetPanel();
        BuildOverviewPanel();
        BuildFooter();
        UpdateDashboard();
    }

    void LoadFleet()
    {
        units.Clear();
        definitions.Clear();

        var runtime = ObsidianRuntimeSystems.Instance;
        if (runtime == null || !runtime.Initialized || runtime.FleetManagement == null)
            return;

        foreach (FleetUnit unit in runtime.FleetManagement.GetFleet())
        {
            if (unit == null || !unit.Valid) continue;
            units.Add(unit);
            definitions.Add(UnitDatabaseRuntime.Get(unit.UnitId));
        }
    }

    void BuildCategoryStrip()
    {
        string[] names = { "AIR", "GROUND", "SEA", "SATELLITE", "COMMAND", "EXPERIMENTAL" };
        float gap = .008f;
        float width = (.65f - gap * 5f) / 6f;

        for (int i = 0; i < names.Length; i++)
        {
            int count = 0;
            foreach (var d in definitions)
                if (d != null && d.Category.ToString().ToUpperInvariant() == names[i]) count++;

            float x = .025f + i * (width + gap);
            GameObject categoryBar = Box("CATEGORY_" + names[i], PANEL, x,.665f,x+width,.765f);
            Outline categoryOutline = categoryBar.AddComponent<Outline>();
            categoryOutline.effectColor = new Color(0f,.72f,1f,1f);
            categoryOutline.effectDistance = new Vector2(1.2f,1.2f);
            TextMeshProUGUI categoryName = CreateText("CAT_NAME_" + i, names[i] + " UNITS", 13, CYAN, x+.012f,.715f,x+width-.01f,.75f);
            categoryName.alignment = TextAlignmentOptions.Center;
            TextMeshProUGUI categoryCount = CreateText("CAT_COUNT_" + i, count.ToString("00"), 27, WHITE, x+.012f,.675f,x+width-.01f,.72f, FontStyles.Bold);
            categoryCount.alignment = TextAlignmentOptions.Center;
        }
    }

    void BuildAssetPanel()
    {
        Box("ASSET_PANEL", PANEL, .025f,.105f,.665f,.645f);
        CreateText("ASSET_TITLE", "FLEET ASSETS", 22, WHITE, .04f,.59f,.42f,.635f, FontStyles.Bold);
        CreateText("ASSET_COUNT", units.Count + " REGISTERED ASSETS", 12, MUTED, .43f,.595f,.64f,.63f);

        GameObject viewport = Box("ASSET_VIEWPORT", BG, .035f,.125f,.655f,.575f);
        viewport.AddComponent<Mask>().showMaskGraphic = false;

        GameObject content = new GameObject("ASSET_LIST", typeof(RectTransform));
        content.transform.SetParent(viewport.transform, false);
        RectTransform cr = content.GetComponent<RectTransform>();
        cr.anchorMin = new Vector2(0,1);
        cr.anchorMax = new Vector2(1,1);
        cr.pivot = new Vector2(.5f,1);
        cr.anchoredPosition = Vector2.zero;
        cr.sizeDelta = new Vector2(0, units.Count * 120f + 12f);
        listContent = content.transform;

        ScrollRect scroll = viewport.AddComponent<ScrollRect>();
        scroll.content = cr;
        scroll.viewport = viewport.GetComponent<RectTransform>();
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;

        for (int i = 0; i < units.Count; i++)
            BuildAssetRow(i);

        if (units.Count == 0)
            CreateText("EMPTY_FLEET", "NO REGISTERED ASSETS", 22, MUTED, .12f,.36f,.55f,.45f, FontStyles.Bold);
    }

    void BuildAssetRow(int index)
    {
        FleetUnit unit = units[index];
        UnitDefinition def = definitions[index];
        float top = -8f - index * 120f;

        GameObject row = Box("ASSET_ROW_" + index, new Color(.025f,.075f,.11f,.98f), 0,0,1,1, listContent);
        Outline neonOutline = row.AddComponent<Outline>();
        neonOutline.effectColor = new Color(0f,.72f,1f,1f);
        neonOutline.effectDistance = new Vector2(2.5f,2.5f);
        neonOutline.useGraphicAlpha = true;
        RectTransform r = row.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(0,1);
        r.anchorMax = new Vector2(1,1);
        r.pivot = new Vector2(.5f,1);
        r.anchoredPosition = new Vector2(0,top);
        r.sizeDelta = new Vector2(-12,112);

        Image edge = Box("ACCENT", CYAN, 0,0,.008f,1,row.transform).GetComponent<Image>();
        edge.raycastTarget = false;

        Image artwork = Box("UNIT_ART", new Color(.01f,.035f,.06f,1), .015f,.08f,.22f,.92f,row.transform).GetComponent<Image>();
        artwork.type = Image.Type.Simple;
        artwork.preserveAspect = true;
        Sprite sprite = GetStoreImage(def);
        if (sprite != null) { artwork.sprite = sprite; artwork.preserveAspect = true; artwork.color = Color.white; }


        string name = def != null && !string.IsNullOrWhiteSpace(def.DisplayName) ? def.DisplayName : unit.UnitId;
        string category = def != null ? def.Category.ToString().ToUpperInvariant() : "UNIT";
        CreateText("UNIT_EMBLEM", CategoryEmblem(category), 22, CYAN, .18f,.68f,.235f,.96f, FontStyles.Bold, row.transform);
        CreateText("UNIT_NAME", name.ToUpperInvariant(), 19, WHITE, .245f,.48f,.64f,.88f, FontStyles.Bold, row.transform);
        CreateText("UNIT_CLASS", category + "  //  " + unit.UnitId, 12, MUTED, .245f,.12f,.64f,.5f, FontStyles.Normal, row.transform);

        Color statusColor = StatusColor(unit.Status);
        CreateText("STATUS", FormatStatus(unit.Status), 14, statusColor, .68f,.25f,.88f,.72f, FontStyles.Bold, row.transform);
        CreateText("RECORD", "OWNERSHIP VERIFIED", 10, MUTED, .68f,.08f,.9f,.3f, FontStyles.Normal, row.transform);

        Button button = row.AddComponent<Button>();
        button.transition = Selectable.Transition.None;
        button.onClick.AddListener(() => SelectUnit(index));
    }

    void BuildOverviewPanel()
    {
        Box("OVERVIEW_PANEL", PANEL, .68f,.105f,.975f,.765f);
        CreateText("OVERVIEW_TITLE", "FLEET OVERVIEW", 22, WHITE, .70f,.715f,.95f,.755f, FontStyles.Bold);

        heroImage = Box("HERO_ART", new Color(.005f,.025f,.045f,1), .695f,.47f,.96f,.70f).GetComponent<Image>();
        heroImage.type = Image.Type.Simple;
        heroImage.preserveAspect = true;

        heroName = CreateText("HERO_NAME", "NO ASSET SELECTED", 21, WHITE, .70f,.405f,.95f,.46f, FontStyles.Bold);
        heroClass = CreateText("HERO_CLASS", "", 13, CYAN, .70f,.37f,.95f,.415f);
        heroStatus = CreateText("HERO_STATUS", "", 14, GREEN, .70f,.325f,.95f,.375f, FontStyles.Bold);

        CreateText("READINESS_LABEL", "FLEET READINESS", 14, MUTED, .70f,.265f,.92f,.30f);
        operationalText = CreateText("READINESS_VALUE", "0%", 25, GREEN, .70f,.215f,.85f,.27f, FontStyles.Bold);
        CreateText("DEPLOYED_LABEL", "DEPLOYED", 12, MUTED, .70f,.16f,.82f,.20f);
        deployedText = CreateText("DEPLOYED_VALUE", "0", 17, WHITE, .87f,.16f,.95f,.20f, FontStyles.Bold);
        CreateText("MAINTENANCE_LABEL", "IN MAINTENANCE", 12, MUTED, .70f,.12f,.85f,.16f);
        maintenanceText = CreateText("MAINTENANCE_VALUE", "0", 17, WHITE, .87f,.12f,.95f,.16f, FontStyles.Bold);
    }

    void BuildFooter()
    {
        Box("FOOTER", new Color(.005f,.025f,.045f,1), .015f,.025f,.985f,.085f);
        CreateText("SYSTEM_STATUS", "●  FLEET SYSTEMS: ONLINE", 15, GREEN, .03f,.035f,.30f,.075f, FontStyles.Bold);
        CreateText("FOOTER_BRAND", "OBSIDIAN PROTOCOL  //  COMMAND AUTHORITY", 12, MUTED, .62f,.035f,.96f,.075f);
        Button close = CreateButton("CLOSE_FLEET", "RETURN TO COMMAND", .79f,.025f,.97f,.085f);
        close.onClick.AddListener(() => gameObject.SetActive(false));
    }

    void UpdateDashboard()
    {
        int operational = 0, deployed = 0, maintenance = 0, unavailable = 0;
        foreach (var u in units)
        {
            if (u.Status == FleetUnitStatus.Operational) operational++;
            if (u.Status == FleetUnitStatus.Deployed) deployed++;
            if (u.Status == FleetUnitStatus.Repairing || u.Status == FleetUnitStatus.Upgrading || u.Status == FleetUnitStatus.Fabricating) maintenance++;
            if (u.Status == FleetUnitStatus.Disabled || u.Status == FleetUnitStatus.Salvaging) unavailable++;
            if (u.Status == FleetUnitStatus.Disabled || u.Status == FleetUnitStatus.Salvaging) unavailable++;
        }

        if (totalText != null) totalText.text = units.Count.ToString("00");
        if (operationalText != null) operationalText.text = operational.ToString("00");
        if (deployedText != null) deployedText.text = deployed.ToString("00");
        if (maintenanceText != null) maintenanceText.text = maintenance.ToString("00");
        if (unavailableText != null) unavailableText.text = unavailable.ToString("00");
        if (unavailableText != null) unavailableText.text = unavailable.ToString("00");

        if (units.Count > 0) SelectUnit(Mathf.Clamp(selectedIndex,0,units.Count-1));
        else
        {
            heroName.text = "NO REGISTERED ASSETS";
            heroClass.text = "PURCHASED UNITS WILL APPEAR HERE";
            heroStatus.text = "FLEET EMPTY";
            heroImage.sprite = null;
            heroImage.color = new Color(.005f,.025f,.045f,1f);
        }
    }

    void SelectUnit(int index)
    {
        if (index < 0 || index >= units.Count) return;
        selectedIndex = index;
        var unit = units[index];
        var def = definitions[index];
        heroName.text = def != null && !string.IsNullOrWhiteSpace(def.DisplayName) ? def.DisplayName.ToUpperInvariant() : unit.UnitId;
        heroClass.text = (def != null ? def.Category.ToString().ToUpperInvariant() : "UNIT") + "  //  " + unit.UnitId;
        heroStatus.text = FormatStatus(unit.Status);
        heroStatus.color = StatusColor(unit.Status);
        heroImage.sprite = GetStoreImage(def);
    }

    static Sprite GetStoreImage(UnitDefinition def)
    {
        return def != null ? def.StoreImage : null;
    }

    static Sprite chamferSprite;

    static Sprite GetChamferSprite()
    {
        if (chamferSprite != null) return chamferSprite;
        const int size = 64;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        Color32 clear = new Color32(255,255,255,0);
        Color32 solid = new Color32(255,255,255,255);

        for (int y=0;y<size;y++)
        for (int x=0;x<size;x++)
        {
            bool inside = x+y >= 13 &&
                (size-1-x)+y >= 13 &&
                x+(size-1-y) >= 13 &&
                (size-1-x)+(size-1-y) >= 13;
            texture.SetPixel(x,y,inside ? solid : clear);
        }

        texture.Apply();
        chamferSprite = Sprite.Create(texture,new Rect(0,0,size,size),
            new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,
            new Vector4(14,14,14,14));
        chamferSprite.name = "OPAW_CHAMFERED_UI_SPRITE";
        return chamferSprite;
    }

    static string CategoryEmblem(string category)
    {
        switch (category)
        {
            case "AIR": return "✈";
            case "GROUND": return "⚔";
            case "SEA": return "⚓";
            case "SATELLITE": return "◉";
            case "COMMAND": return "⬡";
            case "EXPERIMENTAL": return "✥";
            default: return "◇";
        }
    }
    GameObject Box(string name, Color color, float x0,float y0,float x1,float y1, Transform parent = null)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent != null ? parent : root, false);
        RectTransform r = go.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(x0,y0); r.anchorMax = new Vector2(x1,y1);
        r.offsetMin = Vector2.zero; r.offsetMax = Vector2.zero;
        Image image = go.GetComponent<Image>(); image.color = color; image.sprite = GetChamferSprite(); image.type = Image.Type.Sliced;
        return go;
    }

    TextMeshProUGUI CreateText(string name,string value,int size,Color color,float x0,float y0,float x1,float y1,FontStyles style = FontStyles.Normal,Transform parent = null)
    {
        GameObject go = new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(TextMeshProUGUI));
        go.transform.SetParent(parent != null ? parent : root,false);
        RectTransform r = go.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(x0,y0); r.anchorMax = new Vector2(x1,y1);
        r.offsetMin = Vector2.zero; r.offsetMax = Vector2.zero;
        var t = go.GetComponent<TextMeshProUGUI>();
        t.text = value; t.fontSize = size; t.color = color; t.fontStyle = style;
        t.enableAutoSizing = true; t.fontSizeMin = Mathf.Max(10,size-3); t.fontSizeMax = size;
        t.alignment = TextAlignmentOptions.MidlineLeft;
        t.textWrappingMode = TextWrappingModes.NoWrap;
        return t;
    }

    Button CreateButton(string name,string label,float x0,float y0,float x1,float y1)
    {
        GameObject go = Box(name,CYAN,x0,y0,x1,y1);
        Button b = go.AddComponent<Button>();
        CreateText(name+"_TEXT",label,14,BG,.04f,.05f,.96f,.95f,FontStyles.Bold,go.transform);
        return b;
    }

    static Color StatusColor(FleetUnitStatus status)
    {
        if (status == FleetUnitStatus.Operational) return GREEN;
        if (status == FleetUnitStatus.Deployed) return CYAN;
        if (status == FleetUnitStatus.Disabled || status == FleetUnitStatus.Salvaging) return new Color(1f,.25f,.2f,1);
        return new Color(1f,.68f,.15f,1);
    }

    static string FormatStatus(FleetUnitStatus status)
    {
        return status.ToString().Replace("_"," ").ToUpperInvariant();
    }
}