using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class BattlefieldCameraInstaller
{
    static BattlefieldCameraInstaller()
    {
        EditorApplication.delayCall += Install;
    }

    static void Install()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity")
            return;

        GameObject cameraObject = GameObject.Find("[CAMERA] AAA BATTLEFIELD");
        if (cameraObject == null) return;

        if (cameraObject.GetComponent<BattlefieldCameraController>() != null)
            return;

        cameraObject.AddComponent<BattlefieldCameraController>();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[OPAW] Battlefield camera controls installed.");
    }
}
