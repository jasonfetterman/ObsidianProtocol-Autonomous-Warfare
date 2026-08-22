using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Combat.ComponentDamage
{
    public enum ComponentType
    {
        Hull,
        Mobility,
        Sensors,
        Weapons,
        Engine,
        Command
    }

    [Serializable]
    public sealed class ComponentState
    {
        public ComponentType Type;
        public float MaxHealth = 100f;
        public float CurrentHealth = 100f;
        public bool IsDisabled => CurrentHealth <= 0f;

        public ComponentState(ComponentType type, float health)
        {
            Type = type;
            MaxHealth = Mathf.Max(0f, health);
            CurrentHealth = MaxHealth;
        }

        public float ApplyDamage(float damage)
        {
            damage = Mathf.Max(0f, damage);

            float applied =
                Mathf.Min(CurrentHealth, damage);

            CurrentHealth -= applied;

            return damage - applied;
        }

        public void Repair(float amount)
        {
            CurrentHealth =
                Mathf.Clamp(
                    CurrentHealth + Mathf.Max(0f, amount),
                    0f,
                    MaxHealth);
        }

        public void Reset()
        {
            CurrentHealth = MaxHealth;
        }
    }

    public sealed class ComponentDamageSystem
    {
        private readonly Dictionary<ComponentType, ComponentState> components =
            new Dictionary<ComponentType, ComponentState>();

        public IReadOnlyDictionary<ComponentType, ComponentState> Components =>
            components;

        public void RegisterComponent(
            ComponentType type,
            float maxHealth)
        {
            components[type] =
                new ComponentState(type, maxHealth);
        }

        public bool HasComponent(ComponentType type)
        {
            return components.ContainsKey(type);
        }

        public bool TryGetComponent(
            ComponentType type,
            out ComponentState component)
        {
            return components.TryGetValue(
                type,
                out component);
        }

        public float ApplyDamage(
            ComponentType type,
            float damage)
        {
            if (!components.TryGetValue(
                    type,
                    out ComponentState component))
            {
                return Mathf.Max(0f, damage);
            }

            return component.ApplyDamage(damage);
        }

        public void RepairComponent(
            ComponentType type,
            float amount)
        {
            if (components.TryGetValue(
                    type,
                    out ComponentState component))
            {
                component.Repair(amount);
            }
        }

        public void ResetAll()
        {
            foreach (ComponentState component in components.Values)
            {
                component.Reset();
            }
        }
    }
}
