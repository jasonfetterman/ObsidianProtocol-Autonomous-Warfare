using System.Collections;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] private GameObject pauseMenuOverlay;
    [SerializeField] private RectTransform pauseMenuPanel;
    [SerializeField] private RectTransform pauseMenuTitle;

    [Header("Title Animation")]
    [SerializeField] private float titleOffset = 500f;
    [SerializeField] private float titleAnimationTime = 0.22f;

    [Header("Menu Panel Deployment")]
    [SerializeField] private RectTransform[] menuPanels;
    [SerializeField] private float sideOffset = 700f;
    [SerializeField] private float panelAnimationTime = 0.18f;
    [SerializeField] private float panelStagger = 0.025f;
    [SerializeField] private float startingScale = 0.96f;

    [Header("Fade")]
    [SerializeField] private float fadeTime = 0.16f;

    private Coroutine menuCoroutine;
    private bool menuOpen;

    private Vector2 titleFinalPosition;
    private Vector2[] finalPositions;
    private Vector3[] finalScales;

    private CanvasGroup titleCanvasGroup;
    private CanvasGroup[] panelCanvasGroups;

    private void Awake()
    {
        if (pauseMenuOverlay != null)
            pauseMenuOverlay.SetActive(false);

        CachePanelTransforms();
        CacheCanvasGroups();

        menuOpen = false;
    }

    private void CachePanelTransforms()
    {
        if (pauseMenuTitle != null)
            titleFinalPosition = pauseMenuTitle.anchoredPosition;

        if (menuPanels == null)
            return;

        finalPositions = new Vector2[menuPanels.Length];
        finalScales = new Vector3[menuPanels.Length];

        for (int i = 0; i < menuPanels.Length; i++)
        {
            if (menuPanels[i] == null)
                continue;

            finalPositions[i] = menuPanels[i].anchoredPosition;
            finalScales[i] = menuPanels[i].localScale;
        }
    }

    private void CacheCanvasGroups()
    {
        if (pauseMenuTitle != null)
        {
            titleCanvasGroup = pauseMenuTitle.GetComponent<CanvasGroup>();

            if (titleCanvasGroup == null)
                titleCanvasGroup = pauseMenuTitle.gameObject.AddComponent<CanvasGroup>();
        }

        if (menuPanels == null)
            return;

        panelCanvasGroups = new CanvasGroup[menuPanels.Length];

        for (int i = 0; i < menuPanels.Length; i++)
        {
            if (menuPanels[i] == null)
                continue;

            panelCanvasGroups[i] = menuPanels[i].GetComponent<CanvasGroup>();

            if (panelCanvasGroups[i] == null)
                panelCanvasGroups[i] = menuPanels[i].gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void ToggleMenu()
    {
        if (menuCoroutine != null)
            StopCoroutine(menuCoroutine);

        if (!menuOpen)
        {
            menuOpen = true;

            if (pauseMenuOverlay != null)
                pauseMenuOverlay.SetActive(true);

            menuCoroutine = StartCoroutine(OpenMenu());
        }
        else
        {
            menuOpen = false;
            menuCoroutine = StartCoroutine(CloseMenu());
        }
    }

    private IEnumerator OpenMenu()
    {
        PrepareTitle();
        PreparePanels();

        yield return StartCoroutine(DeployTitle());

        int pairCount = Mathf.CeilToInt(menuPanels.Length / 2f);

        for (int pair = 0; pair < pairCount; pair++)
        {
            int leftIndex = pair * 2;
            int rightIndex = leftIndex + 1;

            if (leftIndex < menuPanels.Length && menuPanels[leftIndex] != null)
                StartCoroutine(DeployPanel(menuPanels[leftIndex], finalPositions[leftIndex], finalScales[leftIndex], leftIndex));

            if (rightIndex < menuPanels.Length && menuPanels[rightIndex] != null)
                StartCoroutine(DeployPanel(menuPanels[rightIndex], finalPositions[rightIndex], finalScales[rightIndex], rightIndex));

            yield return new WaitForSecondsRealtime(panelStagger);
        }

        menuCoroutine = null;
    }

    private IEnumerator CloseMenu()
    {
        int pairCount = Mathf.CeilToInt(menuPanels.Length / 2f);

        for (int pair = pairCount - 1; pair >= 0; pair--)
        {
            int leftIndex = pair * 2;
            int rightIndex = leftIndex + 1;

            if (leftIndex < menuPanels.Length && menuPanels[leftIndex] != null)
                StartCoroutine(CollapsePanel(menuPanels[leftIndex], finalPositions[leftIndex], finalScales[leftIndex], leftIndex));

            if (rightIndex < menuPanels.Length && menuPanels[rightIndex] != null)
                StartCoroutine(CollapsePanel(menuPanels[rightIndex], finalPositions[rightIndex], finalScales[rightIndex], rightIndex));

            yield return new WaitForSecondsRealtime(panelStagger);
        }

        yield return new WaitForSecondsRealtime(panelAnimationTime);
        yield return StartCoroutine(CollapseTitle());

        if (!menuOpen && pauseMenuOverlay != null)
            pauseMenuOverlay.SetActive(false);

        menuCoroutine = null;
    }

    private void PrepareTitle()
    {
        if (pauseMenuTitle == null)
            return;

        Vector2 startPosition = titleFinalPosition;
        startPosition.y += titleOffset;

        pauseMenuTitle.anchoredPosition = startPosition;

        if (titleCanvasGroup != null)
            titleCanvasGroup.alpha = 0f;
    }

    private IEnumerator DeployTitle()
    {
        if (pauseMenuTitle == null)
            yield break;

        Vector2 startPosition = pauseMenuTitle.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < titleAnimationTime)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / titleAnimationTime);
            float positionT = 1f - Mathf.Pow(1f - t, 5f);
            float fadeT = Mathf.Clamp01(elapsed / fadeTime);

            pauseMenuTitle.anchoredPosition = Vector2.Lerp(startPosition, titleFinalPosition, positionT);

            if (titleCanvasGroup != null)
                titleCanvasGroup.alpha = fadeT;

            yield return null;
        }

        pauseMenuTitle.anchoredPosition = titleFinalPosition;

        if (titleCanvasGroup != null)
            titleCanvasGroup.alpha = 1f;
    }

    private IEnumerator CollapseTitle()
    {
        if (pauseMenuTitle == null)
            yield break;

        Vector2 startPosition = pauseMenuTitle.anchoredPosition;
        Vector2 endPosition = titleFinalPosition;
        endPosition.y += titleOffset;

        float elapsed = 0f;

        while (elapsed < titleAnimationTime)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / titleAnimationTime);
            float positionT = t * t;
            float fadeT = Mathf.Clamp01(elapsed / fadeTime);

            pauseMenuTitle.anchoredPosition = Vector2.Lerp(startPosition, endPosition, positionT);

            if (titleCanvasGroup != null)
                titleCanvasGroup.alpha = 1f - fadeT;

            yield return null;
        }

        pauseMenuTitle.anchoredPosition = endPosition;

        if (titleCanvasGroup != null)
            titleCanvasGroup.alpha = 0f;
    }

    private void PreparePanels()
    {
        if (menuPanels == null)
            return;

        for (int i = 0; i < menuPanels.Length; i++)
        {
            if (menuPanels[i] == null)
                continue;

            Vector2 startPosition = finalPositions[i];

            if (i % 2 == 0)
                startPosition.x -= sideOffset;
            else
                startPosition.x += sideOffset;

            menuPanels[i].anchoredPosition = startPosition;
            menuPanels[i].localScale = finalScales[i] * startingScale;

            if (panelCanvasGroups != null && panelCanvasGroups[i] != null)
                panelCanvasGroups[i].alpha = 0f;
        }
    }

    private IEnumerator DeployPanel(RectTransform panel, Vector2 targetPosition, Vector3 targetScale, int index)
    {
        Vector2 startPosition = panel.anchoredPosition;
        Vector3 startScale = panel.localScale;

        CanvasGroup canvasGroup = panelCanvasGroups[index];

        float elapsed = 0f;

        while (elapsed < panelAnimationTime)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / panelAnimationTime);
            float positionT = 1f - Mathf.Pow(1f - t, 5f);
            float scaleT = 1f - Mathf.Pow(1f - t, 4f);
            float fadeT = Mathf.Clamp01(elapsed / fadeTime);

            panel.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, positionT);
            panel.localScale = Vector3.Lerp(startScale, targetScale, scaleT);

            if (canvasGroup != null)
                canvasGroup.alpha = fadeT;

            yield return null;
        }

        panel.anchoredPosition = targetPosition;
        panel.localScale = targetScale;

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    private IEnumerator CollapsePanel(RectTransform panel, Vector2 targetPosition, Vector3 targetScale, int index)
    {
        Vector2 startPosition = panel.anchoredPosition;
        Vector2 endPosition = targetPosition;

        if (index % 2 == 0)
            endPosition.x -= sideOffset;
        else
            endPosition.x += sideOffset;

        Vector3 startScale = panel.localScale;
        Vector3 endScale = targetScale * startingScale;

        CanvasGroup canvasGroup = panelCanvasGroups[index];

        float startAlpha = canvasGroup != null ? canvasGroup.alpha : 1f;
        float elapsed = 0f;

        while (elapsed < panelAnimationTime)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / panelAnimationTime);
            float positionT = t * t;
            float fadeT = Mathf.Clamp01(elapsed / fadeTime);

            panel.anchoredPosition = Vector2.Lerp(startPosition, endPosition, positionT);
            panel.localScale = Vector3.Lerp(startScale, endScale, positionT);

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, fadeT);

            yield return null;
        }

        panel.anchoredPosition = endPosition;
        panel.localScale = endScale;

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}