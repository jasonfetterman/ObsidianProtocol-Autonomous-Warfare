using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using ObsidianProtocol.Game.Units;

public sealed class UnitDatabaseManagerWindow : EditorWindow
{
    private const string CanonicalDatabasePath =
        "Assets/Game/Units/Database/UnitDatabase.asset";

    private const string RuntimeDatabasePath =
        "Assets/Resources/UnitDatabase.asset";

    private UnitDatabase database;

    private string searchText = string.Empty;

    private int categoryIndex = 0;

    private Vector2 scrollPosition;

    private string newUnitName = string.Empty;
    private string newUnitId = string.Empty;
    private UnitCategory newUnitCategory = UnitCategory.GroundUnit;

    private static readonly string[] CategoryNames =
    {
        "ALL",
        "AirUnit",
        "GroundUnit",
        "Fighter",
        "Station",
        "Satellite",
        "Naval",
        "Transport",
        "Recon",
        "Shuttle",
        "Medical",
        "Exploration"
    };

    [MenuItem("Obsidian Protocol/Units/UNIT DATABASE MANAGER")]
    public static void Open()
    {
        UnitDatabaseManagerWindow window =
            GetWindow<UnitDatabaseManagerWindow>(
                "Unit Database Manager");

        window.minSize =
            new Vector2(900f, 600f);

        window.Show();
    }

    private void OnEnable()
    {
        LoadDatabase();
    }

    private void LoadDatabase()
    {
        database =
            AssetDatabase.LoadAssetAtPath<UnitDatabase>(
                CanonicalDatabasePath);
    }

    private void OnGUI()
    {
        if (database == null)
        {
            EditorGUILayout.HelpBox(
                "Canonical UnitDatabase.asset was not found.",
                MessageType.Error);

            if (GUILayout.Button("Reload Database"))
                LoadDatabase();

            return;
        }

        DrawHeader();
        DrawNewUnitPanel();
        DrawFilters();
        DrawUnitList();
    }

