using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireViperAirPlacement
{
    private const float FlightAltitude = 30f;

    [MenuItem("Obsidian Protocol/Battlefield/PLACE IRONCLAD VIPER IN AIR")]
    public static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != "Assets/Scenes/SCN03_BATTLEFIELD_AAA/SCN03_BATTLEFIELD_AAA.unity")
        {
            Debug.LogError("[VIPER AIR] Open the Blackspire Expanse scene first.");
            return;
        }

        GameObject groundRoot = GameObject.Find("BLACKSPIRE_FIELD_UNITS");
        GameObject terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (groundRoot == null || terrain == null)
        {
            Debug.LogError("[VIPER AIR] Ground group or terrain not found.");
            return;
        }

        Transform viper = groundRoot.transform.Find("ENEMY_IRONCLAD_VIPER_01");

        if (viper == null)
        {
            GameObject existingViper = GameObject.Find("ENEMY_IRONCLAD_VIPER_01");

            if (existingViper != null)
                viper = existingViper.transform;
        }

        if (viper == null)
        {
            Debug.LogError("[VIPER AIR] Ironclad Viper not found.");
            return;
        }

        MeshCollider terrainCollider = terrain.GetComponent<MeshCollider>();

        if (terrainCollider == null || !terrainCollider.enabled)
        {
            Debug.LogError("[VIPER AIR] Terrain collider unavailable.");
            return;
        }

        GameObject airRoot = GameObject.Find("BLACKSPIRE_AIR_UNITS");

        if (airRoot == null)
        {
            airRoot = new GameObject("BLACKSPIRE_AIR_UNITS");
            Undo.RegisterCreatedObjectUndo(airRoot, "Create Blackspire Air Unit Group");
        }

        airRoot.transform.position = Vector3.zero;
        airRoot.transform.rotation = Quaternion.identity;
        airRoot.transform.localScale = Vector3.one;

        Undo.SetTransformParent(viper, airRoot.transform, "Move Viper to Air Units");

        Renderer[] renderers = viper.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogError("[VIPER AIR] No renderers found.");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
            bounds.Encapsulate(renderer.bounds);

        Vector3 position = viper.position;

        Ray ray = new Ray(
            new Vector3(position.x, 500f, position.z),
            Vector3.down);

        RaycastHit hit;

        if (!terrainCollider.Raycast(ray, out hit, 1000f))
        {
            Debug.LogError("[VIPER AIR] Could not find terrain below Viper.");
            return;
        }

        position.y += hit.point.y + FlightAltitude - bounds.min.y;
        viper.position = position;

        EditorUtility.SetDirty(viper);
        EditorUtility.SetDirty(airRoot);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[VIPER AIR] COMPLETE | Ironclad Viper moved to air group" +
            " | Altitude above terrain: " + FlightAltitude + "m" +
            " | Scale: " + viper.localScale +
            " | Position: " + viper.position);
    }
}
