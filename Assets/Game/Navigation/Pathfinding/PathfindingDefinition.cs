using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Pathfinding
{
    [CreateAssetMenu(
        fileName = "PathfindingDefinition",
        menuName = "Obsidian Protocol/Navigation/Pathfinding Definition")]
    public sealed class PathfindingDefinition : ScriptableObject
    {
        [SerializeField] private float nodeSize = 1f;
        [SerializeField] private int maximumIterations = 10000;

        public float NodeSize => Mathf.Max(0.1f, nodeSize);
        public int MaximumIterations => Mathf.Max(1, maximumIterations);
    }
}
