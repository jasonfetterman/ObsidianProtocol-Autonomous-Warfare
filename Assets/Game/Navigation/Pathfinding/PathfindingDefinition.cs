using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Pathfinding
{
    [CreateAssetMenu(
        fileName = "PathfindingDefinition",
        menuName = "Obsidian Protocol/Navigation/Pathfinding Definition")]
    public sealed class PathfindingDefinition : ScriptableObject
    {
        [Header("Grid")]
        [SerializeField] private float nodeSize = 1f;
        [SerializeField] private int maximumIterations = 10000;

        [Header("World")]
        [SerializeField] private int gridWidth = 256;
        [SerializeField] private int gridHeight = 256;

        public float NodeSize => Mathf.Max(0.1f, nodeSize);
        public int MaximumIterations => Mathf.Max(1, maximumIterations);

        public int GridWidth => Mathf.Max(1, gridWidth);
        public int GridHeight => Mathf.Max(1, gridHeight);
    }
}
