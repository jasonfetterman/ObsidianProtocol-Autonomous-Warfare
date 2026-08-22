using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Offline
{
    public enum OfflinePlayerId
    {
        None,
        Player1,
        Player2
    }

    public enum OfflinePlayerRelationship
    {
        Unassigned,
        Cooperative,
        Competitive
    }

    public enum OfflineSessionState
    {
        None,
        Initializing,
        Ready,
        Running,
        Paused,
        Completed,
        Closed
    }

    public sealed class OfflinePlayerContext
    {
        public OfflinePlayerId PlayerId
        {
            get;
        }

        public string PlayerName
        {
            get;
            private set;
        }

        public bool Connected
        {
            get;
            private set;
        }

        public bool CommandAuthority
        {
            get;
            private set;
        }

        public OfflinePlayerContext(
            OfflinePlayerId playerId)
        {
            PlayerId = playerId;
            PlayerName = string.Empty;
            Connected = false;
            CommandAuthority = false;
        }

        public bool Configure(
            string playerName)
        {
            if (PlayerId ==
                    OfflinePlayerId.None ||
                string.IsNullOrWhiteSpace(
                    playerName))
            {
                return false;
            }

            PlayerName =
                playerName.Trim();

            Connected = true;
            CommandAuthority = true;

            return true;
        }

        public void Disconnect()
        {
            Connected = false;
            CommandAuthority = false;
        }
    }

    public sealed class OfflineSessionConfiguration
    {
        public OfflinePlayerRelationship Relationship
        {
            get;
            private set;
        }

        public bool SharedWorld
        {
            get;
            private set;
        }

        public bool SharedBattlefield
        {
            get;
            private set;
        }

        public bool OfflineAIEnabled
        {
            get;
            private set;
        }

        public bool SaveEnabled
        {
            get;
            private set;
        }

        public bool Valid =>
            Relationship !=
                OfflinePlayerRelationship.Unassigned &&
            SharedWorld &&
            SharedBattlefield;

        public OfflineSessionConfiguration()
        {
            Relationship =
                OfflinePlayerRelationship.Unassigned;

            SharedWorld = true;
            SharedBattlefield = true;
            OfflineAIEnabled = true;
            SaveEnabled = true;
        }

        public bool Configure(
            OfflinePlayerRelationship relationship)
        {
            if (relationship ==
                    OfflinePlayerRelationship.Unassigned)
            {
                return false;
            }

            Relationship = relationship;

            return true;
        }
    }

    public sealed class OfflineMultiplayerSession
    {
        private readonly Dictionary<
            OfflinePlayerId,
            OfflinePlayerContext> players =
            new Dictionary<
                OfflinePlayerId,
                OfflinePlayerContext>();

        public OfflineSessionState State
        {
            get;
            private set;
        }

        public OfflineSessionConfiguration
            Configuration
        {
            get;
            private set;
        }

        public string SessionId
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                SessionId) &&
            Configuration != null &&
            Configuration.Valid;

        public bool BothPlayersReady =>
            HasReadyPlayer(
                OfflinePlayerId.Player1) &&
            HasReadyPlayer(
                OfflinePlayerId.Player2);

        public OfflineMultiplayerSession()
        {
            State =
                OfflineSessionState.None;

            SessionId = string.Empty;

            Configuration =
                new OfflineSessionConfiguration();

            players.Add(
                OfflinePlayerId.Player1,
                new OfflinePlayerContext(
                    OfflinePlayerId.Player1));

            players.Add(
                OfflinePlayerId.Player2,
                new OfflinePlayerContext(
                    OfflinePlayerId.Player2));
        }

        public bool Initialize(
            string sessionId,
            OfflinePlayerRelationship
                relationship)
        {
            if (State !=
                    OfflineSessionState.None &&
                State !=
                    OfflineSessionState.Closed)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                    sessionId))
            {
                return false;
            }

            if (!Configuration.Configure(
                    relationship))
            {
                return false;
            }

            SessionId =
                sessionId.Trim();

            State =
                OfflineSessionState.Initializing;

            return true;
        }

        public bool ConfigurePlayer(
            OfflinePlayerId playerId,
            string playerName)
        {
            if (State !=
                    OfflineSessionState.Initializing &&
                State !=
                    OfflineSessionState.Ready)
            {
                return false;
            }

            if (!players.TryGetValue(
                    playerId,
                    out OfflinePlayerContext
                        player))
            {
                return false;
            }

            if (!player.Configure(
                    playerName))
            {
                return false;
            }

            if (BothPlayersReady)
            {
                State =
                    OfflineSessionState.Ready;
            }

            return true;
        }

        public bool Start()
        {
            if (!Valid ||
                !BothPlayersReady ||
                State !=
                    OfflineSessionState.Ready)
            {
                return false;
            }

            State =
                OfflineSessionState.Running;

            return true;
        }

        public bool Pause()
        {
            if (State !=
                    OfflineSessionState.Running)
            {
                return false;
            }

            State =
                OfflineSessionState.Paused;

            return true;
        }

        public bool Resume()
        {
            if (State !=
                    OfflineSessionState.Paused)
            {
                return false;
            }

            State =
                OfflineSessionState.Running;

            return true;
        }

        public bool Complete()
        {
            if (State !=
                    OfflineSessionState.Running &&
                State !=
                    OfflineSessionState.Paused)
            {
                return false;
            }

            State =
                OfflineSessionState.Completed;

            return true;
        }

        public bool Close()
        {
            if (State ==
                    OfflineSessionState.None ||
                State ==
                    OfflineSessionState.Closed)
            {
                return false;
            }

            foreach (OfflinePlayerContext player
                     in players.Values)
            {
                player.Disconnect();
            }

            State =
                OfflineSessionState.Closed;

            return true;
        }

        public bool HasPlayer(
            OfflinePlayerId playerId)
        {
            return players.ContainsKey(
                playerId);
        }

        public bool HasReadyPlayer(
            OfflinePlayerId playerId)
        {
            return players.TryGetValue(
                       playerId,
                       out OfflinePlayerContext
                           player) &&
                   player.Connected &&
                   player.CommandAuthority;
        }

        public OfflinePlayerContext
            GetPlayer(
                OfflinePlayerId playerId)
        {
            if (players.TryGetValue(
                    playerId,
                    out OfflinePlayerContext
                        player))
            {
                return player;
            }

            return null;
        }

        public IReadOnlyCollection<
            OfflinePlayerContext>
            GetPlayers()
        {
            return players.Values;
        }
    }

    public sealed class OfflineMultiplayerManager
    {
        private readonly List<
            OfflineMultiplayerSession> sessions =
            new List<
                OfflineMultiplayerSession>();

        public OfflineMultiplayerSession
            CreateSession(
                string sessionId,
                OfflinePlayerRelationship
                    relationship)
        {
            if (string.IsNullOrWhiteSpace(
                    sessionId))
            {
                return null;
            }

            OfflineMultiplayerSession session =
                new OfflineMultiplayerSession();

            if (!session.Initialize(
                    sessionId,
                    relationship))
            {
                return null;
            }

            sessions.Add(session);

            return session;
        }

        public bool RemoveSession(
            OfflineMultiplayerSession session)
        {
            if (session == null)
                return false;

            session.Close();

            return sessions.Remove(
                session);
        }

        public OfflineMultiplayerSession
            FindSession(
                string sessionId)
        {
            if (string.IsNullOrWhiteSpace(
                    sessionId))
            {
                return null;
            }

            foreach (
                OfflineMultiplayerSession session
                in sessions)
            {
                if (string.Equals(
                        session.SessionId,
                        sessionId,
                        StringComparison
                            .OrdinalIgnoreCase))
                {
                    return session;
                }
            }

            return null;
        }

        public IReadOnlyCollection<
            OfflineMultiplayerSession>
            GetSessions()
        {
            return sessions;
        }

        public void Clear()
        {
            foreach (
                OfflineMultiplayerSession session
                in sessions)
            {
                session.Close();
            }

            sessions.Clear();
        }
    }
}
