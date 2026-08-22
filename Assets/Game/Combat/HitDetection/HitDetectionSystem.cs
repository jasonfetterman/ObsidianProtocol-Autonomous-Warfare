using UnityEngine;

namespace ObsidianProtocol.Game.Combat.HitDetection
{
    public sealed class HitDetectionSystem : MonoBehaviour
    {
        [SerializeField] private HitDetectionDefinition definition;

        public HitDetectionDefinition Definition => definition;

        public bool TryDetectHit(
            Ray ray,
            out RaycastHit hit)
        {
            hit = default;

            float maxDistance =
                definition != null
                    ? definition.MaximumHitDistance
                    : 5000f;

            LayerMask hitMask =
                definition != null
                    ? definition.HitMask
                    : ~0;

            return Physics.Raycast(
                ray,
                out hit,
                maxDistance,
                hitMask,
                QueryTriggerInteraction.Ignore);
        }
    }
}
