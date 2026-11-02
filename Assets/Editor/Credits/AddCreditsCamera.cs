using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AddCreditsCamera
{
    [MenuItem("Obsidian Protocol/Fix SCN23 Credits Camera")]
    public static void Fix()
    {
        string path = "Assets/Scenes/SCN23_CREDITS/Credits.unity";

        var scene = EditorSceneManager.OpenScene(
            path,
            OpenSceneMode.Single);

        if (Object.FindFirstObjectByType<Camera>() == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.black;
            camera.transform.position = new Vector3(0f, 0f, -10f);
            camera.transform.rotation = Quaternion.identity;
        }

        EditorSceneManager.SaveScene(scene);
        Debug.Log("[CREDITS] Camera added successfully.");
    }
}
