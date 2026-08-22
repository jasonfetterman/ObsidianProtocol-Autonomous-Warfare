using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Terrain
{
    public sealed class TerrainMovementService : MonoBehaviour
    {
        [SerializeField] private TerrainMovementDefinition definition;

        public TerrainMovementDefinition Definition => definition;

        public bool IsSlopeTraversable(float slope)
        {
            if (definition == null)
            {
                return true;
            }

            return slope <= definition.MaximumSlope;
        }

        public float GetMovementCost(float terrainCost)
        {
            float baseCost = definition != null
                ? definition.DefaultMovementCost
                : 1f;

            return Mathf.Max(0.01f, baseCost * terrainCost);
        }
    }
}
