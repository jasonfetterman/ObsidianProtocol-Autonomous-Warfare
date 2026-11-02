using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class SCN17LightingUpgrade
{
    const string ScenePath = "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";

    [MenuItem("Obsidian Protocol/SCN17/Upgrade Existing Lighting")]
    static void Upgrade()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            Debug.LogError("[SCN17 LIGHTING] Open SCN17_PERSISTENT_MILITARY first.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        int ceiling = 0, accents = 0, entrance = 0, key = 0;

        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (go == null || !go.scene.IsValid() || go.scene.path != ScenePath)
                continue;

            Light light = go.GetComponent<Light>();
            if (light == null) continue;

            if (go.name == "CEILING LIGHT")
            {
                Undo.RecordObject(light, "Upgrade Ceiling Lighting");
                light.color = new Color(0.72f, 0.88f, 1f);
                light.intensity = 2.0f;
                light.range = 20f;
                light.shadows = LightShadows.Soft;
                light.shadowStrength = 0.45f;
                ceiling++;
            }
            else if (go.name == "CYAN COMMAND LIGHT")
            {
                Undo.RecordObject(light, "Tune Command Lighting");
                light.intensity = 4.5f;
                light.range = 20f;
                light.shadows = LightShadows.Soft;
                light.shadowStrength = 0.35f;
                accents++;
            }
            else if (go.name == "AMBER REPAIR LIGHT")
            {
                Undo.RecordObject(light, "Tune Repair Lighting");
                light.intensity = 3.2f;
                light.range = 16f;
                light.shadows = LightShadows.Soft;
                light.shadowStrength = 0.3f;
                accents++;
            }
            else if (go.name == "BLUE DEVELOPMENT LIGHT")
            {
                Undo.RecordObject(light, "Tune Development Lighting");
                light.intensity = 3.5f;
                light.range = 19f;
                light.shadows = LightShadows.Soft;
                light.shadowStrength = 0.3f;
                accents++;
            }
            else if (go.name == "ENTRANCE LIGHT LEFT" ||
                     go.name == "ENTRANCE LIGHT RIGHT")
            {
                Undo.RecordObject(light, "Tune Entrance Lighting");
                light.intensity = 2.2f;
                light.range = 6f;
                entrance++;
            }
            else if (go.name == "FACILITY KEY LIGHT")
            {
                Undo.RecordObject(light, "Tune Facility Key Light");
                light.intensity = 0.9f;
                light.shadows = LightShadows.Soft;
                light.shadowStrength = 0.4f;
                key++;
            }

            if (light != null)
                EditorUtility.SetDirty(light);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("[SCN17 LIGHTING] Updated " + ceiling +
            " ceiling lights, " + accents + " accent lights, " +
            entrance + " entrance lights, and " + key +
            " key lights. No new lights or position changes.");
    }
}
