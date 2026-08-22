using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum EquipmentManufacturingType
    {
        Weapon,
        Armor,
        Sensor,
        Communications,
        Mobility,
        Utility,
        Ammunition,
        Module
    }

    public enum EquipmentManufacturingState
    {
        Queued,
        Manufacturing,
        Completed,
        Cancelled,
        Failed
    }

    public sealed class EquipmentManufacturingJob
    {
        public string JobId { get; }
        public string OwnerId { get; }
        public string EquipmentId { get; }
        public EquipmentManufacturingType EquipmentType { get; }

        public float MaterialCost { get; }
        public float ManufacturingTime { get; }

        public float Progress { get; private set; }

        public EquipmentManufacturingState State { get; private set; }

        public bool Active =>
            State == EquipmentManufacturingState.Manufacturing;

        public bool Completed =>
            State == EquipmentManufacturingState.Completed;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            !string.IsNullOrWhiteSpace(EquipmentId) &&
            MaterialCost >= 0f &&
            ManufacturingTime > 0f;

        public EquipmentManufacturingJob(
            string jobId,
            string ownerId,
            string equipmentId,
            EquipmentManufacturingType equipmentType,
            float materialCost,
            float manufacturingTime)
        {
            JobId = jobId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            EquipmentId = equipmentId ?? string.Empty;
            EquipmentType = equipmentType;

            MaterialCost = Mathf.Max(
                0f,
                materialCost);

            ManufacturingTime = Mathf.Max(
                0.01f,
                manufacturingTime);

            Progress = 0f;
            State = EquipmentManufacturingState.Queued;
        }

        public void Begin()
        {
            if (State == EquipmentManufacturingState.Queued)
            {
                State =
                    EquipmentManufacturingState.Manufacturing;
            }
        }

        public void Update(
            float deltaTime)
        {
            if (!Active ||
                deltaTime <= 0f)
            {
                return;
            }

            Progress +=
                deltaTime /
                ManufacturingTime;

            Progress =
                Mathf.Clamp01(
                    Progress);

            if (Progress >= 1f)
            {
                State =
                    EquipmentManufacturingState.Completed;
            }
        }

        public void Cancel()
        {
            if (State == EquipmentManufacturingState.Queued ||
                State == EquipmentManufacturingState.Manufacturing)
            {
                State =
                    EquipmentManufacturingState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State == EquipmentManufacturingState.Queued ||
                State == EquipmentManufacturingState.Manufacturing)
            {
                State =
                    EquipmentManufacturingState.Failed;
            }
        }
    }

    public sealed class EquipmentManufacturingSystem
    {
        private readonly Dictionary<string, EquipmentManufacturingJob> jobs =
            new Dictionary<string, EquipmentManufacturingJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterJob(
            EquipmentManufacturingJob job)
        {
            if (job == null ||
                !job.Valid ||
                jobs.ContainsKey(job.JobId))
            {
                return false;
            }

            jobs.Add(
                job.JobId,
                job);

            return true;
        }

        public bool RemoveJob(
            string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return false;
            }

            return jobs.Remove(jobId);
        }

        public bool TryGetJob(
            string jobId,
            out EquipmentManufacturingJob job)
        {
            return jobs.TryGetValue(
                jobId,
                out job);
        }

        public bool BeginJob(
            string jobId)
        {
            if (!jobs.TryGetValue(
                    jobId,
                    out EquipmentManufacturingJob job))
            {
                return false;
            }

            job.Begin();

            return true;
        }

        public bool CancelJob(
            string jobId)
        {
            if (!jobs.TryGetValue(
                    jobId,
                    out EquipmentManufacturingJob job))
            {
                return false;
            }

            job.Cancel();

            return true;
        }

        public void Update(
            float deltaTime)
        {
            foreach (
                EquipmentManufacturingJob job
                in jobs.Values)
            {
                job.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<EquipmentManufacturingJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public IReadOnlyCollection<EquipmentManufacturingJob>
            GetActiveJobs()
        {
            List<EquipmentManufacturingJob> active =
                new List<EquipmentManufacturingJob>();

            foreach (
                EquipmentManufacturingJob job
                in jobs.Values)
            {
                if (job.Active)
                {
                    active.Add(job);
                }
            }

            return active;
        }

        public IReadOnlyCollection<EquipmentManufacturingJob>
            GetCompletedJobs()
        {
            List<EquipmentManufacturingJob> completed =
                new List<EquipmentManufacturingJob>();

            foreach (
                EquipmentManufacturingJob job
                in jobs.Values)
            {
                if (job.Completed)
                {
                    completed.Add(job);
                }
            }

            return completed;
        }

        public int GetActiveManufacturingCount()
        {
            int count = 0;

            foreach (
                EquipmentManufacturingJob job
                in jobs.Values)
            {
                if (job.Active)
                {
                    count++;
                }
            }

            return count;
        }

        public void Clear()
        {
            jobs.Clear();
        }
    }
}
