using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StoreNeonHeaderMaterialSetup
{
    const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";
    const string ShaderName = "ObsidianProtocol/StoreNeonText";

    static readonly string[] Names =
    {
        "AIR UNITS wall header text",
        "GROUND UNITS wall header text",
        "SEA UNITS wall header text",
        "COMMANDERS wall header text",
        "EXPERIMENTAL UNITS wall header text",
        "SATELLITES wall header text"
    };

    static readonly Color[] Colors =
    {
        new Color(0.00f, 0.88f, 1.00f),
        new Color(1.00f, 0.48f, 0.08f),
        new Color(0.00f, 1.00f, 0.58f),
        new Color(1.00f, 0.82f, 0.20f),
        new Color(1.00f, 0.16f, 0.43f),
        new Color(0.62f, 0.42f, 1.00f)
    };

    [MenuItem("Obsidian Protocol/Store/Apply Neon Header Materials")]
    static void Apply()
    {
        var scene = SceneManager.GetActiveScene();
        if (EditorApplication.isPlaying || scene.path != ScenePath)
        {
            Debug.LogError("Stop Play mode and open SCN16_STORE. No changes made.");
            return;
        }

        var shader = Shader.Find(ShaderName);
        if (shader == null)
        {
            Debug.LogError("Neon shader not found or not compiled: " + ShaderName);
            return;
        }

        var labels = new GameObject[Names.Length];
        var renderers = new MeshRenderer[Names.Length];

        for (int i = 0; i < Names.Length; i++)
        {
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.name == Names[i] && go.scene == scene)
                {
                    labels[i] = go;
                    break;
                }
            }

            if (labels[i] == null ||
                labels[i].GetComponent<TextMesh>() == null ||
                (renderers[i] = labels[i].GetComponent<MeshRenderer>()) == null)
            {
                Debug.LogError("Missing TextMesh or MeshRenderer: " + Names[i] +
                               ". No changes made.");
                return;
            }

            var oldMaterial = renderers[i].sharedMaterial;
            if (oldMaterial == null || oldMaterial.mainTexture == null)
            {
                Debug.LogError("Original font material/texture missing: " +
                               Names[i] + ". No changes made.");
                return;
            }
        }

        string folder = "Assets/Scenes/SCN16_STORE/Materials";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/Scenes/SCN16_STORE", "Materials");

        Undo.IncrementCurrentGroup();
        int group = Undo.GetCurrentGroup();
        Undo.SetCurrentGroupName("Apply Store Neon Header Materials");

        for (int i = 0; i < Names.Length; i++)
        {
            var oldMaterial = renderers[i].sharedMaterial;
            string materialPath = folder + "/" +
                Names[i].Replace(" wall header text", "").Replace(" ", "_") +
                "_Neon.mat";

            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, materialPath);
            }
            else
            {
                material.shader = shader;
            }

            material.SetTexture("_MainTex", oldMaterial.mainTexture);
            material.SetColor("_Color", Colors[i]);
            material.SetFloat("_Intensity", 3.5f);
            material.renderQueue = 3000;

            Undo.RecordObject(renderers[i], "Set neon header material");
            renderers[i].sharedMaterial = material;
            EditorUtility.SetDirty(material);
            EditorUtility.SetDirty(renderers[i]);
        }

        EditorSceneManager.MarkSceneDirty(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(scene);
        Undo.CollapseUndoOperations(group);

        Debug.Log("Applied six category neon materials. Existing labels and screens preserved.");
    }
}
