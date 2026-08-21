using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class MobilityStats
    {
        [Header("Movement")]
        [Min(0f)]
        public float acceleration;

        [Min(0f)]
        public float braking;

        [Min(0f)]
        public float turningRate;

        [Min(0f)]
        public float reverseSpeed;

        [Header("Terrain")]
        [Min(0f)]
        public float climbRate;

        [Min(0f)]
        public float descentRate;

        [Min(0f)]
        public float traction;

        [Min(0f)]
        public float terrainAdaptation;

        [Header("Environmental Mobility")]
        public bool amphibious;

        public bool hoverCapable;

        public bool verticalTakeoff;

        public bool underwaterCapable;

        [Header("Flight / Marine / Ground")]
        [Min(0f)]
        public float takeoffDistanceMeters;

        [Min(0f)]
        public float landingDistanceMeters;

        [Min(0f)]
        public float minimumOperationalSpeed;

        [Min(0f)]
        public float maximumOperationalSpeed;

        [Header("Handling")]
        [Range(0f, 1f)]
        public float stability = 1f;

        [Range(0f, 1f)]
        public float maneuverability = 1f;

        [Range(0f, 1f)]
        public float roughTerrainPerformance = 1f;
    }
}
