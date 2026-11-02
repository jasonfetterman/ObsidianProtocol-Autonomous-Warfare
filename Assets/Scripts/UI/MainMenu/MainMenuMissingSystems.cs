using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ObsidianProtocol.UI.MainMenu
{
    public sealed class MainMenuMissingSystems : MonoBehaviour
    {
        public static MainMenuMissingSystems Instance { get; private set; }

        [Header("Popups")]
        [SerializeField] private GameObject patchNotesPopup;
        [SerializeField] private GameObject networkErrorPopup;
        [SerializeField] private GameObject profileLoginPopup;

        [Header("System Overlays")]
        [SerializeField] private GameObject transitionOverlay;
        [SerializeField] private GameObject loadingScreen;

        [Header("Loading")]
        [SerializeField] private Slider loadingProgress;
        [SerializeField] private Text loadingStatus;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;

        private CanvasGroup transitionGroup;
        private CanvasGroup loadingGroup;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (transitionOverlay != null)
                transitionGroup = transitionOverlay.GetComponent<CanvasGroup>();

            if (loadingScreen != null)
                loadingGroup = loadingScreen.GetComponent<CanvasGroup>();

            CloseAllPopups();

            if (transitionGroup != null)
            {
                transitionGroup.alpha = 0f;
                transitionOverlay.SetActive(false);
            }

            if (loadingGroup != null)
            {
                loadingGroup.alpha = 0f;
                loadingScreen.SetActive(false);
            }
        }

        public void OpenPatchNotes()
        {
            OpenPopup(patchNotesPopup);
        }

        public void OpenNetworkError()
        {
            OpenPopup(networkErrorPopup);
        }

        public void OpenProfileLogin()
        {
            OpenPopup(profileLoginPopup);
        }

        public void ClosePatchNotes()
        {
            ClosePopup(patchNotesPopup);
        }

        public void CloseNetworkError()
        {
            ClosePopup(networkErrorPopup);
        }

        public void CloseProfileLogin()
        {
            ClosePopup(profileLoginPopup);
        }

        private void OpenPopup(GameObject popup)
        {
            if (popup == null)
                return;

            CloseAllPopups();

            popup.SetActive(true);

            CanvasGroup group =
                popup.GetComponent<CanvasGroup>();

            if (group != null)
            {
                group.alpha = 0f;
                StartCoroutine(FadeGroup(group, 0f, 1f, 0.18f));
            }

            PlayClick();
        }

        private void ClosePopup(GameObject popup)
        {
            if (popup == null)
                return;

            popup.SetActive(false);
        }

        public void CloseAllPopups()
        {
            if (patchNotesPopup != null)
                patchNotesPopup.SetActive(false);

            if (networkErrorPopup != null)
                networkErrorPopup.SetActive(false);

            if (profileLoginPopup != null)
                profileLoginPopup.SetActive(false);
        }

        public void ShowTransition()
        {
            if (transitionOverlay == null)
                return;

            transitionOverlay.SetActive(true);

            if (transitionGroup == null)
                transitionGroup =
                    transitionOverlay.GetComponent<CanvasGroup>();

            if (transitionGroup != null)
                StartCoroutine(FadeGroup(
                    transitionGroup,
                    0f,
                    1f,
                    0.25f));
        }

        public void HideTransition()
        {
            if (transitionOverlay == null)
                return;

            if (transitionGroup == null)
                transitionGroup =
                    transitionOverlay.GetComponent<CanvasGroup>();

            if (transitionGroup != null)
                StartCoroutine(
                    FadeOutTransition());
            else
                transitionOverlay.SetActive(false);
        }

        private IEnumerator FadeOutTransition()
        {
            yield return FadeGroup(
                transitionGroup,
                transitionGroup.alpha,
                0f,
                0.25f);

            transitionOverlay.SetActive(false);
        }

        public void ShowLoading()
        {
            if (loadingScreen == null)
                return;

            loadingScreen.SetActive(true);

            if (loadingGroup == null)
                loadingGroup =
                    loadingScreen.GetComponent<CanvasGroup>();

            if (loadingProgress != null)
                loadingProgress.value = 0f;

            if (loadingStatus != null)
                loadingStatus.text =
                    "INITIALIZING COMMAND SYSTEMS...";

            if (loadingGroup != null)
            {
                loadingGroup.alpha = 0f;

                StartCoroutine(
                    FadeGroup(
                        loadingGroup,
                        0f,
                        1f,
                        0.20f));
            }
        }

        public void SetLoadingProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);

            if (loadingProgress != null)
                loadingProgress.value = progress;

            if (loadingStatus != null)
            {
                int percent =
                    Mathf.RoundToInt(progress * 100f);

                loadingStatus.text =
                    "LOADING COMMAND SYSTEMS  " +
                    percent +
                    "%";
            }
        }

        public void CompleteLoading()
        {
            SetLoadingProgress(1f);

            if (loadingStatus != null)
                loadingStatus.text =
                    "COMMAND SYSTEMS READY";

            StartCoroutine(
                HideLoadingAfterDelay());
        }

        private IEnumerator HideLoadingAfterDelay()
        {
            yield return new WaitForSeconds(0.45f);

            if (loadingGroup != null)
            {
                yield return FadeGroup(
                    loadingGroup,
                    loadingGroup.alpha,
                    0f,
                    0.25f);
            }

            if (loadingScreen != null)
                loadingScreen.SetActive(false);
        }

        public void PlayClick()
        {
            if (audioSource == null)
                return;

            audioSource.Play();
        }

        public void PlayHover()
        {
            if (audioSource == null)
                return;

            audioSource.Play();
        }

        private IEnumerator FadeGroup(
            CanvasGroup group,
            float start,
            float end,
            float duration)
        {
            if (group == null)
                yield break;

            float elapsed = 0f;

            group.alpha = start;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;

                float t =
                    Mathf.Clamp01(
                        elapsed / duration);

                t = t * t * (3f - 2f * t);

                group.alpha =
                    Mathf.Lerp(
                        start,
                        end,
                        t);

                yield return null;
            }

            group.alpha = end;
        }
    }

    public sealed class MainMenuTooltipSystem :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        [TextArea]
        public string tooltipText;

        public GameObject tooltipObject;

        public void OnPointerEnter(
            PointerEventData eventData)
        {
            if (tooltipObject == null)
                return;

            tooltipObject.SetActive(true);
        }

        public void OnPointerExit(
            PointerEventData eventData)
        {
            if (tooltipObject == null)
                return;

            tooltipObject.SetActive(false);
        }
    }

    public sealed class MainMenuAudioFeedbackSystem :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerClickHandler
    {
        public AudioSource audioSource;

        public void OnPointerEnter(
            PointerEventData eventData)
        {
            if (audioSource != null)
                audioSource.Play();
        }

        public void OnPointerClick(
            PointerEventData eventData)
        {
            if (audioSource != null)
                audioSource.Play();
        }
    }
}
