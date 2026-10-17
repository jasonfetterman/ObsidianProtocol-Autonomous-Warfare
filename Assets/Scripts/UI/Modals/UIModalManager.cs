using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    /// <summary>
    /// Centralized modal controller. Handles registration, opening, closing,
    /// and ensures only one modal is active at a time.
    /// </summary>
    public sealed class UIModalManager : MonoBehaviour
    {
        public static UIModalManager Instance { get; private set; }

        private readonly Dictionary<string, GameObject> modals = new Dictionary<string, GameObject>();
        private GameObject activeModal;

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

        public void RegisterModal(string id, GameObject modal)
        {
            if (string.IsNullOrEmpty(id) || modal == null)
            {
                Debug.LogError("[UIModalManager] Invalid modal registration.");
                return;
            }

            modals[id] = modal;
            modal.SetActive(false);
        }

        // ---------------------------------------------------------
        // CONTROL
        // ---------------------------------------------------------

        public void OpenModal(string id)
        {
            if (!modals.TryGetValue(id, out var modal))
            {
                Debug.LogError($"[UIModalManager] Modal not found: {id}");
                return;
            }

            if (activeModal != null)
                activeModal.SetActive(false);

            activeModal = modal;
            activeModal.SetActive(true);
        }

        public void CloseModal()
        {
            if (activeModal == null)
                return;

            activeModal.SetActive(false);
            activeModal = null;
        }
    }
}
