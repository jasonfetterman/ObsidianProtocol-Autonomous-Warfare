using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Costs
{
    public sealed class NavigationCostService : MonoBehaviour
    {
        [SerializeField] private NavigationCostDefinition definition;

        public NavigationCostDefinition Definition => definition;

        public float CalculateCost(float terrainCost)
        {
            float baseCost =
                definition != null
                    ? definition.BaseCost
                    : 1f;

            float cost = baseCost * Mathf.Max(0.01f, terrainCost);

            if (definition != null)
            {
                cost = Mathf.Min(
                    cost,
                    definition.MaximumCost);
            }

            return cost;
        }
    }
}
