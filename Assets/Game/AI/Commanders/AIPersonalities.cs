using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AI.Commanders
{
    public enum AICommanderPersonality
    {
        Balanced,
        Aggressive,
        Defensive,
        Cautious,
        Opportunistic,
        LogisticsFocused,
        ReconFocused
    }

    public sealed class AICommanderPersonalityProfile
    {
        public AICommanderPersonality Personality
        {
            get;
        }

        public float Aggression
        {
            get;
            private set;
        }

        public float RiskTolerance
        {
            get;
            private set;
        }

        public float DefensivePriority
        {
            get;
            private set;
        }

        public float ReconnaissancePriority
        {
            get;
            private set;
        }

        public float ReinforcementPriority
        {
            get;
            private set;
        }

        public float RetreatTolerance
        {
            get;
            private set;
        }

        public float LogisticsPriority
        {
            get;
            private set;
        }

        public bool Valid =>
            Personality !=
                AICommanderPersonality.Balanced ||
            Aggression >= 0.0f;

        public AICommanderPersonalityProfile(
            AICommanderPersonality personality)
        {
            Personality =
                personality;

            Aggression = 0.50f;
            RiskTolerance = 0.50f;
            DefensivePriority = 0.50f;
            ReconnaissancePriority = 0.50f;
            ReinforcementPriority = 0.50f;
            RetreatTolerance = 0.50f;
            LogisticsPriority = 0.50f;

            ConfigureDefaults();
        }

        public float ModifyAggression(
            float baseValue)
        {
            return Clamp01(
                baseValue +
                ((Aggression - 0.50f) *
                 0.40f));
        }

        public float ModifyRisk(
            float baseValue)
        {
            return Clamp01(
                baseValue +
                ((RiskTolerance - 0.50f) *
                 0.40f));
        }

        public float ModifyDefense(
            float baseValue)
        {
            return Clamp01(
                baseValue +
                ((DefensivePriority - 0.50f) *
                 0.40f));
        }

        public float ModifyRecon(
            float baseValue)
        {
            return Clamp01(
                baseValue +
                ((ReconnaissancePriority - 0.50f) *
                 0.40f));
        }

        public float ModifyReinforcement(
            float baseValue)
        {
            return Clamp01(
                baseValue +
                ((ReinforcementPriority - 0.50f) *
                 0.40f));
        }

        public float ModifyRetreat(
            float baseValue)
        {
            return Clamp01(
                baseValue +
                ((RetreatTolerance - 0.50f) *
                 0.40f));
        }

        public float ModifyLogistics(
            float baseValue)
        {
            return Clamp01(
                baseValue +
                ((LogisticsPriority - 0.50f) *
                 0.40f));
        }

        private void ConfigureDefaults()
        {
            switch (Personality)
            {
                case AICommanderPersonality.Aggressive:
                    Aggression = 0.90f;
                    RiskTolerance = 0.85f;
                    DefensivePriority = 0.30f;
                    ReconnaissancePriority = 0.40f;
                    ReinforcementPriority = 0.55f;
                    RetreatTolerance = 0.80f;
                    LogisticsPriority = 0.40f;
                    break;

                case AICommanderPersonality.Defensive:
                    Aggression = 0.30f;
                    RiskTolerance = 0.25f;
                    DefensivePriority = 0.90f;
                    ReconnaissancePriority = 0.70f;
                    ReinforcementPriority = 0.80f;
                    RetreatTolerance = 0.25f;
                    LogisticsPriority = 0.75f;
                    break;

                case AICommanderPersonality.Cautious:
                    Aggression = 0.25f;
                    RiskTolerance = 0.15f;
                    DefensivePriority = 0.75f;
                    ReconnaissancePriority = 0.85f;
                    ReinforcementPriority = 0.75f;
                    RetreatTolerance = 0.15f;
                    LogisticsPriority = 0.80f;
                    break;

                case AICommanderPersonality.Opportunistic:
                    Aggression = 0.75f;
                    RiskTolerance = 0.70f;
                    DefensivePriority = 0.45f;
                    ReconnaissancePriority = 0.80f;
                    ReinforcementPriority = 0.50f;
                    RetreatTolerance = 0.60f;
                    LogisticsPriority = 0.55f;
                    break;

                case AICommanderPersonality.LogisticsFocused:
                    Aggression = 0.45f;
                    RiskTolerance = 0.40f;
                    DefensivePriority = 0.65f;
                    ReconnaissancePriority = 0.65f;
                    ReinforcementPriority = 0.90f;
                    RetreatTolerance = 0.45f;
                    LogisticsPriority = 1.00f;
                    break;

                case AICommanderPersonality.ReconFocused:
                    Aggression = 0.50f;
                    RiskTolerance = 0.40f;
                    DefensivePriority = 0.55f;
                    ReconnaissancePriority = 1.00f;
                    ReinforcementPriority = 0.60f;
                    RetreatTolerance = 0.40f;
                    LogisticsPriority = 0.60f;
                    break;

                default:
                    Aggression = 0.50f;
                    RiskTolerance = 0.50f;
                    DefensivePriority = 0.50f;
                    ReconnaissancePriority = 0.50f;
                    ReinforcementPriority = 0.50f;
                    RetreatTolerance = 0.50f;
                    LogisticsPriority = 0.50f;
                    break;
            }
        }

        private static float Clamp01(
            float value)
        {
            return Math.Max(
                0.0f,
                Math.Min(
                    1.0f,
                    value));
        }
    }

    public sealed class AICommanderPersonalityRegistry
    {
        private readonly Dictionary<
            string,
            AICommanderPersonalityProfile> profiles =
            new Dictionary<
                string,
                AICommanderPersonalityProfile>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            string commanderId,
            AICommanderPersonality personality)
        {
            if (string.IsNullOrWhiteSpace(
                    commanderId) ||
                profiles.ContainsKey(
                    commanderId))
            {
                return false;
            }

            AICommanderPersonalityProfile profile =
                new AICommanderPersonalityProfile(
                    personality);

            profiles.Add(
                commanderId,
                profile);

            return true;
        }

        public bool Remove(
            string commanderId)
        {
            if (string.IsNullOrWhiteSpace(
                    commanderId))
            {
                return false;
            }

            return profiles.Remove(
                commanderId);
        }

        public bool TryGet(
            string commanderId,
            out AICommanderPersonalityProfile profile)
        {
            return profiles.TryGetValue(
                commanderId,
                out profile);
        }

        public IReadOnlyCollection<
            AICommanderPersonalityProfile>
            GetProfiles()
        {
            return profiles.Values;
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }

    public sealed class AICommanderPersonalitySelector
    {
        public AICommanderPersonalityProfile CreateProfile(
            AICommanderPersonality personality)
        {
            return new AICommanderPersonalityProfile(
                personality);
        }

        public AICommanderPersonalityProfile
            CreateBalanced()
        {
            return CreateProfile(
                AICommanderPersonality.Balanced);
        }

        public AICommanderPersonalityProfile
            CreateAggressive()
        {
            return CreateProfile(
                AICommanderPersonality.Aggressive);
        }

        public AICommanderPersonalityProfile
            CreateDefensive()
        {
            return CreateProfile(
                AICommanderPersonality.Defensive);
        }

        public AICommanderPersonalityProfile
            CreateCautious()
        {
            return CreateProfile(
                AICommanderPersonality.Cautious);
        }

        public AICommanderPersonalityProfile
            CreateOpportunistic()
        {
            return CreateProfile(
                AICommanderPersonality.Opportunistic);
        }

        public AICommanderPersonalityProfile
            CreateLogisticsFocused()
        {
            return CreateProfile(
                AICommanderPersonality.LogisticsFocused);
        }

        public AICommanderPersonalityProfile
            CreateReconFocused()
        {
            return CreateProfile(
                AICommanderPersonality.ReconFocused);
        }
    }
}
