using System;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Shared HQ/Barracks/Factory production component with a five-slot queue and rally.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Building))]
    public sealed class ProductionQueue : MonoBehaviour
    {
        private Building host;
        private PlayerEconomy economy;
        private UnitType[] allowedTypes;
        private Func<UnitType, Vector3, Unit> unitFactory;
        private ProductionQueueState state;
        private Vector3 rallyPoint;
        private GameObject rallyMarker;
        private bool initialized;

        public int Count => state != null ? state.Count : 0;
        public int Capacity => state != null
            ? state.Capacity
            : BalanceConfig.ProductionQueueCapacity;
        public float Progress => state != null ? state.CurrentProgress : 0f;
        public Vector3 RallyPoint => rallyPoint;

        public void Initialize(
            Building hostBuilding,
            PlayerEconomy factionEconomy,
            UnitType[] trainableTypes,
            Func<UnitType, Vector3, Unit> spawnUnit)
        {
            host = hostBuilding ??
                throw new ArgumentNullException(nameof(hostBuilding));
            economy = factionEconomy ??
                throw new ArgumentNullException(nameof(factionEconomy));
            allowedTypes = trainableTypes ??
                throw new ArgumentNullException(nameof(trainableTypes));
            unitFactory = spawnUnit ??
                throw new ArgumentNullException(nameof(spawnUnit));
            if (allowedTypes.Length == 0)
            {
                throw new ArgumentException(
                    "A production building must train at least one unit.",
                    nameof(trainableTypes));
            }

            state = new ProductionQueueState(economy);
            rallyPoint = FindSpawnPosition();
            initialized = true;
        }

        public UnitType GetQueuedType(int index)
        {
            if (state == null)
            {
                throw new InvalidOperationException(
                    "ProductionQueue.Initialize must be called first.");
            }

            return state.GetEntry(index);
        }

        public bool CanTrain(UnitType type)
        {
            if (!initialized || host == null || !host.IsConstructed ||
                state.Count >= state.Capacity || !IsAllowed(type))
            {
                return false;
            }

            UnitDefinition definition = UnitDefinition.Get(type);
            return economy.CanAfford(
                definition.MineralCost,
                definition.GasCost,
                definition.SupplyCost);
        }

        public bool TryQueue(UnitType type)
        {
            return initialized &&
                   host != null &&
                   host.IsConstructed &&
                   IsAllowed(type) &&
                   state.TryEnqueue(type);
        }

        public bool SetRallyPoint(Vector3 worldPoint)
        {
            if (!initialized || host == null || host.Grid == null)
            {
                return false;
            }

            WalkGrid grid = host.Grid;
            Vector2Int desired = grid.WorldToCell(grid.ClampWorldPosition(worldPoint));
            if (!grid.TryFindNearestWalkable(
                    desired,
                    Mathf.Max(grid.Width, grid.Height),
                    out Vector2Int rallyCell))
            {
                return false;
            }

            rallyPoint = grid.CellToWorld(rallyCell);
            ShowRallyMarker();
            return true;
        }

        private void Update()
        {
            if (initialized && host != null && host.IsConstructed)
            {
                state.Advance(Time.deltaTime, SpawnCompletedUnit);
            }
        }

        private void OnDestroy()
        {
            if (state != null)
            {
                state.RefundAll();
            }

            if (rallyMarker != null)
            {
                Destroy(rallyMarker);
            }
        }

        private bool IsAllowed(UnitType type)
        {
            for (int i = 0; i < allowedTypes.Length; i++)
            {
                if (allowedTypes[i] == type)
                {
                    return true;
                }
            }

            return false;
        }

        private Vector3 FindSpawnPosition()
        {
            WalkGrid grid = host.Grid;
            Vector3 desiredSpawn = host.transform.position + new Vector3(
                host.Footprint.x * grid.CellSize * 0.5f + 1.25f,
                0f,
                0f);
            Vector2Int desiredCell = grid.WorldToCell(desiredSpawn);
            if (grid.TryFindNearestWalkable(
                    desiredCell,
                    Mathf.Max(grid.Width, grid.Height),
                    out Vector2Int spawnCell))
            {
                return grid.CellToWorld(spawnCell);
            }

            return host.transform.position;
        }

        private void SpawnCompletedUnit(UnitType type)
        {
            Vector3 spawnPosition = FindSpawnPosition();
            UnitDefinition definition = UnitDefinition.Get(type);
            spawnPosition.y = definition.GroundHeight;
            Unit unit = unitFactory(type, spawnPosition);
            if (unit == null)
            {
                economy.RefundPurchase(
                    definition.MineralCost,
                    definition.GasCost,
                    definition.SupplyCost);
                return;
            }

            Vector3 difference = rallyPoint - spawnPosition;
            difference.y = 0f;
            if (difference.sqrMagnitude > 0.25f && unit.Mover != null)
            {
                unit.Mover.MoveTo(rallyPoint);
            }
        }

        private void ShowRallyMarker()
        {
            if (rallyMarker == null)
            {
                rallyMarker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                rallyMarker.name = $"{name}_RallyPoint";
                rallyMarker.transform.localScale = new Vector3(0.55f, 0.025f, 0.55f);
                Collider markerCollider = rallyMarker.GetComponent<Collider>();
                if (markerCollider != null)
                {
                    markerCollider.enabled = false;
                    Destroy(markerCollider);
                }

                rallyMarker.GetComponent<Renderer>().material.color =
                    new Color(1f, 0.75f, 0.08f);
            }

            rallyMarker.transform.position = new Vector3(
                rallyPoint.x,
                0.03f,
                rallyPoint.z);
        }
    }
}
