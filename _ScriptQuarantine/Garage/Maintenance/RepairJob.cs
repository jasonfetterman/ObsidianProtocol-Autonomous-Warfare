using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [Serializable]
    public class RepairJob
    {
        [Header("Unit")]
        public string unitInstanceId;

        [Header("Repair")]
        [Min(0f)]
        public float repairAmount;

        [Min(0f)]
        public float durationHours;

        [Min(0f)]
        public float resourceCost;

        [Header("State")]
        public bool active;
        public bool completed;
        public bool cancelled;

        [Header("Progress")]
        [Range(0f, 1f)]
        public float progress;

        public void Start()
        {
            if (completed || cancelled)
                return;

            active = true;
        }

        public void UpdateProgress(float amount)
        {
            if (!active || completed || cancelled)
                return;

            progress = Mathf.Clamp01(
                progress + Mathf.Max(0f, amount));

            if (progress >= 1f)
                Complete();
        }

        public void Complete()
        {
            progress = 1f;
            active = false;
            completed = true;
        }

        public void Cancel()
        {
            active = false;
            cancelled = true;
        }
    }
}
