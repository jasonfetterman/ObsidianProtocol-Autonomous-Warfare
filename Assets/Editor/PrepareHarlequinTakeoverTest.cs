using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PrepareHarlequinTakeoverTest
{
    [MenuItem("Obsidian Protocol/Unit Takeover/Prepare Temporary Test Position")]
    private static void Run()
    {
        GameObject player = GameObject.Find("BLACKSPIRE WALK PLAYER");
        if (!player)
        {
            Debug.LogError("[HARLEQUIN TEST] Player not found in the open scene.");
            return;
        }

        Transform mech = null;
        foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (var smr in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if (smr.sharedMesh != null &&
                    AssetDatabase.GetAssetPath(smr.sharedMesh).Contains("autorig-7e7ecd87-v4-glb"))
                {
                    mech = smr.transform;
                    while (mech.parent != null && mech.parent != root.transform)
                        mech = mech.parent;
                    break;
                }
            }
            if (mech) break;
        }

        if (!mech)
        {
            Debug.LogError("[HARLEQUIN TEST] Existing mech not found in the active scene.");
            return;
        }

        Vector3 target = mech.position - mech.forward * 4f;
        Ray ray = new Ray(new Vector3(target.x, mech.position.y + 100f, target.z), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
            target.y = hit.point.y + 1.1f;
        else
            target.y = mech.position.y;

        Undo.RecordObject(player.transform, "Prepare Temporary Harlequin Test");
        player.transform.position = target;
        SceneView.RepaintAll();

        Debug.Log("[HARLEQUIN TEST] Player temporarily positioned at " + target +
                  ". Do not save the scene; enter Play Mode to test F takeover.", player);
    }
}
