using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class RenderProfile
    {
        public string ProfileId { get; }

        public int MaximumVisibleObjects { get; private set; }

        public int CurrentVisibleObjects { get; private set; }

        public bool DynamicResolutionEnabled { get; private set; }

        public float MinimumRenderScale { get; private set; }

        public float MaximumRenderScale { get; private set; }

        public RenderProfile(
            string profileId,
            int maximumVisibleObjects)
        {
            ProfileId =
                profileId ?? string.Empty;

            MaximumVisibleObjects =
                Math.Max(
                    1,
                    maximumVisibleObjects);

            CurrentVisibleObjects = 0;

            DynamicResolutionEnabled = false;

            MinimumRenderScale = 1f;
            MaximumRenderScale = 1f;
        }

        public bool SetMaximumVisibleObjects(
            int maximum)
        {
            if (maximum <= 0)
            {
                return false;
            }

            MaximumVisibleObjects =
                maximum;

            return true;
        }

        public bool SetVisibleObjects(
            int count)
        {
            if (count < 0)
            {
                return false;
            }

            CurrentVisibleObjects =
                count;

            return true;
        }

        public bool SetDynamicResolution(
            bool enabled,
            float minimumScale,
            float maximumScale)
        {
            if (minimumScale <= 0f ||
                maximumScale <= 0f ||
                minimumScale > maximumScale)
            {
                return false;
            }

            DynamicResolutionEnabled =
                enabled;

            MinimumRenderScale =
                minimumScale;

            MaximumRenderScale =
                maximumScale;

            return true;
        }

        public bool IsOverBudget =>
            CurrentVisibleObjects >
            MaximumVisibleObjects;
    }

    public sealed class RenderingOptimization
    {
        private readonly Dictionary<
            string,
            RenderProfile> profiles =
            new Dictionary<
                string,
                RenderProfile>(
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
            int maximumVisibleObjects)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId) ||
                maximumVisibleObjects <= 0)
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
                new RenderProfile(
                    id,
                    maximumVisibleObjects));

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

        public bool SetVisibleObjects(
            string profileId,
            int count)
        {
            RenderProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetVisibleObjects(
                       count);
        }

        public bool SetDynamicResolution(
            string profileId,
            bool enabled,
            float minimumScale,
            float maximumScale)
        {
            RenderProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetDynamicResolution(
                       enabled,
                       minimumScale,
                       maximumScale);
        }

        public bool IsOverBudget(
            string profileId)
        {
            RenderProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.IsOverBudget;
        }

        public RenderProfile GetProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return null;
            }

            profiles.TryGetValue(
                profileId.Trim(),
                out RenderProfile profile);

            return profile;
        }

        public IReadOnlyCollection<RenderProfile>
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
