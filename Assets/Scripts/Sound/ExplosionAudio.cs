using UnityEngine;

public class ExplosionAudio : MonoBehaviour
{
    public AudioClip explosionClip;

    public void PlayExplosion()
    {
        if (explosionClip != null)
            AudioManager.Instance.PlaySFX(explosionClip, 1f);
    }
}
