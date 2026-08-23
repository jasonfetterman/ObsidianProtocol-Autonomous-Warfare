using UnityEngine;

public class UIAnimationSystem : MonoBehaviour
{
    public static UIAnimationSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(GameObject target)
    {
        if (target == null)
            return;

        target.SetActive(true);
    }

    public void Hide(GameObject target)
    {
        if (target == null)
            return;

        target.SetActive(false);
    }
}
