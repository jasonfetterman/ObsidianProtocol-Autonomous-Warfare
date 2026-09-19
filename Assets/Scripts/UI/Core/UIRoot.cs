using UnityEngine;

namespace ObsidianProtocol.UI
{
    public sealed class UIRoot : MonoBehaviour
    {
        public static UIRoot Instance { get; private set; }

        [Header("UI Orchestrator")]
        [SerializeField] private UIOrchestrator orchestrator;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            BindOrchestrator();
        }

        private void BindOrchestrator()
        {
            if (orchestrator == null)
                orchestrator = UIOrchestrator.Instance;

            if (orchestrator == null)
                Debug.LogError("[UIRoot] UIOrchestrator is missing. Ensure it exists in the scene.");
        }
    }
}
