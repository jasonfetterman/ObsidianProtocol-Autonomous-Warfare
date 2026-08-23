using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class OnlineServerBuild
    {
        public bool Initialized { get; private set; }

        public bool ServerEnabled { get; private set; }

        public bool OnlineMode { get; private set; }

        public string Platform { get; private set; }

        public string BuildId { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            Platform = "DedicatedServer";
            BuildId = "OBSIDIAN-ONLINE-SERVER";
            ServerEnabled = true;
            OnlineMode = true;
            Initialized = true;

            return true;
        }

        public bool IsOnlineServerBuild()
        {
            return Initialized &&
                   ServerEnabled &&
                   OnlineMode &&
                   Platform == "DedicatedServer";
        }

        public string GetBuildId()
        {
            return BuildId;
        }

        public string GetPlatform()
        {
            return Platform;
        }

        public void Reset()
        {
            BuildId = null;
            Platform = null;
            ServerEnabled = false;
            OnlineMode = false;
            Initialized = false;
        }
    }
}
