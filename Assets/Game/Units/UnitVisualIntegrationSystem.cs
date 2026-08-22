using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitVisualRecord
    {
        public string UnitId { get; }

        public string ModelPath { get; private set; }
        public string MaterialPath { get; private set; }
        public string IconPath { get; private set; }

        public bool ModelAssigned { get; private set; }
        public bool MaterialAssigned { get; private set; }
        public bool IconAssigned { get; private set; }

        public UnitVisualRecord(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            ModelPath = string.Empty;
            MaterialPath = string.Empty;
            IconPath = string.Empty;
        }

        public void Configure(
            string modelPath,
            string materialPath,
            string iconPath)
        {
            ModelPath =
                modelPath ?? string.Empty;

            MaterialPath =
                materialPath ?? string.Empty;

            IconPath =
                iconPath ?? string.Empty;

            ModelAssigned =
                !string.IsNullOrWhiteSpace(ModelPath);

            MaterialAssigned =
                !string.IsNullOrWhiteSpace(MaterialPath);

            IconAssigned =
                !string.IsNullOrWhiteSpace(IconPath);
        }
    }

    public sealed class UnitVisualIntegrationSystem
    {
        private readonly Dictionary<string, UnitVisualRecord> visuals =
            new Dictionary<string, UnitVisualRecord>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!visuals.ContainsKey(unitId))
            {
                visuals.Add(
                    unitId,
                    new UnitVisualRecord(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            string modelPath,
            string materialPath,
            string iconPath)
        {
            RegisterUnit(unitId);

            visuals[unitId].Configure(
                modelPath,
                materialPath,
                iconPath);
        }

        public bool TryGetVisual(
            string unitId,
            out UnitVisualRecord visual)
        {
            return visuals.TryGetValue(
                unitId,
                out visual);
        }

        public bool HasModel(string unitId)
        {
            return visuals.TryGetValue(
                       unitId,
                       out UnitVisualRecord visual) &&
                   visual.ModelAssigned;
        }

        public void RemoveUnit(string unitId)
        {
            visuals.Remove(unitId);
        }

        public void Clear()
        {
            visuals.Clear();
        }
    }
}
