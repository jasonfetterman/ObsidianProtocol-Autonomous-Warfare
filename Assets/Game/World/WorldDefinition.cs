using UnityEngine;

namespace ObsidianProtocol.Game.World
{
    [CreateAssetMenu(
        fileName = "WorldDefinition",
        menuName = "Obsidian Protocol/World/World Definition")]
    public sealed class WorldDefinition : ScriptableObject
    {
        [SerializeField] private string worldId;
        [SerializeField] private string displayName;

        public string WorldId => worldId;
        public string DisplayName => displayName;
    }
}
