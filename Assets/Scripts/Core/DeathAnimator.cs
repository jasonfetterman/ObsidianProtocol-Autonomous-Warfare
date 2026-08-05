using UnityEngine;

public class DeathAnimator : MonoBehaviour
{
    public Animator anim;
    public string deathTrigger = "Die";

    bool dead = false;

    void Awake()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void PlayDeath()
    {
        if (dead) return;
        dead = true;

        if (anim != null)
            anim.SetTrigger(deathTrigger);

        // disable movement + combat
        DisableAll();
    }

    void DisableAll()
    {
        MonoBehaviour[] comps = GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            if (c != this)
                c.enabled = false;
        }
    }
}
