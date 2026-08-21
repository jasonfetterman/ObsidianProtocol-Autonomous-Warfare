using System.Collections.Generic;
using UnityEngine;

namespace Obsidian.Vehicles
{
    public class VehicleTerrainPathfinder : MonoBehaviour
    {
        [Header("Terrain Limits")]
        [SerializeField] private float fullSpeedAngle = 20f;
        [SerializeField] private float maximumTerrainAngle = 45f;

        [Header("Path")]
        [SerializeField] private float waypointSpacing = 8f;

        [Header("Search")]
        [SerializeField] private int searchDirections = 16;
        [SerializeField] private float searchDistance = 10f;
        [SerializeField] private int maxSteps = 150;

        [Header("Ground Check")]
        [SerializeField] private float rayHeight = 50f;
        [SerializeField] private float rayDistance = 100f;

        private readonly List<Vector3> path =
            new List<Vector3>();

        private Collider[] ownColliders;

        private void Awake()
        {
            ownColliders =
                GetComponentsInChildren<Collider>();
        }

        public bool BuildPath(
            Vector3 start,
            Vector3 destination)
        {
            path.Clear();

            Vector3 current = start;

            for (int step = 0; step < maxSteps; step++)
            {
                float remaining =
                    FlatDistance(current, destination);

                if (remaining <= waypointSpacing)
                {
                    if (!GetTraversableGroundPoint(
                            destination,
                            out Vector3 finalPoint))
                    {
                        return false;
                    }

                    path.Add(finalPoint);
                    return true;
                }

                if (!FindBestStep(
                        current,
                        destination,
                        out Vector3 nextPoint))
                {
                    return false;
                }

                // Prevent the pathfinder from getting stuck
                // repeatedly selecting almost the same point.
                if (FlatDistance(current, nextPoint) < 1f)
                {
                    return false;
                }

                path.Add(nextPoint);
                current = nextPoint;
            }

            return false;
        }

        public IReadOnlyList<Vector3> GetPath()
        {
            return path;
        }

        private bool FindBestStep(
            Vector3 current,
            Vector3 destination,
            out Vector3 bestPoint)
        {
            bestPoint = Vector3.zero;

            Vector3 toDestination =
                destination - current;

            toDestination.y = 0f;

            if (toDestination.sqrMagnitude < 0.001f)
                return false;

            toDestination.Normalize();

            float baseAngle =
                Mathf.Atan2(
                    toDestination.z,
                    toDestination.x
                ) * Mathf.Rad2Deg;

            float bestScore =
                float.MaxValue;

            bool found = false;

            for (int i = 0;
                 i < searchDirections;
                 i++)
            {
                float angle =
                    baseAngle +
                    (360f / searchDirections) * i;

                Vector3 direction =
                    new Vector3(
                        Mathf.Cos(angle * Mathf.Deg2Rad),
                        0f,
                        Mathf.Sin(angle * Mathf.Deg2Rad)
                    );

                Vector3 candidate =
                    current +
                    direction * searchDistance;

                if (!GetTraversableGroundPoint(
                        candidate,
                        out Vector3 groundPoint))
                {
                    continue;
                }

                float slope =
                    GetSlope(groundPoint);

                float remainingDistance =
                    FlatDistance(
                        groundPoint,
                        destination);

                float directionAngle =
                    Vector3.Angle(
                        toDestination,
                        direction);

                float slopeCost =
                    GetSlopeCost(slope);

                float distanceCost =
                    remainingDistance;

                float directionCost =
                    directionAngle * 0.5f;

                float score =
                    distanceCost +
                    slopeCost +
                    directionCost;

                if (score < bestScore)
                {
                    bestScore = score;
                    bestPoint = groundPoint;
                    found = true;
                }
            }

            return found;
        }

        private float GetSlopeCost(float slope)
        {
            if (slope <= fullSpeedAngle)
            {
                return slope * 0.25f;
            }

            float slopeRange =
                maximumTerrainAngle -
                fullSpeedAngle;

            float slopeAmount =
                Mathf.Clamp01(
                    (slope - fullSpeedAngle) /
                    slopeRange
                );

            // Make 20-45 degree terrain increasingly expensive.
            return
                25f +
                slopeAmount *
                slopeAmount *
                300f;
        }

        private bool GetTraversableGroundPoint(
            Vector3 position,
            out Vector3 groundPoint)
        {
            groundPoint = Vector3.zero;

            Vector3 rayStart =
                new Vector3(
                    position.x,
                    position.y + rayHeight,
                    position.z
                );

            RaycastHit[] hits =
                Physics.RaycastAll(
                    rayStart,
                    Vector3.down,
                    rayDistance,
                    ~0,
                    QueryTriggerInteraction.Ignore
                );

            float bestHeight =
                float.NegativeInfinity;

            bool found = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.collider.isTrigger)
                    continue;

                if (IsOwnCollider(hit.collider))
                    continue;

                float slope =
                    Vector3.Angle(
                        hit.normal,
                        Vector3.up
                    );

                if (slope > maximumTerrainAngle)
                    continue;

                if (hit.point.y > bestHeight)
                {
                    bestHeight = hit.point.y;
                    groundPoint = hit.point;
                    found = true;
                }
            }

            return found;
        }

        private float GetSlope(
            Vector3 groundPoint)
        {
            Vector3 rayStart =
                groundPoint +
                Vector3.up * 2f;

            RaycastHit[] hits =
                Physics.RaycastAll(
                    rayStart,
                    Vector3.down,
                    5f,
                    ~0,
                    QueryTriggerInteraction.Ignore
                );

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.collider.isTrigger)
                    continue;

                if (IsOwnCollider(hit.collider))
                    continue;

                return Vector3.Angle(
                    hit.normal,
                    Vector3.up
                );
            }

            return 90f;
        }

        private bool IsOwnCollider(
            Collider collider)
        {
            if (ownColliders == null)
                return false;

            foreach (Collider own in ownColliders)
            {
                if (collider == own)
                    return true;
            }

            return false;
        }

        private float FlatDistance(
            Vector3 a,
            Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;

            return Vector3.Distance(a, b);
        }
    }
}