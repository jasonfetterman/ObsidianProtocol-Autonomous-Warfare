using UnityEngine;
using System.Collections;

public class PauseMenuAnimation : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.25f;

    [SerializeField]
    private AnimationCurve animationCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Target")]
    [SerializeField] private RectTransform target;

    private Vector3 originalScale;
    private Coroutine animationRoutine;

    private void Awake()
    {
        if (target == null)
            target = transform as RectTransform;

        if (target != null)
            originalScale = target.localScale;
    }

    public void PlayOpenAnimation()
    {
        StartAnimation(true);
    }

    public void PlayCloseAnimation()
    {
        StartAnimation(false);
    }

    private void StartAnimation(bool opening)
    {
        if (target == null)
            return;

        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        animationRoutine =
            StartCoroutine(Animate(opening));
    }

    private IEnumerator Animate(bool opening)
    {
        float elapsed = 0f;

        Vector3 hiddenScale =
            originalScale * 0.92f;

        Vector3 start =
            opening ? hiddenScale : originalScale;

        Vector3 end =
            opening ? originalScale : hiddenScale;

        target.localScale = start;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float normalized =
                Mathf.Clamp01(
                    elapsed / animationDuration);

            float curveValue =
                animationCurve.Evaluate(normalized);

            target.localScale =
                Vector3.LerpUnclamped(
                    start,
                    end,
                    curveValue);

            yield return null;
        }

        target.localScale = end;
        animationRoutine = null;
    }
}