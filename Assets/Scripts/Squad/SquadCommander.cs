using UnityEngine;

public class SquadCommander : MonoBehaviour
{
    public SquadAI squad;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject enemy = hit.collider.GetComponentInParent<EnemyTag>()?.gameObject;

                if (enemy != null)
                    IssueAttack(enemy);
                else
                    IssueMove(hit.point);
            }
        }
    }

    void IssueAttack(GameObject target)
    {
        foreach (var m in squad.members)
        {
            if (m == null) continue;
            m.SetForcedTarget(target);
        }
    }

    void IssueMove(Vector3 pos)
    {
        foreach (var m in squad.members)
        {
            UnitMover mover = m.GetComponent<UnitMover>();
            if (mover != null)
                mover.MoveTo(pos);
        }
    }
}
