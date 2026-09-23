using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Technology
{
    public enum TechnologyCategory
    {
        Command,
        Autonomy,
        Engineering,
        Weapons,
        Defense,
        Mobility,
        Logistics,
        Experimental
    }

    [CreateAssetMenu(
        fileName = "TechnologyDefinition",
        menuName = "Obsidian Protocol/Technology/Technology Definition")]
    public sealed class TechnologyDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string technologyId;
        [SerializeField] private string displayName;
        [TextArea(3, 8)]
        [SerializeField] private string description;
        [SerializeField] private TechnologyCategory category;

        [Header("Research")]
        [Min(1)]
        [SerializeField] private int researchCost = 100;

        [Min(1f)]
        [SerializeField] private float researchTimeSeconds = 60f;

        [Header("Prerequisites")]
        [SerializeField] private List<TechnologyDefinition> prerequisites =
            new List<TechnologyDefinition>();

        [Header("Unlocks")]
        [SerializeField] private List<string> unlockIds =
            new List<string>();

        public string TechnologyId => technologyId;
        public string DisplayName => displayName;
        public string Description => description;
        public TechnologyCategory Category => category;
        public int ResearchCost => researchCost;
        public float ResearchTimeSeconds => researchTimeSeconds;
        public IReadOnlyList<TechnologyDefinition> Prerequisites => prerequisites;
        public IReadOnlyList<string> UnlockIds => unlockIds;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(technologyId) &&
                   !string.IsNullOrWhiteSpace(displayName) &&
                   researchCost > 0 &&
                   researchTimeSeconds > 0f;
        }
    }
}
