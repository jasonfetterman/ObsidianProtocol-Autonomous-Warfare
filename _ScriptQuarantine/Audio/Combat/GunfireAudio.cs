using UnityEngine;

public class GunfireAudio : MonoBehaviour
{
    public AudioClip gunshotClip;

    Shooter shooter;

    void Awake()
    {
        shooter = GetComponent<Shooter>();
    }

    public void PlayGunshot()
    {
        if (gunshotClip != null)
            AudioManager.Instance.PlaySFX(gunshotClip, 1f);
    }
}
