#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class AuditNavButtons
{
    [MenuItem("Tools/Audit Nav Buttons")]
    public static void Audit()
    {
        string[] buttons = { "GAMEPLAY", "CONTROLS", "AUDIO", "VIDEO", "VR", "HUD", "ACCESSIBILITY", "NETWORK", "ACCOUNT", "BACK", "CLOSE_CUSTOMIZATION" };
        Debug.Log("<color=cyan>=== AUDITING NAVIGATION BUTTONS ===</color>");

        foreach (string name in buttons)
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
            {
                Debug.LogWarning($"[-] GameObject NOT found: {name}");
                continue;
            }

            Button btn = go.GetComponent<Button>();
            if (btn != null)
            {
                Debug.Log($"[+] {name}: Button component present.");
            }
            else
            {
                Debug.LogError($"[!] {name}: MISSING Button component! (Has GameObject, but no Button)");
            }
        }
    }
}
#endif
