using System;

namespace ObsidianProtocol.Game.ExperimentalSystems.Units
{
    public sealed class SpecterExperimentalUnit
    {
        public const string UnitId = "SPECTER";
        public const string UnitName = "Specter";

        private readonly ExperimentalFramework framework;
        private readonly ExperimentalUnitRestrictionsSystem restrictions;
        private readonly StealthSystem stealth;
        private readonly CovertNetworkingSystem covertNetworking;
        private readonly ExperimentalAISystem experimentalAI;
        private readonly ExperimentalAbilitySystem abilities;
        private readonly ExperimentalRiskSystem risks;
        private readonly ExperimentalProgressionSystem progression;

        public SpecterExperimentalUnit(
            ExperimentalFramework framework,
            ExperimentalUnitRestrictionsSystem restrictions,
            StealthSystem stealth,
            CovertNetworkingSystem covertNetworking,
            ExperimentalAISystem experimentalAI,
            ExperimentalAbilitySystem abilities,
            ExperimentalRiskSystem risks,
            ExperimentalProgressionSystem progression)
        {
            this.framework = framework;
            this.restrictions = restrictions;
            this.stealth = stealth;
            this.covertNetworking = covertNetworking;
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
                ExperimentalUnitType.Specter);

            framework.ConfigureUnit(
                UnitId,
                authorized: false,
                autonomous: true);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.Stealth);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.CovertNetworking);

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
                0.75f);

            stealth.RegisterUnit(UnitId);

            stealth.SetSignature(
                UnitId,
                StealthSignatureType.Visual,
                0.20f);

            stealth.SetSignature(
                UnitId,
                StealthSignatureType.Thermal,
                0.25f);

            stealth.SetSignature(
                UnitId,
                StealthSignatureType.Radar,
                0.15f);

            stealth.SetSignature(
                UnitId,
                StealthSignatureType.Audio,
                0.20f);

            stealth.SetSignature(
                UnitId,
                StealthSignatureType.Electronic,
                0.10f);

            covertNetworking.RegisterNode(
                UnitId,
                "SPECTER-COVERT");

            covertNetworking.SetPerformance(
                UnitId,
                concealment: 0.95f,
                reliability: 0.70f);

            experimentalAI.RegisterUnit(UnitId);

            experimentalAI.ConfigureUnit(
                UnitId,
                learningRate: 0.12f,
                autonomous: true);

            experimentalAI.SetState(
                UnitId,
                ExperimentalAIState.Initializing);

            abilities.RegisterAbility(
                "SPECTER-SIGNATURE-SUPPRESSION",
                UnitId,
                ExperimentalAbilityType.SignatureSuppression);

            abilities.ConfigureAbility(
                "SPECTER-SIGNATURE-SUPPRESSION",
                power: 0.90f,
                cooldown: 15f);

            risks.RegisterUnit(UnitId);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.SignalExposure,
                0.10f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.AIDeviation,
                0.12f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.NetworkCompromise,
                0.08f);

            progression.RegisterUnit(UnitId);

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

            covertNetworking.SetState(
                UnitId,
                CovertNetworkState.Active);

            return true;
        }

        public bool ActivateStealth()
        {
            if (!abilities.ActivateAbility(
                    "SPECTER-SIGNATURE-SUPPRESSION"))
            {
                return false;
            }

            stealth.SetEnabled(
                UnitId,
                true);

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.Overload,
                0.04f);

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.SignalExposure,
                0.03f);

            return true;
        }

        public void DeactivateStealth()
        {
            abilities.DeactivateAbility(
                "SPECTER-SIGNATURE-SUPPRESSION");

            stealth.SetEnabled(
                UnitId,
                false);
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

        public bool CanTransmitCovertly()
        {
            return covertNetworking.CanTransmit(
                UnitId);
        }

        public float GetOverallSignature()
        {
            return stealth.GetOverallSignature(
                UnitId);
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

        public void Update(
            float deltaTime)
        {
            abilities.Update(
                deltaTime);
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
