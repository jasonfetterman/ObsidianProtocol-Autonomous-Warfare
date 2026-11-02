using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SCN17FirstPersonWalkSetup
{
    private const string TargetScene = "SCN17_PERSISTENT_MILITARY";
    private const string CameraName = "PERSISTENT MILITARY CAMERA";
    private const string PlayerName = "SCN17 FIRST PERSON PLAYER";

    [MenuItem("Obsidian Protocol/Persistent Military/Setup First-Person Walking")]
    public static void Setup()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name != TargetScene)
        {
            EditorUtility.DisplayDialog(
                "Wrong Scene",
                "Open SCN17_PERSISTENT_MILITARY before running this setup.",
                "OK");
            return;
        }

        GameObject existingPlayer = GameObject.Find(PlayerName);
        if (existingPlayer != null)
        {
            EditorUtility.DisplayDialog(
                "Player Already Exists",
                "SCN17 FIRST PERSON PLAYER already exists. No changes were made.",
                "OK");
            return;
        }

        GameObject cameraObject = GameObject.Find(CameraName);
        if (cameraObject == null)
        {
            EditorUtility.DisplayDialog(
                "Camera Not Found",
                "Could not find PERSISTENT MILITARY CAMERA. No changes were made.",
                "OK");
            return;
        }

        Camera playerCamera = cameraObject.GetComponent<Camera>();
        if (playerCamera == null)
        {
            EditorUtility.DisplayDialog(
                "Camera Component Missing",
                "The existing camera object has no Camera component. No changes were made.",
                "OK");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        Vector3 originalCameraPosition = cameraObject.transform.position;
        Quaternion originalCameraRotation = cameraObject.transform.rotation;

        GameObject player = new GameObject(PlayerName);
        Undo.RegisterCreatedObjectUndo(player, "Create Persistent Military Player");

        player.transform.position = originalCameraPosition - Vector3.up * 1.6f;
        player.transform.rotation = Quaternion.Euler(0f, originalCameraRotation.eulerAngles.y, 0f);

        CharacterController characterController = Undo.AddComponent<CharacterController>(player);
        characterController.height = 1.8f;
        characterController.radius = 0.3f;
        characterController.center = new Vector3(0f, 0.9f, 0f);
        characterController.stepOffset = 0.3f;
        characterController.slopeLimit = 45f;

        Undo.SetTransformParent(cameraObject.transform, player.transform, "Attach Existing Camera");
        cameraObject.transform.localPosition = new Vector3(0f, 1.6f, 0f);

        float pitch = originalCameraRotation.eulerAngles.x;
        if (pitch > 180f)
            pitch -= 360f;

        cameraObject.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        SCN17FirstPersonWalkController controller =
            Undo.AddComponent<SCN17FirstPersonWalkController>(player);
        controller.playerCamera = playerCamera;

        EditorSceneManager.MarkSceneDirty(scene);
        bool saved = EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = player;

        EditorUtility.DisplayDialog(
            saved ? "Setup Complete" : "Setup Created",
            saved
                ? "First-person walking was added to SCN17 Persistent Military and the scene was saved.\n\nControls: WASD move, mouse look, Space jump, Escape unlock/lock cursor."
                : "The player was created, but the scene could not be confirmed saved. Save the scene manually before testing.",
            "OK");
    }
}
