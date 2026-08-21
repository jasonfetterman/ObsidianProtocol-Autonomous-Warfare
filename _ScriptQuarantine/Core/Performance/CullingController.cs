using UnityEngine;

public class CullingController
{
    private readonly Renderer[] renderers;
    private readonly Camera cam;
    private readonly Transform transform;

    public CullingController(Transform root)
    {
        transform = root;
        cam = Camera.main;
        renderers = root.GetComponentsInChildren<Renderer>();
    }

    public void Tick()
    {
        if (cam == null || transform == null) return;

        Vector3 screenPos = cam.WorldToViewportPoint(transform.position);

        bool visible =
            screenPos.x >= 0 && screenPos.x <= 1 &&
            screenPos.y >= 0 && screenPos.y <= 1 &&
            screenPos.z > 0;

        foreach (var r in renderers)
            r.enabled = visible;
    }
}
