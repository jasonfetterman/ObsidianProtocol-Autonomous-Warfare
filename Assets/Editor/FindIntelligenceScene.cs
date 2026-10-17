using UnityEditor;
using UnityEngine;

public static class FindIntelligenceScene
{
    [MenuItem("Obsidian Protocol/Build/FIND INTELLIGENCE SCENE")]
    public static void Find()
    {
        string[] guids =
            AssetDatabase.FindAssets(
                "t:Scene Intellegence Intelligence"
            );

        Debug.Log(
            "[INTELLIGENCE SEARCH] RESULTS = " +
            guids.Length
        );

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            Debug.Log(
                "[INTELLIGENCE SEARCH] " +
                path
            );
        }
    }
}
