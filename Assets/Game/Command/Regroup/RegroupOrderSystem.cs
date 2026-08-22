using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Regroup
{
    public sealed class RegroupOrder
    {
        public int CommandId;
        public Vector3 RallyPosition;
        public float RallyRadius = 15f;
        public bool RestoreFormation = true;
        public bool ResumePreviousIntent = true;

        public RegroupOrder(
            int commandId,
            Vector3 rallyPosition)
        {
            CommandId = commandId;
            RallyPosition = rallyPosition;
        }
    }

    public sealed class RegroupOrderSystem
    {
        private readonly Dictionary<int, RegroupOrder> orders =
            new Dictionary<int, RegroupOrder>();

        public void IssueRegroupOrder(
            int commandId,
            Vector3 rallyPosition)
        {
            orders[commandId] =
                new RegroupOrder(
                    commandId,
                    rallyPosition);
        }

        public bool TryGetOrder(
            int commandId,
            out RegroupOrder order)
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
