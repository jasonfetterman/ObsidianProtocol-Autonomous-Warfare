using UnityEngine;

namespace ObsidianProtocol.Game.Combat.CriticalDamage
{
    public sealed class CriticalDamageSystem
    {
        private readonly float criticalMultiplier;

        public CriticalDamageSystem(
            float criticalMultiplier = 2f)
        {
            this.criticalMultiplier =
                Mathf.Max(1f, criticalMultiplier);
        }

        public float CalculateDamage(
            float baseDamage,
            bool criticalHit)
        {
            float damage =
                Mathf.Max(0f, baseDamage);

            return criticalHit
                ? damage * criticalMultiplier
                : damage;
        }

        public bool IsCritical(
            float criticalChance)
        {
            return Random.value <
                   Mathf.Clamp01(criticalChance);
        }
    }
}
