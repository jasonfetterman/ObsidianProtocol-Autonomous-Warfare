using ObsidianProtocol.Game.Modding;
using ObsidianProtocol.Game.Runtime;

namespace ObsidianProtocol.Game.Units
{
    public static class UnitCreationDatabaseBridge
    {
        public static int Populate(UnitCreationFramework framework)
        {
            if (framework == null || UnitDatabaseRuntime.Database == null)
                return 0;

            framework.Initialize();

            int added = 0;

            foreach (UnitDefinition unit in UnitDatabaseRuntime.Database.Units)
            {
                if (unit == null || string.IsNullOrWhiteSpace(unit.UnitId))
                    continue;

                if (framework.CreateUnit(
                    unit.UnitId,
                    unit.DisplayName,
                    unit.Category.ToString()))
                {
                    added++;
                }
            }

            return added;
        }
    }
}
