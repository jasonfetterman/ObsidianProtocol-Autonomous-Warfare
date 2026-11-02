using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AcheronDisableAtmosphereRealistic
{
    [MenuItem("Obsidian Protocol/Acheron/DISABLE REALISTIC ATMOSPHERE")]
    public static void Disable()
    {
        Scene scene = SceneManager.GetActiveScene();

        int count = 0;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            Transform[] all = root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in all)
            {
                if (t.name == "ATMOSPHERE_REALISTIC")
                {
                    t.gameObject.SetActive(false);
                    count++;
                    Debug.Log("ACHERON: Disabled ATMOSPHERE_REALISTIC.");
                }
            }
        }

        Debug.Log("ACHERON: Disabled " + count + " ATMOSPHERE_REALISTIC object(s).");
    }
}
