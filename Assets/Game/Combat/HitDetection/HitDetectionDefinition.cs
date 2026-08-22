using UnityEngine;

namespace ObsidianProtocol.Game.Combat.HitDetection
{
    [CreateAssetMenu(
        fileName = "HitDetectionDefinition",
        menuName = "Obsidian Protocol/Combat/Hit Detection Definition")]
    public sealed class HitDetectionDefinition : ScriptableObject
    {
        [SerializeField] private float maximumHitDistance = 5000f;
        [SerializeField] private LayerMask hitMask = ~0;

        public float MaximumHitDistance =>
            Mathf.Max(0.1f, maximumHitDistance);

        public LayerMask HitMask => hitMask;
    }
}
