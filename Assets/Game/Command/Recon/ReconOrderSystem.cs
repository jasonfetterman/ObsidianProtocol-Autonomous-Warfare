using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Recon
{
    public sealed class ReconOrder
    {
        public int CommandId;
        public Vector3 ObjectivePosition;
        public float SearchRadius = 50f;
        public bool MaintainStealth = false;
        public bool ReportFindings = true;

        public ReconOrder(
            int commandId,
            Vector3 objectivePosition)
        {
            CommandId = commandId;
            ObjectivePosition = objectivePosition;
        }
    }

    public sealed class ReconOrderSystem
    {
        private readonly Dictionary<int, ReconOrder> orders =
            new Dictionary<int, ReconOrder>();

        public void IssueReconOrder(
            int commandId,
            Vector3 objectivePosition)
        {
            orders[commandId] =
                new ReconOrder(
                    commandId,
                    objectivePosition);
        }

        public bool TryGetOrder(
            int commandId,
            out ReconOrder order)
        {
            return orders.TryGetValue(
                commandId,
                out order);
        }

        public void RemoveOrder(int commandId)
        {
            orders.Remove(commandId);
        }

        public void Clear()
        {
            orders.Clear();
        }
    }
}
