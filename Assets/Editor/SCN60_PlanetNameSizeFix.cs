using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public static class SCN60_PlanetNameSizeFix
{
    private const string ScenePath =
        "Assets/Scenes/SCN60_GALAXY_MAP/Galaxy_Map.unity";

    private static readonly string[] PlanetNames =
    {
        "Acheron", "Virellia", "Karthos", "Solara Prime", "Nox Meridian",
        "Thalassa IX", "Dravon's Reach", "Erebos Station", "Halcyon Verge",
        "Myridian Core", "Vantor Helix", "Orpheon Delta", "Cryostria",
        "Zephyria", "Umbra-7", "Caelus Rift", "Pyrrhos Gate", "Luminara",
        "Vexilon", "Astryx", "Kairoth", "Nemeris", "Odyssar",
        "Valtor Prime", "Seraphis", "Kronis-4", "Eon's Cradle",
        "Virex-Theta", "Solaris Echo", "Obsidian World"
    };

    [MenuItem("Obsidian Protocol/SCN60/ENLARGE PLANET NAMES")]
    public static void EnlargePlanetNames()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogError("[SCN60] Stop Play mode first. No changes made.");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            Debug.LogError("[SCN60] Open Galaxy_Map.unity first. No changes made.");
            return;
        }

        List<TMP_Text> sceneLabels = new List<TMP_Text>();

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (TMP_Text candidate in root.GetComponentsInChildren<TMP_Text>(true))
            {
                if (candidate.gameObject.name == "LABEL" &&
                    candidate.gameObject.scene == scene)
                {
                    sceneLabels.Add(candidate);
                }
            }
        }

        Dictionary<string, TMP_Text> matched =
            new Dictionary<string, TMP_Text>();
        List<string> missing = new List<string>();
        List<string> duplicates = new List<string>();

        foreach (string planetName in PlanetNames)
        {
            string[] acceptedTexts = planetName == "Dravon's Reach"
                ? new[] { "Dravon's Reach", "Drovan's Reach" }
                : new[] { planetName };

            List<TMP_Text> found = sceneLabels
                .Where(label => acceptedTexts.Contains(label.text.Trim()))
                .ToList();

            if (found.Count == 0)
            {
                missing.Add(planetName);
            }
            else if (found.Count > 1)
            {
                duplicates.Add(planetName + " (" + found.Count + " labels)");
            }
            else
            {
                matched.Add(planetName, found[0]);
            }
        }

        if (missing.Count > 0 || duplicates.Count > 0 ||
            matched.Count != PlanetNames.Length ||
            matched.Values.Distinct().Count() != PlanetNames.Length)
        {
            Debug.LogError(
                "[SCN60] Validation failed; no changes made. Matched: " +
                matched.Count + "/30. Missing: " +
                string.Join(", ", missing) + ". Duplicates: " +
                string.Join(", ", duplicates)
            );
            return;
        }

        TMP_Text[] labels = matched.Values.ToArray();
        Undo.RecordObjects(labels, "Enlarge Galaxy Map Planet Names");

        foreach (TMP_Text label in labels)
        {
            label.fontSize = 26f;
            EditorUtility.SetDirty(label);
        }

        EditorSceneManager.MarkSceneDirty(scene);

        if (!EditorSceneManager.SaveScene(scene))
        {
            Debug.LogError("[SCN60] Scene save failed. Check the scene in Unity.");
            return;
        }

        Debug.Log("[SCN60] Enlarged all 30 planet-name labels to 26-point text.");
    }
}
