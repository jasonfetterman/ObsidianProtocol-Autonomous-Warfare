using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectBattlefieldCanvases
{
    [MenuItem("Obsidian Protocol/Debug/Inspect Battlefield Canvases")]
    public static void Inspect()
    {
        Scene scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN03  BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity",
            OpenSceneMode.Single);

        Canvas[] canvases = Object.FindObjectsByType<Canvas>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        Debug.Log("===== BATTLEFIELD CANVASES =====");

        foreach (Canvas canvas in canvases)
        {
            CanvasGroup group =
                canvas.GetComponent<CanvasGroup>();

            Debug.Log(
                "[CANVAS] " +
                canvas.gameObject.name +
                " | RenderMode=" +
                canvas.renderMode +
                " | SortOrder=" +
                canvas.sortingOrder +
                " | CanvasGroup=" +
                (group != null));

            if (group != null)
            {
                Debug.Log(
                    "[CANVAS GROUP] " +
                    canvas.gameObject.name +
                    " | BlocksRaycasts=" +
                    group.blocksRaycasts +
                    " | Interactable=" +
                    group.interactable);
            }
        }

        UnityEngine.UI.Graphic[] graphics =
    Object.FindObjectsByType<UnityEngine.UI.Graphic>(
        FindObjectsInactive.Include,
        FindObjectsSortMode.None);

Debug.Log("===== RAYCAST TARGETS =====");

foreach (UnityEngine.UI.Graphic graphic in graphics)
{
    if (!graphic.raycastTarget)
        continue;

    RectTransform rect =
        graphic.GetComponent<RectTransform>();

    Debug.Log(
        "[RAYCAST] " +
        graphic.gameObject.name +
        " | Type=" +
        graphic.GetType().Name +
        " | Size=" +
        (rect != null ? rect.rect.size.ToString() : "none") +
        " | Position=" +
        (rect != null ? rect.position.ToString() : "none"));
}

Debug.Log("===== END RAYCAST TARGETS =====");

Debug.Log("===== END CANVAS INSPECTION =====");
    }
}

