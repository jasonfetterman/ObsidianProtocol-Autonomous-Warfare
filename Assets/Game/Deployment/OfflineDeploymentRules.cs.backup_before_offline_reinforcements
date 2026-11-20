using System;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class OfflineDeploymentRules
    {
        public int DeploymentBudget { get; private set; }
        public int MaximumDeploymentPoints { get; private set; }

        public bool RequireDeploymentZone { get; private set; }
        public bool EnforceDeploymentCosts { get; private set; }
        public bool AllowReinforcements { get; private set; }

        public OfflineDeploymentRules(
            int deploymentBudget,
            int maximumDeploymentPoints)
        {
            DeploymentBudget =
                Math.Max(0, deploymentBudget);

            MaximumDeploymentPoints =
                Math.Max(0, maximumDeploymentPoints);

            RequireDeploymentZone = true;
            EnforceDeploymentCosts = true;
            AllowReinforcements = true;
        }

        public void SetDeploymentBudget(int budget)
        {
            DeploymentBudget =
                Math.Max(0, budget);
        }

        public void SetMaximumDeploymentPoints(int points)
        {
            MaximumDeploymentPoints =
                Math.Max(0, points);
        }

        public void SetRequireDeploymentZone(bool required)
        {
            RequireDeploymentZone = required;
        }

        public void SetEnforceDeploymentCosts(bool enforce)
        {
            EnforceDeploymentCosts = enforce;
        }

        public void SetAllowReinforcements(bool allow)
        {
            AllowReinforcements = allow;
        }
    }
}
