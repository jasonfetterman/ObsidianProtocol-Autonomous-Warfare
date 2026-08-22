using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum SalvageStatus
    {
        Available,
        Queued,
        Processing,
        Complete,
        Cancelled,
        Failed
    }

    public sealed class SalvageJob
    {
        public string JobId { get; }
        public string OwnershipId { get; }

        public int EstimatedRecovery { get; private set; }
        public int RecoveredAmount { get; private set; }

        public float Progress { get; private set; }

        public SalvageStatus Status { get; private set; }

        public SalvageJob(
            string jobId,
            string ownershipId,
            int estimatedRecovery)
        {
            JobId =
                jobId ?? string.Empty;

            OwnershipId =
                ownershipId ?? string.Empty;

            EstimatedRecovery =
                Math.Max(0, estimatedRecovery);

            RecoveredAmount = 0;
            Progress = 0f;

            Status = SalvageStatus.Available;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(OwnershipId);

        public void Queue()
        {
            if (Status == SalvageStatus.Available)
                Status = SalvageStatus.Queued;
        }

        public void Begin()
        {
            if (Status == SalvageStatus.Queued)
                Status = SalvageStatus.Processing;
        }

        public void SetProgress(float progress)
        {
            Progress =
                Math.Max(
                    0f,
                    Math.Min(
                        1f,
                        progress));

            RecoveredAmount =
                (int)Math.Round(
                    EstimatedRecovery *
                    Progress);

            if (Progress >= 1f)
                Status = SalvageStatus.Complete;
        }

        public void Cancel()
        {
            if (Status == SalvageStatus.Queued ||
                Status == SalvageStatus.Processing)
            {
                Status = SalvageStatus.Cancelled;
            }
        }

        public void Fail()
        {
            Status = SalvageStatus.Failed;
        }

        public void Reset()
        {
            RecoveredAmount = 0;
            Progress = 0f;
            Status = SalvageStatus.Available;
        }
    }

    public sealed class SalvageFacility
    {
        private readonly Dictionary<
            string,
            SalvageJob> jobs =
            new Dictionary<
                string,
                SalvageJob>(
                StringComparer.OrdinalIgnoreCase);

        public int QueueCapacity { get; private set; }

        public SalvageFacility(
            int queueCapacity = 10)
        {
            QueueCapacity =
                Math.Max(1, queueCapacity);
        }

        public bool Register(
            SalvageJob job)
        {
            if (job == null ||
                !job.Valid ||
                jobs.ContainsKey(job.JobId))
            {
                return false;
            }

            if (jobs.Count >= QueueCapacity)
                return false;

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
            out SalvageJob job)
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
                    out SalvageJob job))
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
                    out SalvageJob job))
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
                    out SalvageJob job))
            {
                return false;
            }

            job.SetProgress(progress);
            return true;
        }

        public bool Cancel(
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

        public int GetTotalRecovered()
        {
            int total = 0;

            foreach (SalvageJob job in jobs.Values)
            {
                if (job.Status ==
                    SalvageStatus.Complete)
                {
                    total += job.RecoveredAmount;
                }
            }

            return total;
        }

        public IReadOnlyCollection<SalvageJob>
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
