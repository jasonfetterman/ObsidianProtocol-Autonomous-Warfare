using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Nodes
{
    public sealed class NavigationNode : MonoBehaviour
    {
        [SerializeField] private NavigationNodeDefinition definition;

        public NavigationNodeDefinition Definition => definition;

        public float TraversalCost =>
            definition != null ? definition.TraversalCost : 1f;

        public bool IsEnabled { get; private set; }

        private void Awake()
        {
            IsEnabled = definition == null || definition.EnabledByDefault;
        }

        public void SetEnabled(bool enabled)
        {
            IsEnabled = enabled;
        }
    }
}
