using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class ConfigureCommandCenterPlayer
{
    [MenuItem("Obsidian Protocol/VR/Configure Player Movement")]
    public static void Configure()
    {
        const string scenePath = "Assets/Scenes/SCN20_VR_COMMAND/SCN20_VR_COMMAND.unity";

        var scene = EditorSceneManager.GetActiveScene();
        if (scene.path != scenePath)
        {
            Debug.LogError("Open SCN20_VR_COMMAND.unity first.");
            return;
        }

        var origin = GameObject.Find("XR Origin (VR)");
        if (origin == null)
        {
            Debug.LogError("XR Origin (VR) not found.");
            return;
        }

        var camera = origin.GetComponentInChildren<Camera>(true);
        var movement = origin.GetComponent<CommandCenterWalkability>();

        if (camera == null || movement == null)
        {
            Debug.LogError("Camera or CommandCenterWalkability is missing.");
            return;
        }

        var controller = origin.GetComponent<CharacterController>();
        if (controller == null)
            controller = Undo.AddComponent<CharacterController>(origin);

        controller.height = 1.8f;
        controller.radius = 0.3f;
        controller.center = new Vector3(0f, 0.9f, 0f);
        controller.minMoveDistance = 0f;

        Undo.RecordObject(movement, "Configure Command Center Movement");
        movement.viewCamera = camera.transform;
        movement.moveSpeed = 5f;
        movement.jumpHeight = 1.2f;
        movement.gravity = -20f;
        movement.mouseSensitivity = 2f;
        EditorUtility.SetDirty(movement);
        EditorUtility.SetDirty(controller);

        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log("Player movement configured. Save the scene with Ctrl+S.");
    }
}
