using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public static class GarageOfficeActivationBuilder
{
    [MenuItem("Obsidian Protocol/Garage/BUILD OFFICE ACTIVATION POINTS")]
    public static void Build()
    {
        var scene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        if (!scene.path.Contains("SCN04_GARAGE"))
        {
            Debug.LogError("Open SCN04_GARAGE first.");
            return;
        }

        var root = GameObject.Find("GARAGE_PERIMETER_OFFICES");
        if (root == null) { Debug.LogError("GARAGE_PERIMETER_OFFICES not found."); return; }

        string[] offices = { "COMMAND", "INTELLIGENCE", "RESEARCH", "DEPLOYMENT", "MAINTENANCE", "LOGISTICS" };
        string[] panels = { "CommandCenterPanel", "AICorePanel", "UpgradePanel", "DeploymentPanel", "RepairPanel", "StoragePanel" };

        var dark = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        dark.color = new Color(0.035f, 0.09f, 0.12f);
        var cyan = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        cyan.color = new Color(0.0f, 0.8f, 1.0f);

        for (int i = 0; i < offices.Length; i++)
        {
            var office = root.transform.Find(offices[i] + "_OFFICE");
            if (office == null) { Debug.LogWarning("Office missing: " + offices[i]); continue; }

            var old = office.Find("ACTIVATION_CONSOLE");
            if (old != null) Object.DestroyImmediate(old.gameObject);

            var console = GameObject.CreatePrimitive(PrimitiveType.Cube);
            console.name = "ACTIVATION_CONSOLE";
            console.transform.SetParent(office, false);
            console.transform.localPosition = new Vector3(0f, 0.65f, 0f);
            console.transform.localScale = new Vector3(0.65f, 1.3f, 0.45f);
            console.GetComponent<Renderer>().sharedMaterial = dark;

            var screen = GameObject.CreatePrimitive(PrimitiveType.Cube);
            screen.name = "ACCESS_DISPLAY";
            screen.transform.SetParent(console.transform, false);
            screen.transform.localPosition = new Vector3(0f, 0.12f, -0.51f);
            screen.transform.localScale = new Vector3(0.82f, 0.3f, 0.035f);
            screen.GetComponent<Renderer>().sharedMaterial = cyan;
            Object.DestroyImmediate(screen.GetComponent<Collider>());

            var promptObject = new GameObject("INTERACTION_PROMPT");
            promptObject.transform.SetParent(console.transform, false);
            promptObject.transform.localPosition = new Vector3(0f, 0.95f, -0.3f);
            promptObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            var prompt = promptObject.AddComponent<TextMesh>();
            prompt.text = "PRESS E TO ACCESS";
            prompt.fontSize = 48;
            prompt.characterSize = 0.045f;
            prompt.anchor = TextAnchor.MiddleCenter;
            prompt.alignment = TextAlignment.Center;
            prompt.color = new Color(0.2f, 0.9f, 1f);
            prompt.fontStyle = FontStyle.Bold;

            var activation = console.AddComponent<GarageOfficeActivation>();
            var serialized = new SerializedObject(activation);
            serialized.FindProperty("panelName").stringValue = panels[i];
            serialized.ApplyModifiedPropertiesWithoutUndo();

            Debug.Log("Activation installed: " + offices[i] + " -> " + panels[i]);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("GARAGE OFFICE ACTIVATION POINTS BUILT.");
    }
}

