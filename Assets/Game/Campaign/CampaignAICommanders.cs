using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignAICommander
    {
        public string CommanderId { get; }

        public string CommanderName { get; }

        public string BehaviorProfile { get; }

        public bool Active { get; private set; }

        public CampaignAICommander(
            string commanderId,
            string commanderName,
            string behaviorProfile)
        {
            CommanderId =
                commanderId ?? string.Empty;

            CommanderName =
                commanderName ?? string.Empty;

            BehaviorProfile =
                behaviorProfile ?? string.Empty;

            Active = false;
        }

        public bool Activate()
        {
            if (Active)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Active)
            {
                return false;
            }

            Active = false;

            return true;
        }
    }

    public sealed class CampaignAICommanders
    {
        private readonly Dictionary<
            string,
            CampaignAICommander> commanders =
            new Dictionary<
                string,
                CampaignAICommander>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int CommanderCount =>
            commanders.Count;

        public int ActiveCommanderCount
        {
            get
            {
                int count = 0;

                foreach (CampaignAICommander commander
                         in commanders.Values)
                {
                    if (commander.Active)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            commanders.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterCommander(
            string commanderId,
            string commanderName,
            string behaviorProfile)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(commanderId) ||
                string.IsNullOrWhiteSpace(commanderName) ||
                string.IsNullOrWhiteSpace(behaviorProfile))
            {
                return false;
            }

            string id =
                commanderId.Trim();

            if (commanders.ContainsKey(id))
            {
                return false;
            }

            commanders.Add(
                id,
                new CampaignAICommander(
                    id,
                    commanderName.Trim(),
                    behaviorProfile.Trim()));

            return true;
        }

        public bool ActivateCommander(
            string commanderId)
        {
            CampaignAICommander commander =
                GetCommander(commanderId);

            return commander != null &&
                   commander.Activate();
        }

        public bool DeactivateCommander(
            string commanderId)
        {
            CampaignAICommander commander =
                GetCommander(commanderId);

            return commander != null &&
                   commander.Deactivate();
        }

        public CampaignAICommander GetCommander(
            string commanderId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(commanderId))
            {
                return null;
            }

            commanders.TryGetValue(
                commanderId.Trim(),
                out CampaignAICommander commander);

            return commander;
        }

        public IReadOnlyCollection<
            CampaignAICommander>
            GetCommanders()
        {
            return commanders.Values;
        }

        public void Reset()
        {
            commanders.Clear();
            Initialized = false;
        }
    }
}
