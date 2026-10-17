using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public static class StoreLocalBloomSetup
{
    const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";
    const string ProfilePath = "Assets/Scenes/SCN16_STORE/StoreBloomProfile.asset";

    [MenuItem("Obsidian Protocol/Store/Create Store-Only Bloom")]
    static void Apply()
    {
        var scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Stop Play mode and open SCN16_STORE. No changes made.");
            return;
        }

        var existing = AssetDatabase.LoadAssetAtPath<VolumeProfile>(ProfilePath);
        VolumeProfile profile = existing;

        if (profile == null)
        {
            profile = ScriptableObject.CreateInstance<VolumeProfile>();
            AssetDatabase.CreateAsset(profile, ProfilePath);
        }

        if (!profile.TryGet<Bloom>(out var bloom))
            bloom = profile.Add<Bloom>(true);

        bloom.active = true;
        bloom.threshold.Override(0.8f);
        bloom.intensity.Override(1.5f);
        bloom.scatter.Override(0.7f);

        var volumeObject = GameObject.Find("STORE_LOCAL_BLOOM_VOLUME");
        if (volumeObject == null)
        {
            volumeObject = new GameObject("STORE_LOCAL_BLOOM_VOLUME");
            Undo.RegisterCreatedObjectUndo(volumeObject, "Create Store Bloom Volume");
            SceneManager.MoveGameObjectToScene(volumeObject, scene);
        }

        var volume = volumeObject.GetComponent<Volume>();
        if (volume == null)
            volume = Undo.AddComponent<Volume>(volumeObject);

        Undo.RecordObject(volume, "Configure Store Bloom");
        volume.isGlobal = true;
        volume.priority = 50;
        volume.sharedProfile = profile;

        EditorUtility.SetDirty(profile);
        EditorUtility.SetDirty(volume);
        EditorSceneManager.MarkSceneDirty(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(scene);

        Debug.Log("Store-only Bloom created: threshold 0.8, intensity 1.5. Shared profile unchanged.");
    }
}
