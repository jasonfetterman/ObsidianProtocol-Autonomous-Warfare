using UnityEngine;

public class AmbientZone : MonoBehaviour
{
    public AudioClip ambientClip;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            AudioManager.Instance.PlayAmbient(ambientClip, 0.7f);
        }
    }
}
