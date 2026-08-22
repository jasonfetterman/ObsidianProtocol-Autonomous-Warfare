using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Obstacles
{
    public sealed class NavigationObstacle : MonoBehaviour
    {
        [SerializeField] private NavigationObstacleDefinition definition;

        public NavigationObstacleDefinition Definition => definition;

        public float AvoidanceRadius =>
            definition != null ? definition.AvoidanceRadius : 1f;

        public bool BlocksNavigation =>
            definition == null || definition.BlocksNavigation;
    }
}
