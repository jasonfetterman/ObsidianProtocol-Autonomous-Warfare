using UnityEngine;
using System.Collections.Generic;

public class SquadFormation
{
    private SquadAI squad;

    public SquadFormation()
    {
        squad = ServiceLocator.Get<SquadAI>();
    }

    public void ApplyFormation()
    {
        if (squad == null || squad.members.Count == 0)
            return;

        switch (squad.CurrentFormation)
        {
            case SquadAI.FormationType.Line:
                ApplyLine();
                break;

            case SquadAI.FormationType.Wedge:
                ApplyWedge();
                break;

            case SquadAI.FormationType.Circle:
                ApplyCircle();
                break;

            case SquadAI.FormationType.None:
            default:
                // No formation applied
                break;
        }
    }

    private void ApplyLine()
    {
        Vector3 center = squad.SquadCenter;
        float spacing = 2f;

        for (int i = 0; i < squad.members.Count; i++)
        {
            var m = squad.members[i];
            if (m == null) continue;

            Vector3 offset = new Vector3(i * spacing, 0, 0);
            m.MoveTowards(center + offset);
        }
    }

    private void ApplyWedge()
    {
        Vector3 center = squad.SquadCenter;
        float spacing = 2f;

        for (int i = 0; i < squad.members.Count; i++)
        {
            var m = squad.members[i];
            if (m == null) continue;

            int side = (i % 2 == 0) ? 1 : -1;
            int row = i / 2;

            Vector3 offset = new Vector3(side * spacing * row, 0, row * spacing);
            m.MoveTowards(center + offset);
        }
    }

    private void ApplyCircle()
    {
        Vector3 center = squad.SquadCenter;
        float radius = 3f;

        int count = squad.members.Count;
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            var m = squad.members[i];
            if (m == null) continue;

            float angle = step * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            m.MoveTowards(center + offset);
        }
    }
}
