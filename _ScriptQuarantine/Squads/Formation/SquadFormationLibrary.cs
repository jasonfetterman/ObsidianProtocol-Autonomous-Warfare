using System.Collections.Generic;

public class SquadFormationLibrary
{
    private readonly Dictionary<SquadAI.FormationType, SquadFormationPreset> presets =
        new Dictionary<SquadAI.FormationType, SquadFormationPreset>();

    public void AddPreset(SquadFormationPreset preset)
    {
        if (preset == null)
            return;

        presets[preset.type] = preset;
    }

    public SquadFormationPreset GetPreset(SquadAI.FormationType type)
    {
        if (presets.TryGetValue(type, out var preset))
            return preset;

        return null;
    }

    public bool HasPreset(SquadAI.FormationType type)
    {
        return presets.ContainsKey(type);
    }
}
