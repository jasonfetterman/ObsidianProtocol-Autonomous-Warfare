using UnityEngine;

public static class SquadFormationInstaller
{
    public static void Install()
    {
        // Create and register formation library
        var library = new SquadFormationLibrary();
        ServiceLocator.Register(library);

        // Create and register formation service
        var service = new SquadFormationService();
        ServiceLocator.Register(service);

        // ------------------------------------
        // DEFAULT PRESETS
        // ------------------------------------

        // Line
        var line = new SquadFormationPreset(SquadAI.FormationType.Line)
        {
            spacing = 2f
        };
        library.AddPreset(line);

        // Wedge
        var wedge = new SquadFormationPreset(SquadAI.FormationType.Wedge)
        {
            spacing = 2f
        };
        library.AddPreset(wedge);

        // Circle
        var circle = new SquadFormationPreset(SquadAI.FormationType.Circle)
        {
            radius = 4f
        };
        library.AddPreset(circle);

        // Custom formation placeholder (None)
        var custom = new SquadFormationPreset(SquadAI.FormationType.None);
        library.AddPreset(custom);
    }
}
