using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Graph
{
    [CreateAssetMenu(
        fileName = "NavigationGraphDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Graph Definition")]
    public sealed class NavigationGraphDefinition : ScriptableObject
    {
        [SerializeField] private int maximumNodes = 10000;
        [SerializeField] private float connectionDistance = 10f;

        public int MaximumNodes => Mathf.Max(1, maximumNodes);
        public float ConnectionDistance =>
            Mathf.Max(0.1f, connectionDistance);
    }
}
