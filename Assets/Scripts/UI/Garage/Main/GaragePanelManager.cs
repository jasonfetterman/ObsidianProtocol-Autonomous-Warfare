using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaragePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;

    private readonly Dictionary<string, GameObject> panelMap = new Dictionary<string, GameObject>();

    private static readonly string[] PanelNames =
    {
        "FleetPanel",
        "StorePanel",
        "RepairPanel",
        "CustomizePanel",
        "EquipmentPanel",
        "UpgradePanel",
        "AICorePanel",
        "FabricationPanel",
        "StoragePanel",
        "SalvagePanel",
        "DeploymentPanel",
        "CommandCenterPanel"
    };

    private static readonly Dictionary<string, string> ButtonToPanel =
        new Dictionary<string, string>
        {
            { "BUTTON_FLEET", "FleetPanel" },
            { "BUTTON_STORE", "StorePanel" },
            { "BUTTON_REPAIR", "RepairPanel" },
            { "BUTTON_CUSTOMIZE", "CustomizePanel" },
            { "BUTTON_EQUIPMENT", "EquipmentPanel" },
            { "BUTTON_UPGRADE", "UpgradePanel" },
            { "BUTTON_AI_CORE", "AICorePanel" },
            { "BUTTON_FABRICATION", "FabricationPanel" },
            { "BUTTON_STORAGE", "StoragePanel" },
            { "BUTTON_SALVAGE", "SalvagePanel" },
            { "BUTTON_DEPLOYMENT", "DeploymentPanel" },
            { "BUTTON_COMMAND_CENTER", "CommandCenterPanel" }
        };

    private void Awake()
    {
        BuildPanelMap();
        WireNavigationButtons();
    }

    private void BuildPanelMap()
    {
        panelMap.Clear();

        if (panels != null)
        {
            foreach (GameObject panel in panels)
            {
                if (panel != null)
                    panelMap[panel.name] = panel;
            }
        }

        // Runtime fallback: find any missing named panels in the scene.
        foreach (string panelName in PanelNames)
        {
            if (!panelMap.ContainsKey(panelName))
            {
                GameObject found = GameObject.Find(panelName);

                if (found != null)
                    panelMap[panelName] = found;
            }
        }
    }

    private void WireNavigationButtons()
    {
        foreach (KeyValuePair<string, string> entry in ButtonToPanel)
        {
            GameObject buttonObject = GameObject.Find(entry.Key);

            if (buttonObject == null)
            {
                Debug.LogWarning(
                    "[GaragePanelManager] Missing button: " + entry.Key,
                    this
                );
                continue;
            }

            Button button = buttonObject.GetComponent<Button>();

            if (button == null)
            {
                Debug.LogWarning(
                    "[GaragePanelManager] No Button component on: " + entry.Key,
                    buttonObject
                );
                continue;
            }

            if (!panelMap.ContainsKey(entry.Value))
            {
                Debug.LogWarning(
                    "[GaragePanelManager] Missing panel: " + entry.Value,
                    this
                );
                continue;
            }

            GameObject targetPanel = panelMap[entry.Value];

            button.onClick.RemoveListener(
                delegate { ShowPanel(targetPanel); }
            );

            button.onClick.AddListener(
                delegate { ShowPanel(targetPanel); }
            );
        }
    }

    public void ShowPanel(GameObject panelToShow)
    {
        if (panelToShow == null)
        {
            Debug.LogWarning(
                "[GaragePanelManager] ShowPanel received a null panel.",
                this
            );
            return;
        }

        foreach (GameObject panel in panelMap.Values)
        {
            if (panel != null)
                panel.SetActive(panel == panelToShow);
        }

        panelToShow.SetActive(true);
    }

    public void CloseAllPanels()
    {
        foreach (GameObject panel in panelMap.Values)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
}
