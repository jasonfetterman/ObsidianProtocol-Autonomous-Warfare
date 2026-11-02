using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class SCN60_QuickActionsTextFix
{
    [MenuItem("Obsidian Protocol/SCN60/FIX QUICK ACTIONS TEXT SIZE")]
    public static void Fix()
    {
        const string scenePath =
            "Assets/Scenes/SCN60_GALAXY_MAP/Galaxy_Map.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        var root = GameObject.Find("HUD-201 GalaxyMapHUD");
        if (root == null)
        {
            Debug.LogError("[SCN60] HUD-201 GalaxyMapHUD not found.");
            return;
        }

        string[] buttons =
        {
            "HUD-201-06A JumpToHomeworldButton",
            "HUD-201-06B JumpToLastViewedPlanetButton",
            "HUD-201-06C OpenIntelNetworkButton",
            "HUD-201-06D OpenDoctrineManagerButton",
            "HUD-201-06E OpenLogisticsOverviewButton"
        };

        foreach (string buttonName in buttons)
        {
            Transform button = root.transform.Find(
                "HUD-201-06 QuickActionsPanel/" + buttonName
            );

            if (button == null)
            {
                Debug.LogWarning(
                    "[SCN60] Missing Quick Action button: " + buttonName
                );
                continue;
            }

            Transform label = button.Find("LABEL");
            if (label == null)
            {
                Debug.LogWarning(
                    "[SCN60] Missing LABEL under: " + buttonName
                );
                continue;
            }

            Text text = label.GetComponent<Text>();
            if (text != null)
            {
                text.fontSize = 14;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 10;
                text.resizeTextMaxSize = 14;
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[SCN60] Quick Actions text size fixed.");
    }
}
