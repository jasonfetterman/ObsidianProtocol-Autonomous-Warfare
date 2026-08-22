using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum UpgradeStationType
    {
        Armor,
        Engine,
        Sensors,
        Weapons,
        Electronics,
        AI,
        Mobility,
        Energy
    }

    public enum UpgradeStatus
    {
        Available,
        Queued,
        Installing,
        Complete,
        Failed
    }

    public sealed class UpgradeJob
    {
        public string JobId { get; }
        public string OwnershipId { get; }
        public string UpgradeId { get; }
        public UpgradeStationType StationType { get; }

        public int TargetLevel { get; private set; }

        public float Progress { get; private set; }

        public UpgradeStatus Status { get; private set; }

        public UpgradeJob(
            string jobId,
            string ownershipId,
            string upgradeId,
            UpgradeStationType stationType,
            int targetLevel)
        {
            JobId =
                jobId ?? string.Empty;

            OwnershipId =
                ownershipId ?? string.Empty;

            UpgradeId =
                upgradeId ?? string.Empty;

            StationType = stationType;

            TargetLevel =
                Math.Max(1, targetLevel);

            Progress = 0f;

            Status = UpgradeStatus.Available;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnershipId) &&
            !string.IsNullOrWhiteSpace(UpgradeId);

        public void Queue()
        {
            if (Status == UpgradeStatus.Available)
                Status = UpgradeStatus.Queued;
        }

        public void Begin()
        {
            if (Status == UpgradeStatus.Queued)
                Status = UpgradeStatus.Installing;
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
                Status = UpgradeStatus.Complete;
        }

        public void Fail()
        {
            Status = UpgradeStatus.Failed;
        }

        public void Reset()
        {
            Progress = 0f;
            Status = UpgradeStatus.Available;
        }
    }

    public sealed class UpgradeStations
    {
        private readonly Dictionary<
            string,
            UpgradeJob> jobs =
            new Dictionary<
                string,
                UpgradeJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UpgradeJob job)
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
            out UpgradeJob job)
        {
            return jobs.TryGetValue(
                jobId,
                out job);
        }

        public bool Queue(
            string jobId)
        {
            if (!jobs.TryGetValue(
                    jobId,
                    out UpgradeJob job))
            {
                return false;
            }

            job.Queue();
            return true;
        }

        public bool Begin(
            string jobId)
        {
            if (!jobs.TryGetValue(
                    jobId,
                    out UpgradeJob job))
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
                    out UpgradeJob job))
            {
                return false;
            }

            job.SetProgress(progress);
            return true;
        }

        public IReadOnlyCollection<UpgradeJob>
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
