using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Construction
{
    public enum ConstructionState
    {
        Pending,
        Approved,
        InProgress,
        Completed,
        Cancelled,
        Failed
    }

    public sealed class ConstructionOrder
    {
        public string OrderId { get; }

        public string StructureId { get; }

        public string BuilderId { get; }

        public float BuildTime { get; }

        public ConstructionState State { get; private set; }

        public ConstructionOrder(
            string orderId,
            string structureId,
            string builderId,
            float buildTime)
        {
            OrderId =
                orderId ?? string.Empty;

            StructureId =
                structureId ?? string.Empty;

            BuilderId =
                builderId ?? string.Empty;

            BuildTime =
                Math.Max(
                    0f,
                    buildTime);

            State =
                ConstructionState.Pending;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(OrderId) &&
            !string.IsNullOrWhiteSpace(StructureId) &&
            BuildTime > 0f;

        public void Approve()
        {
            if (State ==
                ConstructionState.Pending)
            {
                State =
                    ConstructionState.Approved;
            }
        }

        public void Begin()
        {
            if (State ==
                ConstructionState.Approved)
            {
                State =
                    ConstructionState.InProgress;
            }
        }

        public void Complete()
        {
            if (State ==
                ConstructionState.InProgress)
            {
                State =
                    ConstructionState.Completed;
            }
        }

        public void Cancel()
        {
            if (State !=
                ConstructionState.Completed)
            {
                State =
                    ConstructionState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State !=
                ConstructionState.Completed)
            {
                State =
                    ConstructionState.Failed;
            }
        }
    }

    public sealed class ConstructionFramework
    {
        private readonly Dictionary<string, ConstructionOrder>
            orders =
                new Dictionary<string, ConstructionOrder>(
                    StringComparer.OrdinalIgnoreCase);

        public bool RegisterOrder(
            ConstructionOrder order)
        {
            if (order == null ||
                !order.Valid ||
                orders.ContainsKey(order.OrderId))
            {
                return false;
            }

            orders.Add(
                order.OrderId,
                order);

            return true;
        }

        public bool RemoveOrder(
            string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return false;
            }

            return orders.Remove(
                orderId);
        }

        public bool TryGetOrder(
            string orderId,
            out ConstructionOrder order)
        {
            return orders.TryGetValue(
                orderId,
                out order);
        }

        public bool ApproveOrder(
            string orderId)
        {
            if (!orders.TryGetValue(
                    orderId,
                    out ConstructionOrder order))
            {
                return false;
            }

            order.Approve();

            return true;
        }

        public bool BeginOrder(
            string orderId)
        {
            if (!orders.TryGetValue(
                    orderId,
                    out ConstructionOrder order))
            {
                return false;
            }

            order.Begin();

            return true;
        }

        public bool CompleteOrder(
            string orderId)
        {
            if (!orders.TryGetValue(
                    orderId,
                    out ConstructionOrder order))
            {
                return false;
            }

            order.Complete();

            return true;
        }

        public bool CancelOrder(
            string orderId)
        {
            if (!orders.TryGetValue(
                    orderId,
                    out ConstructionOrder order))
            {
                return false;
            }

            order.Cancel();

            return true;
        }

        public bool FailOrder(
            string orderId)
        {
            if (!orders.TryGetValue(
                    orderId,
                    out ConstructionOrder order))
            {
                return false;
            }

            order.Fail();

            return true;
        }

        public IReadOnlyCollection<ConstructionOrder>
            GetOrders()
        {
            return orders.Values;
        }

        public IReadOnlyCollection<ConstructionOrder>
            GetActiveOrders()
        {
            List<ConstructionOrder> active =
                new List<ConstructionOrder>();

            foreach (
                ConstructionOrder order
                in orders.Values)
            {
                if (order.State ==
                        ConstructionState.Approved ||
                    order.State ==
                        ConstructionState.InProgress)
                {
                    active.Add(
                        order);
                }
            }

            return active;
        }

        public void Clear()
        {
            orders.Clear();
        }
    }
}
