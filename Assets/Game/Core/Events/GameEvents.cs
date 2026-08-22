using System;

namespace ObsidianProtocol.Game.Core
{
    public static class GameEvents
    {
        public static event Action GameStarted;
        public static event Action GamePaused;
        public static event Action GameResumed;

        public static void RaiseGameStarted()
        {
            GameStarted?.Invoke();
        }

        public static void RaiseGamePaused()
        {
            GamePaused?.Invoke();
        }

        public static void RaiseGameResumed()
        {
            GameResumed?.Invoke();
        }
    }
}
