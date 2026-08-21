using System.Collections.Generic;
using UnityEngine;

public class SquadAI
{
    public enum FormationType
    {
        None,
        Line,
        Wedge,
        Circle
    }

    public List<SquadMember> members = new();
    public FormationType CurrentFormation { get; private set; } = FormationType.None;

    private SquadMemory memory;

    public SquadAI()
    {
        memory = ServiceLocator.Get<SquadMemory>();
    }

    public void AddMember(SquadMember member)
    {
        if (member == null)
            return;

        if (!members.Contains(member))
            members.Add(member);
    }

    public void RemoveMember(SquadMember member)
    {
        if (member == null)
            return;

        if (members.Contains(member))
            members.Remove(member);
    }

    public Vector3 SquadCenter
    {
        get
        {
            if (members.Count == 0)
                return Vector3.zero;

            Vector3 sum = Vector3.zero;
            int count = 0;

            for (int i = 0; i < members.Count; i++)
            {
                var m = members[i];
                if (m == null) continue;

                sum += m.transform.position;
                count++;
            }

            return count > 0 ? sum / count : Vector3.zero;
        }
    }

    public void SetFormation(FormationType type)
    {
        CurrentFormation = type;
        memory.SetFormation(type);
    }

    public void ClearFormation()
    {
        CurrentFormation = FormationType.None;
        memory.SetFormation(FormationType.None);
    }

    // Called by SquadController to move the entire squad
    public void MoveSquad(Vector3 target)
    {
        memory.SetMoveTarget(target);

        for (int i = 0; i < members.Count; i++)
        {
            var m = members[i];
            if (m == null) continue;

            m.MoveTowards(target);
        }
    }
}
