using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum RepairStationType
    {
        General,
        Armor,
        Structural,
        Electronics,
        Propulsion,
        Drone,
        Vehicle,
        Naval,
        Air
    }

    public enum RepairStatus
    {
        Idle,
        Queued,
        Repairing,
        Complete,
        Failed
    }

    public sealed class RepairJob
    {
        public string JobId { get; }
        public string OwnershipId { get; }
        public RepairStationType StationType { get; }

        public float Progress { get; private set; }

        public RepairStatus Status { get; private set; }

        public RepairJob(
            string jobId,
            string ownershipId,
            RepairStationType stationType)
        {
            JobId =
                jobId ?? string.Empty;

            OwnershipId =
                ownershipId ?? string.Empty;

            StationType = stationType;

            Progress = 0f;
            Status = RepairStatus.Queued;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnershipId);

        public void Begin()
        {
            if (Status == RepairStatus.Queued)
                Status = RepairStatus.Repairing;
        }

        public void SetProgress(float progress)
        {
            Progress =
                Math.Max(
                    0f,
                    Math.Min(
                        1f,
                        progress));

            if (Progress >= 1f)
                Status = RepairStatus.Complete;
        }

        public void Fail()
        {
            Status = RepairStatus.Failed;
        }

        public void Reset()
        {
            Progress = 0f;
            Status = RepairStatus.Queued;
        }
    }

    public sealed class RepairStations
    {
        private readonly Dictionary<
            string,
            RepairJob> jobs =
            new Dictionary<
                string,
                RepairJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
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

        public bool Remove(
            string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
                return false;

            return jobs.Remove(jobId);
        }

        public bool TryGet(
            string jobId,
            out RepairJob job)
        {
            return jobs.TryGetValue(
                jobId,
                out job);
        }

        public bool Begin(
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

        public bool SetProgress(
            string jobId,
            float progress)
        {
            if (!jobs.TryGetValue(
                    jobId,
                    out RepairJob job))
            {
                return false;
            }

            job.SetProgress(progress);
            return true;
        }

        public IReadOnlyCollection<RepairJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public void Clear()
        {
            jobs.Clear();
        }
    }
}
