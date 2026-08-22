using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum Player2CommandType
    {
        None,
        Move,
        Attack,
        Defend,
        Hold,
        Reconnaissance,
        Reinforce,
        Retreat,
        Follow,
        Patrol,
        Deploy,
        Stop
    }

    public sealed class Player2Command
    {
        public string CommandId { get; }

        public Player2CommandType Type { get; }

        public string TargetId { get; }

        public float X { get; }

        public float Y { get; }

        public float Z { get; }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(CommandId) &&
            Type != Player2CommandType.None;

        public Player2Command(
            string commandId,
            Player2CommandType type,
            string targetId,
            float x,
            float y,
            float z)
        {
            CommandId = commandId ?? string.Empty;
            Type = type;
            TargetId = targetId ?? string.Empty;
            X = x;
            Y = y;
            Z = z;
        }
    }

    public sealed class Player2CommandSystem
    {
        private readonly List<Player2Command> commandQueue =
            new List<Player2Command>();

        private readonly HashSet<string> selectedUnits =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        private OfflineMultiplayerSession session;

        public OfflinePlayerId PlayerId =>
            OfflinePlayerId.Player2;

        public bool Active { get; private set; }

        public bool CommandAuthority { get; private set; }

        public int SelectedUnitCount =>
            selectedUnits.Count;

        public int QueuedCommandCount =>
            commandQueue.Count;

        public bool Initialize(
            OfflineMultiplayerSession offlineSession)
        {
            if (offlineSession == null ||
                !offlineSession.Valid)
            {
                return false;
            }

            OfflinePlayerContext player =
                offlineSession.GetPlayer(
                    OfflinePlayerId.Player2);

            if (player == null ||
                !player.Connected ||
                !player.CommandAuthority)
            {
                return false;
            }

            session = offlineSession;
            Active = true;
            CommandAuthority = true;

            return true;
        }

        public bool SelectUnit(string unitId)
        {
            if (!CanCommand() ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return selectedUnits.Add(unitId.Trim());
        }

        public bool DeselectUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return selectedUnits.Remove(unitId.Trim());
        }

        public void ClearSelection()
        {
            selectedUnits.Clear();
        }

        public bool IssueCommand(Player2Command command)
        {
            if (!CanCommand() ||
                command == null ||
                !command.Valid)
            {
                return false;
            }

            if (commandQueue.Count >= 128)
            {
                return false;
            }

            commandQueue.Add(command);

            return true;
        }

        public bool IssueCommandToSelection(
            Player2CommandType type,
            string targetId,
            float x,
            float y,
            float z)
        {
            if (!CanCommand() ||
                selectedUnits.Count == 0 ||
                type == Player2CommandType.None)
            {
                return false;
            }

            Player2Command command =
                new Player2Command(
                    Guid.NewGuid().ToString("N"),
                    type,
                    targetId,
                    x,
                    y,
                    z);

            return IssueCommand(command);
        }

        public Player2Command DequeueCommand()
        {
            if (commandQueue.Count == 0)
            {
                return null;
            }

            Player2Command command =
                commandQueue[0];

            commandQueue.RemoveAt(0);

            return command;
        }

        public IReadOnlyCollection<string>
            GetSelectedUnits()
        {
            return selectedUnits;
        }

        public IReadOnlyCollection<Player2Command>
            GetQueuedCommands()
        {
            return commandQueue;
        }

        public bool StopCommanding()
        {
            if (!Active)
            {
                return false;
            }

            commandQueue.Clear();
            selectedUnits.Clear();

            Active = false;
            CommandAuthority = false;

            return true;
        }

        public void ClearCommands()
        {
            commandQueue.Clear();
        }

        private bool CanCommand()
        {
            if (!Active ||
                !CommandAuthority ||
                session == null)
            {
                return false;
            }

            if (session.State != OfflineSessionState.Running &&
                session.State != OfflineSessionState.Ready)
            {
                return false;
            }

            OfflinePlayerContext player =
                session.GetPlayer(
                    OfflinePlayerId.Player2);

            return player != null &&
                   player.Connected &&
                   player.CommandAuthority;
        }
    }
}
