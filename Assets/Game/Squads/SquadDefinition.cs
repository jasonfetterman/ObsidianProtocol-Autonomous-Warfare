using UnityEngine;

namespace ObsidianProtocol.Game.Squads
{
    [CreateAssetMenu(
        fileName = "SquadDefinition",
        menuName = "Obsidian Protocol/Squads/Squad Definition")]
    public sealed class SquadDefinition : ScriptableObject
    {
        [SerializeField] private string squadId;
        [SerializeField] private string displayName;
        [SerializeField] private int maximumUnits = 8;

        public string SquadId => squadId;
        public string DisplayName => displayName;
        public int MaximumUnits => maximumUnits;
    }
}
