using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class DeploymentFramework
    {
        private readonly Dictionary<string, DeploymentEntry> entries =
            new Dictionary<string, DeploymentEntry>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            DeploymentEntry entry)
        {
            if (entry == null ||
                !entry.Valid ||
                entries.ContainsKey(entry.DeploymentId))
            {
                return false;
            }

            entries.Add(
                entry.DeploymentId,
                entry);

            return true;
        }

        public bool Remove(
            string deploymentId)
        {
            if (string.IsNullOrWhiteSpace(deploymentId))
            {
                return false;
            }

            return entries.Remove(deploymentId);
        }

        public bool TryGet(
            string deploymentId,
            out DeploymentEntry entry)
        {
            return entries.TryGetValue(
                deploymentId,
                out entry);
        }

        public bool SetAvailable(
            string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentEntry entry))
            {
                return false;
            }

            entry.SetAvailable();
            return true;
        }

        public bool Stage(
            string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentEntry entry))
            {
                return false;
            }

            entry.Stage();
            return true;
        }

        public bool Deploy(
            string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentEntry entry))
            {
                return false;
            }

            entry.Deploy();
            return true;
        }

        public bool Withdraw(
            string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentEntry entry))
            {
                return false;
            }

            entry.Withdraw();
            return true;
        }

        public IReadOnlyCollection<DeploymentEntry>
            GetEntries()
        {
            return entries.Values;
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
