using UnityEngine;

public class AudioFeedbackManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource uiAudioSource;

    [Header("Clips")]
    public AudioClip hoverClip;
    public AudioClip clickClip;
    public AudioClip confirmClip;
    public AudioClip cancelClip;
    public AudioClip transitionClip;

    [Header("Settings")]
    [Range(0f, 1f)] public float uiVolume = 0.75f;

    void Awake()
    {
        if (uiAudioSource == null)
        {
            uiAudioSource = gameObject.AddComponent<AudioSource>();
            uiAudioSource.playOnAwake = false;
            uiAudioSource.loop = false;
            uiAudioSource.volume = uiVolume;
        }
    }

    public void PlayHover() => PlayClip(hoverClip);
    public void PlayClick() => PlayClip(clickClip);
    public void PlayConfirm() => PlayClip(confirmClip);
    public void PlayCancel() => PlayClip(cancelClip);
    public void PlayTransition() => PlayClip(transitionClip);

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && uiAudioSource != null)
            uiAudioSource.PlayOneShot(clip, uiVolume);
    }
}
