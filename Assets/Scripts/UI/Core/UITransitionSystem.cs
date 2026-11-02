using UnityEngine;
using System.Collections;

namespace ObsidianProtocol.UI
{
    public class UITransitionSystem : MonoBehaviour
    {
        public static UITransitionSystem Instance { get; private set; }

        [Header("Transition Settings")]
        [SerializeField] private float transitionDuration = 0.35f;
        [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

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
        /// Plays a transition between two CanvasGroups.
        /// </summary>
        public void Transition(CanvasGroup fromGroup, CanvasGroup toGroup)
        {
            StopAllCoroutines();
            StartCoroutine(TransitionRoutine(fromGroup, toGroup));
        }

        /// <summary>
        /// Plays a transition into a single CanvasGroup (fade in).
        /// </summary>
        public void TransitionIn(CanvasGroup group)
        {
            if (group == null)
                return;

            StopAllCoroutines();
            StartCoroutine(FadeRoutine(group, 0f, 1f));
        }

        /// <summary>
        /// Plays a transition out of a single CanvasGroup (fade out).
        /// </summary>
        public void TransitionOut(CanvasGroup group)
        {
            if (group == null)
                return;

            StopAllCoroutines();
            StartCoroutine(FadeRoutine(group, 1f, 0f));
        }

        private IEnumerator TransitionRoutine(CanvasGroup fromGroup, CanvasGroup toGroup)
        {
            float t = 0f;

            if (fromGroup != null)
                fromGroup.alpha = 1f;

            if (toGroup != null)
                toGroup.alpha = 0f;

            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / transitionDuration);
                float curveValue = transitionCurve.Evaluate(normalized);

                if (fromGroup != null)
                    fromGroup.alpha = Mathf.Lerp(1f, 0f, curveValue);

                if (toGroup != null)
                    toGroup.alpha = Mathf.Lerp(0f, 1f, curveValue);

                yield return null;
            }

            if (fromGroup != null)
                fromGroup.alpha = 0f;

            if (toGroup != null)
                toGroup.alpha = 1f;
        }

        private IEnumerator FadeRoutine(CanvasGroup group, float start, float end)
        {
            float t = 0f;

            group.alpha = start;

            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / transitionDuration);
                group.alpha = Mathf.Lerp(start, end, transitionCurve.Evaluate(normalized));
                yield return null;
            }

            group.alpha = end;
        }
    }
}
