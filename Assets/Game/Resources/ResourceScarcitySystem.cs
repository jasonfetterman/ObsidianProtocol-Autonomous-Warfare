using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public enum ResourceScarcityLevel
    {
        Depleted,
        Critical,
        Low,
        Normal,
        Abundant
    }

    public sealed class ResourceScarcityProfile
    {
        public string ResourceId { get; }

        public int CriticalThreshold { get; }

        public int LowThreshold { get; }

        public int AbundantThreshold { get; }

        public ResourceScarcityProfile(
            string resourceId,
            int criticalThreshold,
            int lowThreshold,
            int abundantThreshold)
        {
            ResourceId =
                resourceId ?? string.Empty;

            CriticalThreshold =
                Math.Max(
                    0,
                    criticalThreshold);

            LowThreshold =
                Math.Max(
                    CriticalThreshold,
                    lowThreshold);

            AbundantThreshold =
                Math.Max(
                    LowThreshold,
                    abundantThreshold);
        }

        public ResourceScarcityLevel Evaluate(
            int amount)
        {
            if (amount <= 0)
            {
                return ResourceScarcityLevel.Depleted;
            }

            if (amount <= CriticalThreshold)
            {
                return ResourceScarcityLevel.Critical;
            }

            if (amount <= LowThreshold)
            {
                return ResourceScarcityLevel.Low;
            }

            if (amount >= AbundantThreshold)
            {
                return ResourceScarcityLevel.Abundant;
            }

            return ResourceScarcityLevel.Normal;
        }
    }

    public sealed class ResourceScarcitySystem
    {
        private readonly Dictionary<string, ResourceScarcityProfile> profiles =
            new Dictionary<string, ResourceScarcityProfile>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterProfile(
            ResourceScarcityProfile profile)
        {
            if (profile == null ||
                string.IsNullOrWhiteSpace(profile.ResourceId) ||
                profiles.ContainsKey(profile.ResourceId))
            {
                return false;
            }

            profiles.Add(
                profile.ResourceId,
                profile);

            return true;
        }

        public bool RemoveProfile(
            string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            return profiles.Remove(
                resourceId);
        }

        public bool TryGetProfile(
            string resourceId,
            out ResourceScarcityProfile profile)
        {
            return profiles.TryGetValue(
                resourceId,
                out profile);
        }

        public ResourceScarcityLevel Evaluate(
            string resourceId,
            int amount)
        {
            if (!profiles.TryGetValue(
                    resourceId,
                    out ResourceScarcityProfile profile))
            {
                return amount <= 0
                    ? ResourceScarcityLevel.Depleted
                    : ResourceScarcityLevel.Normal;
            }

            return profile.Evaluate(
                amount);
        }

        public ResourceScarcityLevel Evaluate(
            string resourceId,
            ResourceInventory inventory)
        {
            if (inventory == null)
            {
                return ResourceScarcityLevel.Depleted;
            }

            return Evaluate(
                resourceId,
                inventory.GetAmount(
                    resourceId));
        }

        public IReadOnlyCollection<ResourceScarcityProfile>
            GetProfiles()
        {
            return profiles.Values;
        }
    }
}
