using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public enum StrategicMapMarkerType
    {
        FriendlyForce,
        EnemyForce,
        Objective,
        Base,
        Resource,
        Threat,
        DeploymentZone,
        TacticalPosition
    }

    public sealed class StrategicMapMarker
    {
        public string MarkerId { get; }
        public StrategicMapMarkerType Type { get; }

        public float PositionX { get; private set; }
        public float PositionZ { get; private set; }

        public string Label { get; }

        public bool Visible { get; private set; }

        public StrategicMapMarker(
            string markerId,
            StrategicMapMarkerType type,
            float positionX,
            float positionZ,
            string label)
        {
            MarkerId = markerId ?? string.Empty;
            Type = type;

            PositionX = positionX;
            PositionZ = positionZ;

            Label = label ?? string.Empty;

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

    public sealed class StrategicMapSystem
    {
        private readonly Dictionary<
            string,
            StrategicMapMarker> markers =
            new Dictionary<
                string,
                StrategicMapMarker>(
                StringComparer.OrdinalIgnoreCase);

        public bool Visible { get; private set; }

        public StrategicMapSystem()
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
            StrategicMapMarker marker)
        {
            if (marker == null ||
                !marker.Valid ||
                markers.ContainsKey(
                    marker.MarkerId))
            {
                return false;
            }

            markers.Add(
                marker.MarkerId,
                marker);

            return true;
        }

        public bool Remove(
            string markerId)
        {
            if (string.IsNullOrWhiteSpace(
                    markerId))
            {
                return false;
            }

            return markers.Remove(
                markerId);
        }

        public bool TryGet(
            string markerId,
            out StrategicMapMarker marker)
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
                    out StrategicMapMarker marker))
            {
                return false;
            }

            marker.SetPosition(
                positionX,
                positionZ);

            return true;
        }

        public IReadOnlyCollection<
            StrategicMapMarker>
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
            Visible = false;
            markers.Clear();
        }
    }
}
