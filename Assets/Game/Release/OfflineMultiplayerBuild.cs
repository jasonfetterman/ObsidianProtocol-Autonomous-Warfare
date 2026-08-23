using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class OfflineMultiplayerBuild
    {
        public bool Initialized { get; private set; }

        public bool MultiplayerEnabled { get; private set; }

        public bool OfflineMode { get; private set; }

        public string Platform { get; private set; }

        public string BuildId { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            Platform = "PC";
            BuildId = "OBSIDIAN-OFFLINE-MP";
            MultiplayerEnabled = true;
            OfflineMode = true;
            Initialized = true;

            return true;
        }

        public bool IsOfflineMultiplayerBuild()
        {
            return Initialized &&
                   MultiplayerEnabled &&
                   OfflineMode;
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
            MultiplayerEnabled = false;
            OfflineMode = false;
            Initialized = false;
        }
    }
}
