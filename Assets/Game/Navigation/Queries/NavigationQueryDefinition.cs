using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Queries
{
    [CreateAssetMenu(
        fileName = "NavigationQueryDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Query Definition")]
    public sealed class NavigationQueryDefinition : ScriptableObject
    {
        [SerializeField] private float maximumSearchDistance = 1000f;
        [SerializeField] private int maximumResults = 32;

        public float MaximumSearchDistance =>
            Mathf.Max(0.1f, maximumSearchDistance);

        public int MaximumResults =>
            Mathf.Max(1, maximumResults);
    }
}
