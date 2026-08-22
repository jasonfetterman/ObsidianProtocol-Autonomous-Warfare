using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum ConstructionTaskType
    {
        Build,
        Repair,
        Fortify,
        ClearObstacle,
        Salvage
    }

    public sealed class ConstructionTask
    {
        public string TaskId { get; }
        public ConstructionTaskType Type { get; }

        public float Progress { get; private set; }
        public float WorkRate { get; private set; }

        public bool Active { get; private set; }
        public bool Complete { get; private set; }

        public ConstructionTask(
            string taskId,
            ConstructionTaskType type)
        {
            TaskId =
                taskId ?? string.Empty;

            Type =
                type;

            Active = false;
            Complete = false;
        }

        public void Start(
            float workRate)
        {
            WorkRate =
                Math.Max(
                    0f,
                    workRate);

            Active = true;
            Complete = false;
        }

        public void Update(
            float deltaTime)
        {
            if (!Active ||
                Complete)
            {
                return;
            }

            Progress +=
                WorkRate *
                Math.Max(
                    0f,
                    deltaTime);

            if (Progress >= 1f)
            {
                Progress = 1f;
                Complete = true;
                Active = false;
            }
        }

        public void Cancel()
        {
            Active = false;
        }
    }

    public sealed class ConstructionVehicleSystem
    {
        private readonly Dictionary<string, ConstructionTask> tasks =
            new Dictionary<string, ConstructionTask>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterTask(
            string taskId,
            ConstructionTaskType type)
        {
            if (string.IsNullOrWhiteSpace(taskId))
            {
                return;
            }

            tasks[taskId] =
                new ConstructionTask(
                    taskId,
                    type);
        }

        public void StartTask(
            string taskId,
            float workRate)
        {
            if (tasks.TryGetValue(
                    taskId,
                    out ConstructionTask task))
            {
                task.Start(workRate);
            }
        }

        public void UpdateTask(
            string taskId,
            float deltaTime)
        {
            if (tasks.TryGetValue(
                    taskId,
                    out ConstructionTask task))
            {
                task.Update(deltaTime);
            }
        }

        public bool IsComplete(
            string taskId)
        {
            return tasks.TryGetValue(
                       taskId,
                       out ConstructionTask task) &&
                   task.Complete;
        }

        public float GetProgress(
            string taskId)
        {
            return tasks.TryGetValue(
                       taskId,
                       out ConstructionTask task)
                ? task.Progress
                : 0f;
        }

        public void CancelTask(
            string taskId)
        {
            if (tasks.TryGetValue(
                    taskId,
                    out ConstructionTask task))
            {
                task.Cancel();
            }
        }

        public void RemoveTask(
            string taskId)
        {
            tasks.Remove(taskId);
        }

        public void Clear()
        {
            tasks.Clear();
        }
    }
}
