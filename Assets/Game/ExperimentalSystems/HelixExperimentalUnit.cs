using System;

namespace ObsidianProtocol.Game.ExperimentalSystems.Units
{
    public sealed class HelixExperimentalUnit
    {
        public const string UnitId = "HELIX";
        public const string UnitName = "Helix";

        private readonly ExperimentalFramework framework;
        private readonly ExperimentalUnitRestrictionsSystem restrictions;
        private readonly ExperimentalAISystem experimentalAI;
        private readonly ExperimentalAbilitySystem abilities;
        private readonly ExperimentalRiskSystem risks;
        private readonly ExperimentalProgressionSystem progression;

        public HelixExperimentalUnit(
            ExperimentalFramework framework,
            ExperimentalUnitRestrictionsSystem restrictions,
            ExperimentalAISystem experimentalAI,
            ExperimentalAbilitySystem abilities,
            ExperimentalRiskSystem risks,
            ExperimentalProgressionSystem progression)
        {
            this.framework = framework;
            this.restrictions = restrictions;
            this.experimentalAI = experimentalAI;
            this.abilities = abilities;
            this.risks = risks;
            this.progression = progression;

            Initialize();
        }

        public void Initialize()
        {
            framework.RegisterUnit(
                UnitId,
                UnitName,
                ExperimentalUnitType.Helix);

            framework.ConfigureUnit(
                UnitId,
                authorized: false,
                autonomous: true);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.ExperimentalAI);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.ExperimentalAbilities);

            restrictions.RegisterUnit(UnitId);

            restrictions.AddRestriction(
                UnitId,
                ExperimentalRestriction.AuthorizationRequired);

            restrictions.AddRestriction(
                UnitId,
                ExperimentalRestriction.ResearchRequired);

            restrictions.AddRestriction(
                UnitId,
                ExperimentalRestriction.StabilityRequired);

            restrictions.AddRestriction(
                UnitId,
                ExperimentalRestriction.DeploymentLimit);

            restrictions.SetDeploymentLimit(
                UnitId,
                1);

            restrictions.SetMinimumStability(
                UnitId,
                0.85f);

            experimentalAI.RegisterUnit(UnitId);

            experimentalAI.ConfigureUnit(
                UnitId,
                learningRate: 0.25f,
                autonomous: true);

            experimentalAI.SetState(
                UnitId,
                ExperimentalAIState.Initializing);

            abilities.RegisterAbility(
                "HELIX-ADAPTIVE-PROCESSING",
                UnitId,
                ExperimentalAbilityType.AdaptiveProcessing);

            abilities.ConfigureAbility(
                "HELIX-ADAPTIVE-PROCESSING",
                power: 1.0f,
                cooldown: 20f);

            risks.RegisterUnit(UnitId);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.AIDeviation,
                0.25f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.Overload,
                0.20f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.SystemFailure,
                0.15f);

            progression.RegisterUnit(UnitId);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.Research);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.FieldTesting);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.Stability);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.MissionCompletion);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.ResourceInvestment);
        }

        public bool Authorize()
        {
            framework.SetAuthorization(
                UnitId,
                true);

            experimentalAI.SetState(
                UnitId,
                ExperimentalAIState.Operational);

            return true;
        }

        public bool CanDeploy(
            bool researchComplete,
            float stability,
            int currentlyDeployed)
        {
            return restrictions.CanDeploy(
                UnitId,
                authorized: true,
                researchComplete,
                stability,
                currentlyDeployed);
        }

        public bool ActivateAdaptiveProcessing()
        {
            if (!abilities.ActivateAbility(
                    "HELIX-ADAPTIVE-PROCESSING"))
            {
                return false;
            }

            experimentalAI.Learn(
                UnitId,
                0.10f);

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.AIDeviation,
                0.08f);

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.Overload,
                0.06f);

            return true;
        }

        public void DeactivateAdaptiveProcessing()
        {
            abilities.DeactivateAbility(
                "HELIX-ADAPTIVE-PROCESSING");
        }

        public void Update(
            float deltaTime)
        {
            abilities.Update(
                deltaTime);
        }

        public float GetLearningProgress()
        {
            if (experimentalAI.TryGetProfile(UnitId, out ExperimentalAIProfile profile))
            {
                return profile.AdaptationLevel;
            }

            return 0f;
        }

        public float GetRisk()
        {
            return risks.GetOverallRisk(
                UnitId);
        }

        public ExperimentalRiskLevel GetRiskLevel()
        {
            return risks.GetRiskLevel(
                UnitId);
        }

        public bool TryGetUnit(
            out ExperimentalUnit unit)
        {
            return framework.TryGetUnit(
                UnitId,
                out unit);
        }
    }
}



