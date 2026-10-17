using UnityEngine;
using System.Collections;

namespace ObsidianProtocol.UI
{
    public class UIAnimationSystem : MonoBehaviour
    {
        public static UIAnimationSystem Instance { get; private set; }

        [Header("Default Animation Settings")]
        [SerializeField] private float fadeDuration = 0.25f;
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        /// <summary>
        /// Fades a CanvasGroup in.
        /// </summary>
        public void FadeIn(CanvasGroup group)
        {
            if (group == null)
                return;

            StopAllCoroutines();
            StartCoroutine(FadeRoutine(group, 0f, 1f));
        }

        /// <summary>
        /// Fades a CanvasGroup out.
        /// </summary>
        public void FadeOut(CanvasGroup group)
        {
            if (group == null)
                return;

            StopAllCoroutines();
            StartCoroutine(FadeRoutine(group, 1f, 0f));
        }

        /// <summary>
        /// Generic fade routine.
        /// </summary>
        private IEnumerator FadeRoutine(CanvasGroup group, float start, float end)
        {
            float t = 0f;

            group.alpha = start;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / fadeDuration);
                group.alpha = Mathf.Lerp(start, end, fadeCurve.Evaluate(normalized));
                yield return null;
            }

            group.alpha = end;
        }

        /// <summary>
        /// Instantly sets alpha.
        /// </summary>
        public void SetAlpha(CanvasGroup group, float alpha)
        {
            if (group == null)
                return;

            group.alpha = Mathf.Clamp01(alpha);
        }
    }
}
