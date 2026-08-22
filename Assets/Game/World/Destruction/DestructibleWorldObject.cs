using UnityEngine;

namespace ObsidianProtocol.Game.World.Destruction
{
    public sealed class DestructibleWorldObject : MonoBehaviour
    {
        [SerializeField] private DestructibleObjectDefinition definition;
        [SerializeField] private float currentHealth;

        public DestructibleObjectDefinition Definition => definition;
        public float CurrentHealth => currentHealth;
        public bool IsDestroyed => currentHealth <= 0f;

        private void Awake()
        {
            if (definition != null)
            {
                currentHealth = definition.MaximumHealth;
            }
        }

        public void ApplyDamage(float damage)
        {
            if (damage <= 0f || IsDestroyed)
            {
                return;
            }

            currentHealth = Mathf.Max(0f, currentHealth - damage);
        }

        public void Repair()
        {
            if (definition != null)
            {
                currentHealth = definition.MaximumHealth;
            }
        }
    }
}
