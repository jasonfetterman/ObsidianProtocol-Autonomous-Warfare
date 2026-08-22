using System;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineNetworkRole
    {
        None,
        Client,
        Server,
        Host
    }

    public enum OnlineSessionState
    {
        Offline,
        Connecting,
        Connected,
        Disconnecting,
        Disconnected
    }

    public sealed class OnlineMultiplayerArchitecture
    {
        public OnlineNetworkRole Role { get; private set; }

        public OnlineSessionState State { get; private set; }

        public bool Initialized { get; private set; }

        public bool IsNetworkActive =>
            State == OnlineSessionState.Connected;

        public bool Initialize(
            OnlineNetworkRole role)
        {
            if (Initialized ||
                role == OnlineNetworkRole.None)
            {
                return false;
            }

            Role = role;
            State = OnlineSessionState.Offline;
            Initialized = true;

            return true;
        }

        public bool BeginConnection()
        {
            if (!Initialized ||
                State != OnlineSessionState.Offline)
            {
                return false;
            }

            State = OnlineSessionState.Connecting;

            return true;
        }

        public bool CompleteConnection()
        {
            if (!Initialized ||
                State != OnlineSessionState.Connecting)
            {
                return false;
            }

            State = OnlineSessionState.Connected;

            return true;
        }

        public bool BeginDisconnect()
        {
            if (!Initialized ||
                State != OnlineSessionState.Connected)
            {
                return false;
            }

            State = OnlineSessionState.Disconnecting;

            return true;
        }

        public bool CompleteDisconnect()
        {
            if (!Initialized ||
                State != OnlineSessionState.Disconnecting)
            {
                return false;
            }

            State = OnlineSessionState.Disconnected;

            return true;
        }

        public bool CanTransmit()
        {
            return Initialized &&
                   State == OnlineSessionState.Connected;
        }

        public bool HasServerAuthority()
        {
            return Initialized &&
                   (Role == OnlineNetworkRole.Server ||
                    Role == OnlineNetworkRole.Host);
        }

        public bool HasClientAuthority()
        {
            return Initialized &&
                   (Role == OnlineNetworkRole.Client ||
                    Role == OnlineNetworkRole.Host);
        }

        public void Reset()
        {
            Role = OnlineNetworkRole.None;
            State = OnlineSessionState.Offline;
            Initialized = false;
        }
    }
}
