using UnityEngine;

namespace ObsidianProtocol.Game.Combat.WeaponDamage
{
    public sealed class WeaponDamageSystem
    {
        public float MaxWeaponIntegrity { get; private set; }
        public float CurrentWeaponIntegrity { get; private set; }

        public float IntegrityPercent =>
            MaxWeaponIntegrity <= 0f
                ? 0f
                : CurrentWeaponIntegrity / MaxWeaponIntegrity;

        public bool IsDisabled =>
            CurrentWeaponIntegrity <= 0f;

        public WeaponDamageSystem(float maxIntegrity)
        {
            MaxWeaponIntegrity =
                Mathf.Max(0f, maxIntegrity);

            CurrentWeaponIntegrity =
                MaxWeaponIntegrity;
        }

        public void ApplyDamage(float amount)
        {
            CurrentWeaponIntegrity =
                Mathf.Clamp(
                    CurrentWeaponIntegrity -
                    Mathf.Max(0f, amount),
                    0f,
                    MaxWeaponIntegrity);
        }

        public void Repair(float amount)
        {
            CurrentWeaponIntegrity =
                Mathf.Clamp(
                    CurrentWeaponIntegrity +
                    Mathf.Max(0f, amount),
                    0f,
                    MaxWeaponIntegrity);
        }

        public float GetWeaponEffectiveness()
        {
            return Mathf.Clamp01(IntegrityPercent);
        }

        public void Reset()
        {
            CurrentWeaponIntegrity =
                MaxWeaponIntegrity;
        }
    }
}
