using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BattlefieldDeploymentRuntimeAudit
{
    [MenuItem("Obsidian Protocol/Battlefield/AUDIT DEPLOYED UNIT RUNTIME")]
    public static void Audit()
    {
        const string scenePath =
            "Assets/Scenes/SCN03 _BATTLEFIELD/[HUD] BATTLEFIELD HUD/Battlefield_HUD.unity";

        EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject context =
            GameObject.Find("[SYSTEM] BATTLEFIELD DEPLOYMENT CONTEXT");

        GameObject area =
            GameObject.Find("[SYSTEM] PLAYER DEPLOYMENT AREA");

        GameObject materializer =
            GameObject.Find("[SYSTEM] BATTLEFIELD DEPLOYMENT");

        GameObject runtime =
            GameObject.Find("[RUNTIME] DEPLOYED UNITS");

        Debug.Log(
            "[BATTLEFIELD RUNTIME AUDIT] " +
            "CONTEXT=" + (context != null) +
            " | PLAYER_AREA=" + (area != null) +
            " | MATERIALIZER=" + (materializer != null) +
            " | RUNTIME_UNITS=" + (runtime != null));

        if (runtime != null)
        {
            Debug.Log(
                "[BATTLEFIELD RUNTIME AUDIT] " +
                "RUNTIME CHILD COUNT=" +
                runtime.transform.childCount);

            for (int i = 0; i < runtime.transform.childCount; i++)
            {
                Transform child = runtime.transform.GetChild(i);

                Debug.Log(
                    "[BATTLEFIELD RUNTIME AUDIT] UNIT " +
                    i +
                    " = " +
                    child.name +
                    " | POS=" +
                    child.position);
            }
        }

        Debug.Log("[BATTLEFIELD RUNTIME AUDIT] COMPLETE.");
    }
}
