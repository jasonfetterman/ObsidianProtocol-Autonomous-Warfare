
namespace ObsidianProtocol.Game.Command
{
    public enum IntentType
    {
        // Standard player commands
        Move = 0,
        Attack = 1,
        Defend = 2,
        Recon = 3,
        Reinforce = 9,
        Retreat = 8,
        Hold = 10,
        Capture = 11,

        // Legacy values retained for compatibility
        Flank = 4,
        Suppress = 5,
        Breach = 6,
        Pursue = 7
    }
}
