using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class StorePolish
    {
        private readonly Dictionary<
            string,
            bool> features =
            new Dictionary<
                string,
                bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int FeatureCount =>
            features.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            features.Clear();

            SetDefault("ItemPreview", true);
            SetDefault("UnitPreview", true);
            SetDefault("CustomizationPreview", true);
            SetDefault("PriceDisplay", true);
            SetDefault("CreditBalance", true);
            SetDefault("PurchaseConfirmation", true);
            SetDefault("InventoryIntegration", true);
            SetDefault("GarageIntegration", true);
            SetDefault("VRStoreInteraction", true);

            Initialized = true;

            return true;
        }

        public bool SetFeature(
            string featureId,
            bool enabled)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(featureId))
            {
                return false;
            }

            features[featureId.Trim()] =
                enabled;

            return true;
        }

        public bool IsEnabled(
            string featureId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(featureId))
            {
                return false;
            }

            return features.TryGetValue(
                featureId.Trim(),
                out bool enabled) &&
                   enabled;
        }

        private void SetDefault(
            string key,
            bool enabled)
        {
            features[key] = enabled;
        }

        public void Reset()
        {
            features.Clear();
            Initialized = false;
        }
    }
}
