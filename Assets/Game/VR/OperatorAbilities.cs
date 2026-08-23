using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public enum OperatorAbilityType
    {
        None,
        Boost,
        Shield,
        SensorPulse,
        EmergencyRepair,
        Stealth,
        Overdrive
    }

    public sealed class OperatorAbility
    {
        public string AbilityId { get; }

        public OperatorAbilityType Type { get; }

        public float Cooldown { get; }

        public float CooldownRemaining { get; private set; }

        public bool Active { get; private set; }

        public OperatorAbility(
            string abilityId,
            OperatorAbilityType type,
            float cooldown)
        {
            AbilityId =
                abilityId ?? string.Empty;

            Type = type;

            Cooldown =
                Math.Max(0f, cooldown);

            CooldownRemaining = 0f;
            Active = false;
        }

        public bool CanActivate()
        {
            return CooldownRemaining <= 0f &&
                   !Active;
        }

        public bool Activate()
        {
            if (!CanActivate())
            {
                return false;
            }

            Active = true;
            CooldownRemaining =
                Cooldown;

            return true;
        }

        public bool Deactivate()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;

            return true;
        }

        public void Update(
            float deltaTime)
        {
            if (deltaTime <= 0f ||
                CooldownRemaining <= 0f)
            {
                return;
            }

            CooldownRemaining =
                Math.Max(
                    0f,
                    CooldownRemaining - deltaTime);
        }

        public void Reset()
        {
            CooldownRemaining = 0f;
            Active = false;
        }
    }

    public sealed class OperatorAbilities
    {
        private readonly Dictionary<
            string,
            OperatorAbility> abilities =
            new Dictionary<
                string,
                OperatorAbility>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string UnitId { get; private set; }

        public int AbilityCount =>
            abilities.Count;

        public bool Initialize(
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            abilities.Clear();

            Active = false;
            Initialized = true;

            return true;
        }

        public bool RegisterAbility(
            string abilityId,
            OperatorAbilityType type,
            float cooldown)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(abilityId) ||
                type == OperatorAbilityType.None ||
                cooldown < 0f)
            {
                return false;
            }

            string id =
                abilityId.Trim();

            if (abilities.ContainsKey(id))
            {
                return false;
            }

            abilities.Add(
                id,
                new OperatorAbility(
                    id,
                    type,
                    cooldown));

            return true;
        }

        public bool Activate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = false;

            foreach (OperatorAbility ability
                     in abilities.Values)
            {
                ability.Reset();
            }

            return true;
        }

        public bool ActivateAbility(
            string abilityId)
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            OperatorAbility ability =
                GetAbility(abilityId);

            return ability != null &&
                   ability.Activate();
        }

        public bool DeactivateAbility(
            string abilityId)
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            OperatorAbility ability =
                GetAbility(abilityId);

            return ability != null &&
                   ability.Deactivate();
        }

        public void Update(
            float deltaTime)
        {
            if (!Initialized ||
                !Active)
            {
                return;
            }

            foreach (OperatorAbility ability
                     in abilities.Values)
            {
                ability.Update(deltaTime);
            }
        }

        public OperatorAbility GetAbility(
            string abilityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(abilityId))
            {
                return null;
            }

            abilities.TryGetValue(
                abilityId.Trim(),
                out OperatorAbility ability);

            return ability;
        }

        public IReadOnlyCollection<OperatorAbility>
            GetAbilities()
        {
            return abilities.Values;
        }

        public void Reset()
        {
            abilities.Clear();

            Initialized = false;
            Active = false;

            UnitId =
                string.Empty;
        }
    }
}
