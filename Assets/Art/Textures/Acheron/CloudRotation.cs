using UnityEngine;

public class CloudRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(
            Vector3.up,
            0.8f * Time.deltaTime,
            Space.Self);
    }
}
