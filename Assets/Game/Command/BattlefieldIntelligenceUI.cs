using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public enum IntelligenceConfidence
    {
        Unknown,
        Low,
        Medium,
        High,
        Confirmed
    }

    public sealed class BattlefieldIntelligence
    {
        public string IntelligenceId { get; }
        public string SubjectId { get; }
        public string Category { get; }
        public string Summary { get; }

        public IntelligenceConfidence Confidence { get; private set; }

        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }

        public bool Active { get; private set; }

        public BattlefieldIntelligence(
            string intelligenceId,
            string subjectId,
            string category,
            string summary,
            IntelligenceConfidence confidence,
            float positionX,
            float positionY,
            float positionZ)
        {
            IntelligenceId =
                intelligenceId ?? string.Empty;

            SubjectId =
                subjectId ?? string.Empty;

            Category =
                category ?? string.Empty;

            Summary =
                summary ?? string.Empty;

            Confidence = confidence;

            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;

            Active = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                IntelligenceId);

        public void SetConfidence(
            IntelligenceConfidence confidence)
        {
            Confidence = confidence;
        }

        public void SetPosition(
            float positionX,
            float positionY,
            float positionZ)
        {
            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;
        }

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
    }

    public sealed class BattlefieldIntelligenceUI
    {
        private readonly Dictionary<
            string,
            BattlefieldIntelligence> intelligence =
            new Dictionary<
                string,
                BattlefieldIntelligence>(
                StringComparer.OrdinalIgnoreCase);

        public bool Visible { get; private set; }

        public BattlefieldIntelligenceUI()
        {
            Visible = false;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public bool Register(
            BattlefieldIntelligence entry)
        {
            if (entry == null ||
                !entry.Valid ||
                intelligence.ContainsKey(
                    entry.IntelligenceId))
            {
                return false;
            }

            intelligence.Add(
                entry.IntelligenceId,
                entry);

            return true;
        }

        public bool Remove(
            string intelligenceId)
        {
            if (string.IsNullOrWhiteSpace(
                    intelligenceId))
            {
                return false;
            }

            return intelligence.Remove(
                intelligenceId);
        }

        public bool TryGet(
            string intelligenceId,
            out BattlefieldIntelligence entry)
        {
            return intelligence.TryGetValue(
                intelligenceId,
                out entry);
        }

        public bool SetConfidence(
            string intelligenceId,
            IntelligenceConfidence confidence)
        {
            if (!intelligence.TryGetValue(
                    intelligenceId,
                    out BattlefieldIntelligence entry))
            {
                return false;
            }

            entry.SetConfidence(confidence);
            return true;
        }

        public IReadOnlyCollection<
            BattlefieldIntelligence>
            GetIntelligence()
        {
            return intelligence.Values;
        }

        public void Clear()
        {
            intelligence.Clear();
        }

        public void Reset()
        {
            Visible = false;
            intelligence.Clear();
        }
    }
}
