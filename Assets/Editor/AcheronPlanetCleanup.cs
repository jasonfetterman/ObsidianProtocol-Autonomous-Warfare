using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AcheronPlanetCleanup
{
    [MenuItem("Obsidian Protocol/Acheron/CLEAN ALL PLANET GEOMETRY")]
    public static void Clean()
    {
        string[] remove =
        {
            "ACHERON_REALISTIC_PLANET",
            "ACHERON_SINGLE_CONTINUOUS_SURFACE",
            "PLANET_SURFACE",
            "CONTINENTS_AND_TERRAIN",
            "Continent_1",
            "Continent_2",
            "Continent_3",
            "Continent_4",
            "VOLCANIC_MOUNTAIN_0",
            "VOLCANIC_MOUNTAIN_1",
            "VOLCANIC_MOUNTAIN_2",
            "VOLCANIC_MOUNTAIN_3",
            "VOLCANIC_MOUNTAIN_4",
            "VOLCANIC_MOUNTAIN_5",
            "VOLCANIC_MOUNTAIN_6"
        };

        foreach (string name in remove)
        {
            GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in objects)
            {
                if (obj == null || !obj.scene.IsValid())
                    continue;

                if (obj.name == name)
                    Object.DestroyImmediate(obj);
            }
        }

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("ACHERON: ALL OLD PLANET GEOMETRY REMOVED.");
    }
}
