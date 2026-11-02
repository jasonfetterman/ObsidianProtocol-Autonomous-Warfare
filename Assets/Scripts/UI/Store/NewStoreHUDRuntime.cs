using System.Text;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Store;
using ObsidianProtocol.Game.Units;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.UI.Store;

public sealed class NewStoreHUDRuntime : MonoBehaviour
{
    private const int CardsPerPage = 12;

    private readonly List<UnitDefinition> units = new List<UnitDefinition>();
    private readonly List<UnitDefinition> filteredUnits = new List<UnitDefinition>();
    private readonly List<Button> infoButtons = new List<Button>();

    private UnitDefinition selected;
    private Transform root;
    private Material featherMaterial;
    private int currentPage;
    private Button previousButton;
    private Button nextButton;
    private TMP_Text pageText;

    private void Start()
    {
        Debug.Log("[STORE DIAGNOSTIC] InventoryActive=" +
            InventoryRuntime.CampaignActive +
            " | InventoryCampaign=" + InventoryRuntime.CampaignId +
            " | EconomyActive=" + EconomyRuntime.CampaignActive);
        root = transform;

        Shader featherShader = Shader.Find("ObsidianProtocol/UI/FeatheredEdge");
        if (featherShader != null)
            featherMaterial = new Material(featherShader);
        else
            Debug.LogWarning("Feathered-edge shader was not found.", this);

        EnsureTechnicalRecordUI();
        LoadUnits();
        CreateNavigationControls();
        ConfigureCategoryButtons();
        filteredUnits.Clear();
        filteredUnits.AddRange(units);
        PopulateCards();

        SelectUnit(units.FirstOrDefault());
    }

    private void LoadUnits()
    {
        units.Clear();

        foreach (InventoryItemDefinition item in InventoryDefinitionRegistry.GetAll())
        {
            if (item == null || item.Type != InventoryItemType.Unit)
                continue;

            UnitDefinition unit = UnitDatabaseRuntime.Get(item.DefinitionId);
            if (unit != null && !units.Any(u => u.UnitId == unit.UnitId))
                units.Add(unit);
        }

        units.Sort((a, b) =>
            string.Compare(a.DisplayName, b.DisplayName, StringComparison.OrdinalIgnoreCase));
    }

    private void PopulateCards()
    {
        infoButtons.Clear();

        for (int i = 0; i < CardsPerPage; i++)
        {
            Transform card = root.Find("ASSET_CATALOG/ASSET_CARD_" + (i + 1));
            if (card == null)
                continue;

            int unitIndex = currentPage * CardsPerPage + i;
            UnitDefinition unit = unitIndex < filteredUnits.Count ? filteredUnits[unitIndex] : null;

            SetImage(card, "ART_FRAME", unit != null ? unit.StoreImage : null);
            SetPlaceholderVisibility(card, "ART_STATUS",
                unit == null || unit.StoreImage == null);

            SetText(card, "UNIT_NAME", unit != null ? unit.DisplayName : "NO ASSET");
            SetText(card, "UNIT_ROLE",
                unit != null ? unit.Category.ToString().ToUpperInvariant() : "");
            SetText(card, "UNIT_COST", unit != null
                ? StoreUnitPricingCatalog.GetPrice(unit).ToString("N0") + " CR"
                : "");

            Button cardButton = card.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.RemoveAllListeners();
                cardButton.interactable = false;
            }

            Button infoButton = GetOrCreateInfoButton(card);
            infoButtons.Add(infoButton);

            infoButton.onClick.RemoveAllListeners();
            infoButton.interactable = unit != null;

            if (unit != null)
            {
                UnitDefinition captured = unit;
                infoButton.onClick.AddListener(() => SelectUnit(captured));
            }
        }

        int totalPages = Mathf.Max(1, Mathf.CeilToInt(filteredUnits.Count / (float)CardsPerPage));

        SetText(root, "ASSET_CATALOG/CATALOG_COUNT",
            "UNIT DATABASE / " + filteredUnits.Count + " ASSETS");

        if (pageText != null)
            pageText.text = "PAGE " + (currentPage + 1) + " / " + totalPages;

