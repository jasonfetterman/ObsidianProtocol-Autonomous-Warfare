using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquadFormationPreset
{
    public SquadAI.FormationType type;

    // Common parameters
    public float spacing = 2f;
    public float radius = 4f;

    // Custom offsets for FormationType.None
    public List<Vector3> customOffsets = new();

    public SquadFormationPreset(SquadAI.FormationType type)
    {
        this.type = type;
    }
}
