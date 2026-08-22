using UnityEngine;

namespace ObsidianProtocol.Game.World.Environment
{
    public sealed class EnvironmentalObject : MonoBehaviour
    {
        [SerializeField] private EnvironmentalObjectDefinition definition;

        public EnvironmentalObjectDefinition Definition => definition;

        public bool BlocksMovement =>
            definition != null && definition.BlocksMovement;
    }
}
