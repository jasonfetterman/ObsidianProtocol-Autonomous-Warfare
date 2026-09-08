using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace MiniRTS
{
    /// <summary>
    /// The only authored scene component. It constructs the complete milestone-six
    /// world at runtime, using imported models over gameplay-only primitive colliders.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    [DisallowMultipleComponent]
    public sealed class GameBootstrap : MonoBehaviour
    {
        private static readonly Vector3 PlayerStart = new Vector3(-32f, 0f, -29f);
        private static readonly Vector3 EnemyStart = new Vector3(32f, 0f, 29f);

        [Header("Runtime Material Textures")]
        [SerializeField] private Texture2D terrainGrassTexture;
        [SerializeField] private Texture2D terrainDirtTexture;
        [SerializeField] private Texture2D rockTexture;
        [SerializeField] private Texture2D blueCrystalTexture;
        [SerializeField] private Texture2D metalBuildingPanelTexture;

        private bool hasBuilt;
        private Material terrainMaterial;
        private Material dirtMaterial;
        private Transform unitsRoot;
        private Transform buildingsRoot;
        private int playerWorkerNumber;
        private int enemyWorkerNumber;
        private int playerMarineNumber;
        private int enemyMarineNumber;
        private int playerTankNumber;
        private int enemyTankNumber;

        public WalkGrid WalkGrid { get; private set; }
        public Camera MainCamera { get; private set; }
        public CameraController CameraController { get; private set; }
        public SelectionController SelectionController { get; private set; }
        public BuildingPlacementController BuildingPlacementController { get; private set; }
        public CommandCardUI CommandCardUI { get; private set; }
        public HelpOverlayUI HelpOverlayUI { get; private set; }
        public EconomyHUD EconomyHUD { get; private set; }
        public FogOfWar FogOfWar { get; private set; }
        public MinimapUI MinimapUI { get; private set; }
        public EnemyAIController EnemyAIController { get; private set; }
        public GameStateController GameStateController { get; private set; }
        public PlayerEconomy PlayerEconomy { get; private set; }
        public PlayerEconomy EnemyEconomy { get; private set; }
        public Headquarters PlayerHeadquarters { get; private set; }
        public Headquarters EnemyHeadquarters { get; private set; }

        private void Awake()
        {
            BuildWorld();
        }

        public void ConfigureMaterialTextures(
            Texture2D grass,
            Texture2D dirt,
            Texture2D rock,
            Texture2D crystal,
            Texture2D buildingPanel)
        {
            terrainGrassTexture = grass;
            terrainDirtTexture = dirt;
            rockTexture = rock;
            blueCrystalTexture = crystal;
            metalBuildingPanelTexture = buildingPanel;
        }

        public void BuildWorld()
        {
            if (hasBuilt)
            {
                return;
            }

            hasBuilt = true;
            WalkGrid = MiniRTS.WalkGrid.CreateMapGrid();
            PlayerEconomy = new PlayerEconomy(
                BalanceConfig.PlayerOwnerId,
                BalanceConfig.StartingMinerals,
                BalanceConfig.StartingGas);
            EnemyEconomy = new PlayerEconomy(
                BalanceConfig.EnemyOwnerId,
                BalanceConfig.StartingMinerals,
                BalanceConfig.StartingGas);
            CreateMaterials();

            Transform worldRoot = CreateRoot("GeneratedWorld", transform);
            ConfigureEnvironment(worldRoot);
            CreateTerrain(worldRoot);
            CreateRockObstacles(CreateRoot("RockObstacles", worldRoot));
            CreateResourceFields(CreateRoot("Resources", worldRoot));
            CreateStartLocations(CreateRoot("StartLocations", worldRoot));
            unitsRoot = CreateRoot("Units", worldRoot);
            buildingsRoot = CreateRoot("Buildings", worldRoot);
            CreateHeadquarters(buildingsRoot);
            SpawnStartingWorkers();
            CreateCameraRig(worldRoot);
            CreateGameSystems(worldRoot);
        }

        private void CreateMaterials()
        {
            terrainMaterial = CreateMaterial(
                "TerrainMaterial",
                new Color(0.62f, 0.72f, 0.58f),
                0.05f,
                texture: terrainGrassTexture,
                textureTiling: new Vector2(20f, 20f));
            dirtMaterial = CreateMaterial(
                "TerrainDirtMaterial",
                new Color(0.72f, 0.68f, 0.62f),
                0.04f,
                texture: terrainDirtTexture,
                textureTiling: Vector2.one);
        }

        private static void ConfigureEnvironment(Transform parent)
        {
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.34f, 0.38f, 0.43f);
            RenderSettings.fog = false;

            GameObject lightObject = new GameObject("Sun");
            Light sun = lightObject.AddComponent<Light>();
            lightObject.transform.SetParent(parent, false);
            sun.type = LightType.Directional;
            sun.color = new Color(1f, 0.95f, 0.86f);
            sun.intensity = 1.25f;
            sun.shadows = LightShadows.Soft;
            lightObject.transform.rotation = Quaternion.Euler(52f, -32f, 0f);
        }

        private void CreateTerrain(Transform parent)
        {
            GameObject terrain = GameObject.CreatePrimitive(PrimitiveType.Plane);
            terrain.name = "Terrain_96x96";
            terrain.transform.SetParent(parent, false);
            terrain.transform.localPosition = Vector3.zero;
            float planeScale = BalanceConfig.MapSize * BalanceConfig.CellSize / 10f;
            terrain.transform.localScale = new Vector3(planeScale, 1f, planeScale);
            terrain.GetComponent<Renderer>().sharedMaterial = terrainMaterial;
            terrain.isStatic = true;

            Transform patchRoot = CreateRoot("DirtPatches", parent);
            CreateDirtPatch(
                "PlayerBaseDirt",
                PlayerStart + new Vector3(1.5f, 0f, 0f),
                new Vector2(13.5f, 10.5f),
                7f,
                11,
                patchRoot);
            CreateDirtPatch(
                "EnemyBaseDirt",
                EnemyStart + new Vector3(-1.5f, 0f, 0f),
                new Vector2(13.5f, 10.5f),
                -173f,
                29,
                patchRoot);
            CreateDirtPatch(
                "CentralDirt",
                new Vector3(0f, 0f, 0f),
                new Vector2(8.5f, 5.5f),
                -24f,
                47,
                patchRoot);
        }

        private void CreateDirtPatch(
            string objectName,
            Vector3 center,
            Vector2 radii,
            float rotationDegrees,
            int seed,
            Transform parent)
        {
            const int segmentCount = 40;
            Vector3[] vertices = new Vector3[segmentCount + 1];
            Vector2[] uvs = new Vector2[segmentCount + 1];
            int[] triangles = new int[segmentCount * 3];
            vertices[0] = Vector3.zero;
            uvs[0] = Vector2.zero;

            float rotation = rotationDegrees * Mathf.Deg2Rad;
            for (int i = 0; i < segmentCount; i++)
            {
                float angle = i * Mathf.PI * 2f / segmentCount;
                float variation =
                    0.9f +
                    Mathf.PerlinNoise(
                        seed * 0.137f + Mathf.Cos(angle) * 0.8f,
                        seed * 0.173f + Mathf.Sin(angle) * 0.8f) * 0.18f;
                Vector2 ellipse = new Vector2(
                    Mathf.Cos(angle) * radii.x * variation,
                    Mathf.Sin(angle) * radii.y * variation);
                float rotatedX =
                    ellipse.x * Mathf.Cos(rotation) -
                    ellipse.y * Mathf.Sin(rotation);
                float rotatedZ =
                    ellipse.x * Mathf.Sin(rotation) +
                    ellipse.y * Mathf.Cos(rotation);
                vertices[i + 1] = new Vector3(rotatedX, 0f, rotatedZ);
                uvs[i + 1] = new Vector2(
                    rotatedX / 4.5f,
                    rotatedZ / 4.5f);

                int next = (i + 1) % segmentCount;
                int triangle = i * 3;
                triangles[triangle] = 0;
                triangles[triangle + 1] = next + 1;
                triangles[triangle + 2] = i + 1;
            }

            Mesh mesh = new Mesh
            {
                name = $"{objectName}Mesh",
                vertices = vertices,
                uv = uvs,
                triangles = triangles
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            GameObject patch = new GameObject(
                objectName,
                typeof(MeshFilter),
                typeof(MeshRenderer));
            patch.transform.SetParent(parent, false);
            patch.transform.position = new Vector3(center.x, 0.012f, center.z);
            patch.GetComponent<MeshFilter>().sharedMesh = mesh;
            MeshRenderer renderer = patch.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = dirtMaterial;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = true;
            patch.isStatic = true;
        }

        private void CreateRockObstacles(Transform parent)
        {
            System.Random random = new System.Random(BalanceConfig.RandomSeed);
            RockCluster[] clusters =
            {
                new RockCluster(new Vector3(-2f, 0f, 1f), 7, 6f),
                new RockCluster(new Vector3(-14f, 0f, 17f), 5, 4.5f),
                new RockCluster(new Vector3(15f, 0f, -16f), 6, 5f),
                new RockCluster(new Vector3(-27f, 0f, 7f), 4, 3.5f),
                new RockCluster(new Vector3(28f, 0f, -7f), 5, 4f)
            };

            int rockNumber = 1;
            for (int clusterIndex = 0; clusterIndex < clusters.Length; clusterIndex++)
            {
                RockCluster cluster = clusters[clusterIndex];
                Transform clusterRoot = CreateRoot(
                    $"RockCluster_{clusterIndex + 1:00}",
                    parent);

                for (int i = 0; i < cluster.Count; i++)
                {
                    float angle = (float)(random.NextDouble() * Math.PI * 2.0);
                    float distance = (float)random.NextDouble() * cluster.Radius;
                    Vector3 position = cluster.Center + new Vector3(
                        Mathf.Cos(angle) * distance,
                        0f,
                        Mathf.Sin(angle) * distance);
                    float width = Mathf.Lerp(2.1f, 4.2f, (float)random.NextDouble());
                    float depth = Mathf.Lerp(2.0f, 4f, (float)random.NextDouble());
                    float height = Mathf.Lerp(1.5f, 3.8f, (float)random.NextDouble());

                    int modelVariant = rockNumber - 1;
                    GameObject rock = CreatePrimitiveColliderBody(
                        PrimitiveType.Cube,
                        $"Rock_{rockNumber:00}");
                    rockNumber++;
                    rock.transform.SetParent(clusterRoot, false);
                    rock.transform.position = new Vector3(position.x, height * 0.5f, position.z);
                    rock.transform.localScale = new Vector3(width, height, depth);
                    rock.transform.rotation = Quaternion.Euler(
                        0f,
                        (float)random.NextDouble() * 360f,
                        Mathf.Lerp(-4f, 4f, (float)random.NextDouble()));
                    ModelVisualFactory.Create(
                        ModelLibrary.GetRock(modelVariant),
                        rock.transform);
                    rock.isStatic = true;

                    float blockingRadius = Mathf.Sqrt(width * width + depth * depth) * 0.52f;
                    WalkGrid.SetCircleWalkable(position, blockingRadius, false);
                }
            }
        }

        private void CreateResourceFields(Transform parent)
        {
            CreateMineralField(
                CreateRoot("PlayerMineralField", parent),
                new Vector3(-21.5f, 0f, -29f),
                0f);
            CreateGeyser(
                CreateRoot("PlayerGeyser", parent),
                new Vector3(-27f, 0f, -19f),
                0);

            CreateMineralField(
                CreateRoot("EnemyMineralField", parent),
                new Vector3(21.5f, 0f, 29f),
                180f);
            CreateGeyser(
                CreateRoot("EnemyGeyser", parent),
                new Vector3(27f, 0f, 19f),
                1);
        }

        private void CreateMineralField(Transform parent, Vector3 center, float rotation)
        {
            Vector3[] offsets =
            {
                new Vector3(-3.3f, 0f, -1.15f),
                new Vector3(-1.1f, 0f, -1.45f),
                new Vector3(1.1f, 0f, -1.45f),
                new Vector3(3.3f, 0f, -1.15f),
                new Vector3(-3.3f, 0f, 1.15f),
                new Vector3(-1.1f, 0f, 1.45f),
                new Vector3(1.1f, 0f, 1.45f),
                new Vector3(3.3f, 0f, 1.15f)
            };

            Quaternion fieldRotation = Quaternion.Euler(0f, rotation, 0f);
            for (int i = 0; i < offsets.Length; i++)
            {
                Vector3 position = center + fieldRotation * offsets[i];
                Transform node = CreateRoot($"MineralCrystal_{i + 1:00}", parent);
                node.position = position;
                ResourceNode resourceNode = node.gameObject.AddComponent<ResourceNode>();
                resourceNode.Initialize(
                    ResourceType.Minerals,
                    BalanceConfig.MineralNodeAmount,
                    false,
                    BalanceConfig.MineralInteractionRadius);

                GameObject mainCrystal = CreatePrimitiveColliderChild(
                    PrimitiveType.Cube,
                    "CrystalCollider",
                    node,
                    new Vector3(0f, 0.72f, 0f),
                    new Vector3(0.72f, 1.45f, 0.72f));
                mainCrystal.transform.localRotation = Quaternion.Euler(8f, 45f, 8f);

                GameObject accentCrystal = CreatePrimitiveColliderChild(
                    PrimitiveType.Cube,
                    "CrystalAccentCollider",
                    node,
                    new Vector3(0.48f, 0.48f, 0.18f),
                    new Vector3(0.38f, 0.92f, 0.38f));
                accentCrystal.transform.localRotation = Quaternion.Euler(14f, 38f, -12f);
                ModelVisualFactory.Create(
                    ModelLibrary.GetMineral(i),
                    node,
                    "MineralModel");
                TintMineralCrystals(node);
                WalkGrid.SetCircleWalkable(position, 0.72f, false);
            }
        }

        private static void TintMineralCrystals(Transform node)
        {
            // Kenney crystal materials import pink/green; minerals read as blue.
            // Instanced materials rather than property blocks: ModelVisual's
            // tint blocks and the SRP batcher both ignore per-index blocks.
            Color mineralBlue = new Color(0.45f, 0.65f, 1f, 1f);
            foreach (Renderer renderer in node.GetComponentsInChildren<Renderer>())
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == null)
                    {
                        continue;
                    }

                    string materialName = materials[i].name.ToLowerInvariant();
                    if (materialName.Contains("crystal") ||
                        materialName.Contains("colormap"))
                    {
                        materials[i].SetColor("_BaseColor", mineralBlue);
                    }
                    else if (materialName.Contains("rock"))
                    {
                        // SpaceKit rocks import Mars-salmon; neutral grey base.
                        materials[i].SetColor(
                            "_BaseColor", new Color(0.55f, 0.56f, 0.6f, 1f));
                    }
                }
            }
        }

        private void CreateGeyser(
            Transform parent,
            Vector3 position,
            int modelVariant)
        {
            parent.position = position;
            ResourceNode resourceNode = parent.gameObject.AddComponent<ResourceNode>();
            resourceNode.Initialize(
                ResourceType.Gas,
                BalanceConfig.GasGeyserAmount,
                true,
                BalanceConfig.GeyserInteractionRadius);

            GameObject baseObject = CreatePrimitiveColliderChild(
                PrimitiveType.Cylinder,
                "GeyserBaseCollider",
                parent,
                new Vector3(0f, 0.35f, 0f),
                new Vector3(1.7f, 0.35f, 1.7f));
            baseObject.isStatic = true;

            GameObject vent = CreatePrimitiveColliderChild(
                PrimitiveType.Cylinder,
                "GasVentCollider",
                parent,
                new Vector3(0f, 0.7f, 0f),
                new Vector3(1.05f, 0.16f, 1.05f));
            vent.isStatic = true;

            ModelVisualFactory.Create(
                ModelLibrary.GetGeyser(modelVariant),
                parent,
                "GeyserModel");

            WalkGrid.SetCircleWalkable(
                position,
                BalanceConfig.GeyserBlockingRadius,
                false);
        }

        private void CreateStartLocations(Transform parent)
        {
            CreateStartMarker("PlayerStart", PlayerStart, BalanceConfig.PlayerColor, parent);
            CreateStartMarker("EnemyStart", EnemyStart, BalanceConfig.EnemyColor, parent);
        }

        private static void CreateStartMarker(
            string markerName,
            Vector3 position,
            Color color,
            Transform parent)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            marker.name = markerName;
            marker.transform.SetParent(parent, false);
            marker.transform.position = new Vector3(position.x, 0.024f, position.z);
            marker.transform.localScale = new Vector3(3.2f, 0.005f, 3.2f);
            marker.GetComponent<Renderer>().material.color =
                new Color(color.r * 0.45f, color.g * 0.45f, color.b * 0.45f);
            DisableAndDestroyCollider(marker);
        }

        private void CreateHeadquarters(Transform parent)
        {
            PlayerHeadquarters = CreateHeadquartersBox(
                "PlayerHQ",
                PlayerStart,
                BalanceConfig.PlayerOwnerId,
                BalanceConfig.PlayerColor,
                PlayerEconomy,
                position => CreateWorker(
                    position,
                    BalanceConfig.PlayerOwnerId,
                    BalanceConfig.PlayerColor,
                    PlayerEconomy));
            PlayerHeadquarters.transform.SetParent(parent, true);

            EnemyHeadquarters = CreateHeadquartersBox(
                "EnemyHQ",
                EnemyStart,
                BalanceConfig.EnemyOwnerId,
                BalanceConfig.EnemyColor,
                EnemyEconomy,
                position => CreateWorker(
                    position,
                    BalanceConfig.EnemyOwnerId,
                    BalanceConfig.EnemyColor,
                    EnemyEconomy));
            EnemyHeadquarters.transform.SetParent(parent, true);
        }

        private Headquarters CreateHeadquartersBox(
            string objectName,
            Vector3 position,
            int ownerId,
            Color factionColor,
            PlayerEconomy factionEconomy,
            Func<Vector3, Unit> workerFactory)
        {
            GameObject headquartersObject = CreatePrimitiveColliderBody(
                PrimitiveType.Cube,
                objectName);
            headquartersObject.transform.position = new Vector3(
                position.x,
                BalanceConfig.HeadquartersHeight * 0.5f,
                position.z);
            headquartersObject.transform.localScale = new Vector3(
                BalanceConfig.HeadquartersFootprintWidth,
                BalanceConfig.HeadquartersHeight,
                BalanceConfig.HeadquartersFootprintDepth);
            ModelVisualFactory.Create(
                ModelLibrary.Get(BuildingType.Headquarters),
                headquartersObject.transform);

            Headquarters headquarters =
                headquartersObject.AddComponent<Headquarters>();
            headquarters.InitializeHeadquarters(
                WalkGrid,
                factionEconomy,
                ownerId,
                factionColor,
                workerFactory);
            return headquarters;
        }

        private void SpawnStartingWorkers()
        {
            Vector3[] playerOffsets =
            {
                new Vector3(4.4f, 0f, -2.4f),
                new Vector3(4.4f, 0f, 0f),
                new Vector3(4.4f, 0f, 2.4f),
                new Vector3(6.1f, 0f, -2.4f),
                new Vector3(6.1f, 0f, 0f),
                new Vector3(6.1f, 0f, 2.4f)
            };
            SpawnFactionWorkers(
                PlayerStart,
                playerOffsets,
                BalanceConfig.PlayerOwnerId,
                BalanceConfig.PlayerColor,
                PlayerEconomy);

            Vector3[] enemyOffsets =
            {
                new Vector3(-4.4f, 0f, 2.4f),
                new Vector3(-4.4f, 0f, 0f),
                new Vector3(-4.4f, 0f, -2.4f),
                new Vector3(-6.1f, 0f, 2.4f),
                new Vector3(-6.1f, 0f, 0f),
                new Vector3(-6.1f, 0f, -2.4f)
            };
            SpawnFactionWorkers(
                EnemyStart,
                enemyOffsets,
                BalanceConfig.EnemyOwnerId,
                BalanceConfig.EnemyColor,
                EnemyEconomy);
        }

        private void SpawnFactionWorkers(
            Vector3 start,
            Vector3[] offsets,
            int ownerId,
            Color factionColor,
            PlayerEconomy factionEconomy)
        {
            for (int i = 0; i < BalanceConfig.StartingWorkerCount; i++)
            {
                if (!factionEconomy.TryUseSupply(
                        BalanceConfig.WorkerSupplyCost))
                {
                    throw new InvalidOperationException(
                        "Starting HQ does not provide enough supply for starting workers.");
                }

                Unit worker = CreateWorker(
                    start + offsets[i],
                    ownerId,
                    factionColor,
                    factionEconomy);
                if (worker == null)
                {
                    factionEconomy.ReleaseSupply(
                        BalanceConfig.WorkerSupplyCost);
                    throw new InvalidOperationException(
                        $"Could not place starting worker for owner {ownerId}.");
                }
            }
        }

        private Unit CreateWorker(
            Vector3 requestedPosition,
            int ownerId,
            Color factionColor,
            PlayerEconomy factionEconomy)
        {
            return CreateUnit(
                UnitType.Worker,
                requestedPosition,
                ownerId,
                factionColor,
                factionEconomy);
        }

        private Unit CreateUnit(
            UnitType type,
            Vector3 requestedPosition,
            int ownerId,
            Color factionColor,
            PlayerEconomy factionEconomy)
        {
            Vector2Int requestedCell = WalkGrid.WorldToCell(requestedPosition);
            if (!WalkGrid.TryFindNearestWalkable(
                    requestedCell,
                    Mathf.Max(WalkGrid.Width, WalkGrid.Height),
                    out Vector2Int spawnCell))
            {
                return null;
            }

            PrimitiveType primitive = type == UnitType.Tank
                ? PrimitiveType.Cube
                : PrimitiveType.Capsule;
            GameObject unitObject = CreatePrimitiveColliderBody(
                primitive,
                GetNextUnitName(type, ownerId));
            unitObject.transform.SetParent(unitsRoot, false);
            Vector3 spawnPosition = WalkGrid.CellToWorld(spawnCell);
            UnitDefinition definition = UnitDefinition.Get(type);
            spawnPosition.y = definition.GroundHeight;
            unitObject.transform.position = spawnPosition;
            unitObject.transform.localScale = GetUnitScale(type);
            ModelVisualFactory.Create(
                ModelLibrary.Get(type),
                unitObject.transform);

            Unit unit = unitObject.AddComponent<Unit>();
            UnitMover mover = unitObject.AddComponent<UnitMover>();
            mover.Initialize(WalkGrid);
            unit.Initialize(
                ownerId,
                factionColor,
                type,
                factionEconomy,
                true);
            Combatant combatant = unitObject.AddComponent<Combatant>();
            combatant.Initialize();

            if (type == UnitType.Worker)
            {
                WorkerGatherer gatherer =
                    unitObject.AddComponent<WorkerGatherer>();
                unitObject.AddComponent<WorkerBuilder>();
                gatherer.Initialize(factionEconomy);
            }

            return unit;
        }

        private Building CreatePlayerBuilding(
            BuildingType type,
            Vector3 snappedPosition,
            ResourceNode geyser)
        {
            return CreateBuilding(
                type,
                snappedPosition,
                geyser,
                BalanceConfig.PlayerOwnerId,
                BalanceConfig.PlayerColor,
                PlayerEconomy);
        }

        private Building CreateEnemyBuilding(
            BuildingType type,
            Vector3 snappedPosition,
            ResourceNode geyser)
        {
            return CreateBuilding(
                type,
                snappedPosition,
                geyser,
                BalanceConfig.EnemyOwnerId,
                BalanceConfig.EnemyColor,
                EnemyEconomy);
        }

        private Building CreateBuilding(
            BuildingType type,
            Vector3 snappedPosition,
            ResourceNode geyser,
            int ownerId,
            Color factionColor,
            PlayerEconomy factionEconomy)
        {
            BuildingDefinition definition = BuildingDefinition.Get(type);
            string factionName =
                ownerId == BalanceConfig.PlayerOwnerId
                    ? "Player"
                    : "Enemy";
            GameObject buildingObject = CreatePrimitiveColliderBody(
                PrimitiveType.Cube,
                $"{factionName}{type}");
            buildingObject.transform.SetParent(buildingsRoot, false);
            buildingObject.transform.position = new Vector3(
                snappedPosition.x,
                definition.Height * 0.5f,
                snappedPosition.z);
            buildingObject.transform.localScale = new Vector3(
                definition.Footprint.x,
                definition.Height,
                definition.Footprint.y);
            ModelVisualFactory.Create(
                ModelLibrary.Get(type),
                buildingObject.transform);

            Building building;
            switch (type)
            {
                case BuildingType.SupplyDepot:
                    SupplyDepot depot = buildingObject.AddComponent<SupplyDepot>();
                    depot.InitializeSupplyDepot(
                        WalkGrid,
                        factionEconomy,
                        ownerId,
                        factionColor,
                        false);
                    building = depot;
                    break;
                case BuildingType.Barracks:
                    Barracks barracks = buildingObject.AddComponent<Barracks>();
                    barracks.InitializeBarracks(
                        WalkGrid,
                        factionEconomy,
                        ownerId,
                        factionColor,
                        (unitType, position) => CreateUnit(
                            unitType,
                            position,
                            ownerId,
                            factionColor,
                            factionEconomy));
                    building = barracks;
                    break;
                case BuildingType.Factory:
                    Factory factory = buildingObject.AddComponent<Factory>();
                    factory.InitializeFactory(
                        WalkGrid,
                        factionEconomy,
                        ownerId,
                        factionColor,
                        (unitType, position) => CreateUnit(
                            unitType,
                            position,
                            ownerId,
                            factionColor,
                            factionEconomy));
                    building = factory;
                    break;
                case BuildingType.Refinery:
                    if (geyser == null)
                    {
                        Destroy(buildingObject);
                        return null;
                    }

                    Refinery refinery = buildingObject.AddComponent<Refinery>();
                    refinery.InitializeRefinery(
                        WalkGrid,
                        factionEconomy,
                        ownerId,
                        factionColor,
                        geyser);
                    building = refinery;
                    break;
                default:
                    Destroy(buildingObject);
                    return null;
            }

            return building;
        }

        private void CreateCameraRig(Transform parent)
        {
            GameObject rigObject = new GameObject("RTSCameraRig");
            rigObject.transform.SetParent(parent, false);
            rigObject.transform.position = PlayerStart;

            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetParent(rigObject.transform, false);
            MainCamera = cameraObject.AddComponent<Camera>();
            MainCamera.fieldOfView = BalanceConfig.CameraFieldOfView;
            MainCamera.nearClipPlane = 0.2f;
            MainCamera.farClipPlane = 250f;
            MainCamera.clearFlags = CameraClearFlags.SolidColor;
            MainCamera.backgroundColor = new Color(0.12f, 0.16f, 0.2f);
            cameraObject.AddComponent<AudioListener>();

            CameraController = rigObject.AddComponent<CameraController>();
            CameraController.Initialize(
                MainCamera,
                BalanceConfig.MapSize * BalanceConfig.CellSize);
            CameraController.FocusOn(PlayerStart + new Vector3(5f, 0f, 4f));
        }

        private void CreateGameSystems(Transform parent)
        {
            GameObject systems = new GameObject("GameSystems");
            systems.transform.SetParent(parent, false);
            BuildingPlacementController =
                systems.AddComponent<BuildingPlacementController>();
            BuildingPlacementController.Initialize(
                MainCamera,
                WalkGrid,
                PlayerEconomy,
                CreatePlayerBuilding);
            SelectionController = systems.AddComponent<SelectionController>();
            SelectionController.Initialize(
                MainCamera,
                WalkGrid,
                BuildingPlacementController);
            BuildingPlacementController.SetSelectionController(SelectionController);
            FogOfWar = systems.AddComponent<FogOfWar>();
            FogOfWar.Initialize(WalkGrid);
            EconomyHUD = systems.AddComponent<EconomyHUD>();
            EconomyHUD.Initialize(PlayerEconomy);
            CommandCardUI = systems.AddComponent<CommandCardUI>();
            CommandCardUI.Initialize(SelectionController, PlayerEconomy);
            HelpOverlayUI = systems.AddComponent<HelpOverlayUI>();
            HelpOverlayUI.Initialize();
            MinimapUI = systems.AddComponent<MinimapUI>();
            MinimapUI.Initialize(
                WalkGrid,
                FogOfWar,
                MainCamera,
                CameraController);
            EnemyAIController =
                systems.AddComponent<EnemyAIController>();
            EnemyAIController.Initialize(
                WalkGrid,
                EnemyEconomy,
                EnemyHeadquarters,
                BalanceConfig.EnemyOwnerId,
                CreateEnemyBuilding,
                BalanceConfig.RandomSeed);
            GameStateController =
                systems.AddComponent<GameStateController>();
            GameStateController.Initialize(
                BalanceConfig.PlayerOwnerId,
                BalanceConfig.EnemyOwnerId);
        }

        private string GetNextUnitName(UnitType type, int ownerId)
        {
            bool player = ownerId == BalanceConfig.PlayerOwnerId;
            int number;
            switch (type)
            {
                case UnitType.Worker:
                    number = player ? ++playerWorkerNumber : ++enemyWorkerNumber;
                    break;
                case UnitType.Marine:
                    number = player ? ++playerMarineNumber : ++enemyMarineNumber;
                    break;
                case UnitType.Tank:
                    number = player ? ++playerTankNumber : ++enemyTankNumber;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }

            return $"{(player ? "Player" : "Enemy")}{type}_{number:00}";
        }

        private static Vector3 GetUnitScale(UnitType type)
        {
            switch (type)
            {
                case UnitType.Worker:
                    return new Vector3(0.62f, 0.8f, 0.62f);
                case UnitType.Marine:
                    return new Vector3(0.72f, 0.95f, 0.72f);
                case UnitType.Tank:
                    return new Vector3(1.45f, 1.1f, 1.8f);
                default:
                    return Vector3.one;
            }
        }

        private static Transform CreateRoot(string objectName, Transform parent)
        {
            GameObject root = new GameObject(objectName);
            root.transform.SetParent(parent, false);
            return root.transform;
        }

        private static GameObject CreatePrimitiveColliderBody(
            PrimitiveType type,
            string objectName)
        {
            GameObject body = new GameObject(objectName);
            AddPrimitiveCollider(body, type);
            return body;
        }

        private static GameObject CreatePrimitiveColliderChild(
            PrimitiveType type,
            string objectName,
            Transform parent,
            Vector3 localPosition,
            Vector3 localScale)
        {
            GameObject child = CreatePrimitiveColliderBody(type, objectName);
            child.transform.SetParent(parent, false);
            child.transform.localPosition = localPosition;
            child.transform.localScale = localScale;
            return child;
        }

        private static void AddPrimitiveCollider(
            GameObject target,
            PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.Cube:
                    target.AddComponent<BoxCollider>();
                    break;
                case PrimitiveType.Capsule:
                case PrimitiveType.Cylinder:
                    target.AddComponent<CapsuleCollider>();
                    break;
                case PrimitiveType.Sphere:
                    target.AddComponent<SphereCollider>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(type),
                        type,
                        "Only primitive collider-backed entity bodies are supported.");
            }
        }

        private static void DisableAndDestroyCollider(GameObject target)
        {
            Collider targetCollider = target.GetComponent<Collider>();
            if (targetCollider != null)
            {
                targetCollider.enabled = false;
                Destroy(targetCollider);
            }
        }

        private static Material CreateMaterial(
            string materialName,
            Color color,
            float smoothness,
            Color? emission = null,
            Texture2D texture = null,
            Vector2? textureTiling = null)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            Material material = new Material(shader)
            {
                name = materialName,
                color = color
            };

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Smoothness"))
            {
                material.SetFloat("_Smoothness", smoothness);
            }

            if (texture != null)
            {
                texture.wrapMode = TextureWrapMode.Repeat;
                texture.filterMode = FilterMode.Trilinear;
                texture.anisoLevel = 4;
                Vector2 tiling = textureTiling ?? Vector2.one;
                material.mainTexture = texture;
                material.mainTextureScale = tiling;
                if (material.HasProperty("_BaseMap"))
                {
                    material.SetTexture("_BaseMap", texture);
                    material.SetTextureScale("_BaseMap", tiling);
                }

                if (material.HasProperty("_MainTex"))
                {
                    material.SetTexture("_MainTex", texture);
                    material.SetTextureScale("_MainTex", tiling);
                }
            }

            if (emission.HasValue && material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emission.Value);
            }

            return material;
        }

        private readonly struct RockCluster
        {
            public readonly Vector3 Center;
            public readonly int Count;
            public readonly float Radius;

            public RockCluster(Vector3 center, int count, float radius)
            {
                Center = center;
                Count = count;
                Radius = radius;
            }
        }
    }
}
