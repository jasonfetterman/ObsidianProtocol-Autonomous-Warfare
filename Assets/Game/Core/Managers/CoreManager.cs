using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class CoreManager : GameManager
    {
        public static CoreManager Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        protected override void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            base.OnDestroy();
        }
    }
}
