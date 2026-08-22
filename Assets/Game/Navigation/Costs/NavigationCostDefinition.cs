using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Costs
{
    [CreateAssetMenu(
        fileName = "NavigationCostDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Cost Definition")]
    public sealed class NavigationCostDefinition : ScriptableObject
    {
        [SerializeField] private float baseCost = 1f;
        [SerializeField] private float maximumCost = 100f;

        public float BaseCost =>
            Mathf.Max(0.01f, baseCost);

        public float MaximumCost =>
            Mathf.Max(BaseCost, maximumCost);
    }
}
