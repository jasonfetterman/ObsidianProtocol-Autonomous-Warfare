using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public sealed class VerticalSliceDeploymentBudget
    {
        private readonly Dictionary<string, int> unitCosts =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int MaximumPoints { get; private set; }

        public int UsedPoints { get; private set; }

        public int AvailablePoints =>
            Math.Max(0, MaximumPoints - UsedPoints);

        public int DeployedUnitCount =>
            unitCosts.Count;

        public bool Initialize(int maximumPoints)
        {
            if (Initialized ||
                maximumPoints < 0)
            {
                return false;
            }

            unitCosts.Clear();

            MaximumPoints = maximumPoints;
            UsedPoints = 0;
            Initialized = true;

            return true;
        }

        public bool CanDeploy(
            string unitId,
            int deploymentCost)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId) ||
                deploymentCost < 0 ||
                unitCosts.ContainsKey(unitId.Trim()))
            {
                return false;
            }

            return UsedPoints + deploymentCost <=
                   MaximumPoints;
        }

        public bool DeployUnit(
            string unitId,
            int deploymentCost)
        {
            if (!CanDeploy(
                    unitId,
                    deploymentCost))
            {
                return false;
            }

            unitCosts.Add(
                unitId.Trim(),
                deploymentCost);

            UsedPoints += deploymentCost;

            return true;
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            string id = unitId.Trim();

            if (!unitCosts.TryGetValue(
                    id,
                    out int cost))
            {
                return false;
            }

            unitCosts.Remove(id);
            UsedPoints -= cost;

            return true;
        }

        public int GetUnitCost(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return 0;
            }

            unitCosts.TryGetValue(
                unitId.Trim(),
                out int cost);

            return cost;
        }

        public IReadOnlyDictionary<string, int>
            GetDeployments()
        {
            return unitCosts;
        }

        public void Reset()
        {
            unitCosts.Clear();

            MaximumPoints = 0;
            UsedPoints = 0;
            Initialized = false;
        }
    }
}
