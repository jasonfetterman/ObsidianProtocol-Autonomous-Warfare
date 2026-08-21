using UnityEngine;

public class SquadFormationDebug : MonoBehaviour
{
    private SquadAI squad;
    private SquadFormationLibrary library;

    public Color lineColor = Color.green;
    public float sphereSize = 0.25f;

    void Awake()
    {
        squad = ServiceLocator.Get<SquadAI>();
        library = ServiceLocator.Get<SquadFormationLibrary>();
    }

    void OnDrawGizmos()
    {
        if (squad == null || library == null)
            return;

        var type = squad.CurrentFormation;
        if (type == SquadAI.FormationType.None)
            return;

        var preset = library.GetPreset(type);
        if (preset == null)
            return;

        DrawFormation(preset);
    }

    private void DrawFormation(SquadFormationPreset preset)
    {
        Vector3 center = squad.SquadCenter;

        Gizmos.color = lineColor;

        switch (preset.type)
        {
            case SquadAI.FormationType.Line:
                DrawLine(preset, center);
                break;

            case SquadAI.FormationType.Wedge:
                DrawWedge(preset, center);
                break;

            case SquadAI.FormationType.Circle:
                DrawCircle(preset, center);
                break;

            case SquadAI.FormationType.None:
                DrawCustom(preset, center);
                break;
        }
    }

    private void DrawLine(SquadFormationPreset preset, Vector3 center)
    {
        for (int i = 0; i < squad.members.Count; i++)
        {
            Vector3 offset = new Vector3(i * preset.spacing, 0, 0);
            Gizmos.DrawSphere(center + offset, sphereSize);
        }
    }

    private void DrawWedge(SquadFormationPreset preset, Vector3 center)
    {
        for (int i = 0; i < squad.members.Count; i++)
        {
            int side = (i % 2 == 0) ? 1 : -1;
            int row = i / 2;

            Vector3 offset = new Vector3(side * preset.spacing * row, 0, row * preset.spacing);
            Gizmos.DrawSphere(center + offset, sphereSize);
        }
    }

    private void DrawCircle(SquadFormationPreset preset, Vector3 center)
    {
        int count = squad.members.Count;
        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = step * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * preset.radius;

            Gizmos.DrawSphere(center + offset, sphereSize);
        }
    }

    private void DrawCustom(SquadFormationPreset preset, Vector3 center)
    {
        for (int i = 0; i < preset.customOffsets.Count; i++)
        {
            Gizmos.DrawSphere(center + preset.customOffsets[i], sphereSize);
        }
    }
}
