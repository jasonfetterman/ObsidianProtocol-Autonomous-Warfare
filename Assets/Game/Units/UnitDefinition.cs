using UnityEngine;

namespace ObsidianProtocol.Game.Units
{
    [CreateAssetMenu(
        fileName = "UnitDefinition",
        menuName = "Obsidian Protocol/Units/Unit Definition")]
    public sealed class UnitDefinition : ScriptableObject
    {
        [SerializeField] private string unitId;
        [SerializeField] private string displayName;

        public string UnitId => unitId;
        public string DisplayName => displayName;
    }
}
