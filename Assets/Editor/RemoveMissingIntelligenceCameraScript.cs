using UnityEditor;
using UnityEngine;

public static class RemoveMissingIntelligenceCameraScript
{
    [MenuItem("Obsidian Protocol/Fix/Remove Missing Camera Script")]
    public static void Fix()
    {
        GameObject camera = GameObject.Find("INTELLIGENCE CAMERA");

        if (camera == null)
        {
            Debug.LogError("[INTELLIGENCE] INTELLIGENCE CAMERA not found.");
            return;
        }

        SerializedObject so = new SerializedObject(camera);
        SerializedProperty components = so.FindProperty("m_Component");

        for (int i = components.arraySize - 1; i >= 0; i--)
        {
            SerializedProperty component = components.GetArrayElementAtIndex(i);
            SerializedProperty componentRef = component.FindPropertyRelative("component");

            if (componentRef != null && componentRef.objectReferenceValue == null)
            {
                components.DeleteArrayElementAtIndex(i);
                Debug.Log("[INTELLIGENCE] Removed missing component from INTELLIGENCE CAMERA.");
            }
        }

        so.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(camera);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(camera.scene);
        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(camera.scene);

        Debug.Log("[INTELLIGENCE] INTELLIGENCE CAMERA repaired.");
    }
}
