using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum BattlefieldFeature
    {
        Unknown,
        Terrain,
        Road,
        Structure,
        Objective,
        Cover,
        Obstacle,
        Water,
        Resource,
        EnemyPosition,
        FriendlyPosition
    }

    public sealed class BattlefieldMapRecord
    {
        public int CellId;
        public BattlefieldFeature Feature;
        public float Confidence;
        public bool Known;

        public BattlefieldMapRecord(int cellId)
        {
            CellId = cellId;
            Feature = BattlefieldFeature.Unknown;
            Confidence = 0f;
            Known = false;
        }
    }

    public sealed class BattlefieldMappingSystem
    {
        private readonly Dictionary<int, BattlefieldMapRecord> map =
            new Dictionary<int, BattlefieldMapRecord>();

        public void RegisterCell(int cellId)
        {
            if (cellId < 0)
            {
                return;
            }

            if (!map.ContainsKey(cellId))
            {
                map.Add(
                    cellId,
                    new BattlefieldMapRecord(cellId));
            }
        }

        public void UpdateCell(
            int cellId,
            BattlefieldFeature feature,
            float confidence)
        {
            RegisterCell(cellId);

            BattlefieldMapRecord record =
                map[cellId];

            record.Feature = feature;
            record.Confidence =
                System.Math.Clamp(
                    confidence,
                    0f,
                    1f);
            record.Known = true;
        }

        public bool TryGetCell(
            int cellId,
            out BattlefieldMapRecord record)
        {
            return map.TryGetValue(
                cellId,
                out record);
        }

        public bool IsKnown(int cellId)
        {
            return map.TryGetValue(
                       cellId,
                       out BattlefieldMapRecord record) &&
                   record.Known;
        }

        public void RemoveCell(int cellId)
        {
            map.Remove(cellId);
        }

        public void Clear()
        {
            map.Clear();
        }
    }
}
