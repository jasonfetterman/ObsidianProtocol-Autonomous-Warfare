using UnityEngine;

namespace ObsidianProtocol.Game.Abilities
{
    [CreateAssetMenu(
        fileName = "AbilityDefinition",
        menuName = "Obsidian Protocol/Abilities/Ability Definition")]
    public sealed class AbilityDefinition : ScriptableObject
    {
        [SerializeField] private string abilityId;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string AbilityId => abilityId;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
