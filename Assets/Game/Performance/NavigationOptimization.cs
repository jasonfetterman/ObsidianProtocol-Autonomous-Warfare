using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum NavigationUpdateMode
    {
        Continuous,
        Scheduled,
        OnDemand,
        Disabled
    }

    public sealed class NavigationProfile
    {
        public string ProfileId { get; }

        public NavigationUpdateMode UpdateMode { get; private set; }

        public float UpdateInterval { get; private set; }

        public float TimeUntilUpdate { get; private set; }

        public bool Enabled =>
            UpdateMode != NavigationUpdateMode.Disabled;

        public NavigationProfile(
            string profileId,
            NavigationUpdateMode updateMode,
            float updateInterval)
        {
            ProfileId =
                profileId ?? string.Empty;

            UpdateMode =
                updateMode;

            UpdateInterval =
                Math.Max(
                    0.001f,
                    updateInterval);

            TimeUntilUpdate = 0f;
        }

        public bool SetUpdateMode(
            NavigationUpdateMode updateMode)
        {
            UpdateMode =
                updateMode;

            return true;
        }

        public bool SetUpdateInterval(
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

        public bool ShouldUpdate(
            float deltaTime)
        {
            if (!Enabled ||
                deltaTime < 0f)
            {
                return false;
            }

            if (UpdateMode ==
                NavigationUpdateMode.Continuous)
            {
                return true;
            }

            if (UpdateMode ==
                NavigationUpdateMode.OnDemand)
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

    public sealed class NavigationOptimization
    {
        private readonly Dictionary<
            string,
            NavigationProfile> profiles =
            new Dictionary<
                string,
                NavigationProfile>(
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
            NavigationUpdateMode updateMode,
            float updateInterval)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId) ||
                updateInterval <= 0f)
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
                new NavigationProfile(
                    id,
                    updateMode,
                    updateInterval));

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

        public bool SetUpdateMode(
            string profileId,
            NavigationUpdateMode updateMode)
        {
            NavigationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetUpdateMode(updateMode);
        }

        public bool SetUpdateInterval(
            string profileId,
            float interval)
        {
            NavigationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetUpdateInterval(interval);
        }

        public bool ShouldUpdate(
            string profileId,
            float deltaTime)
        {
            NavigationProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.ShouldUpdate(deltaTime);
        }

        public NavigationProfile GetProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return null;
            }

            profiles.TryGetValue(
                profileId.Trim(),
                out NavigationProfile profile);

            return profile;
        }

        public IReadOnlyCollection<
            NavigationProfile>
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
