using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Store;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Store.UI
{
    public sealed class StoreCatalogueCard : MonoBehaviour
    {
        public string UnitId;
        public string Category;
    }

    public sealed class StoreCatalogueInteractionController : MonoBehaviour
    {
        [SerializeField] private GameObject detailsPanel;
        [SerializeField] private Image unitArtwork;
        [SerializeField] private TMP_Text unitName;
        [SerializeField] private TMP_Text unitIdentity;
        [SerializeField] private TMP_Text specificationText;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text creditsText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button closeButton;

        private readonly UnitPurchaseService purchaseService = new UnitPurchaseService();
        private string selectedUnitId;

        private void Awake()
        {
            if (purchaseButton != null) purchaseButton.onClick.AddListener(PurchaseSelectedUnit);
            if (closeButton != null) closeButton.onClick.AddListener(CloseDetails);

            foreach (StoreCatalogueCard card in GetComponentsInChildren<StoreCatalogueCard>(true))
            {
                Button button = card.GetComponent<Button>();
                if (button == null || string.IsNullOrWhiteSpace(card.UnitId)) continue;
                string id = card.UnitId;
                button.onClick.AddListener(() => SelectUnit(id));
            }

            foreach (StoreCategoryButton item in GetComponentsInChildren<StoreCategoryButton>(true))
            {
                Button button = item.GetComponent<Button>();
                if (button == null) continue;
                string category = item.Category;
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => ShowCategory(category));
            }
            if (detailsPanel != null) detailsPanel.SetActive(false);
            RefreshCredits();
        }

        private void Update()
        {
            RefreshCredits();
            if (purchaseButton == null) return;

            bool owned = !string.IsNullOrEmpty(selectedUnitId) &&
                InventoryRuntime.CampaignActive && InventoryRuntime.Has(selectedUnitId);

            purchaseButton.interactable = !string.IsNullOrEmpty(selectedUnitId) &&
                EconomyRuntime.CampaignActive && !owned;
        }

        public void ShowCategory(string category)
        {
            bool showAll = string.IsNullOrWhiteSpace(category) ||
                category.Equals("ALL", StringComparison.OrdinalIgnoreCase);

            foreach (StoreCatalogueCard card in GetComponentsInChildren<StoreCatalogueCard>(true))
            {
                bool match = showAll || string.Equals(
                    card.Category, category, StringComparison.OrdinalIgnoreCase);
                card.gameObject.SetActive(match);
            }

            foreach (StoreCategoryButton item in GetComponentsInChildren<StoreCategoryButton>(true))
            {
                var feedback = item.GetComponent<StoreNeonButtonFeedback>();
                if (feedback != null)
                    feedback.SetSelected(string.Equals(item.Category, category, StringComparison.OrdinalIgnoreCase));
            }
        }

        public void SelectUnit(string unitId)
        {
            if (!UnitDatabaseRuntime.EnsureInitialized())
            {
                SetStatus("UNIT DATABASE UNAVAILABLE");
                return;
            }

            UnitDefinition unit = UnitDatabaseRuntime.Get(unitId);
            if (unit == null)
            {
                SetStatus("UNIT DEFINITION NOT FOUND");
                return;
            }

            selectedUnitId = unit.UnitId;
            foreach (StoreCatalogueCard item in GetComponentsInChildren<StoreCatalogueCard>(true))
            {
                var feedback = item.GetComponent<StoreNeonButtonFeedback>();
                if (feedback != null) feedback.SetSelected(item.UnitId == selectedUnitId);
            }
            if (detailsPanel != null)
            {
                detailsPanel.SetActive(true);
                detailsPanel.transform.SetAsLastSibling();
            }
            if (unitName != null) unitName.text = unit.DisplayName;

            if (unitIdentity != null)
                unitIdentity.text = unit.Category.ToString().ToUpperInvariant() +
                    "  //  " + unit.Role + "  //  " + unit.Tier + "  //  " + unit.Status;

            if (unitArtwork != null)
            {
                unitArtwork.sprite = unit.StoreImage;
                unitArtwork.enabled = unit.StoreImage != null;
                unitArtwork.preserveAspect = true;
            }

            if (specificationText != null) specificationText.text = FormatSpecification(unit);

            int cost = unit.Economy != null ? unit.Economy.creditCost : 0;
            if (priceText != null)
                priceText.text = "PROCUREMENT COST  //  " + cost.ToString("N0") + " CREDITS";

            bool owned = InventoryRuntime.CampaignActive && InventoryRuntime.Has(unit.UnitId);
            SetStatus(owned ? "UNIT ALREADY REGISTERED IN YOUR INVENTORY" :
                "UNIT SELECTED // REVIEW SPECIFICATIONS");
            RefreshCredits();
        }

        private string FormatSpecification(UnitDefinition unit)
        {
            StringBuilder b = new StringBuilder();
            var p = unit.Physical;
            var a = unit.Autonomy;
            var s = unit.Sensors;
            var c = unit.Combat;
            var n = unit.Communication;
            var e = unit.Economy;

            AddSection(b, "PHYSICAL PROFILE");
            if (p != null)
            {
                AddLine(b, "Dimensions", p.lengthMeters > 0 || p.widthMeters > 0 || p.heightMeters > 0
                    ? p.lengthMeters.ToString("0.##") + " x " + p.widthMeters.ToString("0.##") +
                      " x " + p.heightMeters.ToString("0.##") + " m" : null);
                AddLine(b, "Mass", p.massKg > 0 ? p.massKg.ToString("N0") + " kg" : null);
                AddLine(b, "Power", p.powerSource);
                AddLine(b, "Endurance", p.endurance);
                AddLine(b, "Payload", p.payloadCapacityKg > 0 ? p.payloadCapacityKg.ToString("0.##") + " kg" : null);
                AddLine(b, "Top speed", p.topSpeedDescription);
                AddLine(b, "Navigation", p.navigationSystem);
            }

            AddSection(b, "AUTONOMY & AI");
            if (a != null)
            {
                AddLine(b, "Behavior profile", a.behaviorProfile);
                AddLine(b, "Target priority", a.targetPriority);
                AddLine(b, "Formation discipline", a.formationDiscipline);
                AddLine(b, "Retreat behavior", a.retreatBehaviorDescription);
            }

            AddSection(b, "SENSORS");
            if (s != null)
            {
                AddLine(b, "Visual range", s.visualRangeDescription);
                AddLine(b, "Thermal range", s.thermalRangeDescription);
                AddLine(b, "Radar", s.radarRange);
                AddLine(b, "Lidar", s.lidarRange);
                AddLine(b, "Signal detection", s.signalDetectionRange);
            }

            AddSection(b, "COMBAT & THREAT");
            if (c != null)
            {
                AddLine(b, "Firepower", c.firepowerDescription);
                AddLine(b, "Weapon systems", c.weaponTypes);
                AddLine(b, "Weapon mounts", c.weaponMounts);
                AddLine(b, "Engagement distance", c.engagementDistance);
                AddLine(b, "Threat level", c.threatLevel > 0 ? c.threatLevel.ToString() : null);
                AddLine(b, "Countermeasures", c.countermeasureRating);
            }

            AddSection(b, "COMMUNICATIONS");
            if (n != null)
            {
                AddLine(b, "Command link", n.commandLinkType);
                AddLine(b, "Mesh range", n.meshRangeDescription);
                AddLine(b, "Network priority", n.networkPriority);
            }

            AddSection(b, "LOGISTICS");
            if (e != null)
            {
                AddLine(b, "Build time", e.buildTimeSeconds > 0 ? e.buildTimeSeconds.ToString("0.#") + " sec" : null);
                AddLine(b, "Factory", e.factoryType);
                AddLine(b, "Maintenance", e.maintenanceCost);
                AddLine(b, "Deployment", e.deploymentRequirements);
            }

            return b.Length == 0 ? "NO SPECIFICATIONS AVAILABLE" : b.ToString().TrimEnd();
        }

        private void AddSection(StringBuilder b, string title)
        {
            if (b.Length > 0) b.AppendLine();
            b.AppendLine("— " + title + " —");
        }

        private void AddLine(StringBuilder b, string label, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                b.AppendLine(label + "  //  " + value);
        }

        public void PurchaseSelectedUnit()
        {
            if (string.IsNullOrWhiteSpace(selectedUnitId))
            {
                SetStatus("SELECT A UNIT FIRST");
                return;
            }
            if (!EconomyRuntime.CampaignActive)
            {
                SetStatus("CAMPAIGN REQUIRED FOR PROCUREMENT");
                return;
            }
            if (InventoryRuntime.CampaignActive && InventoryRuntime.Has(selectedUnitId))
            {
                SetStatus("UNIT ALREADY OWNED");
                return;
            }

            bool success = purchaseService.Purchase(selectedUnitId);
            SetStatus(success ? "PROCUREMENT COMPLETE // UNIT ADDED TO FLEET" :
                "PROCUREMENT FAILED // CHECK CAMPAIGN, CREDITS AND REGISTRATION");
            RefreshCredits();
        }

        public void CloseDetails()
        {
            if (detailsPanel != null) detailsPanel.SetActive(false);
            selectedUnitId = null;
        }
        private void RefreshCredits()
        {
            if (creditsText != null)
                creditsText.text = EconomyRuntime.CampaignActive
                    ? "AVAILABLE CREDITS  //  " + EconomyRuntime.Credits.ToString("N0")
                    : "CAMPAIGN NOT ACTIVE";
        }

        private void SetStatus(string message)
        {
            if (statusText != null) statusText.text = message;
        }
    }
}