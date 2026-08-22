using System;
using System.Collections.Generic;
using ObsidianProtocol.Game.Resources;
using ObsidianProtocol.Game.GroundWarfare;

namespace ObsidianProtocol.Game.Logistics
{
    public enum SupplyDeliveryMode
    {
        ResourceTransfer,
        CargoTransport
    }

    public sealed class SupplyDelivery
    {
        public string DeliveryId { get; }
        public string RequestId { get; }
        public string RouteId { get; }
        public string TransportId { get; }

        public SupplyDeliveryMode Mode { get; }

        public SupplyDelivery(
            string deliveryId,
            string requestId,
            string routeId,
            string transportId,
            SupplyDeliveryMode mode)
        {
            DeliveryId =
                deliveryId ?? string.Empty;

            RequestId =
                requestId ?? string.Empty;

            RouteId =
                routeId ?? string.Empty;

            TransportId =
                transportId ?? string.Empty;

            Mode =
                mode;
        }
    }

    public sealed class SupplyLogisticsCoordinator
    {
        private readonly SupplyFramework supplyFramework;
        private readonly SupplyRouteSystem supplyRouteSystem;
        private readonly ResourceTransportSystem resourceTransportSystem;
        private readonly HeavyTransportSystem heavyTransportSystem;

        private readonly Dictionary<string, SupplyDelivery> deliveries =
            new Dictionary<string, SupplyDelivery>(
                StringComparer.OrdinalIgnoreCase);

        public SupplyLogisticsCoordinator(
            SupplyFramework supplyFramework,
            SupplyRouteSystem supplyRouteSystem,
            ResourceTransportSystem resourceTransportSystem,
            HeavyTransportSystem heavyTransportSystem)
        {
            this.supplyFramework =
                supplyFramework;

            this.supplyRouteSystem =
                supplyRouteSystem;

            this.resourceTransportSystem =
                resourceTransportSystem;

            this.heavyTransportSystem =
                heavyTransportSystem;
        }

        public bool CreateResourceDelivery(
            string deliveryId,
            string requestId,
            string routeId,
            string resourceId,
            ResourceSystem source,
            ResourceSystem destination)
        {
            if (string.IsNullOrWhiteSpace(deliveryId) ||
                string.IsNullOrWhiteSpace(requestId) ||
                string.IsNullOrWhiteSpace(routeId) ||
                string.IsNullOrWhiteSpace(resourceId) ||
                source == null ||
                destination == null)
            {
                return false;
            }

            if (deliveries.ContainsKey(deliveryId))
            {
                return false;
            }

            if (!supplyFramework.TryGetRequest(
                    requestId,
                    out SupplyRequest request))
            {
                return false;
            }

            if (request.State !=
                SupplyRequestState.Pending)
            {
                return false;
            }

            if (!supplyRouteSystem.TryGetRoute(
                    routeId,
                    out SupplyRoute route) ||
                !route.Available)
            {
                return false;
            }

            int amount =
                Math.Max(
                    0,
                    (int)Math.Ceiling(request.Amount));

            if (amount <= 0 ||
                route.Capacity < amount)
            {
                return false;
            }

            if (!resourceTransportSystem.CreateTransportOrder(
                    deliveryId,
                    resourceId,
                    amount,
                    source,
                    destination))
            {
                return false;
            }

            deliveries.Add(
                deliveryId,
                new SupplyDelivery(
                    deliveryId,
                    requestId,
                    routeId,
                    string.Empty,
                    SupplyDeliveryMode.ResourceTransfer));

            return true;
        }

