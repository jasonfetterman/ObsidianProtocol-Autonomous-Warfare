using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum NetworkUpdatePriority
    {
        Critical,
        High,
        Normal,
        Low,
        Background
    }

    public sealed class NetworkUpdateProfile
    {
        public string ProfileId { get; }

        public NetworkUpdatePriority Priority { get; private set; }

        public float UpdateInterval { get; private set; }

        public float TimeUntilUpdate { get; private set; }

        public int MaximumUpdatesPerSecond { get; private set; }

        public bool Enabled { get; private set; }

        public NetworkUpdateProfile(
            string profileId,
            NetworkUpdatePriority priority,
            float updateInterval,
            int maximumUpdatesPerSecond)
        {
            ProfileId =
                profileId ?? string.Empty;

            Priority =
                priority;

            UpdateInterval =
                Math.Max(
                    0.001f,
                    updateInterval);

            TimeUntilUpdate = 0f;

            MaximumUpdatesPerSecond =
                Math.Max(
                    1,
                    maximumUpdatesPerSecond);

            Enabled = true;
        }

        public bool SetPriority(
            NetworkUpdatePriority priority)
        {
            Priority =
                priority;

            return true;
        }

        public bool SetInterval(
            float interval)
        {
            if (interval <= 0f)
            {
                return false;
            }

            UpdateInterval =
                interval;

            TimeUntilUpdate =
                Math.Min(
                    TimeUntilUpdate,
                    UpdateInterval);

            return true;
        }

        public bool SetMaximumUpdatesPerSecond(
            int maximum)
        {
            if (maximum <= 0)
            {
                return false;
            }

            MaximumUpdatesPerSecond =
                maximum;

            return true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled =
                enabled;

            return true;
        }

        public bool ShouldSend(
            float deltaTime)
        {
            if (!Enabled ||
                deltaTime < 0f)
            {
                return false;
            }

            TimeUntilUpdate -=
                deltaTime;

            if (TimeUntilUpdate > 0f)
            {
                return false;
            }

            TimeUntilUpdate =
                UpdateInterval;

            return true;
        }
    }

    public sealed class NetworkOptimization
    {
        private readonly Dictionary<
            string,
            NetworkUpdateProfile> profiles =
            new Dictionary<
                string,
                NetworkUpdateProfile>(
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
            NetworkUpdatePriority priority,
            float updateInterval,
            int maximumUpdatesPerSecond)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId) ||
                updateInterval <= 0f ||
                maximumUpdatesPerSecond <= 0)
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
                new NetworkUpdateProfile(
                    id,
                    priority,
                    updateInterval,
                    maximumUpdatesPerSecond));

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

        public bool SetPriority(
            string profileId,
            NetworkUpdatePriority priority)
        {
            NetworkUpdateProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetPriority(priority);
        }

        public bool SetInterval(
            string profileId,
            float interval)
        {
            NetworkUpdateProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetInterval(interval);
        }

        public bool SetMaximumUpdatesPerSecond(
            string profileId,
            int maximum)
        {
            NetworkUpdateProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetMaximumUpdatesPerSecond(
                       maximum);
        }

        public bool SetEnabled(
            string profileId,
            bool enabled)
        {
            NetworkUpdateProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetEnabled(enabled);
        }

        public bool ShouldSend(
            string profileId,
            float deltaTime)
        {
            NetworkUpdateProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.ShouldSend(deltaTime);
        }

        public NetworkUpdateProfile GetProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return null;
            }

            profiles.TryGetValue(
                profileId.Trim(),
                out NetworkUpdateProfile profile);

            return profile;
        }

        public IReadOnlyCollection<
            NetworkUpdateProfile>
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
