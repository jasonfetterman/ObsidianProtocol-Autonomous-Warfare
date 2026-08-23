using System;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignFramework
    {
        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string CampaignId { get; private set; }

        public int CurrentChapter { get; private set; }

        public bool Initialize(
            string campaignId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(campaignId))
            {
                return false;
            }

            CampaignId =
                campaignId.Trim();

            CurrentChapter = 0;
            Active = false;
            Initialized = true;

            return true;
        }

        public bool Start()
        {
            if (!Initialized ||
                Active)
            {
                return false;
            }

            Active = true;

            if (CurrentChapter == 0)
            {
                CurrentChapter = 1;
            }

            return true;
        }

        public bool AdvanceChapter()
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            CurrentChapter++;

            return true;
        }

        public bool End()
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            Active = false;

            return true;
        }

        public void Reset()
        {
            CampaignId = null;
            CurrentChapter = 0;
            Active = false;
            Initialized = false;
        }
    }
}
