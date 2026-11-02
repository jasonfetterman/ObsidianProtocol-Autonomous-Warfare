using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SCN16StorePlayerSetup
{
    [MenuItem("Obsidian Protocol/Store/Fix Player Movement and Camera")]
    private static void Setup()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("[STORE PLAYER] Stop Play mode before setup.");
            return;
        }

        GameObject cameraObject = GameObject.Find("Main Camera");
        if (cameraObject == null)
        {
            Debug.LogError("[STORE PLAYER] Main Camera not found.");
            return;
        }

        if (GameObject.Find("STORE_PLAYER") != null)
        {
            Debug.LogWarning("[STORE PLAYER] STORE_PLAYER already exists. No changes made.");
            return;
        }

        GameObject player = new GameObject("STORE_PLAYER");
        Undo.RegisterCreatedObjectUndo(player, "Create Store Player");

        player.transform.position = new Vector3(-5f, 0.1f, -8f);
        player.transform.rotation = Quaternion.Euler(0f, -45f, 0f);

        CharacterController character = player.AddComponent<CharacterController>();
        character.height = 2.1f;
        character.radius = 0.3f;
        character.center = new Vector3(0f, 1.05f, 0f);
        character.stepOffset = 0.3f;
        character.skinWidth = 0.08f;

        cameraObject.transform.SetParent(player.transform, false);
        cameraObject.transform.localPosition = new Vector3(0f, 1.9f, 0f);
        cameraObject.transform.localRotation = Quaternion.identity;

        SCN17FirstPersonWalkController movement =
            player.AddComponent<SCN17FirstPersonWalkController>();
        movement.playerCamera = cameraObject.GetComponent<Camera>();

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Selection.activeGameObject = player;
        Debug.Log("[STORE PLAYER] Player created, existing camera attached, and active scene saved.");
    }
}

