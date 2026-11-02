using UnityEngine;
using ObsidianProtocol.Game.Logistics;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Research;
using ObsidianProtocol.Game.Operations;
using ObsidianProtocol.Game.Intelligence;
using ObsidianProtocol.Game.OpenWorld;
using ObsidianProtocol.Game.OnlineWorld;
using ObsidianProtocol.Game.Deployment;

namespace ObsidianProtocol.Game.Core
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        public static GameBootstrap Instance { get; private set; }

        public bool SystemsReady { get; private set; }

        public FleetPersistence Fleet { get; private set; }
        public ResearchManager Research { get; private set; }
        public LogisticsRuntime Logistics { get; private set; }
        public OperationsRuntime Operations { get; private set; }
        public IntelligenceRuntime Intelligence { get; private set; }
        public WorldSimulation WorldSimulation { get; private set; }
        public MatchHistory MilitaryHistory { get; private set; }
        public SceneNavigation SceneNavigation { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null)
                return;

            GameObject root = new GameObject("[SYSTEM] GAME BOOTSTRAP");
            root.AddComponent<GameBootstrap>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning(
                    "[GameBootstrap] Duplicate bootstrap detected. Destroying duplicate.");

                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializePersistentGameState();

            if (BattlefieldDeploymentContext.Instance == null)
            {
                GameObject deploymentContextObject =
                    new GameObject("[SYSTEM] BATTLEFIELD DEPLOYMENT CONTEXT");

                deploymentContextObject.AddComponent<BattlefieldDeploymentContext>();
            }

            if (SaveLoadService.Exists())
            {
                try
                {
                    SaveLoadService.Load();
                    Debug.Log("[GameBootstrap] Existing campaign save loaded.");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[GameBootstrap] Campaign load failed: " + ex);
                }
            }

            InitializeEconomy();
            InitializeFleet();
            InitializeResearch();
            InitializeLogistics();
            InitializeOperations();
            InitializeIntelligence();
            InitializeWorldSimulation();
            InitializeMilitaryHistory();
            InitializeSceneNavigation();

            Debug.Log("[GameBootstrap] Bootstrap initialized.");
        }

        private void InitializePersistentGameState()
        {
            if (PersistentGameState.Instance != null)
                return;

            GameObject stateObject =
                new GameObject("[SYSTEM] PERSISTENT GAME STATE");

            stateObject.AddComponent<PersistentGameState>();

            Debug.Log(
                "[GameBootstrap] PersistentGameState initialized.");
        }

        private void InitializeEconomy()
        {
            if (PersistentGameState.Instance == null)
                return;

            if (!PersistentGameState.Instance.IsInitialized)
                return;

            if (EconomyRuntime.CampaignActive)
                return;

            EconomyRuntime.StartNewCampaign(
                PersistentGameState.Instance.CampaignId,
                10000,
                PersistentGameState.Instance.PlayerId);

            Debug.Log(
                "[GameBootstrap] Economy initialized.");
        }

        private void InitializeFleet()
        {
            if (Fleet != null && Fleet.Initialized)
                return;

            Fleet = new FleetPersistence();
            Fleet.Initialize();

            Debug.Log(
                "[GameBootstrap] Fleet initialized.");
        }

        private void InitializeResearch()
        {
            Research =
                FindAnyObjectByType<ResearchManager>();

            if (Research != null)
            {
                Debug.Log(
                    "[GameBootstrap] Research initialized.");
                return;
            }

            GameObject researchObject =
                new GameObject("[SYSTEM] RESEARCH MANAGER");

            Research =
                researchObject.AddComponent<ResearchManager>();

            DontDestroyOnLoad(researchObject);

            Debug.Log(
                "[GameBootstrap] Research initialized.");
        }

        private void InitializeLogistics()
        {
            Logistics =
                LogisticsRuntime.Instance;

            if (Logistics != null &&
                Logistics.Initialized)
            {
                Debug.Log(
                    "[GameBootstrap] Logistics initialized.");
                return;
            }

            Debug.LogWarning(
                "[GameBootstrap] LogisticsRuntime is not ready yet.");
        }

        private void InitializeOperations()
        {
            Operations =
                OperationsRuntime.Instance;

            if (Operations != null &&
                Operations.Initialized)
            {
                Debug.Log(
                    "[GameBootstrap] Operations initialized.");
                return;
            }

            GameObject operationsObject =
                new GameObject("[SYSTEM] OPERATIONS RUNTIME");

            Operations =
                operationsObject.AddComponent<OperationsRuntime>();

            DontDestroyOnLoad(operationsObject);

            Debug.Log(
                "[GameBootstrap] Operations initialized.");
        }

        private void InitializeIntelligence()
        {
            Intelligence =
                IntelligenceRuntime.Instance;

            if (Intelligence != null)
            {
                Debug.Log(
                    "[GameBootstrap] Intelligence initialized.");
                return;
            }

            GameObject intelligenceObject =
                new GameObject("[SYSTEM] INTELLIGENCE RUNTIME");

            Intelligence =
                intelligenceObject.AddComponent<IntelligenceRuntime>();

            DontDestroyOnLoad(intelligenceObject);

            Debug.Log(
                "[GameBootstrap] Intelligence initialized.");
        }

        private void InitializeWorldSimulation()
        {
            if (WorldSimulation != null &&
                WorldSimulation.Initialized)
            {
                return;
            }

            WorldSimulation =
                new WorldSimulation();

            WorldSimulation.Initialize(1f);

            Debug.Log(
                "[GameBootstrap] WorldSimulation initialized.");
        }

        private void InitializeMilitaryHistory()
        {
            if (MilitaryHistory != null &&
                MilitaryHistory.Initialized)
            {
                return;
            }

            MilitaryHistory =
                new MatchHistory();

            MilitaryHistory.Initialize();

            Debug.Log(
                "[GameBootstrap] MilitaryHistory initialized.");
        }

        private void InitializeSceneNavigation()
        {
            if (SceneNavigation != null &&
                SceneNavigation.Initialized)
            {
                return;
            }

            SceneNavigation =
                new SceneNavigation();

            SceneNavigation.Initialize();

            Debug.Log(
                "[GameBootstrap] SceneNavigation initialized.");
        }
        private void Start()
        {
            if (ObsidianRuntimeSystems.Instance == null)
            {
                Debug.LogError(
                    "[GameBootstrap] ObsidianRuntimeSystems was not initialized.");
                return;
            }

            SystemsReady =
                ObsidianRuntimeSystems.Instance.Initialized;

            if (SaveLoadService.Exists())
            {
                try
                {
                    SaveLoadService.Load();
                    Debug.Log("[GameBootstrap] Existing campaign save loaded.");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("[GameBootstrap] Campaign load failed: " + ex);
                }
            }


            Debug.Log(
                $"[GameBootstrap] Existing runtime systems ready: {SystemsReady}");
        }

        public void RestoreWorldSimulation(WorldSimulation simulation)
        {
            if (simulation != null && simulation.Initialized)
                WorldSimulation = simulation;
        }
        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}











