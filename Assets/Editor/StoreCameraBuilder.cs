using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class StoreCameraBuilder
{
    [MenuItem("Obsidian Protocol/Store/ADD STORE CAMERA")]
    public static void Build()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("[STORE CAMERA] Open SCN16_STORE.unity first.");
            return;
        }

        Camera existing = Camera.main;

        if (existing != null)
        {
            Selection.activeGameObject = existing.gameObject;
            Debug.Log("[STORE CAMERA] Main Camera already exists.");
            return;
        }

        GameObject cameraObject = new GameObject("Main Camera");
        Undo.RegisterCreatedObjectUndo(
            cameraObject,
            "Create Store Main Camera");

        cameraObject.tag = "MainCamera";

        Camera camera =
            cameraObject.AddComponent<Camera>();

        camera.fieldOfView = 70f;
        camera.nearClipPlane = 0.05f;
        camera.farClipPlane = 500f;

        cameraObject.transform.position =
            new Vector3(0f, 2.2f, -8f);

        cameraObject.transform.rotation =
            Quaternion.Euler(0f, 0f, 0f);

        GameObject lightObject =
            new GameObject("STORE_PLAYER_LIGHT");

        Undo.RegisterCreatedObjectUndo(
            lightObject,
            "Create Store Player Light");

        lightObject.transform.SetParent(
            cameraObject.transform);

        lightObject.transform.localPosition =
            new Vector3(0f, 0.2f, 0f);

        Light light =
            lightObject.AddComponent<Light>();

        light.type = LightType.Point;
        light.range = 12f;
        light.intensity = 2f;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = cameraObject;

        Debug.Log(
            "[STORE CAMERA] Main Camera created at entrance.");
    }
}
