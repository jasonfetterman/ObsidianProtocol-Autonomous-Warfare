using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Obstacles
{
    [CreateAssetMenu(
        fileName = "NavigationObstacleDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Obstacle Definition")]
    public sealed class NavigationObstacleDefinition : ScriptableObject
    {
        [SerializeField] private float avoidanceRadius = 1f;
        [SerializeField] private bool blocksNavigation = true;

        public float AvoidanceRadius =>
            Mathf.Max(0.01f, avoidanceRadius);

        public bool BlocksNavigation => blocksNavigation;
    }
}
