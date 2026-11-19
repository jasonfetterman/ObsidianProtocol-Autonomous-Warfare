using UnityEngine;

public class UIAudioFeedbackSystem : MonoBehaviour
{
    public static UIAudioFeedbackSystem Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }

    public void Stop()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
    }
}
