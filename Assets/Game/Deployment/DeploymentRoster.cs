using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class DeploymentRoster
    {
        private readonly List<DeploymentEntry> entries = new();

        public IReadOnlyList<DeploymentEntry> Entries => entries;

        public bool TryAdd(DeploymentBudget budget, DeploymentEntry entry)
        {
            if (budget == null || string.IsNullOrEmpty(entry.UnitId))
            {
                return false;
            }

            if (!budget.TrySpend(entry.DeploymentPoints))
            {
                return false;
            }

            entries.Add(entry);
            return true;
        }

        public bool Remove(DeploymentBudget budget, DeploymentEntry entry)
        {
            if (budget == null || !entries.Remove(entry))
            {
                return false;
            }

            budget.Refund(entry.DeploymentPoints);
            return true;
        }

        public void Clear(DeploymentBudget budget)
        {
            if (budget != null)
            {
                foreach (DeploymentEntry entry in entries)
                {
                    budget.Refund(entry.DeploymentPoints);
                }
            }

            entries.Clear();
        }
    }
}
