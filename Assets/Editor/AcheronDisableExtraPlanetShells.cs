using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AcheronDisableExtraPlanetShells
{
    [MenuItem("Obsidian Protocol/Acheron/DISABLE EXTRA PLANET SHELLS")]
    public static void Disable()
    {
        Scene scene = SceneManager.GetActiveScene();

        string[] objectsToDisable =
        {
            "ATMOSPHERE",
            "CLOUD_LAYER"
        };

        int count = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform[] all = root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in all)
            {
                if (System.Array.IndexOf(objectsToDisable, t.name) >= 0)
                {
                    t.gameObject.SetActive(false);
                    count++;
                    Debug.Log("ACHERON: Disabled extra planet shell: " + t.name);
                }
            }
        }

        Debug.Log("ACHERON: Disabled " + count + " extra planet shell object(s).");
    }
}
