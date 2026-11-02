using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AuditCommanderStartFloor
{
    [MenuItem("Obsidian Protocol/Deployment/AUDIT COMMANDER START FLOOR")]
    public static void Audit()
    {
        Scene scene =
            EditorSceneManager.OpenScene(
                "Assets/Scenes/SCN12_DEPLOYMENT/SCN12_DEPLOYMENT.unity",
                OpenSceneMode.Single);

        GameObject player =
            GameObject.Find("[PLAYER] COMMANDER");

        if (player == null)
        {
            Debug.LogError("[START FLOOR] PLAYER NOT FOUND.");
            return;
        }

        Vector3 origin =
            player.transform.position +
            Vector3.up * 5f;

        Debug.Log(
            "[START FLOOR] PLAYER POSITION = " +
            player.transform.position);

        Ray ray =
            new Ray(origin, Vector3.down);

        RaycastHit[] hits =
            Physics.RaycastAll(
                ray,
                30f,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);

        Debug.Log(
            "[START FLOOR] COLLIDERS BELOW = " +
            hits.Length);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log(
                "[START FLOOR] HIT = " +
                hit.collider.name +
                " | POINT=" +
                hit.point +
                " | DISTANCE=" +
                hit.distance +
                " | LAYER=" +
                LayerMask.LayerToName(
                    hit.collider.gameObject.layer)
            );
        }

        Debug.Log("[START FLOOR] COMPLETE.");
    }
}
