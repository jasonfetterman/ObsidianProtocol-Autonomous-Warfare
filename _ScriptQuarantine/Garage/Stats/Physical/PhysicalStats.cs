using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class PhysicalStats
    {
        [Header("Dimensions")]
        [Min(0f)]
        public float heightMeters;

        [Min(0f)]
        public float lengthMeters;

        [Min(0f)]
        public float widthMeters;

        [Header("Mass")]
        [Min(0f)]
        public float weightKg;

        [Header("Mobility Limits")]
        [Min(0f)]
        public float maxSpeed;

        [Min(0f)]
        public float maxAngleDegrees;

        [Header("Classification")]
        public MobilityUnitType mobilityType;
        public SpeedUnit speedUnit;
    }

    public enum MobilityUnitType
    {
        Air,
        Ground,
        Sea,
        Command,
        Experimental
    }

    public enum SpeedUnit
    {
        KilometersPerHour,
        Knots
    }
}
