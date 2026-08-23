using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Release
{
    public sealed class PostLaunchInfrastructure
    {
        private readonly Dictionary<
            string,
            bool> systems =
            new Dictionary<
                string,
                bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SystemCount =>
            systems.Count;

        public int OperationalCount
        {
            get
            {
                int count = 0;

                foreach (bool operational
                         in systems.Values)
                {
                    if (operational)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool InfrastructureOperational =>
            Initialized &&
            SystemCount > 0 &&
            OperationalCount == SystemCount;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            systems.Clear();

            RegisterSystem(
                "LiveServerMonitoring");

            RegisterSystem(
                "PlayerSupport");

            RegisterSystem(
                "CrashReporting");

            RegisterSystem(
                "Telemetry");

            RegisterSystem(
                "AutomatedBackups");

            RegisterSystem(
                "WorldRecovery");

            RegisterSystem(
                "SecurityMonitoring");

            RegisterSystem(
                "AntiCheatMonitoring");

            RegisterSystem(
                "Moderation");

            RegisterSystem(
                "PlayerReporting");

            RegisterSystem(
                "Matchmaking");

            RegisterSystem(
                "DedicatedServerManagement");

            RegisterSystem(
                "ContentDeployment");

            RegisterSystem(
                "HotfixPipeline");

            RegisterSystem(
                "PatchPipeline");

            RegisterSystem(
                "VersionManagement");

            RegisterSystem(
                "DatabaseMonitoring");

            RegisterSystem(
                "EconomyMonitoring");

            RegisterSystem(
                "ServiceHealthMonitoring");

            RegisterSystem(
                "IncidentRecovery");

            Initialized = true;

            return true;
        }

        public bool RegisterSystem(
            string systemId)
        {
            if (string.IsNullOrWhiteSpace(systemId))
            {
                return false;
            }

            string id =
                systemId.Trim();

            if (systems.ContainsKey(id))
            {
                return false;
            }

            systems.Add(
                id,
                false);

            return true;
        }

        public bool SetOperational(
            string systemId,
            bool operational)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(systemId))
            {
                return false;
            }

            string id =
                systemId.Trim();

            if (!systems.ContainsKey(id))
            {
                return false;
            }

            systems[id] = operational;

            return true;
        }

        public bool IsOperational(
            string systemId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(systemId))
            {
                return false;
            }

            systems.TryGetValue(
                systemId.Trim(),
                out bool operational);

            return operational;
        }

        public IReadOnlyDictionary<
            string,
            bool>
            GetSystems()
        {
            return systems;
        }

        public void Reset()
        {
            systems.Clear();
            Initialized = false;
        }
    }
}
