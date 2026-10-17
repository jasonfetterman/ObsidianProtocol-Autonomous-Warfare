using UnityEditor;
using UnityEngine;

public static class DoctrineHoloVerify
{
    [MenuItem("Obsidian Protocol/Deployment/Verify Doctrine Control Holos")]
    public static void Verify()
    {
        string[] names =
        {
            "[HOLO] DOCTRINE CONTROL - DOCTRINE",
            "[HOLO] DOCTRINE CONTROL - PARAMETERS"
        };

        foreach (string name in names)
        {
            GameObject go = GameObject.Find(name);

            if (go == null)
            {
                Debug.LogError("[VERIFY] NOT FOUND: " + name);
                continue;
            }

            Debug.Log(
                "[VERIFY] FOUND: " + name +
                " | WORLD=" + go.transform.position +
                " | ROTATION=" + go.transform.eulerAngles +
                " | LOCAL SCALE=" + go.transform.localScale +
                " | PARENT=" +
                (go.transform.parent == null ? "NONE" : go.transform.parent.name)
            );
        }
    }
}
