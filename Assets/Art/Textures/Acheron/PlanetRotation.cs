using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(
            Vector3.up,
            0.5f * Time.deltaTime,
            Space.Self);
    }
}
