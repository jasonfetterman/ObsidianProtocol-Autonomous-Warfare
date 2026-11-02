using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DeploymentFacilitySignageBuilder
{
    private const string ScenePath =
        "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

    private const string RootName = "[FACILITY] GENERATED SIGNAGE";

    [MenuItem("Obsidian Protocol/Deployment/Build Industrial Signage")]
    private static void BuildSignage()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog(
                "Facility Signage",
                "Exit Play Mode before building signage.",
                "OK");
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != ScenePath)
        {
            EditorUtility.DisplayDialog(
                "Wrong Scene",
                "Open SCN12_DEPLOYMENT before running this builder.",
                "OK");
            return;
        }

        if (FindSceneObject(scene, RootName) != null)
        {
            EditorUtility.DisplayDialog(
                "Already Built",
                "Signage already exists. No new signs were created.",
                "OK");
            return;
        }

        GameObject building = FindSceneObject(scene, "BUILDING");

        if (building == null)
        {
            EditorUtility.DisplayDialog(
                "Missing BUILDING",
                "Could not find BUILDING. No changes made.",
                "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog(
            "Build Industrial Signage",
            "Create six industrial deployment signs?",
            "Build",
            "Cancel"))
            return;

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Facility Signage");
        Undo.SetTransformParent(
            root.transform, building.transform, "Parent Facility Signage");

        CreateSign(root.transform,
            "SIGN DEPLOYMENT CONTROL", "DEPLOYMENT CONTROL",
            new Vector3(0f, 10.2f, -25.8f), 0f,
            new Color(0.08f, 0.65f, 0.85f));

        CreateSign(root.transform,
            "SIGN AUTHORIZED PERSONNEL", "AUTHORIZED PERSONNEL ONLY",
            new Vector3(-41.2f, 8f, 8f), 90f,
            new Color(1f, 0.62f, 0.12f));

        CreateSign(root.transform,
            "SIGN RESTRICTED OPERATIONS", "RESTRICTED OPERATIONS",
            new Vector3(41.2f, 8f, 8f), -90f,
            new Color(0.95f, 0.22f, 0.16f));

        CreateSign(root.transform,
            "SIGN FINAL AUTHORIZATION", "FINAL AUTHORIZATION",
            new Vector3(0f, 9f, 33.8f), 180f,
            new Color(0.12f, 0.8f, 0.48f));

        CreateSign(root.transform,
            "SIGN LEFT OPERATIONS", "OPERATIONS - LEFT",
            new Vector3(-30f, 7f, 28f), 90f,
            new Color(0.12f, 0.65f, 0.85f));

        CreateSign(root.transform,
            "SIGN RIGHT SUPPORT", "SUPPORT - RIGHT",
            new Vector3(30f, 7f, 28f), -90f,
            new Color(0.12f, 0.65f, 0.85f));

        EditorSceneManager.MarkSceneDirty(scene);
        Selection.activeGameObject = root;

        EditorUtility.DisplayDialog(
            "Signage Created",
            "Six signs created. Inspect them before saving.",
            "OK");
    }

    private static void CreateSign(
        Transform parent,
        string objectName,
        string label,
        Vector3 localPosition,
        float yaw,
        Color accentColor)
    {
        GameObject sign = new GameObject(objectName);
        Undo.RegisterCreatedObjectUndo(sign, "Create " + objectName);
        sign.transform.SetParent(parent, false);
        sign.transform.localPosition = localPosition;
        sign.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);

        GameObject backing = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(backing, "Create Sign Backing");
        backing.name = "ARMORED SIGN PANEL";
        backing.transform.SetParent(sign.transform, false);
        backing.transform.localPosition = Vector3.zero;
        backing.transform.localRotation = Quaternion.identity;
        backing.transform.localScale = new Vector3(10f, 1.8f, 0.18f);
        RemoveCollider(backing);

        SetMaterial(
            backing,
            objectName + " Panel Material",
            new Color(0.045f, 0.065f, 0.08f, 1f),
            0.65f,
            0.7f);

        GameObject accent = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Undo.RegisterCreatedObjectUndo(accent, "Create Sign Accent");
        accent.name = "ACCENT STRIPE";
        accent.transform.SetParent(sign.transform, false);
        accent.transform.localPosition = new Vector3(-4.65f, 0f, -0.115f);
        accent.transform.localRotation = Quaternion.identity;
        accent.transform.localScale = new Vector3(0.25f, 1.45f, 0.05f);
        RemoveCollider(accent);

        SetMaterial(
            accent,
            objectName + " Accent Material",
            accentColor,
            0.25f,
            0.6f);

        GameObject textObject = new GameObject("SIGN TEXT");
        Undo.RegisterCreatedObjectUndo(textObject, "Create Sign Text");
        textObject.transform.SetParent(sign.transform, false);

        // TextMesh faces local +Z. The sign's front is local -Z,
        // so rotate the text to face outward from the panel.
        textObject.transform.localPosition = new Vector3(0f, 0f, -0.101f);
        textObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        textObject.transform.localScale = Vector3.one;

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = label;
        textMesh.fontSize = 64;
        textMesh.characterSize = 0.12f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = new Color(0.75f, 0.9f, 0.96f, 1f);
        textMesh.fontStyle = FontStyle.Bold;
    }

    private static void RemoveCollider(GameObject target)
    {
        Collider collider = target.GetComponent<Collider>();

        if (collider != null)
            Object.DestroyImmediate(collider);
    }

    private static void SetMaterial(
        GameObject target,
        string materialName,
        Color color,
        float metallic,
        float smoothness)
    {
        Renderer renderer = target.GetComponent<Renderer>();

        if (renderer == null)
            return;

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        if (shader == null)
            return;

        Material material = new Material(shader);
        material.name = materialName;
        material.color = color;

        if (material.HasProperty("_Metallic"))
            material.SetFloat("_Metallic", metallic);

        if (material.HasProperty("_Smoothness"))
            material.SetFloat("_Smoothness", smoothness);

        renderer.sharedMaterial = material;
    }

    private static GameObject FindSceneObject(
        Scene scene, string objectName)
    {
        foreach (GameObject sceneRoot in scene.GetRootGameObjects())
        {
            foreach (Transform child in
                sceneRoot.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == objectName)
                    return child.gameObject;
            }
        }

        return null;
    }
}