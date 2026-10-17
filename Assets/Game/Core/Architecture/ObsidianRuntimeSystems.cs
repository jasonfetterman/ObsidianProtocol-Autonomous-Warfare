using UnityEngine;
using ObsidianProtocol.Game.Command;
using ObsidianProtocol.Game.CommandUnits;
using ObsidianProtocol.Game.Garage;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Research;
using ObsidianProtocol.Game.Progression;

namespace ObsidianProtocol.Game.Core
{
    public sealed class ObsidianRuntimeSystems : MonoBehaviour
    {
        public static ObsidianRuntimeSystems Instance { get; private set; }

        public FleetControlSystem Fleet { get; private set; }
        public FleetManagement FleetManagement { get; private set; }

        public IntelligenceProcessingSystem Intelligence { get; private set; }

        public GarageFramework Garage { get; private set; }

        public UnitInspection Inspection { get; private set; }

        public CommandUnitAutonomySystem Autonomy { get; private set; }

        public CommandSystem Commands { get; private set; }

        public WorldMappingSystem WorldMap { get; private set; }

        public ResearchManager Research { get; private set; }

        public FundingManager Funding { get; private set; }

        public ProgressionPersistence Progression { get; private set; }

        public bool Initialized { get; private set; }

        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null)
                return;

            GameObject root =
                new GameObject(
                    "[SYSTEM] OBSIDIAN RUNTIME SYSTEMS");

            root.AddComponent<ObsidianRuntimeSystems>();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);

            Fleet =
                new FleetControlSystem();

            FleetManagement =
                new FleetManagement();

            Intelligence =
                new IntelligenceProcessingSystem();

            Garage =
                new GarageFramework();

            Inspection =
                new UnitInspection();

            Autonomy =
                new CommandUnitAutonomySystem();

            Commands =
                new CommandSystem();

            WorldMap =
                new WorldMappingSystem();

            Progression =
                new ProgressionPersistence();

            Funding =
                FindAnyObjectByType<FundingManager>();

            Research =
                FindAnyObjectByType<ResearchManager>();

            Garage.Open();

            WorldMap.CreateMap(
                "STRATEGIC_WORLD");

            Initialized = true;

            Debug.Log(
                "[SYSTEMS] Obsidian runtime systems initialized.");
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
