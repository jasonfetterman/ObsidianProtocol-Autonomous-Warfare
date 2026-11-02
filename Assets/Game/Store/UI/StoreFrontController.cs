using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Economy;

namespace ObsidianProtocol.Game.Store.UI
{
    public sealed class StoreFrontController : MonoBehaviour
    {
        [Header("Store UI")]
        [SerializeField] private GameObject storeRoot;
        [SerializeField] private Text creditBalanceText;
        [SerializeField] private Text statusText;
        [SerializeField] private Text selectedUnitText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private StoreUnitCatalogBuilder catalogBuilder;
        [SerializeField] private Transform unitListContent;

        private readonly UnitPurchaseService purchaseService =
            new UnitPurchaseService();

        private string selectedUnitId = string.Empty;

        private void Awake()
        {
            if (purchaseButton != null)
                purchaseButton.onClick.AddListener(PurchaseSelectedUnit);

            RefreshCredits();
            SetStatus("Select a unit to begin.");
        }

        private void OnDestroy()
        {
            if (purchaseButton != null)
                purchaseButton.onClick.RemoveListener(PurchaseSelectedUnit);
        }

        private void Update()
        {
            RefreshCredits();

            if (purchaseButton != null)
                purchaseButton.interactable =
                    EconomyRuntime.CampaignActive &&
                    !string.IsNullOrWhiteSpace(selectedUnitId);
        }

        public void RefreshUnitList()
        {
            if (unitListContent == null)
            {
                SetStatus("Unit list container is not assigned.");
                return;
            }

            if (catalogBuilder == null || catalogBuilder.Catalog == null)
            {
                if (!RefreshCatalog())
                    return;
            }

            foreach (Transform child in unitListContent)
                Destroy(child.gameObject);

            foreach (UnitCatalogEntry entry in catalogBuilder.Catalog.GetEntries())
            {
                if (entry == null || !entry.Valid)
                    continue;

                GameObject buttonObject = new GameObject(
                    "Unit_" + entry.CatalogId,
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Image),
                    typeof(Button));

                buttonObject.transform.SetParent(unitListContent, false);

                RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
                buttonRect.sizeDelta = new Vector2(0f, 36f);

                Image buttonImage = buttonObject.GetComponent<Image>();
                buttonImage.color = entry.Available
                    ? new Color(0.16f, 0.22f, 0.28f, 1f)
                    : new Color(0.22f, 0.18f, 0.18f, 1f);

                Button unitButton = buttonObject.GetComponent<Button>();

                GameObject labelObject = new GameObject(
                    "Label",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(Text));

                labelObject.transform.SetParent(buttonObject.transform, false);

                RectTransform labelRect = labelObject.GetComponent<RectTransform>();
                labelRect.anchorMin = Vector2.zero;
                labelRect.anchorMax = Vector2.one;
                labelRect.offsetMin = new Vector2(10f, 2f);
                labelRect.offsetMax = new Vector2(-10f, -2f);

                Text label = labelObject.GetComponent<Text>();
                label.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
                label.fontSize = 14;
                label.alignment = TextAnchor.MiddleLeft;
                label.color = Color.white;
                label.text = entry.DisplayName + "  —  " +
                    entry.CreditCost.ToString("N0") + " credits";

                string unitId = entry.UnitId;
                string displayName = entry.DisplayName;

                unitButton.onClick.AddListener(
                    () => SelectUnit(unitId, displayName));
            }

            SetStatus("Unit list refreshed.");
        }
        public bool RefreshCatalog()
        {
            if (catalogBuilder == null)
            {
                SetStatus("Store catalog builder is not assigned.");
                return false;
            }

            bool built = catalogBuilder.BuildCatalog();

            if (built)
                RefreshUnitList();

            SetStatus(built ? "Catalog loaded." : "Catalog could not be loaded.");
            return built;
        }
public void ShowStore()
        {
            if (storeRoot != null)
                storeRoot.SetActive(true);

            RefreshCredits();
        }

        public void HideStore()
        {
            if (storeRoot != null)
                storeRoot.SetActive(false);
        }

        public void SelectUnit(string unitId, string displayName)
        {
            selectedUnitId = unitId ?? string.Empty;

            if (selectedUnitText != null)
                selectedUnitText.text =
                    string.IsNullOrWhiteSpace(displayName)
                        ? selectedUnitId
                        : displayName;

            SetStatus(
                string.IsNullOrWhiteSpace(selectedUnitId)
                    ? "No unit selected."
                    : "Selected: " + displayName);
        }

        public void ClearSelection()
        {
            selectedUnitId = string.Empty;

            if (selectedUnitText != null)
                selectedUnitText.text = "No unit selected.";

            SetStatus("Select a unit to begin.");
        }

        public void PurchaseSelectedUnit()
        {
            if (string.IsNullOrWhiteSpace(selectedUnitId))
            {
                SetStatus("Select a unit first.");
                return;
            }

            if (!EconomyRuntime.CampaignActive)
            {
                SetStatus("Start or load a campaign before purchasing.");
                return;
            }

            bool purchased = purchaseService.Purchase(selectedUnitId);

            SetStatus(
                purchased
                    ? "Purchase successful."
                    : "Purchase failed. Check credits, availability, and ownership.");

            RefreshCredits();
        }

        public void RefreshCredits()
        {
            if (creditBalanceText != null)
            {
                creditBalanceText.text = EconomyRuntime.CampaignActive
                    ? "CREDITS  " + EconomyRuntime.Credits.ToString("N0")
                    : "CREDITS  â€”";
            }
        }

        public void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message ?? string.Empty;
        }
    }
}