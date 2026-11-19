using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class RestyleOperationsInPlace
{
    private const string ScenePath = "Assets/Scenes/SCN02_COMMAND_CENTER/COMMAND CENTER HUD/Command_Center.unity";

    [MenuItem("Obsidian Protocol/Command Center/RESTYLE OPERATIONS IN PLACE")]
    public static void Apply()
    {
        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        StyleObject(scene, "OPERATIONS_PANEL",
            new Color(.025f, .075f, .11f, .98f), true);

        StyleObject(scene, "FLEET_CONTENT",
            new Color(.025f, .075f, .11f, .98f), true);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[OPERATIONS] Existing panel surfaces restyled. Hierarchy and controls preserved.");
    }

    private static void StyleObject(Scene scene, string objectName, Color color, bool chamfer)
    {
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            Transform[] children = obj.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in children)
            {
                if (child.name != objectName) continue;

                Image image = child.GetComponent<Image>();
                if (image == null)
                {
                    Debug.LogWarning("[OPERATIONS] No Image component on " + objectName);
                    continue;
                }

                image.color = color;

                if (chamfer)
                {
                    image.sprite = GetChamferSprite();
                    image.type = Image.Type.Sliced;
                }

                Outline outline = child.GetComponent<Outline>();
                if (outline == null) outline = child.gameObject.AddComponent<Outline>();
                outline.effectColor = new Color(0f, .72f, 1f, 1f);
                outline.effectDistance = new Vector2(2f, 2f);

                EditorUtility.SetDirty(image);
                EditorUtility.SetDirty(outline);
                Debug.Log("[OPERATIONS] Styled " + objectName);
                return;
            }
        }

        Debug.LogWarning("[OPERATIONS] Could not find " + objectName);
    }

    private static Sprite chamferSprite;

    private static Sprite GetChamferSprite()
    {
        if (chamferSprite != null) return chamferSprite;

        const int size = 64;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        Color32 clear = new Color32(255, 255, 255, 0);
        Color32 solid = new Color32(255, 255, 255, 255);

        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            bool inside = x + y >= 13 &&
                (size - 1 - x) + y >= 13 &&
                x + (size - 1 - y) >= 13 &&
                (size - 1 - x) + (size - 1 - y) >= 13;
            texture.SetPixel(x, y, inside ? solid : clear);
        }

        texture.Apply();
        chamferSprite = Sprite.Create(texture, new Rect(0, 0, size, size),
            new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect,
            new Vector4(14, 14, 14, 14));
        chamferSprite.name = "OPAW_OPERATIONS_CHAMFER";
        return chamferSprite;
    }
}