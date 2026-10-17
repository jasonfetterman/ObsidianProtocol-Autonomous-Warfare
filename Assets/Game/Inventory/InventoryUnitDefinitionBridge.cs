using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Inventory
{
    public static class InventoryUnitDefinitionBridge
    {
        public static int RegisterAll(
            UnitDatabase database)
        {
            if (database == null)
                return 0;

            int registered = 0;

            foreach (UnitDefinition unit in database.Units)
            {
                if (unit == null ||
                    string.IsNullOrWhiteSpace(unit.UnitId))
                {
                    continue;
                }

                InventoryItemDefinition definition =
                    new InventoryItemDefinition
                    {
                        DefinitionId = unit.UnitId,
                        Type = InventoryItemType.Unit,
                        DisplayName = unit.DisplayName,
                        Stackable = false
                    };

                if (InventoryDefinitionRegistry.Register(
                        definition))
                {
                    registered++;
                }
            }

            return registered;
        }
    }
}
