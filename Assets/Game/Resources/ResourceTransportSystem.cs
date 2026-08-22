using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public enum ResourceTransportState
    {
        Pending,
        InTransit,
        Delivered,
        Cancelled
    }

    public sealed class ResourceTransportOrder
    {
        public string TransportId { get; }
        public string ResourceId { get; }
        public int Amount { get; }

        public ResourceTransportState State { get; private set; }

        public ResourceSystem Source { get; }
        public ResourceSystem Destination { get; }

        public ResourceTransportOrder(
            string transportId,
            string resourceId,
            int amount,
            ResourceSystem source,
            ResourceSystem destination)
        {
            TransportId =
                transportId ?? string.Empty;

            ResourceId =
                resourceId ?? string.Empty;

            Amount =
                Math.Max(0, amount);

            Source = source;
            Destination = destination;

            State =
                ResourceTransportState.Pending;
        }

        public void SetState(
            ResourceTransportState state)
        {
            State = state;
        }
    }

    public sealed class ResourceTransportSystem
    {
        private readonly Dictionary<string, ResourceTransportOrder> orders =
            new Dictionary<string, ResourceTransportOrder>(
                StringComparer.OrdinalIgnoreCase);

        public bool CreateTransportOrder(
            string transportId,
            string resourceId,
            int amount,
            ResourceSystem source,
            ResourceSystem destination)
        {
            if (string.IsNullOrWhiteSpace(transportId) ||
                string.IsNullOrWhiteSpace(resourceId) ||
                amount <= 0 ||
                source == null ||
                destination == null ||
                source == destination ||
                orders.ContainsKey(transportId))
            {
                return false;
            }

            if (!source.HasDefinition(resourceId) ||
                !destination.HasDefinition(resourceId) ||
                source.GetAmount(resourceId) < amount)
            {
                return false;
            }

            ResourceTransportOrder order =
                new ResourceTransportOrder(
                    transportId,
                    resourceId,
                    amount,
                    source,
                    destination);

            orders.Add(
                transportId,
                order);

            return true;
        }

        public bool BeginTransport(
            string transportId)
        {
            if (!orders.TryGetValue(
                    transportId,
                    out ResourceTransportOrder order))
            {
                return false;
            }

            if (order.State !=
                ResourceTransportState.Pending)
            {
                return false;
            }

            order.SetState(
                ResourceTransportState.InTransit);

            return true;
        }

        public bool DeliverTransport(
            string transportId)
        {
            if (!orders.TryGetValue(
                    transportId,
                    out ResourceTransportOrder order))
            {
                return false;
            }

            if (order.State !=
                ResourceTransportState.InTransit)
            {
                return false;
            }

            if (!order.Source.TryTransferTo(
                    order.Destination,
                    order.ResourceId,
                    order.Amount))
            {
                return false;
            }

            order.SetState(
                ResourceTransportState.Delivered);

            return true;
        }

        public bool CancelTransport(
            string transportId)
        {
            if (!orders.TryGetValue(
                    transportId,
                    out ResourceTransportOrder order))
            {
                return false;
            }

            if (order.State ==
                    ResourceTransportState.Delivered ||
                order.State ==
                    ResourceTransportState.Cancelled)
            {
                return false;
            }

            order.SetState(
                ResourceTransportState.Cancelled);

            return true;
        }

        public bool TryGetOrder(
            string transportId,
            out ResourceTransportOrder order)
        {
            return orders.TryGetValue(
                transportId,
                out order);
        }

        public IReadOnlyCollection<ResourceTransportOrder>
            GetOrders()
        {
            return orders.Values;
        }
    }
}
