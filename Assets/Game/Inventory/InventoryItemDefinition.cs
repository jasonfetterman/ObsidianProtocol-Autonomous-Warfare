using System;

namespace ObsidianProtocol.Game.Inventory
{
    [Serializable]
    public sealed class InventoryItemDefinition
    {
        public string DefinitionId = "";

        public InventoryItemType Type =
            InventoryItemType.Other;

        public string DisplayName = "";

        public bool Stackable = true;

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(
                DefinitionId);
    }
}
