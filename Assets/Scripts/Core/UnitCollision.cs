using UnityEngine;

public class UnitCollision : MonoBehaviour
{
    public float radius = 0.5f;
    public float pushForce = 5f;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (var h in hits)
        {
            if (h.gameObject == gameObject) continue;

            Vector3 dir = transform.position - h.transform.position;
            float dist = dir.magnitude;

            if (dist < radius && dist > 0.01f)
            {
                Vector3 push = dir.normalized * pushForce * Time.deltaTime;
                transform.position += push;
            }
        }
    }
}
