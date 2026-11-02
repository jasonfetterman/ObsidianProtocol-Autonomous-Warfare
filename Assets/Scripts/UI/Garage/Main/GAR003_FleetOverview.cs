using UnityEngine;
using TMPro;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Runtime;

namespace ObsidianProtocol.UI.Garage
{
    /// <summary>
    /// GAR-003 - Fleet overview.
    /// Displays only units actually owned by the player.
    /// </summary>
    public class GAR003_FleetOverview : MonoBehaviour
    {
        public const string ID = "GAR-003";

        [Header("Garage UI")]
        [SerializeField] private Transform unitContainer;
        [SerializeField] private GameObject unitRowTemplate;

        private void Start()
        {
            if (unitContainer == null || unitRowTemplate == null)
                AutoFindReferences();

            PopulateFleet();
        }

        public void Initialize()
        {
            if (unitContainer == null || unitRowTemplate == null)
                AutoFindReferences();

            PopulateFleet();
            Debug.Log("[Garage UI] GAR-003 initialized.", this);
        }

        public void PopulateFleet()
        {
            if (unitContainer == null || unitRowTemplate == null)
                AutoFindReferences();

            if (unitContainer == null || unitRowTemplate == null)
            {
                Debug.LogWarning(
                    "[Garage UI] GAR-003 could not find GARAGE_UNITS or UnitRowTemplate.",
                    this
                );
                return;
            }

            ClearExistingRows();

            if (!InventoryRuntime.CampaignActive)
            {
                Debug.LogWarning(
                    "[Garage UI] No active campaign. Fleet list remains empty.",
                    this
                );
                return;
            }

            if (!UnitDatabaseRuntime.EnsureInitialized())
            {
                Debug.LogError(
                    "[Garage UI] Unit database could not be initialized.",
                    this
                );
                return;
            }

            var ownedUnits = InventoryRuntime.GetByType(
                InventoryItemType.Unit
            );

            int displayedUnits = 0;

            foreach (InventoryItem item in ownedUnits)
            {
                if (item == null || !item.IsValid)
                    continue;

                var definition = UnitDatabaseRuntime.Get(item.DefinitionId);

                string displayName = definition != null &&
                                     !string.IsNullOrWhiteSpace(definition.DisplayName)
                    ? definition.DisplayName
                    : item.DefinitionId;

                GameObject row = Instantiate(unitRowTemplate, unitContainer);
                row.name = "UNIT_ROW_" + item.InstanceId;

                TMP_Text nameText = FindText(row, "UnitNameText");
                TMP_Text statusText = FindText(row, "UnitStatusText");

                if (nameText != null)
                {
                    nameText.text = item.Quantity > 1
                        ? displayName + " x" + item.Quantity
                        : displayName;
                }

                if (statusText != null)
                    statusText.text = FormatState(item.State);

                row.SetActive(true);
                displayedUnits++;
            }

            Debug.Log(
                "[Garage UI] GAR-003 displayed " +
                displayedUnits +
                " owned unit entries.",
                this
            );
        }

        private void ClearExistingRows()
        {
            for (int i = unitContainer.childCount - 1; i >= 0; i--)
            {
                Transform child = unitContainer.GetChild(i);

                if (child.gameObject == unitRowTemplate)
                    continue;

                Destroy(child.gameObject);
            }
        }

        private void AutoFindReferences()
        {
            if (unitContainer == null)
            {
                GameObject units = GameObject.Find("GARAGE_UNITS");

                if (units != null)
                    unitContainer = units.transform;
            }

            if (unitRowTemplate == null)
            {
                GameObject template = GameObject.Find("UnitRowTemplate");

                if (template != null)
                    unitRowTemplate = template;
            }
        }

        private TMP_Text FindText(GameObject root, string objectName)
        {
            Transform[] children =
                root.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in children)
            {
                if (child.name != objectName)
                    continue;

                TMP_Text text = child.GetComponent<TMP_Text>();

                if (text != null)
                    return text;
            }

            return null;
        }

        private string FormatState(InventoryItemState state)
        {
            switch (state)
            {
                case InventoryItemState.Stored:
                    return "STORED";
                case InventoryItemState.Equipped:
                    return "EQUIPPED";
                case InventoryItemState.Configuring:
                    return "CONFIGURING";
                case InventoryItemState.Maintenance:
                    return "MAINTENANCE";
                case InventoryItemState.Damaged:
                    return "DAMAGED";
                case InventoryItemState.Deployed:
                    return "DEPLOYED";
                case InventoryItemState.Consumed:
                    return "CONSUMED";
                case InventoryItemState.Disabled:
                    return "DISABLED";
                case InventoryItemState.Salvaged:
                    return "SALVAGED";
                default:
                    return state.ToString().ToUpperInvariant();
            }
        }
    }
}
