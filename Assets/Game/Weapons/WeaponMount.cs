using UnityEngine;

namespace ObsidianProtocol.Game.Weapons
{
    public sealed class WeaponMount : MonoBehaviour
    {
        [SerializeField] private WeaponDefinition weapon;

        public WeaponDefinition Weapon => weapon;

        public void SetWeapon(WeaponDefinition definition)
        {
            weapon = definition;
        }

        public void ClearWeapon()
        {
            weapon = null;
        }
    }
}
