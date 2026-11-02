using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class SCN17HologramTextFix
{
    const string ScenePath = "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    const string RootName = "SCN17 HOLOGRAPHIC INFO BOARDS";

    [MenuItem("Obsidian Protocol/SCN17/Fix Hologram Text")]
    public static void Fix()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            Debug.LogError("Open SCN17_PERSISTENT_MILITARY first.");
            return;
        }

        Transform root = GameObject.Find("FACILITY ARCHITECTURE")?
            .transform.Find(RootName);

        if (root == null)
        {
            Debug.LogError("Holographic info boards root not found.");
            return;
        }

        int fixedCount = 0;

        foreach (Transform board in root)
        {
            if (!board.name.StartsWith("HOLOGRAM - "))
                continue;

            Transform textTransform = board.Find("HOLOGRAM TEXT - " +
                board.name.Substring("HOLOGRAM - ".Length));

            if (textTransform == null)
                continue;

            TextMesh text = textTransform.GetComponent<TextMesh>();
            if (text == null)
                continue;

            textTransform.localPosition = new Vector3(-0.44f, 0.43f, -0.02f);
            textTransform.localRotation = Quaternion.identity;
            textTransform.localScale = Vector3.one;

            text.fontSize = 24;
            text.characterSize = 0.035f;
            text.anchor = TextAnchor.UpperLeft;
            text.alignment = TextAlignment.Left;
            text.lineSpacing = 0.85f;

            fixedCount++;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("[SCN17 HOLO] Text sizing adjusted on " + fixedCount + " boards.");
    }
}
