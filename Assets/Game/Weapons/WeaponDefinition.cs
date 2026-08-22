using UnityEngine;

namespace ObsidianProtocol.Game.Weapons
{
    [CreateAssetMenu(
        fileName = "WeaponDefinition",
        menuName = "Obsidian Protocol/Weapons/Weapon Definition")]
    public sealed class WeaponDefinition : ScriptableObject
    {
        [SerializeField] private string weaponId;
        [SerializeField] private string displayName;
        [SerializeField] private string description;

        public string WeaponId => weaponId;
        public string DisplayName => displayName;
        public string Description => description;
    }
}
