using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Resources
{
    public sealed class ResourceEconomicBalanceProfile
    {
        public string ResourceId { get; }

        public float ProductionMultiplier { get; private set; }

        public float ConsumptionMultiplier { get; private set; }

        public float ExtractionMultiplier { get; private set; }

        public float StorageMultiplier { get; private set; }

        public float ScarcityMultiplier { get; private set; }

        public ResourceEconomicBalanceProfile(
            string resourceId)
        {
            ResourceId =
                resourceId ?? string.Empty;

            ProductionMultiplier = 1f;
            ConsumptionMultiplier = 1f;
            ExtractionMultiplier = 1f;
            StorageMultiplier = 1f;
            ScarcityMultiplier = 1f;
        }

        public void Configure(
            float productionMultiplier,
            float consumptionMultiplier,
            float extractionMultiplier,
            float storageMultiplier,
            float scarcityMultiplier)
        {
            ProductionMultiplier =
                ClampMultiplier(
                    productionMultiplier);

            ConsumptionMultiplier =
                ClampMultiplier(
                    consumptionMultiplier);

            ExtractionMultiplier =
                ClampMultiplier(
                    extractionMultiplier);

            StorageMultiplier =
                ClampMultiplier(
                    storageMultiplier);

            ScarcityMultiplier =
                ClampMultiplier(
                    scarcityMultiplier);
        }

        public int ApplyProduction(
            int amount)
        {
            return ApplyMultiplier(
                amount,
                ProductionMultiplier);
        }

        public int ApplyConsumption(
            int amount)
        {
            return ApplyMultiplier(
                amount,
                ConsumptionMultiplier);
        }

        public int ApplyExtraction(
            int amount)
        {
            return ApplyMultiplier(
                amount,
                ExtractionMultiplier);
        }

        public int ApplyStorage(
            int amount)
        {
            return ApplyMultiplier(
                amount,
                StorageMultiplier);
        }

        public int ApplyScarcity(
            int amount)
        {
            return ApplyMultiplier(
                amount,
                ScarcityMultiplier);
        }

        private static float ClampMultiplier(
            float value)
        {
            if (float.IsNaN(value) ||
                float.IsInfinity(value))
            {
                return 1f;
            }

            return Math.Max(
                0f,
                value);
        }

        private static int ApplyMultiplier(
            int amount,
            float multiplier)
        {
            if (amount <= 0 ||
                multiplier <= 0f)
            {
                return 0;
            }

            double result =
                amount * multiplier;

            if (result >= int.MaxValue)
            {
                return int.MaxValue;
            }

            return Math.Max(
                0,
                (int)Math.Round(
                    result));
        }
    }

    public sealed class ResourceEconomicBalanceSystem
    {
        private readonly Dictionary<string, ResourceEconomicBalanceProfile> profiles =
            new Dictionary<string, ResourceEconomicBalanceProfile>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterProfile(
            ResourceEconomicBalanceProfile profile)
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
            out ResourceEconomicBalanceProfile profile)
        {
            return profiles.TryGetValue(
                resourceId,
                out profile);
        }

        public bool ConfigureProfile(
            string resourceId,
            float productionMultiplier,
            float consumptionMultiplier,
            float extractionMultiplier,
            float storageMultiplier,
            float scarcityMultiplier)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            if (!profiles.TryGetValue(
                    resourceId,
                    out ResourceEconomicBalanceProfile profile))
            {
                profile =
                    new ResourceEconomicBalanceProfile(
                        resourceId);

                profiles.Add(
                    resourceId,
                    profile);
            }

            profile.Configure(
                productionMultiplier,
                consumptionMultiplier,
                extractionMultiplier,
                storageMultiplier,
                scarcityMultiplier);

            return true;
        }

        public int ApplyProduction(
            string resourceId,
            int amount)
        {
            return Apply(
                resourceId,
                amount,
                (profile, value) =>
                    profile.ApplyProduction(value));
        }

        public int ApplyConsumption(
            string resourceId,
            int amount)
        {
            return Apply(
                resourceId,
                amount,
                (profile, value) =>
                    profile.ApplyConsumption(value));
        }

        public int ApplyExtraction(
            string resourceId,
            int amount)
        {
            return Apply(
                resourceId,
                amount,
                (profile, value) =>
                    profile.ApplyExtraction(value));
        }

        public int ApplyStorage(
            string resourceId,
            int amount)
        {
            return Apply(
                resourceId,
                amount,
                (profile, value) =>
                    profile.ApplyStorage(value));
        }

        public int ApplyScarcity(
            string resourceId,
            int amount)
        {
            return Apply(
                resourceId,
                amount,
                (profile, value) =>
                    profile.ApplyScarcity(value));
        }

        public IReadOnlyCollection<ResourceEconomicBalanceProfile>
            GetProfiles()
        {
            return profiles.Values;
        }

        private int Apply(
            string resourceId,
            int amount,
            Func<ResourceEconomicBalanceProfile, int, int> operation)
        {
            if (amount <= 0)
            {
                return 0;
            }

            if (!profiles.TryGetValue(
                    resourceId,
                    out ResourceEconomicBalanceProfile profile))
            {
                return amount;
            }

            return operation(
                profile,
                amount);
        }
    }
}
