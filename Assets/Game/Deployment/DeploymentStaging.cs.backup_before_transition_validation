using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public enum DeploymentStagingState
    {
        Unstaged,
        Staged,
        Ready,
        Deployed,
        Cancelled
    }

    public sealed class DeploymentStagingEntry
    {
        public string DeploymentId { get; }
        public string ZoneId { get; }
        public DeploymentStagingState State { get; private set; }

        public DeploymentStagingEntry(
            string deploymentId,
            string zoneId)
        {
            DeploymentId =
                deploymentId ?? string.Empty;

            ZoneId =
                zoneId ?? string.Empty;

            State =
                DeploymentStagingState.Unstaged;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(DeploymentId) &&
            !string.IsNullOrWhiteSpace(ZoneId);

        public void Stage()
        {
            if (State == DeploymentStagingState.Unstaged)
                State = DeploymentStagingState.Staged;
        }

        public void MarkReady()
        {
            if (State == DeploymentStagingState.Staged)
                State = DeploymentStagingState.Ready;
        }

        public void Deploy()
        {
            if (State == DeploymentStagingState.Ready)
                State = DeploymentStagingState.Deployed;
        }

        public void Cancel()
        {
            if (State != DeploymentStagingState.Deployed)
                State = DeploymentStagingState.Cancelled;
        }

        public void Reset()
        {
            if (State != DeploymentStagingState.Deployed)
                State = DeploymentStagingState.Unstaged;
        }
    }

    public sealed class DeploymentStagingSystem
    {
        private readonly Dictionary<string, DeploymentStagingEntry> entries =
            new Dictionary<string, DeploymentStagingEntry>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            DeploymentStagingEntry entry)
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

        public bool Remove(string deploymentId)
        {
            if (string.IsNullOrWhiteSpace(deploymentId))
                return false;

            return entries.Remove(deploymentId);
        }

        public bool TryGet(
            string deploymentId,
            out DeploymentStagingEntry entry)
        {
            return entries.TryGetValue(
                deploymentId,
                out entry);
        }

        public bool Stage(string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentStagingEntry entry))
            {
                return false;
            }

            entry.Stage();
            return true;
        }

        public bool MarkReady(string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentStagingEntry entry))
            {
                return false;
            }

            entry.MarkReady();
            return true;
        }

        public bool Deploy(string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentStagingEntry entry))
            {
                return false;
            }

            entry.Deploy();
            return true;
        }

        public bool Cancel(string deploymentId)
        {
            if (!entries.TryGetValue(
                    deploymentId,
                    out DeploymentStagingEntry entry))
            {
                return false;
            }

            entry.Cancel();
            return true;
        }

        public IReadOnlyCollection<DeploymentStagingEntry>
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
