using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class RepairQueueManager : MonoBehaviour
    {
        [Header("Repair Queue")]
        [SerializeField]
        private List<RepairJob> jobs =
            new List<RepairJob>();

        public IReadOnlyList<RepairJob> Jobs =>
            jobs;

        public int QueueCount =>
            jobs.Count;

        public RepairJob QueueRepair(
            string unitInstanceId,
            float repairAmount,
            float durationHours,
            float resourceCost)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return null;

            RepairJob job = new RepairJob
            {
                unitInstanceId = unitInstanceId,
                repairAmount = Mathf.Max(0f, repairAmount),
                durationHours = Mathf.Max(0f, durationHours),
                resourceCost = Mathf.Max(0f, resourceCost)
            };

            jobs.Add(job);

            return job;
        }

        public void StartNext()
        {
            foreach (RepairJob job in jobs)
            {
                if (job == null)
                    continue;

                if (job.completed || job.cancelled)
                    continue;

                if (!job.active)
                {
                    job.Start();
                    return;
                }
            }
        }

        public void UpdateActiveJob(float progressAmount)
        {
            foreach (RepairJob job in jobs)
            {
                if (job == null || !job.active)
                    continue;

                job.UpdateProgress(progressAmount);
                return;
            }
        }

        public void CancelJob(RepairJob job)
        {
            if (job == null)
                return;

            job.Cancel();
        }

        public void RemoveFinishedJobs()
        {
            jobs.RemoveAll(job =>
                job == null ||
                job.completed ||
                job.cancelled);
        }

        public void ClearQueue()
        {
            jobs.Clear();
        }
    }
}
