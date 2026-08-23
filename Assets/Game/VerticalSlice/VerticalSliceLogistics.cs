using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum LogisticsStatus
    {
        Idle,
        Active,
        Completed,
        Failed
    }

    public sealed class VerticalSliceLogisticsRoute
    {
        public string RouteId { get; }

        public string SourceId { get; }

        public string DestinationId { get; }

        public VerticalSliceLogisticsRoute(
            string routeId,
            string sourceId,
            string destinationId)
        {
            RouteId =
                routeId ?? string.Empty;

            SourceId =
                sourceId ?? string.Empty;

            DestinationId =
                destinationId ?? string.Empty;
        }
    }

    public sealed class VerticalSliceLogistics
    {
        private readonly Dictionary<
            string,
            VerticalSliceLogisticsRoute> routes =
            new Dictionary<
                string,
                VerticalSliceLogisticsRoute>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public VerticalSliceLogisticsRoute ActiveRoute
        {
            get;
            private set;
        }

        public LogisticsStatus Status
        {
            get;
            private set;
        }

        public int RouteCount =>
            routes.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            routes.Clear();

            ActiveRoute = null;

            Status =
                LogisticsStatus.Idle;

            Initialized = true;

            return true;
        }

        public bool RegisterRoute(
            string routeId,
            string sourceId,
            string destinationId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(routeId) ||
                string.IsNullOrWhiteSpace(sourceId) ||
                string.IsNullOrWhiteSpace(destinationId))
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
                new VerticalSliceLogisticsRoute(
                    id,
                    sourceId.Trim(),
                    destinationId.Trim()));

            return true;
        }

        public bool StartRoute(
            string routeId)
        {
            if (!Initialized ||
                Status == LogisticsStatus.Active)
            {
                return false;
            }

            VerticalSliceLogisticsRoute route =
                GetRoute(routeId);

            if (route == null)
            {
                return false;
            }

            ActiveRoute =
                route;

            Status =
                LogisticsStatus.Active;

            return true;
        }

        public bool CompleteRoute()
        {
            if (Status != LogisticsStatus.Active)
            {
                return false;
            }

            Status =
                LogisticsStatus.Completed;

            ActiveRoute = null;

            return true;
        }

        public bool FailRoute()
        {
            if (Status != LogisticsStatus.Active)
            {
                return false;
            }

            Status =
                LogisticsStatus.Failed;

            ActiveRoute = null;

            return true;
        }

        public bool ResetRouteState()
        {
            if (!Initialized)
            {
                return false;
            }

            ActiveRoute = null;

            Status =
                LogisticsStatus.Idle;

            return true;
        }

        public VerticalSliceLogisticsRoute GetRoute(
            string routeId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(routeId))
            {
                return null;
            }

            routes.TryGetValue(
                routeId.Trim(),
                out VerticalSliceLogisticsRoute route);

            return route;
        }

        public IReadOnlyCollection<
            VerticalSliceLogisticsRoute>
            GetRoutes()
        {
            return routes.Values;
        }

        public void Reset()
        {
            routes.Clear();

            ActiveRoute = null;

            Status =
                LogisticsStatus.Idle;

            Initialized = false;
        }
    }
}
