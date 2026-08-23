using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class CampaignCreationDefinition
    {
        public string CampaignId { get; }

        public string CampaignName { get; }

        public string Description { get; }

        public bool Enabled { get; private set; }

        public CampaignCreationDefinition(
            string campaignId,
            string campaignName,
            string description)
        {
            CampaignId =
                campaignId ?? string.Empty;

            CampaignName =
                campaignName ?? string.Empty;

            Description =
                description ?? string.Empty;

            Enabled = true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class CampaignTools
    {
        private readonly Dictionary<
            string,
            CampaignCreationDefinition> campaigns =
            new Dictionary<
                string,
                CampaignCreationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int CampaignCount =>
            campaigns.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            campaigns.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateCampaign(
            string campaignId,
            string campaignName,
            string description)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(campaignId) ||
                string.IsNullOrWhiteSpace(campaignName))
            {
                return false;
            }

            string id =
                campaignId.Trim();

            if (campaigns.ContainsKey(id))
            {
                return false;
            }

            campaigns.Add(
                id,
                new CampaignCreationDefinition(
                    id,
                    campaignName.Trim(),
                    description ?? string.Empty));

            return true;
        }

        public bool RemoveCampaign(
            string campaignId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(campaignId))
            {
                return false;
            }

            return campaigns.Remove(
                campaignId.Trim());
        }

        public CampaignCreationDefinition GetCampaign(
            string campaignId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(campaignId))
            {
                return null;
            }

            campaigns.TryGetValue(
                campaignId.Trim(),
                out CampaignCreationDefinition campaign);

            return campaign;
        }

        public IReadOnlyCollection<
            CampaignCreationDefinition>
            GetCampaigns()
        {
            return campaigns.Values;
        }

        public void Reset()
        {
            campaigns.Clear();
            Initialized = false;
        }
    }
}
