using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class DeploymentPointPool
    {
        public int MaximumPoints { get; private set; }
        public int UsedPoints { get; private set; }
        public int AvailablePoints =>
            Math.Max(0, MaximumPoints - UsedPoints);

        public DeploymentPointPool(int maximumPoints)
        {
            MaximumPoints = Math.Max(0, maximumPoints);
            UsedPoints = 0;
        }

        public bool CanSpend(int points)
        {
            if (points <= 0)
                return false;

            return points <= AvailablePoints;
        }

        public bool TrySpend(int points)
        {
            if (!CanSpend(points))
                return false;

            UsedPoints += points;
            return true;
        }

        public bool Refund(int points)
        {
            if (points <= 0)
                return false;

            if (points > UsedPoints)
                return false;

            UsedPoints -= points;
            return true;
        }

        public void SetMaximum(int maximumPoints)
        {
            MaximumPoints = Math.Max(0, maximumPoints);

            if (UsedPoints > MaximumPoints)
                UsedPoints = MaximumPoints;
        }

        public void Reset()
        {
            UsedPoints = 0;
        }
    }

    public sealed class DeploymentPointLedger
    {
        private readonly Dictionary<string, int> allocations =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

        public int TotalAllocated { get; private set; }

        public bool Allocate(
            string deploymentId,
            int points,
            DeploymentPointPool pool)
        {
            if (pool == null ||
                string.IsNullOrWhiteSpace(deploymentId) ||
                points <= 0 ||
                allocations.ContainsKey(deploymentId))
            {
                return false;
            }

            if (!pool.TrySpend(points))
                return false;

            allocations.Add(
                deploymentId,
                points);

            TotalAllocated += points;

            return true;
        }

        public bool Release(
            string deploymentId,
            DeploymentPointPool pool)
        {
            if (pool == null ||
                string.IsNullOrWhiteSpace(deploymentId))
            {
                return false;
            }

            if (!allocations.TryGetValue(
                    deploymentId,
                    out int points))
            {
                return false;
            }

            if (!pool.Refund(points))
                return false;

            allocations.Remove(deploymentId);
            TotalAllocated -= points;

            return true;
        }

        public bool TryGetAllocation(
            string deploymentId,
            out int points)
        {
            return allocations.TryGetValue(
                deploymentId,
                out points);
        }

        public IReadOnlyDictionary<string, int>
            GetAllocations()
        {
            return allocations;
        }

        public void Clear(DeploymentPointPool pool)
        {
            if (pool != null)
                pool.Refund(TotalAllocated);

            allocations.Clear();
            TotalAllocated = 0;
        }
    }
}
