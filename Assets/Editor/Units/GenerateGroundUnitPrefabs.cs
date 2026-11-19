using UnityEditor;
using UnityEngine;
using System.IO;

public static class GenerateGroundUnitPrefabs
{
    private const string OutputFolder = "Assets/Prefabs/Units/Ground";

    private static readonly (string id, string source, string output, string definition)[] Units =
    {
        ("Bulldog", "Assets/Models/fully_cut_models/ground_Bulldog.fbx", "Bulldog.prefab", "Assets/Game/Units/Database/GROUND UNIT/Bulldog/Bulldog.asset"),
        ("Hammer", "Assets/Models/fully_cut_models/ground_hammer.fbx", "Hammer.prefab", "Assets/Game/Units/Database/GROUND UNIT/Hammer/Hammer.asset"),
        ("Rover", "Assets/Models/fully_cut_models/ground_rover.fbx", "Rover.prefab", "Assets/Game/Units/Database/GROUND UNIT/Rover/Rover.asset")
    };

    [MenuItem("Obsidian Protocol/Units/Generate Ground Unit Prefabs")]
    public static void Generate()
    {
        Directory.CreateDirectory(OutputFolder);
        AssetDatabase.Refresh();

        int created = 0;

        foreach (var unit in Units)
        {
            var model = AssetDatabase.LoadAssetAtPath<GameObject>(unit.source);
            var definition = AssetDatabase.LoadAssetAtPath<ScriptableObject>(unit.definition);

            if (model == null || definition == null)
            {
                Debug.LogError($"[{unit.id}] Missing source model or database asset. Skipped.");
                continue;
            }

            var outputPath = $"{OutputFolder}/{unit.output}";
            var root = new GameObject(unit.id);
            var child = (GameObject)PrefabUtility.InstantiatePrefab(model);
            child.name = model.name;
            child.transform.SetParent(root.transform, false);

            var prefab = PrefabUtility.SaveAsPrefabAsset(root, outputPath);
            Object.DestroyImmediate(root);

            if (prefab == null)
            {
                Debug.LogError($"[{unit.id}] Prefab creation failed.");
                continue;
            }

            var serialized = new SerializedObject(definition);
            var property = serialized.FindProperty("modelPrefab");

            if (property == null)
            {
                Debug.LogError($"[{unit.id}] modelPrefab field not found. Prefab created but not assigned.");
                continue;
            }

            property.objectReferenceValue = prefab;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(definition);

            Debug.Log($"[{unit.id}] Created and assigned: {outputPath}");
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Ground unit prefab generation complete: {created}/{Units.Length} assigned.");
    }
}
