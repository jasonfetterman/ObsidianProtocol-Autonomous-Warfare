using UnityEngine;

namespace ObsidianProtocol.Game.World.Crossings
{
    public sealed class WorldCrossing : MonoBehaviour
    {
        [SerializeField] private CrossingDefinition definition;

        public CrossingDefinition Definition => definition;

        public bool SupportsGroundUnits =>
            definition != null && definition.SupportsGroundUnits;

        public bool SupportsHeavyUnits =>
            definition != null && definition.SupportsHeavyUnits;
    }
}
