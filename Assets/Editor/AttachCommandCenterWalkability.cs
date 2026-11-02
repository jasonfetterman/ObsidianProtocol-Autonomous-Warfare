using UnityEditor;
using UnityEngine;

public static class AttachCommandCenterWalkability
{
    [MenuItem("Obsidian Protocol/VR/Attach Walkability To XR Origin")]
    public static void Attach()
    {
        const string scenePath = "Assets/Scenes/SCN20_VR_COMMAND/SCN20_VR_COMMAND.unity";
        const string objectName = "XR Origin (VR)";
        const string scriptPath = "Assets/Game/CommandCenterWalkability.cs";

        var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        if (scene.path != scenePath)
        {
            Debug.LogError("Open SCN20_VR_COMMAND.unity first. No changes made.");
            return;
        }

        var origin = GameObject.Find(objectName);
        if (origin == null)
        {
            Debug.LogError("XR Origin (VR) was not found. No changes made.");
            return;
        }

        var script = AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
        if (script == null)
        {
            Debug.LogError("Walkability script was not found. No changes made.");
            return;
        }

        var type = script.GetClass();
        if (type == null)
        {
            Debug.LogError("Walkability script has not compiled. No changes made.");
            return;
        }

        if (origin.GetComponent(type) != null)
        {
            Debug.Log("Walkability is already attached to XR Origin (VR).");
            return;
        }

        Undo.AddComponent(origin, type);
        EditorUtility.SetDirty(origin);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("Walkability attached to XR Origin (VR). Save the scene.");
    }
}
