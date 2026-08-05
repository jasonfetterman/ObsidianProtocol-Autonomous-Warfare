using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health target;
    public Image fillImage;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public Camera mainCamera;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (target == null || fillImage == null) return;

        float ratio = target.currentHealth / target.baseHealth;
        fillImage.fillAmount = Mathf.Clamp01(ratio);

        Vector3 worldPos = target.transform.position + offset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        transform.position = screenPos;
    }
}
