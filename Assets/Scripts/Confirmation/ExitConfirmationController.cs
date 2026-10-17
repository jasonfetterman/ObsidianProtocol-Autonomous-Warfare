using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace ObsidianProtocol.Game.Confirmation
{
    public class ExitConfirmationController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject confirmationWindow;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;

        [Header("Optional Text")]
        [SerializeField] private TMP_Text sessionStatusText;

        [Header("Scene")]
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private bool isOpen;

        private void Awake()
        {
            WireButtons();
            CloseImmediate();
        }

        private void Start()
        {
            Open();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isOpen)
                    Cancel();
                else
                    Open();
            }
        }

        private void WireButtons()
        {
            if (cancelButton != null)
            {
                cancelButton.onClick.RemoveListener(Cancel);
                cancelButton.onClick.AddListener(Cancel);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.RemoveListener(ExitToMainMenu);
                mainMenuButton.onClick.AddListener(ExitToMainMenu);
            }

            if (quitButton != null)
            {
                quitButton.onClick.RemoveListener(QuitToDesktop);
                quitButton.onClick.AddListener(QuitToDesktop);
            }
        }

        public void Open()
        {
            isOpen = true;

            if (confirmationWindow != null)
                confirmationWindow.SetActive(true);

            if (sessionStatusText != null)
            {
                sessionStatusText.text =
                    "SESSION STATUS\n\n" +
                    "COMMAND STATE     ACTIVE\n" +
                    "SESSION DATA      PRESERVED\n" +
                    "UNSAVED CHANGES   CHECK REQUIRED";
            }

            SelectCancel();
        }

        public void Cancel()
        {
            isOpen = false;

            if (confirmationWindow != null)
                confirmationWindow.SetActive(false);
        }

        private void CloseImmediate()
        {
            isOpen = false;

            if (confirmationWindow != null)
                confirmationWindow.SetActive(false);
        }

        public void ExitToMainMenu()
        {
            if (string.IsNullOrWhiteSpace(mainMenuSceneName))
            {
                Debug.LogError("[EXIT] Main Menu scene name is empty.");
                return;
            }

            Debug.Log("[EXIT] Loading Main Menu: " + mainMenuSceneName);

            if (Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
            {
                SceneManager.LoadScene(mainMenuSceneName);
            }
            else
            {
                Debug.LogError(
                    "[EXIT] Main Menu scene could not be loaded. " +
                    "Add the scene to Build Settings or update the configured scene name."
                );
            }
        }

        public void QuitToDesktop()
        {
            Debug.Log("[EXIT] Quit to desktop requested.");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void SelectCancel()
        {
            if (cancelButton == null)
                return;

            cancelButton.Select();
        }
    }
}
