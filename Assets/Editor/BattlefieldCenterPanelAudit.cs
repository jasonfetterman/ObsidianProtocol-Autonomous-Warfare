using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BattlefieldCenterPanelAudit
{
    [MenuItem("Obsidian Protocol/Battlefield/AUDIT CENTER PANEL")]
    public static void Audit()
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
            Debug.LogError("[CENTER PANEL AUDIT] NOT FOUND.");
            return;
        }

        Image image =
            panel.GetComponent<Image>();

        CanvasGroup group =
            panel.GetComponent<CanvasGroup>();

        Debug.Log(
            "[CENTER PANEL AUDIT] " +
            "IMAGE=" + (image != null) +
            " | COLOR=" +
            (image != null ? image.color.ToString() : "NONE") +
            " | SPRITE=" +
            (image != null && image.sprite != null
                ? image.sprite.name
                : "NONE") +
            " | CANVAS_GROUP=" + (group != null) +
            " | ALPHA=" +
            (group != null ? group.alpha.ToString() : "N/A"));

        Debug.Log("[CENTER PANEL AUDIT] COMPLETE.");
    }
}
