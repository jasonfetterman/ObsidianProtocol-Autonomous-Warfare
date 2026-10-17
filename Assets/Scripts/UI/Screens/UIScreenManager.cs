using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    public class UIScreenManager : MonoBehaviour
    {
        public static UIScreenManager Instance { get; private set; }

        private readonly Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();
        public GameObject ActiveScreen { get; private set; }

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

        public void RegisterScreen(string id, GameObject screen)
        {
            if (string.IsNullOrEmpty(id) || screen == null)
            {
                Debug.LogError("[UIScreenManager] Invalid screen registration.");
                return;
            }

            screens[id] = screen;
            screen.SetActive(false);
        }

        // ---------------------------------------------------------
        // CONTROL
        // ---------------------------------------------------------

        public void ShowScreen(string id)
        {
            if (!screens.TryGetValue(id, out var screen))
            {
                Debug.LogError($"[UIScreenManager] Screen not found: {id}");
                return;
            }

            if (ActiveScreen != null)
                ActiveScreen.SetActive(false);

            ActiveScreen = screen;
            ActiveScreen.SetActive(true);
        }

        public void HideScreen(string id)
        {
            if (!screens.TryGetValue(id, out var screen))
                return;

            screen.SetActive(false);

            if (ActiveScreen == screen)
                ActiveScreen = null;
        }
    }
}
