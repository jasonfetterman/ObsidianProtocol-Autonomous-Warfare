using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BlackspireGroundUnitScaleFixer
{
    [MenuItem("Obsidian Protocol/Battlefield/SCALE BLACKSPIRE UNITS TO 300")]
    public static void Apply()
    {
        GameObject root = GameObject.Find("BLACKSPIRE_FIELD_UNITS");
        GameObject terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

        if (root == null || terrain == null)
        {
            Debug.LogError("[BLACKSPIRE SCALE] Unit group or terrain not found.");
            return;
        }

        MeshCollider terrainCollider = terrain.GetComponent<MeshCollider>();

        if (terrainCollider == null || !terrainCollider.enabled)
        {
            Debug.LogError("[BLACKSPIRE SCALE] Terrain collider unavailable.");
            return;
        }

        int updated = 0;

        foreach (Transform unit in root.transform)
        {
            Undo.RecordObject(unit, "Scale Blackspire Unit");

            unit.localScale *= 3f;

            Renderer[] renderers = unit.GetComponentsInChildren<Renderer>(true);

            if (renderers.Length == 0)
            {
                Debug.LogWarning("[BLACKSPIRE SCALE] No renderers: " + unit.name);
                continue;
            }

            Bounds bounds = renderers[0].bounds;

            foreach (Renderer renderer in renderers)
                bounds.Encapsulate(renderer.bounds);

            Vector3 position = unit.position;

            Ray ray = new Ray(
                new Vector3(position.x, 500f, position.z),
                Vector3.down);

            RaycastHit hit;

            if (terrainCollider.Raycast(ray, out hit, 1000f))
            {
                position.y += hit.point.y - bounds.min.y;
                unit.position = position;
            }
            else
            {
                Debug.LogWarning("[BLACKSPIRE SCALE] Ground adjustment failed: " + unit.name);
            }

            EditorUtility.SetDirty(unit);
            updated++;

            Debug.Log("[BLACKSPIRE SCALE] " + unit.name +
                " | New scale: " + unit.localScale +
                " | Position: " + unit.position);
        }

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        Debug.Log("[BLACKSPIRE SCALE] COMPLETE | Updated: " + updated);
    }
}
