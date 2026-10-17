using UnityEngine;

namespace ObsidianProtocol.Game.Operations
{
    public sealed class OperationsRuntime : MonoBehaviour
    {
        public static OperationsRuntime Instance { get; private set; }

        public bool Initialized { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialized = true;

            Debug.Log(
                "[OPERATIONS] Runtime initialized.");
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
