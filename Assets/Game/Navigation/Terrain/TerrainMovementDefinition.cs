using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Terrain
{
    [CreateAssetMenu(
        fileName = "TerrainMovementDefinition",
        menuName = "Obsidian Protocol/Navigation/Terrain Movement Definition")]
    public sealed class TerrainMovementDefinition : ScriptableObject
    {
        [SerializeField] private float defaultMovementCost = 1f;
        [SerializeField] private float maximumSlope = 45f;

        public float DefaultMovementCost =>
            Mathf.Max(0.01f, defaultMovementCost);

        public float MaximumSlope =>
            Mathf.Clamp(maximumSlope, 0f, 90f);
    }
}
