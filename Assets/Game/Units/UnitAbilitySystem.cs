using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public enum UnitAbility
    {
        ActiveRecon,
        SensorSweep,
        TargetDesignation,
        ElectronicJamming,
        NetworkRelay,
        EmergencyRepair,
        SmokeScreen,
        Stealth,
        Overwatch,
        Suppression,
        Breach,
        FlankingAssist,
        Pursuit,
        TacticalRetreat,
        ReinforcementCall,
        BattlefieldScan,
        CommandBoost,
        EnergyBoost,
        EmergencyExtraction
    }

    public sealed class UnitAbilityState
    {
        public UnitAbility Ability { get; }
        public bool Enabled { get; private set; }
        public bool OnCooldown { get; private set; }
        public float CooldownRemaining { get; private set; }

        public UnitAbilityState(UnitAbility ability)
        {
            Ability = ability;
            Enabled = true;
            OnCooldown = false;
            CooldownRemaining = 0f;
        }

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;

            if (!enabled)
            {
                OnCooldown = false;
                CooldownRemaining = 0f;
            }
        }

        public bool TryActivate(float cooldown)
        {
            if (!Enabled || OnCooldown)
            {
                return false;
            }

            CooldownRemaining =
                Math.Max(0f, cooldown);

            OnCooldown =
                CooldownRemaining > 0f;

            return true;
        }

        public void Tick(float deltaTime)
        {
            if (!OnCooldown)
            {
                return;
            }

            CooldownRemaining =
                Math.Max(
                    0f,
                    CooldownRemaining -
                    Math.Max(0f, deltaTime));

            if (CooldownRemaining <= 0f)
            {
                OnCooldown = false;
            }
        }
    }

    public sealed class UnitAbilitySystem
    {
        private readonly Dictionary<string, Dictionary<UnitAbility, UnitAbilityState>> abilities =
            new Dictionary<string, Dictionary<UnitAbility, UnitAbilityState>>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!abilities.ContainsKey(unitId))
            {
                abilities.Add(
                    unitId,
                    new Dictionary<UnitAbility, UnitAbilityState>());
            }
        }

        public void AddAbility(
            string unitId,
            UnitAbility ability)
        {
            RegisterUnit(unitId);

            Dictionary<UnitAbility, UnitAbilityState> unitAbilities =
                abilities[unitId];

            if (!unitAbilities.ContainsKey(ability))
            {
                unitAbilities.Add(
                    ability,
                    new UnitAbilityState(ability));
            }
        }

        public void RemoveAbility(
            string unitId,
            UnitAbility ability)
        {
            if (abilities.TryGetValue(
                    unitId,
                    out Dictionary<UnitAbility, UnitAbilityState> unitAbilities))
            {
                unitAbilities.Remove(ability);
            }
        }

        public bool HasAbility(
            string unitId,
            UnitAbility ability)
        {
            return abilities.TryGetValue(
                       unitId,
                       out Dictionary<UnitAbility, UnitAbilityState> unitAbilities) &&
                   unitAbilities.ContainsKey(ability);
        }

        public bool TryActivate(
            string unitId,
            UnitAbility ability,
            float cooldown)
        {
            if (!abilities.TryGetValue(
                    unitId,
                    out Dictionary<UnitAbility, UnitAbilityState> unitAbilities))
            {
                return false;
            }

            if (!unitAbilities.TryGetValue(
                    ability,
                    out UnitAbilityState state))
            {
                return false;
            }

            return state.TryActivate(cooldown);
        }

        public void Tick(
            string unitId,
            float deltaTime)
        {
            if (!abilities.TryGetValue(
                    unitId,
                    out Dictionary<UnitAbility, UnitAbilityState> unitAbilities))
            {
                return;
            }

            foreach (UnitAbilityState state in unitAbilities.Values)
            {
                state.Tick(deltaTime);
            }
        }

        public void RemoveUnit(string unitId)
        {
            abilities.Remove(unitId);
        }

        public void Clear()
        {
            abilities.Clear();
        }
    }
}
