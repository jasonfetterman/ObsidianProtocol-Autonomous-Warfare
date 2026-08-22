using System;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum DedicatedServerState
    {
        Offline,
        Starting,
        Running,
        Stopping,
        Stopped,
        Faulted
    }

    public sealed class DedicatedServerArchitecture
    {
        public DedicatedServerState State { get; private set; }

        public bool Initialized { get; private set; }

        public bool Running =>
            State == DedicatedServerState.Running;

        public bool Start()
        {
            if (!Initialized)
            {
                State =
                    DedicatedServerState.Starting;

                Initialized = true;
            }

            if (State !=
                DedicatedServerState.Starting)
            {
                return false;
            }

            State =
                DedicatedServerState.Running;

            return true;
        }

        public bool Stop()
        {
            if (!Initialized ||
                State != DedicatedServerState.Running)
            {
                return false;
            }

            State =
                DedicatedServerState.Stopping;

            State =
                DedicatedServerState.Stopped;

            return true;
        }

        public bool Fault()
        {
            if (!Initialized)
            {
                return false;
            }

            State =
                DedicatedServerState.Faulted;

            return true;
        }

        public bool CanAcceptConnections()
        {
            return Initialized &&
                   State ==
                   DedicatedServerState.Running;
        }

        public bool HasServerAuthority()
        {
            return CanAcceptConnections();
        }

        public void Reset()
        {
            State =
                DedicatedServerState.Offline;

            Initialized = false;
        }
    }
}
