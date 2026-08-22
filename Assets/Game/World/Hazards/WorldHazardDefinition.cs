using UnityEngine;

namespace ObsidianProtocol.Game.World.Hazards
{
    [CreateAssetMenu(
        fileName = "WorldHazardDefinition",
        menuName = "Obsidian Protocol/World/World Hazard Definition")]
    public sealed class WorldHazardDefinition : ScriptableObject
    {
        [SerializeField] private string hazardId;
        [SerializeField] private string displayName;
        [SerializeField] private float severity = 1f;

        public string HazardId => hazardId;
        public string DisplayName => displayName;
        public float Severity => Mathf.Max(0f, severity);
    }
}
