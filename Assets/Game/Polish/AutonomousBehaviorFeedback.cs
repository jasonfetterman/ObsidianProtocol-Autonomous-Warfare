using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class AutonomousBehaviorFeedback
    {
        private readonly Dictionary<
            string,
            bool> feedbackStates =
            new Dictionary<
                string,
                bool>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int FeedbackCount =>
            feedbackStates.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            feedbackStates.Clear();

            SetDefault("Flanking", true);
            SetDefault("Suppressing", true);
            SetDefault("Breaching", true);
            SetDefault("Retreating", true);
            SetDefault("Pursuing", true);
            SetDefault("Reinforcing", true);
            SetDefault("FiringPosition", true);
            SetDefault("ObjectiveProgress", true);

            Initialized = true;

            return true;
        }

        public bool SetFeedback(
            string behaviorId,
            bool enabled)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(behaviorId))
            {
                return false;
            }

            feedbackStates[behaviorId.Trim()] =
                enabled;

            return true;
        }

        public bool IsEnabled(
            string behaviorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(behaviorId))
            {
                return false;
            }

            return feedbackStates.TryGetValue(
                behaviorId.Trim(),
                out bool enabled) &&
                   enabled;
        }

        private void SetDefault(
            string key,
            bool enabled)
        {
            feedbackStates[key] = enabled;
        }

        public void Reset()
        {
            feedbackStates.Clear();
            Initialized = false;
        }
    }
}
