using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class UnitDeploymentCost
    {
        public string UnitId { get; }
        public int DeploymentPoints { get; }

        public UnitDeploymentCost(
            string unitId,
            int deploymentPoints)
        {
            UnitId =
                unitId ?? string.Empty;

            DeploymentPoints =
                Math.Max(0, deploymentPoints);
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(UnitId) &&
            DeploymentPoints > 0;
    }

    public sealed class UnitDeploymentCostRegistry
    {
        private readonly Dictionary<string, UnitDeploymentCost> costs =
            new Dictionary<string, UnitDeploymentCost>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UnitDeploymentCost cost)
        {
            if (cost == null ||
                !cost.Valid ||
                costs.ContainsKey(cost.UnitId))
            {
                return false;
            }

            costs.Add(
                cost.UnitId,
                cost);

            return true;
        }

        public bool Remove(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return costs.Remove(unitId);
        }

        public bool TryGet(
            string unitId,
            out UnitDeploymentCost cost)
        {
            return costs.TryGetValue(
                unitId,
                out cost);
        }

        public bool TryGetCost(
            string unitId,
            out int deploymentPoints)
        {
            deploymentPoints = 0;

            if (!costs.TryGetValue(
                    unitId,
                    out UnitDeploymentCost cost))
            {
                return false;
            }

            deploymentPoints =
                cost.DeploymentPoints;

            return true;
        }

        public IReadOnlyCollection<UnitDeploymentCost>
            GetCosts()
        {
            return costs.Values;
        }

        public void Clear()
        {
            costs.Clear();
        }
    }
}
