using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class ConfigurationService : MonoBehaviour
    {
        public static ConfigurationService Instance { get; private set; }

        public GameConfiguration Current { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Current = new GameConfiguration();
        }

        public void Apply()
        {
            AudioListener.volume = Mathf.Clamp01(Current.MasterVolume);
            Application.targetFrameRate = Current.TargetFrameRate;

            Screen.fullScreen = Current.Fullscreen;
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
