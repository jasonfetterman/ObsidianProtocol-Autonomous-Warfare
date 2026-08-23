using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class LaunchBuild
    {
        public bool Initialized { get; private set; }

        public bool LaunchEnabled { get; private set; }

        public bool ProductionBuild { get; private set; }

        public bool DevelopmentToolsDisabled { get; private set; }

        public string BuildId { get; private set; }

        public string ReleaseChannel { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            BuildId = "OBSIDIAN-LAUNCH";
            ReleaseChannel = "Production";

            LaunchEnabled = true;
            ProductionBuild = true;
            DevelopmentToolsDisabled = true;

            Initialized = true;

            return true;
        }

        public bool IsLaunchBuild()
        {
            return Initialized &&
                   LaunchEnabled &&
                   ProductionBuild &&
                   DevelopmentToolsDisabled &&
                   ReleaseChannel == "Production";
        }

        public string GetBuildId()
        {
            return BuildId;
        }

        public string GetReleaseChannel()
        {
            return ReleaseChannel;
        }

        public void Reset()
        {
            BuildId = null;
            ReleaseChannel = null;

            LaunchEnabled = false;
            ProductionBuild = false;
            DevelopmentToolsDisabled = false;

            Initialized = false;
        }
    }
}
