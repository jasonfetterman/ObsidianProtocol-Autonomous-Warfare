using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public float speedMultiplier = 0.05f;
    }

    public ParallaxLayer[] layers;
    public Camera mainCamera;
    private Vector3 lastCameraPosition;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        lastCameraPosition = mainCamera.transform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = mainCamera.transform.position - lastCameraPosition;

        foreach (var layer in layers)
        {
            if (layer.layerTransform != null)
            {
                layer.layerTransform.position += delta * layer.speedMultiplier;
            }
        }

        lastCameraPosition = mainCamera.transform.position;
    }
}
