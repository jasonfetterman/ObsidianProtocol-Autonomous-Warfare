using UnityEngine;

public class AcheronPlanetRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = -2f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
