using UnityEngine;

namespace ObsidianProtocol.Game.Combat.CriticalDamage
{
    public sealed class CriticalDamageSystem : MonoBehaviour
    {
        [SerializeField] private CriticalDamageDefinition definition;

        public CriticalDamageDefinition Definition => definition;

        public bool RollCritical()
        {
            if (definition == null)
            {
                return false;
            }

            return Random.value <= definition.CriticalChance;
        }

        public float ApplyCriticalMultiplier(float damage)
        {
            if (damage <= 0f || definition == null)
            {
                return Mathf.Max(0f, damage);
            }

            return damage * definition.CriticalMultiplier;
        }

        public float ResolveDamage(float damage, out bool wasCritical)
        {
            wasCritical = RollCritical();

            if (!wasCritical)
            {
                return Mathf.Max(0f, damage);
            }

            return ApplyCriticalMultiplier(damage);
        }
    }
}
