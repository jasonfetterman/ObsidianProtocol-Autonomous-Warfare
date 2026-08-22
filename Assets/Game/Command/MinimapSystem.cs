using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public enum MinimapMarkerType
    {
        FriendlyUnit,
        FriendlySquad,
        EnemyUnit,
        EnemySquad,
        Objective,
        Tactical,
        Resource,
        Threat
    }

    public sealed class MinimapMarker
    {
        public string MarkerId { get; }
        public MinimapMarkerType Type { get; }

        public float PositionX { get; private set; }
        public float PositionZ { get; private set; }

        public bool Visible { get; private set; }

        public MinimapMarker(
            string markerId,
            MinimapMarkerType type,
            float positionX,
            float positionZ)
        {
            MarkerId = markerId ?? string.Empty;
            Type = type;

            PositionX = positionX;
            PositionZ = positionZ;

            Visible = true;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(MarkerId);

        public void SetPosition(
            float positionX,
            float positionZ)
        {
            PositionX = positionX;
            PositionZ = positionZ;
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }
    }

    public sealed class MinimapSystem
    {
        private readonly Dictionary<string, MinimapMarker> markers =
            new Dictionary<string, MinimapMarker>(
                StringComparer.OrdinalIgnoreCase);

        public bool Visible { get; private set; }

        public MinimapSystem()
        {
            Visible = true;
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
            MinimapMarker marker)
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
            out MinimapMarker marker)
        {
            return markers.TryGetValue(
                markerId,
                out marker);
        }

        public bool SetMarkerPosition(
            string markerId,
            float positionX,
            float positionZ)
        {
            if (!markers.TryGetValue(
                    markerId,
                    out MinimapMarker marker))
            {
                return false;
            }

            marker.SetPosition(
                positionX,
                positionZ);

            return true;
        }

        public IReadOnlyCollection<MinimapMarker>
            GetMarkers()
        {
            return markers.Values;
        }

        public void Clear()
        {
            markers.Clear();
        }

        public void Reset()
        {
            Visible = true;
            markers.Clear();
        }
    }
}
