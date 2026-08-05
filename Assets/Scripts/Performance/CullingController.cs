using UnityEngine;

public class CullingController : MonoBehaviour
{
    Renderer[] renderers;
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        Vector3 screenPos = cam.WorldToViewportPoint(transform.position);

        bool visible =
            screenPos.x >= 0 && screenPos.x <= 1 &&
            screenPos.y >= 0 && screenPos.y <= 1 &&
            screenPos.z > 0;

        foreach (var r in renderers)
            r.enabled = visible;
    }
}
