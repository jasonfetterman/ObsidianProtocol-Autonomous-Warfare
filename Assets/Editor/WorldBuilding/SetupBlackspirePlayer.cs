using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SetupBlackspirePlayer
{
    [MenuItem("Obsidian Protocol/World Building/Setup Blackspire Player")]
    private static void Run()
    {
        const string path = "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity";
        var scene = SceneManager.GetSceneByPath(path);
        bool opened = !scene.IsValid() || !scene.isLoaded;
        if (opened) scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        try
        {
            GameObject terrain = null;
            foreach (var go in scene.GetRootGameObjects())
                if (go.name == "BLACKSPIRE MESHY TERRAIN") terrain = go;

            if (!terrain)
            {
                Debug.LogError("[BLACKSPIRE PLAYER] Terrain root not found.");
                return;
            }

            var colliders = terrain.GetComponentsInChildren<MeshCollider>(true);
            RaycastHit hit = default;
            bool foundGround = false;

            Physics.SyncTransforms();
            foreach (var col in colliders)
            {
                if (!col.enabled || !col.sharedMesh) continue;
                var bounds = col.bounds;
                var origin = new Vector3(bounds.center.x,
                    bounds.max.y + 100f, bounds.center.z);

                if (Physics.Raycast(origin, Vector3.down, out hit,
                    bounds.size.y + 250f) && hit.collider == col)
                {
                    foundGround = true;
                    break;
                }
            }

            if (!foundGround)
            {
                Debug.LogError("[BLACKSPIRE PLAYER] Could not find terrain surface. No player created.");
                return;
            }

            var player = GameObject.Find("BLACKSPIRE WALK PLAYER");
            if (!player)
            {
                player = new GameObject("BLACKSPIRE WALK PLAYER");
                SceneManager.MoveGameObjectToScene(player, scene);
            }

            Undo.RegisterFullObjectHierarchyUndo(player, "Setup Blackspire Player");
            player.transform.SetPositionAndRotation(
                hit.point + Vector3.up * 1.05f, Quaternion.identity);

            var controller = player.GetComponent<CharacterController>();
            if (!controller) controller = Undo.AddComponent<CharacterController>(player);
            controller.height = 1.8f;
            controller.radius = 0.35f;
            controller.center = new Vector3(0f, 0.9f, 0f);
            controller.enabled = true;

            var cameraObject = player.transform.Find("Player Camera");
            if (!cameraObject)
            {
                var cameraGo = new GameObject("Player Camera");
                cameraGo.transform.SetParent(player.transform, false);
                cameraGo.transform.localPosition = new Vector3(0f, 1.8f, 0f);
                cameraObject = cameraGo.transform;
            }

            cameraObject.localPosition = new Vector3(0f, 1.8f, 0f);
            var cam = cameraObject.GetComponent<Camera>();
            if (!cam) cam = Undo.AddComponent<Camera>(cameraObject.gameObject);
            cam.enabled = true;

            var listener = cameraObject.GetComponent<AudioListener>();
            if (!listener) Undo.AddComponent<AudioListener>(cameraObject.gameObject);

            var movement = player.GetComponent<SCN17FirstPersonWalkController>();
            if (!movement) movement = Undo.AddComponent<SCN17FirstPersonWalkController>(player);
            movement.playerCamera = cam;

            Selection.activeGameObject = player;
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Debug.Log("[BLACKSPIRE PLAYER] Ready at ground position " +
                player.transform.position + ". WASD move, mouse look, Space jump, Esc unlocks cursor.");
        }
        finally
        {
            if (opened) EditorSceneManager.CloseScene(scene, true);
        }
    }
}


