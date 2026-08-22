namespace ObsidianProtocol.Game.Deployment
{
    public readonly struct DeploymentEntry
    {
        public string UnitId { get; }
        public int DeploymentPoints { get; }

        public DeploymentEntry(string unitId, int deploymentPoints)
        {
            UnitId = unitId;
            DeploymentPoints = deploymentPoints < 0 ? 0 : deploymentPoints;
        }
    }
}
