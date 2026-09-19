using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    /// <summary>
    /// Handles lightweight popups. Only one popup is active at a time.
    /// </summary>
    public sealed class UIPopupManager : MonoBehaviour
    {
        public static UIPopupManager Instance { get; private set; }

        private readonly Dictionary<string, GameObject> popups = new Dictionary<string, GameObject>();
        private GameObject activePopup;

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

        public void RegisterPopup(string id, GameObject popup)
        {
            if (string.IsNullOrEmpty(id) || popup == null)
            {
                Debug.LogError("[UIPopupManager] Invalid popup registration.");
                return;
            }

            popups[id] = popup;
            popup.SetActive(false);
        }

        // ---------------------------------------------------------
        // CONTROL
        // ---------------------------------------------------------

        public void OpenPopup(string id)
        {
            if (!popups.TryGetValue(id, out var popup))
            {
                Debug.LogError($"[UIPopupManager] Popup not found: {id}");
                return;
            }

            if (activePopup != null)
                activePopup.SetActive(false);

            activePopup = popup;
            activePopup.SetActive(true);
        }

        public void ClosePopup()
        {
            if (activePopup == null)
                return;

            activePopup.SetActive(false);
            activePopup = null;
        }
    }
}
