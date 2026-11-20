using System;

namespace ObsidianProtocol.Game.Deployment
{
    public enum DeploymentValidationFailure
    {
        None,
        InvalidEntry,
        InvalidZone,
        ZoneUnavailable,
        UnitNotAllowed,
        InsufficientBudget,
        InsufficientDeploymentPoints
    }

    public sealed class DeploymentValidationResult
    {
        public bool Valid { get; }
        public DeploymentValidationFailure Failure { get; }

        public DeploymentValidationResult(
            bool valid,
            DeploymentValidationFailure failure)
        {
            Valid = valid;
            Failure = failure;
        }

        public static DeploymentValidationResult Success()
        {
            return new DeploymentValidationResult(
                true,
                DeploymentValidationFailure.None);
        }

        public static DeploymentValidationResult Failed(
            DeploymentValidationFailure failure)
        {
            return new DeploymentValidationResult(
                false,
                failure);
        }
    }

    public sealed class DeploymentValidator
    {
        private readonly DeploymentFramework deploymentFramework;
        private readonly DeploymentZoneSystem zoneSystem;
        private readonly UnitDeploymentCostRegistry costRegistry;

        public DeploymentValidator(
            DeploymentFramework deploymentFramework,
            DeploymentZoneSystem zoneSystem,
            UnitDeploymentCostRegistry costRegistry)
        {
            this.deploymentFramework =
                deploymentFramework;

            this.zoneSystem =
                zoneSystem;

            this.costRegistry =
                costRegistry;
        }

        public DeploymentValidationResult Validate(
            string deploymentId,
            string zoneId,
            string unitId,
            BattleBudget battleBudget,
            DeploymentPointPool pointPool)
        {
            if (deploymentFramework == null ||
                zoneSystem == null ||
                costRegistry == null)
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.InvalidEntry);
            }

            if (!deploymentFramework.TryGet(
                    deploymentId,
                    out DeploymentEntry entry) ||
                entry == null ||
                !entry.Valid)
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.InvalidEntry);
            }

            if (!zoneSystem.TryGetZone(
                    zoneId,
                    out DeploymentZone zone) ||
                zone == null)
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.InvalidZone);
            }

            if (!zone.Enabled)
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.ZoneUnavailable);
            }

            if (!zone.CanDeployUnit(unitId))
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.UnitNotAllowed);
            }

            if (!costRegistry.TryGetCost(
                    unitId,
                    out int deploymentCost))
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.InvalidEntry);
            }

            if (battleBudget == null ||
                !battleBudget.CanCommit(deploymentCost))
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.InsufficientBudget);
            }

            if (pointPool == null ||
                !pointPool.CanSpend(deploymentCost))
            {
                return DeploymentValidationResult.Failed(
                    DeploymentValidationFailure.InsufficientDeploymentPoints);
            }

            return DeploymentValidationResult.Success();
        }
    }
}
