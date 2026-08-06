using UnityEngine;

public class CameraPanController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float zoomSpeed = 120f;
    public float minZoom = 10f;
    public float maxZoom = 80f;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(h, 0f, v) * panSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f)
            return;

        float newSize = cam.orthographicSize - scroll * zoomSpeed * Time.deltaTime;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }
}
