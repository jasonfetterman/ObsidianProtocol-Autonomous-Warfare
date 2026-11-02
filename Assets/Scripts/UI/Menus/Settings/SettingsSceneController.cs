using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ObsidianProtocol.UI.Menus
{
    public class SettingsSceneController : MonoBehaviour
    {
        private readonly string[] allPanels =
        {
            "GENERAL_SETTINGS",
            "DISPLAY_SETTINGS",
            "AUDIO_SETTINGS",
            "CONTROL_SETTINGS",
            "INTERFACE_SETTINGS",
            "ACCESSIBILITY_SETTINGS"
        };

        private void Awake()
        {
            WireNavigation();
            WireControls();

            ShowGameplay();

            Debug.Log("[SETTINGS] Settings HUD initialized.");
        }

        // =========================================================
        // NAVIGATION
        // =========================================================

        private void WireNavigation()
        {
            WireButton("GAMEPLAY", ShowGameplay);
            WireButton("CONTROLS", ShowControls);
            WireButton("AUDIO", ShowAudio);
            WireButton("VIDEO", ShowDisplay);
            WireButton("VR", ShowVR);
            WireButton("HUD", ShowHUD);
            WireButton("ACCESSIBILITY", ShowAccessibility);
            WireButton("NETWORK", ShowNetwork);
            WireButton("ACCOUNT", ShowAccount);

            WireButton("BACK", GoBack);
            WireButton("PREVIOUS", GoBack);

            WireButton("CLOSE_CUSTOMIZATION", CloseCustomization);
        }

        private void WireButton(string objectName, UnityEngine.Events.UnityAction action)
        {
            Button button = FindButton(objectName);

            if (button == null)
            {
                Debug.LogWarning("[SETTINGS] Button not found: " + objectName);
                return;
            }

            button.onClick.RemoveListener(action);
            button.onClick.AddListener(action);
        }

        // =========================================================
        // CATEGORY PANELS
        // =========================================================

        public void ShowGameplay()
        {
            ShowSettingsPanel("GENERAL_SETTINGS", "GAMEPLAY");
        }

        public void ShowControls()
        {
            ShowSettingsPanel("CONTROL_SETTINGS", "CONTROLS");
        }

        public void ShowAudio()
        {
            ShowSettingsPanel("AUDIO_SETTINGS", "AUDIO");
        }

        public void ShowDisplay()
        {
            ShowSettingsPanel("DISPLAY_SETTINGS", "VIDEO");
        }

        public void ShowAccessibility()
        {
            ShowSettingsPanel("ACCESSIBILITY_SETTINGS", "ACCESSIBILITY");
        }

        public void ShowVR()
        {
            Debug.Log("[SETTINGS] VR selected.");

            if (Application.CanStreamedLevelBeLoaded("SCN20_VR_COMMAND"))
            {
                SceneManager.LoadScene("SCN20_VR_COMMAND");
            }
            else
            {
                Debug.LogWarning(
                    "[SETTINGS] VR_Command is not available in Build Settings."
                );
            }
        }

        public void ShowHUD()
        {
            GameObject content = FindObject("SETTINGS_CONTENT");
            GameObject customization = FindObject("HUD_CUSTOMIZATION");

            if (content != null)
                content.SetActive(false);

            if (customization != null)
                customization.SetActive(true);

            MoveActiveIndicator("HUD");
        }

        public void CloseCustomization()
        {
            GameObject content = FindObject("SETTINGS_CONTENT");
            GameObject customization = FindObject("HUD_CUSTOMIZATION");

            if (customization != null)
                customization.SetActive(false);

            if (content != null)
                content.SetActive(true);

            ShowGameplay();
        }

        public void ShowNetwork()
        {
            Debug.Log("[SETTINGS] Network selected.");
            ShowGameplay();
        }

        public void ShowAccount()
        {
            Debug.Log("[SETTINGS] Account selected.");
            ShowGameplay();
        }

        private void ShowSettingsPanel(string targetPanelName, string buttonName)
        {
            GameObject content = FindObject("SETTINGS_CONTENT");
            GameObject customization = FindObject("HUD_CUSTOMIZATION");

            if (content != null)
                content.SetActive(true);

            if (customization != null)
                customization.SetActive(false);

            foreach (string panelName in allPanels)
            {
                GameObject panel = FindObject(panelName);

                if (panel != null)
                {
                    panel.SetActive(
                        string.Equals(
                            panelName,
                            targetPanelName,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );
                }
            }

            MoveActiveIndicator(buttonName);

            Debug.Log("[SETTINGS] Active panel: " + targetPanelName);
        }

        // =========================================================
        // ACTIVE INDICATOR
        // =========================================================

        private void MoveActiveIndicator(string buttonName)
        {
            GameObject buttonObject = FindObject(buttonName);
            GameObject indicator = FindObject("ACTIVE_INDICATOR");

            if (buttonObject == null || indicator == null)
                return;

            RectTransform buttonRect =
                buttonObject.GetComponent<RectTransform>();

            RectTransform indicatorRect =
                indicator.GetComponent<RectTransform>();

            if (buttonRect == null || indicatorRect == null)
                return;

            indicator.transform.SetParent(
                buttonObject.transform,
                false
            );

            indicator.transform.SetAsFirstSibling();

            indicatorRect.anchorMin = new Vector2(0f, 0f);
            indicatorRect.anchorMax = new Vector2(0f, 1f);
            indicatorRect.pivot = new Vector2(0f, 0.5f);

            indicatorRect.anchoredPosition = Vector2.zero;
            indicatorRect.sizeDelta =
                new Vector2(
                    indicatorRect.sizeDelta.x,
                    0f
                );
        }

        // =========================================================
        // CONTROLS
        // =========================================================

        private void WireControls()
        {
            WireSlider("Master_Volume");
            WireSlider("Music_Volume");
            WireSlider("Effects_Volume");
            WireSlider("Voice_Volume");

            WireSlider("Brightness");
            WireSlider("Contrast");
            WireSlider("HUD_Scale");
            WireSlider("Mouse_Sensitivity");

            WireButton("APPLY_AUDIO_SETTINGS", ApplyAudio);
            WireButton("APPLY_DISPLAY_SETTINGS", ApplyDisplay);
            WireButton("APPLY_CONTROL_SETTINGS", ApplyControls);
            WireButton("APPLY_ACCESSIBILITY_SETTINGS", ApplyAccessibility);
            WireButton("APPLY_INTERFACE_SETTINGS", ApplyInterface);
        }

        private void WireSlider(string objectName)
        {
            GameObject objectRoot = FindObject(objectName);

            if (objectRoot == null)
            {
                Debug.LogWarning(
                    "[SETTINGS] Slider object not found: " + objectName
                );
                return;
            }

            Slider slider =
                objectRoot.GetComponentInChildren<Slider>(true);

            if (slider == null)
            {
                Debug.LogWarning(
                    "[SETTINGS] No Slider component found under: " +
                    objectName
                );
                return;
            }

            slider.onValueChanged.RemoveListener(
                delegate { UpdateSliderLabel(objectRoot, slider); }
            );

            slider.onValueChanged.AddListener(
                value => UpdateSliderLabel(objectRoot, slider)
            );

            UpdateSliderLabel(objectRoot, slider);
        }

        private void UpdateSliderLabel(
            GameObject objectRoot,
            Slider slider
        )
        {
            Text valueText = null;

            Transform valueTransform =
                objectRoot.transform.Find("VALUE");

            if (valueTransform != null)
                valueText =
                    valueTransform.GetComponent<Text>();

            if (valueText == null)
            {
                Text[] texts =
                    objectRoot.GetComponentsInChildren<Text>(true);

                if (texts.Length > 0)
                    valueText = texts[texts.Length - 1];
            }

            if (valueText == null)
                return;

            float value = slider.value;

            if (slider.maxValue <= 1.01f)
            {
                valueText.text =
                    Mathf.RoundToInt(value * 100f) + "%";
            }
            else
            {
                valueText.text =
                    value.ToString("0.0");
            }
        }

        // =========================================================
        // APPLY
        // =========================================================

        public void ApplyAudio()
        {
            Debug.Log("[SETTINGS] Audio settings applied.");
        }

        public void ApplyDisplay()
        {
            Debug.Log("[SETTINGS] Display settings applied.");
        }

        public void ApplyControls()
        {
            Debug.Log("[SETTINGS] Control settings applied.");
        }

        public void ApplyAccessibility()
        {
            Debug.Log("[SETTINGS] Accessibility settings applied.");
        }

        public void ApplyInterface()
        {
            Debug.Log("[SETTINGS] Interface settings applied.");
        }

        // =========================================================
        // EXIT
        // =========================================================

        public void GoBack()
        {
            Debug.Log("[SETTINGS] Returning to Main Menu.");

            if (Application.CanStreamedLevelBeLoaded("SCN-01 MAIN MENU"))
            {
                SceneManager.LoadScene("SCN-01 MAIN MENU");
                return;
            }

            SceneManager.LoadScene(0);
        }

        // =========================================================
        // FINDERS
        // =========================================================

        private GameObject FindObject(string objectName)
        {
            Transform[] transforms =
                GetComponentsInChildren<Transform>(true);

            foreach (Transform transform in transforms)
            {
                if (string.Equals(
                    transform.name,
                    objectName,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return transform.gameObject;
                }
            }

            return null;
        }

        private Button FindButton(string objectName)
        {
            GameObject objectRoot = FindObject(objectName);

            if (objectRoot == null)
                return null;

            return objectRoot.GetComponentInChildren<Button>(true);
        }
    }
}

