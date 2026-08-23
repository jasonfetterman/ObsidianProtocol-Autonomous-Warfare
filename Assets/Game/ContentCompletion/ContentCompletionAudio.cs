using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionAudio
    {
        private readonly HashSet<string> audioAssets =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AudioAssetCount =>
            audioAssets.Count;

        public bool Complete =>
            AudioAssetCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            audioAssets.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterAudio(
            string audioId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(audioId))
            {
                return false;
            }

            return audioAssets.Add(
                audioId.Trim());
        }

        public bool ContainsAudio(
            string audioId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(audioId))
            {
                return false;
            }

            return audioAssets.Contains(
                audioId.Trim());
        }

        public IReadOnlyCollection<string>
            GetAudioAssets()
        {
            return audioAssets;
        }

        public void Reset()
        {
            audioAssets.Clear();
            Initialized = false;
        }
    }
}
