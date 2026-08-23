using UnityEngine;
using UnityEngine.UI;

public class HUDDamageIndicators : MonoBehaviour
{
    [SerializeField] private GameObject indicatorVisual;
    [SerializeField] private Text indicatorText;

    public float DamageAmount { get; private set; }
    public bool IsActive { get; private set; }

    public void ShowDamage(float damage)
    {
        DamageAmount = Mathf.Max(0f, damage);
        IsActive = true;

        if (indicatorText != null)
            indicatorText.text = $"-{DamageAmount:0}";

        if (indicatorVisual != null)
            indicatorVisual.SetActive(true);
    }

    public void Hide()
    {
        IsActive = false;

        if (indicatorVisual != null)
            indicatorVisual.SetActive(false);
    }
}
