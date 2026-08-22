using UnityEngine;

namespace ObsidianProtocol.Game.World.Terrain
{
    [CreateAssetMenu(
        fileName = "TerrainDefinition",
        menuName = "Obsidian Protocol/World/Terrain Definition")]
    public sealed class TerrainDefinition : ScriptableObject
    {
        [SerializeField] private string terrainId;
        [SerializeField] private string displayName;

        public string TerrainId => terrainId;
        public string DisplayName => displayName;
    }
}
