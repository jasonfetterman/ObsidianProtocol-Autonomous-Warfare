using System;

namespace ObsidianProtocol.Game.Store
{
    public sealed class DeploymentBudgetSeparation
    {
        public bool OwnershipAffectsDeploymentBudget =>
            false;

        public bool PurchaseAffectsDeploymentBudget =>
            false;

        public bool CreditsAffectDeploymentBudget =>
            false;

        public bool StoreCanIncreaseBattleBudget =>
            false;

        public bool StoreCanIncreaseDeploymentPoints =>
            false;

        public bool ValidateOwnershipForDeployment(
            string playerId,
            string ownershipId)
        {
            return
                !string.IsNullOrWhiteSpace(playerId) &&
                !string.IsNullOrWhiteSpace(ownershipId);
        }

        public bool ValidateDeploymentCost(
            int deploymentPoints,
            int availableDeploymentPoints)
        {
            if (deploymentPoints <= 0)
                return false;

            if (availableDeploymentPoints < 0)
                return false;

            return deploymentPoints <=
                   availableDeploymentPoints;
        }

        public bool ValidateBattleBudget(
            int battleBudget,
            int requestedDeploymentPoints)
        {
            if (battleBudget < 0 ||
                requestedDeploymentPoints < 0)
            {
                return false;
            }

            return requestedDeploymentPoints <=
                   battleBudget;
        }

        public bool CanStorePurchaseBypassDeployment(
            bool purchased,
            int deploymentPoints,
            int availableDeploymentPoints)
        {
            if (!purchased)
                return false;

            return ValidateDeploymentCost(
                deploymentPoints,
                availableDeploymentPoints);
        }
    }
}
