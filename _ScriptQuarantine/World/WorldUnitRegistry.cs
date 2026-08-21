using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldUnitRegistry : MonoBehaviour
    {
        [Header("Registered World Units")]
        [SerializeField]
        private List<PersistentWorldUnit> units =
            new List<PersistentWorldUnit>();

        public IReadOnlyList<PersistentWorldUnit> Units =>
            units;

        public int Count =>
            units.Count;

        public bool Contains(
            string unitInstanceId)
        {
            return Get(unitInstanceId) != null;
        }

        public PersistentWorldUnit Get(
            string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(
                    unitInstanceId))
                return null;

            foreach (PersistentWorldUnit unit in units)
            {
                if (unit == null)
                    continue;

                if (unit.unitInstanceId ==
                    unitInstanceId)
                    return unit;
            }

            return null;
        }

        public bool Register(
            PersistentWorldUnit unit)
        {
            if (unit == null)
                return false;

            if (string.IsNullOrWhiteSpace(
                    unit.unitInstanceId))
                return false;

            if (Contains(unit.unitInstanceId))
                return false;

            units.Add(unit);

            return true;
        }

        public bool Register(
            string unitInstanceId,
            string unitDefinitionId,
            Vector3 position,
            Vector3 rotation)
        {
            if (string.IsNullOrWhiteSpace(
                    unitInstanceId))
                return false;

            if (Contains(unitInstanceId))
                return false;

            PersistentWorldUnit unit =
                new PersistentWorldUnit
                {
                    unitInstanceId =
                        unitInstanceId,

                    unitDefinitionId =
                        unitDefinitionId,

                    position =
                        position,

                    rotation =
                        rotation,

                    active = true,
                    destroyed = false
                };

            return Register(unit);
        }

        public bool Remove(
            string unitInstanceId)
        {
            PersistentWorldUnit unit =
                Get(unitInstanceId);

            if (unit == null)
                return false;

            units.Remove(unit);

            return true;
        }

        public void SetDestroyed(
            string unitInstanceId,
            bool destroyed)
        {
            PersistentWorldUnit unit =
                Get(unitInstanceId);

            if (unit == null)
                return;

            unit.destroyed = destroyed;
            unit.active = !destroyed;
        }

        public void SetActive(
            string unitInstanceId,
            bool active)
        {
            PersistentWorldUnit unit =
                Get(unitInstanceId);

            if (unit == null)
                return;

            unit.active = active;

            if (active)
                unit.destroyed = false;
        }

        public void UpdateTransform(
            string unitInstanceId,
            Vector3 position,
            Vector3 rotation)
        {
            PersistentWorldUnit unit =
                Get(unitInstanceId);

            if (unit == null)
                return;

            unit.position = position;
            unit.rotation = rotation;
        }

        public void Clear()
        {
            units.Clear();
        }
    }
}
