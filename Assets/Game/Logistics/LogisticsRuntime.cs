using UnityEngine;
using ObsidianProtocol.Game.Resources;
using ObsidianProtocol.Game.Construction;

namespace ObsidianProtocol.Game.Logistics
{
    public sealed class LogisticsRuntime : MonoBehaviour
    {
        public static LogisticsRuntime Instance { get; private set; }

        public ResourceSystem Resources { get; private set; }
        public ResourceStorageSystem Storage { get; private set; }
        public SupplyDepotSystem Depots { get; private set; }
        public StrategicSupplyNetworkSystem Network { get; private set; }
        public AutonomousLogisticsSystem Autonomous { get; private set; }
        public EnergyEconomySystem Energy { get; private set; }
        public ResourceStructureSystem Structures { get; private set; }

        public bool Initialized { get; private set; }

        [Header("Resource Definitions")]
        [SerializeField] private ResourceDefinition meat;
        [SerializeField] private ResourceDefinition wood;
        [SerializeField] private ResourceDefinition coal;
        [SerializeField] private ResourceDefinition iron;
        [SerializeField] private ResourceDefinition alloy;
        [SerializeField] private ResourceDefinition electronics;
        [SerializeField] private ResourceDefinition fuel;
        [SerializeField] private ResourceDefinition energy;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            if (Instance != null)
            {
                return;
            }

            GameObject runtimeObject =
                new GameObject("[SYSTEM] LOGISTICS RUNTIME");

            runtimeObject.AddComponent<LogisticsRuntime>();
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

            LoadDefinitions();
            Initialize();
        }

        private void LoadDefinitions()
        {
            meat = UnityEngine.Resources.Load<ResourceDefinition>(
                "MeatResourceDefinition");

            wood = UnityEngine.Resources.Load<ResourceDefinition>(
                "WoodResourceDefinition");

            coal = UnityEngine.Resources.Load<ResourceDefinition>(
                "CoalResourceDefinition");

            iron = UnityEngine.Resources.Load<ResourceDefinition>(
                "IronResourceDefinition");

            alloy = UnityEngine.Resources.Load<ResourceDefinition>(
                "AlloyResourceDefinition");

            electronics = UnityEngine.Resources.Load<ResourceDefinition>(
                "ElectronicsResourceDefinition");

            fuel = UnityEngine.Resources.Load<ResourceDefinition>(
                "FuelResourceDefinition");

            energy = UnityEngine.Resources.Load<ResourceDefinition>(
                "EnergyResourceDefinition");
        }

        private void Initialize()
        {
            if (Initialized)
            {
                return;
            }

            Resources = new ResourceSystem();
            Storage = new ResourceStorageSystem();
            Depots = new SupplyDepotSystem();
            Network = new StrategicSupplyNetworkSystem();
            Autonomous = new AutonomousLogisticsSystem();
            Energy = new EnergyEconomySystem();
            Structures = new ResourceStructureSystem();

            RegisterResources();
            CreateMainStorage();
            CreateDepots();
            CreateNetwork();

            Initialized = true;

            Debug.Log(
                "[LOGISTICS] Runtime initialized with resources, storage, depots and network.");
        }

        private void RegisterResources()
        {
            RegisterResource(meat);
            RegisterResource(wood);
            RegisterResource(coal);
            RegisterResource(iron);
            RegisterResource(alloy);
            RegisterResource(electronics);
            RegisterResource(fuel);
            RegisterResource(energy);
        }

        private void RegisterResource(
            ResourceDefinition definition)
        {
            if (definition == null)
            {
                Debug.LogWarning(
                    "[LOGISTICS] Missing resource definition.");
                return;
            }

            Resources.RegisterDefinition(definition);
        }

        private void CreateMainStorage()
        {
            ResourceStorage mainStorage =
                new ResourceStorage(
                    "MAIN_LOGISTICS_STORAGE",
                    100000);

            mainStorage.SetResourceCapacity("meat", 15000);
            mainStorage.SetResourceCapacity("wood", 20000);
            mainStorage.SetResourceCapacity("coal", 10000);
            mainStorage.SetResourceCapacity("iron", 15000);
            mainStorage.SetResourceCapacity("alloy", 10000);
            mainStorage.SetResourceCapacity("electronics", 10000);
            mainStorage.SetResourceCapacity("fuel", 30000);
            mainStorage.SetResourceCapacity("energy", 30000);

            Storage.RegisterStorage(
                mainStorage);
        }


        private void CreateDepots()
        {
            CreateDepot(
                "NORTH_DEPOT",
                "North Depot",
                50000f);

            CreateDepot(
                "FOOD_STORAGE",
                "Food Storage",
                25000f);

            CreateDepot(
                "IRON_MINE",
                "Iron Mine",
                30000f);

            CreateDepot(
                "ELECTRONICS",
                "Electronics",
                20000f);
        }

        private void CreateDepot(
            string depotId,
            string locationId,
            float capacity)
        {
            SupplyDepot depot =
                new SupplyDepot(
                    depotId,
                    locationId,
                    capacity);

            Depots.RegisterDepot(
                depot);
        }

        private void CreateNetwork()
        {
            CreateNode(
                "COMMAND_CENTER",
                "Command Center",
                StrategicNodeType.CommandCenter);

            CreateNode(
                "NORTH_DEPOT",
                "North Depot",
                StrategicNodeType.SupplyDepot);

            CreateNode(
                "IRON_MINE",
                "Iron Mine",
                StrategicNodeType.ResourceSite);

            CreateNode(
                "ELECTRONICS",
                "Electronics",
                StrategicNodeType.ResourceSite);

            CreateNode(
                "FOOD_STORAGE",
                "Food Storage",
                StrategicNodeType.SupplyDepot);

            CreateNode(
                "FABRICATION",
                "Fabrication",
                StrategicNodeType.FabricationFacility);

            CreateNode(
                "FIELD_FORCES",
                "Field Forces",
                StrategicNodeType.ForwardOperatingLocation);

            CreateLink(
                "NORTH_DEPOT_COMMAND",
                "NORTH_DEPOT",
                "COMMAND_CENTER",
                1000f);

            CreateLink(
                "IRON_MINE_FABRICATION",
                "IRON_MINE",
                "FABRICATION",
                800f);

            CreateLink(
                "ELECTRONICS_COMMAND",
                "ELECTRONICS",
                "COMMAND_CENTER",
                600f);

            CreateLink(
                "FOOD_FIELD_FORCES",
                "FOOD_STORAGE",
                "FIELD_FORCES",
                700f);

            CreateLink(
                "FABRICATION_COMMAND",
                "FABRICATION",
                "COMMAND_CENTER",
                800f);

            CreateLink(
                "COMMAND_FIELD_FORCES",
                "COMMAND_CENTER",
                "FIELD_FORCES",
                1000f);
        }

        private void CreateNode(
            string nodeId,
            string name,
            StrategicNodeType type)
        {
            StrategicSupplyNode node =
                new StrategicSupplyNode(
                    nodeId,
                    name,
                    type);

            if (Network.RegisterNode(node))
            {
                node.Activate();
            }
        }

        private void CreateLink(
            string linkId,
            string origin,
            string destination,
            float capacity)
        {
            StrategicSupplyLink link =
                new StrategicSupplyLink(
                    linkId,
                    origin,
                    destination,
                    capacity);

            if (Network.RegisterLink(link))
            {
                Network.SetLinkActive(
                    linkId,
                    true);
            }
        }

        private void Update()
        {
            if (!Initialized || Energy == null || Resources == null)
                return;

            Energy.Update(Time.deltaTime, Resources, Structures);
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








