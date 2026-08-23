using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSliceCommandType
    {
        Move,
        Attack,
        Defend,
        Flank,
        Retreat,
        Reinforce,
        Hold,
        Stop
    }

    public sealed class VerticalSliceCommand
    {
        public string CommandId { get; }

        public string SquadId { get; }

        public VerticalSliceCommandType Type { get; }

        public bool Executed { get; private set; }

        public VerticalSliceCommand(
            string commandId,
            string squadId,
            VerticalSliceCommandType type)
        {
            CommandId =
                commandId ?? string.Empty;

            SquadId =
                squadId ?? string.Empty;

            Type =
                type;

            Executed = false;
        }

        public bool Execute()
        {
            if (Executed)
            {
                return false;
            }

            Executed = true;

            return true;
        }
    }

    public sealed class VerticalSliceRTSCommandUI
    {
        private readonly Dictionary<
            string,
            VerticalSliceCommand> commands =
            new Dictionary<
                string,
                VerticalSliceCommand>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Visible { get; private set; }

        public int CommandCount =>
            commands.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            commands.Clear();

            Visible = false;
            Initialized = true;

            return true;
        }

        public bool Show()
        {
            if (!Initialized)
            {
                return false;
            }

            Visible = true;

            return true;
        }

        public bool Hide()
        {
            if (!Initialized)
            {
                return false;
            }

            Visible = false;

            return true;
        }

        public bool IssueCommand(
            string commandId,
            string squadId,
            VerticalSliceCommandType type)
        {
            if (!Initialized ||
                !Visible ||
                string.IsNullOrWhiteSpace(commandId) ||
                string.IsNullOrWhiteSpace(squadId))
            {
                return false;
            }

            string id =
                commandId.Trim();

            if (commands.ContainsKey(id))
            {
                return false;
            }

            commands.Add(
                id,
                new VerticalSliceCommand(
                    id,
                    squadId.Trim(),
                    type));

            return true;
        }

        public bool ExecuteCommand(
            string commandId)
        {
            VerticalSliceCommand command =
                GetCommand(commandId);

            return command != null &&
                   command.Execute();
        }

        public VerticalSliceCommand GetCommand(
            string commandId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(commandId))
            {
                return null;
            }

            commands.TryGetValue(
                commandId.Trim(),
                out VerticalSliceCommand command);

            return command;
        }

        public IReadOnlyCollection<
            VerticalSliceCommand>
            GetCommands()
        {
            return commands.Values;
        }

        public void Reset()
        {
            commands.Clear();

            Visible = false;
            Initialized = false;
        }
    }
}
