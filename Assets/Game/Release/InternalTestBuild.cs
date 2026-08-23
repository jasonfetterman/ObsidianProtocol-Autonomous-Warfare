using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class InternalTestBuild
    {
        public bool Initialized { get; private set; }

        public bool InternalTestingEnabled { get; private set; }

        public bool DebugToolsEnabled { get; private set; }

        public string BuildId { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            BuildId = "OBSIDIAN-INTERNAL";
            InternalTestingEnabled = true;
            DebugToolsEnabled = true;
            Initialized = true;

            return true;
        }

        public bool IsInternalBuild()
        {
            return Initialized &&
                   InternalTestingEnabled;
        }

        public bool AreDebugToolsEnabled()
        {
            return Initialized &&
                   DebugToolsEnabled;
        }

        public string GetBuildId()
        {
            return BuildId;
        }

        public void Reset()
        {
            BuildId = null;
            InternalTestingEnabled = false;
            DebugToolsEnabled = false;
            Initialized = false;
        }
    }
}
