using UnityEngine;

namespace ObsidianProtocol.Game.Combat.Armor
{
    public sealed class ArmorSystem : MonoBehaviour
    {
        [SerializeField] private ArmorDefinition definition;

        public ArmorDefinition Definition => definition;

        public float GetArmorValue(Vector3 localHitDirection)
        {
            if (definition == null)
            {
                return 0f;
            }

            Vector3 direction = localHitDirection.normalized;

            if (direction.y > 0.5f)
            {
                return definition.TopArmor;
            }

            if (direction.z >= 0.5f)
            {
                return definition.FrontArmor;
            }

            if (direction.z <= -0.5f)
            {
                return definition.RearArmor;
            }

            return definition.SideArmor;
        }

        public float AbsorbDamage(
            float incomingDamage,
            Vector3 localHitDirection)
        {
            if (incomingDamage <= 0f)
            {
                return 0f;
            }

            float armor = GetArmorValue(localHitDirection);

            return Mathf.Max(0f, incomingDamage - armor);
        }
    }
}
