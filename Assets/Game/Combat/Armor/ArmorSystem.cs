using UnityEngine;

namespace ObsidianProtocol.Game.Combat.Armor
{
    public sealed class ArmorSystem
    {
        public float MaxArmor { get; private set; }
        public float CurrentArmor { get; private set; }

        public bool IsDepleted =>
            CurrentArmor <= 0f;

        public ArmorSystem(float armor)
        {
            MaxArmor = Mathf.Max(0f, armor);
            CurrentArmor = MaxArmor;
        }

        public float AbsorbDamage(float damage)
        {
            if (damage <= 0f || CurrentArmor <= 0f)
            {
                return Mathf.Max(0f, damage);
            }

            float absorbed =
                Mathf.Min(CurrentArmor, damage);

            CurrentArmor -= absorbed;

            return damage - absorbed;
        }

        public void Restore(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            CurrentArmor =
                Mathf.Clamp(
                    CurrentArmor + amount,
                    0f,
                    MaxArmor);
        }

        public void Reset()
        {
            CurrentArmor = MaxArmor;
        }
    }
}
