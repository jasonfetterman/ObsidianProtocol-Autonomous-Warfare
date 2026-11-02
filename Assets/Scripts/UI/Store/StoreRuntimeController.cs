using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Store;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.UI.Store
{
    public sealed class StoreRuntimeController : MonoBehaviour
    {
        private Canvas canvas;
        private RectTransform root;

        private TMP_Text credits;
        private TMP_Text catalogStatus;

        private TMP_Text detailName;
        private TMP_Text detailCategory;
        private TMP_Text detailPrice;
        private TMP_Text detailDeployment;
        private TMP_Text detailMaintenance;
        private TMP_Text detailOperations;
        private TMP_Text detailStatus;
        private TMP_Text detailDescription;

        private Button purchaseButton;

        private GameObject confirmation;
        private TMP_Text confirmationText;

        private RectTransform cardContainer;

        private string selectedId = "";
        private string selectedCategory = "ALL";

        private readonly List<InventoryItemDefinition> unitDefinitions =
            new List<InventoryItemDefinition>();

        private readonly Dictionary<string, Button> categoryButtons =
            new Dictionary<string, Button>();

        private static readonly string[] Categories =
        {
            "ALL",
            "COMMAND",
            "AIR",
            "GROUND",
            "SEA",
            "EXPERIMENTAL",
            "SATELLITE"
        };

        private void Awake()
        {
            BuildStore();
        }

        private void BuildStore()
        {
            canvas = GetComponent<Canvas>();

            if (canvas == null)
                canvas = GetComponentInParent<Canvas>();

            if (canvas == null)
                return;

            Transform storePanel =
                canvas.transform.Find("StorePanel");

            if (storePanel == null)
            {
                Debug.LogError(
                    "StoreRuntimeController: StorePanel was not found under the Garage Canvas.");
                return;
            }

            Transform old =
                storePanel.Find("STORE_FINAL_INTERFACE");

            if (old != null)
                Destroy(old.gameObject);

            Transform old2 =
                storePanel.Find("STORE_REBUILT_INTERFACE");

            if (old2 != null)
                Destroy(old2.gameObject);

            root = Rect(
                "STORE_FINAL_INTERFACE",
                storePanel,
                Vector2.zero,
                Vector2.one);

            Image background =
                root.gameObject.AddComponent<Image>();

            background.color =
                new Color(0.012f, 0.019f, 0.024f, 0.985f);

            BuildTopBar();
            BuildCategoryRail();
            BuildCatalog();
            BuildDetails();
            BuildBottomBar();
            BuildConfirmation();

            LoadDefinitions();
            RefreshEverything();
        }

        private void BuildTopBar()
        {
            RectTransform bar =
                Panel(
                    "TOP_BAR",
                    root,
                    new Vector2(0f, 0.90f),
                    new Vector2(1f, 1f),
                    new Color(0.020f, 0.032f, 0.040f, 1f));

            TMP_Text eyebrow =
                Label(
                    bar,
                    "EYEBROW",
                    "OBSIDIAN PROTOCOL  //  MILITARY PROCUREMENT",
                    10f,
                    TextAlignmentOptions.Left);

            SetRect(
                eyebrow.rectTransform,
                0.025f,
                0.62f,
                0.50f,
                0.82f);

            TMP_Text title =
                Label(
                    bar,
                    "TITLE",
                    "MILITARY STORE",
                    24f,
                    TextAlignmentOptions.Left);

            SetRect(
                title.rectTransform,
                0.025f,
                0.18f,
                0.50f,
                0.66f);

            credits =
                Label(
                    bar,
                    "CREDITS",
                    "10,000 C",
                    19f,
                    TextAlignmentOptions.Right);

            SetRect(
                credits.rectTransform,
                0.70f,
                0.32f,
                0.975f,
                0.78f);

            TMP_Text balance =
                Label(
                    bar,
                    "BALANCE_LABEL",
                    "AVAILABLE CAMPAIGN CREDITS",
                    9f,
                    TextAlignmentOptions.Right);

            SetRect(
                balance.rectTransform,
                0.70f,
                0.12f,
                0.975f,
                0.32f);
        }

        private void BuildCategoryRail()
        {
            RectTransform rail =
                Panel(
                    "CATEGORY_RAIL",
                    root,
                    new Vector2(0.015f, 0.105f),
                    new Vector2(0.155f, 0.885f),
                    new Color(0.020f, 0.032f, 0.040f, 1f));

            TMP_Text heading =
                Label(
                    rail,
                    "HEADING",
                    "PROCUREMENT",
                    12f,
                    TextAlignmentOptions.Left);

            SetRect(
                heading.rectTransform,
                0.10f,
                0.93f,
                0.90f,
                0.985f);

            RectTransform buttons =
                Rect(
                    "BUTTONS",
                    rail,
                    new Vector2(0.07f, 0.08f),
                    new Vector2(0.93f, 0.91f));

            VerticalLayoutGroup layout =
                buttons.gameObject.AddComponent<VerticalLayoutGroup>();

            layout.spacing = 8f;
            layout.padding =
                new RectOffset(0, 0, 4, 4);

            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            foreach (string category in Categories)
            {
                Button button =
                    MakeButton(
                        buttons,
                        category,
                        11f,
                        42f);

                categoryButtons[category] = button;

                string captured = category;

                button.onClick.AddListener(
                    () =>
                    {
                        selectedCategory = captured;
                        RefreshCategories();
                        RefreshCatalog();
                    });
            }
        }

        private void BuildCatalog()
        {
            RectTransform panel =
                Panel(
                    "CATALOG_PANEL",
                    root,
                    new Vector2(0.17f, 0.105f),
                    new Vector2(0.655f, 0.885f),
                    new Color(0.020f, 0.032f, 0.040f, 1f));

            TMP_Text title =
                Label(
                    panel,
                    "TITLE",
                    "AVAILABLE UNITS",
                    16f,
                    TextAlignmentOptions.Left);

            SetRect(
                title.rectTransform,
                0.035f,
                0.935f,
                0.55f,
                0.985f);

            catalogStatus =
                Label(
                    panel,
                    "STATUS",
                    "",
                    9f,
                    TextAlignmentOptions.Right);

            SetRect(
                catalogStatus.rectTransform,
                0.55f,
                0.935f,
                0.965f,
                0.985f);

            RectTransform viewport =
                Rect(
                    "VIEWPORT",
                    panel,
                    new Vector2(0.025f, 0.025f),
                    new Vector2(0.975f, 0.915f));

            Image viewportImage =
                viewport.gameObject.AddComponent<Image>();

            viewportImage.color =
                new Color(0f, 0f, 0f, 0.01f);

            Mask mask =
                viewport.gameObject.AddComponent<Mask>();

            mask.showMaskGraphic = false;

            ScrollRect scroll =
                panel.gameObject.AddComponent<ScrollRect>();

            scroll.viewport = viewport;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType =
                ScrollRect.MovementType.Clamped;

            cardContainer =
                Rect(
                    "CARD_CONTAINER",
                    viewport,
                    new Vector2(0f, 1f),
                    new Vector2(1f, 1f));

            cardContainer.pivot =
                new Vector2(0.5f, 1f);

            GridLayoutGroup grid =
                cardContainer.gameObject.AddComponent<GridLayoutGroup>();

            grid.cellSize =
                new Vector2(245f, 132f);

            grid.spacing =
                new Vector2(10f, 10f);

            grid.padding =
                new RectOffset(5, 5, 5, 5);

            grid.constraint =
                GridLayoutGroup.Constraint.FixedColumnCount;

            grid.constraintCount = 2;

            ContentSizeFitter fitter =
                cardContainer.gameObject.AddComponent<ContentSizeFitter>();

            fitter.verticalFit =
                ContentSizeFitter.FitMode.PreferredSize;

            scroll.content = cardContainer;
        }

        private void BuildDetails()
        {
            RectTransform panel =
                Panel(
                    "DETAIL_PANEL",
                    root,
                    new Vector2(0.67f, 0.105f),
                    new Vector2(0.985f, 0.885f),
                    new Color(0.025f, 0.040f, 0.048f, 1f));

            TMP_Text heading =
                Label(
                    panel,
                    "HEADING",
                    "UNIT INSPECTION",
                    16f,
                    TextAlignmentOptions.Left);

            SetRect(
                heading.rectTransform,
                0.055f,
                0.93f,
                0.945f,
                0.985f);

            detailName =
                Label(
                    panel,
                    "NAME",
                    "SELECT UNIT",
                    22f,
                    TextAlignmentOptions.Left);

            SetRect(
                detailName.rectTransform,
                0.055f,
                0.825f,
                0.945f,
                0.905f);

            detailCategory =
                DetailLine(
                    panel,
                    "CATEGORY",
                    "",
                    0.765f);

            detailPrice =
                DetailLine(
                    panel,
                    "PRICE",
                    "",
                    0.695f);

            detailStatus =
                DetailLine(
                    panel,
                    "STATUS",
                    "",
                    0.625f);

            detailDeployment =
                DetailLine(
                    panel,
                    "DEPLOYMENT",
                    "",
                    0.555f);

            detailMaintenance =
                DetailLine(
                    panel,
                    "MAINTENANCE",
                    "",
                    0.485f);

            detailOperations =
                DetailLine(
                    panel,
                    "OPERATIONS",
                    "",
                    0.415f);

            TMP_Text descriptionHeader =
                Label(
                    panel,
                    "DESCRIPTION_HEADER",
                    "PROCUREMENT NOTES",
                    10f,
                    TextAlignmentOptions.Left);

            SetRect(
                descriptionHeader.rectTransform,
                0.055f,
                0.325f,
                0.945f,
                0.365f);

            detailDescription =
                Label(
                    panel,
                    "DESCRIPTION",
                    "Select a unit to review specifications and procurement status.",
                    10f,
                    TextAlignmentOptions.Left);

            SetRect(
                detailDescription.rectTransform,
                0.055f,
                0.16f,
                0.945f,
                0.32f);

            detailDescription.alignment =
                TextAlignmentOptions.TopLeft;

            detailDescription.overflowMode =
                TextOverflowModes.Ellipsis;

            purchaseButton =
                MakeButton(
                    panel,
                    "PURCHASE UNIT",
                    12f,
                    46f);

            SetRect(
                purchaseButton.GetComponent<RectTransform>(),
                0.055f,
                0.055f,
                0.945f,
                0.13f);

            purchaseButton.onClick.AddListener(
                OpenConfirmation);
        }

        private TMP_Text DetailLine(
            Transform parent,
            string name,
            string value,
            float y)
        {
            TMP_Text text =
                Label(
                    parent,
                    name,
                    value,
                    11f,
                    TextAlignmentOptions.Left);

            SetRect(
                text.rectTransform,
                0.055f,
                y,
                0.945f,
                y + 0.045f);

            return text;
        }

        private void BuildBottomBar()
        {
            RectTransform bar =
                Panel(
                    "BOTTOM_BAR",
                    root,
                    new Vector2(0f, 0f),
                    new Vector2(1f, 0.075f),
                    new Color(0.018f, 0.029f, 0.036f, 1f));

            TMP_Text text =
                Label(
                    bar,
                    "RULE",
                    "OWNERSHIP ≠ BATTLEFIELD POWER    •    DEPLOYMENT BUDGET REMAINS SEPARATE",
                    10f,
                    TextAlignmentOptions.Center);

            SetRect(
                text.rectTransform,
                0.02f,
                0.20f,
                0.98f,
                0.80f);
        }

        private void BuildConfirmation()
        {
            confirmation =
                Panel(
                    "CONFIRMATION",
                    root,
                    new Vector2(0.30f, 0.30f),
                    new Vector2(0.70f, 0.70f),
                    new Color(0.012f, 0.022f, 0.029f, 0.995f))
                    .gameObject;

            TMP_Text header =
                Label(
                    confirmation.transform,
                    "HEADER",
                    "PROCUREMENT AUTHORIZATION",
                    17f,
                    TextAlignmentOptions.Center);

            SetRect(
                header.rectTransform,
                0.08f,
                0.78f,
                0.92f,
                0.90f);

            confirmationText =
                Label(
                    confirmation.transform,
                    "MESSAGE",
                    "",
                    13f,
                    TextAlignmentOptions.Center);

            SetRect(
                confirmationText.rectTransform,
                0.08f,
                0.32f,
                0.92f,
                0.72f);

            Button confirm =
                MakeButton(
                    confirmation.transform,
                    "CONFIRM",
                    11f,
                    40f);

            SetRect(
                confirm.GetComponent<RectTransform>(),
                0.08f,
                0.10f,
                0.43f,
                0.22f);

            confirm.onClick.AddListener(
                CompletePurchase);

            Button cancel =
                MakeButton(
                    confirmation.transform,
                    "CANCEL",
                    11f,
                    40f);

            SetRect(
                cancel.GetComponent<RectTransform>(),
                0.57f,
                0.10f,
                0.92f,
                0.22f);

            cancel.onClick.AddListener(
                CloseConfirmation);

            confirmation.SetActive(false);
        }

        private void LoadDefinitions()
        {
            unitDefinitions.Clear();

            foreach (
                InventoryItemDefinition definition
                in InventoryDefinitionRegistry.GetAll())
            {
                if (definition == null)
                    continue;

                if (definition.Type !=
                    InventoryItemType.Unit)
                    continue;

                unitDefinitions.Add(definition);
            }

            unitDefinitions.Sort(
                (a, b) =>
                    string.Compare(
                        a.DisplayName,
                        b.DisplayName,
                        StringComparison.OrdinalIgnoreCase));
        }

        private void RefreshEverything()
        {
            RefreshCredits();
            RefreshCategories();
            RefreshCatalog();
            RefreshDetails();
        }

        private void RefreshCredits()
        {
            if (credits == null)
                return;

            credits.text =
                EconomyRuntime.Credits.ToString("N0") +
                " C";
        }

        private void RefreshCategories()
        {
            foreach (
                KeyValuePair<string, Button> item
                in categoryButtons)
            {
                item.Value.interactable =
                    item.Key != selectedCategory;
            }
        }

        private void RefreshCatalog()
        {
            if (cardContainer == null)
                return;

            for (
                int i = cardContainer.childCount - 1;
                i >= 0;
                i--)
            {
                Destroy(
                    cardContainer.GetChild(i).gameObject);
            }

            IEnumerable<InventoryItemDefinition> filtered =
                unitDefinitions;

            if (selectedCategory != "ALL")
            {
                filtered =
                    filtered.Where(
                        x =>
                            Category(
                                x.DefinitionId) ==
                            selectedCategory);
            }

            int count = 0;

            foreach (
                InventoryItemDefinition definition
                in filtered)
            {
                CreateCard(definition);
                count++;
            }

            if (catalogStatus != null)
            {
                catalogStatus.text =
                    count +
                    " ITEMS";
            }
        }

        private void CreateCard(
            InventoryItemDefinition definition)
        {
            GameObject card =
                new GameObject(
                    "CARD_" +
                    definition.DefinitionId,
                    typeof(RectTransform),
                    typeof(Image));

            card.transform.SetParent(
                cardContainer,
                false);

            Image image =
                card.GetComponent<Image>();

            image.color =
                new Color(
                    0.035f,
                    0.055f,
                    0.065f,
                    1f);

            RectTransform rect =
                card.GetComponent<RectTransform>();

            TMP_Text name =
                Label(
                    card.transform,
                    "NAME",
                    definition.DisplayName,
                    14f,
                    TextAlignmentOptions.Left);

            SetRect(
                name.rectTransform,
                0.06f,
                0.68f,
                0.94f,
                0.88f);

            TMP_Text category =
                Label(
                    card.transform,
                    "CATEGORY",
                    Category(definition.DefinitionId),
                    9f,
                    TextAlignmentOptions.Left);

            SetRect(
                category.rectTransform,
                0.06f,
                0.55f,
                0.94f,
                0.66f);

            UnitDefinition unit =
                UnitDatabaseRuntime.Get(
                    definition.DefinitionId);

            int price =
                unit != null
                    ? Convert.ToInt32(
                        unit.Economy.creditCost)
                    : 0;

            TMP_Text priceText =
                Label(
                    card.transform,
                    "PRICE",
                    price.ToString("N0") +
                    " C",
                    12f,
                    TextAlignmentOptions.Left);

            SetRect(
                priceText.rectTransform,
                0.06f,
                0.35f,
                0.50f,
                0.52f);

            bool owned =
                InventoryRuntime.Has(
                    definition.DefinitionId);

            TMP_Text state =
                Label(
                    card.transform,
                    "STATE",
                    owned
                        ? "OWNED"
                        : "AVAILABLE",
                    9f,
                    TextAlignmentOptions.Left);

            SetRect(
                state.rectTransform,
                0.06f,
                0.20f,
                0.45f,
                0.32f);

            Button inspect =
                MakeButton(
                    card.transform,
                    owned
                        ? "INSPECT"
                        : "SELECT",
                    10f,
                    32f);

            SetRect(
                inspect.GetComponent<RectTransform>(),
                0.55f,
                0.10f,
                0.94f,
                0.30f);

            string id =
                definition.DefinitionId;

            inspect.onClick.AddListener(
                () =>
                {
                    selectedId = id;
                    RefreshDetails();
                });
        }

        private void RefreshDetails()
        {
            if (detailName == null)
                return;

            if (string.IsNullOrWhiteSpace(selectedId))
            {
                detailName.text = "SELECT UNIT";
                detailCategory.text = "";
                detailPrice.text = "";
                detailStatus.text = "";
                detailDeployment.text = "";
                detailMaintenance.text = "";
                detailOperations.text = "";
                detailDescription.text =
                    "Select a unit from the procurement catalog to inspect specifications and ownership status.";
                purchaseButton.interactable = false;
                return;
            }

            InventoryItemDefinition definition =
                unitDefinitions.FirstOrDefault(
                    x =>
                        x.DefinitionId ==
                        selectedId);

            if (definition == null)
                return;

            UnitDefinition unit =
                UnitDatabaseRuntime.Get(
                    selectedId);

            int price =
                unit != null
                    ? Convert.ToInt32(
                        unit.Economy.creditCost)
                    : 0;

            bool owned =
                InventoryRuntime.Has(
                    selectedId);

            detailName.text =
                definition.DisplayName;

            detailCategory.text =
                "CATEGORY       " +
                Category(selectedId);

            detailPrice.text =
                "PROCUREMENT    " +
                price.ToString("N0") +
                " CREDITS";

            detailStatus.text =
                "OWNERSHIP      " +
                (owned
                    ? "ACQUIRED"
                    : "AVAILABLE");

            detailDeployment.text =
                "DEPLOYMENT     " +
                (
                    unit != null
                        ? Convert.ToInt32(
                            unit.Economy.deploymentPoints)
                        : 0
                ).ToString("N0") +
                " DP";

            detailMaintenance.text =
                "MAINTENANCE    " +
                Convert.ToInt32(
                    unit != null
                        ? unit.Economy.maintenanceCost
                        : 0) +
                " / CYCLE";

            detailOperations.text =
                "OPERATIONS     " +
                Convert.ToInt32(
                    unit != null
                        ? unit.Economy.operationalCost
                        : 0) +
                " / OPERATION";

            detailDescription.text =
                "PROCUREMENT ACCESS\n\n" +
                "Ownership places this unit in the persistent military inventory. " +
                "Battlefield deployment remains governed by mission requirements, " +
                "command doctrine, logistics and the active Deployment Budget.";

            purchaseButton.interactable =
                !owned &&
                EconomyRuntime.CampaignActive &&
                EconomyRuntime.Credits >= price;
        }

        private void OpenConfirmation()
        {
            if (confirmation == null ||
                string.IsNullOrWhiteSpace(selectedId))
                return;

            UnitDefinition unit =
                UnitDatabaseRuntime.Get(selectedId);

            if (unit == null)
                return;

            int price =
                Convert.ToInt32(
                    unit.Economy.creditCost);

            confirmationText.text =
                unit.DisplayName +
                "\n\nPROCUREMENT COST    " +
                price.ToString("N0") +
                " CREDITS\n\n" +
                "CURRENT BALANCE    " +
                EconomyRuntime.Credits.ToString("N0") +
                " CREDITS\n" +
                "BALANCE AFTER      " +
                Math.Max(
                    0,
                    EconomyRuntime.Credits - price)
                    .ToString("N0") +
                " CREDITS";

            confirmation.SetActive(true);
        }

        private void CompletePurchase()
        {
            confirmation.SetActive(false);

            if (string.IsNullOrWhiteSpace(selectedId))
                return;

            UnitPurchaseService service =
                new UnitPurchaseService();

            bool success =
                service.Purchase(selectedId);

            if (success)
            {
                LoadDefinitions();
                RefreshEverything();
            }
        }

        private void CloseConfirmation()
        {
            if (confirmation != null)
                confirmation.SetActive(false);
        }

        private string Category(string id)
        {
            string value =
                id.ToUpperInvariant();

            if (
                value.Contains("ARCHIVE") ||
                value.Contains("COMMAND") ||
                value.Contains("FUSION") ||
                value.Contains("INSIGHT") ||
                value.Contains("NEXUS") ||
                value.Contains("PULSE") ||
                value.Contains("VECTOR") ||
                value.Contains("WORLDMAP"))
                return "COMMAND";

            if (
                value.Contains("WARDEN") ||
                value.Contains("BEACON") ||
                value.Contains("DROP") ||
                value.Contains("IRIS") ||
                value.Contains("LIDAR") ||
                value.Contains("WATCHTOWER"))
                return "AIR";

            if (
                value.Contains("BULLDOG") ||
                value.Contains("FORGE") ||
                value.Contains("HAMMER") ||
                value.Contains("IRONWALKER") ||
                value.Contains("MULE") ||
                value.Contains("PATROL") ||
                value.Contains("ROVER") ||
                value.Contains("CRUSHER") ||
                value.Contains("HAULER"))
                return "GROUND";

            if (
                value.Contains("SURVEYOR") ||
                value.Contains("CURRENT") ||
                value.Contains("RESCUE") ||
                value.Contains("SONAR") ||
                value.Contains("HARBOR") ||
                value.Contains("TIDEBREAKER") ||
                value.Contains("DEPTHWATCH"))
                return "SEA";

            if (
                value.Contains("ECHO") ||
                value.Contains("HELIX") ||
                value.Contains("NULLPOINT") ||
                value.Contains("PHANTOM") ||
                value.Contains("SHADOWGRID") ||
                value.Contains("SPECTER"))
                return "EXPERIMENTAL";

            if (value.Contains("SATELLITE"))
                return "SATELLITE";

            return "OTHER";
        }

        private RectTransform Panel(
            string name,
            Transform parent,
            Vector2 min,
            Vector2 max,
            Color color)
        {
            RectTransform rect =
                Rect(name, parent, min, max);

            Image image =
                rect.gameObject.AddComponent<Image>();

            image.color = color;

            return rect;
        }

        private TMP_Text Label(
            Transform parent,
            string name,
            string value,
            float size,
            TextAlignmentOptions alignment)
        {
            GameObject obj =
                new GameObject(
                    name,
                    typeof(RectTransform),
                    typeof(TextMeshProUGUI));

            obj.transform.SetParent(
                parent,
                false);

            TMP_Text text =
                obj.GetComponent<TMP_Text>();

            text.text = value;
            text.fontSize = size;
            text.alignment = alignment;
            text.enableAutoSizing = false;
            text.textWrappingMode =
                TextWrappingModes.Normal;
            text.overflowMode =
                TextOverflowModes.Ellipsis;

            text.color =
                new Color(
                    0.78f,
                    0.87f,
                    0.90f,
                    1f);

            return text;
        }

        private Button MakeButton(
            Transform parent,
            string textValue,
            float size,
            float height)
        {
            GameObject obj =
                new GameObject(
                    textValue,
                    typeof(RectTransform),
                    typeof(Image),
                    typeof(Button));

            obj.transform.SetParent(
                parent,
                false);

            Image image =
                obj.GetComponent<Image>();

            image.color =
                new Color(
                    0.045f,
                    0.075f,
                    0.085f,
                    1f);

            Button button =
                obj.GetComponent<Button>();

            ColorBlock colors =
                button.colors;

            colors.normalColor =
                new Color(
                    0.045f,
                    0.075f,
                    0.085f,
                    1f);

            colors.highlightedColor =
                new Color(
                    0.085f,
                    0.14f,
                    0.15f,
                    1f);

            colors.pressedColor =
                new Color(
                    0.12f,
                    0.18f,
                    0.19f,
                    1f);

            colors.disabledColor =
                new Color(
                    0.025f,
                    0.035f,
                    0.040f,
                    1f);

            button.colors = colors;

            TMP_Text label =
                Label(
                    obj.transform,
                    "LABEL",
                    textValue,
                    size,
                    TextAlignmentOptions.Center);

            SetRect(
                label.rectTransform,
                0f,
                0f,
                1f,
                1f);

            return button;
        }

        private RectTransform Rect(
            string name,
            Transform parent,
            Vector2 min,
            Vector2 max)
        {
            GameObject obj =
                new GameObject(
                    name,
                    typeof(RectTransform));

            obj.transform.SetParent(
                parent,
                false);

            RectTransform rect =
                obj.GetComponent<RectTransform>();

            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            return rect;
        }

        private void SetRect(
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

