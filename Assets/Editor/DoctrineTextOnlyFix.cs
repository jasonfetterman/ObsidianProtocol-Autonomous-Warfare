using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class DoctrineTextOnlyFix
{
    [MenuItem("Obsidian Protocol/Deployment/Fix Doctrine Text Only")]
    public static void Fix()
    {
        const string scenePath =
            "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity";

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single
        );

        string[] roots =
        {
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            "[HOLO] DOCTRINE CONTROL - PARAMETERS"
        };

        int converted = 0;

        foreach (string rootName in roots)
        {
            GameObject root = GameObject.Find(rootName);

            if (root == null)
            {
                Debug.LogError(
                    "[DOCTRINE TEXT ONLY] MISSING ROOT: " + rootName
                );
                continue;
            }

            TMP_Text[] tmpTexts =
                root.GetComponentsInChildren<TMP_Text>(true);

            foreach (TMP_Text tmp in tmpTexts)
            {
                GameObject go = tmp.gameObject;

                // Capture TEXT properties only.
                string value = tmp.text;
                Color color = tmp.color;
                float fontSize = tmp.fontSize;
                TextAlignmentOptions alignment = tmp.alignment;
                bool raycastTarget = tmp.raycastTarget;

                // IMPORTANT:
                // Do NOT touch RectTransform position, rotation,
                // scale, anchors, pivots, width, or height.

                Object.DestroyImmediate(tmp);

                Text text = go.AddComponent<Text>();

                text.text = value;
                text.color = color;
                text.fontSize = Mathf.RoundToInt(fontSize);
                text.raycastTarget = raycastTarget;

                switch (alignment)
                {
                    case TextAlignmentOptions.TopLeft:
                    case TextAlignmentOptions.MidlineLeft:
                    case TextAlignmentOptions.BottomLeft:
                        text.alignment = TextAnchor.MiddleLeft;
                        break;

                    case TextAlignmentOptions.Top:
                    case TextAlignmentOptions.Center:
                    case TextAlignmentOptions.Bottom:
                        text.alignment = TextAnchor.MiddleCenter;
                        break;

                    case TextAlignmentOptions.TopRight:
                    case TextAlignmentOptions.MidlineRight:
                    case TextAlignmentOptions.BottomRight:
                        text.alignment = TextAnchor.MiddleRight;
                        break;

                    default:
                        text.alignment = TextAnchor.MiddleLeft;
                        break;
                }

                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;

                converted++;

                Debug.Log(
                    "[DOCTRINE TEXT ONLY] Converted " +
                    rootName + " / " + go.name
                );
            }
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[DOCTRINE TEXT ONLY] COMPLETE. Converted " +
            converted +
            " TMP components. NO TRANSFORMS MODIFIED."
        );
    }
}
