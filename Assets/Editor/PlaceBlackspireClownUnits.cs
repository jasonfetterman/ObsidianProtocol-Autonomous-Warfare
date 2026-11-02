using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlaceBlackspireClownUnits
{
    [MenuItem("Obsidian Protocol/World Building/Place Clown Syndicate Units")]
    public static void Run()
    {
        if (SceneManager.GetActiveScene().path !=
            "Assets/Scenes/SCN30_ACHERON/Blackspire_Continent.unity")
        {
            Debug.LogError("[CLOWN PLACEMENT] Open Blackspire_Continent first.");
            return;
        }

        const string folder = "Assets/Art/Models/ClownSyndicate/";
        const string rootName = "CLOWN_SYNDICATE_FIELD_UNITS";

        if (GameObject.Find(rootName))
        {
            Debug.LogWarning("[CLOWN PLACEMENT] Units already exist. Nothing added.");
            return;
        }

        GameObject citadel = GameObject.Find("CRIMSON_CLOWN_CITADEL_HEADQUARTERS");
        if (!citadel)
        {
            Debug.LogError("[CLOWN PLACEMENT] Existing citadel not found. Nothing added.");
            return;
        }

        string[] models =
        {
            folder + "Carnival_Nightmare/Meshy_AI_Carnival_Nightmare_20261018_texture.fbx",
            folder + "Clrimson_Warbuggy/Meshy_AI_Crimson_Clown_Warbuggy_20261018_texture.fbx",
            folder + "Crimson_Armored_Vehicle/Meshy_AI_Clown_Armored_Vehicle_20261018_texture.fbx",
            folder + "Crimson_Crawler/Meshy_AI_Crimson_Crawler_20261018_texture.fbx",
            folder + "Crimson_Fang_Armored_Rover/Meshy_AI_Crimson_Fang_Armored_Rover_20261018_texture.fbx",
            folder + "Crimson_Warbeast/Meshy_AI_Crimson_Warbeast_20261018_texture.fbx",
            folder + "Harlequin_War_Machine/Meshy_AI_Harlequin_War_Machine_20261018_texture.fbx",
            folder + "Jester_Hound_Recon_Drone/Meshy_AI_Jester_Hound_Recon_Drone_20261018_texture.fbx"
        };

        Vector3[] offsets =
        {
            new Vector3(-130, 0, -15),
            new Vector3(-95, 0, -100),
            new Vector3(0, 0, -145),
            new Vector3(100, 0, -100),
            new Vector3(145, 0, 0),
            new Vector3(100, 0, 100),
            new Vector3(0, 0, 145),
            new Vector3(-100, 0, 100)
        };

        GameObject root = new GameObject(rootName);
        Undo.RegisterCreatedObjectUndo(root, "Create Clown Syndicate Unit Group");
        root.transform.position = citadel.transform.position;

        int added = 0;

        for (int i = 0; i < models.Length; i++)
        {
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(models[i]);
            if (!asset)
            {
                Debug.LogWarning("[CLOWN PLACEMENT] Missing asset: " + models[i]);
                continue;
            }

            GameObject unit = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            if (!unit) continue;

            Undo.RegisterCreatedObjectUndo(unit, "Place Clown Syndicate Unit");
            unit.name = asset.name.Replace("_20261018_texture", "");
            unit.transform.SetParent(root.transform, true);
            unit.transform.localScale = Vector3.one * 1000f;
            unit.transform.position = citadel.transform.position + offsets[i];
            unit.transform.rotation = Quaternion.Euler(0, citadel.transform.eulerAngles.y, 0);

            added++;
            Debug.Log("[CLOWN PLACEMENT] Added " + unit.name +
                " at " + unit.transform.position.ToString("F1") +
                " | Scale=" + unit.transform.localScale.ToString("F0"));
        }

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("[CLOWN PLACEMENT] Added " + added +
            " units. Existing citadel was not changed. Inspect positions before saving.");
    }
}
