using UnityEngine;

namespace ObsidianProtocol.Game.World.Structures
{
    [CreateAssetMenu(
        fileName = "WorldStructureDefinition",
        menuName = "Obsidian Protocol/World/World Structure Definition")]
    public sealed class WorldStructureDefinition : ScriptableObject
    {
        [SerializeField] private string structureId;
        [SerializeField] private string displayName;
        [SerializeField] private bool blocksMovement = true;

        public string StructureId => structureId;
        public string DisplayName => displayName;
        public bool BlocksMovement => blocksMovement;
    }
}
