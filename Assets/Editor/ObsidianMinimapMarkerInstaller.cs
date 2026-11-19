using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.UI.HUD.Minimap;

public static class ObsidianMinimapMarkerInstaller
{
    [MenuItem("Obsidian Protocol/UI/Install Blackspire Minimap Markers")]
    public static void Install()
    {
        GameObject display = GameObject.Find("Minimap_Display");

        if (display == null)
        {
            Debug.LogError("[Minimap] Minimap_Display was not found.");
            return;
        }

        Transform existing = display.transform.Find("Minimap_MarkerOverlay");

        GameObject overlay;

        if (existing != null)
        {
            overlay = existing.gameObject;
        }
        else
        {
            overlay = new GameObject(
                "Minimap_MarkerOverlay",
                typeof(RectTransform)
            );

            Undo.RegisterCreatedObjectUndo(
                overlay,
                "Create minimap marker overlay"
            );

            overlay.transform.SetParent(display.transform, false);
        }

        RectTransform rect = overlay.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;

        overlay.transform.SetAsLastSibling();

        if (overlay.GetComponent<MAP003_FriendlyUnits>() == null)
            Undo.AddComponent<MAP003_FriendlyUnits>(overlay);

        if (overlay.GetComponent<MAP012_CameraPosition>() == null)
            Undo.AddComponent<MAP012_CameraPosition>(overlay);

        EditorSceneManager.MarkSceneDirty(display.scene);

        Debug.Log(
            "[Minimap] Blackspire friendly-unit and camera markers installed."
        );
    }
}
