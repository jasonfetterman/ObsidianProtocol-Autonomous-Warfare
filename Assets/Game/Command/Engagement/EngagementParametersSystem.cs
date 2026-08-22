using System;

namespace ObsidianProtocol.Game.Command.Engagement
{
    [Serializable]
    public sealed class EngagementParameters
    {
        public float MaximumEngagementRange = 500f;
        public float MinimumEngagementRange = 0f;
        public float TargetPriorityThreshold = 0.5f;
        public float PursuitRange = 300f;
        public float DisengageHealthPercent = 0.2f;
        public bool AllowPursuit = true;
        public bool AllowTargetSwitching = true;

        public void Clamp()
        {
            MinimumEngagementRange =
                Math.Max(0f, MinimumEngagementRange);

            MaximumEngagementRange =
                Math.Max(
                    MinimumEngagementRange,
                    MaximumEngagementRange);

            TargetPriorityThreshold =
                Math.Max(
                    0f,
                    Math.Min(1f, TargetPriorityThreshold));

            PursuitRange =
                Math.Max(0f, PursuitRange);

            DisengageHealthPercent =
                Math.Max(
                    0f,
                    Math.Min(1f, DisengageHealthPercent));
        }
    }

    public sealed class EngagementParametersSystem
    {
        public EngagementParameters CurrentParameters
        {
            get;
            private set;
        }

        public EngagementParametersSystem()
        {
            CurrentParameters =
                new EngagementParameters();

            CurrentParameters.Clamp();
        }

        public void SetParameters(
            EngagementParameters parameters)
        {
            if (parameters == null)
            {
                return;
            }

            parameters.Clamp();
            CurrentParameters = parameters;
        }

        public void Reset()
        {
            CurrentParameters =
                new EngagementParameters();

            CurrentParameters.Clamp();
        }
    }
}
