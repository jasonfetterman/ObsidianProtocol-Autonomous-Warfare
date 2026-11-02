using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BattlefieldHUDLayoutComponentsAudit
{
    [MenuItem("Obsidian Protocol/Battlefield/AUDIT HUD LAYOUT COMPONENTS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject hud =
            GameObject.Find("[HUD] BATTLEFIELD HUD");

        string[] names =
        {
            "[PANEL] TOP BAR",
            "[PANEL] LEFT COMMAND PANEL",
            "[PANEL] CENTER TACTICAL PANEL",
            "[PANEL] RIGHT INFORMATION PANEL",
            "[PANEL] BOTTOM COMMAND BAR",
            "[DISPLAY] BATTLEFIELD STATUS"
        };

        foreach (string name in names)
        {
            Transform[] all =
                hud.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in all)
            {
                if (t.name != name)
                    continue;

                Debug.Log(
                    "[BATTLEFIELD COMPONENTS] " +
                    name +
                    " | PARENT=" +
                    (t.parent != null
                        ? t.parent.name
                        : "<ROOT>") +
                    " | LAYOUT_GROUP=" +
                    (t.GetComponent<LayoutGroup>() != null) +
                    " | LAYOUT_ELEMENT=" +
                    (t.GetComponent<LayoutElement>() != null) +
                    " | CONTENT_FITTER=" +
                    (t.GetComponent<ContentSizeFitter>() != null) +
                    " | ASPECT=" +
                    (t.GetComponent<AspectRatioFitter>() != null));
            }
        }

        Debug.Log("[BATTLEFIELD COMPONENTS] COMPLETE.");
    }
}
