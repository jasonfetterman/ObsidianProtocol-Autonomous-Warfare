using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class TacticalInformation
    {
        public string InformationId { get; }
        public string SubjectId { get; }
        public string Category { get; }
        public string Message { get; }

        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }

        public float Confidence { get; }

        public bool Active { get; private set; }

        public TacticalInformation(
            string informationId,
            string subjectId,
            string category,
            string message,
            float positionX,
            float positionY,
            float positionZ,
            float confidence)
        {
            InformationId =
                informationId ?? string.Empty;

            SubjectId =
                subjectId ?? string.Empty;

            Category =
                category ?? string.Empty;

            Message =
                message ?? string.Empty;

            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;

            Confidence =
                Math.Max(
                    0f,
                    Math.Min(
                        1f,
                        confidence));

            Active = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                InformationId);

        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
    }

    public sealed class TacticalInformationSystem
    {
        private readonly Dictionary<
            string,
            TacticalInformation> information =
            new Dictionary<
                string,
                TacticalInformation>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            TacticalInformation entry)
        {
            if (entry == null ||
                !entry.Valid ||
                information.ContainsKey(
                    entry.InformationId))
            {
                return false;
            }

            information.Add(
                entry.InformationId,
                entry);

            return true;
        }

        public bool Remove(
            string informationId)
        {
            if (string.IsNullOrWhiteSpace(
                    informationId))
            {
                return false;
            }

            return information.Remove(
                informationId);
        }

        public bool TryGet(
            string informationId,
            out TacticalInformation entry)
        {
            return information.TryGetValue(
                informationId,
                out entry);
        }

        public bool Activate(
            string informationId)
        {
            if (!information.TryGetValue(
                    informationId,
                    out TacticalInformation entry))
            {
                return false;
            }

            entry.Activate();
            return true;
        }

        public bool Deactivate(
            string informationId)
        {
            if (!information.TryGetValue(
                    informationId,
                    out TacticalInformation entry))
            {
                return false;
            }

            entry.Deactivate();
            return true;
        }

        public IReadOnlyCollection<
            TacticalInformation>
            GetInformation()
        {
            return information.Values;
        }

        public void Clear()
        {
            information.Clear();
        }
    }
}
