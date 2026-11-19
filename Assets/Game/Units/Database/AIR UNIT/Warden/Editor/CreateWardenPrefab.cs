using UnityEditor;
using UnityEngine;

public static class CreateWardenPrefab
{
    [MenuItem("OPAW/Units/Create Warden Prefab")]
    private static void Create()
    {
        const string modelPath = "Assets/Game/Units/Database/AIR UNIT/Warden/warden.fbx";
        const string assetPath = "Assets/Game/Units/Database/AIR UNIT/Warden/Warden.asset";
        const string prefabPath = "Assets/Game/Units/Database/AIR UNIT/Warden/Warden.prefab";

        var model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
        var definition = AssetDatabase.LoadMainAssetAtPath(assetPath);

        if (model == null || definition == null)
        {
            Debug.LogError("Warden model or database definition could not be loaded.");
            return;
        }

        var instance = PrefabUtility.InstantiatePrefab(model) as GameObject;
        if (instance == null)
        {
            Debug.LogError("Could not instantiate Warden FBX.");
            return;
        }

        instance.name = "Warden";
        var prefab = PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
        Object.DestroyImmediate(instance);

        if (prefab == null)
        {
            Debug.LogError("Warden prefab creation failed.");
            return;
        }

        var serialized = new SerializedObject(definition);
        var modelPrefab = serialized.FindProperty("modelPrefab");

        if (modelPrefab == null)
        {
            Debug.LogError("modelPrefab property was not found.");
            return;
        }

        modelPrefab.objectReferenceValue = prefab;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(definition);
        AssetDatabase.SaveAssets();

        Debug.Log("Warden prefab created and assigned successfully: " + prefabPath);
    }
}
