using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldBootstrap : MonoBehaviour
    {
        [Header("Core World Systems")]
        [SerializeField]
        private PersistentWorldManager worldManager;

        [SerializeField]
        private WorldSpawnPointRegistry spawnPointRegistry;

        [SerializeField]
        private WorldUnitRegistry unitRegistry;

        [SerializeField]
        private WorldEntityManager entityManager;

        [SerializeField]
        private WorldPersistenceController persistenceController;

        [SerializeField]
        private WorldRuntimeSynchronizer runtimeSynchronizer;

        [SerializeField]
        private WorldControlManager controlManager;

        [Header("Startup")]
        [SerializeField]
        private bool initializeOnAwake = true;

        [SerializeField]
        private bool offlineWorld = true;

        private void Awake()
        {
            if (!initializeOnAwake)
                return;

            InitializeWorld();
        }

        public void InitializeWorld()
        {
            if (worldManager != null)
            {
                worldManager.Initialize();

                if (offlineWorld)
                    worldManager.SetOffline();
                else
                    worldManager.SetOnline();
            }

            if (spawnPointRegistry != null)
                spawnPointRegistry.Refresh();

            if (persistenceController != null)
                persistenceController
                    .SynchronizeRegistry();

            if (runtimeSynchronizer != null)
                runtimeSynchronizer.Synchronize();

            if (controlManager != null)
                controlManager.ResetControl();

            Debug.Log(
                "OBSIDIAN PROTOCOL WORLD BOOTSTRAP COMPLETE.");
        }

        public void SynchronizeWorld()
        {
            if (persistenceController != null)
                persistenceController
                    .SynchronizeRegistry();

            if (runtimeSynchronizer != null)
                runtimeSynchronizer.Synchronize();
        }

        public void CaptureWorld()
        {
            if (runtimeSynchronizer != null)
                runtimeSynchronizer
                    .CaptureRuntimeTransforms();
        }
    }
}
