using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class PublicTestBuild
    {
        public bool Initialized { get; private set; }

        public bool PublicTestingEnabled { get; private set; }

        public bool TelemetryEnabled { get; private set; }

        public bool CrashReportingEnabled { get; private set; }

        public string BuildId { get; private set; }

        public string ReleaseChannel { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            BuildId = "OBSIDIAN-PUBLIC-TEST";
            ReleaseChannel = "PublicTest";

            PublicTestingEnabled = true;
            TelemetryEnabled = true;
            CrashReportingEnabled = true;

            Initialized = true;

            return true;
        }

        public bool IsPublicTestBuild()
        {
            return Initialized &&
                   PublicTestingEnabled &&
                   ReleaseChannel == "PublicTest";
        }

        public bool IsTelemetryEnabled()
        {
            return Initialized &&
                   TelemetryEnabled;
        }

        public bool IsCrashReportingEnabled()
        {
            return Initialized &&
                   CrashReportingEnabled;
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

            PublicTestingEnabled = false;
            TelemetryEnabled = false;
            CrashReportingEnabled = false;

            Initialized = false;
        }
    }
}
