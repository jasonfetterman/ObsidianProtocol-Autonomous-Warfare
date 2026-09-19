using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    public class UIStateSystem : MonoBehaviour
    {
        public static UIStateSystem Instance { get; private set; }

        [Header("Registered States")]
        [SerializeField] private List<UIState> states = new List<UIState>();

        private readonly Dictionary<string, UIState> lookup = new Dictionary<string, UIState>();
        private UIState activeState;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Build lookup table
            foreach (var state in states)
            {
                if (state != null && !string.IsNullOrEmpty(state.stateId))
                {
                    lookup[state.stateId] = state;
                }
            }
        }

        /// <summary>
        /// Sets the active UI state by ID.
        /// </summary>
        public void SetState(string stateId)
        {
            if (!lookup.TryGetValue(stateId, out var state))
            {
                Debug.LogWarning($"[UIStateSystem] State not found: {stateId}");
                return;
            }

            // Deactivate previous state
            if (activeState != null)
                activeState.SetActive(false);

            // Activate new state
            activeState = state;
            activeState.SetActive(true);

            Debug.Log($"[UIStateSystem] Active state: {stateId}");
        }

        /// <summary>
        /// Clears the active state.
        /// </summary>
        public void ClearState()
        {
            if (activeState != null)
                activeState.SetActive(false);

            activeState = null;
        }
    }

    [System.Serializable]
    public class UIState
    {
        public string stateId;
        public GameObject root;

        public void SetActive(bool active)
        {
            if (root != null)
                root.SetActive(active);
        }
    }
}
