using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum ExperimentalRestriction
    {
        AuthorizationRequired,
        ResearchRequired,
        StabilityRequired,
        DeploymentLimit,
        ModeRestricted,
        FactionRestricted,
        MapRestricted,
        MultiplayerRestricted
    }

    public sealed class ExperimentalRestrictionProfile
    {
        private readonly HashSet<ExperimentalRestriction> restrictions =
            new HashSet<ExperimentalRestriction>();

        public string UnitId { get; }

        public int MaximumDeploymentCount { get; private set; }

        public float MinimumStability { get; private set; }

        public ExperimentalRestrictionProfile(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            MaximumDeploymentCount = 1;
            MinimumStability = 0.5f;
        }

        public void AddRestriction(
            ExperimentalRestriction restriction)
        {
            restrictions.Add(
                restriction);
        }

        public bool HasRestriction(
            ExperimentalRestriction restriction)
        {
            return restrictions.Contains(
                restriction);
        }

        public IReadOnlyCollection<ExperimentalRestriction>
            GetRestrictions()
        {
            return restrictions;
        }

        public void SetDeploymentLimit(
            int maximumDeploymentCount)
        {
            MaximumDeploymentCount =
                Math.Max(
                    1,
                    maximumDeploymentCount);
        }

        public void SetMinimumStability(
            float minimumStability)
        {
            MinimumStability =
                Math.Clamp(
                    minimumStability,
                    0f,
                    1f);
        }

        public bool CanDeploy(
            bool authorized,
            bool researchComplete,
            float stability,
            int currentlyDeployed)
        {
            if (HasRestriction(
                    ExperimentalRestriction.AuthorizationRequired) &&
                !authorized)
            {
                return false;
            }

            if (HasRestriction(
                    ExperimentalRestriction.ResearchRequired) &&
                !researchComplete)
            {
                return false;
            }

            if (HasRestriction(
                    ExperimentalRestriction.StabilityRequired) &&
                stability < MinimumStability)
            {
                return false;
            }

            if (HasRestriction(
                    ExperimentalRestriction.DeploymentLimit) &&
                currentlyDeployed >= MaximumDeploymentCount)
            {
                return false;
            }

            return true;
        }
    }

    public sealed class ExperimentalUnitRestrictionsSystem
    {
        private readonly Dictionary<string, ExperimentalRestrictionProfile>
            profiles =
                new Dictionary<string, ExperimentalRestrictionProfile>(
                    StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new ExperimentalRestrictionProfile(
                        unitId));
            }
        }

        public void AddRestriction(
            string unitId,
            ExperimentalRestriction restriction)
        {
            RegisterUnit(unitId);

            profiles[unitId].AddRestriction(
                restriction);
        }

        public void SetDeploymentLimit(
            string unitId,
            int maximumDeploymentCount)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetDeploymentLimit(
                maximumDeploymentCount);
        }

        public void SetMinimumStability(
            string unitId,
            float minimumStability)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetMinimumStability(
                minimumStability);
        }

        public bool CanDeploy(
            string unitId,
            bool authorized,
            bool researchComplete,
            float stability,
            int currentlyDeployed)
        {
            return profiles.TryGetValue(
                       unitId,
                       out ExperimentalRestrictionProfile profile) &&
                   profile.CanDeploy(
                       authorized,
                       researchComplete,
                       stability,
                       currentlyDeployed);
        }

        public bool TryGetProfile(
            string unitId,
            out ExperimentalRestrictionProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public void RemoveUnit(
            string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}
