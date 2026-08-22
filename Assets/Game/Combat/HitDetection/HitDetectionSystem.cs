using UnityEngine;

namespace ObsidianProtocol.Game.Combat.HitDetection
{
    public sealed class HitDetectionSystem : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayers = ~0;
        [SerializeField] private float maxRayDistance = 10000f;

        public bool TryDetectHit(
            Vector3 origin,
            Vector3 direction,
            out RaycastHit hit)
        {
            hit = default;

            if (direction.sqrMagnitude <= 0.0001f)
            {
                return false;
            }

            return Physics.Raycast(
                origin,
                direction.normalized,
                out hit,
                Mathf.Max(0.01f, maxRayDistance),
                targetLayers,
                QueryTriggerInteraction.Ignore);
        }

        public bool TryDetectHit(
            Ray ray,
            out RaycastHit hit)
        {
            return TryDetectHit(
                ray.origin,
                ray.direction,
                out hit);
        }
    }
}
