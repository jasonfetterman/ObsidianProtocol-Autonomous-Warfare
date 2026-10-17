#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Text;

public static class CopyHierarchy
{
    [MenuItem("Tools/Copy Scene Hierarchy")]
    public static void Copy()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Dump(root.transform, "", sb);
        }
        EditorGUIUtility.systemCopyBuffer = sb.ToString();
        Debug.Log("Hierarchy copied to clipboard!");
    }

    static void Dump(Transform t, string indent, StringBuilder sb)
    {
        sb.AppendLine(indent + t.name);
        foreach (Transform child in t)
            Dump(child, indent + "  ", sb);
    }
}
#endif