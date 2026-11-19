using UnityEngine;

public class AcheronCloudDrift : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1.5f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
