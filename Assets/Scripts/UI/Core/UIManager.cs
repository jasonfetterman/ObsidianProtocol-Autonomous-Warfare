using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI Configuration")]
        [SerializeField] private bool persistAcrossScenes = true;

        private readonly Dictionary<string, GameObject> registeredScreens = new();
        private readonly Stack<GameObject> screenHistory = new();

        private GameObject activeScreen;
        private GameObject activeModal;

        public GameObject ActiveScreen => activeScreen;
        public GameObject ActiveModal => activeModal;
        public bool HasActiveModal => activeModal != null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (persistAcrossScenes && transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public bool RegisterScreen(string id, GameObject screen)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogError("[UIManager] Cannot register screen with empty ID.");
                return false;
            }

            if (screen == null)
            {
                Debug.LogError($"[UIManager] Cannot register null screen: {id}");
                return false;
            }

            if (registeredScreens.ContainsKey(id))
            {
                Debug.LogWarning($"[UIManager] Screen already registered: {id}");
                return false;
            }

            registeredScreens.Add(id, screen);
            screen.SetActive(false);

            return true;
        }

        public bool UnregisterScreen(string id)
        {
            if (!registeredScreens.Remove(id))
            {
                Debug.LogWarning($"[UIManager] Screen not registered: {id}");
                return false;
            }

            return true;
        }

        public bool OpenScreen(string id)
        {
            if (!registeredScreens.TryGetValue(id, out GameObject screen))
            {
                Debug.LogError($"[UIManager] Screen not registered: {id}");
                return false;
            }

            if (activeScreen != null && activeScreen != screen)
            {
                screenHistory.Push(activeScreen);
                activeScreen.SetActive(false);
            }

            activeScreen = screen;
            activeScreen.SetActive(true);

            return true;
        }

        public void CloseScreen()
        {
            if (activeScreen == null)
                return;

            activeScreen.SetActive(false);
            activeScreen = null;
        }

        public bool GoBack()
        {
            if (activeScreen != null)
                activeScreen.SetActive(false);

            if (screenHistory.Count == 0)
            {
                activeScreen = null;
                return false;
            }

            activeScreen = screenHistory.Pop();
            activeScreen.SetActive(true);

            return true;
        }

        public bool OpenModal(GameObject modal)
        {
            if (modal == null)
            {
                Debug.LogError("[UIManager] Cannot open null modal.");
                return false;
            }

            if (activeModal != null)
                activeModal.SetActive(false);

            activeModal = modal;
            activeModal.SetActive(true);

            return true;
        }

        public void CloseModal()
        {
            if (activeModal == null)
                return;

            activeModal.SetActive(false);
            activeModal = null;
        }

        public void CloseAll()
        {
            if (activeScreen != null)
                activeScreen.SetActive(false);

            if (activeModal != null)
                activeModal.SetActive(false);

            activeScreen = null;
            activeModal = null;
            screenHistory.Clear();
        }

        public bool HasScreen(string id)
        {
            return registeredScreens.ContainsKey(id);
        }

        public GameObject GetScreen(string id)
        {
            registeredScreens.TryGetValue(id, out GameObject screen);
            return screen;
        }
    }
}