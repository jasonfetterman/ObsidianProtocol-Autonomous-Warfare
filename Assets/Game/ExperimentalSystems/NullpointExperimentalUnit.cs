using System;

namespace ObsidianProtocol.Game.ExperimentalSystems.Units
{
    public sealed class NullpointExperimentalUnit
    {
        public const string UnitId = "NULLPOINT";
        public const string UnitName = "Nullpoint";

        private readonly ExperimentalFramework framework;
        private readonly ExperimentalUnitRestrictionsSystem restrictions;
        private readonly ElectronicDisruptionSystem disruption;
        private readonly CovertNetworkingSystem covertNetworking;
        private readonly ExperimentalAISystem experimentalAI;
        private readonly ExperimentalAbilitySystem abilities;
        private readonly ExperimentalRiskSystem risks;
        private readonly ExperimentalProgressionSystem progression;

        public NullpointExperimentalUnit(
            ExperimentalFramework framework,
            ExperimentalUnitRestrictionsSystem restrictions,
            ElectronicDisruptionSystem disruption,
            CovertNetworkingSystem covertNetworking,
            ExperimentalAISystem experimentalAI,
            ExperimentalAbilitySystem abilities,
            ExperimentalRiskSystem risks,
            ExperimentalProgressionSystem progression)
        {
            this.framework = framework;
            this.restrictions = restrictions;
            this.disruption = disruption;
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
                ExperimentalUnitType.Nullpoint);

            framework.ConfigureUnit(
                UnitId,
                authorized: false,
                autonomous: true);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.ElectronicDisruption);

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
                0.70f);

            covertNetworking.RegisterNode(
                UnitId,
                "NULLPOINT-COVERT");

            covertNetworking.SetPerformance(
                UnitId,
                concealment: 0.85f,
                reliability: 0.75f);

            experimentalAI.RegisterUnit(UnitId);

            experimentalAI.ConfigureUnit(
                UnitId,
                learningRate: 0.10f,
                autonomous: true);

            experimentalAI.SetState(
                UnitId,
                ExperimentalAIState.Initializing);

            abilities.RegisterAbility(
                "NULLPOINT-EMP",
                UnitId,
                ExperimentalAbilityType.ElectronicPulse);

            abilities.ConfigureAbility(
                "NULLPOINT-EMP",
                power: 0.90f,
                cooldown: 12f);

            risks.RegisterUnit(UnitId);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.Overload,
                0.20f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.SignalExposure,
                0.15f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.NetworkCompromise,
                0.10f);

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

        public bool ActivateElectronicPulse(
            string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId))
            {
                return false;
            }

            if (!abilities.ActivateAbility(
                    "NULLPOINT-EMP"))
            {
                return false;
            }

            disruption.ApplyDisruption(
                "NULLPOINT-" + targetId,
                targetId,
                DisruptionType.Network,
                strength: 0.90f,
                duration: 8f);

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.Overload,
                0.08f);

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.SignalExposure,
                0.04f);

            return true;
        }

        public void DeactivateElectronicPulse()
        {
            abilities.DeactivateAbility(
                "NULLPOINT-EMP");
        }

        public bool CanTransmitCovertly()
        {
            return covertNetworking.CanTransmit(
                UnitId);
        }

        public void Update(
            float deltaTime)
        {
            abilities.Update(
                deltaTime);

            disruption.Update(
                deltaTime);
        }

        public float GetNetworkDisruption(
            string targetId)
        {
            return disruption.GetDisruptionStrength(
                targetId,
                DisruptionType.Network);
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
