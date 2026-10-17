using UnityEditor;
using UnityEngine;

public static class DeploymentPlayerHeightAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Player Height")]
    public static void Audit()
    {
        GameObject player = GameObject.Find("[PLAYER] COMMANDER");

        if (player == null)
        {
            Debug.LogError("[PLAYER HEIGHT] [PLAYER] COMMANDER NOT FOUND");
            return;
        }

        Debug.Log(
            "[PLAYER HEIGHT] PLAYER" +
            " | Position=" + player.transform.position +
            " | LocalPosition=" + player.transform.localPosition +
            " | Rotation=" + player.transform.eulerAngles +
            " | LocalScale=" + player.transform.localScale
        );

        Component[] components = player.GetComponentsInChildren<Component>(true);

        foreach (Component component in components)
        {
            if (component == null)
                continue;

            Debug.Log(
                "[PLAYER HEIGHT] COMPONENT" +
                " | Object=" + component.gameObject.name +
                " | Type=" + component.GetType().FullName
            );
        }
    }
}
