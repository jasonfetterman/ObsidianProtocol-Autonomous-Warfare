using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum LogisticsState
    {
        Idle,
        Preparing,
        Moving,
        Delayed,
        Delivered,
        Disrupted,
        Lost
    }

    public sealed class LogisticsRouteRecord
    {
        public string RouteId { get; }

        public string OwnerId { get; }

        public string OriginRegionId { get; }

        public string DestinationRegionId { get; }

        public LogisticsState State { get; private set; }

        public float Progress { get; private set; }

        public float SupplyAmount { get; private set; }

        public long LastUpdateTick { get; private set; }

        public LogisticsRouteRecord(
            string routeId,
            string ownerId,
            string originRegionId,
            string destinationRegionId,
            float supplyAmount)
        {
            RouteId =
                routeId ?? string.Empty;

            OwnerId =
                ownerId ?? string.Empty;

            OriginRegionId =
                originRegionId ?? string.Empty;

            DestinationRegionId =
                destinationRegionId ?? string.Empty;

            State =
                LogisticsState.Idle;

            Progress = 0f;

            SupplyAmount =
                supplyAmount >= 0f
                    ? supplyAmount
                    : 0f;

            LastUpdateTick = 0;
        }

        public bool SetState(
            LogisticsState state,
            long updateTick)
        {
            if (updateTick < LastUpdateTick)
            {
                return false;
            }

            State = state;
            LastUpdateTick = updateTick;

            return true;
        }

        public bool SetProgress(
            float progress,
            long updateTick)
        {
            if (progress < 0f ||
                progress > 100f ||
                updateTick < LastUpdateTick)
            {
                return false;
            }

            Progress = progress;
            LastUpdateTick = updateTick;

            if (Progress >= 100f)
            {
                State =
                    LogisticsState.Delivered;
            }
            else if (Progress > 0f &&
                     State ==
                         LogisticsState.Idle)
            {
                State =
                    LogisticsState.Moving;
            }

            return true;
        }

        public bool SetSupplyAmount(
            float amount)
        {
            if (amount < 0f)
            {
                return false;
            }

            SupplyAmount = amount;

            return true;
        }
    }

    public sealed class PersistentLogistics
    {
        private readonly Dictionary<
            string,
            LogisticsRouteRecord> routes =
            new Dictionary<
                string,
                LogisticsRouteRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RouteCount =>
            routes.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            routes.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterRoute(
            string routeId,
            string ownerId,
            string originRegionId,
            string destinationRegionId,
            float supplyAmount)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(routeId) ||
                string.IsNullOrWhiteSpace(ownerId) ||
                string.IsNullOrWhiteSpace(originRegionId) ||
                string.IsNullOrWhiteSpace(destinationRegionId) ||
                supplyAmount < 0f)
            {
                return false;
            }

            string id =
                routeId.Trim();

            if (routes.ContainsKey(id))
            {
                return false;
            }

            routes.Add(
                id,
                new LogisticsRouteRecord(
                    id,
                    ownerId.Trim(),
                    originRegionId.Trim(),
                    destinationRegionId.Trim(),
                    supplyAmount));

            return true;
        }

        public bool SetRouteState(
            string routeId,
            LogisticsState state,
            long updateTick)
        {
            LogisticsRouteRecord route =
                GetRoute(routeId);

            return route != null &&
                   route.SetState(
                       state,
                       updateTick);
        }

        public bool SetRouteProgress(
            string routeId,
            float progress,
            long updateTick)
        {
            LogisticsRouteRecord route =
                GetRoute(routeId);

            return route != null &&
                   route.SetProgress(
                       progress,
                       updateTick);
        }

        public bool SetSupplyAmount(
            string routeId,
            float amount)
        {
            LogisticsRouteRecord route =
                GetRoute(routeId);

            return route != null &&
                   route.SetSupplyAmount(amount);
        }

        public LogisticsRouteRecord GetRoute(
            string routeId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(routeId))
            {
                return null;
            }

            routes.TryGetValue(
                routeId.Trim(),
                out LogisticsRouteRecord route);

            return route;
        }

        public IReadOnlyCollection<
            LogisticsRouteRecord>
            GetRoutes()
        {
            return routes.Values;
        }

        public void Reset()
        {
            routes.Clear();
            Initialized = false;
        }
    }
}
