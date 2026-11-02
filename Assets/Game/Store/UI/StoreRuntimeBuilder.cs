using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        private Text creditsText;
        private Text statusText;
        private Text selectedNameText;
        private Text selectedCategoryText;
        private Text selectedPriceText;
        private Text selectedDescriptionText;
        private Button purchaseButton;
        private Transform listContent;
        private UnitDefinition selectedUnit;
        private Font font;
        // Automatic runtime Store creation disabled.
        // SCN16_STORE uses the scene-based StoreMasterController.

        private void Awake()
        {
            font = UnityEngine.Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
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
                    : "CAMPAIGN CREDITS   —";

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
            var canvasObject = new GameObject(
                "NewStoreCanvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));

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
                var eventObject = new GameObject(
                    "NewStoreEventSystem",
                    typeof(EventSystem),
                    typeof(StandaloneInputModule));
                eventObject.transform.SetParent(transform, false);
            }

            storeRoot = Panel(
                canvasObject.transform, "StoreRoot",
                new Color(0.025f, 0.04f, 0.055f, 0.97f),
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            Panel(
                storeRoot.transform, "TopAccent",
                new Color(0.83f, 0.48f, 0.13f, 1f),
                new Vector2(0, 0.925f), new Vector2(1, 0.932f),
                Vector2.zero, Vector2.zero);

            MakeText(storeRoot.transform, "Eyebrow",
                "OBSIDIAN PROTOCOL  /  QUARTERMASTER",
                15, new Color(0.87f, 0.57f, 0.24f, 1f),
                new Vector2(0.045f, 0.94f), new Vector2(0.7f, 0.99f),
                TextAnchor.MiddleLeft);

            MakeText(storeRoot.transform, "Title",
                "UNIT ACQUISITION",
                34, Color.white,
                new Vector2(0.045f, 0.86f), new Vector2(0.55f, 0.94f),
                TextAnchor.MiddleLeft);

            creditsText = MakeText(storeRoot.transform, "Credits",
                "CAMPAIGN CREDITS   —",
                20, new Color(0.55f, 0.88f, 0.67f, 1f),
                new Vector2(0.65f, 0.89f), new Vector2(0.94f, 0.95f),
                TextAnchor.MiddleRight);

            Button closeButton = MakeButton(
                storeRoot.transform, "CloseStore", "CLOSE  [F8]",
                new Vector2(0.82f, 0.935f), new Vector2(0.96f, 0.985f),
                new Color(0.2f, 0.23f, 0.25f, 1f), () =>
                {
                    storeRoot.SetActive(false);
                });

            MakeText(storeRoot.transform, "CatalogHeading",
                "AVAILABLE UNITS",
                19, new Color(0.83f, 0.87f, 0.89f, 1f),
                new Vector2(0.045f, 0.79f), new Vector2(0.45f, 0.84f),
                TextAnchor.MiddleLeft);

            MakeText(storeRoot.transform, "CatalogHint",
                "SELECT A UNIT TO REVIEW ITS ACQUISITION COST",
                12, new Color(0.52f, 0.59f, 0.63f, 1f),
                new Vector2(0.045f, 0.755f), new Vector2(0.5f, 0.79f),
                TextAnchor.MiddleLeft);

            Panel(
                storeRoot.transform, "CatalogFrame",
                new Color(0.07f, 0.095f, 0.11f, 1f),
                new Vector2(0.04f, 0.18f), new Vector2(0.49f, 0.75f),
                Vector2.zero, Vector2.zero);

            GameObject scrollObject = new GameObject(
                "UnitCatalogScroll",
                typeof(RectTransform), typeof(Image), typeof(ScrollRect));

            scrollObject.transform.SetParent(storeRoot.transform, false);
            RectTransform scrollRect = scrollObject.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0.045f, 0.185f);
            scrollRect.anchorMax = new Vector2(0.485f, 0.745f);
            scrollRect.offsetMin = new Vector2(6, 6);
            scrollRect.offsetMax = new Vector2(-6, -6);

            Image scrollBackground = scrollObject.GetComponent<Image>();
            scrollBackground.color = new Color(0.045f, 0.065f, 0.078f, 1f);

            GameObject viewport = new GameObject(
                "Viewport",
                typeof(RectTransform), typeof(Image), typeof(Mask));

            viewport.transform.SetParent(scrollObject.transform, false);
            RectTransform viewportRect = viewport.GetComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            viewport.GetComponent<Image>().color = Color.white;
            viewport.GetComponent<Mask>().showMaskGraphic = false;

            GameObject content = new GameObject(
                "UnitListContent",
                typeof(RectTransform), typeof(VerticalLayoutGroup),
                typeof(ContentSizeFitter));

            content.transform.SetParent(viewport.transform, false);
            RectTransform contentRect = content.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0, 0);

            VerticalLayoutGroup layout = content.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 8, 8);
            layout.spacing = 6;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            ContentSizeFitter fitter = content.GetComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ScrollRect scroll = scrollObject.GetComponent<ScrollRect>();
            scroll.viewport = viewportRect;
            scroll.content = contentRect;
            scroll.horizontal = false;
            scroll.vertical = true;
            listContent = content.transform;

            Panel(
                storeRoot.transform, "DetailsFrame",
                new Color(0.07f, 0.095f, 0.11f, 1f),
                new Vector2(0.515f, 0.18f), new Vector2(0.96f, 0.75f),
                Vector2.zero, Vector2.zero);

            MakeText(storeRoot.transform, "DetailsHeading",
                "UNIT DOSSIER",
                19, new Color(0.83f, 0.87f, 0.89f, 1f),
                new Vector2(0.54f, 0.68f), new Vector2(0.92f, 0.73f),
                TextAnchor.MiddleLeft);

            selectedNameText = MakeText(storeRoot.transform, "SelectedUnitName",
                "NO UNIT SELECTED",
                28, Color.white,
                new Vector2(0.54f, 0.59f), new Vector2(0.92f, 0.67f),
                TextAnchor.MiddleLeft);

            selectedCategoryText = MakeText(storeRoot.transform, "SelectedCategory",
                "CATEGORY  —",
                15, new Color(0.82f, 0.57f, 0.27f, 1f),
                new Vector2(0.54f, 0.54f), new Vector2(0.92f, 0.59f),
                TextAnchor.MiddleLeft);

            selectedPriceText = MakeText(storeRoot.transform, "SelectedPrice",
                "ACQUISITION COST  —",
                20, new Color(0.55f, 0.88f, 0.67f, 1f),
                new Vector2(0.54f, 0.47f), new Vector2(0.92f, 0.53f),
                TextAnchor.MiddleLeft);

            selectedDescriptionText = MakeText(storeRoot.transform, "UnitDescription",
                "Select an available unit from the catalog to review its details.",
                15, new Color(0.68f, 0.73f, 0.76f, 1f),
                new Vector2(0.54f, 0.32f), new Vector2(0.92f, 0.46f),
                TextAnchor.UpperLeft);

            purchaseButton = MakeButton(
                storeRoot.transform, "PurchaseUnitButton", "ACQUIRE UNIT",
                new Vector2(0.54f, 0.205f), new Vector2(0.92f, 0.285f),
                new Color(0.55f, 0.31f, 0.1f, 1f), PurchaseSelectedUnit);

            statusText = MakeText(storeRoot.transform, "StoreStatus",
                "STORE INITIALIZING",
                14, new Color(0.65f, 0.72f, 0.75f, 1f),
                new Vector2(0.045f, 0.09f), new Vector2(0.92f, 0.15f),
                TextAnchor.MiddleLeft);

            MakeText(storeRoot.transform, "Footer",
                "OPAW  /  LOGISTICS COMMAND  /  ACQUISITION SYSTEM",
                11, new Color(0.37f, 0.44f, 0.48f, 1f),
                new Vector2(0.045f, 0.025f), new Vector2(0.8f, 0.07f),
                TextAnchor.MiddleLeft);
        }

        private GameObject Panel(
            Transform parent, string name, Color color,
            Vector2 anchorMin, Vector2 anchorMax,
            Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject obj = new GameObject(name, typeof(RectTransform), typeof(Image));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            obj.GetComponent<Image>().color = color;
            return obj;
        }

        private Text MakeText(
            Transform parent, string name, string value, int size,
            Color color, Vector2 anchorMin, Vector2 anchorMax, TextAnchor alignment)
        {
            GameObject obj = new GameObject(
                name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));

            obj.transform.SetParent(parent, false);

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Text text = obj.GetComponent<Text>();
            text.font = font;
            text.text = value;
            text.fontSize = size;
            text.color = color;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
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

            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image image = obj.GetComponent<Image>();
            image.color = color;

            Button button = obj.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(action);

            MakeText(obj.transform, "Label", label, 16, Color.white,
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
                CreateUnitCard(unit);
            }

            SetStatus("CATALOG READY  /  " + units.Count + " purchasable units");
        }

        private void CreateUnitCard(UnitDefinition unit)
        {
            GameObject card = new GameObject(
                "UnitCard_" + unit.UnitId,
                typeof(RectTransform), typeof(Image), typeof(Button), typeof(LayoutElement));

            card.transform.SetParent(listContent, false);
            card.GetComponent<LayoutElement>().preferredHeight = 72;
            card.GetComponent<LayoutElement>().minHeight = 72;

            Image image = card.GetComponent<Image>();
            image.color = new Color(0.11f, 0.145f, 0.16f, 1f);

            Button button = card.GetComponent<Button>();
            button.targetGraphic = image;

            string displayName = unit.DisplayName;
            int cost = unit.Economy.creditCost;

            MakeText(card.transform, "UnitName", displayName,
                17, Color.white,
                new Vector2(0.04f, 0.42f), new Vector2(0.96f, 0.94f),
                TextAnchor.MiddleLeft);

            MakeText(card.transform, "UnitCost", cost.ToString("N0") + " CREDITS",
                12, new Color(0.55f, 0.88f, 0.67f, 1f),
                new Vector2(0.04f, 0.06f), new Vector2(0.96f, 0.42f),
                TextAnchor.MiddleLeft);

            button.onClick.AddListener(() => SelectUnit(unit));
        }

        private void SelectUnit(UnitDefinition unit)
        {
            selectedUnit = unit;

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

        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }
    }
}


