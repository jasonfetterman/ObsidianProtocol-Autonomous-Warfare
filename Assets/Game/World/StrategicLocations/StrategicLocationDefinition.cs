using UnityEngine;

namespace ObsidianProtocol.Game.World.StrategicLocations
{
    [CreateAssetMenu(
        fileName = "StrategicLocationDefinition",
        menuName = "Obsidian Protocol/World/Strategic Location Definition")]
    public sealed class StrategicLocationDefinition : ScriptableObject
    {
        [SerializeField] private string locationId;
        [SerializeField] private string displayName;
        [SerializeField] private int strategicValue = 1;

        public string LocationId => locationId;
        public string DisplayName => displayName;
        public int StrategicValue => Mathf.Max(0, strategicValue);
    }
}
