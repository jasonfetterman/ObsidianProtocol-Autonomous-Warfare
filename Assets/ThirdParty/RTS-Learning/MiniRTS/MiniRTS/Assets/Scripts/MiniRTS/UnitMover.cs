using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Follows A* waypoints and adds lightweight separation steering around nearby units.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Unit))]
    public sealed class UnitMover : MonoBehaviour
    {
        private readonly List<Vector3> waypoints = new List<Vector3>();

        private WalkGrid grid;
        private Unit unit;
        private int waypointIndex;

        public bool IsMoving => waypointIndex < waypoints.Count;
        public Vector3 Destination { get; private set; }

        private void Awake()
        {
            unit = GetComponent<Unit>();
        }

        public void Initialize(WalkGrid walkGrid)
        {
            grid = walkGrid ?? throw new System.ArgumentNullException(nameof(walkGrid));
            unit = GetComponent<Unit>();
        }

        public bool MoveTo(Vector3 worldDestination)
        {
            if (grid == null)
            {
                Debug.LogWarning($"{name} cannot move before UnitMover.Initialize is called.", this);
                return false;
            }

            Vector2Int start = grid.WorldToCell(transform.position);
            Vector2Int requestedGoal = grid.WorldToCell(grid.ClampWorldPosition(worldDestination));

            if (!grid.TryFindNearestWalkable(
                    requestedGoal,
                    Mathf.Max(grid.Width, grid.Height),
                    out Vector2Int goal))
            {
                Stop();
                return false;
            }

            if (!grid.IsWalkable(start) &&
                !grid.TryFindNearestWalkable(start, 4, out start))
            {
                Stop();
                return false;
            }

            List<Vector2Int> path = AStar.FindPath(grid, start, goal);
            if (path == null)
            {
                Stop();
                return false;
            }

            waypoints.Clear();
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 waypoint = grid.CellToWorld(path[i]);
                waypoint.y = unit != null
                    ? unit.GroundHeight
                    : BalanceConfig.WorkerGroundHeight;
                waypoints.Add(waypoint);
            }

            Destination = grid.CellToWorld(goal);
            Destination = new Vector3(
                Destination.x,
                unit != null
                    ? unit.GroundHeight
                    : BalanceConfig.WorkerGroundHeight,
                Destination.z);
            waypointIndex = waypoints.Count > 1 ? 1 : waypoints.Count;
            return true;
        }

        public void Stop()
        {
            waypoints.Clear();
            waypointIndex = 0;
            Destination = transform.position;
        }

        private void Update()
        {
            if (grid == null || !IsMoving)
            {
                return;
            }

            Vector3 position = transform.position;
            Vector3 waypoint = waypoints[waypointIndex];
            Vector3 toWaypoint = waypoint - position;
            toWaypoint.y = 0f;

            if (toWaypoint.sqrMagnitude <=
                BalanceConfig.WaypointTolerance * BalanceConfig.WaypointTolerance)
            {
                waypointIndex++;
                if (!IsMoving)
                {
                    transform.position = Destination;
                    return;
                }

                waypoint = waypoints[waypointIndex];
                toWaypoint = waypoint - position;
                toWaypoint.y = 0f;
            }

            Vector3 pathDirection = toWaypoint.normalized;
            Vector3 steering = pathDirection + CalculateSeparation() *
                BalanceConfig.LocalAvoidanceStrength;
            steering.y = 0f;
            if (steering.sqrMagnitude < 0.001f)
            {
                steering = pathDirection;
            }

            steering.Normalize();
            float deltaTime = Mathf.Min(Time.deltaTime, 0.1f);
            float step = Mathf.Min(
                (unit != null
                    ? unit.MoveSpeed
                    : BalanceConfig.WorkerMoveSpeed) * deltaTime,
                grid.CellSize * 0.45f);

            bool moved = TryMove(position + steering * step);
            if (!moved && steering != pathDirection)
            {
                moved = TryMove(position + pathDirection * step);
            }

            if (moved && pathDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(pathDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    (unit != null
                        ? UnitDefinition.Get(unit.Type).TurnSpeed
                        : BalanceConfig.WorkerTurnSpeed) * deltaTime);
            }
        }

        private Vector3 CalculateSeparation()
        {
            Vector3 separation = Vector3.zero;
            IReadOnlyList<Unit> units = Unit.ActiveUnits;
            float rangeSquared =
                BalanceConfig.LocalAvoidanceRadius * BalanceConfig.LocalAvoidanceRadius;

            for (int i = 0; i < units.Count; i++)
            {
                Unit other = units[i];
                if (other == null || other == unit)
                {
                    continue;
                }

                Vector3 away = transform.position - other.transform.position;
                away.y = 0f;
                float distanceSquared = away.sqrMagnitude;
                if (distanceSquared >= rangeSquared)
                {
                    continue;
                }

                if (distanceSquared < 0.0001f)
                {
                    float angle = Mathf.Abs(GetInstanceID() - other.GetInstanceID()) % 360;
                    away = new Vector3(
                        Mathf.Cos(angle * Mathf.Deg2Rad),
                        0f,
                        Mathf.Sin(angle * Mathf.Deg2Rad));
                    distanceSquared = 0.01f;
                }

                float distance = Mathf.Sqrt(distanceSquared);
                float weight = 1f - distance / BalanceConfig.LocalAvoidanceRadius;
                separation += away / distance * weight;
            }

            return Vector3.ClampMagnitude(separation, 1f);
        }

        private bool TryMove(Vector3 candidate)
        {
            candidate = grid.ClampWorldPosition(candidate);
            candidate.y = unit != null
                ? unit.GroundHeight
                : BalanceConfig.WorkerGroundHeight;

            Vector2Int currentCell = grid.WorldToCell(transform.position);
            Vector2Int candidateCell = grid.WorldToCell(candidate);
            if (!grid.IsWalkable(candidateCell))
            {
                return false;
            }

            int deltaX = candidateCell.x - currentCell.x;
            int deltaY = candidateCell.y - currentCell.y;
            if (deltaX != 0 && deltaY != 0 &&
                (!grid.IsWalkable(currentCell.x + deltaX, currentCell.y) ||
                 !grid.IsWalkable(currentCell.x, currentCell.y + deltaY)))
            {
                return false;
            }

            transform.position = candidate;
            return true;
        }
    }
}
