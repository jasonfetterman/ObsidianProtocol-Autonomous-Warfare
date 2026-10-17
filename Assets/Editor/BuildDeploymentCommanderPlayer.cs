using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BuildDeploymentCommanderPlayer
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    [MenuItem("Obsidian Protocol/Deployment/BUILD COMMANDER PLAYER")]
    public static void Build()
    {
        EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single);

        Camera deploymentCamera =
            Object.FindFirstObjectByType<Camera>();

        if (deploymentCamera == null)
        {
            Debug.LogError(
                "[DEPLOYMENT PLAYER] Deployment Camera not found.");
            return;
        }

        GameObject existingPlayer =
            GameObject.Find("[PLAYER] COMMANDER");

        if (existingPlayer != null)
        {
            Debug.LogWarning(
                "[DEPLOYMENT PLAYER] Player already exists. Build cancelled.");
            return;
        }

        Transform cameraTransform =
            deploymentCamera.transform;

        Vector3 cameraPosition =
            cameraTransform.position;

        Quaternion cameraRotation =
            cameraTransform.rotation;

        GameObject player =
            new GameObject("[PLAYER] COMMANDER");

        player.transform.position =
            cameraPosition;

        player.transform.rotation =
            Quaternion.Euler(
                0f,
                cameraRotation.eulerAngles.y,
                0f);

        CharacterController controller =
            player.AddComponent<CharacterController>();

        // Commander scale for the oversized deployment facility.
        controller.height = 4.0f;
        controller.radius = 0.6f;
        controller.center =
            new Vector3(0f, 2.0f, 0f);
        controller.stepOffset = 0.6f;
        controller.slopeLimit = 45f;
        controller.skinWidth = 0.08f;

        CommanderPlayerMovement movement =
            player.AddComponent<CommanderPlayerMovement>();

        Undo.SetTransformParent(
            cameraTransform,
            player.transform,
            "Parent Deployment Camera");

        cameraTransform.localPosition =
            new Vector3(0f, 3.2f, 0f);

        cameraTransform.localRotation =
            Quaternion.Euler(
                16.35f,
                0f,
                0f);

        SerializedObject movementSerialized =
            new SerializedObject(movement);

        movementSerialized.FindProperty(
            "cameraTransform").objectReferenceValue =
            cameraTransform;

        movementSerialized.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[DEPLOYMENT PLAYER] COMMANDER PLAYER CREATED.");

        Debug.Log(
            "[DEPLOYMENT PLAYER] CAPSULE HEIGHT = " +
            controller.height);

        Debug.Log(
            "[DEPLOYMENT PLAYER] CAPSULE RADIUS = " +
            controller.radius);

        Debug.Log(
            "[DEPLOYMENT PLAYER] CAPSULE CENTER = " +
            controller.center);

        Debug.Log(
            "[DEPLOYMENT PLAYER] CAMERA LOCAL POSITION = " +
            cameraTransform.localPosition);

        Debug.Log(
            "[DEPLOYMENT PLAYER] CAMERA LOCAL ROTATION = " +
            cameraTransform.localEulerAngles);

        Debug.Log(
            "[DEPLOYMENT PLAYER] COMPLETE.");
    }
}
