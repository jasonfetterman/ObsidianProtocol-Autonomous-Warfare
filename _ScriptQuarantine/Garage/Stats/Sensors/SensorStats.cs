using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class SensorStats
    {
        [Header("Visual Sensors")]
        [Min(0f)]
        public float visualRangeMeters;

        [Range(0f, 360f)]
        public float visualFieldOfViewDegrees = 120f;

        [Min(0f)]
        public float lowLightRangeMeters;

        [Header("Thermal")]
        public bool thermalEnabled;

        [Min(0f)]
        public float thermalRangeMeters;

        [Header("LIDAR")]
        public bool lidarEnabled;

        [Min(0f)]
        public float lidarRangeMeters;

        [Header("Radar")]
        public bool radarEnabled;

        [Min(0f)]
        public float radarRangeMeters;

        [Range(0f, 360f)]
        public float radarFieldOfViewDegrees = 360f;

        [Header("Acoustic / Sonar")]
        public bool acousticEnabled;

        [Min(0f)]
        public float acousticRangeMeters;

        [Header("Navigation")]
        public bool gpsEnabled;
        public bool inertialNavigationEnabled;
        public bool terrainMappingEnabled;

        [Header("Detection Quality")]
        [Range(0f, 1f)]
        public float detectionQuality = 1f;

        [Range(0f, 1f)]
        public float identificationQuality = 1f;

        [Range(0f, 1f)]
        public float trackingQuality = 1f;

        [Header("Environmental Performance")]
        [Range(0f, 1f)]
        public float fogPerformance = 1f;

        [Range(0f, 1f)]
        public float rainPerformance = 1f;

        [Range(0f, 1f)]
        public float dustPerformance = 1f;

        [Range(0f, 1f)]
        public float nightPerformance = 1f;
    }
}
