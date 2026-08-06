using UnityEngine;

public class SquadMember : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector3? moveTarget;

    void Update()
    {
        if (moveTarget.HasValue)
            TickMovement();
    }

    public void MoveTowards(Vector3 target)
    {
        moveTarget = target;
    }

    private void TickMovement()
    {
        Vector3 target = moveTarget.Value;
        Vector3 pos = transform.position;

        // Move toward target
        Vector3 dir = (target - pos);
        float dist = dir.magnitude;

        if (dist < 0.1f)
        {
            moveTarget = null;
            return;
        }

        dir.Normalize();
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}

