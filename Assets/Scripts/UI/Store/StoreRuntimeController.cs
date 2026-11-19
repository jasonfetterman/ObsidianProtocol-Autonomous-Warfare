using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Core;
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
        private Image detailArtwork;

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
            RestoreCampaign();
            BuildStore();
        }

        private void RestoreCampaign()
        {
            if (!SaveLoadService.Exists())
            {
                Debug.LogWarning("[STORE] No campaign save exists. Purchases are unavailable.");
                return;
            }

            SaveData save = SaveLoadService.Load();

            if (save == null ||
                string.IsNullOrWhiteSpace(save.CampaignId) ||
                string.IsNullOrWhiteSpace(save.PlayerId))
            {
                Debug.LogError("[STORE] Campaign save is invalid. Purchases are unavailable.");
                return;
            }

            InventoryRuntime.Initialize(save.CampaignId);
            EconomyRuntime.Load(save.CampaignId);

            if (!EconomyRuntime.CampaignActive)
            {
                EconomyRuntime.StartNewCampaign(
                    save.CampaignId,
                    save.Funding,
                    save.PlayerId);
            }

            Debug.Log("[STORE] Existing campaign restored: " + save.CampaignId);
        }

        private void BuildStore()
        {
            canvas = GetComponent<Canvas>();

            if (canvas == null)
                canvas = GetComponentInParent<Canvas>();

            if (canvas == null)
                return;

            Transform storePanel =
                transform;

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
            RectTransform rail = Panel("CATEGORY_NAVIGATION", root,
                new Vector2(0.015f, 0.835f), new Vector2(0.985f, 0.895f),
                new Color(.008f,.022f,.038f,1f));

            HorizontalLayoutGroup layout = rail.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(8,8,3,3);
            layout.spacing = 8f;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = true;

            categoryButtons.Clear();

            foreach (string category in Categories)
            {
                GameObject tile = new GameObject(category,
                    typeof(RectTransform), typeof(Image), typeof(Button), typeof(Outline));
                tile.transform.SetParent(rail, false);

                Image image = tile.GetComponent<Image>();
                image.sprite = GetStoreChamferSprite();
                image.type = Image.Type.Sliced;
                image.color = new Color(.025f,.075f,.11f,.98f);

                Outline outline = tile.GetComponent<Outline>();
                outline.effectColor = new Color(0f,.72f,1f,1f);
                outline.effectDistance = new Vector2(1.2f,1.2f);

                Button button = tile.GetComponent<Button>();
                button.targetGraphic = image;
                categoryButtons[category] = button;

                LayoutElement element = tile.AddComponent<LayoutElement>();
                element.flexibleWidth = 1f;
                element.minWidth = 70f;

                TMP_Text heading = Label(tile.transform, "CATEGORY", category,
                    10f, TextAlignmentOptions.Center);
                heading.fontStyle = FontStyles.Bold;
                heading.color = new Color(0f,.75f,1f,1f);
                SetRect(heading.rectTransform,.04f,.48f,.96f,.92f);

                int count = category == "ALL" ? unitDefinitions.Count :
                    unitDefinitions.Count(x => Category(x.DefinitionId) == category);

                TMP_Text total = Label(tile.transform, "COUNT", count.ToString("D2"),
                    20f, TextAlignmentOptions.Center);
                total.fontStyle = FontStyles.Bold;
                total.color = new Color(.82f,.91f,1f,1f);
                SetRect(total.rectTransform,.04f,.04f,.96f,.57f);

                string captured = category;
                button.onClick.AddListener(() =>
                {
                    selectedCategory = captured;
                    RefreshCategories();
                    RefreshCatalog();
                });
            }

            RefreshCategories();
        }

        private void BuildCatalog()
        {
            RectTransform panel = Panel(
                "FULL_WIDTH_UNIT_CATALOG", root,
                new Vector2(0.015f, 0.325f),
                new Vector2(0.985f, 0.825f),
                new Color(0.014f, 0.025f, 0.033f, 1f));

            TMP_Text title = Label(
                panel, "TITLE", "AVAILABLE MILITARY ASSETS",
                15f, TextAlignmentOptions.Left);
            title.fontStyle = FontStyles.Bold;
            SetRect(title.rectTransform, 0.018f, 0.925f, 0.55f, 0.99f);

            catalogStatus = Label(
                panel, "STATUS", "", 10f, TextAlignmentOptions.Right);
            SetRect(catalogStatus.rectTransform, 0.72f, 0.925f, 0.98f, 0.99f);

            RectTransform viewport = Rect(
                "CATALOG_VIEWPORT", panel,
                new Vector2(0.012f, 0.025f),
                new Vector2(0.988f, 0.90f));

            Image viewportImage = viewport.gameObject.AddComponent<Image>();
            viewportImage.color = new Color(0f, 0f, 0f, 0.01f);
            Mask mask = viewport.gameObject.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            cardContainer = Rect(
                "CATALOG_CONTENT", viewport,
                new Vector2(0f, 1f),
                new Vector2(1f, 1f));
            cardContainer.pivot = new Vector2(0.5f, 1f);

            VerticalLayoutGroup layout = cardContainer.gameObject.AddComponent<VerticalLayoutGroup>();
layout.spacing = 8f;
layout.padding = new RectOffset(8, 8, 8, 8);
layout.childAlignment = TextAnchor.UpperCenter;
layout.childControlWidth = true;
layout.childControlHeight = false;
layout.childForceExpandWidth = true;
layout.childForceExpandHeight = false;

            ContentSizeFitter fitter = cardContainer.gameObject.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            ScrollRect scroll = panel.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.content = cardContainer;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 35f;
        }

        private void BuildDetails()
        {
            RectTransform panel = Panel(
                "UNIT_INSPECTION_DOCK", root,
                new Vector2(0.015f, 0.075f),
                new Vector2(0.985f, 0.31f),
                new Color(0.022f, 0.038f, 0.047f, 1f));

            detailArtwork = null;
            GameObject artworkObject = new GameObject(
                "SELECTED_UNIT_ARTWORK",
                typeof(RectTransform), typeof(Image));
            artworkObject.transform.SetParent(panel, false);
            detailArtwork = artworkObject.GetComponent<Image>();
            detailArtwork.preserveAspect = true;
            detailArtwork.color = new Color(0.035f, 0.065f, 0.075f, 1f);
            detailArtwork.raycastTarget = false;
            SetRect(detailArtwork.rectTransform, 0.015f, 0.08f, 0.19f, 0.92f);

            TMP_Text heading = Label(
                panel, "INSPECTION_HEADING", "SELECTED ASSET",
                9f, TextAlignmentOptions.Left);
            heading.color = new Color(0.25f, 0.78f, 0.86f, 1f);
            SetRect(heading.rectTransform, 0.215f, 0.84f, 0.56f, 0.97f);

            detailName = Label(
                panel, "NAME", "SELECT UNIT",
                20f, TextAlignmentOptions.Left);
            detailName.fontStyle = FontStyles.Bold;
            SetRect(detailName.rectTransform, 0.215f, 0.68f, 0.58f, 0.85f);

            detailDescription = Label(
                panel, "DESCRIPTION",
                "Select an asset to inspect procurement specifications and ownership.",
                9f, TextAlignmentOptions.Left);
            detailDescription.alignment = TextAlignmentOptions.TopLeft;
            detailDescription.overflowMode = TextOverflowModes.Ellipsis;
            SetRect(detailDescription.rectTransform, 0.215f, 0.12f, 0.58f, 0.63f);

            detailCategory = DetailLine(panel, "CATEGORY", "", 0.79f);
            SetRect(detailCategory.rectTransform, 0.62f, 0.78f, 0.79f, 0.94f);

            detailPrice = DetailLine(panel, "PRICE", "", 0.55f);
            SetRect(detailPrice.rectTransform, 0.62f, 0.54f, 0.79f, 0.70f);

            detailStatus = DetailLine(panel, "STATUS", "", 0.31f);
            SetRect(detailStatus.rectTransform, 0.62f, 0.30f, 0.79f, 0.46f);

            detailDeployment = DetailLine(panel, "DEPLOYMENT", "", 0.79f);
            SetRect(detailDeployment.rectTransform, 0.80f, 0.78f, 0.985f, 0.94f);

            detailMaintenance = DetailLine(panel, "MAINTENANCE", "", 0.55f);
            SetRect(detailMaintenance.rectTransform, 0.80f, 0.54f, 0.985f, 0.70f);

            detailOperations = DetailLine(panel, "OPERATIONS", "", 0.31f);
            SetRect(detailOperations.rectTransform, 0.80f, 0.30f, 0.985f, 0.46f);

            purchaseButton = MakeButton(
                panel, "AUTHORIZE PROCUREMENT", 10f, 38f);
            SetRect(
                purchaseButton.GetComponent<RectTransform>(),
                0.62f, 0.07f, 0.985f, 0.23f);
            purchaseButton.onClick.AddListener(OpenConfirmation);
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
            RectTransform bar = Panel(
                "SYSTEM_STATUS_BAR", root,
                new Vector2(0f, 0f),
                new Vector2(1f, 0.055f),
                new Color(0.010f, 0.018f, 0.024f, 1f));

            TMP_Text text = Label(
                bar, "SYSTEM_STATUS",
                "OBSIDIAN PROTOCOL     //     PROCUREMENT NETWORK     //     OWNERSHIP AND DEPLOYMENT ARE SEPARATE SYSTEMS",
                9f, TextAlignmentOptions.Center);
            SetRect(text.rectTransform, 0.02f, 0.15f, 0.98f, 0.85f);
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
            foreach (KeyValuePair<string, Button> item in categoryButtons)
            {
                bool selected = item.Key == selectedCategory;
                item.Value.interactable = !selected;

                Image image = item.Value.GetComponent<Image>();
                if (image != null)
                    image.color = selected
                        ? new Color(.025f,.105f,.12f,1f)
                        : new Color(.025f,.075f,.11f,.98f);

                Outline outline = item.Value.GetComponent<Outline>();
                if (outline != null)
                    outline.effectColor = selected
                        ? new Color(0f,.85f,1f,1f)
                        : new Color(0f,.72f,1f,1f);

                TMP_Text total = item.Value.transform.Find("COUNT")?.GetComponent<TMP_Text>();
                if (total != null)
                {
                    int count = item.Key == "ALL" ? unitDefinitions.Count :
                        unitDefinitions.Count(x => Category(x.DefinitionId) == item.Key);
                    total.text = count.ToString("D2");
                }
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

        private static Sprite GetStoreChamferSprite()
        {
            const int size = 64;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                bool inside = x+y >= 13 && (size-1-x)+y >= 13 &&
                    x+(size-1-y) >= 13 && (size-1-x)+(size-1-y) >= 13;
                texture.SetPixel(x, y, inside ? Color.white : Color.clear);
            }
            texture.Apply();
            return Sprite.Create(texture, new Rect(0,0,size,size),
                new Vector2(.5f,.5f), 100, 0, SpriteMeshType.FullRect,
                new Vector4(14,14,14,14));
        }
        private void CreateCard(InventoryItemDefinition definition)
        {
            UnitDefinition unit = UnitDatabaseRuntime.Get(definition.DefinitionId);
            bool owned = InventoryRuntime.Has(definition.DefinitionId);
            bool selected = selectedId == definition.DefinitionId;

            GameObject card = new GameObject(
                "CARD_" + definition.DefinitionId,
                typeof(RectTransform),
                typeof(Image),
                typeof(Button),
                typeof(Outline));

            card.transform.SetParent(cardContainer, false);

            Image background = card.GetComponent<Image>();
            background.sprite = GetStoreChamferSprite();
            background.type = Image.Type.Sliced;
            background.color = selected
                ? new Color(0.035f, 0.105f, 0.145f, 1f)
                : new Color(0.025f, 0.075f, 0.11f, 0.98f);

            Button cardButton = card.GetComponent<Button>();
            cardButton.targetGraphic = background;
            cardButton.transition = Selectable.Transition.ColorTint;

            ColorBlock cardColors = cardButton.colors;
            cardColors.normalColor = Color.white;
            cardColors.highlightedColor = new Color(0.42f, 0.95f, 1f, 1f);
            cardColors.pressedColor = new Color(0.18f, 0.75f, 0.85f, 1f);
            cardColors.selectedColor = new Color(0.35f, 0.9f, 1f, 1f);
            cardColors.fadeDuration = 0.08f;
            cardButton.colors = cardColors;

            Outline outline = card.GetComponent<Outline>();
            outline.effectColor = selected
                ? new Color(0.05f, 0.9f, 1f, 0.95f)
                : new Color(0f, 0.72f, 1f, 1f);
            outline.effectDistance = new Vector2(1.5f, -1.5f);

            RectTransform rect = card.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, 112f);

            GameObject artObject = new GameObject(
                "UNIT_ARTWORK",
                typeof(RectTransform),
                typeof(Image));
            artObject.transform.SetParent(card.transform, false);

            Image art = artObject.GetComponent<Image>();
            art.sprite = unit != null ? unit.StoreImage : null;
            art.preserveAspect = true;
            art.color = art.sprite != null ? Color.white : new Color(0.035f, 0.065f, 0.075f, 1f);
            SetRect(art.rectTransform, 0.015f, 0.08f, 0.22f, 0.92f);

            TMP_Text name = Label(
                card.transform, "NAME", definition.DisplayName,
                14f, TextAlignmentOptions.Left);
            name.fontStyle = FontStyles.Bold;
            name.color = new Color(0.88f, 0.97f, 1f, 1f);
            SetRect(name.rectTransform, 0.245f, 0.48f, 0.64f, 0.88f);

            TMP_Text category = Label(
                card.transform, "CATEGORY",
                Category(definition.DefinitionId).ToUpperInvariant(),
                9f, TextAlignmentOptions.Left);
            category.color = new Color(0.25f, 0.78f, 0.86f, 1f);
            SetRect(category.rectTransform, 0.245f, 0.12f, 0.64f, 0.42f);

            TMP_Text state = Label(
                card.transform, "STATE",
                owned ? "OWNED" : "AVAILABLE",
                8f, TextAlignmentOptions.Right);
            state.color = owned
                ? new Color(0.35f, 0.9f, 0.65f, 1f)
                : new Color(0.35f, 0.78f, 0.9f, 1f);
            SetRect(state.rectTransform, 0.68f, 0.62f, 0.96f, 0.88f);

            int price = unit != null
                ? Convert.ToInt32(unit.Economy.creditCost)
                : 0;

            TMP_Text priceText = Label(
                card.transform, "PRICE",
                price.ToString("N0") + " CR",
                12f, TextAlignmentOptions.Left);
            priceText.fontStyle = FontStyles.Bold;
            priceText.color = new Color(0.88f, 0.95f, 0.97f, 1f);
            SetRect(priceText.rectTransform, 0.68f, 0.32f, 0.96f, 0.56f);

            Button inspect = MakeButton(
                card.transform,
                owned ? "INSPECT" : "SELECT",
                9f,
                28f);
            SetRect(
                inspect.GetComponent<RectTransform>(),
                0.68f, 0.08f, 0.96f, 0.26f);

            string id = definition.DefinitionId;
            UnityEngine.Events.UnityAction selectUnit = () =>
            {
                selectedId = id;
                RefreshDetails();
                RefreshCatalog();
            };

            cardButton.onClick.AddListener(selectUnit);
            inspect.onClick.AddListener(selectUnit);
            cardButton.onClick.AddListener(
                () => UIAudioFeedbackSystem.Instance?.PlayClick());
        }
        private static string FormatEconomyValue(object value)
    {
        if (value == null) return "0";

        string raw = Convert.ToString(
            value,
            System.Globalization.CultureInfo.InvariantCulture);

        decimal number;
        if (decimal.TryParse(raw,
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out number))
            return number.ToString("N0");

        if (string.IsNullOrWhiteSpace(raw)) return "0";
        return raw;
    }
            private static string BuildUnitDescription(UnitDefinition unit)
        {
            if (unit == null)
                return "UNIT DATABASE RECORD UNAVAILABLE.";

            var sections = new System.Collections.Generic.List<string>();

            AddBriefingSection(sections, "OPERATIONAL ROLE",
                JoinBriefing(unit.Role, unit.Tier, unit.Status));

            if (unit.Physical != null)
                AddBriefingSection(sections, "PLATFORM & MOBILITY",
                    JoinBriefing(unit.Physical.powerSource,
                        unit.Physical.topSpeedDescription,
                        unit.Physical.endurance,
                        unit.Physical.navigationSystem,
                        unit.Physical.payloadCapacityKg > 0f
                            ? "Payload capacity: " + unit.Physical.payloadCapacityKg.ToString("0.##") + " kg"
                            : null));

            if (unit.Sensors != null)
                AddBriefingSection(sections, "SENSOR & RECONNAISSANCE SYSTEMS",
                    JoinBriefing(unit.Sensors.visualRangeDescription,
                        unit.Sensors.thermalRangeDescription,
                        unit.Sensors.acousticRange,
                        unit.Sensors.radarRange,
                        unit.Sensors.lidarRange,
                        unit.Sensors.signalDetectionRange));

            if (unit.Communication != null)
                AddBriefingSection(sections, "NETWORK & COMMUNICATIONS",
                    JoinBriefing(unit.Communication.meshRangeDescription,
                        unit.Communication.commandLinkType,
                        unit.Communication.networkPriority));

            if (unit.Combat != null)
                AddBriefingSection(sections, "THREAT & ENGAGEMENT PROFILE",
                    JoinBriefing(unit.Combat.firepowerDescription,
                        unit.Combat.weaponTypes,
                        unit.Combat.engagementDistance,
                        unit.Combat.preferredTargets,
                        unit.Combat.preferredTerrain,
                        unit.Combat.vulnerabilityProfile));

            if (unit.Autonomy != null)
                AddBriefingSection(sections, "AUTONOMOUS BEHAVIOR",
                    JoinBriefing(unit.Autonomy.behaviorProfile,
                        unit.Autonomy.targetPriority,
                        unit.Autonomy.formationDiscipline,
                        unit.Autonomy.retreatBehaviorDescription));

            if (unit.MissionBehaviors != null && unit.MissionBehaviors.Count > 0)
            {
                var missions = new System.Collections.Generic.List<string>();
                foreach (MissionBehavior item in unit.MissionBehaviors)
                    if (item != null)
                    {
                        string entry = JoinBriefing(item.missionType, item.behaviorFocus);
                        if (!string.IsNullOrWhiteSpace(entry))
                            missions.Add("- " + entry);
                    }
                AddBriefingSection(sections, "MISSION DOCTRINE", string.Join("\n", missions));
            }

            if (unit.BehaviorNodes != null && unit.BehaviorNodes.Count > 0)
            {
                var behaviors = new System.Collections.Generic.List<string>();
                foreach (BehaviorNode item in unit.BehaviorNodes)
                    if (item != null)
                    {
                        string entry = JoinBriefing(item.behaviorNode, item.description);
                        if (!string.IsNullOrWhiteSpace(entry))
                            behaviors.Add("- " + entry);
                    }
                AddBriefingSection(sections, "EXECUTION PROFILE", string.Join("\n", behaviors));
            }

            return sections.Count > 0
                ? string.Join("\n\n", sections)
                : "No operational briefing data is populated for this asset.";
        }

        private static string JoinBriefing(params string[] values)
        {
            var parts = new System.Collections.Generic.List<string>();
            foreach (string value in values)
                if (!string.IsNullOrWhiteSpace(value))
                    parts.Add(value.Trim());
            return string.Join("  |  ", parts);
        }

        private static void AddBriefingSection(
            System.Collections.Generic.List<string> sections,
            string heading,
            string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
                sections.Add(heading + "\n" + content.Trim());
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
                FormatEconomyValue(unit != null ? unit.Economy.maintenanceCost : 0) +
                " / CYCLE";

            detailOperations.text =
                "OPERATIONS     " +
                FormatEconomyValue(unit != null ? unit.Economy.operationalCost : 0) +
                " / OPERATION";

            detailDescription.text = BuildUnitDescription(unit);

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

                private static string Category(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return "UNCLASSIFIED";

            id = id.Trim().ToLowerInvariant();

            if (id.StartsWith("obs-air-")) return "AIR";
            if (id.StartsWith("obs-ground-")) return "GROUND";
            if (id.StartsWith("obs-sea-")) return "SEA";
            if (id.StartsWith("obs-command-")) return "COMMAND";
            if (id.StartsWith("obs-experimental-")) return "EXPERIMENTAL";
            if (id.StartsWith("obs-sat-")) return "SATELLITE";

            return "UNCLASSIFIED";
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
            button.onClick.AddListener(
                () => UIAudioFeedbackSystem.Instance?.PlayClick());

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

