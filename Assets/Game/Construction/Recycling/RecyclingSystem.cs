using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum RecyclingType
    {
        Vehicle,
        Drone,
        Naval,
        Equipment,
        Weapon,
        Component,
        Material,
        Structure
    }

    public enum RecyclingState
    {
        Queued,
        Processing,
        Completed,
        Cancelled,
        Failed
    }

    public sealed class RecyclingJob
    {
        public string JobId { get; }
        public string OwnerId { get; }
        public string SourceId { get; }
        public RecyclingType RecyclingType { get; }

        public float InputValue { get; }
        public float RecoveredValue { get; }
        public float RecyclingTime { get; }

        public float Progress { get; private set; }

        public RecyclingState State { get; private set; }

        public bool Active =>
            State == RecyclingState.Processing;

        public bool Completed =>
            State == RecyclingState.Completed;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            !string.IsNullOrWhiteSpace(SourceId) &&
            InputValue > 0f &&
            RecoveredValue >= 0f &&
            RecyclingTime > 0f;

        public RecyclingJob(
            string jobId,
            string ownerId,
            string sourceId,
            RecyclingType recyclingType,
            float inputValue,
            float recoveredValue,
            float recyclingTime)
        {
            JobId = jobId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            SourceId = sourceId ?? string.Empty;
            RecyclingType = recyclingType;

            InputValue = Mathf.Max(
                0f,
                inputValue);

            RecoveredValue = Mathf.Max(
                0f,
                recoveredValue);

            RecyclingTime = Mathf.Max(
                0.01f,
                recyclingTime);

            Progress = 0f;
            State = RecyclingState.Queued;
        }

        public void Begin()
        {
            if (State == RecyclingState.Queued)
            {
                State = RecyclingState.Processing;
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
                RecyclingTime;

            Progress =
                Mathf.Clamp01(
                    Progress);

            if (Progress >= 1f)
            {
                State =
                    RecyclingState.Completed;
            }
        }

        public void Cancel()
        {
            if (State == RecyclingState.Queued ||
                State == RecyclingState.Processing)
            {
                State =
                    RecyclingState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State == RecyclingState.Queued ||
                State == RecyclingState.Processing)
            {
                State =
                    RecyclingState.Failed;
            }
        }
    }

    public sealed class RecyclingSystem
    {
        private readonly Dictionary<string, RecyclingJob> jobs =
            new Dictionary<string, RecyclingJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterJob(
            RecyclingJob job)
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
            out RecyclingJob job)
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
                    out RecyclingJob job))
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
                    out RecyclingJob job))
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
                RecyclingJob job
                in jobs.Values)
            {
                job.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<RecyclingJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public IReadOnlyCollection<RecyclingJob>
            GetActiveJobs()
        {
            List<RecyclingJob> active =
                new List<RecyclingJob>();

            foreach (
                RecyclingJob job
                in jobs.Values)
            {
                if (job.Active)
                {
                    active.Add(job);
                }
            }

            return active;
        }

        public IReadOnlyCollection<RecyclingJob>
            GetCompletedJobs()
        {
            List<RecyclingJob> completed =
                new List<RecyclingJob>();

            foreach (
                RecyclingJob job
                in jobs.Values)
            {
                if (job.Completed)
                {
                    completed.Add(job);
                }
            }

            return completed;
        }

        public float GetRecoveredValue()
        {
            float total = 0f;

            foreach (
                RecyclingJob job
                in jobs.Values)
            {
                if (job.Completed)
                {
                    total += job.RecoveredValue;
                }
            }

            return total;
        }

        public int GetActiveRecyclingCount()
        {
            int count = 0;

            foreach (
                RecyclingJob job
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
