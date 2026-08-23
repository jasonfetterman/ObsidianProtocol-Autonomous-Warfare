using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public sealed class VisibilityCondition
    {
        public string ZoneId { get; }

        public float BaseVisibility { get; }

        public float CurrentVisibility { get; private set; }

        public float EnvironmentalModifier { get; private set; }

        public float SmokeModifier { get; private set; }

        public float WeatherModifier { get; private set; }

        public float EffectiveVisibility =>
            Math.Max(
                0f,
                BaseVisibility *
                EnvironmentalModifier *
                SmokeModifier *
                WeatherModifier);

        public VisibilityCondition(
            string zoneId,
            float baseVisibility)
        {
            ZoneId =
                zoneId ?? string.Empty;

            BaseVisibility =
                Math.Max(0f, baseVisibility);

            CurrentVisibility =
                BaseVisibility;

            EnvironmentalModifier = 1f;
            SmokeModifier = 1f;
            WeatherModifier = 1f;
        }

        public bool SetEnvironmentalModifier(
            float modifier)
        {
            EnvironmentalModifier =
                ClampModifier(modifier);

            Update();

            return true;
        }

        public bool SetSmokeModifier(
            float modifier)
        {
            SmokeModifier =
                ClampModifier(modifier);

            Update();

            return true;
        }

        public bool SetWeatherModifier(
            float modifier)
        {
            WeatherModifier =
                ClampModifier(modifier);

            Update();

            return true;
        }

        private void Update()
        {
            CurrentVisibility =
                EffectiveVisibility;
        }

        private static float ClampModifier(
            float modifier)
        {
            return Math.Max(
                0f,
                Math.Min(1f, modifier));
        }
    }

    public sealed class VisibilityChanges
    {
        private readonly Dictionary<
            string,
            VisibilityCondition> zones =
            new Dictionary<
                string,
                VisibilityCondition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ZoneCount =>
            zones.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            zones.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterZone(
            string zoneId,
            float baseVisibility)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(zoneId) ||
                baseVisibility < 0f)
            {
                return false;
            }

            string id =
                zoneId.Trim();

            if (zones.ContainsKey(id))
            {
                return false;
            }

            zones.Add(
                id,
                new VisibilityCondition(
                    id,
                    baseVisibility));

            return true;
        }

        public bool SetEnvironmentalModifier(
            string zoneId,
            float modifier)
        {
            VisibilityCondition zone =
                GetZone(zoneId);

            return zone != null &&
                   zone.SetEnvironmentalModifier(
                       modifier);
        }

        public bool SetSmokeModifier(
            string zoneId,
            float modifier)
        {
            VisibilityCondition zone =
                GetZone(zoneId);

            return zone != null &&
                   zone.SetSmokeModifier(
                       modifier);
        }

        public bool SetWeatherModifier(
            string zoneId,
            float modifier)
        {
            VisibilityCondition zone =
                GetZone(zoneId);

            return zone != null &&
                   zone.SetWeatherModifier(
                       modifier);
        }

        public float GetEffectiveVisibility(
            string zoneId)
        {
            VisibilityCondition zone =
                GetZone(zoneId);

            return zone == null
                ? 0f
                : zone.EffectiveVisibility;
        }

        public VisibilityCondition GetZone(
            string zoneId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(zoneId))
            {
                return null;
            }

            zones.TryGetValue(
                zoneId.Trim(),
                out VisibilityCondition zone);

            return zone;
        }

        public IReadOnlyCollection<
            VisibilityCondition>
            GetZones()
        {
            return zones.Values;
        }

        public void Reset()
        {
            zones.Clear();

            Initialized = false;
        }
    }
}
