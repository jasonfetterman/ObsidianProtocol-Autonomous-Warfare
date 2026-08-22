using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class SystemRegistry : MonoBehaviour
    {
        public static SystemRegistry Instance { get; private set; }

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
    }
}
