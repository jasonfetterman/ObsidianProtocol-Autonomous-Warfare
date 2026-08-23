using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OnlineWorld
{
    public sealed class ServiceStatus
    {
        public string ServiceId { get; }

        public bool Online { get; private set; }

        public float Health { get; private set; }

        public DateTime LastUpdatedUtc { get; private set; }

        public ServiceStatus(
            string serviceId)
        {
            ServiceId =
                serviceId ?? string.Empty;

            Online = false;
            Health = 0f;
            LastUpdatedUtc =
                DateTime.UtcNow;
        }

        public bool Update(
            bool online,
            float health)
        {
            Online = online;

            Health =
                Clamp(health);

            LastUpdatedUtc =
                DateTime.UtcNow;

            return true;
        }

        private static float Clamp(
            float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            if (value > 1f)
            {
                return 1f;
            }

            return value;
        }
    }

    public sealed class ServiceMonitoring
    {
        private readonly Dictionary<
            string,
            ServiceStatus> services =
            new Dictionary<
                string,
                ServiceStatus>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ServiceCount =>
            services.Count;

        public int OnlineServiceCount
        {
            get
            {
                int count = 0;

                foreach (ServiceStatus service
                         in services.Values)
                {
                    if (service.Online)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            services.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterService(
            string serviceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serviceId))
            {
                return false;
            }

            string id =
                serviceId.Trim();

            if (services.ContainsKey(id))
            {
                return false;
            }

            services.Add(
                id,
                new ServiceStatus(id));

            return true;
        }

        public bool UpdateService(
            string serviceId,
            bool online,
            float health)
        {
            ServiceStatus service =
                GetService(serviceId);

            return service != null &&
                   service.Update(
                       online,
                       health);
        }

        public ServiceStatus GetService(
            string serviceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serviceId))
            {
                return null;
            }

            services.TryGetValue(
                serviceId.Trim(),
                out ServiceStatus service);

            return service;
        }

        public bool RemoveService(
            string serviceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(serviceId))
            {
                return false;
            }

            return services.Remove(
                serviceId.Trim());
        }

        public IReadOnlyCollection<
            ServiceStatus>
            GetServices()
        {
            return services.Values;
        }

        public void Reset()
        {
            services.Clear();
            Initialized = false;
        }
    }
}
