using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class PCBuild
    {
        public bool Initialized { get; private set; }

        public bool PCEnabled { get; private set; }

        public string Platform { get; private set; }

        public string BuildId { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            Platform = "PC";
            BuildId = "OBSIDIAN-PC";
            PCEnabled = true;
            Initialized = true;

            return true;
        }

        public bool IsPCBuild()
        {
            return Initialized &&
                   PCEnabled &&
                   Platform == "PC";
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
            PCEnabled = false;
            Initialized = false;
        }
    }
}
