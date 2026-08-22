using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.CommandUnits
{
    public enum WorldMapFeatureType
    {
        Terrain,
        FriendlyUnit,
        AlliedUnit,
        UnknownUnit,
        EnemyUnit,
        Resource,
        Objective,
        Structure,
        Hazard,
        NavigationPoint
    }

    public sealed class WorldMapFeature
    {
        public string FeatureId { get; }
        public WorldMapFeatureType Type { get; }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public float Confidence { get; private set; }
        public bool Active { get; private set; }

        public WorldMapFeature(
            string featureId,
            WorldMapFeatureType type,
            float x,
            float y,
            float z,
            float confidence)
        {
            FeatureId =
                featureId ?? string.Empty;

            Type =
                type;

            Update(
                x,
                y,
                z,
                confidence);
        }

        public void Update(
            float x,
            float y,
            float z,
            float confidence)
        {
            X = x;
            Y = y;
            Z = z;

            Confidence =
                Math.Clamp(
                    confidence,
                    0f,
                    1f);

            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
    }

    public sealed class WorldMap
    {
        public string MapId { get; }

        private readonly Dictionary<string, WorldMapFeature> features =
            new Dictionary<string, WorldMapFeature>(
                StringComparer.OrdinalIgnoreCase);

        public WorldMap(
            string mapId)
        {
            MapId =
                mapId ?? string.Empty;
        }

        public void RegisterFeature(
            string featureId,
            WorldMapFeatureType type,
            float x,
            float y,
            float z,
            float confidence)
        {
            if (string.IsNullOrWhiteSpace(featureId))
            {
                return;
            }

            if (features.TryGetValue(
                    featureId,
                    out WorldMapFeature existing))
            {
                existing.Update(
                    x,
                    y,
                    z,
                    confidence);

                return;
            }

            features.Add(
                featureId,
                new WorldMapFeature(
                    featureId,
                    type,
                    x,
                    y,
                    z,
                    confidence));
        }

        public bool TryGetFeature(
            string featureId,
            out WorldMapFeature feature)
        {
            return features.TryGetValue(
                featureId,
                out feature);
        }

        public void RemoveFeature(
            string featureId)
        {
            features.Remove(featureId);
        }

        public IReadOnlyCollection<WorldMapFeature> GetFeatures()
        {
            return features.Values;
        }

        public void Clear()
        {
            features.Clear();
        }
    }

    public sealed class WorldMappingSystem
    {
        private readonly Dictionary<string, WorldMap> maps =
            new Dictionary<string, WorldMap>(
                StringComparer.OrdinalIgnoreCase);

        public void CreateMap(
            string mapId)
        {
            if (string.IsNullOrWhiteSpace(mapId))
            {
                return;
            }

            if (!maps.ContainsKey(mapId))
            {
                maps.Add(
                    mapId,
                    new WorldMap(mapId));
            }
        }

        public void RegisterFeature(
            string mapId,
            string featureId,
            WorldMapFeatureType type,
            float x,
            float y,
            float z,
            float confidence)
        {
            if (!maps.TryGetValue(
                    mapId,
                    out WorldMap map))
            {
                return;
            }

            map.RegisterFeature(
                featureId,
                type,
                x,
                y,
                z,
                confidence);
        }

        public bool TryGetMap(
            string mapId,
            out WorldMap map)
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
