using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class ModDefinition
    {
        public string ModId { get; }

        public string ModName { get; }

        public string Version { get; }

        public bool Loaded { get; private set; }

        public ModDefinition(
            string modId,
            string modName,
            string version)
        {
            ModId =
                modId ?? string.Empty;

            ModName =
                modName ?? string.Empty;

            Version =
                version ?? string.Empty;

            Loaded = false;
        }

        public bool Load()
        {
            if (Loaded)
            {
                return false;
            }

            Loaded = true;

            return true;
        }

        public bool Unload()
        {
            if (!Loaded)
            {
                return false;
            }

            Loaded = false;

            return true;
        }
    }

    public sealed class ModLoadingFramework
    {
        private readonly Dictionary<
            string,
            ModDefinition> mods =
            new Dictionary<
                string,
                ModDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ModCount =>
            mods.Count;

        public int LoadedModCount
        {
            get
            {
                int count = 0;

                foreach (ModDefinition mod
                         in mods.Values)
                {
                    if (mod.Loaded)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            mods.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterMod(
            string modId,
            string modName,
            string version)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(modId) ||
                string.IsNullOrWhiteSpace(modName) ||
                string.IsNullOrWhiteSpace(version))
            {
                return false;
            }

            string id =
                modId.Trim();

            if (mods.ContainsKey(id))
            {
                return false;
            }

            mods.Add(
                id,
                new ModDefinition(
                    id,
                    modName.Trim(),
                    version.Trim()));

            return true;
        }

        public bool LoadMod(
            string modId)
        {
            ModDefinition mod =
                GetMod(modId);

            return mod != null &&
                   mod.Load();
        }

        public bool UnloadMod(
            string modId)
        {
            ModDefinition mod =
                GetMod(modId);

            return mod != null &&
                   mod.Unload();
        }

        public ModDefinition GetMod(
            string modId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(modId))
            {
                return null;
            }

            mods.TryGetValue(
                modId.Trim(),
                out ModDefinition mod);

            return mod;
        }

        public IReadOnlyCollection<
            ModDefinition>
            GetMods()
        {
            return mods.Values;
        }

        public void Reset()
        {
            mods.Clear();
            Initialized = false;
        }
    }
}