        if (previousButton != null)
            previousButton.interactable = currentPage > 0;

        if (nextButton != null)
            nextButton.interactable = currentPage + 1 < totalPages;
    }

    private Button GetOrCreateInfoButton(Transform card)
    {
        Transform existing = card.Find("INFO_BUTTON");
        Button button;

        if (existing != null)
        {
            button = existing.GetComponent<Button>();
            if (button == null)
                button = existing.gameObject.AddComponent<Button>();
        }
        else
        {
            GameObject buttonObject = new GameObject(
                "INFO_BUTTON", typeof(RectTransform), typeof(CanvasRenderer),
                typeof(Image), typeof(Button));

            buttonObject.layer = card.gameObject.layer;
            buttonObject.transform.SetParent(card, false);

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = new Vector2(-6f, 6f);
            rect.sizeDelta = new Vector2(70f, 24f);

            Image background = buttonObject.GetComponent<Image>();
            background.color = new Color(0.08f, 0.35f, 0.43f, 1f);
            background.raycastTarget = true;

            button = buttonObject.GetComponent<Button>();
            button.targetGraphic = background;
            button.transition = Selectable.Transition.ColorTint;

            GameObject labelObject = new GameObject(
                "LABEL", typeof(RectTransform), typeof(CanvasRenderer),
                typeof(TextMeshProUGUI));

            labelObject.layer = card.gameObject.layer;
            labelObject.transform.SetParent(buttonObject.transform, false);

            RectTransform labelRect = labelObject.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();
            label.text = "INFO";
            label.alignment = TextAlignmentOptions.Center;
            label.fontSize = 14f;
            label.fontStyle = FontStyles.Bold;
            label.color = Color.white;
            label.raycastTarget = false;
        }

        button.gameObject.SetActive(true);
        return button;
    }

    private void CreateNavigationControls()
    {
        Transform catalog = root.Find("ASSET_CATALOG");
        if (catalog == null)
        {
            Debug.LogError("ASSET_CATALOG was not found under the runtime HUD root.", this);
            return;
        }

        previousButton = CreateNavigationButton(
            catalog, "CATALOG_PREV", "PREVIOUS",
            new Vector2(0f, 0f), new Vector2(0f, 0f),
            new Vector2(65f, 6f));

        nextButton = CreateNavigationButton(
            catalog, "CATALOG_NEXT", "NEXT",
            new Vector2(1f, 0f), new Vector2(1f, 0f),
            new Vector2(-65f, 6f));

        Transform existingCount = catalog.Find("CATALOG_COUNT");
        if (existingCount != null)
            pageText = CreateOrGetPageLabel(catalog, existingCount);
    }

    private Button CreateNavigationButton(
        Transform parent, string objectName, string label,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 position)
    {
        Transform existing = parent.Find(objectName);
        if (existing != null)
        {
            Button existingButton = existing.GetComponent<Button>();
            if (existingButton != null)
            {
                existingButton.onClick.RemoveAllListeners();
                existingButton.onClick.AddListener(
                    objectName == "CATALOG_PREV" ? PreviousPage : NextPage);
                return existingButton;
            }
        }

        GameObject buttonObject = new GameObject(
            objectName, typeof(RectTransform), typeof(CanvasRenderer),
            typeof(Image), typeof(Button));

        buttonObject.layer = parent.gameObject.layer;
        buttonObject.transform.SetParent(parent, false);

        RectTransform rect = buttonObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(anchorMin.x, 0f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(110f, 30f);

        Image background = buttonObject.GetComponent<Image>();
        background.color = new Color(0.08f, 0.35f, 0.43f, 1f);

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = background;
        button.onClick.AddListener(
            objectName == "CATALOG_PREV" ? PreviousPage : NextPage);

        GameObject labelObject = new GameObject(
            "LABEL", typeof(RectTransform), typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));

        labelObject.layer = parent.gameObject.layer;
        labelObject.transform.SetParent(buttonObject.transform, false);

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        TextMeshProUGUI labelText = labelObject.GetComponent<TextMeshProUGUI>();
        labelText.text = label;
        labelText.alignment = TextAlignmentOptions.Center;
        labelText.fontSize = 13f;
        labelText.fontStyle = FontStyles.Bold;
        labelText.color = Color.white;
        labelText.raycastTarget = false;

        return button;
    }

    private TMP_Text CreateOrGetPageLabel(Transform catalog, Transform countTransform)
    {
        Transform existing = catalog.Find("CATALOG_PAGE_COUNT");
        if (existing != null)
        {
            RectTransform existingRect = existing.GetComponent<RectTransform>();
            if (existingRect != null)
            {
                existingRect.anchorMin = new Vector2(0.5f, 0f);
                existingRect.anchorMax = new Vector2(0.5f, 0f);
                existingRect.pivot = new Vector2(0.5f, 0f);
                existingRect.anchoredPosition = new Vector2(0f, 6f);
                existingRect.sizeDelta = new Vector2(200f, 42f);
            }

            TMP_Text existingText = existing.GetComponent<TMP_Text>();
            if (existingText != null)
                return existingText;
        }
        GameObject labelObject = new GameObject(
            "CATALOG_PAGE_COUNT", typeof(RectTransform), typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));

        labelObject.layer = catalog.gameObject.layer;
        labelObject.transform.SetParent(catalog, false);

        RectTransform rect = labelObject.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.anchoredPosition = new Vector2(0f, 6f);
        rect.sizeDelta = new Vector2(200f, 42f);

        TextMeshProUGUI label = labelObject.GetComponent<TextMeshProUGUI>();
        label.alignment = TextAlignmentOptions.Center;
        label.fontSize = 13f;
        label.color = Color.white;
        label.raycastTarget = false;
        return label;
    }

    private Transform FindCategorySlot(string objectName)
    {
        return GetComponentsInChildren<Transform>(true)
            .FirstOrDefault(t => t != transform && t.name == objectName);
    }
    private void ConfigureCategoryButtons()
    {
        ConfigureAllAssetsButton("CATEGORY_0", "ALL ASSETS");
        ConfigureCategoryButton("CATEGORY_1", "AIR UNIT", UnitCategory.AirUnit);
        ConfigureCategoryButton("CATEGORY_2", "GROUND UNIT", UnitCategory.GroundUnit);
        ConfigureCategoryButton("CATEGORY_3", "FIGHTER", UnitCategory.Fighter);
        ConfigureCategoryButton("CATEGORY_4", "STATION", UnitCategory.Station);
        ConfigureCategoryButton("CATEGORY_5", "SATELLITE", UnitCategory.Satellite);
        ConfigureCategoryButton("CATEGORY_6", "NAVAL", UnitCategory.Naval);
        ConfigureCategoryButton("CATEGORY_7", "TRANSPORT", UnitCategory.Transport);
        ConfigureCategoryButton("CATEGORY_8", "RECON", UnitCategory.Recon);
        ConfigureCategoryButton("CATEGORY_9", "SHUTTLE", UnitCategory.Shuttle);
        ConfigureCategoryButton("CATEGORY_10", "MEDICAL", UnitCategory.Medical);
        ConfigureCategoryButton("CATEGORY_11", "EXPLORATION", UnitCategory.Exploration);
    }

    private void ConfigureAllAssetsButton(string objectName, string label)
    {
        Transform slot = FindCategorySlot(objectName);
        if (slot == null)
        {
            Debug.LogWarning("Store category slot was not found: " + objectName, this);
            return;
        }

        Button button = slot.GetComponent<Button>();
        TMP_Text caption = slot.GetComponentInChildren<TMP_Text>(true);

        if (caption != null)
            caption.text = label;

        if (button == null)
        {
            Debug.LogWarning("Store category slot has no Button: " + objectName, this);
            return;
        }

        button.onClick.RemoveAllListeners();
        button.interactable = true;
        button.onClick.AddListener(ApplyAllAssetsFilter);
        ConfigureStoreButtonClickSound(button);
    }

    private void ApplyAllAssetsFilter()
    {
        filteredUnits.Clear();
        filteredUnits.AddRange(units);
        currentPage = 0;
        PopulateCards();

        if (filteredUnits.Count > 0)
            SelectUnit(filteredUnits[0]);
    }
    private void ConfigureCategoryButton(
        string objectName, string label, UnitCategory category)
    {
        Transform slot = FindCategorySlot(objectName);
        if (slot == null)
        {
            Debug.LogWarning("Store category slot was not found: " + objectName, this);
            return;
        }

        Button button = slot.GetComponent<Button>();
        TMP_Text caption = slot.GetComponentInChildren<TMP_Text>(true);

        if (caption != null)
            caption.text = label;

        if (button == null)
        {
            Debug.LogWarning("Store category slot has no Button: " + objectName, this);
            return;
        }

        button.onClick.RemoveAllListeners();
        button.interactable = true;
        button.onClick.AddListener(() => ApplyCategoryFilter(category));
        ConfigureStoreButtonClickSound(button);
    }

    private void DisableCategoryButton(string objectName)
    {
        Transform slot = FindCategorySlot(objectName);
        if (slot == null)
            return;

        Button button = slot.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.interactable = false;
        }

        TMP_Text caption = slot.GetComponentInChildren<TMP_Text>(true);
        if (caption != null)
            caption.text = "";
    }

    private void ApplyCategoryFilter(UnitCategory category)
    {
        filteredUnits.Clear();
        filteredUnits.AddRange(
            units.Where(unit => unit != null && unit.Category == category));

        currentPage = 0;
        PopulateCards();

        if (filteredUnits.Count > 0)
        {
            SelectUnit(filteredUnits[0]);
            return;
        }

        selected = null;

        Transform purchaseTransform = root.Find("UNIT_DOSSIER/PURCHASE_BUTTON");
        if (purchaseTransform != null)
        {
            Button purchaseButton = purchaseTransform.GetComponent<Button>();
            if (purchaseButton != null)
            {
                purchaseButton.onClick.RemoveAllListeners();
                purchaseButton.interactable = false;
            }
        }

        SetText(root, "UNIT_DOSSIER/DOSSIER_NOTICE", "NO ASSETS IN THIS CATEGORY");
        SetText(root, "UNIT_DOSSIER/DOSSIER_UNIT_NAME", "NO ASSET");
        SetText(root, "UNIT_DOSSIER/DOSSIER_ROLE", category.ToString().ToUpperInvariant());
        SetText(root, "UNIT_DOSSIER/DOSSIER_ID", "UNIT ID   ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â");
        SetText(root, "UNIT_DOSSIER/DOSSIER_SUMMARY", "NO DATABASE-REGISTERED ASSETS MATCH THIS CATEGORY.");
        
SetText(root, "UNIT_DOSSIER/PURCHASE_PRICE", "ACQUISITION COST / ÃƒÆ’Ã†â€™Ãƒâ€ Ã¢â‚¬â„¢ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â‚¬Å¡Ã‚Â¬Ãƒâ€¦Ã‚Â¡ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã†â€™Ãƒâ€šÃ‚Â¢ÃƒÆ’Ã‚Â¢ÃƒÂ¢Ã¢â€šÂ¬Ã…Â¡Ãƒâ€šÃ‚Â¬ÃƒÆ’Ã¢â‚¬Å¡Ãƒâ€šÃ‚Â");
        SetImage(root, "UNIT_DOSSIER/DOSSIER_ART", null);
        SetPlaceholderVisibility(root, "UNIT_DOSSIER/DOSSIER_ART_HINT", true);
    }
    private void PreviousPage()
    {
        if (currentPage <= 0)
            return;

        currentPage--;
        PopulateCards();
    }

    private void NextPage()
    {
        int totalPages = Mathf.Max(1, Mathf.CeilToInt(filteredUnits.Count / (float)CardsPerPage));
        if (currentPage + 1 >= totalPages)
            return;

        currentPage++;
        PopulateCards();
    }

    private void SelectUnit(UnitDefinition unit)
    {
        if (unit == null)
            return;

        selected = unit;

        SetImage(root, "UNIT_DOSSIER/DOSSIER_ART", unit.StoreImage);
        SetPlaceholderVisibility(root, "UNIT_DOSSIER/DOSSIER_ART_HINT", unit.StoreImage == null);
Transform artHint = root.Find("UNIT_DOSSIER/DOSSIER_ART_HINT");
        if (artHint != null)
            artHint.SetAsLastSibling();

        SetText(root, "UNIT_DOSSIER/DOSSIER_UNIT_NAME", unit.DisplayName.ToUpperInvariant());
        SetText(root, "UNIT_DOSSIER/DOSSIER_ROLE", unit.Category.ToString().ToUpperInvariant());
        SetText(root, "UNIT_DOSSIER/DOSSIER_ID", "UNIT ID   " + unit.UnitId);
        SetText(root, "UNIT_DOSSIER/DOSSIER_SUMMARY",
            "DATABASE-REGISTERED " + unit.Category.ToString().ToUpperInvariant() +
            " PLATFORM. Review specifications and procurement eligibility before deployment.");
        
        RenderTechnicalRecord(unit);
SetText(root, "UNIT_DOSSIER/PURCHASE_PRICE",
            "ACQUISITION COST / " + StoreUnitPricingCatalog.GetPrice(unit).ToString("N0") + " CREDITS");

        Transform buttonTransform = root.Find("UNIT_DOSSIER/PURCHASE_BUTTON");
        if (buttonTransform != null)
        {
            Button button = buttonTransform.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(PurchaseSelected);
                ConfigureStoreButtonClickSound(button);
                button.interactable = !InventoryRuntime.Has(unit.UnitId);
            }
        }

        SetText(root, "UNIT_DOSSIER/DOSSIER_NOTICE",
            InventoryRuntime.Has(unit.UnitId) ? "ALREADY OWNED" : "PROCUREMENT READY");
    }

    private void RenderTechnicalRecord(UnitDefinition unit)
    {
        Transform contentTransform =
            root.Find("UNIT_DOSSIER/DOSSIER_TECHNICAL_SCROLL/VIEWPORT/CONTENT");

        if (contentTransform == null)
        {
            Debug.LogError(
                "[STORE DOSSIER] Technical record CONTENT was not found.",
                this);
            return;
        }

        TMP_Text output = contentTransform.GetComponent<TMP_Text>();
        if (output == null)
        {
            Debug.LogError(
                "[STORE DOSSIER] Technical record CONTENT has no TMP_Text.",
                this);
            return;
        }

        StringBuilder record = new StringBuilder(4096);

        AppendRecordSection(record, "IDENTIFICATION", unit);
        AppendRecordSection(record, "PHYSICAL / TECHNICAL", unit.Physical);
        AppendRecordSection(record, "AUTONOMY / AI", unit.Autonomy);
        AppendRecordSection(record, "SENSORS", unit.Sensors);
        AppendRecordSection(record, "COMMUNICATIONS", unit.Communication);
        AppendRecordSection(record, "COMBAT / THREAT", unit.Combat);
        AppendRecordSection(record, "SQUAD / COMMAND", unit.Squad);
        AppendRecordSection(record, "VR OPERATOR", unit.VROperator);
        AppendRecordSection(record, "ECONOMY / LOGISTICS", unit.Economy);
        AppendRecordSection(record, "BUILD REQUIREMENTS", unit.BuildRequirements);
        AppendRecordSection(record, "ENVIRONMENT", unit.Environment);
        AppendRecordSection(record, "NETWORK DYNAMICS", unit.Network);
        AppendRecordSection(record, "COGNITIVE DATA", unit.Cognitive);
        AppendRecordSection(record, "BEHAVIOR NODES", unit.BehaviorNodes);
        AppendRecordSection(record, "MISSION BEHAVIORS", unit.MissionBehaviors);
        AppendRecordSection(record, "UNIT SYNERGIES", unit.UnitSynergies);
        AppendRecordSection(record, "DAMAGE RESPONSES", unit.DamageResponses);
        AppendRecordSection(record, "UPGRADE EFFECTS", unit.Upgrades);
        AppendRecordSection(record, "TACTICAL INTEGRATION", unit.Tactical);
        AppendRecordSection(record, "MAINTENANCE ANALYTICS", unit.Maintenance);
        AppendRecordSection(record, "THREAT RESPONSES", unit.ThreatResponses);
        AppendRecordSection(record, "COMMAND NETWORK", unit.CommandNetwork);
        AppendRecordSection(record, "PERFORMANCE METRICS", unit.Performance);

        if (record.Length == 0)
            record.Append("NO CONFIGURED TECHNICAL DATA.");

        output.fontSize = Mathf.Max(output.fontSize, 18f);
        output.text = record.ToString();
        output.fontSize = 11.5f;
        output.color = new Color(0.68f, 0.78f, 0.84f, 1f);
        output.lineSpacing = 2f;

        ScrollRect scroll = contentTransform.GetComponentInParent<ScrollRect>();
        if (scroll != null)
            scroll.verticalNormalizedPosition = 1f;
    }

    private void AppendRecordSection(
        StringBuilder output,
        string title,
        object value)
    {
        if (value == null)
            return;

        int originalLength = output.Length;
        StringBuilder section = new StringBuilder(512);

        AppendRecordValue(section, value, 0);

        if (section.Length == 0)
            return;

        if (output.Length > 0)
            output.AppendLine().AppendLine();

        output.Append("<color=#55DDF5><b>")
            .Append(title)
            .AppendLine("</b></color>");
        output.AppendLine("<color=#35566A>------------------------------------------------</color>");
        output.Append(section);
    }

    private void AppendRecordValue(
        StringBuilder output,
        object value,
        int depth)
    {
        if (value == null || depth > 4)
            return;

        if (value is string textValue)
        {
            if (!string.IsNullOrWhiteSpace(textValue))
                output.AppendLine(textValue);
            return;
        }

        if (value is IEnumerable sequence)
        {
            int index = 0;

            foreach (object item in sequence)
            {
                if (item == null)
                    continue;

                if (item is string itemText)
                {
                    if (!string.IsNullOrWhiteSpace(itemText))
                    {
                        output.Append("  - ")
                            .AppendLine(itemText);
                    }
                }
                else
                {
                    StringBuilder itemContent = new StringBuilder(256);
                    AppendRecordValue(itemContent, item, depth + 1);

                    if (itemContent.Length > 0)
                    {
                        output.Append("  [")
                            .Append(++index)
                            .AppendLine("]");
                        output.Append(itemContent);
                    }
                }
            }

            return;
        }

        Type type = value.GetType();

        if (type.IsEnum)
        {
            output.AppendLine(value.ToString());
            return;
        }

        if (value is bool boolValue)
        {
            output.AppendLine(boolValue ? "Enabled: YES" : "Enabled: NO");
            return;
        }

        if (type.IsPrimitive || value is decimal)
        {
            double number;

            try
            {
                number = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                return;
            }

            // Default numeric zero values are omitted because the schema
            // does not distinguish untouched defaults from intentional zeroes.
            if (Math.Abs(number) < 0.000001d)
                return;

            output.AppendLine(number.ToString("0.##", CultureInfo.InvariantCulture));
            return;
        }

        FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            if (field.IsStatic)
                continue;

            object fieldValue = field.GetValue(value);
            if (fieldValue == null)
                continue;

            if (fieldValue is string fieldText)
            {
                if (string.IsNullOrWhiteSpace(fieldText))
                    continue;

                output.Append(FormatRecordFieldName(field.Name))
                    .Append(": ")
                    .AppendLine(fieldText);
                continue;
            }

            if (fieldValue is bool fieldBool)
            {
                output.Append(FormatRecordFieldName(field.Name))
                    .Append(": ")
                    .AppendLine(fieldBool ? "YES" : "NO");
                continue;
            }

            if (fieldValue is IEnumerable fieldSequence)
            {
                StringBuilder collectionContent = new StringBuilder(256);
                AppendRecordValue(collectionContent, fieldSequence, depth + 1);

                if (collectionContent.Length > 0)
                {
                    output.AppendLine(FormatRecordFieldName(field.Name));
                    output.Append(collectionContent);
                }

                continue;
            }

            Type fieldType = fieldValue.GetType();

            if (fieldType.IsEnum)
            {
                output.Append(FormatRecordFieldName(field.Name))
                    .Append(": ")
                    .AppendLine(fieldValue.ToString());
                continue;
            }

            if (fieldType.IsPrimitive || fieldValue is decimal)
            {
                double number;

                try
                {
                    number = Convert.ToDouble(
                        fieldValue,
                        CultureInfo.InvariantCulture);
                }
                catch
                {
                    continue;
                }

                if (Math.Abs(number) < 0.000001d)
                    continue;

                output.Append(FormatRecordFieldName(field.Name))
                    .Append(": ")
                    .AppendLine(number.ToString(
                        "0.##",
                        CultureInfo.InvariantCulture));
                continue;
            }

            StringBuilder nestedContent = new StringBuilder(256);
            AppendRecordValue(nestedContent, fieldValue, depth + 1);

            if (nestedContent.Length > 0)
            {
                output.Append("<color=#91B9CB><b>")
                    .Append(FormatRecordFieldName(field.Name))
                    .AppendLine("</b></color>");
                output.Append(nestedContent);
            }
        }
    }

    private string FormatRecordFieldName(string fieldName)
    {
        if (string.IsNullOrEmpty(fieldName))
            return string.Empty;

        StringBuilder formatted = new StringBuilder(fieldName.Length + 8);

        for (int i = 0; i < fieldName.Length; i++)
        {
            char character = fieldName[i];

            if (i > 0 && char.IsUpper(character) &&
                (char.IsLower(fieldName[i - 1]) ||
                 (i + 1 < fieldName.Length && char.IsLower(fieldName[i + 1]))))
            {
                formatted.Append(' ');
            }

            formatted.Append(character);
        }

        return formatted.ToString().ToUpperInvariant();
    }
    private void PurchaseSelected()
    {
        if (selected == null)
            return;

        bool success = new UnitPurchaseService().Purchase(selected.UnitId);

        SetText(root, "UNIT_DOSSIER/DOSSIER_NOTICE",
            success ? "PROCUREMENT COMPLETE" :
            "PROCUREMENT FAILED / CHECK CAMPAIGN, CREDITS AND OWNERSHIP");

        PopulateCards();
        SelectUnit(selected);
    }

    private void SetPlaceholderVisibility(Transform parent, string path, bool visible)
    {
        Transform target = parent.Find(path);
        if (target != null)
            target.gameObject.SetActive(visible);
    }

    private void SetImage(Transform parent, string path, Sprite sprite)
    {
        Transform target = parent.Find(path);
        if (target == null)
            return;

        Image image = target.Find("ART_IMAGE") != null
            ? target.Find("ART_IMAGE").GetComponent<Image>()
            : target.GetComponent<Image>();

        if (image == null)
            return;

        image.sprite = sprite;
        image.color = Color.white;
        image.type = Image.Type.Simple;
        image.preserveAspect = true;
        image.enabled = sprite != null;
        image.raycastTarget = false;

        if (featherMaterial != null)
            image.material = featherMaterial;

        AspectRatioFitter fitter = image.GetComponent<AspectRatioFitter>();
        if (fitter != null && sprite != null)
        {
            fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
            fitter.aspectRatio = sprite.rect.width / sprite.rect.height;
        }
    }

    private void EnsureTechnicalRecordUI()
    {
        Transform dossier = root.Find("UNIT_DOSSIER");
        if (dossier == null)
        {
            Debug.LogError("[STORE DOSSIER] UNIT_DOSSIER was not found.", this);
            return;
        }

        Transform existing = dossier.Find("DOSSIER_TECHNICAL_SCROLL");
        if (existing != null &&
            existing.Find("VIEWPORT/CONTENT") != null &&
            existing.Find("VIEWPORT/CONTENT").GetComponent<TMP_Text>() != null)
        {
            return;
        }

        if (existing != null)
            Destroy(existing.gameObject);

        GameObject scrollObject = new GameObject(
            "DOSSIER_TECHNICAL_SCROLL",
            typeof(RectTransform),
            typeof(ScrollRect),
            typeof(Image));

        scrollObject.layer = dossier.gameObject.layer;
        scrollObject.transform.SetParent(dossier, false);

        RectTransform scrollRectTransform = scrollObject.GetComponent<RectTransform>();
        scrollRectTransform.anchorMin = new Vector2(0.04f, 0.105f);
        scrollRectTransform.anchorMax = new Vector2(0.96f, 0.285f);
        scrollRectTransform.offsetMin = Vector2.zero;
        scrollRectTransform.offsetMax = Vector2.zero;
        scrollRectTransform.localScale = Vector3.one;

        Image background = scrollObject.GetComponent<Image>();
        background.color = new Color(0.012f, 0.022f, 0.035f, 0.97f);
        background.raycastTarget = true;

        GameObject viewportObject = new GameObject(
            "VIEWPORT",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(Image),
            typeof(RectMask2D));

        viewportObject.layer = dossier.gameObject.layer;
        viewportObject.transform.SetParent(scrollObject.transform, false);

        RectTransform viewportRect = viewportObject.GetComponent<RectTransform>();
        viewportRect.anchorMin = Vector2.zero;
        viewportRect.anchorMax = Vector2.one;
        viewportRect.offsetMin = new Vector2(8f, 6f);
        viewportRect.offsetMax = new Vector2(-8f, -6f);
        viewportRect.localScale = Vector3.one;

        Image viewportImage = viewportObject.GetComponent<Image>();
        viewportImage.color = new Color(0.018f, 0.032f, 0.048f, 1f);
        viewportImage.raycastTarget = false;

        GameObject contentObject = new GameObject(
            "CONTENT",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(TextMeshProUGUI),
            typeof(ContentSizeFitter));

        contentObject.layer = dossier.gameObject.layer;
        contentObject.transform.SetParent(viewportObject.transform, false);

        RectTransform contentRect = contentObject.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0f, 1f);
        contentRect.anchorMax = new Vector2(1f, 1f);
        contentRect.pivot = new Vector2(0.5f, 1f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(0f, 0f);
        contentRect.localScale = Vector3.one;

        ContentSizeFitter contentFitter = contentObject.GetComponent<ContentSizeFitter>();
        contentFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        TextMeshProUGUI output = contentObject.GetComponent<TextMeshProUGUI>();
        output.text = "LOADING TECHNICAL RECORD...";
        output.fontSize = 11.5f;
        output.color = new Color(0.68f, 0.78f, 0.84f, 1f);
        output.alignment = TextAlignmentOptions.TopLeft;
        output.textWrappingMode = TextWrappingModes.Normal;
        output.overflowMode = TextOverflowModes.Overflow;
        output.richText = true;
        output.raycastTarget = false;
        output.margin = new Vector4(8f, 8f, 8f, 8f);

        ScrollRect scroll = scrollObject.GetComponent<ScrollRect>();
        scroll.viewport = viewportRect;
        scroll.content = contentRect;
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = 30f;
        scroll.inertia = true;
    }

    private void SetText(Transform parent, string path, string value)
    {
        Transform target = parent.Find(path);
        if (target == null)
            return;

        TMP_Text text = target.GetComponent<TMP_Text>();
        if (text != null)
        {
            text.text = value;
        }
    }

    private void OnDestroy()
    {
        if (featherMaterial != null)
        {
            if (Application.isPlaying)
                Destroy(featherMaterial);
            else
                DestroyImmediate(featherMaterial);
        }
    }

    private void PlayStoreButtonClickSound()
    {
        if (UIAudioFeedbackSystem.Instance != null)
            UIAudioFeedbackSystem.Instance.PlayClick();
    }

    private void ConfigureStoreButtonClickSound(Button button)
    {
        if (button == null)
            return;

        button.onClick.RemoveListener(PlayStoreButtonClickSound);
        button.onClick.AddListener(PlayStoreButtonClickSound);
    }
}


















