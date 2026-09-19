using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

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

    private void Start()
    {
        CloseAllWindows();
        HideAllPopups();

        if (loadingOverlay != null)
            loadingOverlay.SetActive(false);

        if (fadeOverlay != null)
            fadeOverlay.SetActive(false);

        if (connectionStatus != null)
            connectionStatus.text = "● ONLINE";
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

    // ========================================================
    // MAIN MENU BUTTONS
    // ========================================================

    public void Continue()
    {
        OpenWindow(continueWindow);
    }

    public void Campaign()
    {
        OpenWindow(campaignWindow);
    }

    public void Multiplayer()
    {
        OpenWindow(multiplayerWindow);
    }

    public void Garage()
    {
        LoadScene("Garage");
    }

    public void Store()
    {
        LoadScene("Store");
    }

    public void VROperator()
    {
        LoadScene("VROperator");
    }

    public void Settings()
    {
        OpenWindow(settingsWindow);
    }

    public void Credits()
    {
        OpenWindow(creditsWindow);
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

    // ========================================================
    // WINDOWS
    // ========================================================

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

    // ========================================================
    // POPUPS
    // ========================================================

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

    // ========================================================
    // SCENE LOADING
    // ========================================================

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

        yield return new WaitForSeconds(0.35f);

        AsyncOperation operation = null;

        try
        {
            operation = SceneManager.LoadSceneAsync(sceneName);
        }
        catch
        {
            if (loadingOverlay != null)
                loadingOverlay.SetActive(false);

            if (fadeOverlay != null)
                fadeOverlay.SetActive(false);

            if (errorPopup != null)
                errorPopup.SetActive(true);

            yield break;
        }

        while (operation != null && !operation.isDone)
            yield return null;
    }

    // ========================================================
    // HELPERS
    // ========================================================

    private void SetState(GameObject obj, bool state)
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


// ============================================================
// MAIN MENU BUTTON
// ============================================================

public class MainMenuButton : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    public Image hoverEdge;
    public Image buttonBackground;
    public Text buttonText;

    public Color normalText =
        new Color(0.75f, 0.80f, 0.84f, 1f);

    public Color hoverText =
        new Color(1f, 1f, 1f, 1f);

    public Color pressedText =
        new Color(0.55f, 0.75f, 0.82f, 1f);

    public Color normalBackground =
        new Color(0.015f, 0.025f, 0.035f, 0.40f);

    public Color hoverBackground =
        new Color(0.025f, 0.045f, 0.055f, 0.50f);

    public Color pressedBackground =
        new Color(0.035f, 0.065f, 0.075f, 0.60f);

    public Color edgeNormal =
        new Color(0f, 0.85f, 1f, 0f);

    public Color edgeHover =
        new Color(0f, 0.85f, 1f, 1f);

    public Color edgePressed =
        new Color(0.35f, 1f, 1f, 1f);

    private void Start()
    {
        ApplyNormal();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverEdge != null)
            hoverEdge.color = edgeHover;

        if (buttonBackground != null)
            buttonBackground.color = hoverBackground;

        if (buttonText != null)
            buttonText.color = hoverText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyNormal();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (hoverEdge != null)
            hoverEdge.color = edgePressed;

        if (buttonBackground != null)
            buttonBackground.color = pressedBackground;

        if (buttonText != null)
            buttonText.color = pressedText;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (hoverEdge != null)
            hoverEdge.color = edgeHover;

        if (buttonBackground != null)
            buttonBackground.color = hoverBackground;

        if (buttonText != null)
            buttonText.color = hoverText;
    }

    private void ApplyNormal()
    {
        if (hoverEdge != null)
            hoverEdge.color = edgeNormal;

        if (buttonBackground != null)
            buttonBackground.color = normalBackground;

        if (buttonText != null)
            buttonText.color = normalText;
    }
}
