using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ConvertStoreCatalogTo12Cards
{
    private const string ScenePath = "Assets/Scenes/SCN16_STORE/SCN16_STORE.unity";

    [MenuItem("Obsidian Protocol/Store/Convert Catalog to 12 Cards")]
    private static void Convert()
    {
        Scene active = SceneManager.GetActiveScene();

        if (active.isDirty && active.path != ScenePath)
        {
            Debug.LogError("Save or discard the current scene changes before converting the Store scene.");
            return;
        }

        if (active.path != ScenePath)
            active = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        Transform catalog = null;
        foreach (GameObject rootObject in active.GetRootGameObjects())
        {
            catalog = FindDeep(rootObject.transform, "ASSET_CATALOG");
            if (catalog != null)
                break;
        }

        if (catalog == null)
        {
            Debug.LogError("Conversion stopped: ASSET_CATALOG was not found.");
            return;
        }

        for (int i = 1; i <= 12; i++)
        {
            Transform card = catalog.Find("ASSET_CARD_" + i);
            if (card == null || card.GetComponent<RectTransform>() == null)
            {
                Debug.LogError("Conversion stopped: ASSET_CARD_" + i + " or its RectTransform is missing.");
                return;
            }
        }

        float left = 0.025f;
        float right = 0.975f;
        float gapX = 0.020f;
        float width = (right - left - 3f * gapX) / 4f;

        float[] rowMinY = { 0.59f, 0.327f, 0.064f };
        float[] rowMaxY = { 0.835f, 0.572f, 0.309f };

        for (int i = 1; i <= 12; i++)
        {
            Transform card = catalog.Find("ASSET_CARD_" + i);
            RectTransform rect = card.GetComponent<RectTransform>();

            Undo.RecordObject(rect, "Arrange Store Catalog Cards");

            int index = i - 1;
            int column = index % 4;
            int row = index / 4;

            float minX = left + column * (width + gapX);
            rect.anchorMin = new Vector2(minX, rowMinY[row]);
            rect.anchorMax = new Vector2(minX + width, rowMaxY[row]);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;

            EditorUtility.SetDirty(rect);
        }

        EditorSceneManager.MarkSceneDirty(active);

        Debug.Log("Store catalog arranged: 12 cards in 4 columns x 3 rows. Scene NOT saved.");
    }

    private static Transform FindDeep(Transform parent, string targetName)
    {
        if (parent.name == targetName)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindDeep(child, targetName);
            if (result != null)
                return result;
        }

        return null;
    }
}

