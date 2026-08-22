using System;
using System.Collections.Generic;
using UnityEngine;
using ObsidianProtocol.Game.World.Routes;

namespace ObsidianProtocol.Game.Logistics
{
    public enum SupplyRouteState
    {
        Inactive,
        Active
    }

    public sealed class SupplyRoute
    {
        public string RouteId { get; }

        public string OriginId { get; }

        public string DestinationId { get; }

        public TraversalRoute TraversalRoute { get; }

        public float Capacity { get; }

        public float TravelTime { get; }

        public SupplyRouteState State { get; private set; }

        public bool Available =>
            State == SupplyRouteState.Active &&
            TraversalRoute != null &&
            Capacity > 0f;

        public float Length =>
            TraversalRoute != null
                ? TraversalRoute.Length
                : 0f;

        public float MovementCostMultiplier =>
            TraversalRoute != null &&
            TraversalRoute.Definition != null
                ? TraversalRoute.Definition.MovementCostMultiplier
                : 1f;

        public float EffectiveTravelTime =>
            Mathf.Max(
                0f,
                TravelTime * MovementCostMultiplier);

        public SupplyRoute(
            string routeId,
            string originId,
            string destinationId,
            TraversalRoute traversalRoute,
            float capacity,
            float travelTime)
        {
            RouteId =
                routeId ?? string.Empty;

            OriginId =
                originId ?? string.Empty;

            DestinationId =
                destinationId ?? string.Empty;

            TraversalRoute =
                traversalRoute;

            Capacity =
                Mathf.Max(
                    0f,
                    capacity);

            TravelTime =
                Mathf.Max(
                    0f,
                    travelTime);

            State =
                SupplyRouteState.Inactive;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(RouteId) &&
            !string.IsNullOrWhiteSpace(OriginId) &&
            !string.IsNullOrWhiteSpace(DestinationId) &&
            !string.Equals(
                OriginId,
                DestinationId,
                StringComparison.OrdinalIgnoreCase) &&
            TraversalRoute != null &&
            Capacity > 0f;

        public bool Connects(
            string originId,
            string destinationId)
        {
            return
                string.Equals(
                    OriginId,
                    originId,
                    StringComparison.OrdinalIgnoreCase) &&
                string.Equals(
                    DestinationId,
                    destinationId,
                    StringComparison.OrdinalIgnoreCase);
        }

        public void SetActive(
            bool active)
        {
            State =
                active
                    ? SupplyRouteState.Active
                    : SupplyRouteState.Inactive;
        }
    }

    public sealed class SupplyRouteSystem
    {
        private readonly Dictionary<string, SupplyRoute> routes =
            new Dictionary<string, SupplyRoute>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterRoute(
            SupplyRoute route)
        {
            if (route == null ||
                !route.Valid ||
                routes.ContainsKey(route.RouteId))
            {
                return false;
            }

            routes.Add(
                route.RouteId,
                route);

            return true;
        }

        public bool RemoveRoute(
            string routeId)
        {
            if (string.IsNullOrWhiteSpace(routeId))
            {
                return false;
            }

            return routes.Remove(
                routeId);
        }

        public bool TryGetRoute(
            string routeId,
            out SupplyRoute route)
        {
            return routes.TryGetValue(
                routeId,
                out route);
        }

        public bool SetRouteActive(
            string routeId,
            bool active)
        {
            if (!routes.TryGetValue(
                    routeId,
                    out SupplyRoute route))
            {
                return false;
            }

            route.SetActive(active);

            return true;
        }

        public bool TryFindActiveRoute(
            string originId,
            string destinationId,
            out SupplyRoute route)
        {
            foreach (
                SupplyRoute candidate
                in routes.Values)
            {
                if (!candidate.Available)
                {
                    continue;
                }

                if (candidate.Connects(
                        originId,
                        destinationId))
                {
                    route = candidate;
                    return true;
                }
            }

            route = null;
            return false;
        }

        public IReadOnlyCollection<SupplyRoute>
            GetRoutes()
        {
            return routes.Values;
        }

        public IReadOnlyCollection<SupplyRoute>
            GetActiveRoutes()
        {
            List<SupplyRoute> active =
                new List<SupplyRoute>();

            foreach (
                SupplyRoute route
                in routes.Values)
            {
                if (route.State ==
                    SupplyRouteState.Active)
                {
                    active.Add(route);
                }
            }

            return active;
        }

        public void Clear()
        {
            routes.Clear();
        }
    }
}
