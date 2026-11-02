using UnityEngine;

namespace ObsidianProtocol.UI
{
    /// <summary>
    /// Central binding layer for UI elements, screens, windows, modals, and popups.
    /// Ensures all UI components register themselves with the correct subsystem.
    /// </summary>
    public sealed class UIBinder : MonoBehaviour
    {
        public static UIBinder Instance { get; private set; }

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
        // SCREEN BINDING
        // ---------------------------------------------------------

        public void BindScreen(string id, GameObject screen)
        {
            if (UIScreenManager.Instance == null)
            {
                Debug.LogError("[UIBinder] UIScreenManager missing.");
                return;
            }

            UIScreenManager.Instance.RegisterScreen(id, screen);
        }

        // ---------------------------------------------------------
        // WINDOW BINDING
        // ---------------------------------------------------------

        public void BindWindow(string id, GameObject window)
        {
            if (UIWindowManager.Instance == null)
            {
                Debug.LogError("[UIBinder] UIWindowManager missing.");
                return;
            }

            UIWindowManager.Instance.RegisterWindow(id, window);
        }

        // ---------------------------------------------------------
        // MODAL BINDING
        // ---------------------------------------------------------

        public void BindModal(string id, GameObject modal)
        {
            if (UIModalManager.Instance == null)
            {
                Debug.LogError("[UIBinder] UIModalManager missing.");
                return;
            }

            UIModalManager.Instance.RegisterModal(id, modal);
        }

        // ---------------------------------------------------------
        // POPUP BINDING
        // ---------------------------------------------------------

        public void BindPopup(string id, GameObject popup)
        {
            if (UIPopupManager.Instance == null)
            {
                Debug.LogError("[UIBinder] UIPopupManager missing.");
                return;
            }

            UIPopupManager.Instance.RegisterPopup(id, popup);
        }

        // ---------------------------------------------------------
        // NOTIFICATION BINDING
        // ---------------------------------------------------------

        public void BindNotification(string id, GameObject notification)
        {
            if (UINotificationManager.Instance == null)
            {
                Debug.LogError("[UIBinder] UINotificationManager missing.");
                return;
            }

            UINotificationManager.Instance.RegisterNotification(id, notification);
        }

        // ---------------------------------------------------------
        // TOOLTIP BINDING
        // ---------------------------------------------------------

        public void BindTooltip(GameObject tooltip)
        {
            if (UITooltipSystem.Instance == null)
            {
                Debug.LogError("[UIBinder] UITooltipSystem missing.");
                return;
            }

            UITooltipSystem.Instance.RegisterTooltip(tooltip);
        }
    }
}