        public bool CreateCargoDelivery(
            string deliveryId,
            string requestId,
            string routeId,
            string transportId)
        {
            if (string.IsNullOrWhiteSpace(deliveryId) ||
                string.IsNullOrWhiteSpace(requestId) ||
                string.IsNullOrWhiteSpace(routeId) ||
                string.IsNullOrWhiteSpace(transportId))
            {
                return false;
            }

            if (deliveries.ContainsKey(deliveryId))
            {
                return false;
            }

            if (!supplyFramework.TryGetRequest(
                    requestId,
                    out SupplyRequest request))
            {
                return false;
            }

            if (request.State !=
                SupplyRequestState.Pending)
            {
                return false;
            }

            if (!supplyRouteSystem.TryGetRoute(
                    routeId,
                    out SupplyRoute route) ||
                !route.Available)
            {
                return false;
            }

            if (!heavyTransportSystem.TryGetTransport(
                    transportId,
                    out HeavyTransportState transport))
            {
                return false;
            }

            if (request.Amount <= 0f ||
                request.Amount > route.Capacity ||
                request.Amount >
                    transport.GetAvailableCapacity())
            {
                return false;
            }

            deliveries.Add(
                deliveryId,
                new SupplyDelivery(
                    deliveryId,
                    requestId,
                    routeId,
                    transportId,
                    SupplyDeliveryMode.CargoTransport));

            return true;
        }

        public bool BeginResourceDelivery(
            string deliveryId)
        {
            if (!deliveries.TryGetValue(
                    deliveryId,
                    out SupplyDelivery delivery))
            {
                return false;
            }

            if (delivery.Mode !=
                SupplyDeliveryMode.ResourceTransfer)
            {
                return false;
            }

            return resourceTransportSystem.BeginTransport(
                delivery.DeliveryId);
        }

        public bool DeliverResource(
            string deliveryId)
        {
            if (!deliveries.TryGetValue(
                    deliveryId,
                    out SupplyDelivery delivery))
            {
                return false;
            }

            if (delivery.Mode !=
                SupplyDeliveryMode.ResourceTransfer)
            {
                return false;
            }

            if (!resourceTransportSystem.DeliverTransport(
                    delivery.DeliveryId))
            {
                return false;
            }

            supplyFramework.FulfillRequest(
                delivery.RequestId);

            return true;
        }

        public bool LoadCargoDelivery(
            string deliveryId)
        {
            if (!deliveries.TryGetValue(
                    deliveryId,
                    out SupplyDelivery delivery))
            {
                return false;
            }

            if (delivery.Mode !=
                SupplyDeliveryMode.CargoTransport)
            {
                return false;
            }

            if (!supplyFramework.TryGetRequest(
                    delivery.RequestId,
                    out SupplyRequest request))
            {
                return false;
            }

            return heavyTransportSystem.Load(
                delivery.TransportId,
                request.Amount);
        }

        public bool CompleteCargoDelivery(
            string deliveryId)
        {
            if (!deliveries.TryGetValue(
                    deliveryId,
                    out SupplyDelivery delivery))
            {
                return false;
            }

            if (delivery.Mode !=
                SupplyDeliveryMode.CargoTransport)
            {
                return false;
            }

            if (!supplyFramework.TryGetRequest(
                    delivery.RequestId,
                    out SupplyRequest request))
            {
                return false;
            }

            if (!heavyTransportSystem.TryGetTransport(
                    delivery.TransportId,
                    out HeavyTransportState transport))
            {
                return false;
            }

            if (transport.CurrentCargo < request.Amount)
            {
                return false;
            }

            if (!heavyTransportSystem.Unload(
                    delivery.TransportId,
                    request.Amount))
            {
                return false;
            }

            supplyFramework.FulfillRequest(
                delivery.RequestId);

            return true;
        }

        public bool CancelDelivery(
            string deliveryId)
        {
            if (!deliveries.TryGetValue(
                    deliveryId,
                    out SupplyDelivery delivery))
            {
                return false;
            }

            supplyFramework.CancelRequest(
                delivery.RequestId);

            if (delivery.Mode ==
                SupplyDeliveryMode.ResourceTransfer)
            {
                resourceTransportSystem.CancelTransport(
                    delivery.DeliveryId);
            }

            deliveries.Remove(
                deliveryId);

            return true;
        }

        public bool TryGetDelivery(
            string deliveryId,
            out SupplyDelivery delivery)
        {
            return deliveries.TryGetValue(
                deliveryId,
                out delivery);
        }

        public IReadOnlyCollection<SupplyDelivery>
            GetDeliveries()
        {
            return deliveries.Values;
        }

        public void Clear()
        {
            deliveries.Clear();
        }
    }
}
