using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class RTSFeedbackPolish
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

            SetDefault("SelectionFeedback", true);
            SetDefault("CommandFeedback", true);
            SetDefault("AttackFeedback", true);
            SetDefault("MoveFeedback", true);
            SetDefault("ObjectiveFeedback", true);
            SetDefault("AlertFeedback", true);

            Initialized = true;

            return true;
        }

        public bool SetFeedback(
            string feedbackId,
            bool enabled)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(feedbackId))
            {
                return false;
            }

            feedbackStates[feedbackId.Trim()] =
                enabled;

            return true;
        }

        public bool IsEnabled(
            string feedbackId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(feedbackId))
            {
                return false;
            }

            return feedbackStates.TryGetValue(
                feedbackId.Trim(),
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
