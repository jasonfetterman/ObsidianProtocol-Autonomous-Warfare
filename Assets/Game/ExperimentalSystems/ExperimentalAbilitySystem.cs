using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum ExperimentalAbilityType
    {
        SignalIntrusion,
        ElectronicPulse,
        SignatureSuppression,
        CovertRelay,
        AdaptiveProcessing,
        SystemOverdrive
    }

    public enum ExperimentalAbilityState
    {
        Locked,
        Ready,
        Active,
        Cooldown,
        Disabled
    }

    public sealed class ExperimentalAbility
    {
        public string AbilityId { get; }
        public string UnitId { get; }

        public ExperimentalAbilityType Type { get; }

        public ExperimentalAbilityState State
        {
            get;
            private set;
        }

        public float Power { get; private set; }
        public float Cooldown { get; private set; }
        public float RemainingCooldown { get; private set; }

        public bool Active =>
            State ==
            ExperimentalAbilityState.Active;

        public ExperimentalAbility(
            string abilityId,
            string unitId,
            ExperimentalAbilityType type)
        {
            AbilityId =
                abilityId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            Type =
                type;

            State =
                ExperimentalAbilityState.Locked;
        }

        public void Configure(
            float power,
            float cooldown)
        {
            Power =
                Math.Clamp(
                    power,
                    0f,
                    1f);

            Cooldown =
                Math.Max(
                    0f,
                    cooldown);
        }

        public void Unlock()
        {
            if (State ==
                ExperimentalAbilityState.Locked)
            {
                State =
                    ExperimentalAbilityState.Ready;
            }
        }

        public bool Activate()
        {
            if (State !=
                    ExperimentalAbilityState.Ready ||
                RemainingCooldown > 0f)
            {
                return false;
            }

            State =
                ExperimentalAbilityState.Active;

            return true;
        }

        public void Deactivate()
        {
            if (State !=
                ExperimentalAbilityState.Active)
            {
                return;
            }

            RemainingCooldown =
                Cooldown;

            State =
                RemainingCooldown > 0f
                    ? ExperimentalAbilityState.Cooldown
                    : ExperimentalAbilityState.Ready;
        }

        public void Update(
            float deltaTime)
        {
            if (RemainingCooldown <= 0f)
            {
                if (State ==
                    ExperimentalAbilityState.Cooldown)
                {
                    State =
                        ExperimentalAbilityState.Ready;
                }

                return;
            }

            RemainingCooldown =
                Math.Max(
                    0f,
                    RemainingCooldown -
                    Math.Max(
                        0f,
                        deltaTime));

            if (RemainingCooldown <= 0f &&
                State ==
                ExperimentalAbilityState.Cooldown)
            {
                State =
                    ExperimentalAbilityState.Ready;
            }
        }

        public void Disable()
        {
            State =
                ExperimentalAbilityState.Disabled;
        }

        public void Enable()
        {
            if (State ==
                ExperimentalAbilityState.Disabled)
            {
                State =
                    ExperimentalAbilityState.Ready;
            }
        }
    }

    public sealed class ExperimentalAbilitySystem
    {
        private readonly Dictionary<string, ExperimentalAbility> abilities =
            new Dictionary<string, ExperimentalAbility>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterAbility(
            string abilityId,
            string unitId,
            ExperimentalAbilityType type)
        {
            if (string.IsNullOrWhiteSpace(abilityId))
            {
                return;
            }

            abilities[abilityId] =
                new ExperimentalAbility(
                    abilityId,
                    unitId,
                    type);
        }

        public void ConfigureAbility(
            string abilityId,
            float power,
            float cooldown)
        {
            if (abilities.TryGetValue(
                    abilityId,
                    out ExperimentalAbility ability))
            {
                ability.Configure(
                    power,
                    cooldown);
            }
        }

        public void UnlockAbility(
            string abilityId)
        {
            if (abilities.TryGetValue(
                    abilityId,
                    out ExperimentalAbility ability))
            {
                ability.Unlock();
            }
        }

        public bool ActivateAbility(
            string abilityId)
        {
            return abilities.TryGetValue(
                       abilityId,
                       out ExperimentalAbility ability) &&
                   ability.Activate();
        }

        public void DeactivateAbility(
            string abilityId)
        {
            if (abilities.TryGetValue(
                    abilityId,
                    out ExperimentalAbility ability))
            {
                ability.Deactivate();
            }
        }

        public void Update(
            float deltaTime)
        {
            foreach (ExperimentalAbility ability
                in abilities.Values)
            {
                ability.Update(
                    deltaTime);
            }
        }

        public bool IsActive(
            string abilityId)
        {
            return abilities.TryGetValue(
                       abilityId,
                       out ExperimentalAbility ability) &&
                   ability.Active;
        }

        public bool TryGetAbility(
            string abilityId,
            out ExperimentalAbility ability)
        {
            return abilities.TryGetValue(
                abilityId,
                out ability);
        }

        public IReadOnlyCollection<ExperimentalAbility>
            GetAbilities()
        {
            return abilities.Values;
        }

        public void RemoveAbility(
            string abilityId)
        {
            abilities.Remove(
                abilityId);
        }

        public void Clear()
        {
            abilities.Clear();
        }
    }
}
