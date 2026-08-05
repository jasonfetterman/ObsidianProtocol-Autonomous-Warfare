using UnityEngine;

[RequireComponent(typeof(CombatAI))]
public class SquadAutoJoin : MonoBehaviour
{
    public float joinRadius = 10f;

    private CombatAI ai;

    private void Awake()
    {
        ai = GetComponent<CombatAI>();
    }

    private void Update()
    {
        SquadAI[] squads = Object.FindObjectsByType<SquadAI>(FindObjectsInactive.Exclude);

        foreach (var s in squads)
        {
            float dist = Vector3.Distance(transform.position, s.transform.position);

            if (dist < joinRadius && !s.members.Contains(ai))
                s.members.Add(ai);
        }
    }
}
