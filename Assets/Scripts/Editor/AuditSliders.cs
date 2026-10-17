#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class AuditSliders
{
    [MenuItem("Tools/Audit Sliders")]
    public static void Audit()
    {
        string[] sliders = {
            "Master_Volume", "Music_Volume", "Effects_Volume", "Voice_Volume",
            "Brightness", "Contrast", "HUD_Scale", "Mouse_Sensitivity"
        };

        Debug.Log("<color=cyan>=== AUDITING SLIDER CONTROLS ===</color>");

        // Search including inactive objects
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();

        foreach (string name in sliders)
        {
            Transform found = null;
            foreach (var t in allTransforms)
            {
                if (t.name == name && t.gameObject.scene.isLoaded)
                {
                    found = t;
                    break;
                }
            }

            if (found == null)
            {
                Debug.LogWarning($"[-] GameObject NOT found: {name}");
                continue;
            }

            Slider slider = found.GetComponent<Slider>();
            if (slider != null)
            {
                Debug.Log($"[+] {name}: Slider component present.");
            }
            else
            {
                Debug.LogError($"[!] {name}: MISSING Slider component! (Only has layout/images)");
            }
        }
    }
}
#endif
