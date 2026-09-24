using UnityEngine;

namespace ObsidianProtocol.Game.Technology
{
    [CreateAssetMenu(
        fileName = "TechnologyDefinition",
        menuName = "Obsidian Protocol/Technology/Technology Definition")]
    public sealed class TechnologyDefinition : ScriptableObject
    {
        [SerializeField] private string technologyId;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string TechnologyId => technologyId;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
