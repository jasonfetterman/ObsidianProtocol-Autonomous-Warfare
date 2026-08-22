using System;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class MultiplayerDeploymentRules
    {
        public int BattleBudget { get; private set; }
        public int MaximumDeploymentPoints { get; private set; }
        public int MaximumReinforcementPoints { get; private set; }

        public bool RequireDeploymentZone { get; private set; }
        public bool EnforceDeploymentCosts { get; private set; }
        public bool AllowReinforcements { get; private set; }

        public MultiplayerDeploymentRules(
            int battleBudget,
            int maximumDeploymentPoints,
            int maximumReinforcementPoints)
        {
            BattleBudget =
                Math.Max(0, battleBudget);

            MaximumDeploymentPoints =
                Math.Max(0, maximumDeploymentPoints);

            MaximumReinforcementPoints =
                Math.Max(0, maximumReinforcementPoints);

            RequireDeploymentZone = true;
            EnforceDeploymentCosts = true;
            AllowReinforcements = true;
        }

        public void SetBattleBudget(int budget)
        {
            BattleBudget = Math.Max(0, budget);
        }

        public void SetMaximumDeploymentPoints(int points)
        {
            MaximumDeploymentPoints =
                Math.Max(0, points);
        }

        public void SetMaximumReinforcementPoints(int points)
        {
            MaximumReinforcementPoints =
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
