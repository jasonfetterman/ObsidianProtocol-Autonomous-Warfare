using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class DeploymentBudget
    {
        public int maxPoints = 10000;
        public int usedPoints = 0;

        public int AvailablePoints
        {
            get { return Mathf.Max(0, maxPoints - usedPoints); }
        }

        public bool CanDeploy(int cost)
        {
            return cost >= 0 && usedPoints + cost <= maxPoints;
        }

        public bool TrySpend(int cost)
        {
            if (!CanDeploy(cost))
                return false;

            usedPoints += cost;
            return true;
        }

        public void Refund(int cost)
        {
            usedPoints = Mathf.Max(0, usedPoints - Mathf.Max(0, cost));
        }

        public void ResetBudget()
        {
            usedPoints = 0;
        }
    }
}
