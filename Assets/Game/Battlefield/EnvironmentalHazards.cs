using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum EnvironmentalHazardType
    {
        None,
        Flood,
        Fire,
        ToxicZone,
        Radiation,
        ExtremeHeat,
        ExtremeCold,
        UnstableGround
    }

    public sealed class EnvironmentalHazard
    {
        public string HazardId { get; }

        public EnvironmentalHazardType Type { get; }

        public float Intensity { get; private set; }

        public float Radius { get; }

        public bool Active { get; private set; }

        public EnvironmentalHazard(
            string hazardId,
            EnvironmentalHazardType type,
            float intensity,
            float radius)
        {
            HazardId =
                hazardId ?? string.Empty;

            Type = type;

            Intensity =
                ClampIntensity(intensity);

            Radius =
                Math.Max(0f, radius);

            Active =
                Type != EnvironmentalHazardType.None &&
                Intensity > 0f &&
                Radius > 0f;
        }

        public bool Activate()
        {
            if (Type == EnvironmentalHazardType.None ||
                Intensity <= 0f ||
                Radius <= 0f)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;

            return true;
        }

        public bool SetIntensity(
            float intensity)
        {
            Intensity =
                ClampIntensity(intensity);

            if (Intensity <= 0f)
            {
                Active = false;
            }

            return true;
        }

        private static float ClampIntensity(
            float intensity)
        {
            return Math.Max(
                0f,
                Math.Min(1f, intensity));
        }
    }

    public sealed class EnvironmentalHazards
    {
        private readonly Dictionary<
            string,
            EnvironmentalHazard> hazards =
            new Dictionary<
                string,
                EnvironmentalHazard>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int HazardCount =>
            hazards.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            hazards.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterHazard(
            string hazardId,
            EnvironmentalHazardType type,
            float intensity,
            float radius)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(hazardId) ||
                type == EnvironmentalHazardType.None ||
                intensity <= 0f ||
                radius <= 0f)
            {
                return false;
            }

            string id =
                hazardId.Trim();

            if (hazards.ContainsKey(id))
            {
                return false;
            }

            hazards.Add(
                id,
                new EnvironmentalHazard(
                    id,
                    type,
                    intensity,
                    radius));

            return true;
        }

        public bool ActivateHazard(
            string hazardId)
        {
            EnvironmentalHazard hazard =
                GetHazard(hazardId);

            return hazard != null &&
                   hazard.Activate();
        }

        public bool DeactivateHazard(
            string hazardId)
        {
            EnvironmentalHazard hazard =
                GetHazard(hazardId);

            return hazard != null &&
                   hazard.Deactivate();
        }

        public bool SetHazardIntensity(
            string hazardId,
            float intensity)
        {
            EnvironmentalHazard hazard =
                GetHazard(hazardId);

            return hazard != null &&
                   hazard.SetIntensity(intensity);
        }

        public EnvironmentalHazard GetHazard(
            string hazardId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(hazardId))
            {
                return null;
            }

            hazards.TryGetValue(
                hazardId.Trim(),
                out EnvironmentalHazard hazard);

            return hazard;
        }

        public IReadOnlyCollection<
            EnvironmentalHazard>
            GetHazards()
        {
            return hazards.Values;
        }

        public void Reset()
        {
            hazards.Clear();

            Initialized = false;
        }
    }
}
