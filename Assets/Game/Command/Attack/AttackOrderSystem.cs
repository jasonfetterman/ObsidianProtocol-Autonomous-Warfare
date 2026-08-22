using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Attack
{
    public sealed class AttackOrder
    {
        public int CommandId;
        public GameObject Target;
        public Vector3 TargetPosition;
        public bool HasTarget;

        public AttackOrder(
            int commandId,
            GameObject target)
        {
            CommandId = commandId;
            Target = target;
            HasTarget = target != null;

            if (target != null)
            {
                TargetPosition = target.transform.position;
            }
        }

        public AttackOrder(
            int commandId,
            Vector3 targetPosition)
        {
            CommandId = commandId;
            TargetPosition = targetPosition;
            HasTarget = false;
        }
    }

    public sealed class AttackOrderSystem
    {
        private readonly Dictionary<int, AttackOrder> orders =
            new Dictionary<int, AttackOrder>();

        public void IssueAttackOrder(
            int commandId,
            GameObject target)
        {
            orders[commandId] =
                new AttackOrder(commandId, target);
        }

        public void IssueAttackOrder(
            int commandId,
            Vector3 targetPosition)
        {
            orders[commandId] =
                new AttackOrder(commandId, targetPosition);
        }

        public bool TryGetOrder(
            int commandId,
            out AttackOrder order)
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
