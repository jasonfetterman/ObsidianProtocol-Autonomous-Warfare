using UnityEngine;

public class UnitXP : MonoBehaviour
{
    public int level = 1;
    public float xp = 0f;
    public float xpToNext = 100f;

    public float levelDamageBonus = 0.1f;   // +10% damage per level
    public float levelHealthBonus = 0.1f;   // +10% max HP per level

    Health health;

    void Awake()
    {
        health = GetComponent<Health>();

        if (health != null)
        {
            maxHealth = health.currentHealth;
        }
    }

    public float maxHealth;

    public void AddXP(float amount)
    {
        xp += amount;

        if (xp >= xpToNext)
        {
            xp -= xpToNext;
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;

        if (health != null)
        {
            float bonus = maxHealth * levelHealthBonus;
            maxHealth += bonus;
            health.currentHealth += bonus;
        }
    }

    public float GetDamageMultiplier()
    {
        return 1f + (level * levelDamageBonus);
    }
}
