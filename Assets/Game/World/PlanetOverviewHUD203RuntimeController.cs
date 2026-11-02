using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class PlanetOverviewHUD203RuntimeController : MonoBehaviour
{
    [Header("Five Test Planets")]
    [SerializeField] private Transform planetMount;
    [SerializeField] private GameObject[] planetPrefabs = new GameObject[5];
    [SerializeField] private Button[] planetButtons = new Button[5];
    [SerializeField] private TMP_Text selectedPlanetLabel;
    [SerializeField] private TMP_Text statusLabel;
    [SerializeField] private float targetPlanetDiameter = 9f;

    [Header("Optional Panels")]
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private GameObject locationPanel;
    [SerializeField] private GameObject actionsPanel;

    [SerializeField] private GameObject activePlanet;
    [SerializeField] private int selectedPlanetIndex = -1;

    private static readonly string[] PlanetNames =
    {
        "ACHERON",
        "VIRELLIA",
        "KARTHOS",
        "SOLARA PRIME",
        "NOX MERIDIAN"
    };

    private static readonly float[] PlanetRelativeSizes =
    {
        1.80f, 1.20f, 1.70f, 2.20f, 1.50f
    };

    private void Awake()
    {
        BindNavigationButtons();
        for (int i = 0; i < planetButtons.Length; i++)
        {
            Button button = planetButtons[i];
            if (button == null)
                continue;

            int index = i;
            button.onClick.AddListener(() => SelectPlanet(index));
        }

        AdoptExistingPlanet();

        if (selectedPlanetIndex < 0)
            SelectPlanet(0);
    }


    private void BindNavigationButtons()
    {
        BindButton("HUD-203-02A ReturnToStarSystemButton",
            () => LoadTargetScene("Star_System"));
        BindButton("HUD-203-02B ReturnToGalaxyMapButton",
            () => LoadTargetScene("Galaxy_Map"));
        BindButton("HUD-203-02C SurfaceMapButton",
            () => LoadTargetScene("Planet_Surface_Map"));
        BindButton("HUD-203-02D ContinentMapButton",
            () => LoadTargetScene("Continent_Map"));
        BindButton("HUD-203-02E RegionMapButton",
            () => LoadTargetScene("Region_Map"));
        BindButton("HUD-203-02F IslandMapButton",
            () => LoadTargetScene("Island_Map"));
        BindButton("HUD-203-02G BiomeMapButton",
            () => LoadTargetScene("Biome_Map"));

        BindButton("HUD-203-06A DeployReconButton",
            () => LoadTargetScene("SCN12_DEPLOYMENT"));

        BindButton("HUD-203-06B OpenIntelNetworkButton",
            () => LoadTargetScene("SCN18_INTELLIGENCE"));

        BindButton("HUD-203-06C OpenDoctrineManagerButton",
            () => SetStatus("DOCTRINE MANAGER NOT CONFIGURED: DEDICATED SCENE REQUIRED"));

        BindButton("HUD-203-06D OpenLogisticsOverviewButton",
            () => LoadTargetScene("Logistics_Network"));

        BindButton("HUD-203-06E OpenWarfareProfileButton",
            () => LoadTargetScene("Warfare_Profile"));

        BindButton("HUD-203-08A BackButton",
            () => LoadTargetScene("Star_System"));
        BindButton("HUD-203-08B ExitToCommandCenterButton",
            () => LoadTargetScene("Command_Center"));
        BindButton("HUD-203-08C SettingsButton",
            () => LoadTargetScene("Settings"));
        BindButton("HUD-203-08D HelpButton", ShowHelp);
    }

    private void BindButton(
        string objectName,
        UnityEngine.Events.UnityAction action)
    {
        Button[] buttons = FindObjectsByType<Button>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (Button button in buttons)
        {
            if (button.gameObject.name != objectName)
                continue;

            button.onClick.RemoveListener(action);
            button.onClick.AddListener(action);
            return;
        }

        Debug.LogWarning("[HUD-203] Button not found: " + objectName, this);
    }

    private void LoadTargetScene(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);

            if (System.IO.Path.GetFileNameWithoutExtension(scenePath)
                != sceneName)
                continue;

            SceneManager.LoadScene(scenePath);
            return;
        }

        SetStatus("SCENE NOT ENABLED IN BUILD SETTINGS: " + sceneName);
    }

    private void ShowHelp()
    {
        SetNamedText("HUD-203-07A StrategicAlertEntry",
            "HELP // SELECT A PLANET TO INSPECT ITS STRATEGIC OVERVIEW.");
        SetNamedText("HUD-203-07B WarfareAlertEntry",
            "NAVIGATION // USE THE LEFT PANEL TO OPEN PLANETARY MAPS.");
        SetNamedText("HUD-203-07C FactionAlertEntry",
            "INTELLIGENCE // REVIEW PLANETARY CONTROL AND CONDITIONS.");
        SetNamedText("HUD-203-07D ResourceAlertEntry",
            "TIP // USE BACK TO RETURN TO THE STAR SYSTEM.");
    }

    private void SetNamedText(string objectName, string value)
    {
        TMP_Text[] texts = FindObjectsByType<TMP_Text>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (TMP_Text textElement in texts)
        {
            if (textElement.gameObject.name != objectName)
                continue;

            textElement.text = value;
            return;
        }

        Debug.LogWarning("[HUD-203] Help text object not found: " + objectName, this);
    }
    private void AdoptExistingPlanet()
    {
        if (activePlanet == null && planetMount != null)
        {
            for (int i = 0; i < planetMount.childCount; i++)
            {
                Transform child = planetMount.GetChild(i);

                for (int p = 0; p < PlanetNames.Length; p++)
                {
                    if (child.name == PlanetNames[p] + " - HUD203 PLANET")
                    {
                        activePlanet = child.gameObject;
                        selectedPlanetIndex = p;
                        break;
                    }
                }

                if (activePlanet != null)
                    break;
            }
        }

        if (selectedPlanetIndex >= 0 &&
            selectedPlanetIndex < PlanetNames.Length &&
            selectedPlanetLabel != null)
        {
            selectedPlanetLabel.text = PlanetNames[selectedPlanetIndex];
        }
    }

    public void SelectAcheron() => SelectPlanet(0);
    public void SelectVirellia() => SelectPlanet(1);
    public void SelectKarthos() => SelectPlanet(2);
    public void SelectSolaraPrime() => SelectPlanet(3);
    public void SelectNoxMeridian() => SelectPlanet(4);

    public void SelectPlanet(int index)
    {
        if (index < 0 || index >= 5 ||
            index >= planetPrefabs.Length)
        {
            SetStatus("INVALID PLANET SELECTION");
            return;
        }

        if (planetMount == null)
        {
            SetStatus("PLANET DISPLAY IS NOT CONFIGURED");
            return;
        }

        if (planetPrefabs[index] == null)
        {
            SetStatus(PlanetNames[index] + " PREFAB IS NOT ASSIGNED");
            return;
        }

        GameObject replacement = Instantiate(
            planetPrefabs[index], planetMount, false);

        replacement.name = PlanetNames[index] + " - HUD203 PLANET";
        FitPlanet(replacement);
        ApplyRotation(replacement, index);

        if (activePlanet != null)
            Destroy(activePlanet);

        activePlanet = replacement;
        selectedPlanetIndex = index;

        if (selectedPlanetLabel != null)
            selectedPlanetLabel.text = PlanetNames[index];

        SetStatus(PlanetNames[index] + " SELECTED");
    }

    private void ApplyRotation(GameObject planet, int index)
    {
        // Preserve Acheron's existing rotation setup.
        if (index <= 0)
            return;

        if (planet.GetComponent<PlanetRotation>() == null)
            planet.AddComponent<PlanetRotation>();

        Transform[] children =
            planet.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child == planet.transform ||
                !child.name.ToUpperInvariant().Contains("CLOUD"))
                continue;

            if (child.GetComponent<CloudRotation>() == null)
                child.gameObject.AddComponent<CloudRotation>();

            break;
        }
    }
    private void FitPlanet(GameObject planet)
    {
        planet.transform.localPosition = Vector3.zero;
        planet.transform.localRotation = Quaternion.identity;
        planet.transform.localScale = Vector3.one;

        Renderer[] renderers =
            planet.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            SetStatus("PLANET PREFAB HAS NO RENDERERS");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);

        float largestDimension = Mathf.Max(
            bounds.size.x, bounds.size.y, bounds.size.z);

        if (largestDimension <= 0.001f)
        {
            SetStatus("PLANET PREFAB HAS INVALID DIMENSIONS");
            return;
        }

        int planetIndex = System.Array.IndexOf(
            PlanetNames,
            planet.name.Replace(" - HUD203 PLANET", ""));

        float relativeSize = planetIndex >= 0
            ? PlanetRelativeSizes[planetIndex] / PlanetRelativeSizes[0]
            : 1f;

        float desiredDiameter = targetPlanetDiameter * relativeSize;
        float scaleFactor = desiredDiameter / largestDimension;
        planet.transform.localScale = Vector3.one * scaleFactor;
        planet.transform.localPosition = new Vector3(-4.54f, 1.92f, 0f);
    }

    public void ShowOverview()
    {
        SetPanel(informationPanel, false);
        SetPanel(locationPanel, false);
        SetPanel(actionsPanel, true);
    }

    public void ShowInformation()
    {
        SetPanel(informationPanel, true);
        SetPanel(locationPanel, false);
        SetPanel(actionsPanel, true);
    }

    public void ShowLocation()
    {
        SetPanel(informationPanel, false);
        SetPanel(locationPanel, true);
        SetPanel(actionsPanel, true);
    }

    public void ToggleActions()
    {
        if (actionsPanel != null)
            actionsPanel.SetActive(!actionsPanel.activeSelf);
    }

    public void CloseOptionalPanels()
    {
        SetPanel(informationPanel, false);
        SetPanel(locationPanel, false);
        SetPanel(actionsPanel, false);
    }

    private static void SetPanel(GameObject panel, bool visible)
    {
        if (panel != null)
            panel.SetActive(visible);
    }

    private void SetStatus(string message)
    {
        if (statusLabel != null)
            statusLabel.text = message;

        Debug.Log("[HUD-203] " + message, this);
    }
}