    private void DrawHeader()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            "OBSIDIAN PROTOCOL — MASTER UNIT DATABASE",
            EditorStyles.boldLabel);

        GUILayout.FlexibleSpace();

        EditorGUILayout.LabelField(
            "UNITS: " + database.Units.Count,
            GUILayout.Width(100f));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(6f);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            "Canonical:",
            GUILayout.Width(70f));

        EditorGUILayout.SelectableLabel(
            CanonicalDatabasePath,
            EditorStyles.textField,
            GUILayout.Height(18f));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            "Runtime:",
            GUILayout.Width(70f));

        EditorGUILayout.SelectableLabel(
            RuntimeDatabasePath,
            EditorStyles.textField,
            GUILayout.Height(18f));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(8f);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(
            "REFRESH",
            GUILayout.Height(30f)))
        {
            LoadDatabase();
            Repaint();
        }

        if (GUILayout.Button(
            "SYNC RUNTIME DATABASE",
            GUILayout.Height(30f)))
        {
            SyncRuntimeDatabase();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10f);
    }

    private void DrawNewUnitPanel()
    {
        EditorGUILayout.BeginVertical(
            EditorStyles.helpBox);

        EditorGUILayout.LabelField(
            "ADD NEW UNIT",
            EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        newUnitName =
            EditorGUILayout.TextField(
                "Display Name",
                newUnitName);

        newUnitId =
            EditorGUILayout.TextField(
                "Unit ID",
                newUnitId);

        newUnitCategory =
            (UnitCategory)EditorGUILayout.EnumPopup(
                "Category",
                newUnitCategory,
                GUILayout.Width(260f));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(
            "CREATE NEW UNIT",
            GUILayout.Height(28f)))
        {
            CreateNewUnit();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10f);
    }

    private void DrawFilters()
    {
        EditorGUILayout.BeginHorizontal();

        searchText =
            EditorGUILayout.TextField(
                "Search",
                searchText);

        categoryIndex =
            EditorGUILayout.Popup(
                "Category",
                categoryIndex,
                CategoryNames,
                GUILayout.Width(300f));

        if (GUILayout.Button(
            "CLEAR",
            GUILayout.Width(80f)))
        {
            searchText = string.Empty;
            categoryIndex = 0;
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(6f);
    }

    private void DrawUnitList()
    {
        scrollPosition =
            EditorGUILayout.BeginScrollView(
                scrollPosition);

        int visibleCount = 0;

        foreach (UnitDefinition unit in database.Units)
        {
            if (unit == null)
                continue;

            if (!MatchesFilter(unit))
                continue;

            DrawUnitRow(unit);

            visibleCount++;
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(4f);

        EditorGUILayout.LabelField(
            "VISIBLE UNITS: " + visibleCount +
            " / " + database.Units.Count,
            EditorStyles.miniLabel);
    }

    private bool MatchesFilter(UnitDefinition unit)
    {
        if (categoryIndex > 0)
        {
            string selectedCategory =
                CategoryNames[categoryIndex];

            if (unit.Category.ToString() !=
                selectedCategory)
            {
                return false;
            }
        }

        if (string.IsNullOrWhiteSpace(searchText))
            return true;

        string search =
            searchText.Trim();

        return
            unit.DisplayName.IndexOf(
                search,
                StringComparison.OrdinalIgnoreCase) >= 0
            ||
            unit.UnitId.IndexOf(
                search,
                StringComparison.OrdinalIgnoreCase) >= 0
            ||
            unit.Role.IndexOf(
                search,
                StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void DrawUnitRow(UnitDefinition unit)
    {
        EditorGUILayout.BeginVertical(
            EditorStyles.helpBox);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(
            unit.DisplayName,
            EditorStyles.boldLabel,
            GUILayout.Width(180f));

        EditorGUILayout.LabelField(
            unit.UnitId,
            GUILayout.Width(220f));

        EditorGUILayout.LabelField(
            unit.Category.ToString(),
            GUILayout.Width(110f));

        EditorGUILayout.LabelField(
            unit.Role,
            GUILayout.Width(180f));

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(
            "EDIT",
            GUILayout.Width(65f)))
        {
            SelectUnit(unit);
        }

        if (GUILayout.Button(
            "DUPLICATE",
            GUILayout.Width(85f)))
        {
            DuplicateUnit(unit);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void SelectUnit(UnitDefinition unit)
    {
        Selection.activeObject = unit;
        EditorGUIUtility.PingObject(unit);
    }

    private void CreateNewUnit()
    {
        string displayName =
            newUnitName.Trim();

        string unitId =
            newUnitId.Trim();

        if (string.IsNullOrWhiteSpace(displayName))
        {
            EditorUtility.DisplayDialog(
                "Create Unit",
                "Display Name is required.",
                "OK");

            return;
        }

        if (string.IsNullOrWhiteSpace(unitId))
        {
            EditorUtility.DisplayDialog(
                "Create Unit",
                "Unit ID is required.",
                "OK");

            return;
        }

        foreach (UnitDefinition existing in database.Units)
        {
            if (existing == null)
                continue;

            if (string.Equals(
                existing.UnitId,
                unitId,
                StringComparison.OrdinalIgnoreCase))
            {
                EditorUtility.DisplayDialog(
                    "Create Unit",
                    "A unit with this Unit ID already exists.",
                    "OK");

                return;
            }
        }

        string folder =
            "Assets/Game/Units/Database/" +
            newUnitCategory;

        if (!AssetDatabase.IsValidFolder(folder))
        {
            string parent =
                "Assets/Game/Units/Database";

            AssetDatabase.CreateFolder(
                parent,
                newUnitCategory.ToString());
        }

        string safeFileName =
            MakeSafeFileName(displayName);

        string assetPath =
            folder + "/" +
            safeFileName +
            ".asset";

        assetPath =
            AssetDatabase.GenerateUniqueAssetPath(
                assetPath);

        UnitDefinition newUnit =
            CreateInstance<UnitDefinition>();

        SerializedObject serialized =
            new SerializedObject(newUnit);

        serialized.FindProperty("unitId").stringValue =
            unitId;

        serialized.FindProperty("displayName").stringValue =
            displayName;

        serialized.FindProperty("category").enumValueIndex =
            (int)newUnitCategory;

        serialized.FindProperty("role").stringValue =
            newUnitCategory + " Unit";

        serialized.FindProperty("tier").stringValue =
            "Standard";

        serialized.FindProperty("status").stringValue =
            "ACTIVE";

        serialized.ApplyModifiedPropertiesWithoutUndo();

        AssetDatabase.CreateAsset(
            newUnit,
            assetPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        AddUnitToDatabase(newUnit);

        newUnitName = string.Empty;
        newUnitId = string.Empty;

        SelectUnit(newUnit);

        Debug.Log(
            "[UNIT DATABASE MANAGER] NEW UNIT CREATED // " +
            displayName +
            " // " +
            unitId +
            " // " +
            newUnitCategory);
    }

    private void DuplicateUnit(UnitDefinition source)
    {
        if (source == null)
            return;

        string sourcePath =
            AssetDatabase.GetAssetPath(source);

        if (string.IsNullOrWhiteSpace(sourcePath))
            return;

        string directory =
            Path.GetDirectoryName(sourcePath)
                .Replace("\\", "/");

        string baseName =
            source.DisplayName + "_Copy";

        string destination =
            directory + "/" +
            MakeSafeFileName(baseName) +
            ".asset";

        destination =
            AssetDatabase.GenerateUniqueAssetPath(
                destination);

        if (!AssetDatabase.CopyAsset(
            sourcePath,
            destination))
        {
            EditorUtility.DisplayDialog(
                "Duplicate Unit",
                "Failed to duplicate the unit asset.",
                "OK");

            return;
        }

        AssetDatabase.ImportAsset(destination);

        UnitDefinition duplicate =
            AssetDatabase.LoadAssetAtPath<UnitDefinition>(
                destination);

        if (duplicate == null)
        {
            Debug.LogError(
                "[UNIT DATABASE MANAGER] DUPLICATE CREATED " +
                "BUT COULD NOT BE LOADED.");

            return;
        }

        SerializedObject serialized =
            new SerializedObject(duplicate);

        string duplicateId =
            source.UnitId + "-copy";

        int suffix = 2;

        while (UnitIdExists(
            duplicateId,
            duplicate))
        {
            duplicateId =
                source.UnitId +
                "-copy-" +
                suffix;

            suffix++;
        }

        serialized.FindProperty("unitId").stringValue =
            duplicateId;

        serialized.FindProperty("displayName").stringValue =
            source.DisplayName + " Copy";

        serialized.ApplyModifiedPropertiesWithoutUndo();

        AddUnitToDatabase(duplicate);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        SelectUnit(duplicate);

        Debug.Log(
            "[UNIT DATABASE MANAGER] UNIT DUPLICATED // " +
            duplicate.DisplayName +
            " // " +
            duplicate.UnitId);
    }

    private bool UnitIdExists(
        string unitId,
        UnitDefinition ignored)
    {
        foreach (UnitDefinition unit in database.Units)
        {
            if (unit == null || unit == ignored)
                continue;

            if (string.Equals(
                unit.UnitId,
                unitId,
                StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private void AddUnitToDatabase(
        UnitDefinition unit)
    {
        if (unit == null)
            return;

        SerializedObject serialized =
            new SerializedObject(database);

        SerializedProperty units =
            serialized.FindProperty("units");

        for (int i = 0; i < units.arraySize; i++)
        {
            if (units.GetArrayElementAtIndex(i).objectReferenceValue ==
                unit)
            {
                serialized.ApplyModifiedPropertiesWithoutUndo();
                database.RebuildLookup();
                SyncRuntimeDatabase();
                return;
            }
        }

        units.arraySize++;

        units.GetArrayElementAtIndex(
            units.arraySize - 1).objectReferenceValue =
            unit;

        serialized.ApplyModifiedPropertiesWithoutUndo();

        database.RebuildLookup();

        SyncRuntimeDatabase();
    }

    private void SyncRuntimeDatabase()
    {
        if (database == null)
            return;

        UnitDatabase runtimeDatabase =
            AssetDatabase.LoadAssetAtPath<UnitDatabase>(
                RuntimeDatabasePath);

        if (runtimeDatabase == null)
        {
            Debug.LogError(
                "[UNIT DATABASE MANAGER] RUNTIME DATABASE NOT FOUND.");

            return;
        }

        SerializedObject source =
            new SerializedObject(database);

        SerializedObject target =
            new SerializedObject(runtimeDatabase);

        SerializedProperty sourceUnits =
            source.FindProperty("units");

        SerializedProperty targetUnits =
            target.FindProperty("units");

        targetUnits.arraySize =
            sourceUnits.arraySize;

        for (int i = 0;
             i < sourceUnits.arraySize;
             i++)
        {
            targetUnits
                .GetArrayElementAtIndex(i)
                .objectReferenceValue =
                sourceUnits
                    .GetArrayElementAtIndex(i)
                    .objectReferenceValue;
        }

        target.ApplyModifiedPropertiesWithoutUndo();

        runtimeDatabase.RebuildLookup();

        EditorUtility.SetDirty(database);
        EditorUtility.SetDirty(runtimeDatabase);

        AssetDatabase.SaveAssets();

        Debug.Log(
            "[UNIT DATABASE MANAGER] RUNTIME DATABASE SYNCHRONIZED // " +
            sourceUnits.arraySize +
            " UNITS");
    }

    private static string MakeSafeFileName(
        string value)
    {
        foreach (char invalid in
                 Path.GetInvalidFileNameChars())
        {
            value =
                value.Replace(
                    invalid.ToString(),
                    string.Empty);
        }

        return value.Trim();
    }
}
