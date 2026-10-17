using UnityEditor;
using UnityEngine;

public static class DeploymentBudgetNearbyScreensAudit
{
    [MenuItem("Obsidian Protocol/Deployment/Audit Budget Nearby Screens")]
    public static void Audit()
    {
        Vector3 station = Vector3.zero;

        var all = Object.FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach (var t in all)
        {
            if (t.name != "[SCREEN] LEFT OPERATOR" &&
                t.name != "[SCREEN] RIGHT OPERATOR")
                continue;

            float distance = Vector3.Distance(t.position, station);

            if (distance <= 35f)
            {
                Debug.Log(
                    $"SCREEN | {t.name} | " +
                    $"WorldPos={t.position} | " +
                    $"WorldRot={t.eulerAngles} | " +
                    $"DistanceFromBudgetStation={distance:F2}");
            }
        }
    }
}
