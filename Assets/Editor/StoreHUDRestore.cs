using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using ObsidianProtocol.UI.Store;

public static class StoreHUDRestore
{
    [MenuItem("Obsidian Protocol/Store/RESTORE STORE HUD")]
    public static void Restore()
    {
        Scene scene = SceneManager.GetActiveScene();

        GameObject hud = GameObject.Find("MILITARY STORE HUD");

        if (hud == null)
        {
            Debug.LogError("[STORE HUD] MILITARY STORE HUD not found.");
            return;
        }

        hud.SetActive(true);
        hud.transform.localScale = Vector3.one;

        Canvas canvas = hud.GetComponent<Canvas>();

        if (canvas == null)
            canvas = hud.GetComponentInChildren<Canvas>(true);

        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        StoreMasterController controller =
            hud.GetComponent<StoreMasterController>();

        if (controller != null)
            controller.enabled = true;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = hud;

        Debug.Log("[STORE HUD] RESTORED AND ENABLED.");
    }
}
