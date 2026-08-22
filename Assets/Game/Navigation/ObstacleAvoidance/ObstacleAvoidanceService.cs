using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.ObstacleAvoidance
{
    public sealed class ObstacleAvoidanceService : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float detectionDistance = 5f;
        [SerializeField] private float detectionRadius = 0.75f;
        [SerializeField] private LayerMask obstacleLayers = ~0;

        [Header("Steering")]
        [SerializeField] private float avoidanceStrength = 1f;

        public float DetectionDistance => Mathf.Max(0.1f, detectionDistance);
        public float DetectionRadius => Mathf.Max(0.05f, detectionRadius);
        public float AvoidanceStrength => Mathf.Max(0f, avoidanceStrength);

        public Vector3 GetAvoidanceDirection(
            Vector3 position,
            Vector3 desiredDirection)
        {
            desiredDirection.y = 0f;

            if (desiredDirection.sqrMagnitude <= 0.0001f)
            {
                return Vector3.zero;
            }

            Vector3 forward = desiredDirection.normalized;

            if (!Physics.SphereCast(
                    position,
                    DetectionRadius,
                    forward,
                    out RaycastHit hit,
                    DetectionDistance,
                    obstacleLayers,
                    QueryTriggerInteraction.Ignore))
            {
                return forward;
            }

            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0f;

            if (hitNormal.sqrMagnitude <= 0.0001f)
            {
                return forward;
            }

            hitNormal.Normalize();

            Vector3 left = Vector3.Cross(Vector3.up, forward).normalized;
            Vector3 right = -left;

            float leftClearance = GetClearance(
                position,
                left);

            float rightClearance = GetClearance(
                position,
                right);

            Vector3 avoidanceDirection =
                leftClearance >= rightClearance
                    ? left
                    : right;

            Vector3 combined =
                Vector3.Lerp(
                    forward,
                    avoidanceDirection,
                    Mathf.Clamp01(AvoidanceStrength));

            combined.y = 0f;

            return combined.sqrMagnitude > 0.0001f
                ? combined.normalized
                : forward;
        }

        private float GetClearance(
            Vector3 position,
            Vector3 direction)
        {
            if (Physics.SphereCast(
                    position,
                    DetectionRadius,
                    direction,
                    out RaycastHit hit,
                    DetectionDistance,
                    obstacleLayers,
                    QueryTriggerInteraction.Ignore))
            {
                return hit.distance;
            }

            return DetectionDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(
                transform.position,
                DetectionRadius);

            Gizmos.DrawLine(
                transform.position,
                transform.position +
                transform.forward * DetectionDistance);
        }
    }
}
