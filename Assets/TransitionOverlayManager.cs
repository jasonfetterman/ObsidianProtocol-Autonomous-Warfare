using UnityEngine;
using UnityEngine.UI;

public class TransitionOverlayManager : MonoBehaviour
{
    public Image overlayImage;
    public float fadeDuration = 1.0f;

    void Awake()
    {
        if (overlayImage != null)
            overlayImage.color = new Color(0f, 0f, 0f, 0f);
    }

    public void FadeIn() => StartCoroutine(Fade(1f));
    public void FadeOut() => StartCoroutine(Fade(0f));

    private System.Collections.IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = overlayImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            overlayImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        overlayImage.color = new Color(0f, 0f, 0f, targetAlpha);
    }
}