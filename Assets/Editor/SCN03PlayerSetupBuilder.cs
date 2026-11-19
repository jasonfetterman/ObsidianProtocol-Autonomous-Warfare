using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.IO;

public static class SCN03PlayerSetupBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity";

    [MenuItem("Obsidian Protocol/Battlefield/BUILD SCN03 PLAYER AND CAMERA SWITCH")]
    public static void Build()
    {
        if (SceneManager.GetActiveScene().path != ScenePath)
        {
            EditorUtility.DisplayDialog("SCN03 Player Setup",
                "Open SCN03_BATTLEFIELD_AAA first, then run this menu item again.", "OK");
            return;
        }

        GameObject flightObject = GameObject.Find("[CAMERA] AAA BATTLEFIELD");
        GameObject terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (flightObject == null || terrain == null)
        {
            Debug.LogError("SCN03 setup failed: flight camera or terrain not found.");
            return;
        }

        Camera flightCamera = flightObject.GetComponent<Camera>();
        Renderer terrainRenderer = terrain.GetComponentInChildren<Renderer>();

        if (flightCamera == null || terrainRenderer == null)
        {
            Debug.LogError("SCN03 setup failed: required camera or terrain renderer missing.");
            return;
        }

        if (GameObject.Find("[PLAYER] SCN03 FIRST PERSON") != null)
        {
            Debug.LogWarning("SCN03 player already exists. No changes made.");
            return;
        }

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(terrainRenderer.bounds.center, out hit, 500f, NavMesh.AllAreas))
        {
            Debug.LogError("SCN03 setup failed: no NavMesh position found.");
            return;
        }

        string backupPath = ScenePath + ".backup_before_sc03_player_setup";
        if (!File.Exists(backupPath))
            File.Copy(ScenePath, backupPath);

        GameObject player = new GameObject("[PLAYER] SCN03 FIRST PERSON");
        Undo.RegisterCreatedObjectUndo(player, "Build SCN03 Player");
        player.transform.position = hit.position;

        CharacterController character = player.AddComponent<CharacterController>();
        character.height = 1.8f;
        character.radius = 0.35f;
        character.center = new Vector3(0f, 0.9f, 0f);
        character.enabled = false;

        GameObject cameraObject = new GameObject("[CAMERA] SCN03 FIRST PERSON");
        cameraObject.transform.SetParent(player.transform);
        cameraObject.transform.localPosition = new Vector3(0f, 1.65f, 0f);

        Camera playerCamera = cameraObject.AddComponent<Camera>();
        playerCamera.fieldOfView = 75f;
        playerCamera.nearClipPlane = 0.05f;
        playerCamera.enabled = false;

        AudioListener playerListener = cameraObject.AddComponent<AudioListener>();
        playerListener.enabled = false;

        SCN17FirstPersonWalkController movement =
            player.AddComponent<SCN17FirstPersonWalkController>();
        movement.playerCamera = playerCamera;
        movement.enabled = false;

        AudioListener flightListener = flightObject.GetComponent<AudioListener>();
        if (flightListener == null)
            flightListener = flightObject.AddComponent<AudioListener>();

        Behaviour flightController = null;
        foreach (MonoBehaviour component in flightObject.GetComponents<MonoBehaviour>())
        {
            if (component != null && component.GetType().Name == "BattlefieldCameraController")
            {
                flightController = component;
                break;
            }
        }

        SCN03PlayerCameraModeSwitcher switcher =
            player.AddComponent<SCN03PlayerCameraModeSwitcher>();
        switcher.flightCamera = flightCamera;
        switcher.flightController = flightController;
        switcher.playerCamera = playerCamera;
        switcher.flightAudioListener = flightListener;
        switcher.playerAudioListener = playerListener;
        switcher.playerMovement = movement;

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("SCN03 player created at " + hit.position + ". Flight camera remains default; press V to switch.");
        Debug.Log("Backup created: " + backupPath);
    }
}
