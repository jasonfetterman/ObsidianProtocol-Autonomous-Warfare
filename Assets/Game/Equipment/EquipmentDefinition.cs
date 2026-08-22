using UnityEngine;

namespace ObsidianProtocol.Game.Equipment
{
    [CreateAssetMenu(
        fileName = "EquipmentDefinition",
        menuName = "Obsidian Protocol/Equipment/Equipment Definition")]
    public sealed class EquipmentDefinition : ScriptableObject
    {
        [SerializeField] private string equipmentId;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string EquipmentId => equipmentId;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
