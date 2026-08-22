using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum FabricationType
    {
        General,
        Vehicle,
        Drone,
        Naval,
        Equipment,
        Weapon,
        Component
    }

    public enum FabricationState
    {
        Queued,
        Fabricating,
        Completed,
        Cancelled,
        Failed
    }

    public sealed class FabricationJob
    {
        public string JobId { get; }
        public string OwnerId { get; }
        public string BlueprintId { get; }
        public FabricationType FabricationType { get; }

        public float MaterialCost { get; }
        public float FabricationTime { get; }

        public float Progress { get; private set; }

        public FabricationState State { get; private set; }

        public bool Active =>
            State == FabricationState.Fabricating;

        public bool Completed =>
            State == FabricationState.Completed;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            !string.IsNullOrWhiteSpace(BlueprintId) &&
            MaterialCost >= 0f &&
            FabricationTime > 0f;

        public FabricationJob(
            string jobId,
            string ownerId,
            string blueprintId,
            FabricationType fabricationType,
            float materialCost,
            float fabricationTime)
        {
            JobId = jobId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            BlueprintId = blueprintId ?? string.Empty;
            FabricationType = fabricationType;

            MaterialCost = Mathf.Max(
                0f,
                materialCost);

            FabricationTime = Mathf.Max(
                0.01f,
                fabricationTime);

            Progress = 0f;
            State = FabricationState.Queued;
        }

        public void Begin()
        {
            if (State == FabricationState.Queued)
            {
                State = FabricationState.Fabricating;
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
                FabricationTime;

            Progress =
                Mathf.Clamp01(
                    Progress);

            if (Progress >= 1f)
            {
                State =
                    FabricationState.Completed;
            }
        }

        public void Cancel()
        {
            if (State == FabricationState.Queued ||
                State == FabricationState.Fabricating)
            {
                State =
                    FabricationState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State == FabricationState.Queued ||
                State == FabricationState.Fabricating)
            {
                State =
                    FabricationState.Failed;
            }
        }
    }

    public sealed class FabricationSystem
    {
        private readonly Dictionary<string, FabricationJob> jobs =
            new Dictionary<string, FabricationJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterJob(
            FabricationJob job)
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
            out FabricationJob job)
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
                    out FabricationJob job))
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
                    out FabricationJob job))
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
                FabricationJob job
                in jobs.Values)
            {
                job.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<FabricationJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public IReadOnlyCollection<FabricationJob>
            GetActiveJobs()
        {
            List<FabricationJob> active =
                new List<FabricationJob>();

            foreach (
                FabricationJob job
                in jobs.Values)
            {
                if (job.Active)
                {
                    active.Add(job);
                }
            }

            return active;
        }

        public IReadOnlyCollection<FabricationJob>
            GetCompletedJobs()
        {
            List<FabricationJob> completed =
                new List<FabricationJob>();

            foreach (
                FabricationJob job
                in jobs.Values)
            {
                if (job.Completed)
                {
                    completed.Add(job);
                }
            }

            return completed;
        }

        public void Clear()
        {
            jobs.Clear();
        }
    }
}
