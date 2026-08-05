using UnityEngine;

public class RagdollActivator : MonoBehaviour
{
    Rigidbody[] bodies;
    Collider[] colliders;
    Animator anim;

    void Awake()
    {
        bodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        anim = GetComponent<Animator>();

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        if (anim != null)
            anim.enabled = false;

        foreach (var rb in bodies)
            rb.isKinematic = false;

        foreach (var c in colliders)
            c.enabled = true;
    }

    void DisableRagdoll()
    {
        foreach (var rb in bodies)
            rb.isKinematic = true;

        foreach (var c in colliders)
            c.enabled = false;

        if (anim != null)
            anim.enabled = true;
    }
}
