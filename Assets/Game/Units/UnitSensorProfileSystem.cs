using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitSensorProfile
    {
        public string UnitId { get; }

        public float VisualRange { get; private set; }
        public float AudioRange { get; private set; }
        public float ThermalRange { get; private set; }
        public float RadarRange { get; private set; }
        public float LidarRange { get; private set; }

        public float DetectionQuality { get; private set; }
        public float IdentificationQuality { get; private set; }

        public bool HasVisual { get; private set; }
        public bool HasAudio { get; private set; }
        public bool HasThermal { get; private set; }
        public bool HasRadar { get; private set; }
        public bool HasLidar { get; private set; }

        public UnitSensorProfile(string unitId)
        {
            UnitId = unitId ?? string.Empty;
        }

        public void Configure(
            float visualRange,
            float audioRange,
            float thermalRange,
            float radarRange,
            float lidarRange,
            float detectionQuality,
            float identificationQuality,
            bool hasVisual,
            bool hasAudio,
            bool hasThermal,
            bool hasRadar,
            bool hasLidar)
        {
            VisualRange = Math.Max(0f, visualRange);
            AudioRange = Math.Max(0f, audioRange);
            ThermalRange = Math.Max(0f, thermalRange);
            RadarRange = Math.Max(0f, radarRange);
            LidarRange = Math.Max(0f, lidarRange);

            DetectionQuality =
                Math.Clamp(detectionQuality, 0f, 1f);

            IdentificationQuality =
                Math.Clamp(identificationQuality, 0f, 1f);

            HasVisual = hasVisual;
            HasAudio = hasAudio;
            HasThermal = hasThermal;
            HasRadar = hasRadar;
            HasLidar = hasLidar;
        }
    }

    public sealed class UnitSensorProfileSystem
    {
        private readonly Dictionary<string, UnitSensorProfile> profiles =
            new Dictionary<string, UnitSensorProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new UnitSensorProfile(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float visualRange,
            float audioRange,
            float thermalRange,
            float radarRange,
            float lidarRange,
            float detectionQuality,
            float identificationQuality,
            bool hasVisual,
            bool hasAudio,
            bool hasThermal,
            bool hasRadar,
            bool hasLidar)
        {
            RegisterUnit(unitId);

            profiles[unitId].Configure(
                visualRange,
                audioRange,
                thermalRange,
                radarRange,
                lidarRange,
                detectionQuality,
                identificationQuality,
                hasVisual,
                hasAudio,
                hasThermal,
                hasRadar,
                hasLidar);
        }

        public bool TryGetProfile(
            string unitId,
            out UnitSensorProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public void RemoveUnit(string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}
