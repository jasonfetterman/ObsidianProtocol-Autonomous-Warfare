using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace ObsidianProtocol.UI.Popups.System
{
    public class POP024_ExitConfirmation : MonoBehaviour
    {
        public const string ID = "POP-024";

        private TMP_Text messageText;
        private GameObject confirmationWindow;
        private GameObject dimOverlay;
        private Button cancelButton;
        private Button confirmButton;

        private void Awake()
        {
            Debug.Log("[SCN-15] POP-024 Awake.");

            SetupEventSystem();
            SetupCanvasRaycaster();
            DisableBlockingBackgroundRaycasts();
            FindUI();
            WireButtons();
        }

        private void Start()
        {
            FindUI();

            if (confirmationWindow != null)
                confirmationWindow.SetActive(true);

            if (dimOverlay != null)
                dimOverlay.SetActive(true);

            Debug.Log("[SCN-15] POP-024 screen ACTIVE.");
        }

        public void Initialize()
        {
            Debug.Log("[SCN-15] POP-024 initialized.");
        }

        private void SetupEventSystem()
        {
            EventSystem eventSystem =
                UnityEngine.Object.FindFirstObjectByType<EventSystem>();

            if (eventSystem == null)
            {
                GameObject eventObject = new GameObject("EVENT SYSTEM");
                eventSystem = eventObject.AddComponent<EventSystem>();
                eventObject.AddComponent<StandaloneInputModule>();
            }

            BaseInputModule inputModule =
                eventSystem.GetComponent<BaseInputModule>();

            if (inputModule == null)
            {
                inputModule =
                    eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            }

            eventSystem.enabled = true;
            inputModule.enabled = true;
        }

        private void SetupCanvasRaycaster()
        {
            Canvas canvas = GetComponentInParent<Canvas>();

            if (canvas == null)
            {
                GameObject hud = GameObject.Find("EXIT CONFIRMATION HUD");

                if (hud != null)
                    canvas = hud.GetComponent<Canvas>();
            }

            if (canvas == null)
            {
                Debug.LogError(
                    "[SCN-15] EXIT CONFIRMATION HUD Canvas not found."
                );
                return;
            }

            GraphicRaycaster raycaster =
                canvas.GetComponent<GraphicRaycaster>();

            if (raycaster == null)
                raycaster = canvas.gameObject.AddComponent<GraphicRaycaster>();

            raycaster.enabled = true;
        }

        private void DisableBlockingBackgroundRaycasts()
        {
            string[] names =
            {
                "BACKGROUND",
                "DIM OVERLAY",
                "HEADER ACCENT"
            };

            foreach (string objectName in names)
            {
                GameObject obj = GameObject.Find(objectName);

                if (obj == null)
                    continue;

                Graphic graphic = obj.GetComponent<Graphic>();

                if (graphic != null)
                    graphic.raycastTarget = false;
            }
        }

        private void FindUI()
        {
            GameObject window =
                GameObject.Find("EXIT CONFIRMATION WINDOW");

            if (window != null)
                confirmationWindow = window;

            GameObject overlay =
                GameObject.Find("DIM OVERLAY");

            if (overlay != null)
                dimOverlay = overlay;

            GameObject message =
                GameObject.Find("CONFIRMATION MESSAGE");

            if (message != null)
            {
                messageText = message.GetComponent<TMP_Text>();

                if (messageText != null)
                {
                    messageText.text =
                        "Are you sure you want to exit Obsidian Protocol?";
                }
            }

            GameObject cancel =
                GameObject.Find("CANCEL");

            if (cancel != null)
            {
                cancelButton = cancel.GetComponent<Button>();

                if (cancelButton == null)
                {
                    cancelButton =
                        cancel.GetComponentInChildren<Button>(true);
                }
            }

            GameObject confirm =
                GameObject.Find("CONFIRM EXIT");

            if (confirm != null)
            {
                confirmButton = confirm.GetComponent<Button>();

                if (confirmButton == null)
                {
                    confirmButton =
                        confirm.GetComponentInChildren<Button>(true);
                }
            }
        }

        private void WireButtons()
        {
            if (cancelButton != null)
                cancelButton.interactable = true;

            if (confirmButton != null)
                confirmButton.interactable = true;
        }

        public void CancelExit()
        {
            Debug.Log(
                "[SCN-15] CANCEL CLICKED. Returning to Main Menu."
            );

            LoadMainMenu();
        }

        public void ConfirmExit()
        {
            Debug.Log("[SCN-15] CONFIRM EXIT CLICKED.");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void LoadMainMenu()
        {
            int buildCount =
                SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < buildCount; i++)
            {
                string scenePath =
                    SceneUtility.GetScenePathByBuildIndex(i);

                if (
                    scenePath.IndexOf(
                        "MAIN MENU",
                        StringComparison.OrdinalIgnoreCase
                    ) >= 0
                    &&
                    scenePath.EndsWith(
                        ".unity",
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    Debug.Log(
                        "[SCN-15] Loading Main Menu at Build Index " + i
                    );

                    SceneManager.LoadScene(i);
                    return;
                }
            }

            Debug.LogError(
                "[SCN-15] MAIN MENU scene was not found in Build Settings."
            );
        }

        public void SetMessage(TMP_Text message)
        {
            messageText = message;

            if (messageText != null)
            {
                messageText.text =
                    "Are you sure you want to exit Obsidian Protocol?";
            }
        }
    }
}
