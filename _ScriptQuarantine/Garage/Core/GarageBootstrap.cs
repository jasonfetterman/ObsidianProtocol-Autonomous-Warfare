using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageBootstrap : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        private GarageConfiguration configuration;

        [Header("Systems")]
        [SerializeField]
        private GarageModeController modeController;

        [SerializeField]
        private GarageSessionManager sessionManager;

        [SerializeField]
        private GarageUnitFactory unitFactory;

        [SerializeField]
        private GarageUnitRegistry unitRegistry;

        [SerializeField]
        private GarageFleetController fleetController;

        [SerializeField]
        private GarageSaveManager saveManager;

        public GarageConfiguration Configuration =>
            configuration;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (configuration == null)
            {
                Debug.LogWarning(
                    "GarageBootstrap: GarageConfiguration is not assigned.");
            }

            if (unitFactory != null &&
                configuration != null)
            {
                unitFactory.SetDatabase(
                    configuration.unitDatabase);
            }

            if (configuration != null &&
                configuration.logGarageInitialization)
            {
                Debug.Log(
                    "Obsidian Protocol Garage initialized.");
            }
        }

        public void Save()
        {
            if (saveManager == null ||
                sessionManager == null)
                return;

            GaragePersistenceState state =
                new GaragePersistenceState();

            state.activeUnitInstanceId =
                sessionManager.Session.activeUnitInstanceId;

            state.worldId =
                sessionManager.Session.worldId;

            state.onlineSession =
                sessionManager.Session.sessionMode ==
                GarageSessionMode.Online;

            saveManager.Save(state);
        }
    }
}
