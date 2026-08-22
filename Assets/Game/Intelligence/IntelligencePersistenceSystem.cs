using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public sealed class IntelligenceMemory
    {
        public int TargetId;
        public int AreaId;
        public string InformationType;
        public float Confidence;
        public DateTime LastKnownTime;
        public bool Valid;

        public IntelligenceMemory(
            int targetId,
            int areaId,
            string informationType,
            float confidence)
        {
            TargetId = targetId;
            AreaId = areaId;
            InformationType =
                informationType ?? string.Empty;
            Confidence =
                Math.Clamp(confidence, 0f, 1f);
            LastKnownTime = DateTime.UtcNow;
            Valid = true;
        }
    }

    public sealed class IntelligencePersistenceSystem
    {
        private readonly Dictionary<int, IntelligenceMemory> memory =
            new Dictionary<int, IntelligenceMemory>();

        public void Store(
            int targetId,
            int areaId,
            string informationType,
            float confidence)
        {
            if (targetId < 0)
            {
                return;
            }

            if (!memory.TryGetValue(
                    targetId,
                    out IntelligenceMemory entry))
            {
                entry =
                    new IntelligenceMemory(
                        targetId,
                        areaId,
                        informationType,
                        confidence);

                memory.Add(
                    targetId,
                    entry);

                return;
            }

            entry.AreaId = areaId;
            entry.InformationType =
                informationType ?? string.Empty;
            entry.Confidence =
                Math.Clamp(confidence, 0f, 1f);
            entry.LastKnownTime =
                DateTime.UtcNow;
            entry.Valid = true;
        }

        public bool TryGetMemory(
            int targetId,
            out IntelligenceMemory entry)
        {
            return memory.TryGetValue(
                targetId,
                out entry) &&
                entry.Valid;
        }

        public bool HasMemory(int targetId)
        {
            return memory.TryGetValue(
                       targetId,
                       out IntelligenceMemory entry) &&
                   entry.Valid;
        }

        public void Invalidate(int targetId)
        {
            if (memory.TryGetValue(
                    targetId,
                    out IntelligenceMemory entry))
            {
                entry.Valid = false;
            }
        }

        public void Remove(int targetId)
        {
            memory.Remove(targetId);
        }

        public void Clear()
        {
            memory.Clear();
        }
    }
}
