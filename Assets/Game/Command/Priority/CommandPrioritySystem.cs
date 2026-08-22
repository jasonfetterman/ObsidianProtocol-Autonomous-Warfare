using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command.Priority
{
    [Serializable]
    public sealed class CommandPriority
    {
        public int CommandId;
        public float Priority;

        public CommandPriority(
            int commandId,
            float priority)
        {
            CommandId = commandId;
            Priority = priority;
        }
    }

    public sealed class CommandPrioritySystem
    {
        private readonly Dictionary<int, float> priorities =
            new Dictionary<int, float>();

        public void SetPriority(
            int commandId,
            float priority)
        {
            priorities[commandId] =
                Math.Max(0f, priority);
        }

        public float GetPriority(int commandId)
        {
            if (priorities.TryGetValue(
                commandId,
                out float priority))
            {
                return priority;
            }

            return 0f;
        }

        public bool HasPriority(int commandId)
        {
            return priorities.ContainsKey(commandId);
        }

        public void RemovePriority(int commandId)
        {
            priorities.Remove(commandId);
        }

        public void Clear()
        {
            priorities.Clear();
        }
    }
}
