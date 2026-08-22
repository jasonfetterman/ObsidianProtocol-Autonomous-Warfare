using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Layers
{
    public sealed class NavigationLayer : MonoBehaviour
    {
        [SerializeField] private NavigationLayerDefinition definition;

        public NavigationLayerDefinition Definition => definition;

        public int Priority =>
            definition != null ? definition.Priority : 0;
    }
}
