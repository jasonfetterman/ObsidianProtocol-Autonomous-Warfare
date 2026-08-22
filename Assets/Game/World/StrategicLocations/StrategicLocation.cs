using UnityEngine;

namespace ObsidianProtocol.Game.World.StrategicLocations
{
    public sealed class StrategicLocation : MonoBehaviour
    {
        [SerializeField] private StrategicLocationDefinition definition;

        public StrategicLocationDefinition Definition => definition;

        public string LocationId =>
            definition != null ? definition.LocationId : string.Empty;

        public int StrategicValue =>
            definition != null ? definition.StrategicValue : 0;
    }
}
