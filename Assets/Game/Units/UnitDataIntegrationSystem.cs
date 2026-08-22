using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitDataRecord
    {
        public string UnitId { get; }
        public string DisplayName { get; private set; }
        public string FactionId { get; private set; }
        public string Category { get; private set; }

        public UnitDataRecord(string unitId)
        {
            UnitId = unitId ?? string.Empty;
            DisplayName = string.Empty;
            FactionId = string.Empty;
            Category = string.Empty;
        }

        public void Configure(
            string displayName,
            string factionId,
            string category)
        {
            DisplayName =
                displayName ?? string.Empty;

            FactionId =
                factionId ?? string.Empty;

            Category =
                category ?? string.Empty;
        }
    }

    public sealed class UnitDataIntegrationSystem
    {
        private readonly Dictionary<string, UnitDataRecord> records =
            new Dictionary<string, UnitDataRecord>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId,
            string displayName,
            string factionId,
            string category)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!records.TryGetValue(
                    unitId,
                    out UnitDataRecord record))
            {
                record =
                    new UnitDataRecord(unitId);

                records.Add(
                    unitId,
                    record);
            }

            record.Configure(
                displayName,
                factionId,
                category);
        }

        public bool TryGetUnit(
            string unitId,
            out UnitDataRecord record)
        {
            return records.TryGetValue(
                unitId,
                out record);
        }

        public bool IsRegistered(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId) &&
                   records.ContainsKey(unitId);
        }

        public void RemoveUnit(string unitId)
        {
            records.Remove(unitId);
        }

        public void Clear()
        {
            records.Clear();
        }
    }
}
