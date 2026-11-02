using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AcheronRebuildPlanet
{
    [MenuItem("Obsidian Protocol/Acheron/REBUILD PLANET")]
    public static void Build()
    {
        GameObject oldPlanet =
            GameObject.Find("ACHERON_REALISTIC_PLANET");

        if (oldPlanet != null)
        {
            Object.DestroyImmediate(oldPlanet);
        }

        GameObject root =
            new GameObject("ACHERON_REALISTIC_PLANET");

        GameObject surface =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        surface.name = "SURFACE";
        surface.transform.SetParent(root.transform);
        surface.transform.localPosition = Vector3.zero;
        surface.transform.localScale =
            Vector3.one * 500f;

        Renderer sr =
            surface.GetComponent<Renderer>();

        Material surfaceMat =
            new Material(
                Shader.Find(
                    "Universal Render Pipeline/Lit"));

        surfaceMat.color =
            new Color(
                0.05f,
                0.04f,
                0.04f);

        surfaceMat.EnableKeyword("_EMISSION");

        surfaceMat.SetColor(
            "_EmissionColor",
            new Color(
                10f,
                2.5f,
                0.4f));

        sr.sharedMaterial =
            surfaceMat;

        GameObject atmosphere =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        atmosphere.name =
            "ATMOSPHERE";

        atmosphere.transform.SetParent(
            root.transform);

        atmosphere.transform.localScale =
            Vector3.one * 515f;

        Renderer ar =
            atmosphere.GetComponent<
                Renderer>();

        Material atm =
            new Material(
                Shader.Find(
                    "Universal Render Pipeline/Lit"));

        atm.SetColor(
            "_BaseColor",
            new Color(
                1f,
                0.35f,
                0.1f,
                0.15f));

        ar.sharedMaterial = atm;

        GameObject clouds =
            GameObject.CreatePrimitive(
                PrimitiveType.Sphere);

        clouds.name =
            "CLOUD_LAYER";

        clouds.transform.SetParent(
            root.transform);

        clouds.transform.localScale =
            Vector3.one * 507f;

        Renderer cr =
            clouds.GetComponent<
                Renderer>();

        Material cm =
            new Material(
                Shader.Find(
                    "Universal Render Pipeline/Lit"));

        cm.SetColor(
            "_BaseColor",
            new Color(
                0.8f,
                0.8f,
                0.8f,
                0.25f));

        cr.sharedMaterial = cm;

        Debug.Log(
            "ACHERON PLANET REBUILT");

        EditorSceneManager.MarkSceneDirty(
            SceneManager.GetActiveScene());

        EditorSceneManager.SaveScene(
            SceneManager.GetActiveScene());
    }
}