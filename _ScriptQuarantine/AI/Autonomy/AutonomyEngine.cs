public class AutonomyEngine
{
    private readonly WorldState _worldState;
    private readonly WorldMemory _worldMemory;

    public AutonomyEngine(WorldState worldState, WorldMemory worldMemory)
    {
        _worldState = worldState;
        _worldMemory = worldMemory;
    }
}
