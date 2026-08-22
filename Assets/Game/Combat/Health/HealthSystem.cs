using UnityEngine;

public class HealthSystem
{
    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;

    public bool IsDestroyed => CurrentHealth <= 0f;

    public void TakeDamage(float amount)
    {
        Debug.Log($"HealthSystem: Taking {amount} damage.");

        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        if (IsDestroyed)
        {
            Debug.Log("Unit destroyed.");
        }
        else
        {
            Debug.Log($"Current health: {CurrentHealth}/{MaxHealth}");
        }
    }

    public void Heal(float amount)
    {
        Debug.Log($"HealthSystem: Healing {amount} HP.");

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        Debug.Log($"Current health: {CurrentHealth}/{MaxHealth}");
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        Debug.Log("Health reset to maximum.");
    }
}
