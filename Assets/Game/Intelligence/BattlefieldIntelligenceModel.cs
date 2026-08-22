using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class BattlefieldIntelligenceRecord
    {
        public int TargetId;
        public int AreaId;

        public string Faction;
        public string UnitType;

        public TargetClassification Classification;
        public IdentificationStatus Identification;

        public float Confidence;
        public float Distance;
        public float Bearing;

        public bool Detected;
        public bool Tracked;
        public bool ContactLost;

        public DateTime LastUpdated;

        public BattlefieldIntelligenceRecord(int targetId)
        {
            TargetId = targetId;
            AreaId = -1;

            Faction = string.Empty;
            UnitType = string.Empty;

            Classification =
                TargetClassification.Unknown;

            Identification =
                IdentificationStatus.Unknown;

            Confidence = 0f;
            Distance = 0f;
            Bearing = 0f;

            Detected = false;
            Tracked = false;
            ContactLost = false;

            LastUpdated = DateTime.UtcNow;
        }
    }

    public sealed class BattlefieldIntelligenceModel
    {
        private readonly Dictionary<int, BattlefieldIntelligenceRecord> records =
            new Dictionary<int, BattlefieldIntelligenceRecord>();

        public void RegisterTarget(int targetId)
        {
            if (targetId < 0)
            {
                return;
            }

            if (!records.ContainsKey(targetId))
            {
                records.Add(
                    targetId,
                    new BattlefieldIntelligenceRecord(targetId));
            }
        }

        public void UpdateDetection(
            int targetId,
            int areaId,
            float confidence,
            float distance,
            float bearing)
        {
            RegisterTarget(targetId);

            BattlefieldIntelligenceRecord record =
                records[targetId];

            record.AreaId = areaId;
            record.Confidence =
                Math.Clamp(confidence, 0f, 1f);
            record.Distance =
                Math.Max(0f, distance);
            record.Bearing = bearing;
            record.Detected = true;
            record.ContactLost = false;
            record.LastUpdated = DateTime.UtcNow;
        }

        public void UpdateIdentification(
            int targetId,
            string faction,
            string unitType,
            IdentificationStatus identification)
        {
            RegisterTarget(targetId);

            BattlefieldIntelligenceRecord record =
                records[targetId];

            record.Faction =
                faction ?? string.Empty;

            record.UnitType =
                unitType ?? string.Empty;

            record.Identification =
                identification;

            record.LastUpdated =
                DateTime.UtcNow;
        }

        public void UpdateClassification(
            int targetId,
            TargetClassification classification)
        {
            RegisterTarget(targetId);

            BattlefieldIntelligenceRecord record =
                records[targetId];

            record.Classification =
                classification;

            record.LastUpdated =
                DateTime.UtcNow;
        }

        public void SetTracked(
            int targetId,
            bool tracked)
        {
            RegisterTarget(targetId);

            BattlefieldIntelligenceRecord record =
                records[targetId];

            record.Tracked = tracked;
            record.LastUpdated =
                DateTime.UtcNow;
        }

        public void SetContactLost(
            int targetId,
            bool lost)
        {
            RegisterTarget(targetId);

            BattlefieldIntelligenceRecord record =
                records[targetId];

            record.ContactLost = lost;

            if (lost)
            {
                record.Detected = false;
                record.Tracked = false;
            }

            record.LastUpdated =
                DateTime.UtcNow;
        }

        public bool TryGetRecord(
            int targetId,
            out BattlefieldIntelligenceRecord record)
        {
            return records.TryGetValue(
                targetId,
                out record);
        }

        public bool HasIntelligence(int targetId)
        {
            return records.TryGetValue(
                       targetId,
                       out BattlefieldIntelligenceRecord record) &&
                   record.Confidence > 0f;
        }

        public void RemoveTarget(int targetId)
        {
            records.Remove(targetId);
        }

        public void Clear()
        {
            records.Clear();
        }
    }
}
