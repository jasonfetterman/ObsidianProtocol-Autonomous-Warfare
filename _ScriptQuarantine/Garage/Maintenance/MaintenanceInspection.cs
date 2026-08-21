using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class MaintenanceInspection
    {
        public string unitInstanceId;

        [Range(0f, 1f)]
        public float mechanicalScore = 1f;

        [Range(0f, 1f)]
        public float electricalScore = 1f;

        [Range(0f, 1f)]
        public float structuralScore = 1f;

        [Range(0f, 1f)]
        public float sensorScore = 1f;

        [Range(0f, 1f)]
        public float mobilityScore = 1f;

        [Range(0f, 1f)]
        public float overallScore = 1f;

        public bool passed;
        public bool requiresRepair;

        public void Calculate()
        {
            overallScore =
                (mechanicalScore +
                 electricalScore +
                 structuralScore +
                 sensorScore +
                 mobilityScore) / 5f;

            passed = overallScore >= 0.75f;
            requiresRepair = overallScore < 0.90f;
        }
    }
}
