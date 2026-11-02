using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AttachGalaxyMapHUD201Controller
{
    private const string ScenePath =
        "Assets/Scenes/SCN60_GALAXY_MAP/Galaxy_Map.unity";

    private const string ControllerType =
        "GalaxyMapHUD201Controller";

    [MenuItem("Obsidian Protocol/SCN60/Attach GalaxyMapHUD201 Controller")]
    public static void Attach()
    {
        var scene = EditorSceneManager.OpenScene(
            ScenePath,
            OpenSceneMode.Single
        );

        if (!scene.IsValid())
        {
            Debug.LogError("[SCN60] Could not open Galaxy_Map.unity.");
            return;
        }

        GameObject hud = GameObject.Find("HUD-201 GalaxyMapHUD");

        if (hud == null)
        {
            Debug.LogError(
                "[SCN60] HUD-201 GalaxyMapHUD was not found."
            );
            return;
        }

        System.Type controllerType = System.Type.GetType(
            ControllerType + ", Assembly-CSharp"
        );

        if (controllerType == null)
        {
            Debug.LogError(
                "[SCN60] GalaxyMapHUD201Controller type was not found."
            );
            return;
        }

        Component existing = hud.GetComponent(controllerType);

        if (existing == null)
        {
            hud.AddComponent(controllerType);
            Debug.Log(
                "[SCN60] GalaxyMapHUD201Controller attached."
            );
        }
        else
        {
            Debug.Log(
                "[SCN60] GalaxyMapHUD201Controller already attached."
            );
        }

        EditorUtility.SetDirty(hud);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[SCN60] Controller attached and scene saved. " +
            "No RectTransform values were modified."
        );
    }
}
