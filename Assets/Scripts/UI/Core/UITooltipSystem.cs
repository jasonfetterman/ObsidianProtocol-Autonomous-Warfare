using UnityEngine;

namespace ObsidianProtocol.UI
{
    /// <summary>
    /// Central tooltip controller. Handles registration, text assignment, and visibility.
    /// </summary>
    public sealed class UITooltipSystem : MonoBehaviour
    {
        public static UITooltipSystem Instance { get; private set; }

        private GameObject tooltip;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // ---------------------------------------------------------
        // REGISTRATION
        // ---------------------------------------------------------

        public void RegisterTooltip(GameObject t)
        {
            tooltip = t;

            if (tooltip != null)
                tooltip.SetActive(false);
        }

        // ---------------------------------------------------------
        // CONTROL
        // ---------------------------------------------------------

        /// <summary>
        /// Show tooltip without text (legacy support).
        /// </summary>
        public void ShowTooltip()
        {
            if (tooltip == null)
                return;

            tooltip.SetActive(true);
        }

        /// <summary>
        /// Show tooltip with text (required by UIOrchestrator).
        /// </summary>
        public void ShowTooltip(string text)
        {
            if (tooltip == null)
                return;

            // TextMeshPro support
            var tmp = tooltip.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = text;

            // Legacy UnityEngine.UI.Text support
            var uiText = tooltip.GetComponentInChildren<UnityEngine.UI.Text>();
            if (uiText != null)
                uiText.text = text;

            tooltip.SetActive(true);
        }

        /// <summary>
        /// Hide tooltip.
        /// </summary>
        public void HideTooltip()
        {
            if (tooltip == null)
                return;

            tooltip.SetActive(false);
        }
    }
}
