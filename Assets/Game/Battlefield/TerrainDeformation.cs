using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum TerrainDeformationType
    {
        None,
        Crater,
        Impact,
        Collapse,
        Excavation
    }

    public sealed class TerrainDeformationEvent
    {
        public string EventId { get; }

        public TerrainDeformationType Type { get; }

        public float Radius { get; }

        public float Depth { get; }

        public bool Applied { get; private set; }

        public TerrainDeformationEvent(
            string eventId,
            TerrainDeformationType type,
            float radius,
            float depth)
        {
            EventId =
                eventId ?? string.Empty;

            Type = type;

            Radius =
                Math.Max(0f, radius);

            Depth =
                Math.Max(0f, depth);

            Applied = false;
        }

        public bool Apply()
        {
            if (Applied ||
                Type == TerrainDeformationType.None ||
                Radius <= 0f ||
                Depth <= 0f)
            {
                return false;
            }

            Applied = true;

            return true;
        }

        public bool Clear()
        {
            if (!Applied)
            {
                return false;
            }

            Applied = false;

            return true;
        }
    }

    public sealed class TerrainDeformation
    {
        private readonly Dictionary<
            string,
            TerrainDeformationEvent> events =
            new Dictionary<
                string,
                TerrainDeformationEvent>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int EventCount =>
            events.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            events.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterEvent(
            string eventId,
            TerrainDeformationType type,
            float radius,
            float depth)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(eventId) ||
                type == TerrainDeformationType.None ||
                radius <= 0f ||
                depth <= 0f)
            {
                return false;
            }

            string id =
                eventId.Trim();

            if (events.ContainsKey(id))
            {
                return false;
            }

            events.Add(
                id,
                new TerrainDeformationEvent(
                    id,
                    type,
                    radius,
                    depth));

            return true;
        }

        public bool ApplyEvent(
            string eventId)
        {
            TerrainDeformationEvent deformation =
                GetEvent(eventId);

            return deformation != null &&
                   deformation.Apply();
        }

        public bool ClearEvent(
            string eventId)
        {
            TerrainDeformationEvent deformation =
                GetEvent(eventId);

            return deformation != null &&
                   deformation.Clear();
        }

        public TerrainDeformationEvent GetEvent(
            string eventId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(eventId))
            {
                return null;
            }

            events.TryGetValue(
                eventId.Trim(),
                out TerrainDeformationEvent deformation);

            return deformation;
        }

        public IReadOnlyCollection<
            TerrainDeformationEvent>
            GetEvents()
        {
            return events.Values;
        }

        public void Reset()
        {
            events.Clear();

            Initialized = false;
        }
    }
}
