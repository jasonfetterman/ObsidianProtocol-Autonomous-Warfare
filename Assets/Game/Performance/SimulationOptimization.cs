using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public enum SimulationPriority
    {
        Critical,
        High,
        Normal,
        Low,
        Background
    }

    public sealed class SimulationTask
    {
        public string TaskId { get; }

        public SimulationPriority Priority { get; private set; }

        public float UpdateInterval { get; private set; }

        public float TimeUntilUpdate { get; private set; }

        public bool Enabled { get; private set; }

        public SimulationTask(
            string taskId,
            SimulationPriority priority,
            float updateInterval)
        {
            TaskId =
                taskId ?? string.Empty;

            Priority =
                priority;

            UpdateInterval =
                Math.Max(
                    0.001f,
                    updateInterval);

            TimeUntilUpdate = 0f;

            Enabled = true;
        }

        public bool SetPriority(
            SimulationPriority priority)
        {
            Priority = priority;

            return true;
        }

        public bool SetUpdateInterval(
            float interval)
        {
            if (interval <= 0f)
            {
                return false;
            }

            UpdateInterval =
                interval;

            TimeUntilUpdate =
                Math.Min(
                    TimeUntilUpdate,
                    UpdateInterval);

            return true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }

        public bool Tick(
            float deltaTime)
        {
            if (!Enabled ||
                deltaTime < 0f)
            {
                return false;
            }

            TimeUntilUpdate -=
                deltaTime;

            if (TimeUntilUpdate > 0f)
            {
                return false;
            }

            TimeUntilUpdate =
                UpdateInterval;

            return true;
        }
    }

    public sealed class SimulationOptimization
    {
        private readonly Dictionary<
            string,
            SimulationTask> tasks =
            new Dictionary<
                string,
                SimulationTask>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TaskCount =>
            tasks.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            tasks.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterTask(
            string taskId,
            SimulationPriority priority,
            float updateInterval)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(taskId) ||
                updateInterval <= 0f)
            {
                return false;
            }

            string id =
                taskId.Trim();

            if (tasks.ContainsKey(id))
            {
                return false;
            }

            tasks.Add(
                id,
                new SimulationTask(
                    id,
                    priority,
                    updateInterval));

            return true;
        }

        public bool RemoveTask(
            string taskId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(taskId))
            {
                return false;
            }

            return tasks.Remove(
                taskId.Trim());
        }

        public bool SetTaskPriority(
            string taskId,
            SimulationPriority priority)
        {
            SimulationTask task =
                GetTask(taskId);

            return task != null &&
                   task.SetPriority(priority);
        }

        public bool SetTaskInterval(
            string taskId,
            float interval)
        {
            SimulationTask task =
                GetTask(taskId);

            return task != null &&
                   task.SetUpdateInterval(interval);
        }

        public bool SetTaskEnabled(
            string taskId,
            bool enabled)
        {
            SimulationTask task =
                GetTask(taskId);

            return task != null &&
                   task.SetEnabled(enabled);
        }

        public bool ShouldUpdate(
            string taskId,
            float deltaTime)
        {
            SimulationTask task =
                GetTask(taskId);

            return task != null &&
                   task.Tick(deltaTime);
        }

        public SimulationTask GetTask(
            string taskId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(taskId))
            {
                return null;
            }

            tasks.TryGetValue(
                taskId.Trim(),
                out SimulationTask task);

            return task;
        }

        public IReadOnlyCollection<SimulationTask>
            GetTasks()
        {
            return tasks.Values;
        }

        public void Reset()
        {
            tasks.Clear();

            Initialized = false;
        }
    }
}
