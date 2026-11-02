using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class BuildCommandCenterPlayer
{
    [MenuItem("Obsidian Protocol/VR/Create Desktop Player With Camera")]
    public static void Build()
    {
        const string scenePath = "Assets/Scenes/SCN20_VR_COMMAND/SCN20_VR_COMMAND.unity";

        var scene = EditorSceneManager.GetActiveScene();
        if (scene.path != scenePath)
        {
            Debug.LogError("Open SCN20_VR_COMMAND.unity first.");
            return;
        }

        var existing = GameObject.Find("DESKTOP PLAYER");
        if (existing != null)
        {
            Debug.LogWarning("DESKTOP PLAYER already exists. No duplicate created.");
            return;
        }

        var xr = GameObject.Find("XR Origin (VR)");
        if (xr == null)
        {
            Debug.LogError("XR Origin (VR) not found. No player created.");
            return;
        }

        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Undo.RegisterCreatedObjectUndo(player, "Create Desktop Player");
        player.name = "DESKTOP PLAYER";
        player.transform.position = new Vector3(0f, 0.9f, 0f);

        var oldCollider = player.GetComponent<CapsuleCollider>();
        Undo.DestroyObjectImmediate(oldCollider);

        var controller = Undo.AddComponent<CharacterController>(player);
        controller.height = 1.8f;
        controller.radius = 0.3f;
        controller.center = new Vector3(0f, 0.9f, 0f);
        controller.minMoveDistance = 0f;

        var cameraObject = new GameObject("Player Camera");
        Undo.RegisterCreatedObjectUndo(cameraObject, "Create Player Camera");
        cameraObject.transform.SetParent(player.transform, false);
        cameraObject.transform.localPosition = new Vector3(0f, 1.6f, -3.5f);
        cameraObject.transform.localRotation = Quaternion.identity;

        var camera = cameraObject.AddComponent<Camera>();
        camera.tag = "MainCamera";
        camera.nearClipPlane = 0.03f;
        camera.fieldOfView = 65f;
        cameraObject.AddComponent<AudioListener>();

        var movement = Undo.AddComponent<SCN17FirstPersonWalkController>(player);
        movement.playerCamera = camera;
        movement.moveSpeed = 5f;
        movement.jumpHeight = 1.2f;
        movement.gravity = -20f;
        movement.mouseSensitivity = 2f;
        movement.maxLookAngle = 85f;

        var mode = Undo.AddComponent<CommandCenterPlayerMode>(player);
        mode.desktopPlayer = player;
        mode.xrOrigin = xr;

        EditorSceneManager.MarkSceneDirty(scene);
        Selection.activeGameObject = player;

        Debug.Log("Desktop player, visible capsule, CharacterController, camera, jump and mouse-look created. Save the scene with Ctrl+S.");
    }
}

