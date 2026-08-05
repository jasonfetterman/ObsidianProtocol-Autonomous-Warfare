using UnityEngine;

public class AbilityExamples : MonoBehaviour
{
    public AbilityUser abilityUser;

    void Start()
    {
        abilityUser.abilities = new Ability[]
        {
            new Ability
            {
                abilityName = "Grenade",
                cooldown = 8f,
                targetType = AbilityTargetType.Ground,
                aoeRadius = 4f,
                damage = 40f
            },

            new Ability
            {
                abilityName = "Battle Cry",
                cooldown = 12f,
                targetType = AbilityTargetType.Self,
                buffDuration = 5f,
                buffDamageMultiplier = 1.5f
            }
        };
    }
}
