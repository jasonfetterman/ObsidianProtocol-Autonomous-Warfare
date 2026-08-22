using System;

namespace ObsidianProtocol.Game.Deployment
{
    public enum DeploymentState
    {
        Unavailable,
        Available,
        Staged,
        Deployed,
        Withdrawn
    }

    public sealed class DeploymentEntry
    {
        public string DeploymentId { get; }
        public string UnitId { get; }
        public int DeploymentPoints { get; }
        public DeploymentState State { get; private set; }

        public DeploymentEntry(
            string deploymentId,
            string unitId,
            int deploymentPoints)
        {
            DeploymentId = deploymentId ?? string.Empty;
            UnitId = unitId ?? string.Empty;
            DeploymentPoints = Math.Max(0, deploymentPoints);
            State = DeploymentState.Unavailable;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(DeploymentId) &&
            !string.IsNullOrWhiteSpace(UnitId) &&
            DeploymentPoints > 0;

        public void SetAvailable()
        {
            if (State == DeploymentState.Unavailable)
                State = DeploymentState.Available;
        }

        public void Stage()
        {
            if (State == DeploymentState.Available)
                State = DeploymentState.Staged;
        }

        public void Deploy()
        {
            if (State == DeploymentState.Staged)
                State = DeploymentState.Deployed;
        }

        public void Withdraw()
        {
            if (State == DeploymentState.Deployed)
                State = DeploymentState.Withdrawn;
        }
    }
}
