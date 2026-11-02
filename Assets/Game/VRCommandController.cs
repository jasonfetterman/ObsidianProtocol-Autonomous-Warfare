using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Main HUD & Cameras")]
    [SerializeField] private GameObject vrCommandHUD;
    [SerializeField] private GameObject desktopCamera;
    [SerializeField] private GameObject xrOriginObject;

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
        ShowPanel(VRPanel.Garage);

        ObsidianRuntimeSystems systems = ObsidianRuntimeSystems.Instance;
        if (systems != null && systems.Initialized)
            Debug.Log("[VR COMMAND] Connected to Obsidian runtime systems.");
    }

    private void Update()
    {
        // Manual toggle hotkey (Tab or Space) to enter building view immediately on desktop
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Space))
        {
            EnterBuildingFacility();
        }
    }


    public void EnterBuildingFacility()
    {
        // Turn OFF flat PC screen display and flat desktop camera
        if (vrCommandHUD != null)
            vrCommandHUD.SetActive(false);

        if (desktopCamera != null)
            desktopCamera.SetActive(false);

        // Turn ON XR Origin (inside building view)
        if (xrOriginObject != null)
            xrOriginObject.SetActive(true);

        Debug.Log("<color=#00FFCC>[VR COMMAND] Switched to Inside Building View.</color>");
    }

    private void AutoBindSceneObjects()
    {
        vrCommandHUD = FindSceneObject("[HUD] VR COMMAND HUD");
        desktopCamera = FindSceneObject("VR COMMAND CAMERA");

        // Find XR Origin
        GameObject xrCam = FindSceneObject("Main Camera");
        if (xrCam != null && xrCam != desktopCamera)
        {
            xrOriginObject = xrCam.transform.root.gameObject;
        }
        else
        {
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name.Contains("XR Origin") || go.name.Contains("XR Rig"))
                {
                    if (go.scene == SceneManager.GetActiveScene())
                    {
                        xrOriginObject = go;
                        break;
                    }
                }
            }
        }

        commandButton = FindSceneButton("[BUTTON] COMMAND");
        fleetButton = FindSceneButton("[BUTTON] FLEET");
        intelButton = FindSceneButton("[BUTTON] INTEL");
        deployButton = FindSceneButton("[BUTTON] DEPLOY");
        garageButton = FindSceneButton("[BUTTON] GARAGE");

        commandPanel = FindSceneObject("[PANEL] COMMAND CENTER");
        fleetPanel = FindSceneObject("[PANEL] FLEET INSPECTION");
        intelligencePanel = FindSceneObject("[PANEL] INTELLIGENCE SYSTEMS");
        deploymentPanel = FindSceneObject("[PANEL] DEPLOYMENT SYSTEMS");
        garagePanel = FindSceneObject("[PANEL] PHYSICAL GARAGE");
        maintenancePanel = FindSceneObject("[PANEL] MAINTENANCE INTERACTION");
        unitInspectionPanel = FindSceneObject("[PANEL] UNIT INSPECTION");
        battlefieldPanel = FindSceneObject("[PANEL] FULL VR BATTLEFIELD COMMAND");
        holographicPanel = FindSceneObject("[PANEL] HOLOGRAPHIC DISPLAYS");
        physicalGaragePanel = FindSceneObject("[PANEL] PHYSICAL GARAGE");

        Debug.Log("[VR COMMAND] Scene objects auto-bound.");
    }

    private GameObject FindSceneObject(string objectName)
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in objects)
        {
            if (obj.name != objectName) continue;
            if (obj.scene != SceneManager.GetActiveScene()) continue;
            return obj;
        }
        return null;
    }

    private Button FindSceneButton(string objectName)
    {
        GameObject obj = FindSceneObject(objectName);
        return obj != null ? obj.GetComponent<Button>() : null;
    }

    private void WireButtons()
    {
        Wire(commandButton, OpenCommand);
        Wire(fleetButton, OpenFleet);
        Wire(intelButton, OpenIntel);
        Wire(deployButton, OpenDeploy);
        Wire(garageButton, OpenGarage);
    }

    private void Wire(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button == null) return;
        button.onClick.RemoveListener(action);
        button.onClick.AddListener(action);
    }

    public void OpenCommand()      => ShowPanel(VRPanel.Command);
    public void OpenFleet()        => ShowPanel(VRPanel.Fleet);
    public void OpenIntel()        => ShowPanel(VRPanel.Intelligence);
    public void OpenDeploy()       => ShowPanel(VRPanel.Deployment);
    public void OpenGarage()       => ShowPanel(VRPanel.Garage);

    public void ShowPanel(VRPanel panel)
    {
        currentPanel = panel;
        SetPanel(commandPanel, panel == VRPanel.Command);
        SetPanel(fleetPanel, panel == VRPanel.Fleet);
        SetPanel(intelligencePanel, panel == VRPanel.Intelligence);
        SetPanel(deploymentPanel, panel == VRPanel.Deployment);

        SetPanel(maintenancePanel, panel == VRPanel.Maintenance);
        SetPanel(unitInspectionPanel, panel == VRPanel.UnitInspection);
        SetPanel(battlefieldPanel, panel == VRPanel.Battlefield);
        SetPanel(holographicPanel, panel == VRPanel.Holographic);
        SetPanel(garagePanel, panel == VRPanel.Garage || panel == VRPanel.PhysicalGarage);
    }

    private void SetPanel(GameObject panel, bool state)
    {
        if (panel != null) panel.SetActive(state);
    }

    public VRPanel CurrentPanel => currentPanel;
    public void OpenPhysicalGarage() { ShowPanel(VRPanel.PhysicalGarage); }
}

