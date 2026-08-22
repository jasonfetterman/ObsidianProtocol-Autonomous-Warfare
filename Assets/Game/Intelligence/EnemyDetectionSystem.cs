using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class EnemyDetectionRecord
    {
        public int TargetId;
        public int DetectingUnitId;
        public float Distance;
        public DetectionConfidence Confidence;
        public bool IsConfirmed;

        public EnemyDetectionRecord(
            int targetId,
            int detectingUnitId,
            float distance,
            DetectionConfidence confidence)
        {
            TargetId = targetId;
            DetectingUnitId = detectingUnitId;
            Distance = distance;
            Confidence = confidence;
            IsConfirmed =
                confidence == DetectionConfidence.Confirmed;
        }
    }

    public sealed class EnemyDetectionSystem
    {
        private readonly Dictionary<int, EnemyDetectionRecord> enemies =
            new Dictionary<int, EnemyDetectionRecord>();

        public void ReportEnemy(
            int targetId,
            int detectingUnitId,
            float distance,
            DetectionConfidence confidence)
        {
            if (targetId < 0 ||
                detectingUnitId < 0 ||
                confidence == DetectionConfidence.None)
            {
                return;
            }

            enemies[targetId] =
                new EnemyDetectionRecord(
                    targetId,
                    detectingUnitId,
                    distance,
                    confidence);
        }

        public bool IsEnemyDetected(int targetId)
        {
            return enemies.ContainsKey(targetId);
        }

        public bool IsConfirmedEnemy(int targetId)
        {
            return enemies.TryGetValue(
                       targetId,
                       out EnemyDetectionRecord record) &&
                   record.IsConfirmed;
        }

        public bool TryGetEnemy(
            int targetId,
            out EnemyDetectionRecord record)
        {
            return enemies.TryGetValue(
                targetId,
                out record);
        }

        public void ClearEnemy(int targetId)
        {
            enemies.Remove(targetId);
        }

        public void Clear()
        {
            enemies.Clear();
        }
    }
}
