using UnityEngine;

public class Health : MonoBehaviour
{
    public float baseHealth = 100f;
    public float currentHealth;

    ArmorType armorType;
    DamageResolver damageResolver;
    RagdollActivator ragdoll;
    Equipment equipment;
    CoverResolver coverResolver;

    public GameObject lastAttacker;

    LootTable lootTable;
    LootDrop lootDrop;

    void Awake()
    {
        equipment = GetComponent<Equipment>();
        coverResolver = GetComponent<CoverResolver>();

        float bonus = equipment != null ? equipment.GetBonusHealth() : 0f;
        currentHealth = baseHealth + bonus;

        armorType = GetComponent<ArmorType>();
        damageResolver = GetComponent<DamageResolver>();
        ragdoll = GetComponent<RagdollActivator>();

        lootTable = GetComponent<LootTable>();
        lootDrop = GetComponent<LootDrop>();
    }

    public void TakeDamage(float amount, DamageClass type, GameObject attacker)
    {
        lastAttacker = attacker;

        float dmg = amount;

        if (damageResolver != null)
            dmg = damageResolver.ApplyType(type, dmg);

        if (armorType != null)
            dmg = armorType.ApplyArmor(dmg);

        if (equipment != null)
            dmg -= equipment.GetBonusArmor();

        if (coverResolver != null && attacker != null)
            dmg *= coverResolver.GetCoverMultiplier(attacker.transform.position);

        if (dmg < 0f) dmg = 0f;

        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (lastAttacker != null)
        {
            UnitXP xp = lastAttacker.GetComponent<UnitXP>();
            if (xp != null)
                xp.AddXP(50f);
        }

        if (lootTable != null && lootDrop != null)
        {
            Item loot = lootTable.GetLoot();
            lootDrop.Drop(loot);
        }

        if (ragdoll != null)
        {
            ragdoll.EnableRagdoll();
            Destroy(gameObject, 5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
