using UnityEditor;
using UnityEngine;

public static class InspectHarlequinAnimations
{
    [MenuItem("Obsidian Protocol/Unit Takeover/Inspect Harlequin Animations")]
    private static void Run()
    {
        const string path = "Assets/Art/Units/Clown Syndicate/Clown_Assault_Mech/textures/autorig-7e7ecd87-v4-glb.glb";
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(path))
        {
            if (asset is AnimationClip clip)
                Debug.Log("[HARLEQUIN CLIP] " + clip.name + " | Length: " + clip.length + " | Legacy: " + clip.legacy);
            else if (asset is GameObject model)
                Debug.Log("[HARLEQUIN MODEL] " + model.name);
        }
    }
}
