using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class ServiceBayState
    {
        [Header("Bay")]
        public string bayId;
        public string bayName;

        [Header("Occupancy")]
        public string activeUnitInstanceId;
        public bool occupied;

        [Header("Operations")]
        public bool servicing;
        public bool inspecting;
        public bool available = true;

        [Header("Progress")]
        [Range(0f, 1f)]
        public float serviceProgress;

        [Range(0f, 1f)]
        public float inspectionProgress;

        public void AssignUnit(string unitInstanceId)
        {
            activeUnitInstanceId = unitInstanceId;
            occupied = !string.IsNullOrWhiteSpace(unitInstanceId);
            available = !occupied;
        }

        public void ClearUnit()
        {
            activeUnitInstanceId = string.Empty;
            occupied = false;
            servicing = false;
            inspecting = false;
            serviceProgress = 0f;
            inspectionProgress = 0f;
            available = true;
        }

        public void BeginService()
        {
            if (!occupied)
                return;

            servicing = true;
            inspecting = false;
        }

        public void BeginInspection()
        {
            if (!occupied)
                return;

            inspecting = true;
            servicing = false;
        }

        public void SetServiceProgress(float value)
        {
            serviceProgress =
                Mathf.Clamp01(value);

            if (serviceProgress >= 1f)
                servicing = false;
        }

        public void SetInspectionProgress(float value)
        {
            inspectionProgress =
                Mathf.Clamp01(value);

            if (inspectionProgress >= 1f)
                inspecting = false;
        }
    }
}
