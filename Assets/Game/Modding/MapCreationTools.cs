using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class MapCreationDefinition
    {
        public string MapId { get; }

        public string MapName { get; }

        public string RegionType { get; }

        public bool Enabled { get; private set; }

        public MapCreationDefinition(
            string mapId,
            string mapName,
            string regionType)
        {
            MapId =
                mapId ?? string.Empty;

            MapName =
                mapName ?? string.Empty;

            RegionType =
                regionType ?? string.Empty;

            Enabled = true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class MapCreationTools
    {
        private readonly Dictionary<
            string,
            MapCreationDefinition> maps =
            new Dictionary<
                string,
                MapCreationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int MapCount =>
            maps.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            maps.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateMap(
            string mapId,
            string mapName,
            string regionType)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(mapId) ||
                string.IsNullOrWhiteSpace(mapName) ||
                string.IsNullOrWhiteSpace(regionType))
            {
                return false;
            }

            string id =
                mapId.Trim();

            if (maps.ContainsKey(id))
            {
                return false;
            }

            maps.Add(
                id,
                new MapCreationDefinition(
                    id,
                    mapName.Trim(),
                    regionType.Trim()));

            return true;
        }

        public bool RemoveMap(
            string mapId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(mapId))
            {
                return false;
            }

            return maps.Remove(
                mapId.Trim());
        }

        public MapCreationDefinition GetMap(
            string mapId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(mapId))
            {
                return null;
            }

            maps.TryGetValue(
                mapId.Trim(),
                out MapCreationDefinition map);

            return map;
        }

        public IReadOnlyCollection<
            MapCreationDefinition>
            GetMaps()
        {
            return maps.Values;
        }

        public void Reset()
        {
            maps.Clear();
            Initialized = false;
        }
    }
}
