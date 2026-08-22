using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Layers
{
    [CreateAssetMenu(
        fileName = "NavigationLayerDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Layer Definition")]
    public sealed class NavigationLayerDefinition : ScriptableObject
    {
        [SerializeField] private string layerId;
        [SerializeField] private string displayName;
        [SerializeField] private int priority = 0;

        public string LayerId => layerId;
        public string DisplayName => displayName;
        public int Priority => priority;
    }
}
