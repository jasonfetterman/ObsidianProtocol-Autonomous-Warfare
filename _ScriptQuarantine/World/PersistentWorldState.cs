using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.World
{
    [Serializable]
    public class PersistentWorldUnit
    {
        public string unitInstanceId;
        public string unitDefinitionId;

        public Vector3 position;
        public Vector3 rotation;

        public bool active = true;
        public bool destroyed;
    }

    [Serializable]
    public class PersistentWorldState
    {
        [Header("World")]
        public string worldId;
        public string worldName;

        [Header("Session")]
        public bool initialized;
        public bool online;
        public bool offline;

        [Header("Units")]
        public List<PersistentWorldUnit> units =
            new List<PersistentWorldUnit>();

        public void Initialize(
            string id,
            string name)
        {
            worldId = id;
            worldName = name;

            initialized = true;
        }

        public void SetOnline(bool value)
        {
            online = value;

            if (value)
                offline = false;
        }

        public void SetOffline(bool value)
        {
            offline = value;

            if (value)
                online = false;
        }

        public PersistentWorldUnit FindUnit(
            string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
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

        public void AddUnit(
            PersistentWorldUnit unit)
        {
            if (unit == null)
                return;

            if (FindUnit(unit.unitInstanceId) != null)
                return;

            units.Add(unit);
        }

        public void RemoveUnit(
            string unitInstanceId)
        {
            PersistentWorldUnit unit =
                FindUnit(unitInstanceId);

            if (unit == null)
                return;

            units.Remove(unit);
        }

        public void ClearUnits()
        {
            units.Clear();
        }
    }
}
