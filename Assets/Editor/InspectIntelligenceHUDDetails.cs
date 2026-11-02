using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InspectIntelligenceHUDDetails
{
    private const string ScenePath =
        "Assets/Scenes/SCN-18 INTELLIGENCE/Intellegence.unity";

    [MenuItem("Obsidian Protocol/INTELLIGENCE/Inspect HUD Details")]
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
                "[INTELLIGENCE] HUD NOT FOUND.");
            return;
        }

        Debug.Log("===== INTELLIGENCE HUD DETAILS =====");

        foreach (Transform panel in hud.transform)
        {
            RectTransform rect =
                panel.GetComponent<RectTransform>();

            if (rect == null)
                continue;

            Debug.Log(
                $"PANEL: {panel.name} | " +
                $"ACTIVE={panel.gameObject.activeSelf} | " +
                $"SIZE={rect.sizeDelta} | " +
                $"POS={rect.anchoredPosition}");

            foreach (Transform child in panel)
            {
                RectTransform childRect =
                    child.GetComponent<RectTransform>();

                if (childRect == null)
                    continue;

                Debug.Log(
                    $"    CHILD: {child.name} | " +
                    $"ACTIVE={child.gameObject.activeSelf} | " +
                    $"SIZE={childRect.sizeDelta} | " +
                    $"POS={childRect.anchoredPosition}");
            }
        }

        Debug.Log("===== END INTELLIGENCE HUD DETAILS =====");
    }
}
