using UnityEngine;

namespace ObsidianProtocol.Game.World.Regions
{
    [CreateAssetMenu(
        fileName = "WorldRegionDefinition",
        menuName = "Obsidian Protocol/World/World Region Definition")]
    public sealed class WorldRegionDefinition : ScriptableObject
    {
        [SerializeField] private string regionId;
        [SerializeField] private string displayName;

        public string RegionId => regionId;
        public string DisplayName => displayName;
    }
}
