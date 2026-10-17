using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BattlefieldVisibleUIAudit
{
    [MenuItem("Obsidian Protocol/Battlefield/AUDIT VISIBLE HUD OBJECTS")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject hud = GameObject.Find("[HUD] BATTLEFIELD HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[BATTLEFIELD VISIBLE AUDIT] HUD NOT FOUND.");
            return;
        }

        RectTransform hudRect =
            hud.GetComponent<RectTransform>();

        RectTransform[] rects =
            hud.GetComponentsInChildren<RectTransform>(true);

        foreach (RectTransform rt in rects)
        {
            if (rt == hudRect)
                continue;

            if (!rt.gameObject.activeInHierarchy)
                continue;

            Graphic graphic =
                rt.GetComponent<Graphic>();

            CanvasRenderer renderer =
                rt.GetComponent<CanvasRenderer>();

            if (graphic == null && renderer == null)
                continue;

            Vector2 size = rt.rect.size;

            Debug.Log(
                "[BATTLEFIELD VISIBLE AUDIT] " +
                rt.name +
                " | PARENT=" +
                (rt.parent != null
                    ? rt.parent.name
                    : "<ROOT>") +
                " | POS=" +
                rt.anchoredPosition +
                " | SIZE=" +
                size +
                " | GRAPHIC=" +
                (graphic != null
                    ? graphic.GetType().Name
                    : "NONE"));
        }

        Debug.Log(
            "[BATTLEFIELD VISIBLE AUDIT] COMPLETE.");
    }
}
