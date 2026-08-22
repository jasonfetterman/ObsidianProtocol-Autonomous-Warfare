using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Construction
{
    public enum RepairType
    {
        Structural,
        Vehicle,
        Drone,
        Naval,
        Equipment,
        Weapon,
        Emergency
    }

    public enum RepairState
    {
        Queued,
        Repairing,
        Completed,
        Cancelled,
        Failed
    }

    public sealed class RepairJob
    {
        public string JobId { get; }
        public string OwnerId { get; }
        public string TargetId { get; }
        public RepairType RepairType { get; }

        public float RepairAmount { get; }
        public float RepairTime { get; }
        public float MaterialCost { get; }

        public float Progress { get; private set; }

        public RepairState State { get; private set; }

        public bool Active =>
            State == RepairState.Repairing;

        public bool Completed =>
            State == RepairState.Completed;

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnerId) &&
            !string.IsNullOrWhiteSpace(TargetId) &&
            RepairAmount > 0f &&
            RepairTime > 0f &&
            MaterialCost >= 0f;

        public RepairJob(
            string jobId,
            string ownerId,
            string targetId,
            RepairType repairType,
            float repairAmount,
            float repairTime,
            float materialCost)
        {
            JobId = jobId ?? string.Empty;
            OwnerId = ownerId ?? string.Empty;
            TargetId = targetId ?? string.Empty;
            RepairType = repairType;

            RepairAmount = Mathf.Max(
                0f,
                repairAmount);

            RepairTime = Mathf.Max(
                0.01f,
                repairTime);

            MaterialCost = Mathf.Max(
                0f,
                materialCost);

            Progress = 0f;
            State = RepairState.Queued;
        }

        public void Begin()
        {
            if (State == RepairState.Queued)
            {
                State = RepairState.Repairing;
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
                RepairTime;

            Progress =
                Mathf.Clamp01(
                    Progress);

            if (Progress >= 1f)
            {
                State =
                    RepairState.Completed;
            }
        }

        public void Cancel()
        {
            if (State == RepairState.Queued ||
                State == RepairState.Repairing)
            {
                State =
                    RepairState.Cancelled;
            }
        }

        public void Fail()
        {
            if (State == RepairState.Queued ||
                State == RepairState.Repairing)
            {
                State =
                    RepairState.Failed;
            }
        }
    }

    public sealed class RepairSystem
    {
        private readonly Dictionary<string, RepairJob> jobs =
            new Dictionary<string, RepairJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool RegisterJob(
            RepairJob job)
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
            out RepairJob job)
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
                    out RepairJob job))
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
                    out RepairJob job))
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
                RepairJob job
                in jobs.Values)
            {
                job.Update(deltaTime);
            }
        }

        public IReadOnlyCollection<RepairJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public IReadOnlyCollection<RepairJob>
            GetActiveJobs()
        {
            List<RepairJob> active =
                new List<RepairJob>();

            foreach (
                RepairJob job
                in jobs.Values)
            {
                if (job.Active)
                {
                    active.Add(job);
                }
            }

            return active;
        }

        public IReadOnlyCollection<RepairJob>
            GetCompletedJobs()
        {
            List<RepairJob> completed =
                new List<RepairJob>();

            foreach (
                RepairJob job
                in jobs.Values)
            {
                if (job.Completed)
                {
                    completed.Add(job);
                }
            }

            return completed;
        }

        public int GetActiveRepairCount()
        {
            int count = 0;

            foreach (
                RepairJob job
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
