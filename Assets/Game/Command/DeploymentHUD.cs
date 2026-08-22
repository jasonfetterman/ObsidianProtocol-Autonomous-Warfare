using System;

namespace ObsidianProtocol.Game.Command
{
    public sealed class DeploymentHUD
    {
        public bool Visible { get; private set; }

        public int BattleBudget { get; private set; }
        public int UsedBattleBudget { get; private set; }
        public int RemainingBattleBudget =>
            Math.Max(0, BattleBudget - UsedBattleBudget);

        public int DeploymentPoints { get; private set; }
        public int UsedDeploymentPoints { get; private set; }
        public int RemainingDeploymentPoints =>
            Math.Max(
                0,
                DeploymentPoints - UsedDeploymentPoints);

        public int ReinforcementPoints { get; private set; }
        public int UsedReinforcementPoints { get; private set; }
        public int RemainingReinforcementPoints =>
            Math.Max(
                0,
                ReinforcementPoints - UsedReinforcementPoints);

        public string StatusMessage { get; private set; }

        public DeploymentHUD()
        {
            StatusMessage = string.Empty;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public void SetBattleBudget(
            int total,
            int used)
        {
            BattleBudget = Math.Max(0, total);
            UsedBattleBudget = Math.Max(
                0,
                Math.Min(used, BattleBudget));
        }

        public void SetDeploymentPoints(
            int total,
            int used)
        {
            DeploymentPoints = Math.Max(0, total);
            UsedDeploymentPoints = Math.Max(
                0,
                Math.Min(used, DeploymentPoints));
        }

        public void SetReinforcementPoints(
            int total,
            int used)
        {
            ReinforcementPoints = Math.Max(0, total);
            UsedReinforcementPoints = Math.Max(
                0,
                Math.Min(
                    used,
                    ReinforcementPoints));
        }

        public void SetStatus(string message)
        {
            StatusMessage =
                message ?? string.Empty;
        }

        public void Reset()
        {
            Visible = false;

            BattleBudget = 0;
            UsedBattleBudget = 0;

            DeploymentPoints = 0;
            UsedDeploymentPoints = 0;

            ReinforcementPoints = 0;
            UsedReinforcementPoints = 0;

            StatusMessage = string.Empty;
        }
    }
}
