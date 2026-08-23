using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum ProductionStatus
    {
        Queued,
        Producing,
        Completed,
        Cancelled
    }

    public sealed class VerticalSliceProductionJob
    {
        public string JobId { get; }

        public string UnitType { get; }

        public int RequiredResources { get; }

        public int ConsumedResources { get; private set; }

        public ProductionStatus Status { get; private set; }

        public VerticalSliceProductionJob(
            string jobId,
            string unitType,
            int requiredResources)
        {
            JobId = jobId ?? string.Empty;
            UnitType = unitType ?? string.Empty;
            RequiredResources = Math.Max(0, requiredResources);
            ConsumedResources = 0;
            Status = ProductionStatus.Queued;
        }

        public bool Begin()
        {
            if (Status != ProductionStatus.Queued)
            {
                return false;
            }

            Status = ProductionStatus.Producing;

            return true;
        }

        public bool AddResources(int amount)
        {
            if (Status != ProductionStatus.Producing ||
                amount <= 0)
            {
                return false;
            }

            ConsumedResources =
                Math.Min(
                    RequiredResources,
                    ConsumedResources + amount);

            if (ConsumedResources >= RequiredResources)
            {
                Status = ProductionStatus.Completed;
            }

            return true;
        }

        public bool Cancel()
        {
            if (Status == ProductionStatus.Completed ||
                Status == ProductionStatus.Cancelled)
            {
                return false;
            }

            Status = ProductionStatus.Cancelled;

            return true;
        }
    }

    public sealed class VerticalSliceProduction
    {
        private readonly Dictionary<
            string,
            VerticalSliceProductionJob> jobs =
            new Dictionary<
                string,
                VerticalSliceProductionJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int JobCount => jobs.Count;

        public int CompletedJobCount
        {
            get
            {
                int count = 0;

                foreach (VerticalSliceProductionJob job
                         in jobs.Values)
                {
                    if (job.Status ==
                        ProductionStatus.Completed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            jobs.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateJob(
            string jobId,
            string unitType,
            int requiredResources)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(jobId) ||
                string.IsNullOrWhiteSpace(unitType) ||
                requiredResources < 0)
            {
                return false;
            }

            string id = jobId.Trim();

            if (jobs.ContainsKey(id))
            {
                return false;
            }

            jobs.Add(
                id,
                new VerticalSliceProductionJob(
                    id,
                    unitType.Trim(),
                    requiredResources));

            return true;
        }

        public bool BeginJob(string jobId)
        {
            VerticalSliceProductionJob job =
                GetJob(jobId);

            return job != null &&
                   job.Begin();
        }

        public bool AddResources(
            string jobId,
            int amount)
        {
            VerticalSliceProductionJob job =
                GetJob(jobId);

            return job != null &&
                   job.AddResources(amount);
        }

        public bool CancelJob(string jobId)
        {
            VerticalSliceProductionJob job =
                GetJob(jobId);

            return job != null &&
                   job.Cancel();
        }

        public VerticalSliceProductionJob GetJob(
            string jobId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(jobId))
            {
                return null;
            }

            jobs.TryGetValue(
                jobId.Trim(),
                out VerticalSliceProductionJob job);

            return job;
        }

        public IReadOnlyCollection<
            VerticalSliceProductionJob>
            GetJobs()
        {
            return jobs.Values;
        }

        public void Reset()
        {
            jobs.Clear();
            Initialized = false;
        }
    }
}
