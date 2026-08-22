using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Store
{
    public sealed class UnitCatalogEntry
    {
        public string CatalogId { get; }
        public string UnitId { get; }
        public string DisplayName { get; }

        public int CreditCost { get; }

        public bool Available { get; private set; }

        public UnitCatalogEntry(
            string catalogId,
            string unitId,
            string displayName,
            int creditCost)
        {
            CatalogId =
                catalogId ?? string.Empty;

            UnitId =
                unitId ?? string.Empty;

            DisplayName =
                displayName ?? string.Empty;

            CreditCost =
                Math.Max(0, creditCost);

            Available = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(CatalogId) &&
            !string.IsNullOrWhiteSpace(UnitId) &&
            !string.IsNullOrWhiteSpace(DisplayName);

        public void SetAvailable()
        {
            Available = true;
        }

        public void SetUnavailable()
        {
            Available = false;
        }
    }

    public sealed class UnitCatalog
    {
        private readonly Dictionary<
            string,
            UnitCatalogEntry> entries =
            new Dictionary<
                string,
                UnitCatalogEntry>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UnitCatalogEntry entry)
        {
            if (entry == null ||
                !entry.Valid ||
                entries.ContainsKey(
                    entry.CatalogId))
            {
                return false;
            }

            entries.Add(
                entry.CatalogId,
                entry);

            return true;
        }

        public bool Remove(
            string catalogId)
        {
            if (string.IsNullOrWhiteSpace(
                    catalogId))
            {
                return false;
            }

            return entries.Remove(
                catalogId);
        }

        public bool TryGet(
            string catalogId,
            out UnitCatalogEntry entry)
        {
            return entries.TryGetValue(
                catalogId,
                out entry);
        }

        public bool SetAvailable(
            string catalogId)
        {
            if (!entries.TryGetValue(
                    catalogId,
                    out UnitCatalogEntry entry))
            {
                return false;
            }

            entry.SetAvailable();
            return true;
        }

        public bool SetUnavailable(
            string catalogId)
        {
            if (!entries.TryGetValue(
                    catalogId,
                    out UnitCatalogEntry entry))
            {
                return false;
            }

            entry.SetUnavailable();
            return true;
        }

        public IReadOnlyCollection<
            UnitCatalogEntry>
            GetEntries()
        {
            return entries.Values;
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
