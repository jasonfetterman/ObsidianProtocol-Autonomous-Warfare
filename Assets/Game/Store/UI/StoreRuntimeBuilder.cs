using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro;
using UnityEngine.EventSystems;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Store;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Store.UI
{
    public sealed class StoreRuntimeBuilder : MonoBehaviour
    {
        private const KeyCode ToggleKey = KeyCode.F8;

        private readonly UnitPurchaseService purchaseService = new UnitPurchaseService();
        private readonly List<UnitDefinition> units = new List<UnitDefinition>();

        private GameObject storeRoot;
        private TextMeshProUGUI creditsText;
        private Image selectedImage;
        private TextMeshProUGUI statusText;
        private TextMeshProUGUI selectedNameText;
        private TextMeshProUGUI selectedCategoryText;
        private TextMeshProUGUI selectedPriceText;
        private TextMeshProUGUI selectedDescriptionText;
        private Button purchaseButton;
        private Transform listContent;
        private UnitDefinition selectedUnit;
        private string selectedCategoryFilter = "ALL";
        private readonly List<GameObject> categoryTiles = new List<GameObject>();
        // Automatic runtime Store creation disabled.
        // SCN16_STORE uses the scene-based StoreMasterController.

        private void Awake()
        {
            BuildInterface();
            RefreshCatalog();
            SetStatus("STORE ONLINE  |  Press F8 to open or close");
            storeRoot.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(ToggleKey))
                storeRoot.SetActive(!storeRoot.activeSelf);

            if (creditsText != null)
                creditsText.text = EconomyRuntime.CampaignActive
                    ? "CAMPAIGN CREDITS   " + EconomyRuntime.Credits.ToString("N0")
                    : "CAMPAIGN CREDITS   â€”";

            if (purchaseButton != null)
            {
                purchaseButton.interactable =
                    selectedUnit != null &&
                    EconomyRuntime.CampaignActive &&
                    selectedUnit.Economy != null &&
                    selectedUnit.Economy.creditCost > 0;
            }
        }

        private void BuildInterface()
        {
            GameObject canvasObject = new GameObject(
                "NewStoreCanvas",
                typeof(RectTransform), typeof(Canvas),
                typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 300;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventObject = new GameObject(
                    "NewStoreEventSystem",
                    typeof(EventSystem), typeof(StandaloneInputModule));
                eventObject.transform.SetParent(transform, false);
            }

            Color bg = new Color(.008f,.022f,.038f,1f);
            Color panel = new Color(.015f,.045f,.072f,.98f);
            Color cyan = new Color(0f,.65f,1f,1f);
            Color white = new Color(.82f,.91f,1f,1f);
            Color muted = new Color(.35f,.53f,.68f,1f);
            Color green = new Color(.1f,1f,.42f,1f);

            storeRoot = Panel(canvasObject.transform, "StoreRoot", bg,
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            Panel(storeRoot.transform, "CommandHeader", panel,
                new Vector2(.025f,.835f), new Vector2(.975f,.98f),
                Vector2.zero, Vector2.zero);

            Panel(storeRoot.transform, "TopAccent", cyan,
                new Vector2(.025f,.83f), new Vector2(.975f,.835f),
                Vector2.zero, Vector2.zero);

            MakeText(storeRoot.transform, "Eyebrow",
                "OBSIDIAN PROTOCOL  //  QUARTERMASTER DIVISION",
                13, cyan, new Vector2(.045f,.925f), new Vector2(.65f,.97f),
                TextAnchor.MiddleLeft);

            MakeText(storeRoot.transform, "Title",
                "UNIT ACQUISITION",
                32, white, new Vector2(.045f,.86f), new Vector2(.65f,.93f),
                TextAnchor.MiddleLeft);

            creditsText = MakeText(storeRoot.transform, "Credits",
                "CAMPAIGN CREDITS  --",
                18, green, new Vector2(.68f,.89f), new Vector2(.86f,.95f),
                TextAnchor.MiddleRight);

            MakeButton(storeRoot.transform, "CloseStore", "CLOSE  [F8]",
                new Vector2(.875f,.89f), new Vector2(.955f,.95f),
                new Color(.025f,.12f,.18f,1f), () =>
                {
                    storeRoot.SetActive(false);
                });

            // Fleet-matched asset panel: same bounds and hierarchy.
            Panel(storeRoot.transform, "ASSET_PANEL", panel,
                new Vector2(.025f,.105f), new Vector2(.665f,.645f),
                Vector2.zero, Vector2.zero);

            MakeText(storeRoot.transform, "ASSET_TITLE",
                "ACQUISITION CATALOGUE",
                22, white, new Vector2(.04f,.59f), new Vector2(.42f,.635f),
                TextAnchor.MiddleLeft);

            MakeText(storeRoot.transform, "ASSET_COUNT",
                "AVAILABLE ASSETS  //  SELECT TO INSPECT",
                11, muted, new Vector2(.39f,.595f), new Vector2(.645f,.63f),
                TextAnchor.MiddleRight);

            GameObject viewport = new GameObject(
                "ASSET_VIEWPORT",
                typeof(RectTransform), typeof(Image), typeof(Mask));
            viewport.transform.SetParent(storeRoot.transform, false);

            RectTransform viewportRect = viewport.GetComponent<RectTransform>();
            viewportRect.anchorMin = new Vector2(.035f,.125f);
            viewportRect.anchorMax = new Vector2(.655f,.575f);
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            viewport.GetComponent<Image>().color = bg;
            viewport.GetComponent<Mask>().showMaskGraphic = false;

            GameObject content = new GameObject(
                "ASSET_LIST",
                typeof(RectTransform), typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter));
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0,1);
            contentRect.anchorMax = new Vector2(1,1);
            contentRect.pivot = new Vector2(.5f,1);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = Vector2.zero;

            VerticalLayoutGroup layout = content.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(8,8,8,8);
            layout.spacing = 8;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ScrollRect scroll = viewport.AddComponent<ScrollRect>();
            scroll.content = contentRect;
            scroll.viewport = viewportRect;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            listContent = content.transform;

            // Fleet-matched right-side inspection panel.
            Panel(storeRoot.transform, "UNIT_DOSSIER_PANEL", panel,
                new Vector2(.68f,.105f), new Vector2(.975f,.765f),
                Vector2.zero, Vector2.zero);

            MakeText(storeRoot.transform, "DossierHeading",
                "UNIT DOSSIER",
                20, white, new Vector2(.705f,.715f), new Vector2(.95f,.76f),
                TextAnchor.MiddleLeft);

            MakeText(storeRoot.transform, "DossierSubheading",
                "ASSET INTELLIGENCE  //  ACQUISITION REVIEW",
                10, muted, new Vector2(.705f,.685f), new Vector2(.95f,.715f),
                TextAnchor.MiddleLeft);

            Panel(storeRoot.transform, "ArtworkFrame",
                new Color(.005f,.025f,.045f,1f),
                new Vector2(.735f,.485f), new Vector2(.945f,.675f),
                Vector2.zero, Vector2.zero);

            GameObject artworkObject = new GameObject(
                "SelectedUnitArtwork",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            artworkObject.transform.SetParent(storeRoot.transform, false);

            RectTransform artworkRect = artworkObject.GetComponent<RectTransform>();
            artworkRect.anchorMin = new Vector2(.745f,.495f);
            artworkRect.anchorMax = new Vector2(.935f,.665f);
            artworkRect.offsetMin = Vector2.zero;
            artworkRect.offsetMax = Vector2.zero;

            selectedImage = artworkObject.GetComponent<Image>();
            selectedImage.preserveAspect = true;
            selectedImage.raycastTarget = false;

            Panel(storeRoot.transform, "DossierDivider", cyan,
                new Vector2(.705f,.465f), new Vector2(.95f,.468f),
                Vector2.zero, Vector2.zero);

            selectedNameText = MakeText(storeRoot.transform, "SelectedUnitName",
                "NO UNIT SELECTED",
                23, white, new Vector2(.705f,.405f), new Vector2(.95f,.455f),
                TextAnchor.MiddleLeft);

            selectedCategoryText = MakeText(storeRoot.transform, "SelectedCategory",
                "CATEGORY  --",
                12, cyan, new Vector2(.705f,.36f), new Vector2(.95f,.4f),
                TextAnchor.MiddleLeft);

            selectedPriceText = MakeText(storeRoot.transform, "SelectedPrice",
                "ACQUISITION COST  --",
                16, green, new Vector2(.705f,.305f), new Vector2(.95f,.35f),
                TextAnchor.MiddleLeft);

            selectedDescriptionText = MakeText(storeRoot.transform, "UnitDescription",
                "Select an available asset to review its operational details and acquisition information.",
                13, muted, new Vector2(.705f,.18f), new Vector2(.95f,.29f),
                TextAnchor.UpperLeft);

            purchaseButton = MakeButton(
                storeRoot.transform, "PurchaseUnitButton", "ACQUIRE UNIT",
                new Vector2(.705f,.12f), new Vector2(.95f,.17f),
                cyan, PurchaseSelectedUnit);

            Panel(storeRoot.transform, "StatusBar", panel,
                new Vector2(.025f,.045f), new Vector2(.975f,.09f),
                Vector2.zero, Vector2.zero);

            statusText = MakeText(storeRoot.transform, "StoreStatus",
                "ACQUISITION SYSTEM INITIALIZING",
                12, muted, new Vector2(.04f,.048f), new Vector2(.95f,.087f),
                TextAnchor.MiddleLeft);

            MakeText(storeRoot.transform, "Footer",
                "OPAW  //  LOGISTICS COMMAND  //  AUTHORIZED ACQUISITIONS",
                10, muted, new Vector2(.045f,.012f), new Vector2(.8f,.035f),
                TextAnchor.MiddleLeft);
        }
        private GameObject Panel(
            Transform parent, string name, Color color,
            Vector2 anchorMin, Vector2 anchorMax,
            Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject obj = new GameObject(name, typeof(RectTransform), typeof(Image));
            obj.transform.SetParent(parent, false);
            obj.transform.SetAsFirstSibling();

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Image image = obj.GetComponent<Image>();
            image.color = color;
            image.sprite = GetChamferSprite();
            image.type = Image.Type.Sliced;

            if (name != "TopAccent")
            {
                Outline outline = obj.AddComponent<Outline>();
                outline.effectColor = new Color(0f, 0.65f, 1f, 0.75f);
                outline.effectDistance = new Vector2(1.2f, 1.2f);
            }

            return obj;
        }
        private TextMeshProUGUI MakeText(
            Transform parent, string name, string value, int size,
            Color color, Vector2 anchorMin, Vector2 anchorMax, TextAnchor alignment)
        {
            GameObject obj = new GameObject(
                name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            obj.transform.SetParent(parent, false);
            obj.transform.SetAsFirstSibling();

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            text.text = value;
            text.fontSize = size;
            text.color = color;
            text.enableAutoSizing = true;
            text.fontSizeMin = Mathf.Max(10, size - 3);
            text.fontSizeMax = size;
            text.alignment = alignment == TextAnchor.MiddleCenter
                ? TextAlignmentOptions.Center
                : alignment == TextAnchor.MiddleRight
                    ? TextAlignmentOptions.MidlineRight
                    : alignment == TextAnchor.UpperLeft
                        ? TextAlignmentOptions.TopLeft
                        : TextAlignmentOptions.MidlineLeft;
            text.textWrappingMode = name == "UnitDescription"
                ? TextWrappingModes.Normal
                : TextWrappingModes.NoWrap;
            return text;
        }
        private Button MakeButton(
            Transform parent, string name, string label,
            Vector2 anchorMin, Vector2 anchorMax, Color color,
            UnityEngine.Events.UnityAction action)
        {
            GameObject obj = new GameObject(
                name, typeof(RectTransform), typeof(CanvasRenderer),
                typeof(Image), typeof(Button));

            obj.transform.SetParent(parent, false);
            obj.transform.SetAsFirstSibling();

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image image = obj.GetComponent<Image>();
            image.color = color;
            image.sprite = GetChamferSprite();
            image.type = Image.Type.Sliced;

            Outline outline = obj.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0.65f, 1f, 0.9f);
            outline.effectDistance = new Vector2(1.5f, 1.5f);

            Button button = obj.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(action);

            MakeText(obj.transform, "Label", label, 16,
                new Color(0.82f, 0.91f, 1f, 1f),
                Vector2.zero, Vector2.one, TextAnchor.MiddleCenter);

            return button;
        }
        private void RefreshCatalog()
        {
            units.Clear();

            if (!UnitDatabaseRuntime.EnsureInitialized())
            {
                SetStatus("ERROR: Unit database could not initialize.");
                return;
            }

            var database = UnitDatabaseRuntime.Database;
            if (database == null || database.Units == null)
            {
                SetStatus("ERROR: Unit database or unit collection is missing.");
                return;
            }

            foreach (UnitDefinition unit in database.Units)
            {
                if (unit == null ||
                    string.IsNullOrWhiteSpace(unit.UnitId) ||
                    string.IsNullOrWhiteSpace(unit.DisplayName) ||
                    unit.Economy == null ||
                    unit.Economy.creditCost <= 0)
                    continue;

                units.Add(unit);
            }

            BuildCategoryStrip();
            RebuildCatalog();
            SetStatus("CATALOG READY  /  " + units.Count + " purchasable units");
        }

        private void BuildCategoryStrip()
        {
            string[] names = { "AIR", "GROUND", "SEA", "SATELLITE", "COMMAND", "EXPERIMENTAL" };
            float gap = .008f;
            float width = (.65f - gap * 5f) / 6f;

            foreach (GameObject tile in categoryTiles)
                if (tile != null) Destroy(tile);
            categoryTiles.Clear();

            for (int i = 0; i < names.Length; i++)
            {
                string category = names[i];
                int count = 0;
                foreach (UnitDefinition unit in units)
                    if (unit.Category.ToString().ToUpperInvariant() == category) count++;

                float x = .025f + i * (width + gap);
                GameObject tile = Panel(storeRoot.transform, "CATEGORY_" + category,
                    new Color(.015f,.045f,.072f,.98f),
                    new Vector2(x,.665f), new Vector2(x+width,.765f),
                    Vector2.zero, Vector2.zero);
                categoryTiles.Add(tile);

                Button button = tile.AddComponent<Button>();
                button.transition = Selectable.Transition.None;
                button.onClick.AddListener(() => SetCategoryFilter(category));

                MakeText(tile.transform, "CATEGORY_NAME", category + " UNITS",
                    12, new Color(0f,.72f,1f,1f),
                    new Vector2(.03f,.58f), new Vector2(.97f,.91f),
                    TextAnchor.MiddleCenter);
                MakeText(tile.transform, "CATEGORY_COUNT", count.ToString("00"),
                    25, new Color(.82f,.91f,1f,1f),
                    new Vector2(.03f,.08f), new Vector2(.97f,.62f),
                    TextAnchor.MiddleCenter);
            }
        }

        private void SetCategoryFilter(string category)
        {
            selectedCategoryFilter = category;
            RebuildCatalog();
            SetStatus("CATALOG FILTER  /  " + category + " UNITS");
        }

        private void RebuildCatalog()
        {
            if (listContent == null) return;
            for (int i = listContent.childCount - 1; i >= 0; i--)
                Destroy(listContent.GetChild(i).gameObject);

            int shown = 0;
            foreach (UnitDefinition unit in units)
            {
                if (selectedCategoryFilter != "ALL" &&
                    unit.Category.ToString().ToUpperInvariant() != selectedCategoryFilter)
                    continue;
                CreateUnitCard(unit);
                shown++;
            }

            if (shown == 0)
                MakeText(listContent, "EMPTY_CATALOG", "NO AVAILABLE UNITS IN THIS CATEGORY",
                    18, new Color(.35f,.53f,.68f,1f),
                    new Vector2(.05f,.4f), new Vector2(.95f,.6f),
                    TextAnchor.MiddleCenter);
        }

        private void CreateUnitCard(UnitDefinition unit)
        {
            GameObject card = new GameObject(
                "ASSET_ROW_" + unit.UnitId,
                typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));
            card.transform.SetParent(listContent, false);

            LayoutElement element = card.GetComponent<LayoutElement>();
            element.preferredHeight = 112;
            element.minHeight = 112;

            Image image = card.GetComponent<Image>();
            image.color = new Color(.025f,.075f,.11f,.98f);
            image.sprite = GetChamferSprite();
            image.type = Image.Type.Sliced;

            Outline outline = card.AddComponent<Outline>();
            outline.effectColor = new Color(0f,.72f,1f,1f);
            outline.effectDistance = new Vector2(2.5f,2.5f);
            outline.useGraphicAlpha = true;

            Button button = card.GetComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(() => SelectUnit(unit));

            GameObject accent = new GameObject("ACCENT",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            accent.transform.SetParent(card.transform, false);
            RectTransform accentRect = accent.GetComponent<RectTransform>();
            accentRect.anchorMin = new Vector2(0,0);
            accentRect.anchorMax = new Vector2(.008f,1);
            accentRect.offsetMin = Vector2.zero;
            accentRect.offsetMax = Vector2.zero;
            accent.GetComponent<Image>().color = new Color(0f,.65f,1f,1f);
            accent.GetComponent<Image>().raycastTarget = false;

            GameObject artObject = new GameObject("UNIT_ART",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            artObject.transform.SetParent(card.transform, false);
            RectTransform artRect = artObject.GetComponent<RectTransform>();
            artRect.anchorMin = new Vector2(.015f,.08f);
            artRect.anchorMax = new Vector2(.22f,.92f);
            artRect.offsetMin = Vector2.zero;
            artRect.offsetMax = Vector2.zero;

            Image art = artObject.GetComponent<Image>();
            art.sprite = unit.StoreImage;
            art.preserveAspect = true;
            art.color = Color.white;
            art.raycastTarget = false;

            string category = unit.Category.ToString().ToUpperInvariant();
            MakeText(card.transform, "UNIT_EMBLEM", CategoryEmblem(category),
                22, new Color(0f,.72f,1f,1f),
                new Vector2(.18f,.68f), new Vector2(.235f,.96f),
                TextAnchor.MiddleCenter);
            MakeText(card.transform, "UNIT_NAME", unit.DisplayName.ToUpperInvariant(),
                19, new Color(.82f,.91f,1f,1f),
                new Vector2(.245f,.48f), new Vector2(.66f,.88f),
                TextAnchor.MiddleLeft);
            MakeText(card.transform, "UNIT_CLASS",
                category + "  //  " + unit.UnitId,
                12, new Color(.35f,.53f,.68f,1f),
                new Vector2(.245f,.12f), new Vector2(.66f,.5f),
                TextAnchor.MiddleLeft);
            MakeText(card.transform, "ACQUISITION_PRICE",
                unit.Economy.creditCost.ToString("N0") + " CREDITS",
                14, new Color(.1f,1f,.42f,1f),
                new Vector2(.68f,.25f), new Vector2(.96f,.72f),
                TextAnchor.MiddleRight);
            MakeText(card.transform, "SALE_LABEL", "AVAILABLE",
                10, new Color(.35f,.53f,.68f,1f),
                new Vector2(.68f,.08f), new Vector2(.96f,.3f),
                TextAnchor.MiddleRight);
        }

        private static string CategoryEmblem(string category)
        {
            switch (category)
            {
                case "AIR": return "\u2708";
                case "GROUND": return "\u2694";
                case "SEA": return "\u2693";
                case "SATELLITE": return "\u25c9";
                case "COMMAND": return "\u2b21";
                case "EXPERIMENTAL": return "\u2725";
                default: return "\u25c7";
            }
        }
        private void SelectUnit(UnitDefinition unit)
        {
            selectedUnit = unit;
            selectedImage.sprite = unit.StoreImage;
            selectedImage.color = unit.StoreImage != null ? Color.white : new Color(0.01f,0.035f,0.055f,1f);



            selectedNameText.text = unit.DisplayName.ToUpperInvariant();
            selectedCategoryText.text =
                "CATEGORY  " + unit.Category + "   /   ROLE  " + unit.Role;
            selectedPriceText.text =
                "ACQUISITION COST  " + unit.Economy.creditCost.ToString("N0") + " CREDITS";

            selectedDescriptionText.text =
                "UNIT ID: " + unit.UnitId +
                "\nTIER: " + unit.Tier +
                "\nSTATUS: " + unit.Status +
                "\n\nPurchase adds this unit to campaign inventory and registers it with fleet management.";

            SetStatus("SELECTED  /  " + unit.DisplayName);
        }

        private void PurchaseSelectedUnit()
        {
            if (selectedUnit == null)
            {
                SetStatus("Select a unit before acquiring.");
                return;
            }

            if (!EconomyRuntime.CampaignActive)
            {
                SetStatus("PURCHASE BLOCKED  /  Start or load a campaign first.");
                return;
            }

            bool success = purchaseService.Purchase(selectedUnit.UnitId);

            if (success)
            {
                SetStatus("ACQUISITION COMPLETE  /  " + selectedUnit.DisplayName);
            }
            else
            {
                SetStatus("ACQUISITION FAILED  /  Check credits, ownership, inventory, and fleet registration.");
            }
        }

        private static Sprite chamferSprite;

        private static Sprite GetChamferSprite()
        {
            if (chamferSprite != null) return chamferSprite;
            const int size = 64;
            Texture2D texture = new Texture2D(size,size,TextureFormat.RGBA32,false);
            texture.filterMode = FilterMode.Bilinear;
            Color32 clear = new Color32(255,255,255,0);
            Color32 solid = new Color32(255,255,255,255);
            for (int y=0;y<size;y++)
            for (int x=0;x<size;x++)
            {
                bool inside = x+y>=13 && (size-1-x)+y>=13 &&
                    x+(size-1-y)>=13 && (size-1-x)+(size-1-y)>=13;
                texture.SetPixel(x,y,inside ? solid : clear);
            }
            texture.Apply();
            chamferSprite = Sprite.Create(texture,new Rect(0,0,size,size),
                new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,new Vector4(14,14,14,14));
            chamferSprite.name = "OPAW_STORE_CHAMFER";
            return chamferSprite;
        }
        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }
    }
}


