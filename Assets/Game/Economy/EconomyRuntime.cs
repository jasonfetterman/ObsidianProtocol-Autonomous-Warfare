
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

        public const string ENERGY = "energy";
        public const string METALS = "metals";
        public const string RARE_MATERIALS = "rare_materials";
        public const string FUEL = "fuel";
        public const string MUNITIONS = "munitions";

        private static EconomySaveData data;
        private static string campaignId = "";
        private static string playerId = "PLAYER";

        public static bool CampaignActive =>
            !string.IsNullOrWhiteSpace(campaignId) && data != null;

        public static EconomySaveData Data => data;

        public static int Credits =>
            data != null ? Mathf.Max(0, data.Credits) : 0;

        public static int Energy => GetResource(ENERGY);
        public static int Metals => GetResource(METALS);
        public static int RareMaterials => GetResource(RARE_MATERIALS);
        public static int Fuel => GetResource(FUEL);
        public static int Munitions => GetResource(MUNITIONS);

        public static void StartNewCampaign(
            string activeCampaignId,
            int startingCredits,
            string activePlayerId = "PLAYER")
        {
            if (string.IsNullOrWhiteSpace(activeCampaignId))
                return;

            if (campaignId == activeCampaignId &&
                data != null &&
                data.CampaignId == activeCampaignId)
                return;

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
            if (!CampaignActive || amount <= 0 || data.Credits < amount)
                return false;

            data.Credits -= amount;
            Save();
            return true;
        }

        public static void AddCredits(int amount)
        {
            if (!CampaignActive || amount <= 0)
                return;

            data.Credits = AddSafely(data.Credits, amount);
            Save();
        }

        public static void SetCredits(int amount)
        {
            if (!CampaignActive)
                return;

            data.Credits = Mathf.Max(0, amount);
            Save();
        }

        public static int GetResource(string id)
        {
            if (data == null || string.IsNullOrWhiteSpace(id))
                return 0;

            if (data.Resources == null)
                data.Resources = new List<EconomyResourceEntry>();

            EconomyResourceEntry entry = data.Resources.Find(
                r => r != null &&
                string.Equals(r.ResourceId, id,
                    StringComparison.OrdinalIgnoreCase));

            return entry != null ? Mathf.Max(0, entry.Amount) : 0;
        }

        public static void SetResource(string id, int amount)
        {
            if (!CampaignActive || string.IsNullOrWhiteSpace(id))
                return;

            if (data.Resources == null)
                data.Resources = new List<EconomyResourceEntry>();

            EconomyResourceEntry entry = data.Resources.Find(
                r => r != null &&
                string.Equals(r.ResourceId, id,
                    StringComparison.OrdinalIgnoreCase));

            if (entry == null)
            {
                entry = new EconomyResourceEntry
                {
                    ResourceId = id
                };

                data.Resources.Add(entry);
            }

            entry.Amount = Mathf.Max(0, amount);
            Save();
        }

        public static void AddResource(string id, int amount)
        {
            if (!CampaignActive || amount <= 0)
                return;

            SetResource(id, AddSafely(GetResource(id), amount));
        }

        public static bool SpendResource(string id, int amount)
        {
            if (!CampaignActive || amount <= 0)
                return false;

            int current = GetResource(id);

            if (current < amount)
                return false;

            SetResource(id, current - amount);
            return true;
        }

        private static int AddSafely(int current, int amount)
        {
            if (current > int.MaxValue - amount)
                return int.MaxValue;

            return current + amount;
        }

        public static void Save()
        {
            if (!CampaignActive)
                return;

            data.CampaignId = campaignId;

            string json = JsonUtility.ToJson(data, true);

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

            string key = SaveKeyPrefix + campaignId;

            if (!PlayerPrefs.HasKey(key))
            {
                data = null;
                return;
            }

            string json = PlayerPrefs.GetString(key, "");

            if (string.IsNullOrWhiteSpace(json))
            {
                data = null;
                return;
            }

            try
            {
                data = JsonUtility.FromJson<EconomySaveData>(json);

                if (data == null || data.CampaignId != campaignId)
                {
                    data = null;
                    return;
                }

                if (data.Resources == null)
                    data.Resources = new List<EconomyResourceEntry>();

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
