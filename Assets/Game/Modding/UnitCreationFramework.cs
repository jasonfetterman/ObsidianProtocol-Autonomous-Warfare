using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class UnitCreationDefinition
    {
        public string UnitId { get; }

        public string UnitName { get; }

        public string UnitType { get; }

        public bool Enabled { get; private set; }

        public UnitCreationDefinition(
            string unitId,
            string unitName,
            string unitType)
        {
            UnitId =
                unitId ?? string.Empty;

            UnitName =
                unitName ?? string.Empty;

            UnitType =
                unitType ?? string.Empty;

            Enabled = true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class UnitCreationFramework
    {
        private readonly Dictionary<
            string,
            UnitCreationDefinition> definitions =
            new Dictionary<
                string,
                UnitCreationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitDefinitionCount =>
            definitions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            definitions.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateUnit(
            string unitId,
            string unitName,
            string unitType)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId) ||
                string.IsNullOrWhiteSpace(unitName) ||
                string.IsNullOrWhiteSpace(unitType))
            {
                return false;
            }

            string id =
                unitId.Trim();

            if (definitions.ContainsKey(id))
            {
                return false;
            }

            definitions.Add(
                id,
                new UnitCreationDefinition(
                    id,
                    unitName.Trim(),
                    unitType.Trim()));

            return true;
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return definitions.Remove(
                unitId.Trim());
        }

        public UnitCreationDefinition GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            definitions.TryGetValue(
                unitId.Trim(),
                out UnitCreationDefinition definition);

            return definition;
        }

        public IReadOnlyCollection<
            UnitCreationDefinition>
            GetUnits()
        {
            return definitions.Values;
        }

        public void Reset()
        {
            definitions.Clear();
            Initialized = false;
        }
    }
}
