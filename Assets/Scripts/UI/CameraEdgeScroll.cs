using UnityEngine;

public class CameraEdgeScroll : MonoBehaviour
{
    public float scrollSpeed = 25f;
    public int edgeSize = 20;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.mousePosition.x <= edgeSize)
            move.x -= scrollSpeed * Time.deltaTime;

        if (Input.mousePosition.x >= Screen.width - edgeSize)
            move.x += scrollSpeed * Time.deltaTime;

        if (Input.mousePosition.y <= edgeSize)
            move.z -= scrollSpeed * Time.deltaTime;

        if (Input.mousePosition.y >= Screen.height - edgeSize)
            move.z += scrollSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);
    }
}
