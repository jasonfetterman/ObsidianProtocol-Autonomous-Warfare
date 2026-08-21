using UnityEngine;

public class CameraRotateController : MonoBehaviour
{
    public float rotateSpeed = 120f;
    public float tiltSpeed = 80f;
    public float minTilt = -89f;
    public float maxTilt = 89f;

    private float currentTilt = 45f;

    private void Start()
    {
        currentTilt = transform.eulerAngles.x;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(2))
            return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the CameraRig horizontally.
        transform.Rotate(
            Vector3.up,
            mouseX * rotateSpeed * Time.deltaTime,
            Space.World
        );

        // Tilt the CameraRig vertically.
        currentTilt -= mouseY * tiltSpeed * Time.deltaTime;
        currentTilt = Mathf.Clamp(currentTilt, minTilt, maxTilt);

        Vector3 angles = transform.eulerAngles;
        angles.x = currentTilt;
        transform.eulerAngles = angles;
    }
}