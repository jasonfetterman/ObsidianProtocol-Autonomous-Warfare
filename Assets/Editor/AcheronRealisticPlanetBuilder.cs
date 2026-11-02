using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AcheronRealisticPlanetBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN30_ACHERON/Acheron.unity";
    private const string SurfaceName = "ACHERON_SINGLE_CONTINUOUS_SURFACE";
    private const int Seed = 30147;

    [MenuItem("Obsidian Protocol/Acheron/BUILD REALISTIC PLANET - SINGLE SURFACE")]
    public static void Build()
    {
        EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        GameObject surface = GameObject.Find(SurfaceName);

        if (surface == null)
        {
            Debug.LogError("ACHERON: " + SurfaceName + " was not found.");
            return;
        }

        MeshFilter filter = surface.GetComponent<MeshFilter>();
        MeshRenderer renderer = surface.GetComponent<MeshRenderer>();

        if (filter == null || renderer == null || filter.sharedMesh == null)
        {
            Debug.LogError("ACHERON: Existing planet surface is missing MeshFilter, MeshRenderer, or mesh.");
            return;
        }

        CreateRealisticMaterials(renderer);

        DisableExtraPlanetShells();

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("ACHERON: EXISTING SINGLE SURFACE IMPROVED.");
        Debug.Log("ACHERON: No planet spheres, clouds, or atmosphere geometry were created.");
    }

    private static void CreateRealisticMaterials(MeshRenderer renderer)
    {
        Material ocean = CreateMaterial(
            "ACHERON_OCEAN_REALISTIC",
            new Color(0.006f, 0.018f, 0.032f),
            0.35f,
            0.72f
        );

        Material land = CreateMaterial(
            "ACHERON_LAND_REALISTIC",
            new Color(0.22f, 0.075f, 0.028f),
            0.05f,
            0.42f
        );

        Material mountains = CreateMaterial(
            "ACHERON_MOUNTAINS_REALISTIC",
            new Color(0.055f, 0.040f, 0.032f),
            0.02f,
            0.28f
        );

        renderer.sharedMaterials = new[]
        {
            ocean,
            land,
            mountains
        };
    }

    private static Material CreateMaterial(
        string name,
        Color color,
        float metallic,
        float smoothness)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        Material material = new Material(shader);
        material.name = name;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);

        if (material.HasProperty("_Color"))
            material.SetColor("_Color", color);

        if (material.HasProperty("_Metallic"))
            material.SetFloat("_Metallic", metallic);

        if (material.HasProperty("_Smoothness"))
            material.SetFloat("_Smoothness", smoothness);

        return material;
    }

    private static void DisableExtraPlanetShells()
    {
        string[] names =
        {
            "ATMOSPHERE",
            "ATMOSPHERE_REALISTIC",
            "CLOUD_LAYER"
        };

        foreach (string name in names)
        {
            GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in objects)
            {
                if (obj == null)
                    continue;

                if (!obj.scene.IsValid())
                    continue;

                if (obj.name != name)
                    continue;

                Renderer r = obj.GetComponent<Renderer>();

                if (r != null)
                    r.enabled = false;

                obj.SetActive(false);

                Debug.Log("ACHERON: Disabled extra shell: " + name);
            }
        }
    }
}
