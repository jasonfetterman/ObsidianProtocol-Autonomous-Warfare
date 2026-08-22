using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum SalvageType
    {
        Vehicle,
        Drone,
        Naval,
        Equipment,
        Weapon,
        Structure,
        Wreckage
    }

    public enum SalvageState
    {
        Available,
        Assigned,
        Processing,
        Completed,
        Cancelled,
        Failed
    }

    public sealed class SalvageJob
    {
        public string JobId { get; }
        public string OwnerId { get; }
        public string TargetId { get; }
        public SalvageType SalvageType { get; }

        public float SalvageValue { get; }
        public float SalvageTime { get; }

        public float Progress { get; private set; }

        public SalvageState State { get; private set; }

        public bool Active =>
            State == SalvageState.Processing;

        public bool Completed =>
            State == SalvageState.Completed;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            !string.IsNullOrWhiteSpace(TargetId) &&
            SalvageValue >= 0f &&
            SalvageTime > 0f;

        public SalvageJob(
            string jobId,
            string ownerId,
            string targetId,
            SalvageType salvageType,
            float salvageValue,
            float salvageTime)
        {
            JobId = jobId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            TargetId = targetId ?? string.Empty;
            SalvageType = salvageType;

            SalvageValue = Mathf.Max(
                0f,
                salvageValue);

            SalvageTime = Mathf.Max(
                0.01f,
                salvageTime);

            Progress = 0f;
            State = SalvageState.Available;
        }

        public void Assign()
        {
            if (State == SalvageState.Available)
            {
                State = SalvageState.Assigned;
            }
        }

        public void Begin()
        {
            if (State == SalvageState.Assigned ||
                State == SalvageState.Available)
            {
                State = SalvageState.Processing;
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
                SalvageTime;

            Progress =
                Mathf.Clamp01(
                    Progress);

            if (Progress >= 1f)
            {
                State =
                    SalvageState.Completed;
            }
        }

        public void Cancel()
        {
            if (State == SalvageState.Available ||
                State == SalvageState.Assigned ||
                State == SalvageState.Processing)
            {
                State =
                    SalvageState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State == SalvageState.Available ||
                State == SalvageState.Assigned ||
                State == SalvageState.Processing)
            {
                State =
                    SalvageState.Failed;
            }
        }
    }

    public sealed class SalvageSystem
    {
        private readonly Dictionary<string, SalvageJob> jobs =
            new Dictionary<string, SalvageJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterJob(
            SalvageJob job)
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
            out SalvageJob job)
        {
            return jobs.TryGetValue(
                jobId,
                out job);
        }

        public bool AssignJob(
            string jobId)
        {
            if (!jobs.TryGetValue(
                    jobId,
                    out SalvageJob job))
            {
                return false;
            }

            job.Assign();

            return true;
        }

        public bool BeginJob(
            string jobId)
        {
            if (!jobs.TryGetValue(
                    jobId,
                    out SalvageJob job))
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
                    out SalvageJob job))
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
                SalvageJob job
                in jobs.Values)
            {
                job.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<SalvageJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public IReadOnlyCollection<SalvageJob>
            GetActiveJobs()
        {
            List<SalvageJob> active =
                new List<SalvageJob>();

            foreach (
                SalvageJob job
                in jobs.Values)
            {
                if (job.Active)
                {
                    active.Add(job);
                }
            }

            return active;
        }

        public IReadOnlyCollection<SalvageJob>
            GetCompletedJobs()
        {
            List<SalvageJob> completed =
                new List<SalvageJob>();

            foreach (
                SalvageJob job
                in jobs.Values)
            {
                if (job.Completed)
                {
                    completed.Add(job);
                }
            }

            return completed;
        }

        public float GetCompletedSalvageValue()
        {
            float total = 0f;

            foreach (
                SalvageJob job
                in jobs.Values)
            {
                if (job.Completed)
                {
                    total += job.SalvageValue;
                }
            }

            return total;
        }

        public int GetActiveSalvageCount()
        {
            int count = 0;

            foreach (
                SalvageJob job
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
