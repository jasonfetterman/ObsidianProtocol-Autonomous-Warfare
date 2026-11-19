using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class DeploymentRoster
    {
        private readonly List<DeploymentEntry> entries =
            new List<DeploymentEntry>();

        public IReadOnlyList<DeploymentEntry> Entries => entries;

        public bool TryAdd(DeploymentBudget budget, DeploymentEntry entry)
        {
            if (budget == null || entry == null ||
                string.IsNullOrEmpty(entry.UnitId) ||
                entry.DeploymentPoints <= 0)
                return false;

            if (!budget.TrySpend(entry.DeploymentPoints))
                return false;

            entries.Add(entry);
            return true;
        }

        public bool TryAdd(DeploymentEntry entry)
        {
            if (entry == null ||
                string.IsNullOrEmpty(entry.UnitId) ||
                entry.DeploymentPoints <= 0)
                return false;

            entries.Add(entry);
            return true;
        }

        public bool Remove(DeploymentBudget budget, DeploymentEntry entry)
        {
            if (entry == null || !entries.Remove(entry))
                return false;

            if (budget != null)
                budget.Refund(entry.DeploymentPoints);

            return true;
        }

        public bool Remove(DeploymentEntry entry)
        {
            return entry != null && entries.Remove(entry);
        }

        public void Clear(DeploymentBudget budget)
        {
            if (budget != null)
            {
                foreach (DeploymentEntry entry in entries)
                    budget.Refund(entry.DeploymentPoints);
            }

            entries.Clear();
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
