using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.0f;

    void Awake()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
