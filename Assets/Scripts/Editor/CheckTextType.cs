#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class CheckTextType
{
    [MenuItem("Tools/Check Text Type")]
    public static void Check()
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        Transform masterVal = null;
        foreach (var t in allTransforms)
        {
            if (t.name == "VALUE" && t.parent != null && t.parent.name == "Master_Volume")
            {
                masterVal = t;
                break;
            }
        }

        if (masterVal == null)
        {
            Debug.LogWarning("Could not find Master_Volume/VALUE");
            return;
        }

        if (masterVal.GetComponent<TMP_Text>() != null)
            Debug.Log("<color=green>[+] Text type is: TextMeshPro (TMP_Text)</color>");
        else if (masterVal.GetComponent<Text>() != null)
            Debug.Log("<color=green>[+] Text type is: Legacy UI Text</color>");
        else
            Debug.LogWarning("No known Text component on Master_Volume/VALUE");
    }
}
#endif
