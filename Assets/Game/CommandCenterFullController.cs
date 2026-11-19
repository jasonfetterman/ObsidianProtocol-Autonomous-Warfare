using UnityEngine;
using UnityEngine.SceneManagement;
using ObsidianProtocol.UI.Store;

public class CommandCenterFullController : MonoBehaviour
{
    public GameObject CommandPanel;
    public GameObject OperationsPanel;
    public GameObject FleetPanel;
    public GameObject DeployPanel;
    public GameObject LogisticsPanel;
    public GameObject ResearchPanel;
    public GameObject GaragePanel;
    public GameObject FinancePanel;
    public GameObject IntelligencePanel;
    public GameObject StorePanel;

    public void OpenCommand()
    {
        Open(CommandPanel);
    }

    public void OpenOperations()
    {
        Open(OperationsPanel);
    }

    public void OpenFleet()
    {
        Debug.Log("[FLEET] OpenFleet invoked.");

        if (FleetPanel == null)
        {
            Debug.LogError("[FLEET] FleetPanel reference is missing.");
            return;
        }

        Open(FleetPanel);
        Debug.Log("[FLEET] Activated panel: " + FleetPanel.name);

        FleetPanelManager manager = FleetPanel.GetComponent<FleetPanelManager>();
        if (manager == null)
        {
            Debug.Log("[FLEET] Adding FleetPanelManager.");
            manager = FleetPanel.AddComponent<FleetPanelManager>();
        }

        manager.RefreshList();
        Debug.Log("[FLEET] RefreshList completed.");
    }

    public void OpenDeploy()
    {
        Open(DeployPanel);
    }

    public void OpenLogistics()
    {
        Open(LogisticsPanel);
    }

    public void OpenResearch()
    {
        Open(ResearchPanel);
    }

    public void OpenGarage()
    {
        Open(GaragePanel);
    }

    public void OpenFinance()
    {
        Open(FinancePanel);
    }

    public void OpenIntelligence()
    {
        Open(IntelligencePanel);
    }

    public void OpenStore()
    {
        Open(StorePanel);
        if (StorePanel != null)
            StorePanel.GetComponent<StoreMasterController>()?.OpenStore();
    }

    public void EmergencyRecall()
    {
        Debug.Log("[COMMAND CENTER] EMERGENCY RECALL COMMAND ISSUED.");
    }

    public void CloseAll()
    {
        if (StorePanel != null && StorePanel.activeSelf)
            StorePanel.GetComponent<StoreMasterController>()?.CloseStore();
        Set(CommandPanel, false);
        Set(OperationsPanel, false);
        Set(FleetPanel, false);
        Set(DeployPanel, false);
        Set(LogisticsPanel, false);
        Set(ResearchPanel, false);
        Set(GaragePanel, false);
        Set(FinancePanel, false);
        Set(IntelligencePanel, false);
        Set(StorePanel, false);
    }

    private void Open(GameObject target)
    {
        CloseAll();
        Set(target, true);
    }

    private void Set(GameObject target, bool state)
    {
        if (target != null)
            target.SetActive(state);
    }
    public void OpenGarageScene()
    {
        SceneManager.LoadScene("SCN04_GARAGE");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("SCN‑01  MAIN MENU");
    }
}
