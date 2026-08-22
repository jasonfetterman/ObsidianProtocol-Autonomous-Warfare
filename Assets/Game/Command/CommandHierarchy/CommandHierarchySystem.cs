using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Command.Hierarchy
{
    public sealed class CommandHierarchySystem
    {
        private readonly Dictionary<int, int> parentCommands =
            new Dictionary<int, int>();

        private int nextCommandId = 1;

        public int CreateRootCommand()
        {
            return nextCommandId++;
        }

        public int CreateChildCommand(int parentCommandId)
        {
            if (!parentCommands.ContainsKey(parentCommandId) &&
                parentCommandId <= 0)
            {
                return -1;
            }

            int commandId = nextCommandId++;
            parentCommands[commandId] = parentCommandId;

            return commandId;
        }

        public bool HasParent(int commandId)
        {
            return parentCommands.ContainsKey(commandId);
        }

        public bool TryGetParent(
            int commandId,
            out int parentCommandId)
        {
            return parentCommands.TryGetValue(
                commandId,
                out parentCommandId);
        }

        public void RemoveCommand(int commandId)
        {
            parentCommands.Remove(commandId);
        }

        public void Clear()
        {
            parentCommands.Clear();
            nextCommandId = 1;
        }
    }
}
