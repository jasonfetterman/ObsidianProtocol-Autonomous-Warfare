using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BattlefieldCenterPanelTransparency
{
    [MenuItem("Obsidian Protocol/Battlefield/FIX CENTER PANEL TRANSPARENCY")]
    public static void Fix()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject panel =
            GameObject.Find("[PANEL] CENTER TACTICAL PANEL");

        if (panel == null)
        {
            Debug.LogError("[CENTER PANEL] NOT FOUND.");
            return;
        }

        Image image = panel.GetComponent<Image>();

        if (image == null)
        {
            Debug.LogError("[CENTER PANEL] IMAGE COMPONENT NOT FOUND.");
            return;
        }

        Color c = image.color;
        c.a = 0f;
        image.color = c;

        EditorUtility.SetDirty(image);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[CENTER PANEL] BACKGROUND MADE TRANSPARENT. " +
            "TACTICAL CHILD CONTROLS PRESERVED.");
    }
}
