using System;

namespace ObsidianProtocol.Game.Core
{
    [Serializable]
    public sealed class GameConfiguration
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 1f;
        public float EffectsVolume = 1f;
        public bool Fullscreen = true;
        public int TargetFrameRate = 60;
    }
}
