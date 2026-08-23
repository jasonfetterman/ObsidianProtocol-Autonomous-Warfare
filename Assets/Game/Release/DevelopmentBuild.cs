using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class DevelopmentBuild
    {
        public bool Initialized { get; private set; }

        public bool DevelopmentMode { get; private set; }

        public string BuildId { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            BuildId = "OBSIDIAN-DEV";
            DevelopmentMode = true;
            Initialized = true;

            return true;
        }

        public bool IsDevelopmentBuild()
        {
            return Initialized &&
                   DevelopmentMode;
        }

        public string GetBuildId()
        {
            return BuildId;
        }

        public void Reset()
        {
            BuildId = null;
            DevelopmentMode = false;
            Initialized = false;
        }
    }
}
