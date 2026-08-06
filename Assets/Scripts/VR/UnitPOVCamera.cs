using UnityEngine;

public class UnitPOVCamera : MonoBehaviour
{
    public Transform mountPoint;
    public float fov = 90f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = fov;
    }

    void LateUpdate()
    {
        if (mountPoint == null)
            return;

        transform.position = mountPoint.position;
        transform.rotation = mountPoint.rotation;
    }
}

