using UnityEngine;

public class UIAudio : MonoBehaviour
{
    public AudioClip clickClip;

    public void PlayClick()
    {
        if (clickClip != null)
            AudioManager.Instance.PlaySFX(clickClip, 0.8f);
    }
}
