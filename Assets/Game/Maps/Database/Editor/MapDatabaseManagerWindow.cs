#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Game.Maps.Editor
{
    public sealed class MapDatabaseManagerWindow : EditorWindow
    {
        private MapDatabase database;
        private Vector2 listScroll;
        private Vector2 inspectorScroll;
        private string search = "";
        private int selectedIndex = -1;

        [MenuItem("Obsidian Protocol/Maps/Map Database Manager")]
        public static void Open()
        {
            var window = GetWindow<MapDatabaseManagerWindow>("Map Database Manager");
            window.minSize = new Vector2(1100, 650);
            window.LoadDatabase();
            window.Show();
        }

        private void OnEnable() => LoadDatabase();

        private void LoadDatabase()
        {
            string[] guids = AssetDatabase.FindAssets("t:MapDatabase");
            if (guids.Length == 0)
            {
                database = null;
                selectedIndex = -1;
                return;
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            database = AssetDatabase.LoadAssetAtPath<MapDatabase>(path);

            if (database != null && selectedIndex >= database.Maps.Count)
                selectedIndex = database.Maps.Count - 1;
        }

        private void OnGUI()
        {
            DrawToolbar();

            if (database == null)
            {
                EditorGUILayout.HelpBox(
                    "No MapDatabase asset exists yet. Build the initial database first.",
                    MessageType.Info);

                if (GUILayout.Button("BUILD INITIAL MAP DATABASE", GUILayout.Height(32)))
                {
                    BuildMapDatabase.Build();
                    LoadDatabase();
                }
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawMapList();
                DrawInspector();
            }
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("OBSIDIAN PROTOCOL — MAP DATABASE", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Rebuild From Seed", EditorStyles.toolbarButton))
                {
                    if (EditorUtility.DisplayDialog(
                        "Rebuild Map Database",
                        "This updates the seeded maps from MapDatabaseSeed.json. Manually created maps are not deleted.",
                        "Rebuild", "Cancel"))
                    {
                        BuildMapDatabase.Build();
                        LoadDatabase();
                    }
                }

                if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                {
                    AssetDatabase.Refresh();
                    LoadDatabase();
                    Repaint();
                }
            }
        }

        private void DrawMapList()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(330)))
            {
                EditorGUILayout.LabelField("MAPS", EditorStyles.boldLabel);

                using (new EditorGUILayout.HorizontalScope())
                {
                    search = EditorGUILayout.TextField(search, GUI.skin.FindStyle("ToolbarSearchTextField") ?? GUI.skin.textField);

                    if (GUILayout.Button("+", GUILayout.Width(28)))
                        CreateMap();
                }

                listScroll = EditorGUILayout.BeginScrollView(listScroll);

                var filtered = database.Maps
                    .Select((map, index) => new { map, index })
                    .Where(x => x.map != null &&
                        (string.IsNullOrWhiteSpace(search) ||
                         Contains(x.map.MapId, search) ||
                         Contains(x.map.DisplayName, search) ||
                         Contains(x.map.Environment, search) ||
                         Contains(x.map.WarfareFocus, search)))
                    .ToList();

                foreach (var item in filtered)
                {
                    bool selected = selectedIndex == item.index;
                    string label = $"{item.map.MapId}  —  {item.map.DisplayName}";

                    if (GUILayout.Toggle(selected, label, "Button"))
                    {
                        selectedIndex = item.index;
                        GUI.FocusControl(null);
                    }
                }

                EditorGUILayout.EndScrollView();

                using (new EditorGUILayout.HorizontalScope())
                {
                    GUI.enabled = selectedIndex >= 0 && selectedIndex < database.Maps.Count;

                    if (GUILayout.Button("Duplicate"))
                        DuplicateSelected();

                    if (GUILayout.Button("Delete"))
                        DeleteSelected();

                    GUI.enabled = true;
                }
            }
        }

        private void DrawInspector()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                if (selectedIndex < 0 || selectedIndex >= database.Maps.Count || database.Maps[selectedIndex] == null)
                {
                    EditorGUILayout.HelpBox("Select a map.", MessageType.Info);
                    return;
                }

                MapDefinition selected = database.Maps[selectedIndex];

                EditorGUILayout.LabelField(
                    $"{selected.MapId} — {selected.DisplayName}",
                    EditorStyles.boldLabel);

                inspectorScroll = EditorGUILayout.BeginScrollView(inspectorScroll);

                SerializedObject so = new SerializedObject(selected);
                so.Update();

                SerializedProperty property = so.GetIterator();
                bool enterChildren = true;

                while (property.NextVisible(enterChildren))
                {
                    enterChildren = false;

                    if (property.name == "m_Script")
                        continue;

                    EditorGUILayout.PropertyField(property, true);
                }

                if (so.ApplyModifiedProperties())
                {
                    EditorUtility.SetDirty(selected);
                    database.RebuildLookup();
                    EditorUtility.SetDirty(database);
                }

                EditorGUILayout.EndScrollView();

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Save"))
                    {
                        EditorUtility.SetDirty(selected);
                        EditorUtility.SetDirty(database);
                        AssetDatabase.SaveAssets();
                    }

                    if (GUILayout.Button("Ping Asset"))
                        EditorGUIUtility.PingObject(selected);

                    if (GUILayout.Button("Select Asset"))
                    {
                        Selection.activeObject = selected;
                        EditorGUIUtility.PingObject(selected);
                    }
                }
            }
        }

        private void CreateMap()
        {
            string id = GetNextId();
            string path = EditorUtility.SaveFilePanelInProject(
                "Create Map Definition",
                id + "_NewMap",
                "asset",
                "Choose where to create the map definition.",
                "Assets/Game/Maps/Database/Definitions");

            if (string.IsNullOrEmpty(path))
                return;

            var map = CreateInstance<MapDefinition>();
            map.SetIdentity(id, "New Map", "Unspecified", "Unspecified");

            AssetDatabase.CreateAsset(map, path);
            database.Add(map);
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            selectedIndex = FindIndex(map);
        }

        private void DuplicateSelected()
        {
            if (!IsSelectionValid())
                return;

            MapDefinition source = database.Maps[selectedIndex];
            string sourcePath = AssetDatabase.GetAssetPath(source);
            string newPath = AssetDatabase.GenerateUniqueAssetPath(sourcePath);

            if (!AssetDatabase.CopyAsset(sourcePath, newPath))
            {
                EditorUtility.DisplayDialog("Duplicate Failed", "Unity could not duplicate the map asset.", "OK");
                return;
            }

            AssetDatabase.Refresh();

            var duplicate = AssetDatabase.LoadAssetAtPath<MapDefinition>(newPath);
            database.Add(duplicate);
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();

            selectedIndex = FindIndex(duplicate);
        }

        private void DeleteSelected()
        {
            if (!IsSelectionValid())
                return;

            MapDefinition map = database.Maps[selectedIndex];

            if (!EditorUtility.DisplayDialog(
                "Delete Map",
                $"Delete '{map.DisplayName}'?\n\nThis deletes the map definition asset.",
                "Delete", "Cancel"))
                return;

            string path = AssetDatabase.GetAssetPath(map);
            database.Remove(map);
            EditorUtility.SetDirty(database);

            if (!string.IsNullOrEmpty(path))
                AssetDatabase.DeleteAsset(path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            selectedIndex = Mathf.Clamp(selectedIndex - 1, -1, database.Maps.Count - 1);
        }


        private int FindIndex(MapDefinition target)
        {
            for (int i = 0; i < database.Maps.Count; i++)
                if (database.Maps[i] == target)
                    return i;
            return -1;
        }

        private bool IsSelectionValid()
        {
            return database != null &&
                   selectedIndex >= 0 &&
                   selectedIndex < database.Maps.Count &&
                   database.Maps[selectedIndex] != null;
        }

        private string GetNextId()
        {
            int max = 0;
            foreach (var map in database.Maps)
            {
                if (map == null || string.IsNullOrWhiteSpace(map.MapId))
                    continue;

                string digits = new string(map.MapId.Where(char.IsDigit).ToArray());
                if (int.TryParse(digits, out int n))
                    max = Mathf.Max(max, n);
            }

            return $"MAP-{max + 1:000}";
        }

        private static bool Contains(string value, string query)
        {
            return !string.IsNullOrEmpty(value) &&
                   value.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
#endif
