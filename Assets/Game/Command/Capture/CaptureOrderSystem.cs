using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Capture
{
    public sealed class CaptureOrder
    {
        public int CommandId;
        public Vector3 ObjectivePosition;
        public float CaptureRadius = 10f;
        public float RequiredHoldTime = 5f;
        public bool SecureAfterCapture = true;

        public CaptureOrder(
            int commandId,
            Vector3 objectivePosition)
        {
            CommandId = commandId;
            ObjectivePosition = objectivePosition;
        }
    }

    public sealed class CaptureOrderSystem
    {
        private readonly Dictionary<int, CaptureOrder> orders =
            new Dictionary<int, CaptureOrder>();

        public void IssueCaptureOrder(
            int commandId,
            Vector3 objectivePosition)
        {
            orders[commandId] =
                new CaptureOrder(
                    commandId,
                    objectivePosition);
        }

        public bool TryGetOrder(
            int commandId,
            out CaptureOrder order)
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
