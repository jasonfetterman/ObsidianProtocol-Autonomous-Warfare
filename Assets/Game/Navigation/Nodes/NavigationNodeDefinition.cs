using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Nodes
{
    [CreateAssetMenu(
        fileName = "NavigationNodeDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Node Definition")]
    public sealed class NavigationNodeDefinition : ScriptableObject
    {
        [SerializeField] private float traversalCost = 1f;
        [SerializeField] private bool enabledByDefault = true;

        public float TraversalCost => Mathf.Max(0.01f, traversalCost);
        public bool EnabledByDefault => enabledByDefault;
    }
}
