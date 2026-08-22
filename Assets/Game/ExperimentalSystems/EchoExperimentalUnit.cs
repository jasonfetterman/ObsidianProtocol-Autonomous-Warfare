using System;
using ObsidianProtocol.Game.ExperimentalSystems;

namespace ObsidianProtocol.Game.ExperimentalSystems.Units
{
    public sealed class EchoExperimentalUnit
    {
        public const string UnitId = "ECHO";
        public const string UnitName = "Echo";

        private readonly ExperimentalFramework framework;
        private readonly ExperimentalUnitRestrictionsSystem restrictions;
        private readonly SignalIntelligenceSystem signalIntelligence;
        private readonly ElectronicDisruptionSystem electronicDisruption;
        private readonly ExperimentalAISystem experimentalAI;
        private readonly ExperimentalAbilitySystem abilities;
        private readonly ExperimentalRiskSystem risks;
        private readonly ExperimentalProgressionSystem progression;

        public EchoExperimentalUnit(
            ExperimentalFramework framework,
            ExperimentalUnitRestrictionsSystem restrictions,
            SignalIntelligenceSystem signalIntelligence,
            ElectronicDisruptionSystem electronicDisruption,
            ExperimentalAISystem experimentalAI,
            ExperimentalAbilitySystem abilities,
            ExperimentalRiskSystem risks,
            ExperimentalProgressionSystem progression)
        {
            this.framework =
                framework;

            this.restrictions =
                restrictions;

            this.signalIntelligence =
                signalIntelligence;

            this.electronicDisruption =
                electronicDisruption;

            this.experimentalAI =
                experimentalAI;

            this.abilities =
                abilities;

            this.risks =
                risks;

            this.progression =
                progression;

            Initialize();
        }

        public void Initialize()
        {
            framework.RegisterUnit(
                UnitId,
                UnitName,
                ExperimentalUnitType.Echo);

            framework.ConfigureUnit(
                UnitId,
                authorized: false,
                autonomous: true);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.SignalIntelligence);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.ExperimentalAI);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.ExperimentalAbilities);

            restrictions.RegisterUnit(
                UnitId);

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
                0.65f);

            signalIntelligence.DetectSignal(
                "ECHO-SIGNAL",
                UnitId,
                SignalType.Telemetry,
                0f,
                0f);

            experimentalAI.RegisterUnit(
                UnitId);

            experimentalAI.ConfigureUnit(
                UnitId,
                learningRate: 0.15f,
                autonomous: true);

            experimentalAI.SetState(
                UnitId,
                ExperimentalAIState.Initializing);

            abilities.RegisterAbility(
                "ECHO-SIGNAL-ANALYSIS",
                UnitId,
                ExperimentalAbilityType.AdaptiveProcessing);

            abilities.ConfigureAbility(
                "ECHO-SIGNAL-ANALYSIS",
                power: 0.75f,
                cooldown: 8f);

            risks.RegisterUnit(
                UnitId);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.AIDeviation,
                0.15f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.SignalExposure,
                0.10f);

            progression.RegisterUnit(
                UnitId);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.Research);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.Stability);

            progression.AddRequirement(
                UnitId,
                ExperimentalProgressionRequirement.FieldTesting);
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

        public bool ActivateSignalAnalysis()
        {
            if (!abilities.ActivateAbility(
                    "ECHO-SIGNAL-ANALYSIS"))
            {
                return false;
            }

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.Overload,
                0.05f);

            experimentalAI.Learn(
                UnitId,
                0.05f);

            return true;
        }

        public void DeactivateSignalAnalysis()
        {
            abilities.DeactivateAbility(
                "ECHO-SIGNAL-ANALYSIS");
        }

        public void Update(
            float deltaTime)
        {
            abilities.Update(
                deltaTime);

            electronicDisruption.Update(
                deltaTime);
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
