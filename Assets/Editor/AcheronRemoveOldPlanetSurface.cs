using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AcheronRemoveOldPlanetSurface
{
    [MenuItem("Obsidian Protocol/Acheron/REMOVE OLD PLANET SURFACE")]
    public static void Remove()
    {
        GameObject old = GameObject.Find("PLANET_SURFACE");

        if (old != null)
        {
            Renderer r = old.GetComponent<Renderer>();

            if (r != null)
                r.enabled = false;

            Debug.Log("Acheron: OLD PLANET_SURFACE renderer disabled.");
        }
        else
        {
            Debug.Log("Acheron: PLANET_SURFACE not found.");
        }

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );
    }
}
