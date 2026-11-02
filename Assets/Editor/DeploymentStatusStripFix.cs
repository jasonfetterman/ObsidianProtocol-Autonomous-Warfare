using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class DeploymentStatusStripFix
{
    private const string ScenePath = "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    private static readonly (string name, Color color)[] Strips =
    {
        ("[LIGHT] STATUS STRIP_doctrine_control", new Color32(170, 70, 255, 255)),
        ("[LIGHT] STATUS STRIP_command_intent", new Color32(60, 140, 255, 255)),
        ("[LIGHT] STATUS STRIP_command_structure", new Color32(80, 190, 255, 255)),
        ("[LIGHT] STATUS STRIP_operationselection", new Color32(0, 220, 255, 255)),
        ("[LIGHT] STATUS STRIP_rules_of_engagement", new Color32(255, 190, 40, 255)),
        ("[LIGHT] STATUS STRIP_deployment_budget", new Color32(57, 255, 136, 255)),
        ("[LIGHT] STATUS STRIP_force_composition", new Color32(255, 140, 40, 255)),
        ("[LIGHT] STATUS STRIP_formation_control", new Color32(40, 230, 200, 255))
    };

    [MenuItem("Obsidian Protocol/Deployment/Fix Status Strips")]
    public static void Fix()
    {
        var scene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(ScenePath);

        foreach (var strip in Strips)
        {
            var go = GameObject.Find(strip.name);

            if (go == null)
            {
                Debug.LogError("[STATUS STRIPS] Missing: " + strip.name);
                continue;
            }

            var renderer = go.GetComponent<MeshRenderer>();

            if (renderer == null)
            {
                Debug.LogError("[STATUS STRIPS] No MeshRenderer: " + strip.name);
                continue;
            }

            var source = renderer.sharedMaterial;

            if (source == null)
            {
                Debug.LogError("[STATUS STRIPS] No material: " + strip.name);
                continue;
            }

            var material = new Material(source);
            material.name = "MAT_STATUS_" + strip.name
                .Replace("[LIGHT] STATUS STRIP_", "")
                .Replace(" ", "_");

            material.SetColor("_BaseColor", strip.color);
            material.SetColor("_Color", strip.color);
            material.SetColor("_EmissionColor", strip.color * 1.5f);

            material.EnableKeyword("_EMISSION");
            material.SetFloat("_EmissionIntensity", 1.5f);

            renderer.sharedMaterial = material;

            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            Debug.Log("[STATUS STRIPS] Fixed: " + strip.name);
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(scene);
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(scene);

        Debug.Log("[STATUS STRIPS] ALL 8 COMPLETE.");
    }
}

