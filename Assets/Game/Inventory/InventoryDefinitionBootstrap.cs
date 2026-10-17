using UnityEngine;
using ObsidianProtocol.Game.Units;
using ObsidianProtocol.Game.Runtime;

namespace ObsidianProtocol.Game.Inventory
{
    public static class InventoryDefinitionBootstrap
    {
        [RuntimeInitializeOnLoadMethod(
            RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (!UnitDatabaseRuntime.EnsureInitialized())
                return;

            UnitDatabase database = UnitDatabaseRuntime.Database;

            int registered =
                InventoryUnitDefinitionBridge.RegisterAll(database);

            Debug.Log(
                "[INVENTORY] Unit definitions registered: " +
                registered);
        }
    }
}
