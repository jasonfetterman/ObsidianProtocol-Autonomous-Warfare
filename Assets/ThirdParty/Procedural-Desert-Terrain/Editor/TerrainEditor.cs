using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralTerrain), true)]
public class TerrainEditor : Editor
{
    ProceduralTerrain proceduralTerrain;
    Editor editor;

    private void OnEnable()
    {
        proceduralTerrain = (ProceduralTerrain)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawSettingsEditor(proceduralTerrain.gizmoSettings, proceduralTerrain.BuildMesh,  ref proceduralTerrain.gizmoSettingsFoldout, ref editor);
        DrawSettingsEditor(proceduralTerrain.terrainProperties, proceduralTerrain.BuildMesh, ref proceduralTerrain.terrainPropertiesFoldout, ref editor);
    }

    void DrawSettingsEditor(Object settings, System.Action action ,ref bool shouldShow, ref Editor editor)
    {
        if (settings != null)
        {
            // 1. 'ref' keyword inssures that the value of the caller is changed when 'shouldShow' is changed
            // 2. Makes it possible to open / close the settings window from the inspector
            shouldShow = EditorGUILayout.InspectorTitlebar(shouldShow, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (shouldShow)
                {
                    // create boilerplate to populate settings in the inspector window 
                    //Editor editor = CreateEditor(settings);
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed && proceduralTerrain.terrainProperties.autoRefresh)
                    {
                        action?.Invoke();
                    }
                }
            }
        }
    }
}
