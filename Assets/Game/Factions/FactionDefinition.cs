using UnityEngine;

namespace ObsidianProtocol.Game.Factions
{
    [CreateAssetMenu(
        fileName = "FactionDefinition",
        menuName = "Obsidian Protocol/Factions/Faction Definition")]
    public sealed class FactionDefinition : ScriptableObject
    {
        [SerializeField] private string factionId;
        [SerializeField] private string displayName;

        public string FactionId => factionId;
        public string DisplayName => displayName;
    }
}
