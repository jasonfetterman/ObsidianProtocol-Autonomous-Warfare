using UnityEngine;

namespace ObsidianProtocol.Game.World.Structures
{
    public sealed class WorldStructure : MonoBehaviour
    {
        [SerializeField] private WorldStructureDefinition definition;

        public WorldStructureDefinition Definition => definition;

        public bool BlocksMovement =>
            definition != null && definition.BlocksMovement;
    }
}
