using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class MakeGalaxyPlanetsRound
{
    [MenuItem("Obsidian Protocol/Galaxy/MAKE ALL PLANETS ROUND")]
    public static void Execute()
    {
        Sprite circle =
            AssetDatabase.GetBuiltinExtraResource<Sprite>(
                "UI/Skin/Knob.psd");

        if (circle == null)
        {
            Debug.LogError("Could not find built-in circle sprite.");
            return;
        }

        foreach (Button button in Object.FindObjectsOfType<Button>(true))
        {
            string n = button.gameObject.name.ToUpper();

            if (n == "AUREX")
                continue;

            Image img = button.GetComponent<Image>();

            if (img == null)
                continue;

            img.sprite = circle;
            img.type = Image.Type.Simple;
            img.preserveAspect = true;

            RectTransform rt =
                button.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(24f, 24f);
        }

        Debug.Log("All planet markers converted to round markers.");
    }
}