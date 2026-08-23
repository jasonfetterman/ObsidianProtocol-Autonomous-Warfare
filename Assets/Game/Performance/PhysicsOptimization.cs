using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum PhysicsSimulationMode
    {
        Full,
        Reduced,
        Disabled
    }

    public sealed class PhysicsProfile
    {
        public string ProfileId { get; }

        public PhysicsSimulationMode Mode { get; private set; }

        public float UpdateInterval { get; private set; }

        public float TimeUntilUpdate { get; private set; }

        public bool CollisionEnabled { get; private set; }

        public PhysicsProfile(
            string profileId,
            PhysicsSimulationMode mode,
            float updateInterval)
        {
            ProfileId =
                profileId ?? string.Empty;

            Mode =
                mode;

            UpdateInterval =
                Math.Max(
                    0.001f,
                    updateInterval);

            TimeUntilUpdate = 0f;

            CollisionEnabled =
                mode != PhysicsSimulationMode.Disabled;
        }

        public bool SetMode(
            PhysicsSimulationMode mode)
        {
            Mode = mode;

            CollisionEnabled =
                mode != PhysicsSimulationMode.Disabled;

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

        public bool SetCollisionEnabled(
            bool enabled)
        {
            CollisionEnabled =
                enabled &&
                Mode != PhysicsSimulationMode.Disabled;

            return true;
        }

        public bool ShouldSimulate(
            float deltaTime)
        {
            if (Mode == PhysicsSimulationMode.Disabled ||
                deltaTime < 0f)
            {
                return false;
            }

            if (Mode == PhysicsSimulationMode.Full)
            {
                return true;
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

    public sealed class PhysicsOptimization
    {
        private readonly Dictionary<
            string,
            PhysicsProfile> profiles =
            new Dictionary<
                string,
                PhysicsProfile>(
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
            PhysicsSimulationMode mode,
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
                new PhysicsProfile(
                    id,
                    mode,
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

        public bool SetMode(
            string profileId,
            PhysicsSimulationMode mode)
        {
            PhysicsProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetMode(mode);
        }

        public bool SetUpdateInterval(
            string profileId,
            float interval)
        {
            PhysicsProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetUpdateInterval(interval);
        }

        public bool SetCollisionEnabled(
            string profileId,
            bool enabled)
        {
            PhysicsProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.SetCollisionEnabled(enabled);
        }

        public bool ShouldSimulate(
            string profileId,
            float deltaTime)
        {
            PhysicsProfile profile =
                GetProfile(profileId);

            return profile != null &&
                   profile.ShouldSimulate(deltaTime);
        }

        public PhysicsProfile GetProfile(
            string profileId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(profileId))
            {
                return null;
            }

            profiles.TryGetValue(
                profileId.Trim(),
                out PhysicsProfile profile);

            return profile;
        }

        public IReadOnlyCollection<PhysicsProfile>
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
