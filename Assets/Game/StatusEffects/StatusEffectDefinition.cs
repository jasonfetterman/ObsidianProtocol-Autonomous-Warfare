using UnityEngine;

namespace ObsidianProtocol.Game.StatusEffects
{
    [CreateAssetMenu(
        fileName = "StatusEffectDefinition",
        menuName = "Obsidian Protocol/Status Effects/Status Effect Definition")]
    public sealed class StatusEffectDefinition : ScriptableObject
    {
        [SerializeField] private string effectId;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string EffectId => effectId;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
