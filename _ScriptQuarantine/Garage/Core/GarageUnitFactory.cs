using System;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageUnitFactory : MonoBehaviour
    {
        [SerializeField]
        private UnitDefinitionDatabase database;

        public UnitDefinitionDatabase Database => database;

        public void SetDatabase(UnitDefinitionDatabase value)
        {
            database = value;
        }

        public OwnedUnit CreateUnit(
            string unitId,
            string ownerId)
        {
            if (database == null)
            {
                Debug.LogError(
                    "GarageUnitFactory: UnitDefinitionDatabase is not assigned.");
                return null;
            }

            UnitDefinition definition =
                database.GetUnit(unitId);

            if (definition == null)
            {
                Debug.LogError(
                    $"GarageUnitFactory: Unit '{unitId}' was not found.");
                return null;
            }

            OwnedUnit unit = new OwnedUnit();

            unit.Initialize(
                definition,
                Guid.NewGuid().ToString("N"));

            unit.ownerId = ownerId;

            return unit;
        }
    }
}
