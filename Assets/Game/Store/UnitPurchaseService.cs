using System;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Units;
using ObsidianProtocol.Game.Economy;

namespace ObsidianProtocol.Game.Store
{
    public sealed class UnitPurchaseService
    {
        public bool Purchase(
            string unitId,
            string playerId = "PLAYER")
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            if (!InventoryDefinitionRegistry.TryGet(
                    unitId,
                    out InventoryItemDefinition definition))
                return false;

            if (definition.Type != InventoryItemType.Unit)
                return false;

            UnitDefinition unit =
                UnitDatabaseRuntime.Get(unitId);

            if (unit == null)
                return false;

            int creditCost = unit.Economy.creditCost;

            if (creditCost <= 0)
                return false;

            if (!InventoryRuntime.CampaignActive)
                return false;

            if (InventoryRuntime.Has(unitId))
                return false;

            if (!EconomyRuntime.CampaignActive)
                return false;

            if (!EconomyRuntime.SpendCredits(creditCost))
                return false;

            InventoryItem purchased =
                InventoryRuntime.Add(
                    unitId,
                    InventoryItemType.Unit,
                    1,
                    Guid.NewGuid().ToString("N"));

            if (purchased == null)
            {
                EconomyRuntime.AddCredits(creditCost);
                return false;
            }

            return true;
        }
    }
}
