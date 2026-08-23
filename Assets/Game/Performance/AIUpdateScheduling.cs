using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Performance
{
    public sealed class AIScheduledUpdate
    {
        public string AgentId { get; }

        public float UpdateInterval { get; private set; }

        public float TimeUntilUpdate { get; private set; }

        public bool Enabled { get; private set; }

        public AIScheduledUpdate(
            string agentId,
            float updateInterval)
        {
            AgentId =
                agentId ?? string.Empty;

            UpdateInterval =
                Math.Max(
                    0.001f,
                    updateInterval);

            TimeUntilUpdate = 0f;

            Enabled = true;
        }

        public bool SetInterval(
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

    public sealed class AIUpdateScheduling
    {
        private readonly Dictionary<
            string,
            AIScheduledUpdate> agents =
            new Dictionary<
                string,
                AIScheduledUpdate>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AgentCount =>
            agents.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            agents.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterAgent(
            string agentId,
            float updateInterval)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(agentId) ||
                updateInterval <= 0f)
            {
                return false;
            }

            string id =
                agentId.Trim();

            if (agents.ContainsKey(id))
            {
                return false;
            }

            agents.Add(
                id,
                new AIScheduledUpdate(
                    id,
                    updateInterval));

            return true;
        }

        public bool RemoveAgent(
            string agentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(agentId))
            {
                return false;
            }

            return agents.Remove(
                agentId.Trim());
        }

        public bool SetUpdateInterval(
            string agentId,
            float interval)
        {
            AIScheduledUpdate agent =
                GetAgent(agentId);

            return agent != null &&
                   agent.SetInterval(interval);
        }

        public bool SetEnabled(
            string agentId,
            bool enabled)
        {
            AIScheduledUpdate agent =
                GetAgent(agentId);

            return agent != null &&
                   agent.SetEnabled(enabled);
        }

        public bool ShouldUpdate(
            string agentId,
            float deltaTime)
        {
            AIScheduledUpdate agent =
                GetAgent(agentId);

            return agent != null &&
                   agent.Tick(deltaTime);
        }

        public int UpdateAll(
            float deltaTime)
        {
            if (!Initialized ||
                deltaTime <= 0f)
            {
                return 0;
            }

            int updateCount = 0;

            foreach (AIScheduledUpdate agent
                     in agents.Values)
            {
                if (agent.Tick(deltaTime))
                {
                    updateCount++;
                }
            }

            return updateCount;
        }

        public AIScheduledUpdate GetAgent(
            string agentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(agentId))
            {
                return null;
            }

            agents.TryGetValue(
                agentId.Trim(),
                out AIScheduledUpdate agent);

            return agent;
        }

        public IReadOnlyCollection<
            AIScheduledUpdate>
            GetAgents()
        {
            return agents.Values;
        }

        public void Reset()
        {
            agents.Clear();

            Initialized = false;
        }
    }
}
