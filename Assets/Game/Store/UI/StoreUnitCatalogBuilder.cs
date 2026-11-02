using System;
using ObsidianProtocol.Game.Runtime;
using UnityEngine;

namespace ObsidianProtocol.Game.Store.UI
{
    public sealed class StoreUnitCatalogBuilder : MonoBehaviour
    {
        public UnitCatalog Catalog { get; private set; }

        public bool BuildCatalog()
        {
            if (!UnitDatabaseRuntime.EnsureInitialized())
            {
                Debug.LogError("[Store] Unit database could not be initialized.");
                return false;
            }

            var database = UnitDatabaseRuntime.Database;
            if (database == null || database.Units == null)
            {
                Debug.LogError("[Store] Unit database contains no unit collection.");
                return false;
            }

            var catalog = new UnitCatalog();
            int registered = 0;

            foreach (var unit in database.Units)
            {
                if (unit == null ||
                    string.IsNullOrWhiteSpace(unit.UnitId) ||
                    string.IsNullOrWhiteSpace(unit.DisplayName) ||
                    unit.Economy == null ||
                    unit.Economy.creditCost <= 0)
                {
                    continue;
                }

                string id = unit.UnitId.Trim();
                string name = unit.DisplayName.Trim();

                var entry = new UnitCatalogEntry(
                    id,
                    id,
                    name,
                    unit.Economy.creditCost);

                if (!catalog.Register(entry))
                {
                    Debug.LogWarning(
                        "[Store] Skipped duplicate or invalid unit: " + id);
                    continue;
                }

                catalog.SetAvailable(id);
                registered++;
            }

            Catalog = catalog;
            Debug.Log("[Store] Registered " + registered + " purchasable units.");
            return true;
        }

        public void ClearCatalog()
        {
            if (Catalog != null)
                Catalog.Clear();

            Catalog = null;
        }
    }
}