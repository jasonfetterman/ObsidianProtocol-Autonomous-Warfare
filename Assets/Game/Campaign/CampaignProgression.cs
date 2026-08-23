using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignProgression
    {
        private readonly HashSet<string> unlockedContent =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ProgressionLevel { get; private set; }

        public int UnlockedContentCount =>
            unlockedContent.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            unlockedContent.Clear();
            ProgressionLevel = 0;
            Initialized = true;

            return true;
        }

        public bool AdvanceLevel()
        {
            if (!Initialized)
            {
                return false;
            }

            ProgressionLevel++;

            return true;
        }

        public bool UnlockContent(
            string contentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(contentId))
            {
                return false;
            }

            return unlockedContent.Add(
                contentId.Trim());
        }

        public bool IsUnlocked(
            string contentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(contentId))
            {
                return false;
            }

            return unlockedContent.Contains(
                contentId.Trim());
        }

        public IReadOnlyCollection<string>
            GetUnlockedContent()
        {
            return unlockedContent;
        }

        public void Reset()
        {
            unlockedContent.Clear();
            ProgressionLevel = 0;
            Initialized = false;
        }
    }
}
