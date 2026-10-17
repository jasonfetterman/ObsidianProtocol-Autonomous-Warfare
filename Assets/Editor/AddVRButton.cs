using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public static class AddVRButton
{
    [MenuItem("Obsidian Protocol/Add VR Button")]
    public static void Add()
    {
        string scenePath = null;

        foreach (string guid in AssetDatabase.FindAssets("t:Scene"))
        {
            string p = AssetDatabase.GUIDToAssetPath(guid);

            if (p.Contains("SCN-01") && p.Contains("MAIN MENU") && p.EndsWith(".unity"))
            {
                scenePath = p;
                break;
            }
        }

        if (scenePath == null)
        {
            Debug.LogError("[VR] Main Menu scene not found.");
            return;
        }

        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        Button commandButton = null;

        foreach (var b in Object.FindObjectsByType<Button>(FindObjectsSortMode.None))
        {
            var tmp = b.GetComponentInChildren<TMP_Text>(true);
            if (tmp != null && tmp.text.Trim().Equals("COMMAND", System.StringComparison.OrdinalIgnoreCase))
            {
                commandButton = b;
                break;
            }

            var legacy = b.GetComponentInChildren<UnityEngine.UI.Text>(true);
            if (legacy != null && legacy.text.Trim().Equals("COMMAND", System.StringComparison.OrdinalIgnoreCase))
            {
                commandButton = b;
                break;
            }
        }

        if (commandButton == null)
        {
            Debug.LogError("[VR] COMMAND button not found.");
            return;
        }

        var oldVR = GameObject.Find("VR OPERATOR");
        if (oldVR != null)
            Object.DestroyImmediate(oldVR);

        GameObject vr = Object.Instantiate(commandButton.gameObject, commandButton.transform.parent);
        vr.name = "VR OPERATOR";

        var vrButton = vr.GetComponent<Button>();

        // Remove EVERYTHING inherited from COMMAND.
        while (vrButton.onClick.GetPersistentEventCount() > 0)
            UnityEventTools.RemovePersistentListener(vrButton.onClick, 0);

        vrButton.onClick.RemoveAllListeners();

        var script = vr.GetComponent<VRSceneButton>();
        if (script == null)
            script = vr.AddComponent<VRSceneButton>();

        UnityEventTools.AddPersistentListener(vrButton.onClick, script.OpenVR);

        var tmpText = vr.GetComponentInChildren<TMP_Text>(true);
        if (tmpText != null)
            tmpText.text = "VR";

        var legacyText = vr.GetComponentInChildren<UnityEngine.UI.Text>(true);
        if (legacyText != null)
            legacyText.text = "VR";

        var rt = vr.GetComponent<RectTransform>();
        var commandRT = commandButton.GetComponent<RectTransform>();

        rt.anchoredPosition = commandRT.anchoredPosition;
        rt.anchoredPosition -= new Vector2(0, commandRT.rect.height + 10f);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = vr;

        Debug.Log("[VR] VR button created. COMMAND action removed. VR -> VR_Command.");
    }
}
