using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Intelligence;
using UnityEngine.SceneManagement;

public static class WireIdentifiedPersistent
{
    [MenuItem("Obsidian Protocol/Fix/Wire IDENTIFIED Persistent")]
    public static void Wire()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject hud = FindInScene(scene, "INTELLIGENCE HUD");
        GameObject buttonObject = FindInScene(scene, "BUTTON - IDENTIFIED");

        if (hud == null || buttonObject == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] IDENTIFIED wiring target not found. " +
                "HUD=" + (hud != null) +
                " BUTTON=" + (buttonObject != null));
            return;
        }

        IntelligenceContactFunctionController controller =
            hud.GetComponent<IntelligenceContactFunctionController>();

        if (controller == null)
            controller = hud.AddComponent<IntelligenceContactFunctionController>();

        Button button = buttonObject.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] BUTTON - IDENTIFIED has no Button component.");
            return;
        }

        button.onClick.RemoveAllListeners();

        UnityEventTools.AddPersistentListener(
            button.onClick,
            controller.ShowIdentified);

        EditorUtility.SetDirty(button);
        EditorUtility.SetDirty(controller);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[INTELLIGENCE] IDENTIFIED persistent OnClick saved.");
    }

    private static GameObject FindInScene(Scene scene, string objectName)
    {
        GameObject[] roots = scene.GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            Transform[] transforms =
                root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in transforms)
            {
                if (t.name == objectName)
                    return t.gameObject;
            }
        }

        return null;
    }
}
