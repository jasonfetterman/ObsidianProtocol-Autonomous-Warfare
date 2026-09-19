using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    /// <summary>
    /// Controls all UI windows. Only one window can be active at a time.
    /// </summary>
    public sealed class UIWindowManager : MonoBehaviour
    {
        public static UIWindowManager Instance { get; private set; }

        private readonly Dictionary<string, GameObject> windows = new Dictionary<string, GameObject>();
        private GameObject activeWindow;

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

        public void RegisterWindow(string id, GameObject window)
        {
            if (string.IsNullOrEmpty(id) || window == null)
            {
                Debug.LogError("[UIWindowManager] Invalid window registration.");
                return;
            }

            windows[id] = window;
            window.SetActive(false);
        }

        // ---------------------------------------------------------
        // CONTROL
        // ---------------------------------------------------------

        public void OpenWindow(string id)
        {
            if (!windows.TryGetValue(id, out var window))
            {
                Debug.LogError($"[UIWindowManager] Window not found: {id}");
                return;
            }

            if (activeWindow != null)
                activeWindow.SetActive(false);

            activeWindow = window;
            activeWindow.SetActive(true);
        }

        public void CloseWindow()
        {
            if (activeWindow == null)
                return;

            activeWindow.SetActive(false);
            activeWindow = null;
        }
    }
}
