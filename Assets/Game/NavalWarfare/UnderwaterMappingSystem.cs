using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.NavalWarfare
{
    public enum UnderwaterMapFeature
    {
        Seafloor,
        DepthChange,
        Obstacle,
        Wreck,
        Structure,
        Passage,
        Unknown
    }

    public sealed class UnderwaterMapPoint
    {
        public string PointId { get; }

        public UnderwaterMapFeature Feature { get; }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public float Depth { get; private set; }
        public float Confidence { get; private set; }

        public UnderwaterMapPoint(
            string pointId,
            UnderwaterMapFeature feature,
            float x,
            float y,
            float z,
            float depth,
            float confidence)
        {
            PointId =
                pointId ?? string.Empty;

            Feature =
                feature;

            X = x;
            Y = y;
            Z = z;

            Depth =
                Math.Max(
                    0f,
                    depth);

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);
        }

        public void Update(
            float depth,
            float confidence)
        {
            Depth =
                Math.Max(
                    0f,
                    depth);

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);
        }
    }

    public sealed class UnderwaterMap
    {
        public string MapId { get; }

        private readonly Dictionary<string, UnderwaterMapPoint> points =
            new Dictionary<string, UnderwaterMapPoint>(
                StringComparer.OrdinalIgnoreCase);

        public UnderwaterMap(
            string mapId)
        {
            MapId =
                mapId ?? string.Empty;
        }

        public void AddPoint(
            string pointId,
            UnderwaterMapFeature feature,
            float x,
            float y,
            float z,
            float depth,
            float confidence)
        {
            if (string.IsNullOrWhiteSpace(pointId))
            {
                return;
            }

            points[pointId] =
                new UnderwaterMapPoint(
                    pointId,
                    feature,
                    x,
                    y,
                    z,
                    depth,
                    confidence);
        }

        public void UpdatePoint(
            string pointId,
            float depth,
            float confidence)
        {
            if (points.TryGetValue(
                    pointId,
                    out UnderwaterMapPoint point))
            {
                point.Update(
                    depth,
                    confidence);
            }
        }

        public bool TryGetPoint(
            string pointId,
            out UnderwaterMapPoint point)
        {
            return points.TryGetValue(
                pointId,
                out point);
        }

        public IReadOnlyCollection<UnderwaterMapPoint> GetPoints()
        {
            return points.Values;
        }

        public void RemovePoint(
            string pointId)
        {
            points.Remove(pointId);
        }

        public void Clear()
        {
            points.Clear();
        }
    }

    public sealed class UnderwaterMappingSystem
    {
        private readonly Dictionary<string, UnderwaterMap> maps =
            new Dictionary<string, UnderwaterMap>(
                StringComparer.OrdinalIgnoreCase);

        public void CreateMap(
            string mapId)
        {
            if (string.IsNullOrWhiteSpace(mapId))
            {
                return;
            }

            maps[mapId] =
                new UnderwaterMap(mapId);
        }

        public void AddPoint(
            string mapId,
            string pointId,
            UnderwaterMapFeature feature,
            float x,
            float y,
            float z,
            float depth,
            float confidence)
        {
            if (!maps.TryGetValue(
                    mapId,
                    out UnderwaterMap map))
            {
                return;
            }

            map.AddPoint(
                pointId,
                feature,
                x,
                y,
                z,
                depth,
                confidence);
        }

        public void UpdatePoint(
            string mapId,
            string pointId,
            float depth,
            float confidence)
        {
            if (maps.TryGetValue(
                    mapId,
                    out UnderwaterMap map))
            {
                map.UpdatePoint(
                    pointId,
                    depth,
                    confidence);
            }
        }

        public bool TryGetMap(
            string mapId,
            out UnderwaterMap map)
        {
            return maps.TryGetValue(
                mapId,
                out map);
        }

        public void RemoveMap(
            string mapId)
        {
            maps.Remove(mapId);
        }

        public void Clear()
        {
            maps.Clear();
        }
    }
}
