using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class ServerOptimizationProfile
    {
        public string ProfileId { get; }

        public int MaximumSimulationUnits { get; private set; }

        public int MaximumNetworkUpdatesPerTick { get; private set; }

        public int MaximumTasksPerTick { get; private set; }

        public float TickInterval { get; private set; }

        public bool AdaptiveLoadEnabled { get; private set; }

        public ServerOptimizationProfile(
            string profileId,
            int maximumSimulationUnits,
            int maximumNetworkUpdatesPerTick,
            int maximumTasksPerTick,
            float tickInterval)
        {
            ProfileId =
                profileId ?? string.Empty;

            MaximumSimulationUnits =
                Math.Max(
                    1,
                    maximumSimulationUnits);

            MaximumNetworkUpdatesPerTick =
                Math.Max(
                    1,
                    maximumNetworkUpdatesPerTick);

            MaximumTasksPerTick =
                Math.Max(
                    1,
                    maximumTasksPerTick);

            TickInterval =
                Math.Max(
                    0.001f,
                    tickInterval);

            AdaptiveLoadEnabled = true;
        }

        public bool SetSimulationUnitLimit(
            int limit)
        {
            if (limit <= 0)
            {
                return false;
            }

            MaximumSimulationUnits =
                limit;

            return true;
        }

        public bool SetNetworkUpdateLimit(
            int limit)
        {
            if (limit <= 0)
            {
                return false;
            }

            MaximumNetworkUpdatesPerTick =
                limit;

            return true;
        }

        public bool SetTaskLimit(
            int limit)
        {
            if (limit <= 0)
            {
                return false;
            }

            MaximumTasksPerTick =
                limit;

            return true;
        }

        public bool SetTickInterval(
            float interval)
        {
            if (interval <= 0f)
            {
                return false;
            }

            TickInterval =
                interval;

            return true;
        }

        public bool SetAdaptiveLoad(
            bool enabled)
        {
            AdaptiveLoadEnabled =
                enabled;

            return true;
        }
    }

    public sealed class ServerOptimization
    {
        private readonly Dictionary<
            string,
            ServerOptimizationProfile> profiles =
            new Dictionary<
                string,
                ServerOptimizationProfile>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ProfileCount =>
            profiles.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            profiles.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterProfile(
            string profileId,
            int maximumSimulationUnits,
            int maximumNetworkUpdatesPerTick,
            int maximumTasksPerTick,
            float tickInterval)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId) ||
                maximumSimulationUnits <= 0 ||
                maximumNetworkUpdatesPerTick <= 0 ||
                maximumTasksPerTick <= 0 ||
                tickInterval <= 0f)
            {
                return false;
            }

            string id =
                profileId.Trim();

            if (profiles.ContainsKey(id))
            {
                return false;
            }

            profiles.Add(
                id,
                new ServerOptimizationProfile(
                    id,
                    maximumSimulationUnits,
                    maximumNetworkUpdatesPerTick,
                    maximumTasksPerTick,
                    tickInterval));

            return true;
        }

        public bool RemoveProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return false;
            }

            return profiles.Remove(
                profileId.Trim());
        }

        public bool SetSimulationUnitLimit(
            string profileId,
            int limit)
        {
            ServerOptimizationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetSimulationUnitLimit(limit);
        }

        public bool SetNetworkUpdateLimit(
            string profileId,
            int limit)
        {
            ServerOptimizationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetNetworkUpdateLimit(limit);
        }

        public bool SetTaskLimit(
            string profileId,
            int limit)
        {
            ServerOptimizationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetTaskLimit(limit);
        }

        public bool SetTickInterval(
            string profileId,
            float interval)
        {
            ServerOptimizationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetTickInterval(interval);
        }

        public bool SetAdaptiveLoad(
            string profileId,
            bool enabled)
        {
            ServerOptimizationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetAdaptiveLoad(enabled);
        }

        public ServerOptimizationProfile GetProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return null;
            }

            profiles.TryGetValue(
                profileId.Trim(),
                out ServerOptimizationProfile profile);

            return profile;
        }

        public IReadOnlyCollection<
            ServerOptimizationProfile>
            GetProfiles()
        {
            return profiles.Values;
        }

        public void Reset()
        {
            profiles.Clear();

            Initialized = false;
        }
    }
}
