using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class InspectSCN15VisualLayers
{
    [MenuItem("Obsidian Protocol/Debug/Inspect SCN15 Visual Layers")]
    public static void Inspect()
    {
        Debug.Log("===== SCN15 VISUAL LAYERS =====");

        GameObject hud = GameObject.Find("EXIT CONFIRMATION HUD");

        if (hud == null)
        {
            Debug.LogError("EXIT CONFIRMATION HUD NOT FOUND.");
            return;
        }

        Canvas canvas = hud.GetComponent<Canvas>();

        for (int i = 0; i < hud.transform.childCount; i++)
        {
            Transform child = hud.transform.GetChild(i);

            Image image = child.GetComponent<Image>();

            string imageInfo = "NO IMAGE";

            if (image != null)
            {
                imageInfo =
                    "IMAGE enabled=" + image.enabled +
                    " raycastTarget=" + image.raycastTarget +
                    " color=" + image.color;
            }

            Debug.Log(
                "CHILD[" + i + "]: " +
                child.name +
                " | active=" + child.gameObject.activeSelf +
                " | siblingIndex=" + child.GetSiblingIndex() +
                " | " + imageInfo
            );
        }

        Debug.Log("===== END SCN15 VISUAL LAYERS =====");
    }
}
