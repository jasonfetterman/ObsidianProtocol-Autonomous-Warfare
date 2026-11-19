using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class BuildMinimapInPanel
{
    [MenuItem("Obsidian Protocol/Battlefield/Build Minimap In Existing Panel")]
    public static void Build()
    {
        var panel = GameObject.Find("Minimap_Panel");
        if (panel == null) { Debug.LogError("Minimap_Panel not found."); return; }

        var texture = AssetDatabase.LoadAssetAtPath<RenderTexture>(
            "Assets/UI/HUD/Battlefield/BattleHUD/Battle_Minimap_RT.renderTexture");
        if (texture == null) { Debug.LogError("Battle_Minimap_RT not found."); return; }

        var existing = panel.transform.Find("Minimap_Display");
        GameObject display;

        if (existing != null) display = existing.gameObject;
        else
        {
            display = new GameObject("Minimap_Display",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(RawImage));
            display.transform.SetParent(panel.transform, false);
        }

        var rect = display.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;

        var image = display.GetComponent<RawImage>();
        image.texture = texture;
        image.color = Color.white;
        image.raycastTarget = false;

        var camObject = GameObject.Find("Minimap_Camera");
        if (camObject == null) { Debug.LogError("Minimap_Camera not found."); return; }
        var cam = camObject.GetComponent<Camera>();
        cam.enabled = true;
        cam.orthographic = true;
        cam.orthographicSize = 512f;
        var terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");
        if (terrain == null) { Debug.LogError("Blackspire terrain not found."); return; }
        var terrainRenderer = terrain.GetComponent<Renderer>();
        cam.transform.position = new Vector3(terrainRenderer.bounds.center.x, 250f, terrainRenderer.bounds.center.z);
        cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        cam.targetTexture = texture;
        cam.cullingMask = ~0;
        EditorUtility.SetDirty(camObject);
        EditorUtility.SetDirty(panel);
        EditorSceneManager.MarkSceneDirty(panel.scene);
        Debug.Log("Minimap display built inside existing Minimap_Panel.");
    }
}



