using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Camera minimapCamera;
    public float moveScale = 1f;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;

            if (!RectTransformUtility.RectangleContainsScreenPoint(
                minimapCamera.GetComponentInChildren<RectTransform>(), mousePos))
                return;

            Ray ray = minimapCamera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 pos = hit.point;
                Vector3 camPos = mainCamera.transform.position;
                camPos.x = pos.x * moveScale;
                camPos.z = pos.z * moveScale;
                mainCamera.transform.position = camPos;
            }
        }
    }
}

