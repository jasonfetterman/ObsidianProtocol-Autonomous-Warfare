using System;
using ObsidianProtocol.Game.Core;
using ObsidianProtocol.Game.Garage;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Runtime;
using ObsidianProtocol.Game.Units;
using ObsidianProtocol.Game.Economy;
using UnityEngine;

namespace ObsidianProtocol.Game.Store
{
    public sealed class UnitPurchaseService
    {
        private static bool Fail(string reason)
        {
            Debug.LogError("[UnitPurchase] " + reason);
            return false;
        }

        public bool Purchase(string unitId, string playerId = "PLAYER")
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return Fail("Unit ID is empty.");

            if (!InventoryDefinitionRegistry.TryGet(unitId, out InventoryItemDefinition definition))
                return Fail("Inventory definition not found: " + unitId);

            if (definition.Type != InventoryItemType.Unit)
                return Fail("Item is not a unit: " + unitId);

            UnitDefinition unit = UnitDatabaseRuntime.Get(unitId);
            if (unit == null)
                return Fail("Unit database entry missing: " + unitId);

            int creditCost = StoreUnitPricingCatalog.GetPrice(unit);
            if (creditCost <= 0)
                return Fail("Invalid price for " + unitId + ": " + creditCost);

            if (!InventoryRuntime.CampaignActive)
                return Fail("Inventory campaign is inactive.");

            if (InventoryRuntime.Has(unitId))
                return Fail("Unit already owned: " + unitId);

            if (!EconomyRuntime.CampaignActive)
                return Fail("Economy campaign is inactive.");

            ObsidianRuntimeSystems runtime = ObsidianRuntimeSystems.Instance;
            if (runtime == null || !runtime.Initialized || runtime.FleetManagement == null)
                return Fail("Runtime or fleet management is unavailable.");

            if (!EconomyRuntime.SpendCredits(creditCost))
                return Fail("Insufficient credits or economy unavailable. Cost: " +
                    creditCost + "; current credits: " + EconomyRuntime.Credits);

            InventoryItem purchased = InventoryRuntime.Add(
                unitId, InventoryItemType.Unit, 1, Guid.NewGuid().ToString("N"));

            if (purchased == null)
            {
                EconomyRuntime.AddCredits(creditCost);
                return Fail("Inventory rejected unit: " + unitId + ". Credits refunded.");
            }

            FleetUnit fleetUnit = new FleetUnit(purchased.InstanceId, unitId);
            if (!runtime.FleetManagement.Register(fleetUnit))
            {
                InventoryRuntime.Remove(unitId, 1);
                EconomyRuntime.AddCredits(creditCost);
                return Fail("Fleet registration rejected unit: " + unitId +
                    ". Inventory and credits rolled back.");
            }

            SaveLoadService.SaveFleetSnapshot();
            Debug.Log("[UnitPurchase] Purchase successful: " + unitId);
            return true;
        }
    }
}
