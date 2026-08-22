using System;

namespace ObsidianProtocol.Game.ExperimentalSystems.Units
{
    public sealed class ShadowgridExperimentalUnit
    {
        public const string UnitId = "SHADOWGRID";
        public const string UnitName = "Shadowgrid";

        private readonly ExperimentalFramework framework;
        private readonly ExperimentalUnitRestrictionsSystem restrictions;
        private readonly SignalIntelligenceSystem signalIntelligence;
        private readonly CovertNetworkingSystem covertNetworking;
        private readonly ExperimentalAISystem experimentalAI;
        private readonly ExperimentalAbilitySystem abilities;
        private readonly ExperimentalRiskSystem risks;
        private readonly ExperimentalProgressionSystem progression;

        public ShadowgridExperimentalUnit(
            ExperimentalFramework framework,
            ExperimentalUnitRestrictionsSystem restrictions,
            SignalIntelligenceSystem signalIntelligence,
            CovertNetworkingSystem covertNetworking,
            ExperimentalAISystem experimentalAI,
            ExperimentalAbilitySystem abilities,
            ExperimentalRiskSystem risks,
            ExperimentalProgressionSystem progression)
        {
            this.framework = framework;
            this.restrictions = restrictions;
            this.signalIntelligence = signalIntelligence;
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
                ExperimentalUnitType.Shadowgrid);

            framework.ConfigureUnit(
                UnitId,
                authorized: false,
                autonomous: true);

            framework.AddCapability(
                UnitId,
                ExperimentalCapability.SignalIntelligence);

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
                "SHADOWGRID");

            covertNetworking.SetPerformance(
                UnitId,
                concealment: 0.90f,
                reliability: 0.92f);

            signalIntelligence.DetectSignal(
                "SHADOWGRID-NETWORK",
                UnitId,
                SignalType.Communication,
                0f,
                0f);

            experimentalAI.RegisterUnit(UnitId);

            experimentalAI.ConfigureUnit(
                UnitId,
                learningRate: 0.18f,
                autonomous: true);

            experimentalAI.SetState(
                UnitId,
                ExperimentalAIState.Initializing);

            abilities.RegisterAbility(
                "SHADOWGRID-RELAY",
                UnitId,
                ExperimentalAbilityType.CovertRelay);

            abilities.ConfigureAbility(
                "SHADOWGRID-RELAY",
                power: 0.85f,
                cooldown: 10f);

            risks.RegisterUnit(UnitId);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.NetworkCompromise,
                0.20f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.SignalExposure,
                0.12f);

            risks.SetRisk(
                UnitId,
                ExperimentalRiskType.AIDeviation,
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

        public bool ActivateRelay()
        {
            if (!abilities.ActivateAbility(
                    "SHADOWGRID-RELAY"))
            {
                return false;
            }

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.NetworkCompromise,
                0.05f);

            risks.AddRisk(
                UnitId,
                ExperimentalRiskType.SignalExposure,
                0.03f);

            return true;
        }

        public void DeactivateRelay()
        {
            abilities.DeactivateAbility(
                "SHADOWGRID-RELAY");
        }

        public bool CanTransmitCovertly()
        {
            return covertNetworking.CanTransmit(
                UnitId);
        }

        public bool SendCovertMessage(
            string targetNodeId,
            string payload)
        {
            if (!abilities.IsActive(
                    "SHADOWGRID-RELAY"))
            {
                return false;
            }

            return covertNetworking.Send(
                "SHADOWGRID-MESSAGE-" +
                Guid.NewGuid().ToString("N"),
                UnitId,
                targetNodeId,
                payload);
        }

        public void Update(
            float deltaTime)
        {
            abilities.Update(
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
