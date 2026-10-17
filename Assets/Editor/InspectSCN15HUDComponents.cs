using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class InspectSCN15HUDComponents
{
    [MenuItem("Obsidian Protocol/Debug/Inspect SCN15 HUD Components")]
    public static void Inspect()
    {
        GameObject hud = GameObject.Find("EXIT CONFIRMATION HUD");

        if (hud == null)
        {
            Debug.LogError("EXIT CONFIRMATION HUD NOT FOUND.");
            return;
        }

        Debug.Log("===== SCN15 HUD COMPONENTS =====");

        Component[] components =
            hud.GetComponents<Component>();

        for (int i = 0; i < components.Length; i++)
        {
            Component c = components[i];

            if (c == null)
                Debug.Log("COMPONENT[" + i + "]: MISSING SCRIPT");
            else
                Debug.Log(
                    "COMPONENT[" + i + "]: " +
                    c.GetType().FullName
                );
        }

        Debug.Log("===== END SCN15 HUD COMPONENTS =====");
    }
}
