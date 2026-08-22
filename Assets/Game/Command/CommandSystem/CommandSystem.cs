using System;
using UnityEngine;

namespace ObsidianProtocol.Game.Command
{
    public enum CommandType
    {
        None,
        Move,
        Attack,
        Defend,
        Hold,
        Patrol,
        Escort,
        Recon,
        Capture,
        Reinforce,
        Retreat,
        Regroup,
        Follow,
        Support
    }

    [Serializable]
    public sealed class Command
    {
        public CommandType Type;
        public Vector3 Position;
        public GameObject Target;
        public float Priority = 1f;

        public Command(
            CommandType type,
            Vector3 position)
        {
            Type = type;
            Position = position;
        }

        public Command(
            CommandType type,
            Vector3 position,
            GameObject target)
        {
            Type = type;
            Position = position;
            Target = target;
        }
    }

    public sealed class CommandSystem
    {
        public event Action<Command> CommandIssued;

        public Command CurrentCommand { get; private set; }

        public bool HasCommand =>
            CurrentCommand != null &&
            CurrentCommand.Type != CommandType.None;

        public void IssueCommand(Command command)
        {
            if (command == null)
            {
                return;
            }

            CurrentCommand = command;
            CommandIssued?.Invoke(command);
        }

        public void ClearCommand()
        {
            CurrentCommand = null;
        }
    }
}
