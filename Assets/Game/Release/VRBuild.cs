using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class VRBuild
    {
        public bool Initialized { get; private set; }

        public bool VREnabled { get; private set; }

        public string Platform { get; private set; }

        public string BuildId { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            Platform = "VR";
            BuildId = "OBSIDIAN-VR";
            VREnabled = true;
            Initialized = true;

            return true;
        }

        public bool IsVRBuild()
        {
            return Initialized &&
                   VREnabled &&
                   Platform == "VR";
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
            VREnabled = false;
            Initialized = false;
        }
    }
}
