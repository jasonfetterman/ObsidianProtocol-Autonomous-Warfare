using UnityEngine;

public class AbilityUser : MonoBehaviour
{
    public Ability[] abilities;

    UnitMover mover;
    Shooter shooter;
    Health health;

    void Awake()
    {
        mover = GetComponent<UnitMover>();
        shooter = GetComponent<Shooter>();
        health = GetComponent<Health>();
    }

    public void UseAbility(int index, Vector3 targetPos, GameObject targetObj)
    {
        if (index < 0 || index >= abilities.Length) return;

        Ability ability = abilities[index];
        if (!ability.IsReady()) return;

        ability.Trigger();

        switch (ability.targetType)
        {
            case AbilityTargetType.Self:
                ApplyEffects(ability, gameObject);
                break;

            case AbilityTargetType.Ally:
            case AbilityTargetType.Enemy:
                if (targetObj != null)
                    ApplyEffects(ability, targetObj);
                break;

            case AbilityTargetType.Ground:
                ApplyAOE(ability, targetPos);
                break;
        }
    }

    void ApplyEffects(Ability ability, GameObject target)
    {
        Health h = target.GetComponent<Health>();
        if (h != null)
        {
            if (ability.damage > 0)
                h.TakeDamage(ability.damage, DamageClass.Kinetic, gameObject);

            if (ability.heal > 0)
                h.currentHealth = Mathf.Min(h.currentHealth + ability.heal, h.baseHealth);
        }

        if (ability.buffDuration > 0)
        {
            BuffSystem buff = target.GetComponent<BuffSystem>();
            if (buff != null)
                buff.ApplyBuff(ability.buffDuration, ability.buffDamageMultiplier);
        }
    }

    void ApplyAOE(Ability ability, Vector3 pos)
    {
        Collider[] hits = Physics.OverlapSphere(pos, ability.aoeRadius);

        foreach (var h in hits)
        {
            ApplyEffects(ability, h.gameObject);
        }
    }
}
