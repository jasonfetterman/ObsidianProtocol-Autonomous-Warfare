using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    public float maxHealth = 500f;
    public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
