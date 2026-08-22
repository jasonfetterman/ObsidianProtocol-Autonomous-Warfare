using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum FabricationStatus
    {
        Available,
        Queued,
        Fabricating,
        Complete,
        Cancelled,
        Failed
    }

    public sealed class FabricationJob
    {
        public string JobId { get; }
        public string BlueprintId { get; }
        public string UnitId { get; }

        public int Quantity { get; private set; }

        public float Progress { get; private set; }

        public FabricationStatus Status { get; private set; }

        public FabricationJob(
            string jobId,
            string blueprintId,
            string unitId,
            int quantity)
        {
            JobId =
                jobId ?? string.Empty;

            BlueprintId =
                blueprintId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            Quantity =
                Math.Max(1, quantity);

            Progress = 0f;
            Status = FabricationStatus.Available;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(JobId) &&
            !string.IsNullOrWhiteSpace(BlueprintId);

        public void Queue()
        {
            if (Status == FabricationStatus.Available)
                Status = FabricationStatus.Queued;
        }

        public void Begin()
        {
            if (Status == FabricationStatus.Queued)
                Status = FabricationStatus.Fabricating;
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
                Status = FabricationStatus.Complete;
        }

        public void Cancel()
        {
            if (Status == FabricationStatus.Queued ||
                Status == FabricationStatus.Fabricating)
            {
                Status = FabricationStatus.Cancelled;
            }
        }

        public void Fail()
        {
            Status = FabricationStatus.Failed;
        }

        public void Reset()
        {
            Progress = 0f;
            Status = FabricationStatus.Available;
        }
    }

    public sealed class FabricationFacility
    {
        private readonly Dictionary<
            string,
            FabricationJob> jobs =
            new Dictionary<
                string,
                FabricationJob>(
                StringComparer.OrdinalIgnoreCase);

        public int QueueCapacity { get; private set; }

        public FabricationFacility(
            int queueCapacity = 10)
        {
            QueueCapacity =
                Math.Max(1, queueCapacity);
        }

        public bool Register(
            FabricationJob job)
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
            out FabricationJob job)
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
                    out FabricationJob job))
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
                    out FabricationJob job))
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
                    out FabricationJob job))
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
                    out FabricationJob job))
            {
                return false;
            }

            job.Cancel();
            return true;
        }

        public IReadOnlyCollection<FabricationJob>
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
