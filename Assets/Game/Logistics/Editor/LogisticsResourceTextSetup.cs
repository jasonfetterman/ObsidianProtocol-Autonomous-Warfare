#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class LogisticsResourceTextSetup
{
    [MenuItem("Obsidian Protocol/Logistics/Convert Resource Text To TMP")]
    public static void Run()
    {
        if (Application.isPlaying)
        {
            Debug.LogError("[LOGISTICS] STOP PLAY MODE FIRST.");
            return;
        }

        var scene = EditorSceneManager.OpenScene(
            "Assets/Scenes/SCN-08  LOGISTICS BAY/[HUD] LOGISTICS HUD/Logistics_Bay.unity");

        string[] resources =
        {
            "MEAT","WOOD","COAL","IRON","ALLOY","ELECTRONICS","FUEL"
        };

        foreach (string resource in resources)
        {
            Convert("RESOURCE OVERVIEW/" + resource + "/VALUE");
            Convert("RESOURCE OVERVIEW/" + resource + "/RATE");
        }

        Convert("STORAGE/fuel/fuel_value"); Convert("STORAGE/energy/energy_value"); Convert("STORAGE/materials/materials_value"); Convert("STORAGE/electronics/electronics_value"); EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[LOGISTICS] ALL RESOURCE VALUE/RATE TEXT CONVERTED TO TMP.");
    }

    private static void Convert(string path)
    {
        GameObject hud = GameObject.Find("LOGISTICS HUD");
        if (hud == null)
        {
            Debug.LogError("[LOGISTICS] LOGISTICS HUD NOT FOUND.");
            return;
        }

        Transform target = hud.transform.Find(path);
        if (target == null)
        {
            Debug.LogWarning("[LOGISTICS] NOT FOUND: " + path);
            return;
        }

        Text oldText = target.GetComponent<Text>();
        TMP_Text existingTMP = target.GetComponent<TMP_Text>();

        if (existingTMP != null)
            return;

        string oldValue = oldText != null ? oldText.text : "0";

        if (oldText != null)
            Object.DestroyImmediate(oldText);

        TextMeshProUGUI tmp = target.gameObject.AddComponent<TextMeshProUGUI>();
        tmp.text = oldValue;
        tmp.fontSize = 24;
        tmp.alignment = TextAlignmentOptions.Center;
    }
}
#endif

