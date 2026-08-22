using UnityEngine;

namespace ObsidianProtocol.Game.World.Routes
{
    [CreateAssetMenu(
        fileName = "TraversalRouteDefinition",
        menuName = "Obsidian Protocol/World/Traversal Route Definition")]
    public sealed class TraversalRouteDefinition : ScriptableObject
    {
        [SerializeField] private string routeId;
        [SerializeField] private string displayName;
        [SerializeField] private float movementCostMultiplier = 1f;

        public string RouteId => routeId;
        public string DisplayName => displayName;
        public float MovementCostMultiplier =>
            Mathf.Max(0.01f, movementCostMultiplier);
    }
}
