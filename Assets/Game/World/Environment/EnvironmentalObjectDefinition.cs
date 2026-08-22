using UnityEngine;

namespace ObsidianProtocol.Game.World.Environment
{
    [CreateAssetMenu(
        fileName = "EnvironmentalObjectDefinition",
        menuName = "Obsidian Protocol/World/Environmental Object Definition")]
    public sealed class EnvironmentalObjectDefinition : ScriptableObject
    {
        [SerializeField] private string objectId;
        [SerializeField] private string displayName;
        [SerializeField] private bool blocksMovement;

        public string ObjectId => objectId;
        public string DisplayName => displayName;
        public bool BlocksMovement => blocksMovement;
    }
}
