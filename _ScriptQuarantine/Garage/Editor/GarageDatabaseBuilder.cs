using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace ObsidianProtocol.Garage.Editor
{
    public static class GarageDatabaseBuilder
    {
        private const string DatabasePath =
            "Assets/Scripts/Garage/Definitions/UnitDefinitionDatabase.asset";

        [MenuItem("Obsidian Protocol/Garage/Rebuild Unit Database")]
        public static void Rebuild()
        {
            UnitDefinitionDatabase database =
                AssetDatabase.LoadAssetAtPath<UnitDefinitionDatabase>(DatabasePath);

            if (database == null)
            {
                database = ScriptableObject.CreateInstance<UnitDefinitionDatabase>();

                AssetDatabase.CreateAsset(database, DatabasePath);
                AssetDatabase.SaveAssets();
            }

            string[] guids = AssetDatabase.FindAssets(
                "t:UnitDefinition",
                new[]
                {
                    "Assets/Scripts/Garage/Definitions"
                });

            List<UnitDefinition> definitions =
                new List<UnitDefinition>();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                UnitDefinition definition =
                    AssetDatabase.LoadAssetAtPath<UnitDefinition>(assetPath);

                if (definition != null)
                    definitions.Add(definition);
            }

            database.units = definitions;
            database.BuildLookup();

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                $"Garage database rebuilt successfully. " +
                $"Unit definitions found: {definitions.Count}"
            );
        }
    }
}
