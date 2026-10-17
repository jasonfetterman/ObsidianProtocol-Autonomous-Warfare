using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommandCenterFullController : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(true);
    }

    private void Start()
    {
        gameObject.SetActive(true);
        CloseAll();
        WireButtons();

        Debug.Log("[COMMAND CENTER] READY");
    }

    private void WireButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            switch (button.gameObject.name)
            {
                case "BUTTON_COMMAND":
                    Wire(button, OpenCommand);
                    break;
                case "BUTTON_OPERATIONS":
                    Wire(button, OpenOperations);
                    break;
                case "BUTTON_FLEET":
                    Wire(button, OpenFleet);
                    break;
                case "BUTTON_DEPLOY":
                    Wire(button, OpenDeploy);
                    break;
                case "BUTTON_LOGISTICS":
                    Wire(button, OpenLogistics);
                    break;
                case "BUTTON_RESEARCH":
                    Wire(button, OpenResearch);
                    break;
                case "BUTTON_GARAGE":
                    Wire(button, OpenGarage);
                    break;
                case "BUTTON_FINANCE":
                    Wire(button, OpenFinance);
                    break;
                case "BUTTON_INTELLIGENCE":
                    Wire(button, OpenIntelligence);
                    break;
                case "BUTTON_STORE":
                    Wire(button, OpenStore);
                    break;
                case "BUTTON_BACK":
                    Wire(button, BackToMainMenu);
                    break;
                case "BACK":
                    Wire(button, BackToMainMenu);
                    break;
                case "CLOSE":
                    Wire(button, CloseAll);
                    break;
            }
        }

        Debug.Log("[COMMAND CENTER] BUTTONS FOUND: " + buttons.Length);
        Debug.Log("[COMMAND CENTER] BUTTON WIRING COMPLETE");
    }

    private void Wire(Button button, UnityEngine.Events.UnityAction action)
    {
        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    public void OpenCommand()       { OpenPanel("COMMAND_PANEL"); }
    public void OpenOperations()    { OpenPanel("OPERATIONS_PANEL"); }
    public void OpenFleet()         { OpenPanel("FLEET_PANEL"); }
    public void OpenDeploy()        { OpenPanel("DEPLOY_PANEL"); }
    public void OpenLogistics()     { OpenPanel("LOGISTICS_PANEL"); }
    public void OpenResearch()      { OpenPanel("RESEARCH_PANEL"); }
    public void OpenGarage()        { OpenPanel("GARAGE_PANEL"); }
    public void OpenFinance()       { OpenPanel("FINANCE_PANEL"); }
    public void OpenIntelligence()  { OpenPanel("INTELLIGENCE_PANEL"); }
    public void OpenStore()         { OpenPanel("STORE_PANEL"); }

    public void BackToMainMenu()
    {
        Debug.Log("[COMMAND CENTER] BACK ? MAIN MENU");
        SceneManager.LoadScene(0);
    }

    public void CloseAll()
    {
        string[] panels =
        {
            "COMMAND_PANEL",
            "OPERATIONS_PANEL",
            "FLEET_PANEL",
            "DEPLOY_PANEL",
            "LOGISTICS_PANEL",
            "RESEARCH_PANEL",
            "GARAGE_PANEL",
            "FINANCE_PANEL",
            "INTELLIGENCE_PANEL",
            "STORE_PANEL"
        };

        foreach (string panelName in panels)
        {
            GameObject panel = FindPanel(panelName);

            if (panel != null)
                panel.SetActive(false);
        }
    }

    private void OpenPanel(string panelName)
    {
        CloseAll();

        GameObject panel = FindPanel(panelName);

        if (panel == null)
        {
            Debug.LogError("[COMMAND CENTER] PANEL NOT FOUND: " + panelName);
            return;
        }

        panel.SetActive(true);
        Debug.Log("[COMMAND CENTER] OPENED: " + panelName);
    }

    private GameObject FindPanel(string panelName)
    {
        Transform[] transforms =
            GetComponentsInChildren<Transform>(true);

        foreach (Transform t in transforms)
        {
            if (t.gameObject.name == panelName)
                return t.gameObject;
        }

        return null;
    }
}


