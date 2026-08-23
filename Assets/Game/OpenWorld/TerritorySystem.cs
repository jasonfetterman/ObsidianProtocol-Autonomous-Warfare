using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum TerritoryControlState
    {
        Unclaimed,
        Contested,
        Controlled
    }

    public sealed class TerritoryRecord
    {
        public string TerritoryId { get; }

        public TerritoryControlState ControlState { get; private set; }

        public float ControlPercent { get; private set; }

        public string ControllerId { get; private set; }

        public long Revision { get; private set; }

        public TerritoryRecord(
            string territoryId)
        {
            TerritoryId =
                territoryId ?? string.Empty;

            ControlState =
                TerritoryControlState.Unclaimed;

            ControllerId =
                string.Empty;
        }

        public bool UpdateControl(
            TerritoryControlState state,
            float controlPercent,
            string controllerId,
            long revision)
        {
            if (string.IsNullOrWhiteSpace(TerritoryId) ||
                revision < 0)
            {
                return false;
            }

            if (controlPercent < 0f ||
                controlPercent > 100f)
            {
                return false;
            }

            if (state ==
                TerritoryControlState.Controlled &&
                string.IsNullOrWhiteSpace(controllerId))
            {
                return false;
            }

            ControlState = state;
            ControlPercent = controlPercent;

            ControllerId =
                state == TerritoryControlState.Unclaimed
                    ? string.Empty
                    : controllerId?.Trim() ?? string.Empty;

            Revision = revision;

            return true;
        }
    }

    public sealed class TerritorySystem
    {
        private readonly Dictionary<
            string,
            TerritoryRecord> territories =
            new Dictionary<
                string,
                TerritoryRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TerritoryCount =>
            territories.Count;

        public long Revision { get; private set; }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            territories.Clear();
            Revision = 0;
            Initialized = true;

            return true;
        }

        public bool RegisterTerritory(
            string territoryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId))
            {
                return false;
            }

            string id =
                territoryId.Trim();

            if (territories.ContainsKey(id))
            {
                return false;
            }

            territories.Add(
                id,
                new TerritoryRecord(id));

            return true;
        }

        public bool SetControl(
            string territoryId,
            TerritoryControlState state,
            float controlPercent,
            string controllerId)
        {
            TerritoryRecord record =
                GetTerritory(territoryId);

            if (record == null)
            {
                return false;
            }

            Revision++;

            return record.UpdateControl(
                state,
                controlPercent,
                controllerId,
                Revision);
        }

        public bool IsControlledBy(
            string territoryId,
            string controllerId)
        {
            TerritoryRecord record =
                GetTerritory(territoryId);

            if (record == null ||
                record.ControlState !=
                    TerritoryControlState.Controlled ||
                string.IsNullOrWhiteSpace(controllerId))
            {
                return false;
            }

            return string.Equals(
                record.ControllerId,
                controllerId.Trim(),
                StringComparison.OrdinalIgnoreCase);
        }

        public bool IsContested(
            string territoryId)
        {
            TerritoryRecord record =
                GetTerritory(territoryId);

            return record != null &&
                   record.ControlState ==
                       TerritoryControlState.Contested;
        }

        public TerritoryRecord GetTerritory(
            string territoryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId))
            {
                return null;
            }

            territories.TryGetValue(
                territoryId.Trim(),
                out TerritoryRecord record);

            return record;
        }

        public IReadOnlyCollection<TerritoryRecord>
            GetTerritories()
        {
            return territories.Values;
        }

        public void Reset()
        {
            territories.Clear();
            Revision = 0;
            Initialized = false;
        }
    }
}
