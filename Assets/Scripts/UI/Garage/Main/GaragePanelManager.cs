using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GaragePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;

    private readonly Dictionary<string, GameObject> panelMap =
        new Dictionary<string, GameObject>();

    private readonly Dictionary<Button, UnityAction> wiredActions =
        new Dictionary<Button, UnityAction>();

    private static readonly string[] PanelNames =
    {
        "FleetPanel", "StorePanel", "RepairPanel", "CustomizePanel",
        "EquipmentPanel", "UpgradePanel", "AICorePanel", "FabricationPanel",
        "StoragePanel", "SalvagePanel", "DeploymentPanel", "CommandCenterPanel"
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
        Debug.Log("[GaragePanelManager] Runtime scene: name=" + gameObject.scene.name + ", path=" + gameObject.scene.path + ", loaded=" + gameObject.scene.isLoaded, this);
        Debug.Log("[GaragePanelManager DIAG] activeScene=" + SceneManager.GetActiveScene().name +
                  ", managerSceneValid=" + gameObject.scene.IsValid() +
                  ", managerSceneLoaded=" + gameObject.scene.isLoaded +
                  ", root=" + transform.root.name +
                  ", directChildren=" + transform.childCount +
                  ", navigationChild=" + (transform.Find("GARAGE_NAVIGATION") != null), this);
        BuildPanelMap();
        Transform nav = FindRecursive(transform, "GARAGE_NAVIGATION");
        Debug.Log("[GaragePanelManager NAVDIAG] localFleet=" +
                  (FindRecursive(transform, "BUTTON_FLEET") != null) +
                  ", navFound=" + (nav != null) +
                  ", navChildren=" + (nav != null ? nav.childCount : -1) +
                  ", sceneCount=" + SceneManager.sceneCount, this);
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

        foreach (string panelName in PanelNames)
        {
            if (!panelMap.ContainsKey(panelName))
            {
                GameObject found = FindSceneObject(panelName);
                if (found != null)
                    panelMap[panelName] = found;
            }

            if (!panelMap.ContainsKey(panelName))
                Debug.LogError("[GaragePanelManager] PANEL MISSING: " + panelName, this);
        }

        Debug.Log("[GaragePanelManager] Panels resolved: " +
                  panelMap.Count + " / " + PanelNames.Length, this);
    }

    private void WireNavigationButtons()
    {
        int wiredCount = 0;

        foreach (KeyValuePair<string, string> entry in ButtonToPanel)
        {
            GameObject buttonObject = FindSceneObject(entry.Key);

            if (buttonObject == null)
            {
                Debug.LogError("[GaragePanelManager] BUTTON MISSING: " + entry.Key, this);
                continue;
            }

            Button button = buttonObject.GetComponent<Button>();

            if (button == null)
            {
                Debug.LogError("[GaragePanelManager] BUTTON COMPONENT MISSING: " +
                               entry.Key, buttonObject);
                continue;
            }

            if (!panelMap.TryGetValue(entry.Value, out GameObject targetPanel) ||
                targetPanel == null)
            {
                Debug.LogError("[GaragePanelManager] TARGET PANEL MISSING: " +
                               entry.Value, buttonObject);
                continue;
            }

            if (wiredActions.TryGetValue(button, out UnityAction previousAction))
                button.onClick.RemoveListener(previousAction);

            GameObject capturedPanel = targetPanel;
            UnityAction action = () => ShowPanel(capturedPanel);

            wiredActions[button] = action;
            button.onClick.AddListener(action);
            wiredCount++;

            Debug.Log("[GaragePanelManager] WIRED: " + entry.Key +
                      " -> " + entry.Value, buttonObject);
        }

        Debug.Log("[GaragePanelManager] Navigation wired: " +
                  wiredCount + " / " + ButtonToPanel.Count, this);
    }

    private GameObject FindSceneObject(string objectName)
    {
        // Search this manager's hierarchy first. The runtime diagnostics
        // confirm the garage navigation buttons exist here.
        Transform localFound = FindRecursive(transform, objectName);
        if (localFound != null)
            return localFound.gameObject;

        // Fallback: search other loaded scenes.
        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
        {
            Scene scene = SceneManager.GetSceneAt(sceneIndex);
            if (!scene.IsValid() || !scene.isLoaded)
                continue;

            foreach (GameObject sceneRoot in scene.GetRootGameObjects())
            {
                if (sceneRoot == transform.root.gameObject)
                    continue;

                Transform found = FindRecursive(sceneRoot.transform, objectName);
                if (found != null)
                    return found.gameObject;
            }
        }

        return null;
    }
    private Transform FindRecursive(Transform parent, string objectName)
    {
        if (parent.name == objectName)
            return parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform found = FindRecursive(parent.GetChild(i), objectName);
            if (found != null)
                return found;
        }

        return null;
    }

    public void ShowPanel(GameObject panelToShow)
    {
        if (panelToShow == null)
        {
            Debug.LogError("[GaragePanelManager] Cannot open a null panel.", this);
            return;
        }

        foreach (GameObject panel in panelMap.Values)
        {
            if (panel != null)
                panel.SetActive(panel == panelToShow);
        }

        panelToShow.SetActive(true);
        Debug.Log("[GaragePanelManager] OPENED: " + panelToShow.name, panelToShow);
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



