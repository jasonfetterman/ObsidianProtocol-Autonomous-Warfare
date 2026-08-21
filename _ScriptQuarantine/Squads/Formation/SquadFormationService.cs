using UnityEngine;

public class SquadFormationService
{
    private SquadAI squad;
    private SquadFormationLibrary library;

    public SquadFormationService()
    {
        squad = ServiceLocator.Get<SquadAI>();
        library = ServiceLocator.Get<SquadFormationLibrary>();
    }

    // Called every frame by SquadFormationDriver
    public void TickFormation()
    {
        if (squad == null || squad.members.Count == 0)
            return;

        var type = squad.CurrentFormation;
        if (type == SquadAI.FormationType.None)
            return;

        var preset = library.GetPreset(type);
        if (preset == null)
            return;

        ApplyPreset(preset);
    }

    private void ApplyPreset(SquadFormationPreset preset)
    {
        Vector3 center = squad.SquadCenter;

        switch (preset.type)
        {
            case SquadAI.FormationType.Line:
                ApplyLine(preset, center);
                break;

            case SquadAI.FormationType.Wedge:
                ApplyWedge(preset, center);
                break;

            case SquadAI.FormationType.Circle:
                ApplyCircle(preset, center);
                break;

            case SquadAI.FormationType.None:
                ApplyCustom(preset, center);
                break;
        }
    }

    private void ApplyLine(SquadFormationPreset preset, Vector3 center)
    {
        for (int i = 0; i < squad.members.Count; i++)
        {
            Vector3 offset = new Vector3(i * preset.spacing, 0, 0);
            squad.members[i].MoveTowards(center + offset);
        }
    }

    private void ApplyWedge(SquadFormationPreset preset, Vector3 center)
    {
        for (int i = 0; i < squad.members.Count; i++)
        {
            int side = (i % 2 == 0) ? 1 : -1;
            int row = i / 2;

            Vector3 offset = new Vector3(side * preset.spacing * row, 0, row * preset.spacing);
            squad.members[i].MoveTowards(center + offset);
        }
    }

    private void ApplyCircle(SquadFormationPreset preset, Vector3 center)
    {
        int count = squad.members.Count;
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = step * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * preset.radius;

            squad.members[i].MoveTowards(center + offset);
        }
    }

    private void ApplyCustom(SquadFormationPreset preset, Vector3 center)
    {
        for (int i = 0; i < squad.members.Count; i++)
        {
            Vector3 offset = (i < preset.customOffsets.Count)
                ? preset.customOffsets[i]
                : Vector3.zero;

            squad.members[i].MoveTowards(center + offset);
        }
    }
}
