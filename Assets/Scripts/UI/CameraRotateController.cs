using UnityEngine;

public class CameraRotateController : MonoBehaviour
{
    public float rotateSpeed = 120f;
    public float tiltSpeed = 80f;
    public float minTilt = 10f;
    public float maxTilt = 70f;

    private float currentTilt = 45f;

    void Update()
    {
        HandleRotate();
        HandleTilt();
    }

    private void HandleRotate()
    {
        if (!Input.GetMouseButton(2))
            return;

        float delta = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, delta * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void HandleTilt()
    {
        if (!Input.GetMouseButton(2))
            return;

        float delta = -Input.GetAxis("Mouse Y");
        currentTilt = Mathf.Clamp(currentTilt + delta * tiltSpeed * Time.deltaTime, minTilt, maxTilt);

        Vector3 angles = transform.eulerAngles;
        angles.x = currentTilt;
        transform.eulerAngles = angles;
    }
}
