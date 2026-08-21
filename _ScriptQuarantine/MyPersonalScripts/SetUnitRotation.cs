using UnityEngine;

public class SetUnitRotation : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
}