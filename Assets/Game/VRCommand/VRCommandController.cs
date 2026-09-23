using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ObsidianProtocol.Game.Core;

public class VRCommandController : MonoBehaviour
{
    public enum VRPanel
    {
        Command,
        Fleet,
        Intelligence,
        Deployment,
        Garage,
        Maintenance,
        UnitInspection,
        Battlefield,
        Holographic,
        PhysicalGarage
    }

    [Header("Main HUD")]
    [SerializeField] private GameObject vrCommandHUD;

    [Header("Navigation")]
    [SerializeField] private Button commandButton;
    [SerializeField] private Button fleetButton;
    [SerializeField] private Button intelButton;
    [SerializeField] private Button deployButton;
    [SerializeField] private Button garageButton;

    [Header("Panels")]
    [SerializeField] private GameObject commandPanel;
    [SerializeField] private GameObject fleetPanel;
    [SerializeField] private GameObject intelligencePanel;
    [SerializeField] private GameObject deploymentPanel;
    [SerializeField] private GameObject garagePanel;
    [SerializeField] private GameObject maintenancePanel;
    [SerializeField] private GameObject unitInspectionPanel;
    [SerializeField] private GameObject battlefieldPanel;
    [SerializeField] private GameObject holographicPanel;
    [SerializeField] private GameObject physicalGaragePanel;

    private VRPanel currentPanel;

    private void Awake()
    {
        AutoBindSceneObjects();
        WireButtons();
    }

    private void Start()
    {
        ShowPanel(VRPanel.Command);

        ObsidianRuntimeSystems systems =
            ObsidianRuntimeSystems.Instance;

        if (systems != null && systems.Initialized)
        {
            Debug.Log(
                "[VR COMMAND] Connected to Obsidian runtime systems.");
        }
        else
        {
            Debug.LogError(
                "[VR COMMAND] Obsidian runtime systems are not available.");
        }

        Debug.Log("[VR COMMAND] Runtime initialized.");
    }

    private void AutoBindSceneObjects()
    {
        vrCommandHUD =
            FindSceneObject("[HUD] VR COMMAND HUD");

        commandButton =
            FindSceneButton("[BUTTON] COMMAND");

        fleetButton =
            FindSceneButton("[BUTTON] FLEET");

        intelButton =
            FindSceneButton("[BUTTON] INTEL");

        deployButton =
            FindSceneButton("[BUTTON] DEPLOY");

        garageButton =
            FindSceneButton("[BUTTON] GARAGE");

        commandPanel =
            FindSceneObject("[PANEL] COMMAND CENTER");

        fleetPanel =
            FindSceneObject("[PANEL] FLEET INSPECTION");

        intelligencePanel =
            FindSceneObject("[PANEL] INTELLIGENCE SYSTEMS");

        deploymentPanel =
            FindSceneObject("[PANEL] DEPLOYMENT SYSTEMS");

        garagePanel =
            FindSceneObject("[PANEL] PHYSICAL GARAGE");

        maintenancePanel =
            FindSceneObject("[PANEL] MAINTENANCE INTERACTION");

        unitInspectionPanel =
            FindSceneObject("[PANEL] UNIT INSPECTION");

        battlefieldPanel =
            FindSceneObject("[PANEL] FULL VR BATTLEFIELD COMMAND");

        holographicPanel =
            FindSceneObject("[PANEL] HOLOGRAPHIC DISPLAYS");

        physicalGaragePanel =
            FindSceneObject("[PANEL] PHYSICAL GARAGE");

        Debug.Log("[VR COMMAND] Scene objects auto-bound.");
    }

    private GameObject FindSceneObject(string objectName)
    {
        GameObject[] objects =
            Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj.name != objectName)
                continue;

            if (obj.scene != SceneManager.GetActiveScene())
                continue;

            return obj;
        }

        Debug.LogWarning(
            "[VR COMMAND] Missing scene object: " +
            objectName);

        return null;
    }

    private Button FindSceneButton(string objectName)
    {
        GameObject obj =
            FindSceneObject(objectName);

        if (obj == null)
            return null;

        Button button =
            obj.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning(
                "[VR COMMAND] No Button component on: " +
                objectName);
        }

        return button;
    }

    private void WireButtons()
    {
        Wire(commandButton, OpenCommand);
        Wire(fleetButton, OpenFleet);
        Wire(intelButton, OpenIntel);
        Wire(deployButton, OpenDeploy);
        Wire(garageButton, OpenGarage);

        Debug.Log("[VR COMMAND] Navigation buttons wired.");
    }

    private void Wire(
        Button button,
        UnityEngine.Events.UnityAction action)
    {
        if (button == null)
            return;

        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    public void OpenCommand()
    {
        ShowPanel(VRPanel.Command);
    }

    public void OpenFleet()
    {
        ShowPanel(VRPanel.Fleet);
    }

    public void OpenIntel()
    {
        ShowPanel(VRPanel.Intelligence);
    }

    public void OpenDeploy()
    {
        ShowPanel(VRPanel.Deployment);
    }

    public void OpenGarage()
    {
        ShowPanel(VRPanel.Garage);
    }

    public void ShowPanel(VRPanel panel)
    {
        currentPanel = panel;

        SetPanel(
            commandPanel,
            panel == VRPanel.Command);

        SetPanel(
            fleetPanel,
            panel == VRPanel.Fleet);

        SetPanel(
            intelligencePanel,
            panel == VRPanel.Intelligence);

        SetPanel(
            deploymentPanel,
            panel == VRPanel.Deployment);

        SetPanel(
            garagePanel,
            panel == VRPanel.Garage);

        SetPanel(
            maintenancePanel,
            panel == VRPanel.Maintenance);

        SetPanel(
            unitInspectionPanel,
            panel == VRPanel.UnitInspection);

        SetPanel(
            battlefieldPanel,
            panel == VRPanel.Battlefield);

        SetPanel(
            holographicPanel,
            panel == VRPanel.Holographic);

        SetPanel(
            physicalGaragePanel,
            panel == VRPanel.PhysicalGarage);

        Debug.Log(
            "[VR COMMAND] Opened panel: " +
            panel);
    }

    private void SetPanel(
        GameObject panel,
        bool state)
    {
        if (panel != null)
            panel.SetActive(state);
    }

    public VRPanel CurrentPanel =>
        currentPanel;
}
