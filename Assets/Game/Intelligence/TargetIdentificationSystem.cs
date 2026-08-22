using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum IdentificationStatus
    {
        Unknown,
        Suspected,
        Identified,
        Confirmed
    }

    public sealed class TargetIdentificationRecord
    {
        public int TargetId;
        public IdentificationStatus Status;
        public string Faction;
        public string UnitType;
        public float Confidence;

        public TargetIdentificationRecord(int targetId)
        {
            TargetId = targetId;
            Status = IdentificationStatus.Unknown;
            Faction = string.Empty;
            UnitType = string.Empty;
            Confidence = 0f;
        }
    }

    public sealed class TargetIdentificationSystem
    {
        private readonly Dictionary<int, TargetIdentificationRecord> targets =
            new Dictionary<int, TargetIdentificationRecord>();

        public void RegisterTarget(int targetId)
        {
            if (targetId < 0)
            {
                return;
            }

            if (!targets.ContainsKey(targetId))
            {
                targets.Add(
                    targetId,
                    new TargetIdentificationRecord(targetId));
            }
        }

        public void IdentifyTarget(
            int targetId,
            string faction,
            string unitType,
            float confidence,
            IdentificationStatus status)
        {
            RegisterTarget(targetId);

            TargetIdentificationRecord record =
                targets[targetId];

            record.Faction = faction ?? string.Empty;
            record.UnitType = unitType ?? string.Empty;
            record.Confidence =
                Math.Clamp(confidence, 0f, 1f);
            record.Status = status;
        }

        public bool IsIdentified(int targetId)
        {
            return targets.TryGetValue(
                       targetId,
                       out TargetIdentificationRecord record) &&
                   record.Status ==
                       IdentificationStatus.Identified ||
                   targets.TryGetValue(
                       targetId,
                       out record) &&
                   record.Status ==
                       IdentificationStatus.Confirmed;
        }

        public bool IsConfirmed(int targetId)
        {
            return targets.TryGetValue(
                       targetId,
                       out TargetIdentificationRecord record) &&
                   record.Status ==
                       IdentificationStatus.Confirmed;
        }

        public bool TryGetTarget(
            int targetId,
            out TargetIdentificationRecord record)
        {
            return targets.TryGetValue(
                targetId,
                out record);
        }

        public void RemoveTarget(int targetId)
        {
            targets.Remove(targetId);
        }

        public void Clear()
        {
            targets.Clear();
        }
    }
}
