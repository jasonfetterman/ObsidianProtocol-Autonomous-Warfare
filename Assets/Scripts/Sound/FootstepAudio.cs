using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioClip[] footstepClips;
    public float stepInterval = 0.5f;

    UnitMover mover;
    float nextStepTime;

    void Awake()
    {
        mover = GetComponent<UnitMover>();
    }

    void Update()
    {
        if (mover == null || footstepClips.Length == 0) return;

        if (mover.IsMoving() && Time.time >= nextStepTime)
        {
            nextStepTime = Time.time + stepInterval;

            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            AudioManager.Instance.PlaySFX(clip, 0.6f);
        }
    }
}
