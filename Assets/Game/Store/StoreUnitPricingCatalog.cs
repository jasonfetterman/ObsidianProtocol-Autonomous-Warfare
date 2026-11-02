using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Store
{
    public static class StoreUnitPricingCatalog
    {
        public static int GetPrice(UnitDefinition unit)
        {
            return unit != null ? unit.Economy.creditCost : 0;
        }
    }
}
