using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionUI
    {
        private readonly HashSet<string> screens =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ScreenCount =>
            screens.Count;

        public bool Complete =>
            ScreenCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            screens.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterScreen(
            string screenId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(screenId))
            {
                return false;
            }

            return screens.Add(
                screenId.Trim());
        }

        public bool ContainsScreen(
            string screenId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(screenId))
            {
                return false;
            }

            return screens.Contains(
                screenId.Trim());
        }

        public IReadOnlyCollection<string>
            GetScreens()
        {
            return screens;
        }

        public void Reset()
        {
            screens.Clear();
            Initialized = false;
        }
    }
}
