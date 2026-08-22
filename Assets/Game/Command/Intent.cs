using UnityEngine;

namespace ObsidianProtocol.Game.Command
{
    public sealed class Intent
    {
        public IntentType Type { get; }
        public Vector3 Position { get; }
        public float Priority { get; }

        public Intent(IntentType type, Vector3 position, float priority = 1f)
        {
            Type = type;
            Position = position;
            Priority = Mathf.Max(0f, priority);
        }
    }
}
