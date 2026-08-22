using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitDeploymentProfile
    {
        public string UnitId { get; }

        public int DeploymentPoints { get; private set; }

        public float MinimumDeploymentDistance { get; private set; }
        public float MaximumDeploymentDistance { get; private set; }

        public bool CanDeploy { get; private set; }
        public bool RequiresCommandLink { get; private set; }
        public bool RequiresDeploymentZone { get; private set; }

        public UnitDeploymentProfile(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            DeploymentPoints = 0;
            MinimumDeploymentDistance = 0f;
            MaximumDeploymentDistance = 0f;

            CanDeploy = true;
            RequiresCommandLink = true;
            RequiresDeploymentZone = true;
        }

        public void Configure(
            int deploymentPoints,
            float minimumDeploymentDistance,
            float maximumDeploymentDistance,
            bool canDeploy,
            bool requiresCommandLink,
            bool requiresDeploymentZone)
        {
            DeploymentPoints =
                Math.Max(0, deploymentPoints);

            MinimumDeploymentDistance =
                Math.Max(0f, minimumDeploymentDistance);

            MaximumDeploymentDistance =
                Math.Max(
                    MinimumDeploymentDistance,
                    maximumDeploymentDistance);

            CanDeploy = canDeploy;
            RequiresCommandLink = requiresCommandLink;
            RequiresDeploymentZone = requiresDeploymentZone;
        }
    }

    public sealed class UnitDeploymentProfileSystem
    {
        private readonly Dictionary<string, UnitDeploymentProfile> profiles =
            new Dictionary<string, UnitDeploymentProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new UnitDeploymentProfile(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            int deploymentPoints,
            float minimumDeploymentDistance,
            float maximumDeploymentDistance,
            bool canDeploy,
            bool requiresCommandLink,
            bool requiresDeploymentZone)
        {
            RegisterUnit(unitId);

            profiles[unitId].Configure(
                deploymentPoints,
                minimumDeploymentDistance,
                maximumDeploymentDistance,
                canDeploy,
                requiresCommandLink,
                requiresDeploymentZone);
        }

        public bool TryGetProfile(
            string unitId,
            out UnitDeploymentProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public bool CanDeployUnit(
            string unitId,
            int availableDeploymentPoints)
        {
            return profiles.TryGetValue(
                       unitId,
                       out UnitDeploymentProfile profile) &&
                   profile.CanDeploy &&
                   profile.DeploymentPoints <=
                   Math.Max(0, availableDeploymentPoints);
        }

        public void RemoveUnit(string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}
