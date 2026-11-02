using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Inventory
{
    public enum InventoryItemType
    {
        Unit,
        Vehicle,
        Equipment,
        Weapon,
        Module,
        Cosmetic,
        Wallpaper,
        Material,
        Resource,
        Supply,
        Fuel,
        Energy,
        Component,
        Research,
        StoreItem,
        Other
    }

    public enum InventoryItemState
    {
        Stored,
        Equipped,
        Configuring,
        Maintenance,
        Damaged,
        Deployed,
        Consumed,
        Disabled,
        Salvaged
    }

    [Serializable]
    public sealed class InventoryItem
    {
        public string InstanceId = "";
        public string DefinitionId = "";

        public InventoryItemType Type =
            InventoryItemType.Other;

        public InventoryItemState State =
            InventoryItemState.Stored;

        public int Quantity = 1;

        public string Location = "";
        public string EquippedTo = "";

        public string AcquiredAt = "";
        public string UpdatedAt = "";

        public bool IsUnique =>
            !string.IsNullOrWhiteSpace(InstanceId);

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(DefinitionId) &&
            Quantity > 0;
    }

    [Serializable]
    public sealed class InventorySaveData
    {
        public string PlayerId = "PLAYER";
        public string CampaignId = "";

        public List<InventoryItem> Items =
            new List<InventoryItem>();
    }
}

