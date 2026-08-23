using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class BackgroundSimulationTask
    {
        public string TaskId { get; }

        public float SimulationInterval { get; private set; }

        public float TimeUntilSimulation { get; private set; }

        public bool Enabled { get; private set; }

        public BackgroundSimulationTask(
            string taskId,
            float simulationInterval)
        {
            TaskId =
                taskId ?? string.Empty;

            SimulationInterval =
                Math.Max(
                    0.001f,
                    simulationInterval);

            TimeUntilSimulation = 0f;

            Enabled = true;
        }

        public bool SetInterval(
            float interval)
        {
            if (interval <= 0f)
            {
                return false;
            }

            SimulationInterval =
                interval;

            TimeUntilSimulation =
                Math.Min(
                    TimeUntilSimulation,
                    SimulationInterval);

            return true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled =
                enabled;

            return true;
        }

        public bool ShouldSimulate(
            float deltaTime)
        {
            if (!Enabled ||
                deltaTime < 0f)
            {
                return false;
            }

            TimeUntilSimulation -=
                deltaTime;

            if (TimeUntilSimulation > 0f)
            {
                return false;
            }

            TimeUntilSimulation =
                SimulationInterval;

            return true;
        }
    }

    public sealed class BackgroundSimulation
    {
        private readonly Dictionary<
            string,
            BackgroundSimulationTask> tasks =
            new Dictionary<
                string,
                BackgroundSimulationTask>(
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
            float simulationInterval)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(taskId) ||
                simulationInterval <= 0f)
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
                new BackgroundSimulationTask(
                    id,
                    simulationInterval));

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

        public bool SetInterval(
            string taskId,
            float interval)
        {
            BackgroundSimulationTask task =
                GetTask(taskId);

            return task != null &&
                   task.SetInterval(interval);
        }

        public bool SetEnabled(
            string taskId,
            bool enabled)
        {
            BackgroundSimulationTask task =
                GetTask(taskId);

            return task != null &&
                   task.SetEnabled(enabled);
        }

        public bool ShouldSimulate(
            string taskId,
            float deltaTime)
        {
            BackgroundSimulationTask task =
                GetTask(taskId);

            return task != null &&
                   task.ShouldSimulate(deltaTime);
        }

        public int UpdateAll(
            float deltaTime)
        {
            if (!Initialized ||
                deltaTime < 0f)
            {
                return 0;
            }

            int simulationCount = 0;

            foreach (BackgroundSimulationTask task
                     in tasks.Values)
            {
                if (task.ShouldSimulate(deltaTime))
                {
                    simulationCount++;
                }
            }

            return simulationCount;
        }

        public BackgroundSimulationTask GetTask(
            string taskId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(taskId))
            {
                return null;
            }

            tasks.TryGetValue(
                taskId.Trim(),
                out BackgroundSimulationTask task);

            return task;
        }

        public IReadOnlyCollection<
            BackgroundSimulationTask>
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
