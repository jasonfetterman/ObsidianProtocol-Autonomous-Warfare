using UnityEngine;

namespace ObsidianProtocol.Game.World.PointsOfInterest
{
    [CreateAssetMenu(
        fileName = "PointOfInterestDefinition",
        menuName = "Obsidian Protocol/World/Point Of Interest Definition")]
    public sealed class PointOfInterestDefinition : ScriptableObject
    {
        [SerializeField] private string pointId;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string PointId => pointId;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
