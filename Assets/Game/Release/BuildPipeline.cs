using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Release
{
    public sealed class BuildPipeline
    {
        private readonly Dictionary<
            string,
            string> buildTargets =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TargetCount =>
            buildTargets.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            buildTargets.Clear();

            RegisterTarget("Development", "Development");
            RegisterTarget("InternalTest", "InternalTest");
            RegisterTarget("PC", "PC");
            RegisterTarget("VR", "VR");
            RegisterTarget("OfflineMultiplayer", "OfflineMultiplayer");
            RegisterTarget("OnlineServer", "OnlineServer");
            RegisterTarget("PublicTest", "PublicTest");
            RegisterTarget("Launch", "Launch");

            Initialized = true;

            return true;
        }

        public bool RegisterTarget(
            string targetId,
            string targetType)
        {
            if (string.IsNullOrWhiteSpace(targetId) ||
                string.IsNullOrWhiteSpace(targetType))
            {
                return false;
            }

            string id = targetId.Trim();

            if (buildTargets.ContainsKey(id))
            {
                return false;
            }

            buildTargets.Add(
                id,
                targetType.Trim());

            return true;
        }

        public bool HasTarget(
            string targetId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(targetId))
            {
                return false;
            }

            return buildTargets.ContainsKey(
                targetId.Trim());
        }

        public string GetTargetType(
            string targetId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(targetId))
            {
                return null;
            }

            buildTargets.TryGetValue(
                targetId.Trim(),
                out string targetType);

            return targetType;
        }

        public IReadOnlyDictionary<
            string,
            string>
            GetTargets()
        {
            return buildTargets;
        }

        public void Reset()
        {
            buildTargets.Clear();
            Initialized = false;
        }
    }
}
