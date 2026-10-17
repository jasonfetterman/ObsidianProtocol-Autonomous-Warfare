using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using ObsidianProtocol.Game.Campaign;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [Header("Windows")]
    public GameObject continueWindow;
    public GameObject campaignWindow;
    public GameObject multiplayerWindow;
    public GameObject settingsWindow;
    public GameObject creditsWindow;

    [Header("Popups")]
    public GameObject confirmationPopup;
    public GameObject warningPopup;
    public GameObject errorPopup;
    public GameObject connectionPopup;
    public GameObject exitConfirmationPopup;

    [Header("Overlays")]
    public GameObject loadingOverlay;
    public GameObject fadeOverlay;

    [Header("Connection")]
    public Text connectionStatus;

    private void Awake()
    {
        Instance = this;
    }

    private void WireMainMenuButtons()
{
    Button[] buttons = FindObjectsByType<Button>(
        FindObjectsInactive.Include,
        FindObjectsSortMode.None);

    foreach (Button button in buttons)
    {
        Text text = button.GetComponentInChildren<Text>(true);

        if (text == null)
            continue;

        string label = text.text.Trim().ToUpper();

        if (label == "CAMPAIGN") { button.onClick.RemoveListener(Campaign); button.onClick.AddListener(Campaign); }
        else if (label == "MULTIPLAYER")
        {
            button.onClick.RemoveListener(Multiplayer);
            button.onClick.AddListener(Multiplayer);
        }
        else if (label == "GARAGE")
        {
            button.onClick.RemoveListener(Garage);
            button.onClick.AddListener(Garage);
        }
        else if (label == "VR OPERATOR")
        {
            button.onClick.RemoveListener(VROperator);
            button.onClick.AddListener(VROperator);
        }
        else if (label == "SETTINGS")
        {
            button.onClick.RemoveListener(Settings);
            button.onClick.AddListener(Settings);
        }
        else if (label == "CREDITS")
        {
            button.onClick.RemoveListener(Credits);
            button.onClick.AddListener(Credits);
        }
        else if (label == "STORE")
        {
            button.onClick.RemoveListener(Store);
            button.onClick.AddListener(Store);
        }
    }
}
private void Start()
    {
        WireMainMenuButtons();

        CloseAllWindows();
        HideAllPopups();

        if (loadingOverlay != null)
            loadingOverlay.SetActive(false);

        if (fadeOverlay != null)
            fadeOverlay.SetActive(false);

        if (connectionStatus != null)
            connectionStatus.text = "? ONLINE";
    }

    private void Start_DISABLED()
    {
        CloseAllWindows();
        HideAllPopups();

        if (loadingOverlay != null)
            loadingOverlay.SetActive(false);

        if (fadeOverlay != null)
            fadeOverlay.SetActive(false);

        if (connectionStatus != null)
            connectionStatus.text = "? ONLINE";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AnyWindowOpen())
            {
                CloseAllWindows();
                return;
            }

            if (AnyPopupOpen())
            {
                HideAllPopups();
                return;
            }
        }
    }

    public void Continue()
    {
        OpenWindow(continueWindow);
    }

    public void Campaign()
    {
        Debug.Log("[MainMenu] CAMPAIGN selected.");

        OpenWindow(campaignWindow);

        if (campaignWindow != null)
        {
            ObsidianProtocol.UI.CampaignSetupController setup =
                campaignWindow.GetComponent<ObsidianProtocol.UI.CampaignSetupController>();

            if (setup == null)
                setup = campaignWindow.AddComponent<ObsidianProtocol.UI.CampaignSetupController>();

            setup.Build();
        }
    }

    public void Multiplayer()
    {
        LoadScene("MultiplayerHUD");
    }

    public void Garage()
    {
        LoadScene("SCN04_GARAGE");
    }

    public void Store()
    {
        LoadScene("SCN16_STORE");
    }

    public void VROperator()
    {
        LoadScene("SCN20_VR_COMMAND");
    }

    public void Settings() { Debug.Log("[MainMenu] SETTINGS CLICKED."); LoadScene("Settings"); }

    public void Credits()
{
    Debug.Log("[MainMenu] CREDITS CLICKED.");
    LoadScene("SCN23_CREDITS");
}

    public void Exit()
    {
        if (exitConfirmationPopup != null)
            exitConfirmationPopup.SetActive(true);
    }

    public void ConfirmExit()
    {
        Application.Quit();
    }

    public void OpenWindow(GameObject window)
    {
        CloseAllWindows();

        if (window != null)
            window.SetActive(true);
    }

    public void CloseAllWindows()
    {
        SetState(continueWindow, false);
        SetState(campaignWindow, false);
        SetState(multiplayerWindow, false);
        SetState(settingsWindow, false);
        SetState(creditsWindow, false);
    }

    public void HideAllPopups()
    {
        SetState(confirmationPopup, false);
        SetState(warningPopup, false);
        SetState(errorPopup, false);
        SetState(connectionPopup, false);
        SetState(exitConfirmationPopup, false);
    }

    public void ShowConfirmation()
    {
        HideAllPopups();

        if (confirmationPopup != null)
            confirmationPopup.SetActive(true);
    }

    public void ShowWarning()
    {
        HideAllPopups();

        if (warningPopup != null)
            warningPopup.SetActive(true);
    }

    public void ShowError()
    {
        HideAllPopups();

        if (errorPopup != null)
            errorPopup.SetActive(true);
    }

    public void ShowConnection()
    {
        HideAllPopups();

        if (connectionPopup != null)
            connectionPopup.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (loadingOverlay != null)
            loadingOverlay.SetActive(true);

        if (fadeOverlay != null)
            fadeOverlay.SetActive(true);

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            Debug.LogError(
                $"[MainMenu] Failed to load scene: {sceneName}"
            );

            if (loadingOverlay != null)
                loadingOverlay.SetActive(false);

            if (fadeOverlay != null)
                fadeOverlay.SetActive(false);

            yield break;
        }

        while (!operation.isDone)
            yield return null;
    }

    private void SetState(
        GameObject obj,
        bool state
    )
    {
        if (obj != null)
            obj.SetActive(state);
    }

    private bool IsOpen(GameObject obj)
    {
        return obj != null && obj.activeSelf;
    }

    private bool AnyWindowOpen()
    {
        return
            IsOpen(continueWindow) ||
            IsOpen(campaignWindow) ||
            IsOpen(multiplayerWindow) ||
            IsOpen(settingsWindow) ||
            IsOpen(creditsWindow);
    }

    private bool AnyPopupOpen()
    {
        return
            IsOpen(confirmationPopup) ||
            IsOpen(warningPopup) ||
            IsOpen(errorPopup) ||
            IsOpen(connectionPopup) ||
            IsOpen(exitConfirmationPopup);
    }
}

public class MainMenuButton :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    // Existing MainMenuBuilder dependencies.
    public Image hoverEdge;
    public Image buttonBackground;
    public Text buttonText;

    [SerializeField] private float hoverScale = 1.03f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale =
            originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale =
            originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale =
            originalScale;
    }
}




































