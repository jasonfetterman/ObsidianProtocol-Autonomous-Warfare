using UnityEngine;

namespace ObsidianProtocol.Game.World.Crossings
{
    [CreateAssetMenu(
        fileName = "CrossingDefinition",
        menuName = "Obsidian Protocol/World/Crossing Definition")]
    public sealed class CrossingDefinition : ScriptableObject
    {
        [SerializeField] private string crossingId;
        [SerializeField] private string displayName;
        [SerializeField] private bool supportsGroundUnits = true;
        [SerializeField] private bool supportsHeavyUnits = true;

        public string CrossingId => crossingId;
        public string DisplayName => displayName;
        public bool SupportsGroundUnits => supportsGroundUnits;
        public bool SupportsHeavyUnits => supportsHeavyUnits;
    }
}
