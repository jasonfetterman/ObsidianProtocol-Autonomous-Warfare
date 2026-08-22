using UnityEngine;

namespace ObsidianProtocol.Game.World.TerrainMaterials
{
    public sealed class TerrainMaterial : MonoBehaviour
    {
        [SerializeField] private TerrainMaterialDefinition definition;

        public TerrainMaterialDefinition Definition => definition;

        public float MovementCost =>
            definition != null ? definition.MovementCost : 1f;
    }
}
