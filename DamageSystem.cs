using UnityEngine;

public class DamageSystem
{
    // Core damage application method
    public float ApplyDamage(float baseDamage, float armorValue)
    {
        Debug.Log($\"Applying damage: base={baseDamage}, armor={armorValue}\");

        // Simple placeholder formula:
        // EffectiveDamage = BaseDamage - ArmorValue (minimum 0)
        float effectiveDamage = Mathf.Max(0f, baseDamage - armorValue);

        Debug.Log($\"Effective damage: {effectiveDamage}\");

        return effectiveDamage;
    }

    // Damage with multipliers (used by critical hits, component hits, etc.)
    public float ApplyDamageWithMultiplier(float baseDamage, float armorValue, float multiplier)
    {
        float rawDamage = baseDamage * multiplier;
        float effectiveDamage = Mathf.Max(0f, rawDamage - armorValue);

        Debug.Log($\"Damage with multiplier: base={baseDamage}, mult={multiplier}, armor={armorValue}, final={effectiveDamage}\");

        return effectiveDamage;
    }

    // Damage falloff (used by AoE, explosions, etc.)
    public float ApplyFalloffDamage(float baseDamage, float falloff)
    {
        float finalDamage = baseDamage * Mathf.Clamp01(falloff);

        Debug.Log($\"Falloff damage: base={baseDamage}, falloff={falloff}, final={finalDamage}\");

        return finalDamage;
    }
}
