using UnityEngine;

namespace Obsidian.VR
{
    public class DroneHealth : MonoBehaviour
    {
        [Header("Health Settings")]
        public float maxHealth = 100f;
        public float currentHealth = 100f;

        [Header("Runtime")]
        public bool IsDead = false;

        private void Awake()
        {
            currentHealth = maxHealth;
            IsDead = false;
        }

        public void ApplyDamage(float amount)
        {
            if (IsDead)
                return;

            currentHealth -= amount;

            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                IsDead = true;
            }
        }

        public float GetHealth()
        {
            return currentHealth;
        }

        public float GetHealthPercent()
        {
            return maxHealth > 0f ? currentHealth / maxHealth : 0f;
        }

        // ---------------------------------------------------------
        // REQUIRED BY YOUR ERROR LOG
        // ---------------------------------------------------------

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            IsDead = false;
        }
    }
}
