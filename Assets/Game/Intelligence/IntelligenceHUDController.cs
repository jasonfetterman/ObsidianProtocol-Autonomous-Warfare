using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Intelligence;
using ObsidianProtocol.Game.AI.Commanders;

public class IntelligenceHUDController : MonoBehaviour
{
    private GameObject strategicMap;
    private Button openMapButton;
    private Button closeMapButton;
    private IntelligenceRuntime runtime;
    private BattlefieldEvaluationRuntime threatRuntime;
    private Text sensorDataText;
    private Text threatStatusText;

    private void Awake()
    {
        strategicMap = FindChild(transform, "STRATEGIC INTELLIGENCE MAP");

        if (strategicMap == null)
        {
            Debug.LogError("[INTELLIGENCE HUD] STRATEGIC INTELLIGENCE MAP not found.");
            return;
        }

        openMapButton = FindButton(strategicMap.transform, "BUTTON - OPEN FULL MAP");
        closeMapButton = FindButton(strategicMap.transform, "BUTTON - CLOSE MAP");

        if (openMapButton != null)
        {
            MoveOpenButtonToMainHUD();
            openMapButton.onClick.AddListener(OpenStrategicMap);
        }

        if (closeMapButton != null)
            closeMapButton.onClick.AddListener(CloseStrategicMap);

        GameObject sensorNetwork = FindChild(transform, "SENSOR NETWORK");
        GameObject dataObject = sensorNetwork != null
            ? FindChild(sensorNetwork.transform, "DATA")
            : null;

        if (dataObject != null)
            sensorDataText = dataObject.GetComponent<Text>();

        GameObject threatObject = FindChild(transform, "THREAT STATUS");

        if (threatObject != null)
            threatStatusText = threatObject.GetComponent<Text>();

        CloseStrategicMap();
    }

    private void Start()
    {
        runtime = IntelligenceRuntime.Instance;
        threatRuntime = FindFirstObjectByType<BattlefieldEvaluationRuntime>();

        if (runtime == null)
        {
            Debug.LogError("[INTELLIGENCE HUD] IntelligenceRuntime not found.");
            return;
        }

        Debug.Log("[INTELLIGENCE HUD] Connected to IntelligenceRuntime.");

        UpdateSensorDisplay();
        UpdateThreatDisplay();
    }

    private void Update()
    {
        if (runtime == null)
            return;

        if (sensorDataText != null)
            UpdateSensorDisplay();

        UpdateThreatDisplay();
    }

    private void UpdateSensorDisplay()
    {
        if (sensorDataText == null || runtime == null)
            return;

        sensorDataText.text =
            "ACTIVE SENSORS\n\n" +
            runtime.ActiveSensorCount + " ACTIVE\n\n" +
            "DETECTION RANGE\n\n" +
            GetSensorRange() + " KM\n\n" +
            "COVERAGE\n\n" +
            "84%\n\n" +
            "SENSOR DAMAGE\n\n" +
            "3%";
    }


    private void UpdateThreatDisplay()
    {
        if (threatStatusText == null)
            return;

        if (threatRuntime == null)
            threatRuntime = FindFirstObjectByType<BattlefieldEvaluationRuntime>();

        threatStatusText.text =
            "THREAT STATUS\n\n" +
            (threatRuntime == null
                ? "OFFLINE"
                : threatRuntime.ThreatLevel.ToString().ToUpperInvariant());
    }

    private string GetSensorRange()
    {
        foreach (SensorDefinition sensor in runtime.Sensors.GetSensors())
            return sensor.Range.ToString("0.0");

        return "0.0";
    }

    private void MoveOpenButtonToMainHUD()
    {
        RectTransform buttonRect = openMapButton.GetComponent<RectTransform>();

        if (buttonRect == null)
            return;

        buttonRect.SetParent(transform, false);
        buttonRect.anchorMin = new Vector2(0.5f, 0f);
        buttonRect.anchorMax = new Vector2(0.5f, 0f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = new Vector2(165f, 145f);
        buttonRect.sizeDelta = new Vector2(220f, 48f);
        openMapButton.gameObject.SetActive(true);
    }

    public void OpenStrategicMap()
    {
        if (strategicMap == null)
            return;

        strategicMap.SetActive(true);

        if (openMapButton != null)
            openMapButton.gameObject.SetActive(false);

        if (closeMapButton != null)
        {
            closeMapButton.gameObject.SetActive(true);
            closeMapButton.interactable = true;
        }

        Debug.Log("[INTELLIGENCE HUD] Strategic map opened.");
    }

    public void CloseStrategicMap()
    {
        if (strategicMap == null)
            return;

        strategicMap.SetActive(false);

        if (openMapButton != null)
            openMapButton.gameObject.SetActive(true);

        if (closeMapButton != null)
            closeMapButton.interactable = false;

        Debug.Log("[INTELLIGENCE HUD] Strategic map closed.");
    }

    private static GameObject FindChild(Transform parent, string objectName)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == objectName)
                return child.gameObject;
        }

        return null;
    }

    private static Button FindButton(Transform parent, string objectName)
    {
        GameObject buttonObject = FindChild(parent, objectName);

        if (buttonObject == null)
            return null;

        return buttonObject.GetComponent<Button>();
    }
}



