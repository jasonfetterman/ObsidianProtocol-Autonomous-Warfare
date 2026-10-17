using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Maps
{
    [CreateAssetMenu(fileName = "MapDatabase", menuName = "Obsidian Protocol/Maps/Map Database")]
    public sealed class MapDatabase : ScriptableObject
    {
        [SerializeField] private List<MapDefinition> maps = new List<MapDefinition>();
        private Dictionary<string, MapDefinition> lookup;

        public IReadOnlyList<MapDefinition> Maps => maps;

        private void OnEnable() => RebuildLookup();

        public void RebuildLookup()
        {
            lookup = new Dictionary<string, MapDefinition>();
            foreach (var map in maps)
            {
                if (map == null || string.IsNullOrWhiteSpace(map.MapId))
                    continue;

                if (!lookup.ContainsKey(map.MapId))
                    lookup.Add(map.MapId, map);
            }
        }

        public MapDefinition GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            if (lookup == null)
                RebuildLookup();

            lookup.TryGetValue(id, out var result);
            return result;
        }

        public void Add(MapDefinition map)
        {
            if (map == null || maps.Contains(map))
                return;

            maps.Add(map);
            RebuildLookup();
        }

        public bool Remove(MapDefinition map)
        {
            bool removed = maps.Remove(map);
            if (removed)
                RebuildLookup();
            return removed;
        }
    }
}
