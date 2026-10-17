#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Game.Maps.Editor
{
    [Serializable] internal class MapSeedFile { public List<MapSeed> maps = new List<MapSeed>(); }
    [Serializable] internal class MapSeed
    {
        public string id, name, environment, warfareFocus, executiveSummary, deployment, coordinate;
        public List<MapTextSeed> goals = new List<MapTextSeed>();
        public ARMapSettings ar = new ARMapSettings();
        public List<MapLocationSeed> locations = new List<MapLocationSeed>();
        public List<MapTextSeed> chokepoints = new List<MapTextSeed>();
        public string terrain, vegetation, props, effects, audio, texture, engine, rendering, anchors, interaction, multiplayer;
        public List<MapLodSeed> lod = new List<MapLodSeed>();
        public List<MapTextSeed> milestones = new List<MapTextSeed>();
        public List<MapTextSeed> testing = new List<MapTextSeed>();
    }

    [Serializable] internal class MapTextSeed { public string name, description; }
    [Serializable] internal class MapLocationSeed { public string name, coordinate, sizeOrElevation, description; }
    [Serializable] internal class MapLodSeed { public string category; public int count, trianglesPerItem, totalTriangles; }

    public static class BuildMapDatabase
    {
        private const string Root = "Assets/Game/Maps/Database";
        private const string AssetRoot = Root + "/Definitions";
        private const string ResourcesRoot = "Assets/Resources";
        private const string DatabasePath = ResourcesRoot + "/MapDatabase.asset";
        private const string SeedPath = Root + "/MapDatabaseSeed.json";

        [MenuItem("Obsidian Protocol/Maps/BUILD INITIAL MAP DATABASE")]
        public static void Build()
        {
            try
            {
                EnsureFolder("Assets/Game");
                EnsureFolder("Assets/Game/Maps");
                EnsureFolder(Root);
                EnsureFolder(AssetRoot);
                EnsureFolder(ResourcesRoot);

                TextAsset seedAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(SeedPath);
                if (seedAsset == null)
                    throw new Exception("Seed file not found: " + SeedPath);

                MapSeedFile seed = JsonUtility.FromJson<MapSeedFile>(seedAsset.text);
                if (seed == null || seed.maps == null)
                    throw new Exception("Seed JSON is empty or invalid.");

                MapDatabase database = AssetDatabase.LoadAssetAtPath<MapDatabase>(DatabasePath);
                if (database == null)
                {
                    database = ScriptableObject.CreateInstance<MapDatabase>();
                    AssetDatabase.CreateAsset(database, DatabasePath);
                }

                int created = 0;
                int updated = 0;

                foreach (MapSeed map in seed.maps)
                {
                    if (string.IsNullOrWhiteSpace(map.id))
                        continue;

                    MapDefinition definition = FindById(database, map.id);

                    if (definition == null)
                    {
                        string path = AssetRoot + "/" + Safe(map.id + "_" + map.name) + ".asset";
                        definition = ScriptableObject.CreateInstance<MapDefinition>();
                        AssetDatabase.CreateAsset(definition, path);
                        database.Add(definition);
                        created++;
                    }
                    else
                    {
                        updated++;
                    }

                    Apply(definition, map);
                    EditorUtility.SetDirty(definition);
                }

                database.RebuildLookup();
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log($"MAP DATABASE COMPLETE — Seed maps: {seed.maps.Count} | Created: {created} | Updated: {updated}");
                EditorUtility.DisplayDialog(
                    "Obsidian Protocol",
                    $"Map database built.\n\nSeed maps: {seed.maps.Count}\nCreated: {created}\nUpdated: {updated}\n\nOpen: Obsidian Protocol → Maps → Map Database Manager",
                    "OK");
            }
            catch (Exception e)
            {
                Debug.LogError("MAP DATABASE BUILD FAILED: " + e);
                EditorUtility.DisplayDialog("Map Database Build Failed", e.Message, "OK");
            }
        }

        private static void Apply(MapDefinition d, MapSeed m)
        {
            var goals = new List<MapTextEntry>();
            foreach (var x in m.goals)
                goals.Add(new MapTextEntry { name = x.name, description = x.description });

            var locations = new List<MapLocation>();
            foreach (var x in m.locations)
                locations.Add(new MapLocation { name = x.name, coordinate = x.coordinate, sizeOrElevation = x.sizeOrElevation, description = x.description });

            var choke = new List<MapTextEntry>();
            foreach (var x in m.chokepoints)
                choke.Add(new MapTextEntry { name = x.name, description = x.description });

            var lod = new List<MapLodEntry>();
            foreach (var x in m.lod)
                lod.Add(new MapLodEntry { category = x.category, count = x.count, trianglesPerItem = x.trianglesPerItem, totalTriangles = x.totalTriangles });

            var milestones = new List<MapTextEntry>();
            foreach (var x in m.milestones)
                milestones.Add(new MapTextEntry { name = x.name, description = x.description });

            var testing = new List<MapTextEntry>();
            foreach (var x in m.testing)
                testing.Add(new MapTextEntry { name = x.name, description = x.description });

            d.SetIdentity(m.id, m.name, m.environment, m.warfareFocus);
            d.SetCore(
                m.executiveSummary, goals, m.deployment, m.ar, m.coordinate,
                locations, choke, m.terrain, m.vegetation, m.props, m.effects, m.audio,
                lod, m.texture, m.engine, m.rendering, m.anchors, m.interaction, m.multiplayer,
                milestones, testing);
        }

        private static MapDefinition FindById(MapDatabase database, string id)
        {
            foreach (var map in database.Maps)
                if (map != null && string.Equals(map.MapId, id, StringComparison.OrdinalIgnoreCase))
                    return map;
            return null;
        }

        private static string Safe(string value)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                value = value.Replace(c, '_');
            return value.Replace(' ', '_');
        }

        private static void EnsureFolder(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
#endif
