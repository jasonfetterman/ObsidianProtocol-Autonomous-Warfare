using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum CommandLossEffect
    {
        FleetControlLoss,
        DataLoss,
        MappingLoss,
        IntelligenceLoss,
        FusionLoss,
        NetworkDegradation,
        AnalyticsLoss,
        AutonomyDegradation
    }

    public sealed class CommandLossState
    {
        public string UnitId { get; }

        private readonly HashSet<CommandLossEffect> activeEffects =
            new HashSet<CommandLossEffect>();

        public bool Destroyed { get; private set; }

        public float CommandCapabilityMultiplier { get; private set; }
        public float NetworkEfficiencyMultiplier { get; private set; }
        public float IntelligenceEfficiencyMultiplier { get; private set; }

        public CommandLossState(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            CommandCapabilityMultiplier = 1f;
            NetworkEfficiencyMultiplier = 1f;
            IntelligenceEfficiencyMultiplier = 1f;
        }

        public void ApplyDestruction(
            IEnumerable<CommandLossEffect> effects)
        {
            Destroyed = true;

            activeEffects.Clear();

            if (effects != null)
            {
                foreach (CommandLossEffect effect in effects)
                {
                    activeEffects.Add(effect);
                }
            }

            Recalculate();
        }

        public bool HasEffect(
            CommandLossEffect effect)
        {
            return activeEffects.Contains(effect);
        }

        public IReadOnlyCollection<CommandLossEffect> GetEffects()
        {
            return activeEffects;
        }

        private void Recalculate()
        {
            CommandCapabilityMultiplier = 1f;
            NetworkEfficiencyMultiplier = 1f;
            IntelligenceEfficiencyMultiplier = 1f;

            if (HasEffect(
                    CommandLossEffect.FleetControlLoss))
            {
                CommandCapabilityMultiplier *= 0.5f;
            }

            if (HasEffect(
                    CommandLossEffect.NetworkDegradation))
            {
                NetworkEfficiencyMultiplier *= 0.5f;
            }

            if (HasEffect(
                    CommandLossEffect.IntelligenceLoss))
            {
                IntelligenceEfficiencyMultiplier *= 0.5f;
            }

            if (HasEffect(
                    CommandLossEffect.FusionLoss))
            {
                IntelligenceEfficiencyMultiplier *= 0.75f;
            }

            if (HasEffect(
                    CommandLossEffect.AutonomyDegradation))
            {
                CommandCapabilityMultiplier *= 0.75f;
            }
        }

        public void Restore()
        {
            Destroyed = false;

            activeEffects.Clear();

            CommandCapabilityMultiplier = 1f;
            NetworkEfficiencyMultiplier = 1f;
            IntelligenceEfficiencyMultiplier = 1f;
        }
    }

    public sealed class CommandUnitDestructionConsequencesSystem
    {
        private readonly Dictionary<string, CommandLossState> states =
            new Dictionary<string, CommandLossState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new CommandLossState(unitId));
            }
        }

        public void ApplyDestruction(
            string unitId,
            IEnumerable<CommandLossEffect> effects)
        {
            RegisterUnit(unitId);

            states[unitId].ApplyDestruction(
                effects);
        }

        public bool IsDestroyed(
            string unitId)
        {
            return states.TryGetValue(
                       unitId,
                       out CommandLossState state) &&
                   state.Destroyed;
        }

        public float GetCommandCapability(
            string unitId)
        {
            return states.TryGetValue(
                       unitId,
                       out CommandLossState state)
                ? state.CommandCapabilityMultiplier
                : 1f;
        }

        public float GetNetworkEfficiency(
            string unitId)
        {
            return states.TryGetValue(
                       unitId,
                       out CommandLossState state)
                ? state.NetworkEfficiencyMultiplier
                : 1f;
        }

        public float GetIntelligenceEfficiency(
            string unitId)
        {
            return states.TryGetValue(
                       unitId,
                       out CommandLossState state)
                ? state.IntelligenceEfficiencyMultiplier
                : 1f;
        }

        public bool TryGetState(
            string unitId,
            out CommandLossState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void RestoreUnit(
            string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out CommandLossState state))
            {
                state.Restore();
            }
        }

        public void RemoveUnit(
            string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
