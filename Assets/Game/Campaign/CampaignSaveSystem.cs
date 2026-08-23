using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Campaign
{
    public sealed class CampaignSaveData
    {
        private readonly Dictionary<
            string,
            string> values =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public string SaveId { get; }

        public DateTime SavedAtUtc { get; private set; }

        public CampaignSaveData(
            string saveId)
        {
            SaveId =
                saveId ?? string.Empty;

            SavedAtUtc =
                DateTime.UtcNow;
        }

        public bool SetValue(
            string key,
            string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            values[key.Trim()] =
                value ?? string.Empty;

            return true;
        }

        public string GetValue(
            string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            values.TryGetValue(
                key.Trim(),
                out string value);

            return value;
        }

        public IReadOnlyDictionary<
            string,
            string>
            GetValues()
        {
            return values;
        }

        public void RefreshTimestamp()
        {
            SavedAtUtc =
                DateTime.UtcNow;
        }
    }

    public sealed class CampaignSaveSystem
    {
        private readonly Dictionary<
            string,
            CampaignSaveData> saves =
            new Dictionary<
                string,
                CampaignSaveData>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SaveCount =>
            saves.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            saves.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateSave(
            string saveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(saveId))
            {
                return false;
            }

            string id =
                saveId.Trim();

            if (saves.ContainsKey(id))
            {
                return false;
            }

            saves.Add(
                id,
                new CampaignSaveData(id));

            return true;
        }

        public bool SaveValue(
            string saveId,
            string key,
            string value)
        {
            CampaignSaveData save =
                GetSave(saveId);

            if (save == null)
            {
                return false;
            }

            bool result =
                save.SetValue(key, value);

            if (result)
            {
                save.RefreshTimestamp();
            }

            return result;
        }

        public CampaignSaveData GetSave(
            string saveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(saveId))
            {
                return null;
            }

            saves.TryGetValue(
                saveId.Trim(),
                out CampaignSaveData save);

            return save;
        }

        public bool DeleteSave(
            string saveId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(saveId))
            {
                return false;
            }

            return saves.Remove(
                saveId.Trim());
        }

        public IReadOnlyCollection<
            CampaignSaveData>
            GetSaves()
        {
            return saves.Values;
        }

        public void Reset()
        {
            saves.Clear();
            Initialized = false;
        }
    }
}
