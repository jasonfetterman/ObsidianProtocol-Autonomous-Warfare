using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreScreenDiagnostic
{
    [MenuItem("Obsidian Protocol/Store/Diagnose Screen Placement")]
    public static void Run()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (EditorApplication.isPlaying ||
            scene.path != "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity")
        {
            Debug.LogError("Open SCN16_STORE and stop Play mode. Nothing changed.");
            return;
        }

        string[] names =
        {
            "AIR UNITS wall screen",
            "GROUND UNITS wall screen",
            "SEA UNITS wall screen",
            "COMMANDERS wall screen",
            "EXPERIMENTAL UNITS wall screen",
            "SATELLITES wall screen"
        };

        foreach (string name in names)
        {
            GameObject screen = null;

            foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.scene == scene && go.name == name)
                {
                    screen = go;
                    break;
                }
            }

            if (screen == null)
            {
                Debug.LogError("[STORE DIAGNOSTIC] Missing screen: " + name);
                continue;
            }

            Transform s = screen.transform;
            Renderer renderer = screen.GetComponent<Renderer>();

            Debug.Log(
                "\n[STORE SCREEN] " + name +
                "\n  World position: " + s.position +
                "\n  World rotation: " + s.eulerAngles +
                "\n  Local scale: " + s.localScale +
                "\n  World scale: " + s.lossyScale +
                "\n  Forward: " + s.forward +
                "\n  Renderer bounds: " +
                (renderer != null ? renderer.bounds.ToString() : "NO RENDERER"));

            Transform ui = s.Find("PROCUREMENT UI");

            if (ui == null)
            {
                Debug.LogWarning("  PROCUREMENT UI not found.");
                continue;
            }

            Debug.Log(
                "  UI local position: " + ui.localPosition +
                "\n  UI world position: " + ui.position +
                "\n  UI local rotation: " + ui.localEulerAngles +
                "\n  UI world rotation: " + ui.eulerAngles +
                "\n  UI local scale: " + ui.localScale +
                "\n  UI world scale: " + ui.lossyScale);

            int count = 0;

            foreach (Transform child in ui.GetComponentsInChildren<Transform>(true))
            {
                if (child == ui || count >= 8)
                    continue;

                TextMesh text = child.GetComponent<TextMesh>();
                Renderer childRenderer = child.GetComponent<Renderer>();

                Debug.Log(
                    "  CHILD " + child.name +
                    (text != null ? " | TEXT: " + text.text : "") +
                    " | local pos: " + child.localPosition +
                    " | world pos: " + child.position +
                    " | world scale: " + child.lossyScale +
                    (childRenderer != null
                        ? " | bounds: " + childRenderer.bounds
                        : ""));

                count++;
            }
        }

        Debug.Log("[STORE DIAGNOSTIC] Complete. No scene objects were modified.");
    }
}
