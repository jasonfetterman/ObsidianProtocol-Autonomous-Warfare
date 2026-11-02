using UnityEditor;
using UnityEngine;

public static class RepairAcheronMaterials
{
    private const string PrefabPath = "Assets/Art/Models/Planets/Acheron/ACHERON_REALISTIC_PLANET.prefab";
    private const string TextureFolder = "Assets/Art/Textures/Acheron/";
    private const string MaterialFolder = "Assets/Art/Textures/Acheron/Materials/";

    [MenuItem("Obsidian Protocol/World Building/Repair Acheron Materials")]
    private static void Repair()
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        var albedo = AssetDatabase.LoadAssetAtPath<Texture2D>(TextureFolder + "Acheron_Albedo.png");
        var emission = AssetDatabase.LoadAssetAtPath<Texture2D>(TextureFolder + "Acheron_Emission.png");
        var lava = AssetDatabase.LoadAssetAtPath<Texture2D>(TextureFolder + "Acheron_LavaDetail.png");

        if (shader == null || albedo == null || emission == null || lava == null)
        {
            Debug.LogError("[ACHERON REPAIR] Stopped: required URP shader or verified texture asset is missing.");
            return;
        }

        var surfacePath = MaterialFolder + "Acheron_Surface.mat";
        var cloudPath = MaterialFolder + "Acheron_CloudLayer.mat";

        var surface = AssetDatabase.LoadAssetAtPath<Material>(surfacePath);
        if (surface == null)
        {
            surface = new Material(shader);
            AssetDatabase.CreateAsset(surface, surfacePath);
        }

        surface.shader = shader;
        surface.SetTexture("_BaseMap", albedo);
        surface.SetTexture("_MainTex", albedo);
        surface.SetTexture("_BumpMap", lava);
        surface.SetTexture("_DetailAlbedoMap", lava);
        surface.SetTexture("_EmissionMap", emission);
        surface.SetTextureScale("_MainTex", new Vector2(2f, 1f));
        surface.SetColor("_BaseColor", Color.white);
        surface.SetColor("_Color", Color.white);
        surface.SetColor("_EmissionColor", Color.black);
        surface.SetFloat("_Surface", 0f);
        surface.renderQueue = -1;
        surface.EnableKeyword("_NORMALMAP");
        surface.EnableKeyword("_DETAIL_SCALED");
        EditorUtility.SetDirty(surface);

        var clouds = AssetDatabase.LoadAssetAtPath<Material>(cloudPath);
        if (clouds == null)
        {
            clouds = new Material(shader);
            AssetDatabase.CreateAsset(clouds, cloudPath);
        }

        clouds.shader = shader;
        clouds.SetTexture("_BaseMap", lava);
        clouds.SetTexture("_MainTex", lava);
        clouds.SetTextureScale("_BaseMap", new Vector2(4f, 2f));
        clouds.SetTextureScale("_MainTex", new Vector2(4f, 2f));
        var cloudTint = new Color(0.4705881f, 0.4705881f, 0.4705881f, 0.1764706f);
        clouds.SetColor("_BaseColor", cloudTint);
        clouds.SetColor("_Color", cloudTint);
        clouds.SetFloat("_Surface", 1f);
        clouds.SetFloat("_Blend", 0f);
        clouds.SetFloat("_SrcBlend", 1f);
        clouds.SetFloat("_DstBlend", 10f);
        clouds.SetFloat("_SrcBlendAlpha", 1f);
        clouds.SetFloat("_DstBlendAlpha", 10f);
        clouds.SetFloat("_ZWrite", 0f);
        clouds.renderQueue = 3000;
        clouds.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        clouds.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        EditorUtility.SetDirty(clouds);

        AssetDatabase.SaveAssets();

        var root = PrefabUtility.LoadPrefabContents(PrefabPath);
        if (root == null)
        {
            Debug.LogError("[ACHERON REPAIR] Could not load the planet prefab.");
            return;
        }

        try
        {
            var surfaceTransform = System.Array.Find(root.GetComponentsInChildren<Transform>(true), t => t.name == "SURFACE");
            var cloudTransform = System.Array.Find(root.GetComponentsInChildren<Transform>(true), t => t.name == "CLOUD_LAYER");

            if (surfaceTransform == null || cloudTransform == null)
            {
                Debug.LogError("[ACHERON REPAIR] Stopped: SURFACE or CLOUD_LAYER was not found. Prefab was not saved.");
                return;
            }

            var surfaceRenderer = surfaceTransform.GetComponent<Renderer>();
            var cloudRenderer = cloudTransform.GetComponent<Renderer>();

            if (surfaceRenderer == null || cloudRenderer == null)
            {
                Debug.LogError("[ACHERON REPAIR] Stopped: a required renderer is missing. Prefab was not saved.");
                return;
            }

            surfaceRenderer.sharedMaterial = surface;
            cloudRenderer.sharedMaterial = clouds;

            PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            Debug.Log("[ACHERON REPAIR] SUCCESS: surface and cloud materials assigned to the Acheron prefab. Atmosphere and scenes were not changed.");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }
}
