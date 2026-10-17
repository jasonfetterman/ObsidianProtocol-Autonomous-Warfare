using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectIntelligenceHUDLayout
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Inspect HUD Layout")]
    public static void Inspect()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                ScenePath,
                OpenSceneMode.Single);

        GameObject hud =
            GameObject.Find("INTELLIGENCE HUD");

        if (hud == null)
        {
            Debug.LogError(
                "[INTELLIGENCE] INTELLIGENCE HUD not found.");
            return;
        }

        Debug.Log("===== INTELLIGENCE HUD LAYOUT =====");

        foreach (Transform child in hud.transform)
        {
            RectTransform rect =
                child.GetComponent<RectTransform>();

            if (rect == null)
                continue;

            Debug.Log(
                $"[HUD PANEL] {child.name} | " +
                $"ACTIVE={child.gameObject.activeSelf} | " +
                $"POS={rect.anchoredPosition} | " +
                $"SIZE={rect.sizeDelta} | " +
                $"ANCHOR MIN={rect.anchorMin} | " +
                $"ANCHOR MAX={rect.anchorMax}");
        }

        Debug.Log("===== END INTELLIGENCE HUD LAYOUT =====");
    }
}
