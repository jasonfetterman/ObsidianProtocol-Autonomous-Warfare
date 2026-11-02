using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SCN60PanelOrderFix
{
    [MenuItem("Obsidian Protocol/SCN60/FIX NAVIGATION AND OBJECT INFO ORDER")]
    public static void Fix()
    {
        const string scenePath =
            "Assets/Scenes/SCN60_GALAXY_MAP/Galaxy_Map.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        GameObject hud = GameObject.Find("HUD-201 GalaxyMapHUD");

        if (hud == null)
        {
            Debug.LogError("[SCN60] HUD-201 GalaxyMapHUD not found.");
            return;
        }

        Transform navigation =
            hud.transform.Find("HUD-201-02 NavigationPanel");

        Transform objectInfo =
            hud.transform.Find("HUD-201-04 GalaxyObjectInfoPanel");

        if (navigation == null || objectInfo == null)
        {
            Debug.LogError(
                "[SCN60] NavigationPanel or GalaxyObjectInfoPanel not found."
            );
            return;
        }

        // Navigation behind Object Info.
        navigation.SetSiblingIndex(0);
        objectInfo.SetAsLastSibling();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[SCN60] Panel order fixed: NavigationPanel behind GalaxyObjectInfoPanel."
        );
    }
}
