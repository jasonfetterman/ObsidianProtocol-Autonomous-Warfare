using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public static class SCN17CyanOrangeGlowSetup
{
    const string ScenePath =
        "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    const string ProfilePath =
        "Assets/Settings/SCN17_CyanOrangeGlowProfile.asset";

    [MenuItem("Obsidian Protocol/SCN17/Setup Cyan and Orange Bloom")]
    static void Apply()
    {
        Scene scene = EditorSceneManager.GetActiveScene();
        if (scene.path != ScenePath)
        {
            Debug.LogError("[SCN17 GLOW] Open SCN17_PERSISTENT_MILITARY first.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        VolumeProfile profile =
            AssetDatabase.LoadAssetAtPath<VolumeProfile>(ProfilePath);

        if (profile == null)
        {
            profile = ScriptableObject.CreateInstance<VolumeProfile>();
            AssetDatabase.CreateAsset(profile, ProfilePath);
        }

        Bloom bloom = profile.TryGet<Bloom>(out Bloom existing)
            ? existing
            : profile.Add<Bloom>(true);

        bloom.active = true;
        bloom.threshold.Override(0.8f);
        bloom.intensity.Override(1.5f);
        bloom.scatter.Override(0.7f);
        bloom.tint.Override(Color.white);
        EditorUtility.SetDirty(profile);

        GameObject volumeObject = GameObject.Find("SCN17 CYAN ORANGE BLOOM");
        if (volumeObject == null)
            volumeObject = new GameObject("SCN17 CYAN ORANGE BLOOM");

        Undo.RegisterCreatedObjectUndo(volumeObject, "Setup SCN17 Bloom");
        Volume volume = volumeObject.GetComponent<Volume>();
        if (volume == null) volume = Undo.AddComponent<Volume>(volumeObject);
        volume.isGlobal = true;
        volume.priority = 100f;
        volume.sharedProfile = profile;

        foreach (Camera camera in Object.FindObjectsByType<Camera>(
                     FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (camera.gameObject.scene != scene) continue;

            var data = camera.GetUniversalAdditionalCameraData();
            Undo.RecordObject(data, "Enable SCN17 Post Processing");
            data.renderPostProcessing = true;
            EditorUtility.SetDirty(data);
        }

        GameObject lighting = GameObject.Find("LIGHTING");
        if (lighting == null)
        {
            Debug.LogError("[SCN17 GLOW] LIGHTING object not found; runtime glow was not attached.");
            return;
        }

        if (lighting.GetComponent<SCN17CyanOrangeGlowRuntime>() == null)
            Undo.AddComponent<SCN17CyanOrangeGlowRuntime>(lighting);

        EditorUtility.SetDirty(volumeObject);
        EditorSceneManager.MarkSceneDirty(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[SCN17 GLOW] Cyan/orange emission and Bloom configured. Check Play mode.");
    }
}
