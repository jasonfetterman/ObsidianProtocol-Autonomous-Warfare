using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Economy
{
    [Serializable]
    public sealed class EconomySaveData
    {
        public string PlayerId = "PLAYER";
        public string CampaignId = "";
        public int Credits = 0;

        public List<EconomyResourceEntry> Resources =
            new List<EconomyResourceEntry>();
    }

    [Serializable]
    public sealed class EconomyResourceEntry
    {
        public string ResourceId = "";
        public int Amount;
    }

    public static class EconomyRuntime
    {
        private const string SaveKeyPrefix =
            "obsidian_protocol_economy_";

        private static EconomySaveData data;
        private static string campaignId = "";
        private static string playerId = "PLAYER";

        public static bool CampaignActive =>
            !string.IsNullOrWhiteSpace(campaignId) && data != null;

        public static int Credits =>
            data != null ? Mathf.Max(0, data.Credits) : 0;

        public static EconomySaveData Data =>
            data;

        public static void StartNewCampaign(
            string activeCampaignId,
            int startingCredits,
            string activePlayerId = "PLAYER")
        {
            if (string.IsNullOrWhiteSpace(activeCampaignId))
                return;

            // Preserve existing economy data when initialization is repeated
            // for the same campaign.
            if (campaignId == activeCampaignId &&
                data != null &&
                data.CampaignId == activeCampaignId)
            {
                return;
            }

            campaignId = activeCampaignId;
            playerId = string.IsNullOrWhiteSpace(activePlayerId)
                ? "PLAYER"
                : activePlayerId.Trim();

            data = new EconomySaveData
            {
                PlayerId = playerId,
                CampaignId = campaignId,
                Credits = Mathf.Max(0, startingCredits)
            };

            Save();
        }

        public static bool SpendCredits(int amount)
        {
            if (!CampaignActive ||
                data == null ||
                amount <= 0 ||
                data.Credits < amount)
            {
                return false;
            }

            data.Credits -= amount;
            Save();

            return true;
        }

        public static void AddCredits(int amount)
        {
            if (!CampaignActive ||
                data == null ||
                amount <= 0)
            {
                return;
            }

            if (data.Credits > int.MaxValue - amount)
                data.Credits = int.MaxValue;
            else
                data.Credits += amount;

            Save();
        }

        public static void SetCredits(int amount)
        {
            if (!CampaignActive || data == null)
                return;

            data.Credits = Mathf.Max(0, amount);
            Save();
        }

        public static void Save()
        {
            if (!CampaignActive || data == null)
                return;

            data.CampaignId = campaignId;

            string json =
                JsonUtility.ToJson(data, true);

            PlayerPrefs.SetString(
                SaveKeyPrefix + campaignId,
                json);

            PlayerPrefs.Save();
        }

        public static void Load(string activeCampaignId)
        {
            if (string.IsNullOrWhiteSpace(activeCampaignId))
                return;

            campaignId = activeCampaignId;

            string key =
                SaveKeyPrefix + campaignId;

            if (!PlayerPrefs.HasKey(key))
            {
                data = null;
                return;
            }

            string json =
                PlayerPrefs.GetString(key, "");

            if (string.IsNullOrWhiteSpace(json))
            {
                data = null;
                return;
            }

            try
            {
                data =
                    JsonUtility.FromJson<EconomySaveData>(
                        json);

                if (data == null ||
                    string.IsNullOrWhiteSpace(data.CampaignId) ||
                    data.CampaignId != campaignId)
                {
                    data = null;
                    return;
                }

                playerId = string.IsNullOrWhiteSpace(data.PlayerId)
                    ? "PLAYER"
                    : data.PlayerId.Trim();
            }
            catch
            {
                data = null;
            }
        }
    }
}




