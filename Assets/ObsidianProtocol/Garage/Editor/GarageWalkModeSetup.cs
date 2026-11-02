using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class GarageWalkModeSetup
{
    private const string MainSceneName = "SCN04_GARAGE.unity";

    [MenuItem("Obsidian Protocol/Garage/Setup Walk Mode (SCN04 GARAGE)")]
    private static void Setup()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (!scene.path.EndsWith(MainSceneName))
        {
            EditorUtility.DisplayDialog(
                "Garage Walk Mode",
                "Open SCN04_GARAGE.unity first. No changes were made.",
                "OK");
            return;
        }

        if (GameObject.Find("GARAGE_WALK_MODE_MANAGER") != null ||
            GameObject.Find("GARAGE_WALK_PLAYER") != null)
        {
            EditorUtility.DisplayDialog(
                "Garage Walk Mode",
                "Walk-mode objects already exist. No duplicates were created.",
                "OK");
            return;
        }

        Camera garageCamera = GameObject.Find("GARAGE_CAMERA") != null
            ? GameObject.Find("GARAGE_CAMERA").GetComponent<Camera>()
            : null;

        GameObject hudObject = GameObject.Find("GARAGE_HUD");
        Canvas hudCanvas = hudObject != null ? hudObject.GetComponent<Canvas>() : null;

        if (garageCamera == null || hudCanvas == null)
        {
            EditorUtility.DisplayDialog(
                "Garage Walk Mode",
                "Could not find GARAGE_CAMERA or the Canvas on GARAGE_HUD. No changes were made.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Set Up Garage Walk Mode",
            "Create a first-person walking player and F8 mode toggle in SCN04_GARAGE?",
            "Create Setup", "Cancel"))
            return;

        GameObject manager = new GameObject("GARAGE_WALK_MODE_MANAGER");
        Undo.RegisterCreatedObjectUndo(manager, "Create Garage Walk Mode Manager");

        GameObject player = new GameObject("GARAGE_WALK_PLAYER");
        Undo.RegisterCreatedObjectUndo(player, "Create Garage Walk Player");
        player.transform.position = new Vector3(0f, 0.1f, -18f);

        CharacterController character = Undo.AddComponent<CharacterController>(player);
        character.height = 1.8f;
        character.radius = 0.3f;
        character.center = new Vector3(0f, 0.9f, 0f);
        character.stepOffset = 0.3f;
        character.slopeLimit = 45f;

        GameObject cameraObject = new GameObject("GARAGE_WALK_CAMERA");
        Undo.RegisterCreatedObjectUndo(cameraObject, "Create Garage Walk Camera");
        cameraObject.transform.SetParent(player.transform, false);
        cameraObject.transform.localPosition = new Vector3(0f, 1.6f, 0f);
        cameraObject.transform.localRotation = Quaternion.identity;
        cameraObject.tag = "Untagged";

        Camera walkCamera = cameraObject.AddComponent<Camera>();
        walkCamera.enabled = false;
        walkCamera.orthographic = false;
        walkCamera.fieldOfView = 75f;
        walkCamera.nearClipPlane = 0.05f;
        walkCamera.farClipPlane = 500f;
        walkCamera.depth = garageCamera.depth + 1f;

        SCN17FirstPersonWalkController controller =
            Undo.AddComponent<SCN17FirstPersonWalkController>(player);
        controller.playerCamera = walkCamera;

        player.SetActive(false);

        GarageWalkModeToggle toggle = Undo.AddComponent<GarageWalkModeToggle>(manager);
        toggle.garageCamera = garageCamera;
        toggle.walkCamera = walkCamera;
        toggle.walkPlayer = player;
        toggle.garageHudCanvas = hudCanvas;
        toggle.toggleKey = KeyCode.F8;

        EditorSceneManager.MarkSceneDirty(scene);

        Selection.activeGameObject = manager;

        EditorUtility.DisplayDialog(
            "Garage Walk Mode Created",
            "Setup created in SCN04_GARAGE.\n\nSave the scene with Ctrl+S.\nPress Play, then F8 to enter or exit walking mode.\nWASD moves, mouse looks, Space jumps, Escape releases or recaptures the mouse.",
            "OK");
    }
}

