using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[InitializeOnLoad]
public static class MainMenuCameraRepair
{
    static MainMenuCameraRepair()
    {
        EditorApplication.delayCall += Repair;
    }

    private static void Repair()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (string.IsNullOrEmpty(scene.path) ||
            !scene.path.Contains("SCN01_MAIN_MENU"))
            return;

        Camera existing = Resources.FindObjectsOfTypeAll<Camera>()
            .FirstOrDefault(c => c.gameObject.scene == scene);

        if (existing != null)
            return;

        GameObject cameraObject = new GameObject("Main Camera");
        SceneManager.MoveGameObjectToScene(cameraObject, scene);

        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0, 0, -10);
        cameraObject.transform.rotation = Quaternion.identity;

        Camera camera = cameraObject.AddComponent<Camera>();
        camera.enabled = true;
        camera.targetDisplay = 0;
        camera.cullingMask = -1;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.015f, 0.025f, 0.04f);
        camera.nearClipPlane = 0.1f;
        camera.farClipPlane = 1000f;
        camera.fieldOfView = 60f;

        if (Resources.FindObjectsOfTypeAll<AudioListener>()
            .All(a => a.gameObject.scene != scene))
            cameraObject.AddComponent<AudioListener>();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[OPAW] Main Menu camera created and scene saved.");
    }
}
