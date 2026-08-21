using UnityEngine;

public class RTSCameraStartPosition : MonoBehaviour
{
    [SerializeField] private Transform startingUnit;
    [SerializeField] private float heightOffset = 30f;

    private void Start()
    {
        if (startingUnit == null)
            return;

        Vector3 position = startingUnit.position;
        position.y += heightOffset;

        transform.position = position;
    }
}