using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FixCheckoutVideoSurfaceScale
{
    [MenuItem("Obsidian Protocol/Store/FIX CHECKOUT VIDEO SURFACE SIZE")]
    public static void Fix()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.IsValid() || !scene.isLoaded ||
            !scene.path.Replace('\\', '/').EndsWith("SCN16_STORE/SCN16_STORE.unity"))
        {
            Debug.LogError("[CHECKOUT SIZE FIX] Open SCN16_STORE first.");
            return;
        }

        GameObject screen = GameObject.Find("CHECKOUT_WALL_VIDEO_SCREEN_NEW");
        if (screen == null)
        {
            Debug.LogError("[CHECKOUT SIZE FIX] Screen not found.");
            return;
        }

        Transform surface = screen.transform.Find("Checkout Video Surface");
        if (surface == null)
        {
            Debug.LogError("[CHECKOUT SIZE FIX] Video surface not found.");
            return;
        }

        Undo.RecordObject(screen.transform, "Resize checkout screen");
        Undo.RecordObject(surface, "Resize checkout video surface");

        screen.transform.localScale = new Vector3(4f, 3f, screen.transform.localScale.z);
        surface.localScale = new Vector3(1f, 1f, surface.localScale.z);

        EditorUtility.SetDirty(screen.transform);
        EditorUtility.SetDirty(surface);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[CHECKOUT SIZE FIX SUCCESS] Screen = 4 wide x 3 high; child surface fills screen.");
    }
}
