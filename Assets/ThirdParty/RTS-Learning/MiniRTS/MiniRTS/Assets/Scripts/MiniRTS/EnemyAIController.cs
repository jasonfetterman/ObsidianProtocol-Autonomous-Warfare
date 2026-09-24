using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Deterministic red-faction economy, construction, production, and wave control.
    /// The AI is omniscient, matching FogOfWar's documented red-observer policy, but
    /// Combatant still acquires targets only inside each unit's normal aggro radius.
    /// All units and sites are created through the same paid queues, worker builders,
    /// placement validator, gatherers, movers, and combatants used by the player.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class EnemyAIController : MonoBehaviour
    {
        private readonly List<ResourceNode> nearbyMinerals =
            new List<ResourceNode>();
        private readonly List<Unit> availableMilitary = new List<Unit>();
        private readonly HashSet<int> activeWaveUnitIds = new HashSet<int>();

        private WalkGrid grid;
        private PlayerEconomy economy;
        private Headquarters headquarters;
        private Func<BuildingType, Vector3, ResourceNode, Building>
            buildingFactory;
        private System.Random random;
        private Vector3 homePosition;
        private Vector3 defenderRallyPoint;
        private int ownerId;
        private int resourceCursor;
        private int waveIndex;
        private float nextThinkTime;
        private float nextWaveTime;
        private bool initialized;
        private bool defendingBase;

        public int WaveIndex => waveIndex;
        public float NextWaveTime => nextWaveTime;
        public bool IsDefendingBase => defendingBase;

        public void Initialize(
            WalkGrid walkGrid,
            PlayerEconomy factionEconomy,
            Headquarters factionHeadquarters,
            int factionOwnerId,
            Func<BuildingType, Vector3, ResourceNode, Building>
                createBuilding,
            int seed)
        {
            grid = walkGrid ??
                throw new ArgumentNullException(nameof(walkGrid));
            economy = factionEconomy ??
                throw new ArgumentNullException(nameof(factionEconomy));
            headquarters = factionHeadquarters ??
                throw new ArgumentNullException(nameof(factionHeadquarters));
            buildingFactory = createBuilding ??
                throw new ArgumentNullException(nameof(createBuilding));
            if (factionOwnerId == BalanceConfig.PlayerOwnerId)
            {
                throw new ArgumentOutOfRangeException(nameof(factionOwnerId));
            }

            ownerId = factionOwnerId;
            random = new System.Random(seed);
            homePosition = headquarters.transform.position;
            homePosition.y = 0f;
            defenderRallyPoint = FindDefenderRallyPoint();
            CacheNearbyMinerals();
            if (nearbyMinerals.Count > 0)
            {
                resourceCursor = random.Next(nearbyMinerals.Count);
            }

            nextThinkTime = Time.time;
            nextWaveTime =
                Time.time + BalanceConfig.EnemyAIFirstWaveSeconds;
            initialized = true;
            AssignIdleWorkers();
            UpdateProductionRallyPoints();
        }

        private void Update()
        {
            if (!initialized)
            {
                return;
            }

            float currentTime = Time.time;
            if (currentTime >= nextThinkTime)
            {
                nextThinkTime =
                    currentTime + BalanceConfig.EnemyAIThinkInterval;
                Think();
            }

            if (!defendingBase && currentTime >= nextWaveTime)
            {
                LaunchWave();
                nextWaveTime =
                    currentTime + BalanceConfig.EnemyAIWaveInterval;
            }
        }

        private void Think()
        {
            ResumeOrphanedConstruction();
            AssignIdleWorkers();
            HandleBaseDefense();
            ExecuteBuildOrder(EnemyAIBuildOrder.Decide(CaptureSnapshot()));
            UpdateProductionRallyPoints();
            if (!defendingBase)
            {
                RallyUncommittedDefenders();
            }
        }

        private EnemyAIEconomySnapshot CaptureSnapshot()
        {
            int workerCount = 0;
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit != null &&
                    unit.IsAlive &&
                    unit.OwnerId == ownerId &&
                    unit.Type == UnitType.Worker)
                {
                    workerCount++;
                }
            }

            int constructedBarracks = 0;
            bool supplyDepotUnderConstruction = false;
            bool barracksUnderConstruction = false;
            bool barracksCanQueue = false;
            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Building building = buildings[i];
                if (building == null ||
                    !building.IsAlive ||
                    building.OwnerId != ownerId)
                {
                    continue;
                }

                if (building.Type == BuildingType.SupplyDepot &&
                    !building.IsConstructed)
                {
                    supplyDepotUnderConstruction = true;
                }
                else if (building.Type == BuildingType.Barracks)
                {
                    if (!building.IsConstructed)
                    {
                        barracksUnderConstruction = true;
                        continue;
                    }

                    constructedBarracks++;
                    Barracks barracks = building as Barracks;
                    ProductionQueue queue =
                        barracks != null ? barracks.ProductionQueue : null;
                    barracksCanQueue |=
                        queue != null && queue.Count < queue.Capacity;
                }
            }

            ProductionQueue headquartersQueue =
                headquarters != null
                    ? headquarters.ProductionQueue
                    : null;
            bool headquartersCanQueue =
                headquarters != null &&
                headquarters.IsAlive &&
                headquarters.IsConstructed &&
                headquartersQueue != null &&
                headquartersQueue.Count < headquartersQueue.Capacity;
            int queuedWorkers =
                headquartersQueue != null ? headquartersQueue.Count : 0;

            return new EnemyAIEconomySnapshot(
                economy.Minerals,
                economy.Gas,
                economy.SupplyUsed,
                economy.SupplyCap,
                workerCount,
                queuedWorkers,
                headquartersCanQueue,
                supplyDepotUnderConstruction,
                constructedBarracks,
                barracksUnderConstruction,
                barracksCanQueue);
        }

        private void ExecuteBuildOrder(EnemyAIBuildAction action)
        {
            switch (action)
            {
                case EnemyAIBuildAction.TrainWorker:
                    if (headquarters != null)
                    {
                        headquarters.TryQueueWorker();
                    }

                    break;
                case EnemyAIBuildAction.BuildSupplyDepot:
                    TryPlaceBuilding(BuildingType.SupplyDepot);
                    break;
                case EnemyAIBuildAction.BuildBarracks:
                    TryPlaceBuilding(BuildingType.Barracks);
                    break;
                case EnemyAIBuildAction.TrainMarine:
                    TryQueueMarine();
                    break;
            }
        }

        private bool TryPlaceBuilding(BuildingType type)
        {
            BuildingDefinition definition = BuildingDefinition.Get(type);
            if (!TryFindBuildPosition(definition, out Vector3 position))
            {
                return false;
            }

            WorkerBuilder builder = FindClosestAvailableBuilder(position);
            if (builder == null ||
                !economy.TrySpend(
                    definition.MineralCost,
                    definition.GasCost))
            {
                return false;
            }

            Building site = buildingFactory(type, position, null);
            if (site != null && builder.BeginConstruction(site))
            {
                return true;
            }

            economy.Refund(definition.MineralCost, definition.GasCost);
            if (site != null)
            {
                Destroy(site.gameObject);
            }

            return false;
        }

        private bool TryFindBuildPosition(
            BuildingDefinition definition,
            out Vector3 position)
        {
            int sampleOffset =
                random.Next(BalanceConfig.EnemyAIBuildSearchSamplesPerRing);
            for (int radius = BalanceConfig.EnemyAIBuildSearchMinimumRadius;
                 radius <= BalanceConfig.EnemyAIBuildSearchMaximumRadius;
                 radius += 2)
            {
                for (int sample = 0;
                     sample < BalanceConfig.EnemyAIBuildSearchSamplesPerRing;
                     sample++)
                {
                    int sampleIndex =
                        (sample + sampleOffset) %
                        BalanceConfig.EnemyAIBuildSearchSamplesPerRing;
                    float angle =
                        sampleIndex * Mathf.PI * 2f /
                        BalanceConfig.EnemyAIBuildSearchSamplesPerRing;
                    Vector3 desired = homePosition + new Vector3(
                        Mathf.Cos(angle) * radius,
                        0f,
                        Mathf.Sin(angle) * radius);
                    Vector3 snapped = BuildingFootprint.SnapToGrid(
                        grid,
                        desired,
                        definition.Footprint);
                    snapped.y = 0f;
                    if (BuildingPlacementValidator.IsFootprintAvailable(
                            grid,
                            snapped,
                            definition.Footprint))
                    {
                        position = snapped;
                        return true;
                    }
                }
            }

            position = Vector3.zero;
            return false;
        }

        private WorkerBuilder FindClosestAvailableBuilder(Vector3 position)
        {
            WorkerBuilder closest = null;
            float closestDistanceSquared = float.MaxValue;
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null ||
                    !unit.IsAlive ||
                    unit.OwnerId != ownerId ||
                    unit.Type != UnitType.Worker)
                {
                    continue;
                }

                WorkerBuilder candidate =
                    unit.GetComponent<WorkerBuilder>();
                if (candidate == null || candidate.IsBuilding)
                {
                    continue;
                }

                Vector3 difference =
                    unit.transform.position - position;
                difference.y = 0f;
                float distanceSquared = difference.sqrMagnitude;
                if (distanceSquared < closestDistanceSquared)
                {
                    closest = candidate;
                    closestDistanceSquared = distanceSquared;
                }
            }

            return closest;
        }

        private void ResumeOrphanedConstruction()
        {
            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Building site = buildings[i];
                if (site == null ||
                    !site.IsAlive ||
                    site.IsConstructed ||
                    site.OwnerId != ownerId ||
                    HasAssignedBuilder(site))
                {
                    continue;
                }

                WorkerBuilder builder =
                    FindClosestAvailableBuilder(site.transform.position);
                if (builder != null)
                {
                    builder.BeginConstruction(site);
                }
            }
        }

        private bool HasAssignedBuilder(Building site)
        {
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null ||
                    !unit.IsAlive ||
                    unit.OwnerId != ownerId ||
                    unit.Type != UnitType.Worker)
                {
                    continue;
                }

                WorkerBuilder builder =
                    unit.GetComponent<WorkerBuilder>();
                if (builder != null && builder.Target == site)
                {
                    return true;
                }
            }

            return false;
        }

        private void TryQueueMarine()
        {
            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Barracks barracks = buildings[i] as Barracks;
                if (barracks != null &&
                    barracks.IsAlive &&
                    barracks.IsConstructed &&
                    barracks.OwnerId == ownerId &&
                    barracks.ProductionQueue.TryQueue(UnitType.Marine))
                {
                    return;
                }
            }
        }

        private void CacheNearbyMinerals()
        {
            nearbyMinerals.Clear();
            IReadOnlyList<ResourceNode> nodes = ResourceNode.ActiveNodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                ResourceNode node = nodes[i];
                if (node != null &&
                    node.Type == ResourceType.Minerals &&
                    node.IsHarvestable)
                {
                    nearbyMinerals.Add(node);
                }
            }

            nearbyMinerals.Sort(CompareMineralsByHomeDistance);
            if (nearbyMinerals.Count >
                BalanceConfig.StartingWorkerCount + 2)
            {
                nearbyMinerals.RemoveRange(
                    BalanceConfig.StartingWorkerCount + 2,
                    nearbyMinerals.Count -
                    BalanceConfig.StartingWorkerCount - 2);
            }
        }

        private int CompareMineralsByHomeDistance(
            ResourceNode left,
            ResourceNode right)
        {
            float leftDistance =
                (left.transform.position - homePosition).sqrMagnitude;
            float rightDistance =
                (right.transform.position - homePosition).sqrMagnitude;
            int comparison = leftDistance.CompareTo(rightDistance);
            return comparison != 0
                ? comparison
                : left.GetInstanceID().CompareTo(right.GetInstanceID());
        }

        private void AssignIdleWorkers()
        {
            RemoveDepletedMinerals();
            if (nearbyMinerals.Count == 0)
            {
                CacheNearbyMinerals();
            }

            if (nearbyMinerals.Count == 0)
            {
                return;
            }

            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null ||
                    !unit.IsAlive ||
                    unit.OwnerId != ownerId ||
                    unit.Type != UnitType.Worker)
                {
                    continue;
                }

                WorkerBuilder builder =
                    unit.GetComponent<WorkerBuilder>();
                WorkerGatherer gatherer =
                    unit.GetComponent<WorkerGatherer>();
                if (gatherer == null ||
                    (builder != null && builder.IsBuilding) ||
                    gatherer.Phase != GatherPhase.Idle)
                {
                    continue;
                }

                ResourceNode target =
                    nearbyMinerals[resourceCursor % nearbyMinerals.Count];
                resourceCursor =
                    (resourceCursor + 1) % nearbyMinerals.Count;
                gatherer.BeginGather(target);
            }
        }

        private void RemoveDepletedMinerals()
        {
            for (int i = nearbyMinerals.Count - 1; i >= 0; i--)
            {
                if (nearbyMinerals[i] == null ||
                    !nearbyMinerals[i].IsHarvestable)
                {
                    nearbyMinerals.RemoveAt(i);
                }
            }

            if (nearbyMinerals.Count > 0)
            {
                resourceCursor %= nearbyMinerals.Count;
            }
            else
            {
                resourceCursor = 0;
            }
        }

        private void HandleBaseDefense()
        {
            Unit threat = FindBaseThreat();
            if (threat == null)
            {
                defendingBase = false;
                return;
            }

            if (defendingBase)
            {
                return;
            }

            defendingBase = true;
            activeWaveUnitIds.Clear();
            IssueAttackMoveToMilitary(
                threat.transform.position,
                int.MaxValue,
                false);
        }

        private Unit FindBaseThreat()
        {
            Unit closestThreat = null;
            float closestDistanceSquared = float.MaxValue;
            float defenseRadiusSquared =
                BalanceConfig.EnemyAIBaseDefenseRadius *
                BalanceConfig.EnemyAIBaseDefenseRadius;
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int unitIndex = 0; unitIndex < units.Count; unitIndex++)
            {
                Unit unit = units[unitIndex];
                if (unit == null ||
                    !unit.IsAlive ||
                    unit.OwnerId == ownerId)
                {
                    continue;
                }

                for (int buildingIndex = 0;
                     buildingIndex < buildings.Count;
                     buildingIndex++)
                {
                    Building building = buildings[buildingIndex];
                    if (building == null ||
                        !building.IsAlive ||
                        building.OwnerId != ownerId)
                    {
                        continue;
                    }

                    float distance = building.DistanceTo(
                        unit.transform.position);
                    float distanceSquared = distance * distance;
                    if (distanceSquared <= defenseRadiusSquared &&
                        distanceSquared < closestDistanceSquared)
                    {
                        closestThreat = unit;
                        closestDistanceSquared = distanceSquared;
                    }
                }
            }

            return closestThreat;
        }

        private void LaunchWave()
        {
            activeWaveUnitIds.Clear();
            int availableCount = CollectMilitary();
            int committedCount =
                EnemyAIWaveSizing.CalculateCommittedUnitCount(
                    waveIndex,
                    availableCount);
            Vector3 target = FindPlayerBaseTarget();
            if (committedCount > 0)
            {
                IssueAttackMoveToMilitary(
                    target,
                    committedCount,
                    true);
            }

            waveIndex++;
        }

        private int CollectMilitary()
        {
            availableMilitary.Clear();
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit != null &&
                    unit.IsAlive &&
                    unit.OwnerId == ownerId &&
                    unit.Type != UnitType.Worker &&
                    unit.Combatant != null)
                {
                    availableMilitary.Add(unit);
                }
            }

            return availableMilitary.Count;
        }

        private void IssueAttackMoveToMilitary(
            Vector3 destination,
            int maximumCount,
            bool recordAsWave)
        {
            int count = CollectMilitary();
            if (count == 0)
            {
                return;
            }

            int commandCount = Math.Min(count, maximumCount);
            int startIndex = random.Next(count);
            for (int i = 0; i < commandCount; i++)
            {
                Unit unit =
                    availableMilitary[(startIndex + i) % count];
                if (unit.Combatant.IssueAttackMove(destination) &&
                    recordAsWave)
                {
                    activeWaveUnitIds.Add(unit.GetInstanceID());
                }
            }
        }

        private Vector3 FindPlayerBaseTarget()
        {
            Building fallback = null;
            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Building building = buildings[i];
                if (building == null ||
                    !building.IsAlive ||
                    building.OwnerId == ownerId)
                {
                    continue;
                }

                if (building.Type == BuildingType.Headquarters)
                {
                    return building.transform.position;
                }

                fallback = building;
            }

            return fallback != null
                ? fallback.transform.position
                : -homePosition;
        }

        private Vector3 FindDefenderRallyPoint()
        {
            Vector3 towardCenter = -homePosition;
            towardCenter.y = 0f;
            if (towardCenter.sqrMagnitude > 0.001f)
            {
                towardCenter.Normalize();
            }

            Vector3 desired =
                homePosition +
                towardCenter *
                BalanceConfig.EnemyAIDefenderRallyDistance;
            Vector2Int desiredCell = grid.WorldToCell(desired);
            return grid.TryFindNearestWalkable(
                    desiredCell,
                    BalanceConfig.EnemyAIBuildSearchMaximumRadius,
                    out Vector2Int rallyCell)
                ? grid.CellToWorld(rallyCell)
                : homePosition;
        }

        private void UpdateProductionRallyPoints()
        {
            IReadOnlyList<Building> buildings = Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Barracks barracks = buildings[i] as Barracks;
                if (barracks != null &&
                    barracks.IsAlive &&
                    barracks.IsConstructed &&
                    barracks.OwnerId == ownerId)
                {
                    barracks.ProductionQueue.SetRallyPoint(
                        defenderRallyPoint);
                }
            }
        }

        private void RallyUncommittedDefenders()
        {
            float leashSquared =
                BalanceConfig.EnemyAIDefenderLeashRadius *
                BalanceConfig.EnemyAIDefenderLeashRadius;
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null ||
                    !unit.IsAlive ||
                    unit.OwnerId != ownerId ||
                    unit.Type == UnitType.Worker ||
                    unit.Combatant == null ||
                    activeWaveUnitIds.Contains(unit.GetInstanceID()) ||
                    unit.Combatant.CurrentOrder != CombatOrderType.Idle)
                {
                    continue;
                }

                Vector3 difference =
                    unit.transform.position - defenderRallyPoint;
                difference.y = 0f;
                if (difference.sqrMagnitude > leashSquared)
                {
                    unit.Combatant.IssueAttackMove(defenderRallyPoint);
                }
            }
        }
    }
}
