using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum ConstructionStatus
    {
        Planned,
        Building,
        Completed,
        Cancelled
    }

    public sealed class VerticalSliceConstructionJob
    {
        public string JobId { get; }

        public string StructureType { get; }

        public int RequiredResources { get; }

        public int CurrentResources { get; private set; }

        public ConstructionStatus Status { get; private set; }

        public VerticalSliceConstructionJob(
            string jobId,
            string structureType,
            int requiredResources)
        {
            JobId =
                jobId ?? string.Empty;

            StructureType =
                structureType ?? string.Empty;

            RequiredResources =
                Math.Max(
                    0,
                    requiredResources);

            CurrentResources = 0;

            Status =
                ConstructionStatus.Planned;
        }

        public bool Begin()
        {
            if (Status != ConstructionStatus.Planned)
            {
                return false;
            }

            Status =
                ConstructionStatus.Building;

            return true;
        }

        public bool AddResources(
            int amount)
        {
            if (Status != ConstructionStatus.Building ||
                amount <= 0)
            {
                return false;
            }

            CurrentResources =
                Math.Min(
                    RequiredResources,
                    CurrentResources + amount);

            if (CurrentResources >=
                RequiredResources)
            {
                Status =
                    ConstructionStatus.Completed;
            }

            return true;
        }

        public bool Cancel()
        {
            if (Status == ConstructionStatus.Completed ||
                Status == ConstructionStatus.Cancelled)
            {
                return false;
            }

            Status =
                ConstructionStatus.Cancelled;

            return true;
        }
    }

    public sealed class VerticalSliceConstruction
    {
        private readonly Dictionary<
            string,
            VerticalSliceConstructionJob> jobs =
            new Dictionary<
                string,
                VerticalSliceConstructionJob>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int JobCount =>
            jobs.Count;

        public int CompletedJobCount
        {
            get
            {
                int count = 0;

                foreach (VerticalSliceConstructionJob job
                         in jobs.Values)
                {
                    if (job.Status ==
                        ConstructionStatus.Completed)
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
            string structureType,
            int requiredResources)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(jobId) ||
                string.IsNullOrWhiteSpace(structureType) ||
                requiredResources < 0)
            {
                return false;
            }

            string id =
                jobId.Trim();

            if (jobs.ContainsKey(id))
            {
                return false;
            }

            jobs.Add(
                id,
                new VerticalSliceConstructionJob(
                    id,
                    structureType.Trim(),
                    requiredResources));

            return true;
        }

        public bool BeginJob(
            string jobId)
        {
            VerticalSliceConstructionJob job =
                GetJob(jobId);

            return job != null &&
                   job.Begin();
        }

        public bool AddResources(
            string jobId,
            int amount)
        {
            VerticalSliceConstructionJob job =
                GetJob(jobId);

            return job != null &&
                   job.AddResources(amount);
        }

        public bool CancelJob(
            string jobId)
        {
            VerticalSliceConstructionJob job =
                GetJob(jobId);

            return job != null &&
                   job.Cancel();
        }

        public VerticalSliceConstructionJob GetJob(
            string jobId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(jobId))
            {
                return null;
            }

            jobs.TryGetValue(
                jobId.Trim(),
                out VerticalSliceConstructionJob job);

            return job;
        }

        public IReadOnlyCollection<
            VerticalSliceConstructionJob>
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
