using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Store;
using ObsidianProtocol.Game.Units;
using ObsidianProtocol.Game.Runtime;

namespace ObsidianProtocol.UI.Store
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class StoreMasterController : MonoBehaviour
    {
        private TMP_Text creditsText;
        private TMP_Text statusText;

        private TMP_Text selectedName;
        private TMP_Text selectedCategory;
        private TMP_Text selectedPrice;
        private TMP_Text selectedOwnership;
        private TMP_Text selectedDP;
        private TMP_Text selectedMaintenance;
        private TMP_Text selectedOperations;
        private TMP_Text selectedDescription;
        private StoreUnitPreview3D unitPreview;

        private TMP_InputField searchField;
        private Transform catalogRoot;
        private ScrollRect catalogScroll;

        private GameObject confirmationPanel;
        private TMP_Text confirmationText;

        private UnitDefinition selectedUnit;

        private string activeCategory = "ALL";

        private readonly List<UnitDefinition> units =
            new List<UnitDefinition>();

        private readonly Color background =
            new Color(0.008f, 0.012f, 0.018f, 1f);

        private readonly Color panel =
            new Color(0.025f, 0.035f, 0.045f, 1f);

        private readonly Color panel2 =
            new Color(0.045f, 0.060f, 0.072f, 1f);

        private readonly Color line =
            new Color(0.18f, 0.23f, 0.27f, 1f);

        private readonly Color accent =
            new Color(0.88f, 0.68f, 0.22f, 1f);

        private readonly Color good =
            new Color(0.30f, 0.78f, 0.48f, 1f);

        private readonly Color danger =
            new Color(0.85f, 0.28f, 0.24f, 1f);

        private Canvas canvas;
        private CanvasScaler canvasScaler;
        private GraphicRaycaster raycaster;

        private bool interfaceBuilt;
        private GameObject storeInterface;
        private CommanderPlayerMovement commanderMovement;
        private bool storeOpen;

        private void Awake()
        {
            EnsureCanvas();
        }

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                EnsureCanvas();
                LoadUnits();
                BuildInterface();
                RefreshCredits();
                RefreshCatalog();

                if (units.Count > 0)
                    SelectUnit(units.FirstOrDefault(u => string.Equals(u.DisplayName, "Warden", StringComparison.OrdinalIgnoreCase)) ?? units[0]);

                Canvas.ForceUpdateCanvases();
            }
        }

        private void Start()
        {
            Debug.Log("[STORE] START ENTERED");

            EnsureCanvas();
            Debug.Log("[STORE] CANVAS READY");

            LoadUnits();
            Debug.Log("[STORE] UNITS LOADED = " + units.Count);

            BuildInterface();
            Debug.Log("[STORE] INTERFACE BUILT");

            RefreshCredits();
            RefreshCatalog();

            if (units.Count > 0)
                SelectUnit(units.FirstOrDefault(u => string.Equals(u.DisplayName, "Warden", StringComparison.OrdinalIgnoreCase)) ?? units[0]);

            Canvas.ForceUpdateCanvases();

            Debug.Log(
                "[STORE] MASTER INTERFACE ACTIVE | " +
                "UNITS=" + units.Count +
                " | CANVAS=SCREEN_SPACE_OVERLAY");
        }

        private void EnsureCanvas()
        {

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            RectTransform rect =
                GetComponent<RectTransform>();

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localScale = Vector3.one;

            canvas =
                GetComponent<Canvas>();

            if (canvas == null)
                canvas = gameObject.AddComponent<Canvas>();

            canvas.enabled = true;
            canvas.renderMode =
                RenderMode.ScreenSpaceOverlay;

            canvas.worldCamera = null;
            canvas.planeDistance = 100f;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 5000;

            canvasScaler =
                GetComponent<CanvasScaler>();

            if (canvasScaler == null)
                canvasScaler =
                    gameObject.AddComponent<CanvasScaler>();

            canvasScaler.enabled = true;
            canvasScaler.uiScaleMode =
                CanvasScaler.ScaleMode.ScaleWithScreenSize;

            canvasScaler.referenceResolution =
                new Vector2(1920f, 1080f);

            canvasScaler.screenMatchMode =
                CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

            canvasScaler.matchWidthOrHeight = 0.5f;

            raycaster =
                GetComponent<GraphicRaycaster>();

            if (raycaster == null)
                raycaster =
                    gameObject.AddComponent<GraphicRaycaster>();

            raycaster.enabled = true;

            gameObject.SetActive(true);
        }

        private void LoadUnits()
        {
            units.Clear();

            foreach (InventoryItemDefinition definition
                in InventoryDefinitionRegistry.GetAll())
            {
                if (definition == null)
                    continue;

                if (definition.Type !=
                    InventoryItemType.Unit)
                    continue;

                UnitDefinition unit =
                    UnitDatabaseRuntime.Get(
                        definition.DefinitionId);

                if (unit != null)
                    units.Add(unit);
            }

            units.Sort(
                (a, b) =>
                    string.Compare(
                        a.DisplayName,
                        b.DisplayName,
                        StringComparison.OrdinalIgnoreCase));
        }

        private void BuildInterface()
        {
            Transform old =
                transform.Find("STORE_MASTER_INTERFACE");

            if (old != null)
            {
                if (Application.isPlaying)
                    Destroy(old.gameObject);
                else
                    DestroyImmediate(old.gameObject);
            }

            GameObject root =
                CreateUI(
                    "STORE_MASTER_INTERFACE",
                    transform);

            RectTransform rootRect =
                root.GetComponent<RectTransform>();

            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;
            rootRect.localScale = Vector3.one;

            Image backgroundImage =
                root.AddComponent<Image>();

            backgroundImage.color =
                background;

            BuildTop(root.transform);
            BuildCategories(root.transform);
            BuildCatalog(root.transform);
            BuildInspection(root.transform);
            BuildBottom(root.transform);
            BuildConfirmation(root.transform);

            interfaceBuilt = true;
            storeInterface = root;
            root.SetActive(false);
        }

        private void Update()
        {
            if (Application.isPlaying && storeOpen && Input.GetKeyDown(KeyCode.Escape))
                CloseStore();
        }

        public void OpenStore()
        {
            if (storeInterface == null)
                storeInterface = transform.Find("STORE_MASTER_INTERFACE")?.gameObject;
            if (storeInterface == null)
            {
                Debug.LogError("[STORE] Interface is missing.");
                return;
            }
            if (commanderMovement == null)
                commanderMovement = FindFirstObjectByType<CommanderPlayerMovement>();
            if (commanderMovement != null)
                commanderMovement.enabled = false;
            storeInterface.SetActive(true);
            storeOpen = true;
            Canvas.ForceUpdateCanvases();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            RefreshCredits();
            RefreshCatalog();
            Debug.Log("[STORE] OPEN");
        }

        public void CloseStore()
        {
            if (storeInterface == null)
                storeInterface = transform.Find("STORE_MASTER_INTERFACE")?.gameObject;
            if (storeInterface != null)
                storeInterface.SetActive(false);
            storeOpen = false;
            if (commanderMovement == null)
                commanderMovement = FindFirstObjectByType<CommanderPlayerMovement>();
            if (commanderMovement != null)
                commanderMovement.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("[STORE] CLOSED");
        }
        private void BuildTop(Transform parent)
        {
            GameObject bar =
                Panel(
                    "TOP_BAR",
                    parent,
                    new Vector2(0f, 0.89f),
                    new Vector2(1f, 1f),
                    panel);

            TMP_Text title =
                Label(
                    "MILITARY PROCUREMENT",
                    bar.transform,
                    25,
                    TextAlignmentOptions.Left);

            Stretch(
                title.rectTransform,
                0.025f,
                0.28f,
                0.55f,
                0.92f);

            TMP_Text subtitle =
                Label(
                    "OBSIDIAN PROTOCOL  //  AUTHORIZED PROCUREMENT TERMINAL",
                    bar.transform,
                    8,
                    TextAlignmentOptions.Left);

            subtitle.color =
                new Color(0.55f, 0.60f, 0.64f, 1f);

            Stretch(
                subtitle.rectTransform,
                0.027f,
                0.06f,
                0.60f,
                0.28f);

            creditsText =
                Label(
                    "CREDITS  0",
                    bar.transform,
                    18,
                    TextAlignmentOptions.Right);

            creditsText.color = accent;

            Stretch(
                creditsText.rectTransform,
                0.55f,
                0.32f,
                0.78f,
                0.90f);

            statusText =
                Label(
                    "PROCUREMENT SYSTEM READY",
                    bar.transform,
                    8,
                    TextAlignmentOptions.Right);

            statusText.color = good;

            Stretch(
                statusText.rectTransform,
                0.55f,
                0.05f,
                0.78f,
                0.27f);
            Button closeButton = MakeButton("CLOSE STORE", bar.transform, 10);
            RectTransform closeRect = closeButton.GetComponent<RectTransform>();
            closeRect.anchorMin = new Vector2(0.82f, 0.20f);
            closeRect.anchorMax = new Vector2(0.98f, 0.80f);
            closeRect.offsetMin = Vector2.zero;
            closeRect.offsetMax = Vector2.zero;
            closeButton.onClick.AddListener(CloseStore);
        }
        private void BuildCategories(Transform parent)
        {
            GameObject rail =
                Panel(
                    "CATEGORY_RAIL",
                    parent,
                    new Vector2(0.01f, 0.075f),
                    new Vector2(0.17f, 0.875f),
                    panel);

            TMP_Text heading =
                Label(
                    "STORE CATALOG",
                    rail.transform,
                    13,
                    TextAlignmentOptions.Left);

            heading.color = accent;

            Stretch(
                heading.rectTransform,
                0.08f,
                0.92f,
                0.92f,
                0.98f);

            string[] categories =
            {
                "ALL",
                "COMMAND",
                "AIR",
                "GROUND",
                "SEA",
                "EXPERIMENTAL",
                "SATELLITE"
            };

            float y = 0.82f;

            foreach (string category in categories)
            {
                string captured = category;

                Button button =
                    MakeButton(
                        category,
                        rail.transform,
                        10);

                RectTransform rect =
                    button.GetComponent<RectTransform>();

                rect.anchorMin =
                    new Vector2(0.08f, y - 0.065f);

                rect.anchorMax =
                    new Vector2(0.92f, y);

                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;

                button.onClick.AddListener(
                    () =>
                    {
                        activeCategory = captured;
                        RefreshCatalog();
                    });

                y -= 0.095f;
            }

            TMP_Text rule =
                Label(
                    "OWNERSHIP DOES NOT INCREASE\nCOMPETITIVE DEPLOYMENT POWER.",
                    rail.transform,
                    8,
                    TextAlignmentOptions.Center);

            rule.color =
                new Color(0.50f, 0.55f, 0.58f, 1f);

            Stretch(
                rule.rectTransform,
                0.08f,
                0.03f,
                0.92f,
                0.18f);
        }

        private void BuildCatalog(Transform parent)
        {
            GameObject catalog =
                Panel(
                    "CATALOG_PANEL",
                    parent,
                    new Vector2(0.18f, 0.075f),
                    new Vector2(0.665f, 0.875f),
                    panel);

            TMP_Text heading =
                Label(
                    "AVAILABLE UNITS",
                    catalog.transform,
                    13,
                    TextAlignmentOptions.Left);

            heading.color = accent;

            Stretch(
                heading.rectTransform,
                0.025f,
                0.925f,
                0.50f,
                0.985f);

            searchField =
                CreateSearchField(
                    catalog.transform);

            GameObject scroll =
                CreateUI(
                    "CATALOG_SCROLL",
                    catalog.transform);

            RectTransform scrollRect =
                scroll.GetComponent<RectTransform>();

            Stretch(
                scrollRect,
                0.025f,
                0.035f,
                0.975f,
                0.905f);

            catalogScroll = scroll.AddComponent<ScrollRect>();
            ScrollRect scrollComponent = catalogScroll;

            scrollComponent.horizontal = false;
            scrollComponent.vertical = true;
            scrollComponent.movementType =
                ScrollRect.MovementType.Clamped;

            GameObject viewport =
                CreateUI(
                    "VIEWPORT",
                    scroll.transform);

            RectTransform viewportRect =
                viewport.GetComponent<RectTransform>();

            Stretch(
                viewportRect,
                0f,
                0f,
                1f,
                1f);

            Image viewportImage =
                viewport.AddComponent<Image>();

            viewportImage.color =
                new Color(0f, 0f, 0f, 0f);

            Mask mask =
                viewport.AddComponent<Mask>();

            mask.showMaskGraphic = false;

            GameObject content =
                CreateUI(
                    "UNIT_CATALOG",
                    viewport.transform);

            catalogRoot =
                content.transform;

            RectTransform contentRect =
                content.GetComponent<RectTransform>();

            contentRect.anchorMin =
                new Vector2(0f, 1f);

            contentRect.anchorMax =
                new Vector2(1f, 1f);

            contentRect.pivot =
                new Vector2(0.5f, 1f);

            contentRect.offsetMin =
                Vector2.zero;

            contentRect.offsetMax =
                Vector2.zero;

            contentRect.sizeDelta = new Vector2(0f, 600f);

            scrollComponent.viewport =
                viewportRect;

            scrollComponent.content =
                contentRect;
        }

        private void BuildInspection(Transform parent)
        {
            GameObject detail =
                Panel(
                    "INSPECTION_PANEL",
                    parent,
                    new Vector2(0.675f, 0.075f),
                    new Vector2(0.99f, 0.875f),
                    panel);

            TMP_Text heading =
                Label(
                    "UNIT INSPECTION  //  LIVE MODEL",
                    detail.transform,
                    13,
                    TextAlignmentOptions.Left);

            heading.color = accent;
            Stretch(heading.rectTransform, 0.05f, 0.925f, 0.95f, 0.985f);

            selectedName =
                Label("SELECT UNIT", detail.transform, 20, TextAlignmentOptions.Left);
            Stretch(selectedName.rectTransform, 0.05f, 0.845f, 0.95f, 0.92f);

            selectedCategory =
                Label("CATEGORY", detail.transform, 8, TextAlignmentOptions.Left);
            selectedCategory.color = accent;
            Stretch(selectedCategory.rectTransform, 0.05f, 0.805f, 0.95f, 0.845f);

            GameObject previewHost =
                Panel(
                    "UNIT_3D_PREVIEW_HOST",
                    detail.transform,
                    new Vector2(0.05f, 0.425f),
                    new Vector2(0.95f, 0.795f),
                    background);

            unitPreview = previewHost.AddComponent<StoreUnitPreview3D>();
            unitPreview.Initialize();

            selectedPrice =
                Label("PROCUREMENT COST", detail.transform, 12, TextAlignmentOptions.Left);
            selectedPrice.color = accent;
            Stretch(selectedPrice.rectTransform, 0.05f, 0.375f, 0.95f, 0.42f);

            selectedOwnership =
                Label("OWNERSHIP STATUS", detail.transform, 9, TextAlignmentOptions.Left);
            Stretch(selectedOwnership.rectTransform, 0.05f, 0.335f, 0.95f, 0.375f);

            selectedDP =
                Label("DEPLOYMENT POINTS", detail.transform, 9, TextAlignmentOptions.Left);
            Stretch(selectedDP.rectTransform, 0.05f, 0.295f, 0.95f, 0.335f);

            selectedMaintenance =
                Label("MAINTENANCE", detail.transform, 9, TextAlignmentOptions.Left);
            Stretch(selectedMaintenance.rectTransform, 0.05f, 0.255f, 0.95f, 0.295f);

            selectedOperations =
                Label("OPERATIONAL COST", detail.transform, 9, TextAlignmentOptions.Left);
            Stretch(selectedOperations.rectTransform, 0.05f, 0.215f, 0.95f, 0.255f);

            selectedDescription =
                Label("", detail.transform, 8, TextAlignmentOptions.Left);
            selectedDescription.textWrappingMode = TextWrappingModes.Normal;
            Stretch(selectedDescription.rectTransform, 0.05f, 0.105f, 0.95f, 0.205f);

            Button purchase = MakeButton("PURCHASE UNIT", detail.transform, 11);
            RectTransform purchaseRect = purchase.GetComponent<RectTransform>();
            purchaseRect.anchorMin = new Vector2(0.05f, 0.055f);
            purchaseRect.anchorMax = new Vector2(0.95f, 0.095f);
            purchaseRect.offsetMin = Vector2.zero;
            purchaseRect.offsetMax = Vector2.zero;
            purchase.onClick.AddListener(RequestPurchase);

            Button refresh = MakeButton("REFRESH", detail.transform, 8);
            RectTransform refreshRect = refresh.GetComponent<RectTransform>();
            refreshRect.anchorMin = new Vector2(0.05f, 0.015f);
            refreshRect.anchorMax = new Vector2(0.95f, 0.045f);
            refreshRect.offsetMin = Vector2.zero;
            refreshRect.offsetMax = Vector2.zero;
            refresh.onClick.AddListener(RefreshCurrent);
        }

        private void BuildBottom(Transform parent)
        {
            GameObject bottom =
                Panel(
                    "SYSTEM_BAR",
                    parent,
                    new Vector2(0.01f, 0.005f),
                    new Vector2(0.99f, 0.06f),
                    panel2);

            TMP_Text text =
                Label(
                    "PROCUREMENT // INVENTORY PERSISTENCE ACTIVE // DEPLOYMENT BUDGET SEPARATE",
                    bottom.transform,
                    8,
                    TextAlignmentOptions.Center);

            text.color =
                new Color(0.55f, 0.60f, 0.63f, 1f);

            Stretch(
                text.rectTransform,
                0.02f,
                0f,
                0.98f,
                1f);
        }

        private void BuildConfirmation(Transform parent)
        {
            confirmationPanel =
                Panel(
                    "CONFIRMATION",
                    parent,
                    new Vector2(0.30f, 0.28f),
                    new Vector2(0.70f, 0.72f),
                    new Color(0.018f, 0.025f, 0.032f, 0.99f));

            confirmationPanel.SetActive(false);

            TMP_Text title =
                Label(
                    "CONFIRM PROCUREMENT",
                    confirmationPanel.transform,
                    17,
                    TextAlignmentOptions.Center);

            title.color = accent;

            Stretch(
                title.rectTransform,
                0.05f,
                0.78f,
                0.95f,
                0.94f);

            confirmationText =
                Label(
                    "",
                    confirmationPanel.transform,
                    10,
                    TextAlignmentOptions.Center);

            confirmationText.textWrappingMode =
                TextWrappingModes.Normal;

            Stretch(
                confirmationText.rectTransform,
                0.08f,
                0.38f,
                0.92f,
                0.73f);

            Button confirm =
                MakeButton(
                    "CONFIRM PURCHASE",
                    confirmationPanel.transform,
                    10);

            RectTransform confirmRect =
                confirm.GetComponent<RectTransform>();

            confirmRect.anchorMin =
                new Vector2(0.08f, 0.16f);

            confirmRect.anchorMax =
                new Vector2(0.46f, 0.29f);

            confirmRect.offsetMin = Vector2.zero;
            confirmRect.offsetMax = Vector2.zero;

            confirm.onClick.AddListener(
                CompletePurchase);

            Button cancel =
                MakeButton(
                    "CANCEL",
                    confirmationPanel.transform,
                    10);

            RectTransform cancelRect =
                cancel.GetComponent<RectTransform>();

            cancelRect.anchorMin =
                new Vector2(0.54f, 0.16f);

            cancelRect.anchorMax =
                new Vector2(0.92f, 0.29f);

            cancelRect.offsetMin = Vector2.zero;
            cancelRect.offsetMax = Vector2.zero;

            cancel.onClick.AddListener(
                () =>
                {
                    confirmationPanel.SetActive(false);
                });
        }

        private TMP_InputField CreateSearchField(
            Transform parent)
        {
            GameObject box =
                Panel(
                    "SEARCH_FIELD",
                    parent,
                    new Vector2(0.64f, 0.915f),
                    new Vector2(0.975f, 0.985f),
                    panel2);

            TMP_InputField field =
                box.AddComponent<TMP_InputField>();

            GameObject textObject =
                CreateUI(
                    "TEXT",
                    box.transform);

            TMP_Text text =
                textObject.AddComponent<TextMeshProUGUI>();

            text.fontSize = 18;
            text.color = Color.white;
            text.textWrappingMode =
                TextWrappingModes.NoWrap;

            Stretch(
                text.rectTransform,
                0.05f,
                0.05f,
                0.95f,
                0.95f);

            field.textComponent = text;

            field.onValueChanged.AddListener(
                delegate
                {
                    RefreshCatalog();
                });

            return field;
        }

        private void RefreshCatalog()
        {
            if (!interfaceBuilt ||
                catalogRoot == null)
                return;

            for (int i = catalogRoot.childCount - 1;
                 i >= 0;
                 i--)
            {
                Destroy(
                    catalogRoot.GetChild(i).gameObject);
            }

            string search =
                searchField != null
                    ? searchField.text.Trim()
                    : "";

            List<UnitDefinition> filtered =
                units.Where(
                    u =>
                        MatchesCategory(
                            u,
                            activeCategory) &&
                        (
                            string.IsNullOrWhiteSpace(search) ||
                            u.DisplayName.IndexOf(
                                search,
                                StringComparison.OrdinalIgnoreCase) >= 0 ||
                            u.UnitId.IndexOf(
                                search,
                                StringComparison.OrdinalIgnoreCase) >= 0
                        ))
                .ToList();

            RectTransform content = catalogRoot as RectTransform;
            RectTransform viewport = content.parent as RectTransform;

            float availableWidth = viewport.rect.width;
            Debug.Log(
                "[STORE LAYOUT] RootActive=" + (storeInterface != null && storeInterface.activeInHierarchy) +
                " | CatalogActive=" + (catalogRoot != null && catalogRoot.gameObject.activeInHierarchy) +
                " | Viewport=" + viewport.rect.width + "x" + viewport.rect.height +
                " | FilteredUnits=" + filtered.Count +
                " | Screen=" + Screen.width + "x" + Screen.height);
            int columns = 3;
            float gap = 12f;
            float cardHeight = 226f;
            float cardWidth = Mathf.Max(150f, (availableWidth - gap * (columns - 1)) / columns);

            int rows = Mathf.CeilToInt(filtered.Count / (float)columns);
            content.sizeDelta = new Vector2(0f, Mathf.Max(600f, rows * (cardHeight + gap)));

            for (int i = 0; i < filtered.Count; i++)
            {
                UnitDefinition unit = filtered[i];
                int row = i / columns;
                int column = i % columns;

                GameObject card = CreateUI("UNIT_CARD_" + unit.UnitId, catalogRoot);
                RectTransform cardRect = card.GetComponent<RectTransform>();
                cardRect.anchorMin = new Vector2(0f, 1f);
                cardRect.anchorMax = new Vector2(0f, 1f);
                cardRect.pivot = new Vector2(0f, 1f);
                cardRect.sizeDelta = new Vector2(cardWidth, cardHeight);
                cardRect.anchoredPosition = new Vector2(column * (cardWidth + gap), -row * (cardHeight + gap));

                Image cardImage = card.AddComponent<Image>();
                cardImage.color = panel2;

                Outline outline = card.AddComponent<Outline>();
                outline.effectColor = line;
                outline.effectDistance = new Vector2(1f, 1f);

                Button button = card.AddComponent<Button>();
                ColorBlock colors = button.colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color(accent.r, accent.g, accent.b, 0.22f);
                colors.pressedColor = new Color(accent.r, accent.g, accent.b, 0.35f);
                button.colors = colors;

                GameObject art = CreateUI("UNIT_IMAGE_PLACEHOLDER", card.transform);
                RectTransform artRect = art.GetComponent<RectTransform>();
                Stretch(artRect, 0.06f, 0.43f, 0.94f, 0.94f);
                Image artImage = art.AddComponent<Image>();
                artImage.color = background;

                TMP_Text artLabel = Label("UNIT VISUAL\nART PENDING", art.transform, 10, TextAlignmentOptions.Center);
                Stretch(artLabel.rectTransform, 0.05f, 0.05f, 0.95f, 0.95f);
                artLabel.color = accent;

                TMP_Text name = Label(unit.DisplayName, card.transform, 13, TextAlignmentOptions.Left);
                Stretch(name.rectTransform, 0.06f, 0.29f, 0.94f, 0.41f);

                TMP_Text category = Label(GetCategory(unit), card.transform, 9, TextAlignmentOptions.Left);
                category.color = accent;
                Stretch(category.rectTransform, 0.06f, 0.20f, 0.94f, 0.29f);

                TMP_Text price = Label(FormatCredits(StoreUnitPricingCatalog.GetPrice(unit)), card.transform, 11, TextAlignmentOptions.Left);
                price.color = accent;
                Stretch(price.rectTransform, 0.06f, 0.06f, 0.60f, 0.19f);

                bool owned = InventoryRuntime.Has(unit.UnitId);
                TMP_Text ownership = Label(owned ? "OWNED" : "AVAILABLE", card.transform, 8, TextAlignmentOptions.Right);
                ownership.color = owned ? good : Color.white;
                Stretch(ownership.rectTransform, 0.53f, 0.06f, 0.94f, 0.19f);

                button.onClick.AddListener(() => SelectUnit(unit));
            }
            if (catalogScroll != null)
            {
                catalogScroll.StopMovement();
                catalogScroll.verticalNormalizedPosition = 1f;
            }

            Canvas.ForceUpdateCanvases();
        }

        private void SelectUnit(
            UnitDefinition unit)
        {
            selectedUnit = unit;

            if (unit == null)
                return;

            if (unitPreview != null)
                unitPreview.ShowUnit(unit);

            bool owned =
                InventoryRuntime.Has(
                    unit.UnitId);

            selectedName.text =
                unit.DisplayName;

            selectedCategory.text =
                GetCategory(unit) +
                "  //  " +
                unit.UnitId;

            selectedPrice.text =
                "PROCUREMENT COST   " +
                FormatCredits(
                    StoreUnitPricingCatalog.GetPrice(unit));

            selectedOwnership.text =
                "OWNERSHIP STATUS   " +
                (owned ? "OWNED" : "AVAILABLE");

            selectedOwnership.color =
                owned ? good : Color.white;

            selectedDP.text =
                "DEPLOYMENT POINTS   " +
                unit.Economy.deploymentPoints;

            selectedMaintenance.text =
                "MAINTENANCE   " +
                Safe(unit.Economy.maintenanceCost);

            selectedOperations.text =
                "OPERATIONAL COST   " +
                Safe(unit.Economy.operationalCost);

            selectedDescription.text =
                "ROLE   " +
                Safe(unit.Role) +
                "\n" +
                "TIER   " +
                Safe(unit.Tier) +
                "\n" +
                "STATUS   " +
                Safe(unit.Status) +
                "\n\n" +
                "FACTORY   " +
                Safe(unit.Economy.factoryType) +
                "\n" +
                "BUILD TIME   " +
                unit.Economy.buildTimeSeconds.ToString("0") +
                " SEC\n" +
                "LAUNCH METHOD   " +
                Safe(unit.Economy.launchMethod) +
                "\n" +
                "DEPLOYMENT REQUIREMENTS   " +
                Safe(unit.Economy.deploymentRequirements);
        }

        private void RequestPurchase()
        {
            if (selectedUnit == null)
                return;

            if (!InventoryRuntime.CampaignActive ||
                !EconomyRuntime.CampaignActive)
            {
                SetStatus(
                    "NO ACTIVE CAMPAIGN",
                    danger);

                return;
            }

            if (InventoryRuntime.Has(
                selectedUnit.UnitId))
            {
                SetStatus(
                    "UNIT ALREADY OWNED",
                    danger);

                return;
            }

            int price =
                StoreUnitPricingCatalog.GetPrice(selectedUnit);

            if (price <= 0)
            {
                SetStatus(
                    "PRICE NOT CONFIGURED",
                    danger);

                return;
            }

            if (EconomyRuntime.Credits < price)
            {
                SetStatus(
                    "INSUFFICIENT CREDITS",
                    danger);

                return;
            }

            confirmationText.text =
                selectedUnit.DisplayName +
                "\n\n" +
                "PROCUREMENT COST   " +
                FormatCredits(price) +
                "\n\n" +
                "Credits will be deducted immediately." +
                "\nThe unit will be added to persistent inventory." +
                "\n\n" +
                "Battlefield deployment budget is unchanged.";

            confirmationPanel.SetActive(true);
        }

        private void CompletePurchase()
        {
            confirmationPanel.SetActive(false);

            if (selectedUnit == null)
                return;

            UnitPurchaseService service =
                new UnitPurchaseService();

            bool success =
                service.Purchase(
                    selectedUnit.UnitId);

            if (!success)
            {
                SetStatus(
                    "PROCUREMENT FAILED",
                    danger);

                return;
            }

            SetStatus(
                "PROCUREMENT COMPLETE  //  " +
                selectedUnit.DisplayName,
                good);

            RefreshCredits();
            RefreshCatalog();
            SelectUnit(selectedUnit);
        }

        private void RefreshCurrent()
        {
            LoadUnits();
            RefreshCredits();
            RefreshCatalog();

            if (selectedUnit != null)
                SelectUnit(selectedUnit);
            else if (units.Count > 0)
                SelectUnit(units.FirstOrDefault(u => string.Equals(u.DisplayName, "Warden", StringComparison.OrdinalIgnoreCase)) ?? units[0]);
        }

        private void RefreshCredits()
        {
            if (creditsText != null)
            {
                creditsText.text =
                    "CREDITS  " +
                    EconomyRuntime.Credits.ToString("N0");
            }
        }

        private void SetStatus(
            string message,
            Color color)
        {
            if (statusText == null)
                return;

            statusText.text = message;
            statusText.color = color;
        }

        private bool MatchesCategory(
            UnitDefinition unit,
            string category)
        {
            if (category == "ALL")
                return true;

            return GetCategory(unit)
                .Equals(
                    category,
                    StringComparison.OrdinalIgnoreCase);
        }

        private string GetCategory(
            UnitDefinition unit)
        {
            switch (
                unit.Category.ToString()
                    .ToUpperInvariant())
            {
                case "COMMAND":
                    return "COMMAND";

                case "AIR":
                    return "AIR";

                case "GROUND":
                    return "GROUND";

                case "SEA":
                    return "SEA";

                case "EXPERIMENTAL":
                    return "EXPERIMENTAL";

                case "SATELLITE":
                    return "SATELLITE";
            }

            string id =
                unit.UnitId.ToLowerInvariant();

            if (id.Contains("command"))
                return "COMMAND";

            if (id.Contains("satellite"))
                return "SATELLITE";

            return "OTHER";
        }

        private string Safe(
            string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "NOT SPECIFIED"
                : value;
        }

        private string FormatCredits(
            int amount)
        {
            return amount.ToString("N0") + " C";
        }

        private GameObject Panel(
            string name,
            Transform parent,
            Vector2 min,
            Vector2 max,
            Color color)
        {
            GameObject go =
                CreateUI(name, parent);

            RectTransform rect =
                go.GetComponent<RectTransform>();

            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            Image image =
                go.AddComponent<Image>();

            image.color = color;

            Outline outline =
                go.AddComponent<Outline>();

            outline.effectColor = line;
            outline.effectDistance =
                new Vector2(1f, 1f);

            return go;
        }

        private Button MakeButton(
            string text,
            Transform parent,
            int fontSize)
        {
            GameObject go =
                CreateUI(
                    "BUTTON_" + text,
                    parent);

            Image image =
                go.AddComponent<Image>();

            image.color = panel2;

            Outline outline =
                go.AddComponent<Outline>();

            outline.effectColor = line;
            outline.effectDistance =
                new Vector2(1f, 1f);

            Button button =
                go.AddComponent<Button>();

            ColorBlock colors =
                button.colors;

            colors.normalColor = panel2;

            colors.highlightedColor =
                new Color(
                    accent.r,
                    accent.g,
                    accent.b,
                    0.25f);

            colors.pressedColor =
                new Color(
                    accent.r,
                    accent.g,
                    accent.b,
                    0.45f);

            button.colors = colors;

            TMP_Text label =
                Label(
                    text,
                    go.transform,
                    fontSize,
                    TextAlignmentOptions.Center);

            Stretch(
                label.rectTransform,
                0.03f,
                0.03f,
                0.97f,
                0.97f);

            return button;
        }

        private TMP_Text Label(
            string text,
            Transform parent,
            int fontSize,
            TextAlignmentOptions alignment)
        {
            GameObject go =
                CreateUI(
                    "TEXT_" +
                    Guid.NewGuid().ToString("N"),
                    parent);

            TMP_Text label =
                go.AddComponent<TextMeshProUGUI>();

            label.text = text;
            label.fontSize = Mathf.Clamp(fontSize, 18, 24);
            label.color = Color.white;
            label.alignment = alignment;
            label.textWrappingMode =
                TextWrappingModes.Normal;
            label.overflowMode =
                TextOverflowModes.Ellipsis;

            return label;
        }

        private GameObject CreateUI(
            string name,
            Transform parent)
        {
            GameObject go =
                new GameObject(
                    name,
                    typeof(RectTransform));

            go.transform.SetParent(
                parent,
                false);

            go.transform.localScale =
                Vector3.one;

            return go;
        }

        private void Stretch(
            RectTransform rect,
            float minX,
            float minY,
            float maxX,
            float maxY)
        {
            rect.anchorMin =
                new Vector2(minX, minY);

            rect.anchorMax =
                new Vector2(maxX, maxY);

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}



















