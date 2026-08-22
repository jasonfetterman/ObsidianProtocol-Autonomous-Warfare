using System;
using UnityEngine;

namespace ObsidianProtocol.Game.Combat.Destruction
{
    public sealed class DestructionSystem
    {
        public bool IsDestroyed { get; private set; }

        public event Action Destroyed;

        public bool TryDestroy()
        {
            if (IsDestroyed)
            {
                return false;
            }

            IsDestroyed = true;
            Destroyed?.Invoke();

            return true;
        }

        public bool TryDestroy(float currentHealth)
        {
            if (currentHealth > 0f)
            {
                return false;
            }

            return TryDestroy();
        }

        public void Reset()
        {
            IsDestroyed = false;
        }
    }

    public sealed class DestructibleEntity : MonoBehaviour
    {
        private DestructionSystem destructionSystem;

        public bool IsDestroyed =>
            destructionSystem != null &&
            destructionSystem.IsDestroyed;

        public event Action Destroyed;

        private void Awake()
        {
            destructionSystem = new DestructionSystem();
            destructionSystem.Destroyed += HandleDestroyed;
        }

        public bool DestroyEntity()
        {
            if (destructionSystem == null)
            {
                destructionSystem = new DestructionSystem();
                destructionSystem.Destroyed += HandleDestroyed;
            }

            return destructionSystem.TryDestroy();
        }

        public void ResetDestruction()
        {
            destructionSystem?.Reset();
        }

        private void HandleDestroyed()
        {
            Destroyed?.Invoke();
        }

        private void OnDestroy()
        {
            if (destructionSystem != null)
            {
                destructionSystem.Destroyed -= HandleDestroyed;
            }
        }
    }
}
