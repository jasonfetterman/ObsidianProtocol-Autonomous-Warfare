using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class GaragePolish
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

            SetDefault("UnitDisplay", true);
            SetDefault("Lighting", true);
            SetDefault("MaintenanceStations", true);
            SetDefault("CustomizationBay", true);
            SetDefault("WeaponStations", true);
            SetDefault("UpgradeStations", true);
            SetDefault("AIInterface", true);
            SetDefault("FleetCommand", true);
            SetDefault("DeploymentArea", true);
            SetDefault("VRWalkthrough", true);

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
