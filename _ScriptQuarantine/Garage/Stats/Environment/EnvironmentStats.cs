using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class EnvironmentStats
    {
        [Header("Temperature")]
        [Min(0f)] public float minimumOperatingTemperature;
        [Min(0f)] public float maximumOperatingTemperature;
        [Min(0f)] public float thermalTolerance;

        [Header("Weather")]
        [Range(0f, 1f)] public float rainResistance;
        [Range(0f, 1f)] public float snowResistance;
        [Range(0f, 1f)] public float dustResistance;
        [Range(0f, 1f)] public float sandResistance;
        [Range(0f, 1f)] public float fogResistance;

        [Header("Terrain")]
        [Range(0f, 1f)] public float mudResistance;
        [Range(0f, 1f)] public float iceResistance;
        [Range(0f, 1f)] public float looseTerrainResistance;
        [Range(0f, 1f)] public float roughTerrainResistance;

        [Header("Water")]
        [Range(0f, 1f)] public float waterResistance;
        [Min(0f)] public float maximumWaterDepth;

        [Header("Operational Conditions")]
        [Range(0f, 1f)] public float nightPerformance;
        [Range(0f, 1f)] public float lowVisibilityPerformance;
        [Range(0f, 1f)] public float extremeWeatherPerformance;

        [Header("Environmental Protection")]
        [Range(0f, 1f)] public float corrosionResistance;
        [Range(0f, 1f)] public float sealing;
        [Range(0f, 1f)] public float environmentalHardening;

        public bool canOperateInRain;
        public bool canOperateInSnow;
        public bool canOperateInDust;
        public bool canOperateInExtremeWeather;
        public bool canOperateInWater;
    }
}
