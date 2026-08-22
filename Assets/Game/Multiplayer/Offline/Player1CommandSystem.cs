using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum Player1CommandType
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

    public sealed class Player1Command
    {
        public string CommandId
        {
            get;
        }

        public Player1CommandType Type
        {
            get;
        }

        public string TargetId
        {
            get;
        }

        public float X
        {
            get;
        }

        public float Y
        {
            get;
        }

        public float Z
        {
            get;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                CommandId) &&
            Type !=
                Player1CommandType.None;

        public Player1Command(
            string commandId,
            Player1CommandType type,
            string targetId,
            float x,
            float y,
            float z)
        {
            CommandId =
                commandId ?? string.Empty;

            Type = type;

            TargetId =
                targetId ?? string.Empty;

            X = x;
            Y = y;
            Z = z;
        }
    }

    public sealed class Player1CommandSystem
    {
        private readonly List<
            Player1Command> commandQueue =
            new List<
                Player1Command>();

        private readonly HashSet<
            string> selectedUnits =
            new HashSet<
                string>(
                StringComparer.OrdinalIgnoreCase);

        private OfflineMultiplayerSession
            session;

        public OfflinePlayerId PlayerId =>
            OfflinePlayerId.Player1;

        public bool Active
        {
            get;
            private set;
        }

        public bool CommandAuthority
        {
            get;
            private set;
        }

        public int SelectedUnitCount =>
            selectedUnits.Count;

        public int QueuedCommandCount =>
            commandQueue.Count;

        public bool Initialize(
            OfflineMultiplayerSession
                offlineSession)
        {
            if (offlineSession == null ||
                !offlineSession.Valid)
            {
                return false;
            }

            OfflinePlayerContext player =
                offlineSession.GetPlayer(
                    OfflinePlayerId.Player1);

            if (player == null ||
                !player.Connected ||
                !player.CommandAuthority)
            {
                return false;
            }

            session =
                offlineSession;

            Active = true;
            CommandAuthority = true;

            return true;
        }

        public bool SelectUnit(
            string unitId)
        {
            if (!CanCommand() ||
                string.IsNullOrWhiteSpace(
                    unitId))
            {
                return false;
            }

            return selectedUnits.Add(
                unitId.Trim());
        }

        public bool DeselectUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(
                    unitId))
            {
                return false;
            }

            return selectedUnits.Remove(
                unitId.Trim());
        }

        public void ClearSelection()
        {
            selectedUnits.Clear();
        }

        public bool IssueCommand(
            Player1Command command)
        {
            if (!CanCommand() ||
                command == null ||
                !command.Valid)
            {
                return false;
            }

            if (commandQueue.Count >= 128)
                return false;

            commandQueue.Add(
                command);

            return true;
        }

        public bool IssueCommandToSelection(
            Player1CommandType type,
            string targetId,
            float x,
            float y,
            float z)
        {
            if (!CanCommand() ||
                selectedUnits.Count == 0 ||
                type ==
                    Player1CommandType.None)
            {
                return false;
            }

            string commandId =
                Guid.NewGuid()
                    .ToString("N");

            Player1Command command =
                new Player1Command(
                    commandId,
                    type,
                    targetId,
                    x,
                    y,
                    z);

            return IssueCommand(
                command);
        }

        public Player1Command
            DequeueCommand()
        {
            if (commandQueue.Count == 0)
                return null;

            Player1Command command =
                commandQueue[0];

            commandQueue.RemoveAt(0);

            return command;
        }

        public IReadOnlyCollection<
            string>
            GetSelectedUnits()
        {
            return selectedUnits;
        }

        public IReadOnlyCollection<
            Player1Command>
            GetQueuedCommands()
        {
            return commandQueue;
        }

        public bool StopCommanding()
        {
            if (!Active)
                return false;

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

            if (session.State !=
                    OfflineSessionState.Running &&
                session.State !=
                    OfflineSessionState.Ready)
            {
                return false;
            }

            OfflinePlayerContext player =
                session.GetPlayer(
                    OfflinePlayerId.Player1);

            return player != null &&
                   player.Connected &&
                   player.CommandAuthority;
        }
    }
}
