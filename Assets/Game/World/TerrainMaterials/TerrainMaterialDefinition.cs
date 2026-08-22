using UnityEngine;

namespace ObsidianProtocol.Game.World.TerrainMaterials
{
    [CreateAssetMenu(
        fileName = "TerrainMaterialDefinition",
        menuName = "Obsidian Protocol/World/Terrain Material Definition")]
    public sealed class TerrainMaterialDefinition : ScriptableObject
    {
        [SerializeField] private string materialId;
        [SerializeField] private string displayName;
        [SerializeField] private float movementCost = 1f;

        public string MaterialId => materialId;
        public string DisplayName => displayName;
        public float MovementCost => Mathf.Max(0.01f, movementCost);
    }
}
