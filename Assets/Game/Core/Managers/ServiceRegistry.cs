using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class ServiceRegistry : MonoBehaviour
    {
        public static ServiceRegistry Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
