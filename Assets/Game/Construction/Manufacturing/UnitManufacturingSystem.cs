using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum UnitManufacturingState
    {
        Queued,
        Manufacturing,
        Completed,
        Cancelled,
        Failed
    }

    public sealed class UnitManufacturingJob
    {
        public string JobId { get; }
        public string OwnerId { get; }
        public string UnitId { get; }

        public float MaterialCost { get; }
        public float ManufacturingTime { get; }

        public float Progress { get; private set; }

        public UnitManufacturingState State { get; private set; }

        public bool Active =>
            State == UnitManufacturingState.Manufacturing;

        public bool Completed =>
            State == UnitManufacturingState.Completed;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            !string.IsNullOrWhiteSpace(UnitId) &&
            MaterialCost >= 0f &&
            ManufacturingTime > 0f;

        public UnitManufacturingJob(
            string jobId,
            string ownerId,
            string unitId,
            float materialCost,
            float manufacturingTime)
        {
            JobId = jobId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            UnitId = unitId ?? string.Empty;

            MaterialCost = Mathf.Max(
                0f,
                materialCost);

            ManufacturingTime = Mathf.Max(
                0.01f,
                manufacturingTime);

            Progress = 0f;
            State = UnitManufacturingState.Queued;
        }

        public void Begin()
        {
            if (State == UnitManufacturingState.Queued)
            {
                State = UnitManufacturingState.Manufacturing;
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
                    UnitManufacturingState.Completed;
            }
        }

        public void Cancel()
        {
            if (State == UnitManufacturingState.Queued ||
                State == UnitManufacturingState.Manufacturing)
            {
                State =
                    UnitManufacturingState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State == UnitManufacturingState.Queued ||
                State == UnitManufacturingState.Manufacturing)
            {
                State =
                    UnitManufacturingState.Failed;
            }
        }
    }

    public sealed class UnitManufacturingSystem
    {
        private readonly Dictionary<string, UnitManufacturingJob> jobs =
            new Dictionary<string, UnitManufacturingJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterJob(
            UnitManufacturingJob job)
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
            out UnitManufacturingJob job)
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
                    out UnitManufacturingJob job))
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
                    out UnitManufacturingJob job))
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
                UnitManufacturingJob job
                in jobs.Values)
            {
                job.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<UnitManufacturingJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public IReadOnlyCollection<UnitManufacturingJob>
            GetActiveJobs()
        {
            List<UnitManufacturingJob> active =
                new List<UnitManufacturingJob>();

            foreach (
                UnitManufacturingJob job
                in jobs.Values)
            {
                if (job.Active)
                {
                    active.Add(job);
                }
            }

            return active;
        }

        public IReadOnlyCollection<UnitManufacturingJob>
            GetCompletedJobs()
        {
            List<UnitManufacturingJob> completed =
                new List<UnitManufacturingJob>();

            foreach (
                UnitManufacturingJob job
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
                UnitManufacturingJob job
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
