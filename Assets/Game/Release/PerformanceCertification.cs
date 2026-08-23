using System;

namespace ObsidianProtocol.Game.Release
{
    public sealed class PerformanceCertification
    {
        public bool Initialized { get; private set; }

        public bool Certified { get; private set; }

        public float MinimumFPS { get; private set; }

        public float AverageFPS { get; private set; }

        public float MaximumFrameTimeMs { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            MinimumFPS = 60f;
            AverageFPS = 0f;
            MaximumFrameTimeMs = 0f;
            Certified = false;

            Initialized = true;

            return true;
        }

        public bool SubmitResults(
            float averageFPS,
            float maximumFrameTimeMs)
        {
            if (!Initialized ||
                averageFPS < 0f ||
                maximumFrameTimeMs < 0f)
            {
                return false;
            }

            AverageFPS = averageFPS;
            MaximumFrameTimeMs =
                maximumFrameTimeMs;

            Certified =
                AverageFPS >= MinimumFPS &&
                MaximumFrameTimeMs <= 16.67f;

            return true;
        }

        public bool IsCertified()
        {
            return Initialized &&
                   Certified;
        }

        public void SetMinimumFPS(
            float minimumFPS)
        {
            if (!Initialized ||
                minimumFPS < 0f)
            {
                return;
            }

            MinimumFPS = minimumFPS;
        }

        public void Reset()
        {
            Initialized = false;
            Certified = false;
            MinimumFPS = 0f;
            AverageFPS = 0f;
            MaximumFrameTimeMs = 0f;
        }
    }
}
