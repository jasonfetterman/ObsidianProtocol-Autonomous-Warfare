using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class TacticalMarker
    {
        public string MarkerId { get; }
        public TacticalMarkerType Type { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }
        public string Label { get; }

        public TacticalMarker(
            string markerId,
            TacticalMarkerType type,
            float positionX,
            float positionY,
            float positionZ,
            string label)
        {
            MarkerId = markerId ?? string.Empty;
            Type = type;

            PositionX = positionX;
            PositionY = positionY;
            PositionZ = positionZ;

            Label = label ?? string.Empty;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(MarkerId);
    }

    public sealed class TacticalMarkerSystem
    {
        private readonly Dictionary<string, TacticalMarker> markers =
            new Dictionary<string, TacticalMarker>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(TacticalMarker marker)
        {
            if (marker == null ||
                !marker.Valid ||
                markers.ContainsKey(marker.MarkerId))
            {
                return false;
            }

            markers.Add(
                marker.MarkerId,
                marker);

            return true;
        }

        public bool Remove(string markerId)
        {
            if (string.IsNullOrWhiteSpace(markerId))
                return false;

            return markers.Remove(markerId);
        }

        public bool TryGet(
            string markerId,
            out TacticalMarker marker)
        {
            return markers.TryGetValue(
                markerId,
                out marker);
        }

        public IReadOnlyCollection<TacticalMarker>
            GetMarkers()
        {
            return markers.Values;
        }

        public void Clear()
        {
            markers.Clear();
        }
    }
}
