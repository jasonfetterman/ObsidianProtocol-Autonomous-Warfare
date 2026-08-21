using UnityEditor;
using UnityEngine;

namespace ObsidianProtocol.Garage.Editor
{
    public static class CreateGarageConfiguration
    {
        private const string AssetPath =
            "Assets/Scripts/Garage/Core/GarageConfiguration.asset";

        [MenuItem("Obsidian Protocol/Garage/Create Garage Configuration")]
        public static void Create()
        {
            GarageConfiguration existing =
                AssetDatabase.LoadAssetAtPath<GarageConfiguration>(
                    AssetPath);

            if (existing != null)
            {
                Selection.activeObject = existing;
                EditorGUIUtility.PingObject(existing);

                Debug.Log("GarageConfiguration already exists.");
                return;
            }

            GarageConfiguration configuration =
                ScriptableObject.CreateInstance<GarageConfiguration>();

            AssetDatabase.CreateAsset(
                configuration,
                AssetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = configuration;
            EditorGUIUtility.PingObject(configuration);

            Debug.Log(
                "GarageConfiguration created successfully.");
        }
    }
}
