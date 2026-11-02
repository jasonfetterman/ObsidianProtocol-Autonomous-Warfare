using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BattlefieldHUDLayoutAudit
{
    [MenuItem("Obsidian Protocol/Battlefield/AUDIT HUD LAYOUT")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject hud =
            GameObject.Find("[HUD] BATTLEFIELD HUD");

        if (hud == null)
        {
            Debug.LogError("[BATTLEFIELD LAYOUT] HUD NOT FOUND.");
            return;
        }

        Canvas canvas =
            hud.GetComponentInParent<Canvas>();

        RectTransform canvasRect =
            canvas != null
                ? canvas.GetComponent<RectTransform>()
                : null;

        CanvasScaler scaler =
            canvas != null
                ? canvas.GetComponent<CanvasScaler>()
                : null;

        Debug.Log(
            "[BATTLEFIELD LAYOUT] CANVAS SIZE=" +
            (canvasRect != null
                ? canvasRect.rect.size.ToString()
                : "UNKNOWN"));

        Debug.Log(
            "[BATTLEFIELD LAYOUT] SCALE MODE=" +
            (scaler != null
                ? scaler.uiScaleMode.ToString()
                : "NONE"));

        string[] panels =
        {
            "[PANEL] TOP BAR",
            "[PANEL] LEFT COMMAND PANEL",
            "[PANEL] CENTER TACTICAL PANEL",
            "[PANEL] RIGHT INFORMATION PANEL",
            "[PANEL] BOTTOM COMMAND BAR",
            "[DISPLAY] BATTLEFIELD STATUS"
        };

        foreach (string name in panels)
        {
            Transform[] all =
                hud.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in all)
            {
                if (t.name != name)
                    continue;

                RectTransform rt =
                    t.GetComponent<RectTransform>();

                if (rt == null)
                    continue;

                Debug.Log(
                    "[BATTLEFIELD LAYOUT] " +
                    name +
                    " | ANCHORED=" +
                    rt.anchoredPosition +
                    " | SIZE=" +
                    rt.rect.size +
                    " | ANCHOR_MIN=" +
                    rt.anchorMin +
                    " | ANCHOR_MAX=" +
                    rt.anchorMax +
                    " | PIVOT=" +
                    rt.pivot);
            }
        }

        Debug.Log("[BATTLEFIELD LAYOUT] COMPLETE.");
    }
}
