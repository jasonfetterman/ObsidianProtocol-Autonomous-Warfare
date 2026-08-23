using System.Collections;
using UnityEngine;

public class UITransitionSystem : MonoBehaviour
{
    public static UITransitionSystem Instance { get; private set; }

    [SerializeField] private float defaultDuration = 0.25f;

    private Coroutine activeTransition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void FadeIn(CanvasGroup target, float duration = -1f)
    {
        if (target == null)
            return;

        StartTransition(target, 1f, duration);
    }

    public void FadeOut(CanvasGroup target, float duration = -1f)
    {
        if (target == null)
            return;

        StartTransition(target, 0f, duration);
    }

    private void StartTransition(CanvasGroup target, float targetAlpha, float duration)
    {
        if (activeTransition != null)
            StopCoroutine(activeTransition);

        if (duration < 0f)
            duration = defaultDuration;

        activeTransition = StartCoroutine(FadeRoutine(target, targetAlpha, duration));
    }

    private IEnumerator FadeRoutine(
        CanvasGroup target,
        float targetAlpha,
        float duration)
    {
        float startAlpha = target.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float progress = duration > 0f
                ? elapsed / duration
                : 1f;

            target.alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                progress);

            yield return null;
        }

        target.alpha = targetAlpha;
        activeTransition = null;
    }
}
