using System;
using ObsidianProtocol.Game.Progression;

namespace ObsidianProtocol.Game.Core
{
    [Serializable]
    public sealed class SaveData
    {
        public int Version = 1;

        public string CampaignId;

        public string CommanderName;

        public int Funding;

        public ProgressionSaveData Progression;
    }
}