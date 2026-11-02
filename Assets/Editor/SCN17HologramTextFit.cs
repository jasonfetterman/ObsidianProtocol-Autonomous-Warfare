using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class SCN17HologramTextFit
{
    const string ScenePath = "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    const string RootName = "SCN17 HOLOGRAPHIC INFO BOARDS";

    [MenuItem("Obsidian Protocol/SCN17/Fit Text To Every Screen")]
    public static void Fix()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            Debug.LogError("Open SCN17_PERSISTENT_MILITARY first.");
            return;
        }

        var architecture = GameObject.Find("FACILITY ARCHITECTURE");
        var root = architecture != null ? architecture.transform.Find(RootName) : null;

        if (root == null)
        {
            Debug.LogError("Holographic info boards not found.");
            return;
        }

        int fixedCount = 0;

        foreach (Transform board in root)
        {
            if (!board.name.StartsWith("HOLOGRAM - "))
                continue;

            string screenName = board.name.Substring("HOLOGRAM - ".Length);
            Transform textTransform = board.Find("HOLOGRAM TEXT - " + screenName);
            if (textTransform == null) continue;

            var text = textTransform.GetComponent<TextMesh>();
            if (text == null) continue;

            Vector3 scale = board.localScale;
            float sx = Mathf.Abs(scale.x);
            float sy = Mathf.Abs(scale.y);
            if (sx < 0.001f || sy < 0.001f) continue;

            // Position is normalized to the board. Do not multiply by its size.
            textTransform.localPosition = new Vector3(-0.44f, 0.43f, -0.025f);
            textTransform.localRotation = Quaternion.identity;

            // Cancel parent scaling so glyphs don't become oversized.
            textTransform.localScale = new Vector3(
                1f / scale.x, 1f / scale.y, 1f);

            text.anchor = TextAnchor.UpperLeft;
            text.alignment = TextAlignment.Left;
            text.fontSize = 24;
            text.lineSpacing = 0.8f;

            string[] lines = text.text.Split('\n');
            int maxChars = 1;
            foreach (string line in lines)
                maxChars = Mathf.Max(maxChars, line.Length);

            // Estimate a glyph size that fits both the width and height.
            float worldWidth = sx * board.parent.lossyScale.x;
            float worldHeight = sy * board.parent.lossyScale.y;
            float byWidth = worldWidth * 0.82f / (maxChars * 0.62f);
            float byHeight = worldHeight * 0.82f /
                (Mathf.Max(1, lines.Length) * 1.25f);

            text.characterSize = Mathf.Clamp(
                Mathf.Min(byWidth, byHeight), 0.003f, 0.04f);

            fixedCount++;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        Debug.Log("[SCN17 HOLO] Corrected text transform and fit on " +
            fixedCount + " displays.");
    }
}
