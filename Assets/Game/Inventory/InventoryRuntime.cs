using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Inventory
{
    public static class InventoryRuntime
    {
        private const string SaveKeyPrefix =
            "obsidian_protocol_inventory_";

        private static InventorySaveData data;
        private static string campaignId = "";
        private static string playerId = "PLAYER";

        public static bool Initialized =>
            data != null;

        public static bool CampaignActive =>
            !string.IsNullOrWhiteSpace(campaignId);

        public static InventorySaveData Data =>
            data;

        public static IReadOnlyList<InventoryItem> Items =>
            data != null
                ? data.Items
                : Array.Empty<InventoryItem>();

        public static string CampaignId =>
            campaignId;

        public static void Initialize()
        {
            if (!CampaignActive)
                return;

            Initialize(campaignId);
        }

        public static void Initialize(
            string activeCampaignId)
        {
            activeCampaignId =
                activeCampaignId ?? "";

            if (string.IsNullOrWhiteSpace(
                    activeCampaignId))
            {
                return;
            }

            if (data != null &&
                string.Equals(
                    campaignId,
                    activeCampaignId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            campaignId = activeCampaignId;

            Load();

            if (data == null)
            {
                data = new InventorySaveData
                {
                    PlayerId = playerId,
                    CampaignId = campaignId,
                    Items = new List<InventoryItem>()
                };

                Save();
            }

            if (data.Items == null)
                data.Items = new List<InventoryItem>();

            data.CampaignId = campaignId;
        }

        public static void StartNewCampaign(
            string activeCampaignId,
            string activePlayerId = "PLAYER")
        {
            if (string.IsNullOrWhiteSpace(
                    activeCampaignId))
            {
                return;
            }

            // Prevent repeated initialization from wiping an active campaign.
            if (string.Equals(
                    campaignId,
                    activeCampaignId,
                    StringComparison.OrdinalIgnoreCase) &&
                data != null &&
                string.Equals(
                    data.CampaignId,
                    activeCampaignId,
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            campaignId =
                activeCampaignId;
            playerId = string.IsNullOrWhiteSpace(activePlayerId)
                ? "PLAYER"
                : activePlayerId.Trim();

            data = new InventorySaveData
            {
                PlayerId = playerId,
                CampaignId = campaignId,
                Items = new List<InventoryItem>()
            };

            Save();
        }

        public static InventoryItem Add(
            string definitionId,
            InventoryItemType type,
            int quantity = 1,
            string instanceId = "",
            string location = "")
        {
            if (!CampaignActive)
                return null;

            Initialize(campaignId);

            if (string.IsNullOrWhiteSpace(definitionId) ||
                quantity <= 0)
            {
                return null;
            }

            bool stackable = true;

            if (InventoryDefinitionRegistry.TryGet(
                    definitionId,
                    out InventoryItemDefinition definition))
            {
                if (definition.Type != type)
                    return null;

                stackable = definition.Stackable;
            }

            if (!stackable &&
                string.IsNullOrWhiteSpace(instanceId))
            {
                instanceId =
                    Guid.NewGuid().ToString("N");
            }

            if (stackable &&
                string.IsNullOrWhiteSpace(instanceId))
            {
                InventoryItem existing =
                    FindStack(
                        definitionId,
                        type,
                        location);

                if (existing != null)
                {
                    existing.Quantity += quantity;
                    Touch(existing);
                    Save();
                    return existing;
                }
            }

            InventoryItem item =
                new InventoryItem
                {
                    InstanceId =
                        instanceId ?? "",

                    DefinitionId =
                        definitionId,

                    Type =
                        type,

                    State =
                        InventoryItemState.Stored,

                    Quantity =
                        quantity,

                    Location =
                        location ?? "",

                    AcquiredAt =
                        DateTime.UtcNow.ToString("o"),

                    UpdatedAt =
                        DateTime.UtcNow.ToString("o")
                };

            data.Items.Add(item);
            Save();

            return item;
        }

        public static bool Remove(
            string definitionId,
            int quantity = 1)
        {
            if (!CampaignActive)
                return false;

            Initialize(campaignId);

            if (string.IsNullOrWhiteSpace(definitionId) ||
                quantity <= 0)
            {
                return false;
            }

            for (int i = data.Items.Count - 1; i >= 0; i--)
            {
                InventoryItem item =
                    data.Items[i];

                if (item == null ||
                    !string.Equals(
                        item.DefinitionId,
                        definitionId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (item.Quantity < quantity)
                    continue;

                item.Quantity -= quantity;

                if (item.Quantity <= 0)
                    data.Items.RemoveAt(i);

                Save();
                return true;
            }

            return false;
        }

        public static InventoryItem Find(
            string definitionId)
        {
            if (!CampaignActive)
                return null;

            Initialize(campaignId);

            if (string.IsNullOrWhiteSpace(definitionId))
                return null;

            foreach (InventoryItem item in data.Items)
            {
                if (item == null)
                    continue;

                if (string.Equals(
                    item.DefinitionId,
                    definitionId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        public static InventoryItem FindInstance(
            string instanceId)
        {
            if (!CampaignActive)
                return null;

            Initialize(campaignId);

            if (string.IsNullOrWhiteSpace(instanceId))
                return null;

            foreach (InventoryItem item in data.Items)
            {
                if (item == null)
                    continue;

                if (string.Equals(
                    item.InstanceId,
                    instanceId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        public static int Count(
            string definitionId)
        {
            if (!CampaignActive)
                return 0;

            Initialize(campaignId);

            int total = 0;

            foreach (InventoryItem item in data.Items)
            {
                if (item == null)
                    continue;

                if (string.Equals(
                    item.DefinitionId,
                    definitionId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    total += Mathf.Max(
                        0,
                        item.Quantity);
                }
            }

            return total;
        }

        public static bool Has(
            string definitionId,
            int quantity = 1)
        {
            return Count(definitionId) >=
                   Mathf.Max(0, quantity);
        }

        public static IReadOnlyList<InventoryItem> GetByType(
            InventoryItemType type)
        {
            if (!CampaignActive)
                return Array.Empty<InventoryItem>();

            Initialize(campaignId);

            List<InventoryItem> result =
                new List<InventoryItem>();

            foreach (InventoryItem item in data.Items)
            {
                if (item != null &&
                    item.Type == type)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static IReadOnlyList<InventoryItem> GetByLocation(
            string location)
        {
            if (!CampaignActive)
                return Array.Empty<InventoryItem>();

            Initialize(campaignId);

            List<InventoryItem> result =
                new List<InventoryItem>();

            foreach (InventoryItem item in data.Items)
            {
                if (item == null)
                    continue;

                if (string.Equals(
                    item.Location,
                    location ?? "",
                    StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static bool SetState(
            string instanceId,
            InventoryItemState state)
        {
            InventoryItem item =
                FindInstance(instanceId);

            if (item == null)
                return false;

            item.State = state;
            Touch(item);
            Save();

            return true;
        }

        public static bool SetLocation(
            string instanceId,
            string location)
        {
            InventoryItem item =
                FindInstance(instanceId);

            if (item == null)
                return false;

            item.Location =
                location ?? "";

            Touch(item);
            Save();

            return true;
        }

        public static void Save()
        {
            if (data == null ||
                !CampaignActive)
            {
                return;
            }

            data.CampaignId =
                campaignId;

            string json =
                JsonUtility.ToJson(
                    data,
                    true);

            PlayerPrefs.SetString(
                GetSaveKey(),
                json);

            PlayerPrefs.Save();
        }

        public static void Load()
        {
            if (!CampaignActive)
                return;

            string key =
                GetSaveKey();

            if (!PlayerPrefs.HasKey(key))
            {
                data = null;
                return;
            }

            string json =
                PlayerPrefs.GetString(
                    key,
                    "");

            if (string.IsNullOrWhiteSpace(json))
            {
                data = null;
                return;
            }

            try
            {
                data =
                    JsonUtility.FromJson<InventorySaveData>(
                        json);
            }
            catch
            {
                data = null;
            }

            if (data == null ||
                string.IsNullOrWhiteSpace(data.CampaignId) ||
                !string.Equals(
                    data.CampaignId,
                    campaignId,
                    StringComparison.OrdinalIgnoreCase))
            {
                data = null;
                return;
            }

            if (data.Items == null)
                data.Items = new List<InventoryItem>();

            playerId = string.IsNullOrWhiteSpace(data.PlayerId)
                ? "PLAYER"
                : data.PlayerId.Trim();
        }

        public static void Clear()
        {
            if (!CampaignActive)
                return;

            data =
                new InventorySaveData
                {
                    PlayerId = playerId,
                    CampaignId = campaignId,
                    Items = new List<InventoryItem>()
                };

            Save();
        }

        private static string GetSaveKey()
        {
            return SaveKeyPrefix + campaignId;
        }

        private static InventoryItem FindStack(
            string definitionId,
            InventoryItemType type,
            string location)
        {
            foreach (InventoryItem item in data.Items)
            {
                if (item == null ||
                    item.IsUnique)
                {
                    continue;
                }

                if (item.Type != type)
                    continue;

                if (!string.Equals(
                    item.DefinitionId,
                    definitionId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!string.Equals(
                    item.Location,
                    location ?? "",
                    StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                return item;
            }

            return null;
        }

        private static void Touch(
            InventoryItem item)
        {
            if (item == null)
                return;

            item.UpdatedAt =
                DateTime.UtcNow.ToString("o");
        }
    }
}


