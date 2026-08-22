using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public enum DeploymentRestriction
    {
        None,
        ZoneRestricted,
        UnitRestricted,
        BudgetRestricted,
        MultiplayerRestricted,
        OfflineRestricted
    }

    public sealed class DeploymentRestrictionSet
    {
        private readonly HashSet<DeploymentRestriction> restrictions =
            new HashSet<DeploymentRestriction>();

        public bool Add(DeploymentRestriction restriction)
        {
            if (restriction == DeploymentRestriction.None)
                return false;

            return restrictions.Add(restriction);
        }

        public bool Remove(DeploymentRestriction restriction)
        {
            return restrictions.Remove(restriction);
        }

        public bool Contains(DeploymentRestriction restriction)
        {
            return restrictions.Contains(restriction);
        }

        public bool IsRestricted =>
            restrictions.Count > 0;

        public IReadOnlyCollection<DeploymentRestriction>
            GetRestrictions()
        {
            return restrictions;
        }

        public void Clear()
        {
            restrictions.Clear();
        }
    }

    public sealed class DeploymentRestrictionRegistry
    {
        private readonly Dictionary<string, DeploymentRestrictionSet> entries =
            new Dictionary<string, DeploymentRestrictionSet>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            string deploymentId,
            DeploymentRestrictionSet restrictionSet)
        {
            if (string.IsNullOrWhiteSpace(deploymentId) ||
                restrictionSet == null ||
                entries.ContainsKey(deploymentId))
            {
                return false;
            }

            entries.Add(
                deploymentId,
                restrictionSet);

            return true;
        }

        public bool Remove(string deploymentId)
        {
            if (string.IsNullOrWhiteSpace(deploymentId))
                return false;

            return entries.Remove(deploymentId);
        }

        public bool TryGet(
            string deploymentId,
            out DeploymentRestrictionSet restrictionSet)
        {
            return entries.TryGetValue(
                deploymentId,
                out restrictionSet);
        }

        public bool IsRestricted(string deploymentId)
        {
            return entries.TryGetValue(
                       deploymentId,
                       out DeploymentRestrictionSet restrictionSet) &&
                   restrictionSet.IsRestricted;
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
