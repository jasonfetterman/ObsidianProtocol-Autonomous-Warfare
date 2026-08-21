using UnityEngine;

public enum AbilityTargetType
{
    Self,
    Ally,
    Enemy,
    Ground
}

[System.Serializable]
public class Ability
{
    public string abilityName;
    public float cooldown = 5f;
    public float range = 10f;
    public AbilityTargetType targetType;

    public float aoeRadius = 0f;
    public float damage = 0f;
    public float heal = 0f;
    public float buffDuration = 0f;
    public float buffDamageMultiplier = 1f;

    [HideInInspector] public float lastUseTime = -Mathf.Infinity;

    public bool IsReady()
    {
        return Time.time >= lastUseTime + cooldown;
    }

    public void Trigger()
    {
        lastUseTime = Time.time;
    }
}
