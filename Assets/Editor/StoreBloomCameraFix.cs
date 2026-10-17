using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public static class StoreBloomCameraFix
{
    [MenuItem("Obsidian Protocol/Store/Enable Store Camera Post Processing")]
    static void Apply()
    {
        var scene = SceneManager.GetActiveScene();
        if (EditorApplication.isPlaying ||
            scene.path != "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("Stop Play mode and open SCN16_STORE.");
            return;
        }

        var cameraObject = GameObject.Find("Main Camera");
        if (!cameraObject || !cameraObject.scene.IsValid())
        {
            Debug.LogError("Main Camera not found. No changes made.");
            return;
        }

        var data = cameraObject.GetComponent<UniversalAdditionalCameraData>();
        if (!data)
        {
            Debug.LogError("URP camera data missing. No changes made.");
            return;
        }

        Undo.RecordObject(data, "Enable Store post processing");
        data.renderPostProcessing = true;
        EditorUtility.SetDirty(data);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Store Main Camera post-processing enabled.");
    }
}
